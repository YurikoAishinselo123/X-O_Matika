using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    public static TicTacToeUI Instance { get; private set; }

    [SerializeField] private TMP_Text xScoreText;
    [SerializeField] private TMP_Text oScoreText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] buttons; // Array for all 9 buttons

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

        AssignButtonClickEvents();
    }

    private void AssignButtonClickEvents()
    {
        // Loop through all buttons and assign a click event
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Fixes closure issue in loops
            buttons[i].onClick.AddListener(() => TicTacToeManager.Instance.SetButtonSprite(index));
        }
    }

    public void UpdateBoard(int index, Sprite sprite)
    {
        buttons[index].image.sprite = sprite;
    }

    public void UpdateScore(bool isXWinner)
    {
        int xScore = GetScore(xScoreText.text);
        int oScore = GetScore(oScoreText.text);

        if (isXWinner)
            xScore++;
        else
            oScore++;

        xScoreText.text = $"X: {xScore}";
        oScoreText.text = $"O: {oScore}";
    }

    private int GetScore(string scoreText)
    {
        string[] parts = scoreText.Split(':');
        return parts.Length < 2 ? 0 : int.TryParse(parts[1].Trim(), out int score) ? score : 0;
    }

    public void UpdateTurn(bool isXTurn)
    {
        turnText.text = isXTurn ? "GILIRAN X" : "GILIRAN O";
    }

    public void ResetBoard()
    {
        foreach (var button in buttons)
        {
            button.image.sprite = null;
        }
    }
}
