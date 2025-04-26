using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    public static TicTacToeUI Instance { get; private set; }

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] buttons;
    [SerializeField] private QuizUI gameplayUI;

    [Header("Pause System")]
    [SerializeField] private GameObject ticTacToeCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [Header("Winner System")]
    [SerializeField] private GameObject winnerCanvas;
    [SerializeField] private Button restartButton;
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
        quitButton.onClick.AddListener(QuitGame);
        ticTacToeCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(false);
    }

    void Start()
    {
        // AudioManager.Instance.StopBacksound();
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
        ticTacToeCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        TicTacToeManager.Instance.PauseTimer();
    }

    public void ResumeGame()
    {
        ticTacToeCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        winnerCanvas.SetActive(false);
        TicTacToeManager.Instance.ResumeTimer();
    }

    private void RestartGame()
    {
        Debug.Log("Restart");
        SceneLoader.Instance.LoadGameplay();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.LoadMainMenu();
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
        turnText.text = isXTurn ? "GILIRAN X" : "GILIRAN O";
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

        if (winner == "X")
            winnerText.text = "SIMBOL X";
        else if (winner == "O")
            winnerText.text = "SIMBOL O";
        else
            winnerText.text = "Invalid";
    }
}