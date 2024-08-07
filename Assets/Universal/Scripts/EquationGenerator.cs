using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationGenerator : MonoBehaviour
{
    public enum Difficulty { EASY, MEDIUM, HARD }
    public Difficulty difficulty;
    public int numberOne;
    public int numberTwo;
    public int correctAnswer;
    public List<int> dummyAnswers;
    private string operatorSign = "";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            GenerateMultiplication();
        if (Input.GetKeyDown(KeyCode.A))
            GenerateAddition();
        if (Input.GetKeyDown(KeyCode.D))
            GenerateDivision();

        if (Input.GetKeyDown(KeyCode.R))
            GenerateRandomEquation();
    }

    public void GenerateRandomEquation()
    {
        int rnd = Random.Range(1, 100);
        if (rnd <= 35)
            GenerateAddition();
        else if (rnd <= 60)
            GenerateSubtraction();
        else if (rnd <= 90)
            GenerateMultiplication();
        else
            GenerateDivision();
    }

    public void GenerateMultiplication()
    {
        operatorSign = " x ";
        numberOne = GetRandomNumber();
        numberTwo = GetRandomNumber();
        correctAnswer = numberOne * numberTwo;
        Debug.Log(numberOne + operatorSign + numberTwo + " = " + correctAnswer);
        GenerateDummyAnswers();
    }

    public void GenerateAddition()
    {
        operatorSign = " + ";
        numberOne = GetRandomNumber();
        numberTwo = GetRandomNumber();
        correctAnswer = numberOne + numberTwo;
        Debug.Log(numberOne + operatorSign + numberTwo + " = " + correctAnswer);
        GenerateDummyAnswers();
    }

    public void GenerateSubtraction()
    {
        operatorSign = " - ";
        numberOne = GetRandomNumber();
        numberTwo = GetRandomNumber();
        correctAnswer = numberOne - numberTwo;
        Debug.Log(numberOne + operatorSign + numberTwo + " = " + correctAnswer);
        GenerateDummyAnswers();
    }

    public void GenerateDivision()
    {
        operatorSign = " / ";
        numberOne = GetRandomNumber();
        numberTwo = GetRandomNumber();
        correctAnswer = Mathf.RoundToInt(numberOne / numberTwo);
        Debug.Log(numberOne + operatorSign + numberTwo + " = " + correctAnswer);
        GenerateDummyAnswers();
    }

    /// <summary>
    /// Gets a random number based on our difficulty
    /// </summary>
    private int GetRandomNumber()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return (Random.Range(1, 10));
            case Difficulty.MEDIUM:
                return (Random.Range(1, 20));
            case Difficulty.HARD:
                return (Random.Range(1, 100));
            default:
                return (Random.Range(1, 10));
        }
    }

    /// <summary>
    /// This will generate a set of dummy answers
    /// </summary>
    private void GenerateDummyAnswers()
    {
        for (int i = 0; i < dummyAnswers.Count; i++)
        {
            int dummy;
            do
            {
                dummy = Random.Range(correctAnswer - 10, correctAnswer + 10);
            }
            while (dummy == correctAnswer || dummyAnswers.Contains(dummy));
            dummyAnswers[i] = dummy;
            Debug.Log("Dummy answer: " + dummyAnswers[i]);
        }
    }
}