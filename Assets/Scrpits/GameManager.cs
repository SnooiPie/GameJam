using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public CharacterMovement character;
    public List<Transform> waypoints;
    public int currentSphereID = -1;
    private SphereInteraction currentTargetSphere; // Mevcut hedef sphere'i takip et

    public List<int> correctOrder = new List<int> { 1, 2, 3 }; // Doğru sıra
    public List<int> currentOrder = new List<int>();
    public Text orderText; // UI referansı

    public List<int> diskOrder = new List<int> { 0, 1, 2 }; // örnek: mavi, kırmızı, yeşil
    public int currentDiskIndex = 0; // AI'nın sıradaki hedefi

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

    public void AddDiskToOrder(int id)
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
            var renkler = currentOrder.ConvertAll(id => GetDiskColorName(id));
            orderText.text = "Sıra: " + string.Join(" - ", renkler);
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
            orderText.text = "Yanlış sıra! Diskler geri verildi.";
            ReturnDisks();
        }
    }

    void ReturnDisks()
    {
        // Diskleri geri ver, sıfırla
        currentOrder.Clear();
        UpdateOrderUI();
        // Diskleri sahneye geri koymak için ek kod ekle
    }

    public void ResetDiskOrder()
    {
        currentOrder.Clear();
        currentDiskIndex = 0;
        UpdateOrderUI();
        // Diskleri sahneye geri koymak için ek kod ekleyebilirsin
    }

    string GetDiskColorName(int id)
    {
        switch (id)
        {
            case 0: return "Kırmızı";
            case 1: return "Yeşil";
            case 2: return "Mavi";
            default: return "Bilinmiyor";
        }
    }

    public void ShowOnlySphere1Colors()
    {
        var sphere = FindSphereByID(1);
        if (sphere != null && orderText != null)
        {
            var renkler = sphere.diskColors.ConvertAll(id => GetDiskColorName(id));
            orderText.text = "Sphere 1 Renkleri: " + string.Join(" - ", renkler);
        }
        else
        {
            orderText.text = "Sphere 1 veya renkler bulunamadı.";
        }
    }
}