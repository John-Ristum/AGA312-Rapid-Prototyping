using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystemPT4 : MonoBehaviour
{
    [Header("Battle System")]
    public GameObject buttons;
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

    public BattleState battleState;
    bool actionSelected;
    bool attackLanded;
    int unitDamage;

    [Header("Equations")]
    public EquationGenerator equationGenerator;
    public enum EquationType { Addition, Subtraction, Multiplication}
    public EquationType equationType;
    public enum EquationState { Standard, Reversed, Heal}
    public EquationState equationState;

    public TMP_InputField num1Box;
    public TMP_InputField num2Box;
    public TMP_InputField num3Box;
    public TMP_Text operatorText;
    public GameObject equationPanel;
    public BoostBarPT1 timerBar;
    public float equationTimer = 5f;
    public float blockTimer = 10f;
    float timer;
    float timerMax;
    bool isTiming;
    string input;
    string validCharacters = "0123456789-";

    void Start()
    {
        equationPanel.SetActive(false);
        battleState = BattleState.START;
        SetTimer(equationTimer);
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        LockInputToNumbers(validCharacters, num3Box);
        LockInputToNumbers(validCharacters, num2Box);

        timerBar.UpdateBoostBar(timer, timerMax);

        if (isTiming)
            timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (battleState == BattleState.PLAYERTURN)
            {
                attackLanded = false;
                StartCoroutine(PlayerAttack());
            }
            else if (battleState == BattleState.ENEMYTURN)
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
        timer = timerMax;

        bool isDead = false;

        if (attackLanded)
        {
            playerUnit.transform.position = enemyUnit.oppStandPoint.position;
            playerUnit.anim.SetTrigger(playerUnit.attackAnim);
            enemyUnit.anim.SetBool("Idle", false);
            enemyUnit.anim.SetTrigger("Hurt");

            //Damage the enemy
            isDead = enemyUnit.TakeDamage(unitDamage);

            enemyUnit.HUD.SetHP(enemyUnit.currentHP);

            dialogueText.text = "Your attack was successful!";
        }
        else
            dialogueText.text = "Your attack was unsuccessful.";

        yield return new WaitForSeconds(2f);

        //Check if the enemy is dead
        //Change state based on what happened
        if (isDead)
        {
            //end the battle
            battleState = BattleState.WON;
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
        enemyUnit.transform.position = playerUnit.oppStandPoint.transform.position;
        enemyUnit.anim.SetTrigger("Attack");

        //dialogueText.text = enemyUnit.unitName + " attacks!";

        //yield return new WaitForSeconds(1f);

        isTiming = false;
        equationPanel.SetActive(false);
        timer = timerMax;

        bool isDead = false;

        if (attackLanded)
        {
            isDead = playerUnit.TakeDamage(enemyUnit.attackDamage);

            playerUnit.HUD.SetHP(playerUnit.currentHP);

            dialogueText.text = "The enemy's attack landed.";
            playerUnit.anim.SetBool("Idle", false);
            playerUnit.anim.SetTrigger("Hurt");
        }
        else
        {
            dialogueText.text = "You blocked the enemy's attack!";
            playerUnit.anim.SetBool("Idle", false);
            playerUnit.anim.SetTrigger("Guard");
        }

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            //end the battle
            battleState = BattleState.LOST;
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
        if (battleState == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (battleState == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    void PlayerTurn()
    {
        buttons.SetActive(true);
        actionSelected = false;
        dialogueText.text = "Choose an action:";
        playerUnit.anim.SetBool("Idle", true);
    }

    IEnumerator PlayerHeal()
    {
        isTiming = false;
        equationPanel.SetActive(false);
        timer = timerMax;

        if (attackLanded)
        {
            playerUnit.Heal(5);
            playerUnit.anim.SetTrigger("Heal");

            playerHUD.SetHP(playerUnit.currentHP);
            dialogueText.text = "You feel renewed strength!";
        }
        else
            dialogueText.text = "You were unsucessful at healing.";

        yield return new WaitForSeconds(2f);

        CycleTurns();
        //battleState = BattleState.ENEMYTURN;
        //StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        if (actionSelected)
            return;

        buttons.SetActive(false);
        actionSelected = true;
        playerUnit.isBlocking = false;
        playerUnit.attackAnim = "Attack";
        SetTimer(equationTimer);

        equationGenerator.difficulty = EquationGenerator.Difficulty.EASY;

        unitDamage = playerUnit.attackDamage;

        equationType = EquationType.Addition;

        equationState = EquationState.Standard;

        CreateEquation();
        //StartCoroutine(PlayerAttack());
    }

    public void OnSpecialButton()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        if (actionSelected)
            return;

        buttons.SetActive(false);
        actionSelected = true;
        playerUnit.isBlocking = false;
        playerUnit.attackAnim = "Special";
        SetTimer(equationTimer);

        //equationGenerator.difficulty = EquationGenerator.Difficulty.MEDIUM;
        equationGenerator.difficulty = EquationGenerator.Difficulty.EASY;

        unitDamage = playerUnit.specialDamage;

        equationType = EquationType.Addition;
        equationType = EquationType.Subtraction;

        equationState = EquationState.Standard;

        CreateEquation();
        //StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        if (battleState != BattleState.PLAYERTURN)
            return;

        if (actionSelected)
            return;

        buttons.SetActive(false);
        actionSelected = true;
        playerUnit.isBlocking = false;
        SetTimer(equationTimer);

        equationGenerator.difficulty = EquationGenerator.Difficulty.EASY;

        equationType = EquationType.Multiplication;

        equationState = EquationState.Heal;

        CreateEquation();
    }

    public void OnBlockButton()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        if (battleState != BattleState.PLAYERTURN)
            return;

        if (actionSelected)
            return;

        actionSelected = true;
        playerUnit.isBlocking = true;

        playerUnit.anim.SetBool("Idle", false);
        playerUnit.anim.SetTrigger("Guard");
        //SetTimer(blockTimer);

        dialogueText.text = "You are now guarding";

        Invoke("CycleTurns", 2f);
    }

    public void CycleTurns()
    {
        enemyUnit.anim.SetBool("Idle", true);
        playerUnit.transform.position = playerBattleStation.transform.position;
        enemyUnit.transform.position = enemyBattleStation.transform.position;

        if (unitsInBattle[unitTurn].party == PartyType.Player)
        {
            battleState = BattleState.PLAYERTURN;
            PlayerTurn();
        }    
        else if (unitsInBattle[unitTurn].party == PartyType.Enemy)
        {
            battleState = BattleState.ENEMYTURN;
            CreateReverseEquation();
        }

        unitTurn++;
        if (unitTurn >= unitsInBattle.Count)
            unitTurn = 0;
    }

    public void SetTimer(float _timerType)
    {
        timer = _timerType;
        timerMax = timer;
    }

    public void CheckForBlock(UnitPT4 _playerUnit)
    {
        if (_playerUnit.isBlocking)
            SetTimer(blockTimer);
        else
            SetTimer(equationTimer);
    }

    #region EquationManagement
    void CreateEquation()
    {
        equationTimer = timerMax;
        isTiming = true;

        switch (equationType)
        {
            case EquationType.Addition:
                operatorText.text = "+";
                equationGenerator.GenerateAddition();
                break;
            case EquationType.Subtraction:
                operatorText.text = "-";
                equationGenerator.GenerateSubtraction();
                break;
            case EquationType.Multiplication:
                operatorText.text = "x";
                equationGenerator.GenerateMultiplication();
                break;
        }


        num1Box.text = equationGenerator.numberOne.ToString();
        num1Box.interactable = false;

        num2Box.text = equationGenerator.numberTwo.ToString();
        num2Box.interactable = false;

        num3Box.text = null;
        num3Box.interactable = true;

        equationPanel.SetActive(true);
        num3Box.Select();

        Debug.Log(equationGenerator.correctAnswer);
    }

    void CreateReverseEquation()
    {
        equationType = EquationType.Addition;
        equationGenerator.difficulty = EquationGenerator.Difficulty.EASY;

        dialogueText.text = enemyUnit.unitName + " attacks!";

        equationState = EquationState.Reversed;

        //SetTimer(equationTimer);
        CheckForBlock(playerUnit);
        isTiming = true;

        switch (equationType)
        {
            case EquationType.Addition:
                operatorText.text = "+";
                equationGenerator.GenerateAddition();
                break;
            case EquationType.Subtraction:
                operatorText.text = "-";
                equationGenerator.GenerateSubtraction();
                break;
            case EquationType.Multiplication:
                operatorText.text = "x";
                equationGenerator.GenerateMultiplication();
                break;
        }


        num1Box.text = equationGenerator.numberOne.ToString();
        num1Box.interactable = false;

        num2Box.text = null;
        num2Box.interactable = true;

        num3Box.text = equationGenerator.correctAnswer.ToString();
        num3Box.interactable = false;

        equationPanel.SetActive(true);
        num2Box.Select();

        Debug.Log(equationGenerator.numberTwo);
    }

    public void CalculateEquation(string _s)
    {
        input = _s;

        switch (equationState)
        {
            case EquationState.Standard:
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
                break;
            case EquationState.Reversed:
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
                break;
            case EquationState.Heal:
                if (input.Length > 0)
                {
                    int answer = int.Parse(input);

                    if (answer == equationGenerator.correctAnswer)
                        attackLanded = true;
                    //Debug.Log("Correct!");
                    else
                        attackLanded = false;
                    //Debug.Log("Incorrect");
                    StartCoroutine(PlayerHeal());
                }
                break;
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
