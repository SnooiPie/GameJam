using UnityEngine;
using UnityEngine.UI;

public class FearManager : MonoBehaviour
{
    public static FearManager Instance;
    
    public float maxFear = 100f;
    public float currentFear = 0f;
    public float wrongSphereFearIncrease = 25f;
    public float fearDecreaseRate = 5f; // Saniyede azalma miktarı
    
    public Slider fearSlider;
    public Image fearFillImage;
    public Color lowFearColor = Color.green;
    public Color highFearColor = Color.red;
    
    public GameObject gameOverPanel;
    public Text gameOverText;
    
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
        UpdateFearUI();
        gameOverPanel.SetActive(false);
    }
    
    void Update()
    {
        // Zamanla korkuyu azalt (opsiyonel)
        if (currentFear > 0)
        {
            currentFear -= fearDecreaseRate * Time.deltaTime;
            currentFear = Mathf.Clamp(currentFear, 0, maxFear);
            UpdateFearUI();
        }
    }
    
    public void IncreaseFear(float amount)
    {
        currentFear += amount;
        currentFear = Mathf.Clamp(currentFear, 0, maxFear);
        
        UpdateFearUI();
        
        // Korku maksimuma ulaştığında oyunu bitir
        if (currentFear >= maxFear)
        {
            GameOver();
        }
    }
    
    public void DecreaseFear(float amount)
    {
        currentFear -= amount;
        currentFear = Mathf.Clamp(currentFear, 0, maxFear);
        UpdateFearUI();
    }
    
    void UpdateFearUI()
    {
        if (fearSlider != null)
        {
            fearSlider.value = currentFear / maxFear;
        }
        
        if (fearFillImage != null)
        {
            fearFillImage.color = Color.Lerp(lowFearColor, highFearColor, currentFear / maxFear);
        }
    }
    
    void GameOver()
    {
        Debug.Log("Game Over! Fear level reached maximum!");
        gameOverPanel.SetActive(true);
        gameOverText.text = "ÇOK KORKTUN!\nOyun Bitti";
        
        // Oyunu duraklat
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}