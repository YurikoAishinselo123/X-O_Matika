using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeManager : MonoBehaviour
{
    public static TicTacToeManager Instance { get; private set; }

    [SerializeField] private Sprite xSprite;
    [SerializeField] private Sprite oSprite;

    private bool gameOver = false;
    private bool isXTurn = true;
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


    public void SetButtonSprite(int index)
    {
        if (gameOver || boardState[index] != null) return;

        StartCoroutine(DelayedSetSprite(index));
    }

    private IEnumerator DelayedSetSprite(int index)
    {
        yield return new WaitForSeconds(0f);

        Sprite currentSprite = isXTurn ? xSprite : oSprite;
        boardState[index] = currentSprite;

        TicTacToeUI.Instance.UpdateBoard(index, currentSprite);

        if (CheckWin())
        {
            TicTacToeUI.Instance.UpdateScore(isXTurn);
        }
        if (IsBoardFull())
        {
            TicTacToeUI.Instance.GameFinish();
            yield break;
        }

        isXTurn = !isXTurn;
        TicTacToeUI.Instance.UpdateTurn(isXTurn);
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

    public void ResetBoard()
    {
        for (int i = 0; i < boardState.Length; i++)
        {
            boardState[i] = null;
        }
        isXTurn = true;

        // Tell UI to reset buttons
        TicTacToeUI.Instance.ResetBoard();
    }
}
