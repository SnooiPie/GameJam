using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentSphereID = -1;
    public List<int> collectedDisks = new List<int>();
    public int[] correctDiskSequence = { 1, 2, 3 };
    private int currentExpectedDiskIndex = 0;

    // UI Elements - IMAGE BASED
    public Image[] diskSlots = new Image[3]; // 3 tane image slotu
    public Sprite[] diskSprites = new Sprite[3]; // K, M, Y sprite'ları
    public Sprite emptySlotSprite; // Boş slot sprite'ı
    public GameObject diskUIPanel; // Disk UI paneli

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Başlangıçta slotları boş göster
        UpdateDisksUI();
    }

    public void SetCurrentSphereID(int id)
    {
        currentSphereID = id;
    }

    public void ClearCurrentSphereID()
    {
        currentSphereID = -1;
    }

    // Özel kaset toplandığında
    public void OnSpecialKasetCollected()
    {
        collectedDisks.AddRange(new int[] { 1, 2, 3 });
        UpdateDisksUI();
        diskUIPanel.SetActive(true);
        Debug.Log("3 müzik diski envantere eklendi!");
    }

    // UI Güncelleme - IMAGE BASED
    public void UpdateDisksUI()
    {
        for (int i = 0; i < diskSlots.Length; i++)
        {
            if (diskSlots[i] != null)
            {
                if (i < collectedDisks.Count)
                {
                    // Diski göster
                    int diskID = collectedDisks[i];
                    diskSlots[i].sprite = diskSprites[diskID - 1]; // ID 1-based
                    diskSlots[i].color = Color.white;
                }
                else
                {
                    // Boş slot
                    diskSlots[i].sprite = emptySlotSprite;
                    diskSlots[i].color = Color.gray;
                }
            }
        }
    }

    // Müzik kutusu etkileşimi
    public bool TryInteractWithMusicBox(int boxIndex)
    {
        if (collectedDisks.Count == 0)
        {
            Debug.Log("Hiç disk yok!");
            return false;
        }

        if (currentExpectedDiskIndex < correctDiskSequence.Length)
        {
            int expectedDisk = correctDiskSequence[currentExpectedDiskIndex];

            if (collectedDisks[0] == expectedDisk)
            {
                // DOĞRU - İlk diski kaldır
                collectedDisks.RemoveAt(0);
                currentExpectedDiskIndex++;
                UpdateDisksUI();

                Debug.Log($"Doğru! Kutu {boxIndex}'e disk yerleştirildi.");

                if (collectedDisks.Count == 0)
                {
                    Debug.Log("TEBRİKLER! Puzzle çözüldü!");
                    diskUIPanel.SetActive(false);
                }

                return true;
            }
        }

        // YANLIŞ
        Debug.Log("Yanlış sıra! Fear arttı.");
        if (FearManager.Instance != null)
        {
            FearManager.Instance.IncreaseFear(30f);
        }
        return false;
    }
}