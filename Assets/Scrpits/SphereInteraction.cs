using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // List kullanabilmek için ekledik

public class SphereInteraction : MonoBehaviour
{
    public int id;
    public bool isCorrectSphere = false;
    public List<int> diskColors = new List<int> { 0, 1, 2 }; // Örnek: Kırmızı, Yeşil, Mavi
    //public GameObject text;

    private void Start()
    {
        
    }
    public void Collect()
{
    // Sıralama kontrolü
    if (GameManager.Instance.diskOrder[GameManager.Instance.currentDiskIndex] == id)
    {
        GameManager.Instance.AddDiskToOrder(id); // UI ve sıralama güncellenir
        GameManager.Instance.currentDiskIndex++; // Bir sonraki disk
        Destroy(gameObject); // Disk yok edilir
    }
    else
    {
        // Yanlış disk alındı, hata mesajı veya geri verme işlemi
        Debug.Log("Yanlış disk! Sıra bozuldu.");
        // İstersen diskleri geri ver, sıralamayı sıfırla
        GameManager.Instance.ResetDiskOrder();
    }
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

    private void OnDestroy()
    {
        CharacterMovement ai = FindObjectOfType<CharacterMovement>();
        if (ai != null)
        {
            ai.TeleportInstantly(transform.position);
        }
    }
}