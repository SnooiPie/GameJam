using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Horizontal");
        float moveX = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;

        transform.Translate(movement, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sphere"))
        {
            Debug.Log("Ghost touched the sphere!");
        }
    }
}
