using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeManager : MonoBehaviour
{
    public static TicTacToeManager Instance { get; private set; }

    [SerializeField] private Sprite xSprite;
    [SerializeField] private Sprite oSprite;

    private int totalXCount = 0;
    private int totalOCount = 0;
    private bool gameOver = false;
    private bool isXTurn = true;
    [SerializeField] private int turnTimer;
    private Coroutine turnTimerCoroutine;
    private bool isTimerPaused = false;
    public int timer;

    private Sprite[] boardState = new Sprite[9];

    private void Awake()
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

    void Start()
    {
        // StartTurnTimer();
        // CheckTimerCondition();
        AudioManager.Instance.PlayMainThemeBacksound();
        timer = turnTimer;
    }

    public void SetButtonSprite(int index)
    {
        if (gameOver || boardState[index] != null) return;
        TicTacToeUI.Instance.ShowTicTacToe();
        StartTurnTimer();
        StartCoroutine(DelayedSetSprite(index));
    }


    public void SetButtonSpriteIndex(int index)
    {
        if (gameOver || boardState[index] != null) return;
        GameplayManager.Instance.SaveSelectedIndex(index);
        TicTacToeUI.Instance.HideTicTacToe();
        StopTurnTimer();
    }

    private IEnumerator DelayedSetSprite(int index)
    {
        Sprite currentSprite = isXTurn ? xSprite : oSprite;
        boardState[index] = currentSprite;

        if (isXTurn)
            totalXCount++;
        else
            totalOCount++;

        TicTacToeUI.Instance.UpdateBoard(index, currentSprite);

        if (CheckWin())
        {
            gameOver = true;
            TicTacToeUI.Instance.GameFinish(isXTurn ? "X" : "O");
            yield break;
        }
        if (IsBoardFull())
        {
            gameOver = true;
            DetermineWinnerByCount();
            yield break;
        }

        isXTurn = !isXTurn;
        TicTacToeUI.Instance.UpdateTurn(isXTurn);
        StartTurnTimer();
    }

    private bool IsBoardFull()
    {
        foreach (var cell in boardState)
        {
            if (cell == null) return false;
        }
        return true;
    }

    private bool CheckWin()
    {
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diagonals
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0], b = winPatterns[i, 1], c = winPatterns[i, 2];
            if (boardState[a] != null && boardState[a] == boardState[b] && boardState[a] == boardState[c])
            {
                return true;
            }
        }
        return false;
    }

    public void StartTurnTimer()
    {
        StopTurnTimer();
        turnTimerCoroutine = StartCoroutine(TurnTimerRoutine());
        TicTacToeUI.Instance.UpdateTimer(turnTimer);
    }

    public void PauseTimer()
    {
        isTimerPaused = true;
    }

    public void ResumeTimer()
    {
        isTimerPaused = false;
    }

    private void StopTurnTimer()
    {
        if (turnTimerCoroutine != null)
        {
            StopCoroutine(turnTimerCoroutine);
        }
    }

    private IEnumerator TurnTimerRoutine()
    {
        timer = turnTimer;
        while (timer >= 0)
        {
            while (isTimerPaused)
            {
                yield return null;
            }

            TicTacToeUI.Instance.UpdateTimer(timer);
            if (timer <= 10)
            {
                AudioManager.Instance.PlayTimerSFXPanic();
                // AudioManager.Instance.StopBacksound();
            }
            yield return new WaitForSeconds(1);
            timer--;
        }

        SwitchTurn();
        TicTacToeUI.Instance.UpdateTurn(isXTurn);
    }


    public void SwitchTurn()
    {
        TicTacToeUI.Instance.ShowTicTacToe();
        isXTurn = !isXTurn;
        TicTacToeUI.Instance.UpdateTurn(isXTurn);
        StartTurnTimer();
        GameplayManager.Instance.CheckTimerCondition();
        Debug.Log("Changes : " + isXTurn);

    }

    private void DetermineWinnerByCount()
    {
        if (totalXCount > totalOCount)
            TicTacToeUI.Instance.GameFinish("X");
        else if (totalOCount > totalXCount)
            TicTacToeUI.Instance.GameFinish("O");
    }

    public void ResetBoard()
    {
        for (int i = 0; i < boardState.Length; i++)
        {
            boardState[i] = null;
        }
        isXTurn = true;

        TicTacToeUI.Instance.ResetBoard();
    }

    public void EnableTicTacToe(bool turn)
    {
        isXTurn = turn;
        TicTacToeUI.Instance.ResumeGame();
    }
}