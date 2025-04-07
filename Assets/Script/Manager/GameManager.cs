using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [SerializeField] private TicTacToeManager ticTacToeManager;
    [SerializeField] private QuizManager quizManager;

    private bool isXTurn = true;

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

    private void Start()
    {
        StartNewTurn();
    }

    public void StartNewTurn()
    {
        
    }

    public void OnAnswerChecked(bool isCorrect)
    {
        if (isCorrect)
        {
            ticTacToeManager.EnableTicTacToe(isXTurn);
        }
        else
        {
            SwapTurn();
        }
    }

    public void SwapTurn()
    {
        isXTurn = !isXTurn;
        StartNewTurn();
    }
}
