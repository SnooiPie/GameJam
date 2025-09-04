using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id; // Sphere ID
    
    public void Collect()
    {
        Debug.Log($"Sphere {id} collected!");
        Destroy(gameObject);
    }
    
    public void OnDiscovered()
    {
        // Keşfedildiğinde görsel feedback
        GetComponent<Renderer>().material.color = Color.yellow;
        Debug.Log($"Sphere {id} discovered!");
    }
}