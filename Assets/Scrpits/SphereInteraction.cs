using System;
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent için ekle

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
        Debug.Log($"Sphere {id} collected! Type: {(isSpecialKaset ? "Kaset" : "Disk")}");

        if (isSpecialKaset && GameManager.Instance != null)
        {
            GameManager.Instance.OnSpecialKasetCollected();
        }

        if (GameManager.Instance != null && GameManager.Instance.currentSphereID == id)
        {
            GameManager.Instance.ClearCurrentSphereID();
        }

        // Move AI to this sphere's position using NavMeshAgent
        CharacterMovement ai = FindObjectOfType<CharacterMovement>();
        if (ai != null)
        {
            NavMeshAgent agent = ai.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                Debug.Log("AI found, moving with NavMeshAgent...");
                agent.SetDestination(transform.position);
            }
            else
            {
                Debug.LogWarning("NavMeshAgent not found on AI!");
            }
        }
        else
        {
            Debug.LogWarning("AI not found!");
        }

        // Increase fear if needed
        if (FearManager.Instance != null)
        {
            FearManager.Instance.ChangeFear(25f);
        }

        Destroy(gameObject);
    }

    public void HighlightTemporarily(float duration = 1f)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = Color.yellow;
            Invoke(nameof(ResetColor), duration);
        }
    }

    void ResetColor()
    {
        SetSphereColor();
    }
}