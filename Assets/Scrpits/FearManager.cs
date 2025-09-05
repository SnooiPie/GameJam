using UnityEngine;
using UnityEngine.UI;

public class FearManager : MonoBehaviour
{
    public static FearManager Instance;

    [Header("Fear Settings")]
    public float maxFear = 100f;
    public float currentFear = 0f;
    public float fearDecreaseRate = 2f;
    public bool showDebugLogs = true;

    [Header("UI References")]
    public Slider fearSlider;
    public Image fearFillImage;
    public Color lowFearColor = Color.green;
    public Color mediumFearColor = Color.yellow;
    public Color highFearColor = Color.red;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Text gameOverText;
    public Text scoreText;

    private int totalScore = 0;
    private int correctSpheres = 0;
    private int wrongSpheres = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Ensure only one instance
    }

    void Start()
    {
        InitializeUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // Gradually decrease fear
        if (currentFear > 0f)
        {
            ChangeFear(-fearDecreaseRate * Time.deltaTime);
        }
    }

    /// <summary>
    /// Change fear by a positive or negative amount.
    /// </summary>
    public void ChangeFear(float amount)
    {
        if (amount == 0) return;

        currentFear = Mathf.Clamp(currentFear + amount, 0f, maxFear);

        if (showDebugLogs)
            Debug.Log("Fear changed by " + amount + " | Current Fear: " + currentFear);

        UpdateFearUI();

        if (currentFear >= maxFear)
            GameOver();
    }

    /// <summary>
    /// Add points to score.
    /// </summary>
    public void AddScore(int points, bool isCorrect)
    {
        if (isCorrect && points > 0)
        {
            totalScore += points;
            correctSpheres++;
        }
        else
        {
            wrongSpheres++;
        }

        UpdateScoreUI();

        if (showDebugLogs)
            Debug.Log($"Score: {totalScore} | Correct: {correctSpheres} | Wrong: {wrongSpheres}");
    }

    private void InitializeUI()
    {
        if (fearSlider != null)
        {
            fearSlider.maxValue = maxFear;
            fearSlider.value = currentFear;
        }

        UpdateFearUI();
        UpdateScoreUI();
    }

    private void UpdateFearUI()
    {
        if (fearSlider != null)
            fearSlider.value = currentFear;

        if (fearFillImage != null)
        {
            float percent = currentFear / maxFear;
            fearFillImage.color = percent < 0.4f ? lowFearColor :
                                  percent < 0.7f ? mediumFearColor : highFearColor;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {totalScore}\nCorrect: {correctSpheres}\nWrong: {wrongSpheres}";
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER! Final Score: " + totalScore);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
                gameOverText.text = $"GAME OVER!\nFinal Score: {totalScore}\nCorrect: {correctSpheres} | Wrong: {wrongSpheres}";
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        currentFear = 0f;
        totalScore = 0;
        correctSpheres = 0;
        wrongSpheres = 0;
        InitializeUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}
