using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id;
    public bool isCorrectSphere = false;
    
    public void Collect()
    {
        Debug.Log($"Sphere {id} toplandı!");
        
        if (!isCorrectSphere && FearManager.Instance != null)
        {
            FearManager.Instance.IncreaseFear(25f);
        }
        
        // GameManager'daki ID'yi temizle
        if (GameManager.Instance != null && GameManager.Instance.currentSphereID == id)
        {
            GameManager.Instance.ClearCurrentSphereID();
        }
        
        Destroy(gameObject);
    }
    
    public void HighlightTemporarily()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
            Invoke("ResetColor", 2f);
        }
    }
    
    private void ResetColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = isCorrectSphere ? Color.green : Color.red;
        }
    }
}