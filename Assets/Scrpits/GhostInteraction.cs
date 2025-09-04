using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    private SphereInteraction currentSphere;

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
        if (other.GetComponent<SphereInteraction>() == currentSphere)
        {
            currentSphere = null;
        }
    }

    void Update()
    {
        if (currentSphere != null && Input.GetKeyDown(KeyCode.E))
        {
            currentSphere.Interact();

            // Only move if the sphere is ready (interactions reached)
            if (currentSphere.CanActivate)
            {
                CharacterMovement character = FindObjectOfType<CharacterMovement>();
                character.MoveToSphere(currentSphere.transform, currentSphere);
            }
        }
    }
}
