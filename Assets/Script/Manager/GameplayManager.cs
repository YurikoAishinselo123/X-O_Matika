using System.Net.NetworkInformation;
using NUnit.Framework;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    private int currentTicTacToeIndex;
    private bool isXTurn = true;
    private bool backsoundIsPlaying = true;
    private string difficulty;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartNewTurn();
        AudioManager.Instance.StopBacksound();
        CheckTimerCondition();
    }

    public void StartNewTurn()
    {
        TicTacToeManager.Instance.StartTurnTimer();
    }

    public void SaveSelectedIndex(int index)
    {
        AudioManager.Instance.StopBacksound();
        currentTicTacToeIndex = index;
        QuizManager.Instance.StartQuizFlow();
    }

    public void HandleQuizResult(bool isCorrect)
    {
        CheckTimerCondition();
        if (isCorrect)
        {
            Debug.Log("Correct Answer! Proceed to place sprite.");
            TicTacToeManager.Instance.SetButtonSprite(currentTicTacToeIndex);
        }
        else
        {
            Debug.Log("Wrong Answer! Change turn.");
            TicTacToeManager.Instance.SwitchTurn();
        }
    }

    public void CheckTimerCondition()
    {
        difficulty = difficulty = QuizManager.Instance.GetSelectedDifficulty();
        switch (difficulty)
        {
            case "Easy":
                AudioManager.Instance.PlayTimerBacksoundEasy();
                break;
            case "Medium":
                AudioManager.Instance.PlayTimerBacksoundMedium();
                break;
            case "Hard":
                AudioManager.Instance.PlayTimerBacksoundHard();
                break;
            default:
                AudioManager.Instance.PlayTimerBacksoundEasy();
                break;
        }
    }

}


