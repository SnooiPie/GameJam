using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxPuzzle : MonoBehaviour
{
    // M�zik kutular�n�n do�ru renk kombinasyonu
    public string[] correctColorSequence = new string[3];

    // Oyuncunun se�ti�i renk kombinasyonu
    private string[] playerColorSequence = new string[3];

    // Her m�zik kutusunun do�ru olup olmad���
    private bool[] correctDisks = new bool[3];

    // T�m diskler do�ru yerle�tirildi mi?
    private bool allCorrect = false;

    // UI i�in referans
    private MusicBoxUI musicBoxUI;

    void Start()
    {
        // Do�ru renk s�ras�n� rastgele belirle (�rne�in)
        // Ger�ek oyunda bu de�erler ba�ka bir mekanizma taraf�ndan belirlenmeli
        correctColorSequence[0] = "K�rm�z�";
        correctColorSequence[1] = "Ye�il";
        correctColorSequence[2] = "Mavi";

        // UI scriptini bul
        musicBoxUI = FindObjectOfType<MusicBoxUI>();
        if (musicBoxUI == null)
        {
            Debug.LogError("MusicBoxUI bulunamad�! Sahneye ekledi�inden emin ol.");
        }
    }

    // M�zik kutusuna t�kland���nda �a�r�l�r
    public void OnMusicBoxClicked(int boxIndex)
    {
        // UI'� a� ve hangi kutuya renk atanaca��n� belirt
        if (musicBoxUI != null)
        {
            musicBoxUI.OpenColorSelection(boxIndex, this);
        }
    }

    // Renk se�ildi�inde �a�r�l�r
    public void ColorSelected(int boxIndex, string color)
    {
        // Se�ilen rengi kaydet
        playerColorSequence[boxIndex] = color;

        // Do�ru renk mi kontrol et
        if (playerColorSequence[boxIndex] == correctColorSequence[boxIndex])
        {
            correctDisks[boxIndex] = true;
            Debug.Log(boxIndex + " numaral� kutuya DO�RU renk yerle�tirildi: " + color);
        }
        else
        {
            correctDisks[boxIndex] = false;
            Debug.Log(boxIndex + " numaral� kutuya YANLI� renk yerle�tirildi: " + color);
        }

        // T�m diskler do�ru mu kontrol et
        CheckAllDisks();
    }

    // T�m disklerin do�ru olup olmad���n� kontrol eder
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

    // T�m diskler do�ru yerle�tirildi�inde �a�r�l�r
    private void Open()
    {
        Debug.Log("Tebrikler! T�m diskler do�ru yerle�tirildi. Kap� a��ld�!");
        // Buraya kap�y� a�ma veya puzzle'� tamamlama kodunu ekle
    }
}