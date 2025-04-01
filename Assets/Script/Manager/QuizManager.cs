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
        StartCoroutine(LoadQuestionsFromJson());
    }

    public void StartQuiz()
    {
        selectedDifficulty = ((DifficultyLevel)PlayerPrefs.GetInt("SelectedDifficulty", (int)DifficultyLevel.Easy)).ToString();

        Debug.Log($"Starting Quiz with difficulty: {selectedDifficulty}");

        if (allQuestions.ContainsKey(selectedDifficulty) && allQuestions[selectedDifficulty].Count > 0)
        {
            currentQuestions = ShuffleList.ShuffleListItems(new List<Question>(allQuestions[selectedDifficulty]));
            currentQuestions = currentQuestions.GetRange(0, Mathf.Min(5, currentQuestions.Count));

            questionIndex = 0;
            score = 0;
            LoadNextQuestion();
        }
        else
        {
            Debug.LogError($"No questions found for difficulty: {selectedDifficulty}");
        }
    }
    IEnumerator LoadQuestionsFromJson()
    {
        string difficulty = ((DifficultyLevel)PlayerPrefs.GetInt("SelectedDifficulty", (int)DifficultyLevel.Easy)).ToString();
        string fileName = difficulty + "Question.json"; // Example: EasyQuestion.json
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        Debug.Log($"Loading JSON from path: {path}");
        Debug.Log("Detected Platform: " + Application.platform);

        if (Application.platform == RuntimePlatform.Android)
        {
            if (path.Contains("://"))  // Android requires UnityWebRequest
            {
                using (UnityWebRequest request = UnityWebRequest.Get(path))
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        ProcessJsonData(request.downloadHandler.text);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load JSON: {request.error}");
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid Android path: " + path);
            }
        }
        else
        {
            // Unity Editor & PC/Mac use normal file access
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                ProcessJsonData(jsonData);
                Debug.Log($"Successfully loaded JSON from: {path}");
            }
            else
            {
                Debug.LogError($"File not found at: {path}");
            }
        }
    }

    void ProcessJsonData(string jsonData)
    {
        Debug.Log($"Loaded JSON: {jsonData}");

        QuestionList loadedQuestions = JsonUtility.FromJson<QuestionList>(jsonData);

        if (loadedQuestions != null)
        {
            allQuestions["Easy"] = FixQuestionData(loadedQuestions.Easy);
            allQuestions["Medium"] = FixQuestionData(loadedQuestions.Medium);
            allQuestions["Hard"] = FixQuestionData(loadedQuestions.Hard);

            Debug.Log($"Questions Loaded: Easy({allQuestions["Easy"].Count}), Medium({allQuestions["Medium"].Count}), Hard({allQuestions["Hard"].Count})");

            StartQuiz();
        }
        else
        {
            Debug.LogError("Failed to parse questions from JSON.");
        }
    }

    private List<Question> FixQuestionData(List<Question> questions)
    {
        if (questions == null || questions.Count == 0) return new List<Question>();

        foreach (var q in questions)
        {
            q.questionImage = LoadImage(q.questionImagePath);

            if (q.questionImage == null)
            {
                Debug.LogWarning($"Failed to load image for question: {q.question} (Path: {q.questionImagePath})");
            }
            else
            {
                Debug.Log($"Successfully loaded image for question: {q.question}");
            }
        }

        return questions;
    }



    // private IEnumerator LoadImageCoroutine(string imagePath, System.Action<Sprite> callback)
    // {
    //     if (string.IsNullOrEmpty(imagePath))
    //     {
    //         callback?.Invoke(null);
    //         yield break;
    //     }
    //     string fullPath = Path.Combine(Application.streamingAssetsPath, imagePath, ".png");
    //     // string fullPath = Path.Combine(Application.streamingAssetsPath, "Question Image", imagePath);

    //     Debug.Log($"Attempting to load image from: {fullPath}");

    //     if (Application.platform == RuntimePlatform.Android)
    //     {
    //         using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(fullPath))
    //         {
    //             yield return request.SendWebRequest();

    //             if (request.result == UnityWebRequest.Result.Success)
    //             {
    //                 Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    //                 Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    //                 callback?.Invoke(sprite);
    //                 yield break;
    //             }
    //             else
    //             {
    //                 Debug.LogError($"Failed to load image: {request.error}");
    //             }
    //         }
    //     }
    //     else
    //     {
    //         if (File.Exists(fullPath))
    //         {
    //             byte[] imageData = File.ReadAllBytes(fullPath);
    //             Texture2D texture = new Texture2D(2, 2);
    //             if (texture.LoadImage(imageData))
    //             {
    //                 callback?.Invoke(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f));
    //                 yield break;
    //             }
    //         }
    //         Debug.LogWarning($"Image not found at {fullPath}");
    //     }

    //     callback?.Invoke(null);
    // }
    private Sprite LoadImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return null;

        // Convert path to a format compatible with Resources.Load
        string resourcePath = Path.Combine("Question Image", imagePath).Replace("\\", "/");

        // Remove .png extension (Resources.Load does NOT use file extensions)
        if (resourcePath.EndsWith(".png"))
            resourcePath = resourcePath.Substring(0, resourcePath.Length - 4);

        Debug.Log($"🔍 Loading image from Resources: {resourcePath}");

        // Load the sprite from the Resources folder
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite != null)
        {
            Debug.Log($"Successfully loaded image from Resources: {resourcePath}");
            return sprite;
        }

        Debug.LogWarning($"Image not found in Resources: {resourcePath}");
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
