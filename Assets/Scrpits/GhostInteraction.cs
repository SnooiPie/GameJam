using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    private SphereID currentSphere; // the sphere ghost is inside

    void OnTriggerEnter(Collider other)
    {
        SphereID sphere = other.GetComponent<SphereID>();
        if (sphere != null)
        {
            currentSphere = sphere;
            Debug.Log("Ghost near sphere " + sphere.id);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SphereID>() == currentSphere)
        {
            currentSphere = null;
        }
    }

    public SphereID GetCurrentSphere()
    {
        return currentSphere;
    }
}
