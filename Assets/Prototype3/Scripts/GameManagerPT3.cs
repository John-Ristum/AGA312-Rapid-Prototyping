using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerPT3 : GameBehaviour
{
    public delegate void StartGame();
    public static event StartGame startGame;

    public enum GameTypeState { Single, Multi }
    public GameTypeState gameType;
    public enum PowerUpState { Normal, Rapidfire, Shotgun, Ghost }
    public PowerUpState powerUp;
    float powerUpTimer = 10f;
    public PowerUpManagerPT3 powerUpManager;
    public int lives = 3;
    public int score;
    int lastScore;

    public GameObject player2;

    [Header("UI")]
    public GameObject boostBar;
    BoostBarPT1 boostBarScript;
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public GameObject startPanel;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public PauseController pauseController;

    // Start is called before the first frame update
    void Start()
    {
        inGamePanel.SetActive(false);
        startPanel.SetActive(true);

        boostBarScript = boostBar.GetComponent<BoostBarPT1>();

        score = 0;
        lastScore = 0;
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;
    }

    public void SetGame1()
    {
        gameType = GameTypeState.Single;

        SetUpGame();
    }

    public void SetGame2()
    {
        gameType = GameTypeState.Multi;

        SetUpGame();
    }

    public void SetUpGame()
    {
        if (gameType == GameTypeState.Single)
            player2.SetActive(false);
        else
            player2.SetActive(true);

        startPanel.SetActive(false);
        inGamePanel.SetActive(true);

        startGame();
    }

    // Update is called once per frame
    void Update()
    {
        TimePowerUp();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    startGame();
        //    SetUpGame();
        //}
    }

    void TimePowerUp()
    {
        if (powerUp == PowerUpState.Normal)
        {
            boostBar.SetActive(false);
            powerUpTimer = 10f;
        }
        else
        {
            boostBar.SetActive(true);

            powerUpTimer -= Time.deltaTime;

            if (powerUpTimer <= 0)
                powerUp = PowerUpState.Normal;
        }

        boostBarScript.UpdateBoostBar(powerUpTimer, 10f);
    }

    public void IncreaseScore(int _points)
    {
        score += _points;
        scoreText.text = "Score: " + score;

        if (score == lastScore + 5)
        {
            powerUpManager.SpawnPowerUp();
            lastScore = score;
        }
        //Debug.Log("Score: " + score);
    }

    public void EndGame()
    {
        finalScoreText.text = "Score: " + score;
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        pauseController.gameEnded = true;
        Time.timeScale = 0;
    }
}
