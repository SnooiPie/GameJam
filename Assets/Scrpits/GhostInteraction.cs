using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    public SphereID CurrentSphere { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        var s = other.GetComponent<SphereID>();
        if (s != null) CurrentSphere = s;
    }

    void OnTriggerExit(Collider other)
    {
        var s = other.GetComponent<SphereID>();
        if (s != null && s == CurrentSphere) CurrentSphere = null;
    }
}
