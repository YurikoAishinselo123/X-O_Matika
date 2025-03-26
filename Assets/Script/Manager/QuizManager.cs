using UnityEngine;
using System.Collections.Generic;
using System.IO;

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
    public QuestionType questionType;
    public string questionImagePath; // Store image path instead of Sprite
    public List<string> answers; // Match JSON key "answers" instead of "options"
    public int correctAnswerIndex;

    public Sprite questionImage; // Load dynamically
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE
}

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [SerializeField] private GameplayUI gameplayUI;
    private Dictionary<string, List<Question>> allQuestions = new Dictionary<string, List<Question>>();
    private List<Question> currentQuestions;
    private Question currentQuestion;
    private int questionIndex;
    private int score;
    private string selectedDifficulty;

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
        LoadQuestionsFromJson();
        StartQuiz("Easy");
    }

    public void StartQuiz(string difficulty)
    {
        selectedDifficulty = difficulty;
        if (allQuestions.ContainsKey(difficulty))
        {
            currentQuestions = ShuffleList.ShuffleListItems(allQuestions[difficulty]);
            currentQuestions = currentQuestions.GetRange(0, Mathf.Min(5, currentQuestions.Count));

            questionIndex = 0;
            score = 0;
            LoadNextQuestion();
        }
        else
        {
            Debug.LogError("Difficulty level not found!");
        }
    }

    void LoadQuestionsFromJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "questions.json");

        if (File.Exists(path))
        {
            Debug.Log($"Found JSON file at: {path}");

            string jsonData = File.ReadAllText(path);
            Debug.Log($"Raw JSON Data: {jsonData}");

            QuestionList loadedQuestions = JsonUtility.FromJson<QuestionList>(jsonData);

            if (loadedQuestions != null)
            {
                allQuestions["Easy"] = FixQuestionData(loadedQuestions.Easy);
                allQuestions["Medium"] = FixQuestionData(loadedQuestions.Medium);
                allQuestions["Hard"] = FixQuestionData(loadedQuestions.Hard);

                Debug.Log($"Questions Loaded: Easy({allQuestions["Easy"].Count}), Medium({allQuestions["Medium"].Count}), Hard({allQuestions["Hard"].Count})");
            }
            else
            {
                Debug.LogError("Failed to parse questions from JSON.");
            }
        }
        else
        {
            Debug.LogError($"Question file not found at: {path}");
        }
    }

    private List<Question> FixQuestionData(List<Question> questions)
    {
        if (questions == null) return new List<Question>();

        foreach (var q in questions)
        {
            q.questionImage = LoadImage(q.questionImagePath);
        }

        return questions;
    }

    private Sprite LoadImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return null;

        string fullPath = Path.Combine(Application.streamingAssetsPath, imagePath);
        if (File.Exists(fullPath))
        {
            byte[] imageData = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageData))
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }

        Debug.LogWarning($"Image not found at {fullPath}");
        return null;
    }

    public void LoadNextQuestion()
    {
        if (questionIndex < currentQuestions.Count)
        {
            currentQuestion = currentQuestions[questionIndex];
            gameplayUI.SetQuestion(currentQuestion);
            questionIndex++;
        }
        else
        {
            EndQuiz();
        }
    }

    public bool Answer(string selectedAnswer)
    {
        bool isCorrect = selectedAnswer == currentQuestion.answers[currentQuestion.correctAnswerIndex];
        if (isCorrect) score++;

        LoadNextQuestion();
        return isCorrect;
    }

    void EndQuiz()
    {
        Debug.Log("Quiz Ended! Score: " + score + "/" + currentQuestions.Count);
    }
}
