using UnityEngine;
using UnityEngine.UI;

public class SphereInteraction : MonoBehaviour
{
    public int id;
    public bool isCorrectSphere = false;
    //public GameObject text;

    private void Start()
    {
        
    }
    public void Collect()
{
    if (!isCorrectSphere && FearManager.Instance != null)
    {
        FearManager.Instance.ChangeFear(25f);
    }

    // ------------------- EKLE -------------------
    CharacterMovement ai = FindObjectOfType<CharacterMovement>();
    if (ai != null)
    {
        ai.TeleportInstantly(transform.position);
    }
    // ------------------- EKLE -------------------

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