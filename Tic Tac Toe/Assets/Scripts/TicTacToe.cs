using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    public UnityEvent<Button> ButtonChoice;
    bool isX = true;
    bool winnable;

    [SerializeField] GameObject[] panels;
    [SerializeField] GameObject EndScreen;
    TMP_Text[] sideText;
    TMP_Text[] buttonText;
    [SerializeField] Button[] buttons;
    string[,] letters = new string[3, 3];
    [SerializeField] Button makingMoves;
    private void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonChoice.Invoke(button));
        }
        ButtonChoice.AddListener(XorO);
        SetSpaces();
        makingMoves.onClick.AddListener(MakeMove);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    void XorO(Button button)
    {
        ChooseSpace(button);
        button.interactable = false;
        CheckWin();
    }

    void ChooseSpace(Button button)
    {
        var text = button.GetComponentInChildren<TMP_Text>();
        switch (isX)
        {
            case true:
                text.alpha = 255f;
                text.text = "X";
                isX = false;
                break;
            case false:
                text.alpha = 255f;
                text.text = "O";
                isX = true;
                break;  
        }
        button.onClick = null;
    }


    void CheckWin()
    {
        int counter = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                letters[i, j] = buttons[counter++].GetComponentInChildren<TMP_Text>().text;
            }
        }
        bool win = false;
        bool allChecked = false;
        bool verticalWin = false, horizontalWin = false, diagonalWin = false;
        while (win == false && allChecked == false)
        {
            verticalWin = letters[0, 0] == letters[0, 1] && letters[0, 0] == letters[0, 2] || letters[1, 0] == letters[1, 1] && letters[1, 0] == letters[1, 2] || letters[2, 0] == letters[2, 1] && letters[2, 0] == letters[2, 2];
            horizontalWin = letters[0, 0] == letters[1, 0] && letters[0, 0] == letters[2, 0] || letters[0, 1] == letters[1, 1] && letters[0, 1] == letters[2, 1] || letters[0, 2] == letters[1, 2] && letters[0, 2] == letters[2, 2];
            diagonalWin = letters[0, 0] == letters[1, 1] && letters[0, 0] == letters[2, 2] || letters[2, 0] == letters[1, 1] && letters[2, 0] == letters[0, 2];

            if (horizontalWin || verticalWin || diagonalWin)
            {
                win = true;
                EndScreen.SetActive(true);
                EndScreen.GetComponent<GameOver>().GameFinished.Invoke(0);
              
            }
            else
            {
                allChecked = true;
                IsWinnable();
            }
        }
    }

    void IsWinnable()
    {
        foreach (Button button in buttons)
        {
            if (button.interactable == true)
            {
                winnable = true;
                goto Winnable;
            }
            else
            {
                winnable = false;
            }

        }
        Winnable:
            if (!winnable)
            {
                EndScreen.SetActive(true);
                EndScreen.GetComponent<GameOver>().GameFinished.Invoke(1);
            }   
    }

    void FillBox(int num)
    {
        try
        {
            if (num < 7 && num is 0 or 1 or 3 or 5 or 6)
            {
                if (buttons[num].onClick == null)
                {
                    buttons[num + 2].onClick.Invoke();
                }
                else if (buttons[num].onClick != null)
                {
                    buttons[num].onClick.Invoke();
                }
            }
            else if (num < 7 && num is 2)
            {
                if (buttons[num].onClick == null)
                {
                    buttons[num + 4].onClick.Invoke();
                }
                else
                {
                    buttons[num].onClick.Invoke();
                }
            }
            else if (num >= 7)
            {
                if (buttons[num].onClick != null)
                {
                    buttons[num].onClick.Invoke();
                }
                else if (buttons[num].onClick == null && buttons[num - 2] != null)
                {
                    buttons[num - 2].onClick.Invoke();
                }
                else if (num == 7 && buttons[1].onClick != null)
                {
                    buttons[1].onClick.Invoke();
                }
                else if (num == 8 && buttons[0].onClick != null)
                {
                    buttons[0].onClick.Invoke();
                }
            }
            else
            {
                buttons[4].onClick.Invoke();
            }
        }
        catch (NullReferenceException)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].onClick != null)
                {
                    FillBox(i);
                }
            }
        }
    }

    void FillBox(int num, bool diagonal)
    {
        if (num is 0 or 2 && diagonal == false)
        {
            if (buttons[num].onClick == null)
            {
                buttons[num + 6].onClick.Invoke();
            }
            else
            {
                buttons[num].onClick.Invoke();
            }
        }
        if (num is 0 && diagonal == true)
        {
            if (buttons[num].onClick == null)
            {
                buttons[num + 8].onClick.Invoke();
            }
            else
            {
                buttons[num].onClick.Invoke();
            }
        }
        if (num is 2 && diagonal == true)
        {
            if (buttons[num].onClick == null)
            {
                buttons[num + 4].onClick.Invoke();
            }
            else
            {
                buttons[num].onClick.Invoke();
            }
        }
    }

    void SetSpaces()
    {
        buttonText = new TMP_Text[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonText[i] = buttons[i].GetComponentInChildren<TMP_Text>();
        }
    }

    bool CheckXBoard(List<int> xChoices, List<int> oChoices)
    {
        //Left To Right
        if (xChoices.Contains(0) && xChoices.Contains(2) && !oChoices.Contains(1))
        {
            FillBox(1);
            return true;
        }
        else if (xChoices.Contains(0) && xChoices.Contains(1) && !oChoices.Contains(2))
        {
            FillBox(2);
            return true;
        }
        else if (xChoices.Contains(2) && xChoices.Contains(1) && !oChoices.Contains(0))
        {
            FillBox(1);
            return true;
        }
        else if (xChoices.Contains(3) && xChoices.Contains(4) && !oChoices.Contains(5))
        {
            FillBox(5);
            return true;
        }
        else if (xChoices.Contains(4) && xChoices.Contains(5) && !oChoices.Contains(3))
        {
            FillBox(3);
            return true;
        }
        else if (xChoices.Contains(3) && xChoices.Contains(5) && !oChoices.Contains(4))
        {
            FillBox(4);
            return true;
        }
        else if (xChoices.Contains(6) && xChoices.Contains(7) && !oChoices.Contains(8))
        {
            FillBox(8);
            return true;
        }
        else if (xChoices.Contains(6) && xChoices.Contains(8) && !oChoices.Contains(7))
        {
            FillBox(7);
            return true;
        }
        else if (xChoices.Contains(7) && xChoices.Contains(8) && !oChoices.Contains(6))
        {
            FillBox(6);
            return true;
        }
        //Diagonals
        else if (xChoices.Contains(0) && xChoices.Contains(4) && !oChoices.Contains(8))
        {
            FillBox(8);
            return true;
        }
        else if (xChoices.Contains(0) && xChoices.Contains(8) && !oChoices.Contains(4))
        {
            FillBox(4);
            return true;
        }
        else if (xChoices.Contains(4) && xChoices.Contains(8) && !oChoices.Contains(0))
        {
            FillBox(0);
            return true;
        }
        else if (xChoices.Contains(6) && xChoices.Contains(2) && !oChoices.Contains(4))
        {
            FillBox(4);
            return true;
        }
        else if (xChoices.Contains(2) && xChoices.Contains(4) && !oChoices.Contains(6))
        {
            FillBox(6);
            return true;
        }
        else if (xChoices.Contains(4) && xChoices.Contains(6) && !oChoices.Contains(2))
        {
            FillBox(2);
            return true;
        }
        //Verticals
        else if (xChoices.Contains(0) && xChoices.Contains(3) && !oChoices.Contains(6))
        {
            FillBox(6);
            return true;
        }
        else if (xChoices.Contains(0) && xChoices.Contains(6) && !oChoices.Contains(3))
        {
            FillBox(3);
            return true;
        }
        else if (xChoices.Contains(6) && xChoices.Contains(3) && !oChoices.Contains(0))
        {
            FillBox(0);
            return true;
        }
        else if (xChoices.Contains(1) && xChoices.Contains(4) && !oChoices.Contains(7))
        {
            FillBox(7);
            return true;
        }
        else if (xChoices.Contains(4) && xChoices.Contains(7) && !oChoices.Contains(1))
        {
            FillBox(1);
            return true;
        }
        else if (xChoices.Contains(1) && xChoices.Contains(7) && !oChoices.Contains(4))
        {
            FillBox(4);
            return true;
        }
        else if (xChoices.Contains(2) && xChoices.Contains(5) && !oChoices.Contains(8))
        {
            FillBox(8);
            return true;
        }
        else if (xChoices.Contains(5) && xChoices.Contains(8) && !oChoices.Contains(2))
        {
            FillBox(2);
            return true;
        }
        else if (xChoices.Contains(2) && xChoices.Contains(8) && !oChoices.Contains(5))
        {
            FillBox(5);
            return true;
        }
        else { return false; }
    }

    void MakeMove()
    {
        List<int> xChoices = new List<int>();
        List<int> oChoices = new List<int>();

        for (int i = 0; i < buttonText.Length;i++)
        {
            if (buttonText[i].text == "X" && !xChoices.Contains(i))
            {
                xChoices.Add(i);
            }
            else if (buttonText[i].text == "O" && !oChoices.Contains(i))
            {
                oChoices.Add(i);
            }
        }

        switch (xChoices.Count + oChoices.Count)
        {
            case 0:
                FillBox(0);
                break;
            case 1:
                if (xChoices[0] is 0 or 2 or 6 or 8)
                {
                    FillBox(4);
                }
                else if (xChoices[0] is 1 or 3)
                {
                    FillBox(0);
                }
                else if (xChoices[0] is 5 or 7)
                {
                    FillBox(8);
                }
                else
                {
                    FillBox(8);
                }
                break;
            case 2:
                int xChoice = xChoices[0];
                int oChoice = oChoices[0];
                if (xChoice is 0 or 2 && oChoice is 7)
                {
                    FillBox(0);
                }
                if (xChoice is 1 && oChoice is 0)
                {
                    FillBox(3);
                }
                if (xChoice is 1 && oChoice is 2)
                {
                    FillBox(5);
                }
                else if (xChoice is 0 or 6 && oChoice is 2)
                {
                    FillBox(0, false);
                }
                else if (xChoice is 0 or 2 && oChoice is 6 or 8)
                {
                    FillBox(0);
                }
                else if (xChoice is 2 or 8 && oChoice is 6)
                {
                    FillBox(2, false);
                }
                else if (xChoice is 6 or 8 && oChoice is 0)
                {
                    FillBox(8);
                }
                else if (xChoice is 6 or 8 && oChoice is 1)
                {
                    FillBox(6);
                }
                else if (xChoice is 0 or 6 && oChoice is 5)
                {
                    FillBox(0, false);
                }
                else if (xChoice is 2 or 8 && oChoice is 3)
                {
                    FillBox(2, false);
                }
                else if (xChoice is 3 && oChoice is 4 or 6 )
                {
                    FillBox(2);
                }
                else if (xChoice is 3 && oChoice is 0 or 1)
                {
                    FillBox(8);
                }
                else if (xChoice is 5 && oChoice is 2 or 8)
                {
                    FillBox(0);
                }
                else if (xChoice is 4 && oChoice is 3)
                {
                    FillBox(6);
                }
                else if (xChoice is 4 && oChoice is 1 or 3)
                {
                    FillBox(0);
                }
                else if (xChoice is 4 && oChoice is 2 or 8)
                {
                    FillBox(5);
                }
                else if (xChoice is 4 && oChoice is 0 or 6)
                {
                    FillBox(3);
                }
                else if (xChoice is 1 && oChoice is 4 or 5)
                {
                    FillBox(6);
                }
                else if (xChoice is 1 && oChoice is 3)
                {
                    FillBox(8);
                }
                else if (xChoice is 4 && oChoice is 5 or 7)
                {
                    FillBox(8);
                }
                else if (xChoice is 5 && oChoice is 4 or 7)
                {
                    FillBox(0);
                }
                else if (xChoice is 5 && oChoice is 1)
                {
                    FillBox(6);
                }
                else if (xChoice is 7 && oChoice is 3 or 4 or 6)
                {
                    FillBox(2);
                }
                else if (xChoice is 7 && oChoice is 4 or 5 or 8)
                {
                    FillBox(0);
                }
                else if (xChoice is 0 or 2 && oChoice is 4)
                {
                    FillBox(2, false);
                }
                else if (xChoice is 6 or 8 && oChoice is 4)
                {
                    FillBox(6);
                }
                else if (xChoice is 1 && oChoice is 6 or 7 or 8)
                {
                    FillBox(0);
                }
                else if (xChoice is 3 && oChoice is 2 or 5 or 8)
                {
                    FillBox(0);
                }
                else if (xChoice is 5 && oChoice is 0 or 3 or 6)
                {
                    FillBox(2);
                }
                else if (xChoice is 7 && oChoice is 0 or 1 or 2)
                {
                    FillBox(6);
                }
                break;
            default:
                if (CheckXBoard(xChoices, oChoices) || CheckXBoard(oChoices, xChoices))
                {
                    break;
                }
                else
                {
                    List<int> buttonList = new List<int>();
                    System.Random rand = new System.Random();
                    int counter = 0;
                    int randomNum;
                    foreach (Button button in buttons)
                    {
                        if (button.interactable == true)
                        {
                            buttonList.Add(counter);
                        }
                        counter++;
                    }
                    randomNum = rand.Next(buttonList.Count);
                    FillBox(buttonList[randomNum]);
                    break;
                }
        }

    }
   
}
