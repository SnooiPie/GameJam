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
    {
        throw new NotImplementedException();
    }
}