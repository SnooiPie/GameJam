using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        SphereInteraction sphere = other.GetComponent<SphereInteraction>();
        if (sphere != null)
        {
            sphere.Collect();
        }
    }
}