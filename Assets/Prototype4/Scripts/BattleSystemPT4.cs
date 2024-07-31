using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystemPT4 : MonoBehaviour
{
    [Header("Battle System")]
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform playerBattleStation2;
    public Transform enemyBattleStation;

    UnitPT4 playerUnit;
    UnitPT4 playerUnit2;
    UnitPT4 enemyUnit;

    public List<UnitPT4> unitsInBattle;
    int unitTurn = 0;

    public TMP_Text dialogueText;

    public BattleHudPT4 playerHUD;
    public BattleHudPT4 playerHUD2;
    public BattleHudPT4 enemyHUD;

    public BattleState state;
    bool actionSelected;
    bool attackLanded;

    [Header("Equations")]
    public EquationGenerator equationGenerator;
    public bool isReversed;

    public TMP_InputField num1Box;
    public TMP_InputField num2Box;
    public TMP_InputField num3Box;
    public GameObject equationPanel;
    public BoostBarPT1 timerBar;
    public float equationTimer = 5f;
    float timerMax;
    bool isTiming;
    string input;
    string validCharacters = "0123456789";

    void Start()
    {
        equationPanel.SetActive(false);
        state = BattleState.START;
        timerMax = equationTimer;
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        LockInputToNumbers(validCharacters, num3Box);
        LockInputToNumbers(validCharacters, num2Box);

        timerBar.UpdateBoostBar(equationTimer, timerMax);

        if (isTiming)
            equationTimer -= Time.deltaTime;

        if (equationTimer <= 0)
        {
            if (state == BattleState.PLAYERTURN)
            {
                attackLanded = false;
                StartCoroutine(PlayerAttack());
            }
            else if (state == BattleState.ENEMYTURN)
            {
                attackLanded = true;
                StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<UnitPT4>();
        playerUnit.HUD = playerHUD;
        unitsInBattle.Add(playerUnit);

        //GameObject playerGO2 = Instantiate(playerPrefab2, playerBattleStation2);
        //playerUnit2 = playerGO2.GetComponent<UnitPT4>();
        //playerUnit2.HUD = playerHUD2;
        //unitsInBattle.Add(playerUnit2);

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<UnitPT4>();
        enemyUnit.HUD = enemyHUD;
        unitsInBattle.Add(enemyUnit);

        //dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerUnit.HUD.SetHUD(playerUnit);
        enemyUnit.HUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(0f);

        CycleTurns();
        //state = BattleState.PLAYERTURN;
        //PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        isTiming = false;
        equationPanel.SetActive(false);
        equationTimer = timerMax;

        bool isDead = false;

        if (attackLanded)
        {
            //Damage the enemy
            isDead = enemyUnit.TakeDamage(playerUnit.damage);

            enemyUnit.HUD.SetHP(enemyUnit.currentHP);

            dialogueText.text = "The attack is successful!";
        }
        else
            dialogueText.text = "The attack is unsuccessful.";

        yield return new WaitForSeconds(2f);

        //Check if the enemy is dead
        //Change state based on what happened
        if (isDead)
        {
            //end the battle
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //enemy turn
            CycleTurns();
            //state = BattleState.ENEMYTURN;
            //CreateReverseEquation();
            //StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        //dialogueText.text = enemyUnit.unitName + " attacks!";

        //yield return new WaitForSeconds(1f);

        isTiming = false;
        equationPanel.SetActive(false);
        equationTimer = timerMax;

        bool isDead = false;

        if (attackLanded)
        {
            isDead = playerUnit.TakeDamage(enemyUnit.damage);

            playerUnit.HUD.SetHP(playerUnit.currentHP);

            dialogueText.text = "The attack is successful!";
        }
        else
            dialogueText.text = "The attack is unsuccessful.";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            //end the battle
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            //player turn
            CycleTurns();
            //state = BattleState.PLAYERTURN;
            //PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    void PlayerTurn()
    {
        actionSelected = false;
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (actionSelected)
            return;

        actionSelected = true;

        CreateEquation();
        //StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    public void CycleTurns()
    {
        if (unitsInBattle[unitTurn].party == PartyType.Player)
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }    
        else if (unitsInBattle[unitTurn].party == PartyType.Enemy)
        {
            state = BattleState.ENEMYTURN;
            CreateReverseEquation();
        }

        unitTurn++;
        if (unitTurn >= unitsInBattle.Count)
            unitTurn = 0;
    }

    #region EquationManagement
    void CreateEquation()
    {
        isReversed = false;

        equationTimer = timerMax;
        isTiming = true;

        equationGenerator.GenerateAddition();


        num1Box.text = equationGenerator.numberOne.ToString();
        num1Box.interactable = false;

        num2Box.text = equationGenerator.numberTwo.ToString();
        num2Box.interactable = false;

        num3Box.text = null;
        num3Box.interactable = true;

        equationPanel.SetActive(true);
        num3Box.Select();
    }

    void CreateReverseEquation()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        isReversed = true;

        equationTimer = timerMax;
        isTiming = true;

        equationGenerator.GenerateAddition();


        num1Box.text = equationGenerator.numberOne.ToString();
        num1Box.interactable = false;

        num2Box.text = null;
        num2Box.interactable = true;

        num3Box.text = equationGenerator.correctAnswer.ToString();
        num3Box.interactable = false;

        equationPanel.SetActive(true);
        num2Box.Select();
    }

    public void CalculateEquation(string _s)
    {
        input = _s;

        if (!isReversed)
        {
            if (input.Length > 0)
            {
                int answer = int.Parse(input);

                if (answer == equationGenerator.correctAnswer)
                    attackLanded = true;
                    //Debug.Log("Correct!");
                else
                    attackLanded = false;
                //Debug.Log("Incorrect");
                StartCoroutine(PlayerAttack());
            }
        }
        else
        {
            if (input.Length > 0)
            {
                int answer = int.Parse(input);

                if (answer == equationGenerator.numberTwo)
                    attackLanded = false;
                //Debug.Log("Correct!");
                else
                    attackLanded = true;
                //Debug.Log("Incorrect");
                StartCoroutine(EnemyTurn());
            }
        }
    }

    void LockInputToNumbers(string _validCharacters, TMP_InputField _inputField)
    {
        _inputField.onValidateInput = (string _text, int _charIndex, char _addedChar) =>
        {
            return ValidateChar(_validCharacters, _addedChar);
        };
    }

    private char ValidateChar(string _validCharacters, char _addedChar)
    {
        if (_validCharacters.IndexOf(_addedChar) != -1)
        {
            return _addedChar;
        }
        else
        {
            return '\0';
        }
    }

    public delegate char OnValidateInput(string _text, int _charIndex, char _addedChar);
    #endregion
}
