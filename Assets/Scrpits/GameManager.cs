using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public CharacterMovement character;
    public List<Transform> waypoints;
    public int currentSphereID = -1;
    private SphereInteraction currentTargetSphere; // Mevcut hedef sphere'i takip et

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentSphereID != -1)
        {
            // Mevcut hedef sphere'i bul
            currentTargetSphere = FindSphereByID(currentSphereID);
            
            if (currentTargetSphere != null && character != null)
            {
                Debug.Log($"AI, {currentTargetSphere.id} ID'li sphere'e gönderiliyor...");
                
                // ID'yi hemen temizle (tekrar E basmayı engelle)
                int sentSphereID = currentSphereID;
                currentSphereID = -1;
                
                character.MoveToSphere(waypoints, currentTargetSphere, sentSphereID);
            }
            else
            {
                Debug.LogError("Sphere veya karakter bulunamadı!");
            }
        }
    }

    public void SetCurrentSphereID(int sphereID)
    {
        // Sadece yeni bir sphere seçilirse güncelle
        if (sphereID != currentSphereID)
        {
            currentSphereID = sphereID;
            Debug.Log($"GameManager: Sphere ID {sphereID} ayarlandı");
        }
    }

    public void ClearCurrentSphereID()
    {
        currentSphereID = -1;
        currentTargetSphere = null;
        Debug.Log("GameManager: Sphere ID temizlendi");
    }

    SphereInteraction FindSphereByID(int id)
    {
        SphereInteraction[] spheres = FindObjectsOfType<SphereInteraction>();
        foreach (var sphere in spheres)
        {
            if (sphere.id == id && sphere.gameObject.activeInHierarchy)
            {
                return sphere;
            }
        }
        return null;
    }
}