using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id; // Sphere ID
    public bool isCorrectSphere = false; // Inspector'da hangi sphere'lerin doğru olduğunu işaretle
    
    public void Collect()
    {
        Debug.Log($"Sphere {id} collected!");
        
        // Doğru veya yanlış sphere kontrolü
        if (!isCorrectSphere)
        {
            FearManager.Instance.IncreaseFear(25f);
            Debug.Log($"Yanlış sphere! Korku arttı: +25");
        }
        else
        {
            Debug.Log($"Doğru sphere! İlerleme kaydedildi.");
            // Doğru sphere için ödül veya ilerleme ekleyebilirsin
        }
        
        Destroy(gameObject);
    }
    
    public void OnDiscovered()
    {
        // Keşfedildiğinde görsel feedback
        GetComponent<Renderer>().material.color = isCorrectSphere ? Color.green : Color.red;
        Debug.Log($"Sphere {id} discovered! ({(isCorrectSphere ? "Doğru" : "Yanlış")})");
    }
}