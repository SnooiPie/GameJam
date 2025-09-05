using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed = 5f;
    private int lastTriggeredSphereID = -1;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        // Hareket
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(-moveVertical, 0.0f, moveHorizontal);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sphere"))
        {
            SphereInteraction sphere = other.GetComponent<SphereInteraction>();
            if (sphere != null)
            {
                lastTriggeredSphereID = sphere.id;
                
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetCurrentSphereID(sphere.id);
                    Debug.Log($"Sphere {sphere.id} seçildi. E tuşu ile AI gönder.");
                }
                
                sphere.HighlightTemporarily();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sphere"))
        {
            SphereInteraction sphere = other.GetComponent<SphereInteraction>();
            if (sphere != null && sphere.id == lastTriggeredSphereID)
            {
                Debug.Log("Sphere alanından çıkıldı, ID temizleniyor");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ClearCurrentSphereID();
                }
                lastTriggeredSphereID = -1;
            }
        }
    }
}