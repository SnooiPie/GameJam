using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportManager : MonoBehaviour
{
    [Header("Teleport Settings")]
    private Transform teleportTarget; // Teleport olunacak hedef nokta
    public Transform teleportTarget1; // Teleport olunacak hedef nokta
    public Transform teleportTarget2; // Teleport olunacak hedef nokta
    public GameObject player; // Oyuncu objesi
    public Text interactionText; // Ekranda gösterilecek text
    public KeyCode teleportKey = KeyCode.T; // Teleport tuşu

    private bool isInTrigger = false;

    void Start()
    {
        // Başlangıçta text'i gizle
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Eğer trigger içindeysek ve T tuşuna basılırsa
        if (isInTrigger && Input.GetKeyDown(teleportKey))
        {
            TeleportPlayer();
        }
        if (player.transform.position.y > 0)
        {
            teleportTarget = teleportTarget2;
        }
        else
        {
            teleportTarget = teleportTarget1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Sadece player trigger'a girdiğinde
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            ShowText(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Player trigger'dan çıktığında
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            ShowText(false);
        }
    }

    void ShowText(bool show)
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(show);
            if (show)
            {
                interactionText.text = "T'ye basarak teleport ol";
            }
        }
    }

    void TeleportPlayer()
    {
        if (player != null && teleportTarget != null)
        {
            // Player'ı teleport et
            player.transform.position = teleportTarget.position;
            
            // Text'i gizle
            ShowText(false);
            //isInTrigger = false;
            
            Debug.Log("Teleport gerçekleştirildi!");
        }
        else
        {
            Debug.LogWarning("Player veya teleport target ayarlanmamış!");
        }
    }
}