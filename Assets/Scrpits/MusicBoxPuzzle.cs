using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxPuzzle : MonoBehaviour
{
    // Müzik kutularýnýn doðru renk kombinasyonu
    public string[] correctColorSequence = new string[3];

    // Oyuncunun seçtiði renk kombinasyonu
    private string[] playerColorSequence = new string[3];

    // Her müzik kutusunun doðru olup olmadýðý
    private bool[] correctDisks = new bool[3];

    // Tüm diskler doðru yerleþtirildi mi?
    private bool allCorrect = false;

    // UI için referans
    private MusicBoxUI musicBoxUI;

    void Start()
    {
        // Doðru renk sýrasýný rastgele belirle (örneðin)
        // Gerçek oyunda bu deðerler baþka bir mekanizma tarafýndan belirlenmeli
        correctColorSequence[0] = "Kýrmýzý";
        correctColorSequence[1] = "Yeþil";
        correctColorSequence[2] = "Mavi";

        // UI scriptini bul
        musicBoxUI = FindObjectOfType<MusicBoxUI>();
        if (musicBoxUI == null)
        {
            Debug.LogError("MusicBoxUI bulunamadý! Sahneye eklediðinden emin ol.");
        }
    }

    // Müzik kutusuna týklandýðýnda çaðrýlýr
    public void OnMusicBoxClicked(int boxIndex)
    {
        // UI'ý aç ve hangi kutuya renk atanacaðýný belirt
        if (musicBoxUI != null)
        {
            musicBoxUI.OpenColorSelection(boxIndex, this);
        }
    }

    // Renk seçildiðinde çaðrýlýr
    public void ColorSelected(int boxIndex, string color)
    {
        // Seçilen rengi kaydet
        playerColorSequence[boxIndex] = color;

        // Doðru renk mi kontrol et
        if (playerColorSequence[boxIndex] == correctColorSequence[boxIndex])
        {
            correctDisks[boxIndex] = true;
            Debug.Log(boxIndex + " numaralý kutuya DOÐRU renk yerleþtirildi: " + color);
        }
        else
        {
            correctDisks[boxIndex] = false;
            Debug.Log(boxIndex + " numaralý kutuya YANLIÞ renk yerleþtirildi: " + color);
        }

        // Tüm diskler doðru mu kontrol et
        CheckAllDisks();
    }

    // Tüm disklerin doðru olup olmadýðýný kontrol eder
    private void CheckAllDisks()
    {
        allCorrect = true;

        for (int i = 0; i < 3; i++)
        {
            if (!correctDisks[i])
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            Open();
        }
    }

    // Tüm diskler doðru yerleþtirildiðinde çaðrýlýr
    private void Open()
    {
        Debug.Log("Tebrikler! Tüm diskler doðru yerleþtirildi. Kapý açýldý!");
        // Buraya kapýyý açma veya puzzle'ý tamamlama kodunu ekle
    }
}