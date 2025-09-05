using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Bu satırı EKLE

public class FearManager : MonoBehaviour
{
    public static FearManager Instance;
    
    public float maxFear = 100f;
    public float currentFear = 0f;
    public float wrongSphereFearIncrease = 25f;
    
    public Slider fearSlider;
    public Image fearFillImage;
    public Color lowFearColor = Color.green;
    public Color highFearColor = Color.red;
    
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
    
    public void IncreaseFear(float amount)
    {
        currentFear += amount;
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
}