using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    private SphereInteraction currentSphere;

    // Public property for GameManager to access
    public SphereInteraction CurrentSphere => currentSphere;

    void OnTriggerEnter(Collider other)
    {
        SphereInteraction sphere = other.GetComponent<SphereInteraction>();
        if (sphere != null)
        {
            currentSphere = sphere;
        }
    }

    void OnTriggerExit(Collider other)
    {
        SphereInteraction sphere = other.GetComponent<SphereInteraction>();
        if (sphere != null && sphere == currentSphere)
        {
            currentSphere = null;
        }
    }
}
