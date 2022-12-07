using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Puzzle4UI : MonoBehaviour
{
    [SerializeField] private GameObject puzzle4Menu, puzzle4FirstButton;
    [SerializeField] private string codeString, codeSolution;
    [SerializeField] private int buttonsInputed;
    [SerializeField] private TextMeshProUGUI codeInput, codeResult;
    [SerializeField] public bool codeSolved;
    void Start()
    {
        codeSolved = false;
        codeResult.text = "";
        Time.timeScale = 0;
        codeSolution = "245316";
    }

    void Update()
    {
        codeInput.text = codeString.ToString();
        if (buttonsInputed >= 7)
        {
            codeString = "";
            buttonsInputed = 0;
        }
        if (string.Compare(codeString, codeSolution) == 0)
        {
            codeResult.text = "Code Solved";
            codeSolved = true;
        }
    }

    public void OpenPuzzleUI()
    {
        Time.timeScale = 0;
        buttonsInputed = 0;
        codeResult.text = "";
        codeString = "";
        codeInput.text = "";
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(puzzle4FirstButton);
    }

    public void ClosePuzzleUI()
    {
        puzzle4Menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void AddOneToCode()
    {
        codeString += "1";
        buttonsInputed++;
    }

    public void AddTwoToCode()
    {
        codeString += "2";
        buttonsInputed++;
    }

    public void AddThreeToCode()
    {
        codeString += "3";
        buttonsInputed++;
    }

    public void AddFourToCode()
    {
        codeString += "4";
        buttonsInputed++;
    }

    public void AddFiveToCode()
    {
        codeString += "5";
        buttonsInputed++;
    }

    public void AddSixToCode()
    {
        codeString += "6";
        buttonsInputed++;
    }
}
