using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private GameObject QuizCanvas;
    [SerializeField] private TMP_Text quizTimer;
    private Question currentQuestion;
    private bool answered;
    private QuizManager quizManager;

    void Start()
    {
        quizManager = QuizManager.Instance;
        if (quizManager == null)
        {
            Debug.LogError("QuizManager instance not found!");
        }
    }

    public void SetQuestion(Question question)
    {
        currentQuestion = question;
        answered = false;
        Debug.Log($"Setting Question: {question.question}");
        Debug.Log($"Options: {string.Join(", ", question.answers)}");

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

    void SelectAnswer(string selectedAnswer)
    {
        if (!answered)
        {
            answered = true;
            bool isCorrect = quizManager.Answer(selectedAnswer);
            Debug.Log("Answer is " + (isCorrect ? "Correct!" : "Wrong!"));
        }
    }

    public void ShowQuestion()
    {
        QuizCanvas.SetActive(true);
        currentQuestion = quizManager.GetNextQuestion();
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

}