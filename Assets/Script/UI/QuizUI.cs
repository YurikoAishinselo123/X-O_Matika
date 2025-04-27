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
        // Debug.Log($"Setting Question: {question.question}");
        // Debug.Log($"Options: {string.Join(", ", question.answers)}");

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
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = question.answers[i];
                optionButtons[i].name = question.answers[i];

                // Add listener for answer selection
                optionButtons[i].onClick.RemoveAllListeners();
                string answerText = question.answers[i]; // Avoids captured variable issue
                optionButtons[i].onClick.AddListener(() => SelectAnswer(answerText));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

    }

    private void ChangeBackgroundColor()
    {
        string difficulty = QuizManager.Instance.GetSelectedDifficulty();
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

    void SelectAnswer(string selectedAnswer)
    {
        if (!answered)
        {
            HideQuiz();
            answered = true;
            bool isCorrect = QuizManager.Instance.Answer(selectedAnswer);
            Debug.Log("answer : " + isCorrect);
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

    private void HideQuiz()
    {
        QuizCanvas.SetActive(false);
    }

}