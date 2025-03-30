using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeManager : MonoBehaviour
{
    public static TicTacToeManager Instance { get; private set; }

    [SerializeField] private Sprite xSprite;
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Button[] buttons;

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

    public void SetButtonSprite(Image buttonImage, int index)
    {
        if (boardState[index] != null) return; // Prevent overwriting

        StartCoroutine(DelayedSetSprite(buttonImage, index));
    }

    private IEnumerator DelayedSetSprite(Image buttonImage, int index)
    {
        yield return new WaitForSeconds(0f); // 1-second delay before placing X or O

        Sprite currentSprite = isXTurn ? xSprite : oSprite;
        buttonImage.sprite = currentSprite;
        boardState[index] = currentSprite;

        if (CheckWin())
        {
            TicTacToeUI.Instance.UpdateScore(isXTurn);
            ResetBoard();
            yield break;
        }

        isXTurn = !isXTurn;
        TicTacToeUI.Instance.UpdateTurn(isXTurn);
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
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.sprite = null;
            boardState[i] = null;
        }
        isXTurn = true;
    }
}
