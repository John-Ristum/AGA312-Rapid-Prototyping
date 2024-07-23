using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Playground : GameBehaviour
{
    public enum Direction { North, East, South, West };
    public GameObject player;
    public float moveDistance = 2f;
    public float moveTweenTime = 1f;
    public Ease moveEase;
    public float shakeStrength = 0.4f;
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public Ease scoreEase;
    private int score = 0;
    public int scoreBonus = 100;

    void Start()
    {
        player.transform.position = _SAVE.GetLastCheckpoint();
        player.GetComponent<Renderer>().material.color = _SAVE.GetColour();
        highScoreText.text = "Highest Score: " + _SAVE.GetHighestScore().ToString();

        ExecuteAfterSeconds(2f, () =>
        {
            player.transform.localScale = Vector3.one * 2;
        });
        print("Game Started");

        ExecuteAfterFrames(1, () =>
        {
            print("one frame later");
        });
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.GetComponent<Renderer>().material.color = ColorX.GetRandomColour();
        //}

        if (Input.GetKeyDown(KeyCode.W))
            MovePlayer(Direction.North);
        if (Input.GetKeyDown(KeyCode.D))
            MovePlayer(Direction.East);
        if (Input.GetKeyDown(KeyCode.S))
            MovePlayer(Direction.South);
        if (Input.GetKeyDown(KeyCode.A))
            MovePlayer(Direction.West);
    }

    void MovePlayer(Direction _direction)
    {
        switch (_direction)
        {
            case Direction.North:
                player.transform.DOMoveZ(player.transform.position.z + moveDistance, moveTweenTime).SetEase(moveEase).OnComplete(() =>
                {
                    ShakeCamera();
                    TweenX.TweenNumbers(scoreText, score, score + scoreBonus, 1f, scoreEase);
                });
                break;
            case Direction.East:
                player.transform.DOMoveX(player.transform.position.x + moveDistance, moveTweenTime).SetEase(moveEase).OnComplete(() =>
                {
                    ShakeCamera();
                    TweenX.TweenNumbers(scoreText, score, score + scoreBonus, 1f, scoreEase);
                });
                break;
            case Direction.South:
                player.transform.DOMoveZ(player.transform.position.z - moveDistance, moveTweenTime).SetEase(moveEase).OnComplete(() =>
                {
                    ShakeCamera();
                    TweenX.TweenNumbers(scoreText, score, score + scoreBonus, 1f, scoreEase);
                });
                break;
            case Direction.West:
                player.transform.DOMoveX(player.transform.position.x - moveDistance, moveTweenTime).SetEase(moveEase).OnComplete(() =>
                {
                    ShakeCamera();
                    TweenX.TweenNumbers(scoreText, score, score + scoreBonus, 1f, scoreEase);
                });
                break;
        }
        _SAVE.SetLastPosition(player.transform.position);
        IncreaseScore();
        ChangeColour();
    }

    void ShakeCamera()
    {
        Camera.main.DOShakePosition(moveTweenTime / 2, shakeStrength);
    }

    void ChangeColour()
    {
        Color c = ColorX.GetRandomColour();
        _SAVE.SetColour(c);
        player.GetComponent<Renderer>().material.DOColor(c, moveTweenTime);
    }

    void IncreaseScore()
    {
        score = score + scoreBonus;
        _SAVE.SetScore(score);
    }
}
