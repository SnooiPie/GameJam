using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    public int sphereID;                // Unique ID for this sphere
    public int requiredInteractions = 1; // How many times before activation
    private int currentInteractions = 0;

    public bool CanActivate => currentInteractions >= requiredInteractions;

    public void Interact()
    {
        currentInteractions++;
        Debug.Log($"Sphere {sphereID} interacted {currentInteractions} times.");
    }

    public void TriggerAction()
    {
        // Put unique animation or effect here
        Debug.Log($"Sphere {sphereID} triggered its action!");
    }
}
