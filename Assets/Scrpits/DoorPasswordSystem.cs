using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DoorPasswordSystem : MonoBehaviour
{
    [Header("Kapı Ayarları")]
    public string correctPassword = "1234"; // Doğru şifre
    public GameObject doorObject; // Kapı objesi
    public Vector3 openRotation = new Vector3(0, 90, 0); // Kapı açılma rotasyonu

    [Header("UI Ayarları")]
    public Canvas passwordCanvas; // Şifre canvas'ı
    public InputField passwordInputField; // Şifre giriş input'u
    public Text messageText; // Mesaj text'i
    public Button submitButton; // Gönder butonu
    public Button closeButton; // Kapat butonu

    [Header("Etkileşim Ayarları")]
    public Text interactionText; // "E'ye bas" yazısı
    public KeyCode interactionKey = KeyCode.E; // Etkileşim tuşu

    private bool isInTrigger = false;
    private bool isDoorOpen = false;
    private Vector3 originalDoorRotation;

    void Start()
    {
        InitializeUI();
        
        // Kapının orijinal rotasyonunu kaydet
        if (doorObject != null)
        {
            originalDoorRotation = doorObject.transform.eulerAngles;
        }
    }

    void InitializeUI()
    {
        // Başlangıçta UI elementlerini gizle
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(false);
        }

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        // Buton eventlerini ayarla
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(CheckPassword);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseCanvas);
        }

        // Input field eventi (Enter ile gönderme)
        if (passwordInputField != null)
        {
            passwordInputField.onEndEdit.AddListener(delegate { OnInputEndEdit(); });
        }
    }

    void Update()
    {
        HandleInteraction();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDoorOpen)
        {
            isInTrigger = true;
            ShowInteractionText(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            ShowInteractionText(false);
            
            // Eğer dışarı çıkılırsa canvas'ı da kapat
            if (passwordCanvas != null && passwordCanvas.gameObject.activeSelf)
            {
                CloseCanvas();
            }
        }
    }

    void HandleInteraction()
    {
        // ESC ile canvas'ı kapat
        if (passwordCanvas != null && passwordCanvas.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCanvas();
            return;
        }

        // E tuşu ile etkileşim (sadece trigger içindeyse ve kapı kapalıysa)
        if (isInTrigger && Input.GetKeyDown(interactionKey) && !isDoorOpen)
        {
            if (!passwordCanvas.gameObject.activeSelf)
            {
                OpenCanvas();
                Debug.Log("Kapı şifre girişi açıldı.");
            }
        }
    }

    void OnInputEndEdit()
    {
        // Enter tuşu ile gönderme
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckPassword();
        }
    }

    void ShowInteractionText(bool show)
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(show);
            if (show)
            {
                interactionText.text = "E'ye basarak şifre gir";
            }
        }
    }

    void OpenCanvas()
    {
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(true);
            
            // Input field'ı hazırla
            if (passwordInputField != null)
            {
                passwordInputField.text = "";
                passwordInputField.Select();
                passwordInputField.ActivateInputField();
            }

            // SADECE fare imlecini göster, kilidini açma (FPS oyunu için)
            Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None; // BU SATIRI KALDIR veya yorum satırı yap

            SetMessage("Şifreyi giriniz", Color.white);
            
            // Interaction text'i gizle
            ShowInteractionText(false);
            
            // Player hareketini durdur (opsiyonel)
            // FindObjectOfType<PlayerMovement>().SetMovement(false);
        }
    }

    void CloseCanvas()
    {
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(false);
            
            // SADECE fare imlecini gizle, kilitleme (FPS oyunu için)
            Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked; // BU SATIRI KALDIR veya yorum satırı yap

            // Eğer hala trigger içindeyse interaction text'i göster
            if (isInTrigger && !isDoorOpen)
            {
                ShowInteractionText(true);
            }
            
            // Player hareketini devam ettir (opsiyonel)
            // FindObjectOfType<PlayerMovement>().SetMovement(true);
        }
    }

    public void CheckPassword()
    {
        if (passwordInputField == null) return;

        string enteredPassword = passwordInputField.text;

        if (enteredPassword == correctPassword)
        {
            // Şifre doğru
            SetMessage("Şifre doğru! Kapı açılıyor...", Color.green);
            StartCoroutine(OpenDoorAfterDelay(1.5f));
        }
        else
        {
            // Şifre yanlış
            SetMessage("Şifre yanlış! Tekrar deneyin.", Color.red);
            
            // Input'u temizle
            passwordInputField.text = "";
            passwordInputField.Select();
            passwordInputField.ActivateInputField();
            
            // Titreme efekti
            StartCoroutine(ShakeInputField());
        }
    }

    IEnumerator OpenDoorAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        OpenDoor();
        CloseCanvas();
    }

    void OpenDoor()
    {
        if (doorObject != null)
        {
            // KAPIDOĞRU EKSENDE AÇILACAK
            doorObject.transform.Rotate(openRotation);
            isDoorOpen = true;
            
            // Trigger'ı devre dışı bırak (artık etkileşim olmasın)
            GetComponent<Collider>().isTrigger = false;
            
            Debug.Log("Kapı açıldı! Yeni rotasyon: " + doorObject.transform.eulerAngles);
        }
    }

    IEnumerator ShakeInputField()
    {
        if (passwordInputField == null) yield break;

        RectTransform inputRT = passwordInputField.GetComponent<RectTransform>();
        Vector3 originalPos = inputRT.localPosition;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 10f;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            inputRT.localPosition = originalPos + new Vector3(x, 0, 0);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        inputRT.localPosition = originalPos;
    }

    void SetMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
        }
    }
}