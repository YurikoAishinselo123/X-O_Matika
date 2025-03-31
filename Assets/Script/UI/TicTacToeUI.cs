using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    public static TicTacToeUI Instance { get; private set; }

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] buttons;

    private int xScore = 0;
    private int oScore = 0;

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

    private void RestartGame()
    {
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
            buttons[i].onClick.AddListener(() => TicTacToeManager.Instance.SetButtonSprite(index));
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
        else if (winner == "Y")
            winnerText.text = "SIMBOL Y";
        else
            winnerText.text = "Invalid";
    }
}
