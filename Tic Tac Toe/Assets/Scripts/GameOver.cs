using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour
{
    GameObject panel;
    Button restartButton;
    public UnityEvent<int> GameFinished;
    TMP_Text winnerText;
    private void OnEnable()
    {
        panel = GetComponent<GameObject>();
        winnerText = GetComponentInChildren<TMP_Text>();
        restartButton = GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(ResetScene);
        GameFinished.AddListener(WinnerText);
    }

   void ResetScene()
    {
        SceneManager.LoadScene(0);
    }

    public void WinnerText(int num)
    {
        switch (num)
        {
            case 0:
                winnerText.text = "We have a Winner!";
                break;
            case 1:
                winnerText.text = "No Winner";
                break;
        }
    }
}