using UnityEngine;

public class PuzzleTable : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public bool isCorrect = false;
    public GameObject successPanel;

    [Header("Animation Settings")]
    public float scaleAmount = 1.2f;
    public float animationSpeed = 5f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isHovering = false;
    private bool isAnimating = false;
    private Camera playerCamera;
    private Collider tableCollider;
    private float panelHideTimer = -1f; // Panel gizleme sayacý eklendi

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        playerCamera = Camera.main;

        // Collider'ý al veya ekle
        tableCollider = GetComponent<Collider>();
        if (tableCollider == null)
        {
            tableCollider = gameObject.AddComponent<BoxCollider>();
        }
    }

    void Update()
    {
        CheckMousePosition();

        if (isAnimating)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);

            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                isAnimating = false;
            }
        }

        // Mouse hover sýrasýnda sol týk kontrolü
        if (isHovering && Input.GetMouseButtonDown(0))
        {
            OnTableClick();
        }

        // Panel gizleme sayacý kontrolü eklendi
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
        float distance = Vector3.Distance(playerCamera.transform.position, transform.position);  //buralar mecbur yapay zekadan alýndý

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
            if (successPanel != null)
            {
                successPanel.SetActive(true);
                panelHideTimer = 5f;
            }
        }
        else
        {
            Debug.Log("yanlýþ");
        }
    }
}