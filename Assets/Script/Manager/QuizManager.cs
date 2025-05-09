using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

[System.Serializable]
public class QuestionList
{
    public List<Question> Easy;
    public List<Question> Medium;
    public List<Question> Hard;
}

[System.Serializable]
public class Question
{
    public string question;
    public string questionImagePath;
    public List<string> answers;
    public int correctAnswerIndex;
    public Sprite questionImage;
}

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    private Dictionary<string, List<Question>> allQuestions = new Dictionary<string, List<Question>>();
    private List<Question> currentQuestions;
    private Question currentQuestion;
    private int questionIndex;
    private int score;
    public bool isCorrect = false;
    [SerializeField] private int maxQuestions = 20;
    private string selectedDifficulty;
    private int currentQuizTimer;
    [SerializeField] private int quizTimer;
    [SerializeField] private int transitionDelay;
    private bool answered = false;
    private Coroutine questionTimerCoroutine;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentQuizTimer = quizTimer;
    }

    void Update()
    {
        // CheckTimerCondition();
    }

    public void StartQuiz()
    {
        if (!allQuestions.ContainsKey(selectedDifficulty))
        {
            Debug.LogError($"Difficulty {selectedDifficulty} not found in question list.");
            return;
        }

        if (allQuestions[selectedDifficulty].Count == 0)
        {
            Debug.LogError($"No questions available for {selectedDifficulty}.");
            return;
        }

        currentQuestions = ShuffleList.ShuffleListItems(new List<Question>(allQuestions[selectedDifficulty]));
        currentQuestions = currentQuestions.GetRange(0, Mathf.Min(maxQuestions, currentQuestions.Count));

        questionIndex = 0;
        score = 0;
        LoadNextQuestion();
    }

    public void StartQuizFlow()
    {
        StartCoroutine(LoadQuestionsAndStartQuiz());
    }

    private IEnumerator LoadQuestionsAndStartQuiz()
    {
        yield return StartCoroutine(LoadQuestionsFromJson());
        QuizUI.Instance.ShowQuiz();
        GameplayManager.Instance.CheckTimerCondition();
        StartQuiz();
    }

    public string GetSelectedDifficulty()
    {
        selectedDifficulty = ((DifficultyLevel)PlayerPrefs.GetInt("SelectedDifficulty", (int)DifficultyLevel.Easy)).ToString();
        return selectedDifficulty;
    }

    IEnumerator LoadQuestionsFromJson()
    {

        string fileName = selectedDifficulty + "Question.json";
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        if (Application.platform == RuntimePlatform.Android)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(path))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    ProcessJsonData(request.downloadHandler.text);
                    StartQuiz();
                }
                else
                {
                    Debug.LogError("Failed to load JSON: " + request.error);
                }
            }
        }
        else
        {
            if (File.Exists(path))
            {
                ProcessJsonData(File.ReadAllText(path));
                StartQuiz();
            }
            else
            {
                Debug.LogError("File not found: " + path);
            }
        }
    }

    void ProcessJsonData(string jsonData)
    {
        QuestionList loadedQuestions = JsonUtility.FromJson<QuestionList>(jsonData);

        if (loadedQuestions == null)
        {
            Debug.LogError("Failed to parse questions from JSON.");
            return;
        }

        allQuestions["Easy"] = FixQuestionData(loadedQuestions.Easy);
        allQuestions["Medium"] = FixQuestionData(loadedQuestions.Medium);
        allQuestions["Hard"] = FixQuestionData(loadedQuestions.Hard);
    }

    private List<Question> FixQuestionData(List<Question> questions)
    {
        if (questions == null || questions.Count == 0) return new List<Question>();

        foreach (var q in questions)
        {
            q.questionImage = LoadImage(q.questionImagePath);
        }
        return questions;
    }

    private Sprite LoadImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return null;
        string resourcePath = Path.Combine("Question Image", imagePath).Replace("\\", "/");
        if (resourcePath.EndsWith(".png"))
            resourcePath = resourcePath.Substring(0, resourcePath.Length - 4);
        return Resources.Load<Sprite>(resourcePath);
    }

    public void LoadNextQuestion()
    {
        if (questionIndex < currentQuestions.Count)
        {
            currentQuestion = currentQuestions[questionIndex];
            QuizUI.Instance.SetQuestion(currentQuestion);
            questionIndex++;
            answered = false;
            currentQuizTimer = quizTimer;

            if (questionTimerCoroutine != null)
            {
                StopCoroutine(questionTimerCoroutine);
            }

            StartQuestionTimer();
        }
        else
        {
            EndQuiz();
        }
    }

    private void StartQuestionTimer()
    {
        if (questionTimerCoroutine != null)
        {
            StopCoroutine(questionTimerCoroutine);
        }
        questionTimerCoroutine = StartCoroutine(QuestionTimer());
    }

    private void StopQuestionTimer()
    {
        if (questionTimerCoroutine != null)
        {
            StopCoroutine(questionTimerCoroutine);
            questionTimerCoroutine = null;
        }
    }

    public bool Answer(string selectedAnswer)
    {
        isCorrect = selectedAnswer == currentQuestion.answers[currentQuestion.correctAnswerIndex];
        StopQuestionTimer();
        if (isCorrect) score++;
        answered = true;
        StartCoroutine(TransitionDelay());
        return isCorrect;
    }

    private IEnumerator QuestionTimer()
    {
        while (currentQuizTimer > 0)
        {
            QuizUI.Instance.UpdateQuizTimer(currentQuizTimer);
            if (currentQuizTimer <= 10)
            {
                AudioManager.Instance.PlayTimerSFXPanic();
                // AudioManager.Instance.StopBacksound();
            }

            yield return new WaitForSeconds(1f);
            currentQuizTimer--;
        }

        QuizUI.Instance.UpdateQuizTimer(0);

        if (!answered)
        {
            answered = true;
            isCorrect = false;
            QuizUI.Instance.HideQuiz();
            GameplayManager.Instance.HandleQuizResult(isCorrect);
            LoadNextQuestion();
            // Change to tic tac toe
        }
    }

    private IEnumerator TransitionDelay()
    {
        yield return new WaitForSeconds(transitionDelay);
        QuizUI.Instance.HideQuiz();
        GameplayManager.Instance.HandleQuizResult(isCorrect);
    }

    public Question GetNextQuestion()
    {
        if (questionIndex < currentQuestions.Count)
        {
            return currentQuestions[questionIndex++];
        }
        else
        {
            return null;
        }
    }

    void EndQuiz()
    {
        Debug.Log("Quiz Ended! Score: " + score + "/" + currentQuestions.Count);
    }
}