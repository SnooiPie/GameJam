using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed = 180f;
    private int lastTriggeredSphereID = -1;

    public Transform[] borderPoints = new Transform[4];

    float xmin, xmax, zmin, zmax;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xmin, xmax);
        pos.z = Mathf.Clamp(pos.z, zmin, zmax);
        transform.position = pos;
    }

    void Start()
    {

        if (borderPoints.Length == 0) return;

        xmin = float.MaxValue;
        xmax = float.MinValue;
        zmin = float.MaxValue;
        zmax = float.MinValue;

        foreach (var t in borderPoints)
        {
            if (t == null) continue;
            xmin = Mathf.Min(xmin, t.position.x);
            xmax = Mathf.Max(xmax, t.position.x);
            zmin = Mathf.Min(zmin, t.position.z);
            zmax = Mathf.Max(zmax, t.position.z);
        }

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