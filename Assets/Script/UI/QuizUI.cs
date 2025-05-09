using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuizUI : MonoBehaviour
{
    public static QuizUI Instance;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private GameObject QuizCanvas;
    [SerializeField] private TMP_Text quizTimer;
    private Question currentQuestion;
    private bool answered;
    public string difficulty;

    [Header("Background System")]
    [SerializeField] private Image BackgroundCanvas;

    void Awake()
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
        ChangeBackgroundColor();
    }


    public void SetQuestion(Question question)
    {
        currentQuestion = question;
        answered = false;

        questionText.text = question.question;
        if (question.questionImage != null)
        {
            questionImage.sprite = question.questionImage;
            questionImage.gameObject.SetActive(true);
        }
        else
        {
            questionImage.gameObject.SetActive(false);
        }

        // Shuffle and assign answer options
        for (int i = 0; i < optionButtons.Count; i++)
        {
            if (i < question.answers.Count)
            {
                Button btn = optionButtons[i];
                btn.gameObject.SetActive(true);

                TMP_Text buttonText = btn.GetComponentInChildren<TMP_Text>();
                buttonText.text = question.answers[i];
                buttonText.color = Color.black; // ✅ Set text color to black

                btn.name = question.answers[i];

                // Add listener with both answer and button reference
                btn.onClick.RemoveAllListeners();
                string answerText = question.answers[i]; // Capture correctly
                Button clickedButton = btn;              // Capture button reference
                btn.onClick.AddListener(() => SelectAnswer(answerText, clickedButton));

                // Reset button background color
                btn.image.color = Color.white;
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChangeBackgroundColor()
    {
        difficulty = QuizManager.Instance.GetSelectedDifficulty();
        Color newColor;
        switch (difficulty)
        {
            case "Easy":
                ColorUtility.TryParseHtmlString("#5BBE0A", out newColor);
                break;
            case "Medium":
                ColorUtility.TryParseHtmlString("#16A1D8", out newColor);
                break;
            case "Hard":
                ColorUtility.TryParseHtmlString("#8716D8", out newColor);
                break;
            default:
                ColorUtility.TryParseHtmlString("#5BBE0A", out newColor);
                break;
        }
        BackgroundCanvas.color = newColor;
    }

    void SelectAnswer(string selectedAnswer, Button clickedButton)
    {
        if (!answered)
        {
            answered = true;

            bool isCorrect = QuizManager.Instance.Answer(selectedAnswer);
            Debug.Log("answer : " + isCorrect);

            Color color;
            if (isCorrect)
            {
                ColorUtility.TryParseHtmlString("#66D70B", out color);
                AudioManager.Instance.PlayCorrectSFX();
            }
            else
            {
                ColorUtility.TryParseHtmlString("#E91515", out color); // Red
                AudioManager.Instance.PlayUnCorrectSFX();
            }

            clickedButton.image.color = color;
            TMP_Text buttonText = clickedButton.GetComponentInChildren<TMP_Text>();
            buttonText.color = Color.white;

            foreach (var button in optionButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

    public void ShowQuestion()
    {
        QuizCanvas.SetActive(true);
        currentQuestion = QuizManager.Instance.GetNextQuestion();
        SetQuestion(currentQuestion);
    }

    public void UpdateQuizTimer(int timeLeft)
    {
        quizTimer.text = timeLeft.ToString();

        if (timeLeft <= 10)
        {
            quizTimer.color = Color.red;
        }
        else
        {
            quizTimer.color = Color.black;
        }
    }

    public void ShowQuiz()
    {
        QuizCanvas.SetActive(true);
    }

    public void HideQuiz()
    {
        QuizCanvas.SetActive(false);
    }

}