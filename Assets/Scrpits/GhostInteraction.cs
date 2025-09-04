using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    private SphereID currentSphere;

    // 👈 Public property so GameManager can see it
    public SphereID CurrentSphere => currentSphere;

    void OnTriggerEnter(Collider other)
    {
        SphereID sphere = other.GetComponent<SphereID>();
        if (sphere != null)
        {
            currentSphere = sphere;
        }
    }

    void OnTriggerExit(Collider other)
    {
        SphereID sphere = other.GetComponent<SphereID>();
        if (sphere != null && sphere == currentSphere)
        {
            currentSphere = null;
        }
    }
}
