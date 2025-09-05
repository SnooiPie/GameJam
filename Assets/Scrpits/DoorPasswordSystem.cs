using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorPasswordSystem : MonoBehaviour
{
    [Header("Door Settings")]
    public string correctPassword = "1234"; // Doğru şifre
    public Animator doorAnimator; // Kapı animatorü
    public string openAnimation = "Open"; // Açılma animasyonu adı
    public string closeAnimation = "Close"; // Kapanma animasyonu adı

    [Header("UI Settings")]
    public Canvas passwordCanvas; // Şifre canvas'ı
    public InputField passwordInputField; // Şifre giriş input'u
    public Text messageText; // Mesaj text'i
    public Button submitButton; // Gönder butonu
    public Button closeButton; // Kapat butonu

    [Header("Interaction Settings")]
    public float interactionDistance = 3f; // Etkileşim mesafesi
    public KeyCode interactionKey = KeyCode.E; // Etkileşim tuşu

    private bool isInRange = false;
    private bool isDoorOpen = false;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        InitializeUI();
    }

    void InitializeUI()
    {
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(false);
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(CheckPassword);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseCanvas);
        }

        if (passwordInputField != null)
        {
            // Enter tuşu ile gönderme
            passwordInputField.onEndEdit.AddListener(delegate { OnInputEndEdit(); });
        }
    }

    void Update()
    {
        CheckPlayerDistance();
        HandleInteraction();
    }

    void CheckPlayerDistance()
    {
        if (playerCamera == null) return;

        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        isInRange = distance <= interactionDistance;

        // Eğer çok uzaktaysak canvas'ı kapat
        if (!isInRange && passwordCanvas != null && passwordCanvas.gameObject.activeSelf)
        {
            CloseCanvas();
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

        // E tuşu ile etkileşim
        if (isInRange && Input.GetKeyDown(interactionKey) && !isDoorOpen)
        {
            if (!passwordCanvas.gameObject.activeSelf)
            {
                OpenCanvas();
            }
        }
    }

    void OnInputEndEdit()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckPassword();
        }
    }

    void OpenCanvas()
    {
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(true);
            
            // Input field'ı temizle ve focusla
            if (passwordInputField != null)
            {
                passwordInputField.text = "";
                passwordInputField.Select();
                passwordInputField.ActivateInputField();
            }

            // UI ayarları
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f; // Oyunu duraklat

            SetMessage("Şifreyi giriniz", Color.white);
        }
    }

    void CloseCanvas()
    {
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(false);
            
            // UI ayarları
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f; // Oyunu devam ettir
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
            
            // Input'u temizle ve yeniden focusla
            passwordInputField.text = "";
            passwordInputField.Select();
            passwordInputField.ActivateInputField();
            
            // Titreme efekti (opsiyonel)
            StartCoroutine(ShakeInputField());
        }
    }

    IEnumerator OpenDoorAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Realtime kullanıyoruz çünkü timescale 0
        
        OpenDoor();
        CloseCanvas();
    }

    void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.Play(openAnimation);
            isDoorOpen = true;
        }
        else
        {
            // Animator yoksa direkt rotate edelim
            transform.Rotate(0, 90, 0); // Kapıyı 90 derece aç
            isDoorOpen = true;
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
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            inputRT.localPosition = originalPos + new Vector3(x, y, 0);

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

    // Gizmos olarak etkileşim mesafesini göster
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}