using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    public static TicTacToeUI Instance { get; private set; }

    [SerializeField] private TMP_Text xScoreText;
    [SerializeField] private TMP_Text oScoreText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] buttons;


    [Header("Pause System")]
    [SerializeField] private GameObject ticTacToeCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;


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
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PauseGame()
    {
        ticTacToeCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    private void ResumeGame()
    {
        ticTacToeCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    private void QuitGame()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    private void AssignButtonClickEvents()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
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
