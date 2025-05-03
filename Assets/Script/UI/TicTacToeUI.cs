using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    public static TicTacToeUI Instance { get; private set; }

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] buttons;

    [Header("Pause System")]
    [SerializeField] private GameObject ticTacToeCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [Header("Winner System")]
    [SerializeField] private GameObject winnerCanvas;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitWinnerButton;
    [SerializeField] private TMP_Text winnerText;

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
        restartButton.onClick.AddListener(RestartGame);
        quitWinnerButton.onClick.AddListener(QuitGame);
        quitButton.onClick.AddListener(QuitGame);
        ticTacToeCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(false);
    }

    void Start()
    {
        TicTacToeManager.Instance.StartTurnTimer();
    }

    public void HideTicTacToe()
    {
        ticTacToeCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(false);
    }

    public void ShowTicTacToe()
    {
        ticTacToeCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(false);
    }

    private void PauseGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        ticTacToeCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        TicTacToeManager.Instance.PauseTimer();
    }

    public void ResumeGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        ticTacToeCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(false);
        TicTacToeManager.Instance.ResumeTimer();
    }

    private void RestartGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        Debug.Log("Restart");
        SceneLoader.Instance.LoadGameplay();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.LoadMainMenu();
        // AudioManager.Instance.PlayMainThemeBacksound();
    }

    private void AssignButtonClickEvents()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => TicTacToeManager.Instance.SetButtonSpriteIndex(index));
        }
    }

    public void UpdateBoard(int index, Sprite sprite)
    {
        buttons[index].image.sprite = sprite;
    }

    public void UpdateTurn(bool isXTurn)
    {
        if (isXTurn)
        {
            turnText.text = "X";
            turnText.color = Color.black;
        }
        else
        {
            string difficulty = QuizManager.Instance.GetSelectedDifficulty();
            Color newColor;
            turnText.text = "O";

            switch (difficulty)
            {
                case "Easy":
                    ColorUtility.TryParseHtmlString("#5BBE0A", out newColor); // Green
                    break;
                case "Medium":
                    ColorUtility.TryParseHtmlString("#16A1D8", out newColor); // Blue
                    break;
                case "Hard":
                    ColorUtility.TryParseHtmlString("#8716D8", out newColor); // Purple
                    break;
                default:
                    ColorUtility.TryParseHtmlString("#5BBE0A", out newColor); // Default white
                    break;
            }
            turnText.color = newColor;
        }
    }


    public void ResetBoard()
    {
        foreach (var button in buttons)
        {
            button.image.sprite = null;
        }
    }

    public void UpdateTimer(int timeLeft)
    {
        timerText.text = timeLeft.ToString();

        if (timeLeft <= 10)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.black;
        }
    }

    public void GameFinish(string winner)
    {
        ticTacToeCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(true);
        TicTacToeManager.Instance.PauseTimer();
        AudioManager.Instance.PlayWinBacksound();
        if (winner == "X")
            winnerText.text = "SIMBOL X";
        else if (winner == "O")
            winnerText.text = "SIMBOL O";
        else
            winnerText.text = "Invalid";
    }
}