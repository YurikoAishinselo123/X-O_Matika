using System.Net.NetworkInformation;
using NUnit.Framework;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    private int currentTicTacToeIndex;
    private bool isXTurn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    }

    public void StartNewTurn()
    {
        TicTacToeManager.Instance.StartTurnTimer();
    }

    public void SaveSelectedIndex(int index)
    {
        currentTicTacToeIndex = index;
        QuizManager.Instance.StartQuizFlow();
    }

    public void HandleQuizResult(bool isCorrect)
    {
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


}
