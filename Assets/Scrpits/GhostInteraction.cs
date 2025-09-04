using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    public SphereInteraction currentSphere;

    void Update()
    {
        if (currentSphere != null && Input.GetKeyDown(KeyCode.E))
        {
            currentSphere.Interact();
        }
    }
}
