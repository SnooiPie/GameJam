using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int id; // Sphere ID
    private bool isInteracted = false;

    // Ghost intereact için
    public void Interact()
    {
        if (!isInteracted)
        {
            isInteracted = true;
            Debug.Log($"Sphere {id} interacted.");
        }
    }

    // Karakter geldiğinde tetiklenecek
    public void TriggerAction()
    {
        Debug.Log($"Sphere {id} collected by character.");
        Destroy(gameObject); // İster toplama mantığı
    }

    // GameManager tarafından kontrol edilecek
    public bool CanActivate => !isInteracted;
}
