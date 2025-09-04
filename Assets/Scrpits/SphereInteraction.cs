using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id; // Sphere ID

    public void Collect()
    {
        Debug.Log($"Sphere {id} collected!");
        Destroy(gameObject);
    }
}
