using System;
using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id;
    public bool isSpecialKaset = false;

    void Start()
    {
        SetSphereColor();
    }

    void SetSphereColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            switch (id)
            {
                case 1: renderer.material.color = Color.red; break;
                case 2: renderer.material.color = Color.blue; break;
                case 3: renderer.material.color = Color.green; break;
            }
        }
    }

    public void Collect()
{
    if (!isCorrectSphere && FearManager.Instance != null)
    {
        Debug.Log($"Sphere {id} toplandı! - Tür: {(isSpecialKaset ? "Kaset" : "Disk")}");

        if (isSpecialKaset && GameManager.Instance != null)
        {
            GameManager.Instance.OnSpecialKasetCollected();
        }

        if (GameManager.Instance != null && GameManager.Instance.currentSphereID == id)
        {
            GameManager.Instance.ClearCurrentSphereID();
        }

        Destroy(gameObject);
    }

    internal void HighlightTemporarily()
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
        throw new NotImplementedException();
    }
}