using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed = 5f;
    public GhostInteraction interactionHandler;
    
    void Update()
    {
        // Horizontal movement (A/D keys)
        float move = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(0, 0, move) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sphere"))
        {
            SphereInteraction sphere = other.GetComponent<SphereInteraction>();
            if (sphere != null)
            {
                interactionHandler.DiscoverSphere(sphere);
            }
        }
    }
}