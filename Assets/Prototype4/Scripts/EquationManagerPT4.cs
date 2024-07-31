using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquationManagerPT4 : MonoBehaviour
{
    public EquationGenerator equationGenerator;
    public bool isReversed;

    public TMP_InputField num1Box;
    public TMP_InputField num2Box;
    public TMP_InputField num3Box;
    public TMP_InputField inputFieldReversed;
    int numRev1 = 2;
    int numRev3 = 5;
    string input;
    string validCharacters = "0123456789";

    // Start is called before the first frame update
    void Start()
    {
        equationGenerator.GenerateAddition();

        num1Box.text = equationGenerator.numberOne.ToString();
        num2Box.text = equationGenerator.numberTwo.ToString();

        if (!isReversed)
            num3Box.Select();
        else
            inputFieldReversed.Select();
    }

    // Update is called once per frame
    void Update()
    {
        LockInputToNumbers(validCharacters, num3Box);

        num1Box.text = equationGenerator.numberOne.ToString();
        num2Box.text = equationGenerator.numberTwo.ToString();
    }

    public void CalculateEquation(string _s)
    {
        input = _s;

        if (!isReversed)
        {
            if (input.Length > 0)
            {
                int num3 = int.Parse(input);

                if (num3 == equationGenerator.correctAnswer)
                    Debug.Log("Correct!");
                else
                    Debug.Log("Incorrect");
            }
        }
        else
        {
            if (input.Length > 0)
            {
                int numRev2 = int.Parse(input);

                if (numRev3 == (numRev1 + numRev2))
                    Debug.Log("Correct!");
                else
                    Debug.Log("Incorrect");
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
}
