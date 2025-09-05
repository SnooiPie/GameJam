using UnityEngine;

public class PuzzleTable : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public bool isCorrect = false;
    public GameObject successPanel;

    [Header("Animation Settings")]
    public float scaleAmount = 1.2f;
    public float animationSpeed = 5f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f; // Hareket hızı
    public float oscillationSpeed = 2f; // Salınım hızı
    public float oscillationAmount = 0.1f; // Salınım miktarı
    public float screenFillRatio = 0.8f; // Ekranın ne kadarını kaplaması (0.8 = %80)

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isHovering = false;
    private bool isAnimating = false;
    private bool isMovingToPlayer = false;
    private bool isAtPlayer = false;
    private Camera playerCamera;
    private Collider tableCollider;
    private float panelHideTimer = -1f;
    private float oscillationTimer = 0f;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        originalPosition = transform.position;
        targetPosition = originalPosition;
        playerCamera = Camera.main;

        // Collider'ı al veya ekle
        tableCollider = GetComponent<Collider>();
        if (tableCollider == null)
        {
            tableCollider = gameObject.AddComponent<BoxCollider>();
        }
    }

    void Update()
    {
        // ESC tuşu kontrolü
        if (Input.GetKeyDown(KeyCode.Escape) && (isAtPlayer || isMovingToPlayer))
        {
            ReturnToOriginalPosition();
        }

        // Sadece orijinal pozisyondayken hover kontrolü yap
        if (!isMovingToPlayer && !isAtPlayer)
        {
            CheckMousePosition();
        }

        // Scale animasyonu
        if (isAnimating)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);

            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                isAnimating = false;
            }
        }

        // Position animasyonu
        if (isMovingToPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                isMovingToPlayer = false;
                isAtPlayer = true;
                oscillationTimer = 0f;
            }
        }

        // Salınım animasyonu
        if (isAtPlayer)
        {
            oscillationTimer += Time.deltaTime * oscillationSpeed;
            float yOffset = Mathf.Sin(oscillationTimer) * oscillationAmount;
            transform.position = targetPosition + Vector3.up * yOffset;
        }

        // Mouse hover sırasında sol tık kontrolü (sadece orijinal pozisyondayken)
        if (isHovering && Input.GetMouseButtonDown(0) && !isMovingToPlayer && !isAtPlayer)
        {
            OnTableClick();
        }

        // Panel gizleme sayacı kontrolü
        if (panelHideTimer > 0f)
        {
            panelHideTimer -= Time.deltaTime;
            if (panelHideTimer <= 0f)
            {
                if (successPanel != null)
                {
                    successPanel.SetActive(false);
                }
            }
        }
    }

    void CheckMousePosition()
    {
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCamera.nearClipPlane));
        Vector3 direction = (transform.position - playerCamera.transform.position).normalized;
        float distance = Vector3.Distance(playerCamera.transform.position, transform.position);

        Ray ray = new Ray(playerCamera.transform.position, direction);
        RaycastHit hit;

        // Tablonun plane'ine raycast yap
        if (tableCollider.bounds.Contains(playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance))))
        {
            if (!isHovering)
            {
                isHovering = true;
                targetScale = originalScale * scaleAmount;
                isAnimating = true;
            }
        }
        else
        {
            if (isHovering)
            {
                isHovering = false;
                targetScale = originalScale;
                isAnimating = true;
            }
        }
    }

    void OnTableClick()
    {
        if (isCorrect)
        {
            // Başarı panelini göster
            if (successPanel != null)
            {
                successPanel.SetActive(true);
                panelHideTimer = 5f;
            }
        }
        else
        {
            Debug.Log("yanlış");
        }

        // Nesneyi kameraya doğru hareket ettir
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        // Nesnenin bounds'unu al
        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null) return;

        Bounds bounds = objectRenderer.bounds;
        float objectSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

        // Kamera için gerekli uzaklığı hesapla
        float distance = CalculateDistanceToFitObject(objectSize);

        // Ekranın ortası için hedef pozisyon hesapla
        Vector3 cameraForward = playerCamera.transform.forward;
        targetPosition = playerCamera.transform.position + cameraForward * distance;
        
        isMovingToPlayer = true;
        isHovering = false;
        targetScale = originalScale; // Scale'i normale döndür
        isAnimating = true;
    }

    float CalculateDistanceToFitObject(float objectSize)
    {
        // Kameranın field of view'una göre gerekli uzaklığı hesapla
        float fov = playerCamera.fieldOfView;
        float fovRadians = fov * Mathf.Deg2Rad;
        
        // Nesnenin ekranın belirtilen oranını kaplaması için gerekli uzaklık
        float distance = (objectSize / screenFillRatio) / (2f * Mathf.Tan(fovRadians / 2f));
        
        // Minimum ve maksimum uzaklık sınırları
        distance = Mathf.Clamp(distance, playerCamera.nearClipPlane + 0.5f, 20f);
        
        return distance;
    }

    void ReturnToOriginalPosition()
    {
        targetPosition = originalPosition;
        isMovingToPlayer = true;
        isAtPlayer = false;
        oscillationTimer = 0f;
        
        // Hareket tamamlandığında tekrar hover kontrolü yapılabilir
        StartCoroutine(ResetToOriginalState());
    }

    System.Collections.IEnumerator ResetToOriginalState()
    {
        // Pozisyon animasyonu tamamlanana kadar bekle
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            yield return null;
        }
        
        isMovingToPlayer = false;
        isAtPlayer = false;
        transform.position = originalPosition;
    }
}