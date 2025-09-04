using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id = 0;
    private bool isInteracted = false;

    public void Interact()
    {
        if (!isInteracted)
        {
            isInteracted = true;
            Debug.Log($"Sphere {id} interacted.");
        }
    }

    public void TriggerAction()
    {
        Debug.Log($"Sphere {id} collected by character.");
        Destroy(gameObject); // Sphere toplandıktan sonra yok edilebilir
    }

    public bool CanActivate => !isInteracted;
}
