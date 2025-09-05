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

    [Header("Karakter Ayarları")]
    public GameObject hiddenCharacter; // Kapı açıldığında görünür olacak karakter

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

        // Karakteri başta gizle
        if (hiddenCharacter != null)
        {
            hiddenCharacter.SetActive(false);
        }
    }

    void InitializeUI()
    {
        if (passwordCanvas != null) passwordCanvas.gameObject.SetActive(false);
        if (interactionText != null) interactionText.gameObject.SetActive(false);

        if (submitButton != null) submitButton.onClick.AddListener(CheckPassword);
        if (closeButton != null) closeButton.onClick.AddListener(CloseCanvas);

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

            if (passwordCanvas != null && passwordCanvas.gameObject.activeSelf)
            {
                CloseCanvas();
            }
        }
    }

    void HandleInteraction()
    {
        if (passwordCanvas != null && passwordCanvas.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCanvas();
            return;
        }

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

            if (passwordInputField != null)
            {
                passwordInputField.text = "";
                passwordInputField.Select();
                passwordInputField.ActivateInputField();
            }

            Cursor.visible = true;

            SetMessage("Şifreyi giriniz", Color.white);
            ShowInteractionText(false);
        }
    }

    void CloseCanvas()
    {
        if (passwordCanvas != null)
        {
            passwordCanvas.gameObject.SetActive(false);
            Cursor.visible = false;

            if (isInTrigger && !isDoorOpen)
            {
                ShowInteractionText(true);
            }
        }
    }

    public void CheckPassword()
    {
        if (passwordInputField == null) return;

        string enteredPassword = passwordInputField.text;

        if (enteredPassword == correctPassword)
        {
            SetMessage("Şifre doğru! Kapı açılıyor...", Color.green);
            StartCoroutine(OpenDoorAfterDelay(1.5f));
        }
        else
        {
            SetMessage("Şifre yanlış! Tekrar deneyin.", Color.red);

            passwordInputField.text = "";
            passwordInputField.Select();
            passwordInputField.ActivateInputField();

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
            doorObject.transform.Rotate(openRotation);
            isDoorOpen = true;
            GetComponent<Collider>().isTrigger = false;

            Debug.Log("Kapı açıldı! Yeni rotasyon: " + doorObject.transform.eulerAngles);

            // Karakteri görünür yap
            if (hiddenCharacter != null)
            {
                hiddenCharacter.SetActive(true);
            }
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
