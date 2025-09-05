using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SphereInteraction : MonoBehaviour
{
    public int id;
    public bool isCorrectSphere = false;

    private void Start()
    {
        
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