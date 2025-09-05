using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public CharacterMovement character;
    public List<Transform> waypoints;
    public int currentSphereID = -1;
    private SphereInteraction currentTargetSphere;

    public List<int> correctOrder = new List<int> { 1, 2, 3 }; // Doğru sıra
    public List<int> currentOrder = new List<int>();
    public Text orderText; // UI referansı

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
            currentTargetSphere = FindSphereByID(currentSphereID);
            
            if (currentTargetSphere != null && character != null)
            {
                Debug.Log($"AI, {currentTargetSphere.id} ID'li sphere'e gönderiliyor...");
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

    // Disk ile ilgili fonksiyonlar kaldırıldı

    // Sıra güncellemesi için fonksiyon
    public void AddSphereToOrder(int id)
    {
        currentOrder.Add(id);
        UpdateOrderUI();

        if (currentOrder.Count == correctOrder.Count)
        {
            CheckOrder();
        }
    }

    void UpdateOrderUI()
    {
        if (orderText != null)
        {
            orderText.text = "Sıra: " + string.Join(" - ", currentOrder);
        }
    }

    void CheckOrder()
    {
        bool isCorrect = true;
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (currentOrder[i] != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            orderText.text = "Doğru! Puzzle çözüldü.";
            // Puzzle tamamlandı, ödül vs.
        }
        else
        {
            orderText.text = "Yanlış sıra! Tekrar deneyin.";
            currentOrder.Clear();
            UpdateOrderUI();
        }
    }
}

