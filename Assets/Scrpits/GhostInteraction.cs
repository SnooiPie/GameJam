using UnityEngine;
using System.Collections.Generic;

public class GhostInteraction : MonoBehaviour
{
    public CharacterMovement aiCharacter;   // Reference to your existing AI script
    private List<SphereInteraction> discoveredSpheres = new List<SphereInteraction>();
    private int currentTargetID = -1;
    
    void Update()
    {
        // Check if player is near any sphere and press E
        if (Input.GetKeyDown(KeyCode.E) && discoveredSpheres.Count > 0)
        {
            // You can modify this logic to choose which sphere to target
            // For now, let's target the most recently discovered sphere
            SphereInteraction targetSphere = discoveredSpheres[discoveredSpheres.Count - 1];
            aiCharacter.MoveToSphere(aiCharacter.waypoints, targetSphere);
            Debug.Log($"Sending AI to sphere ID: {targetSphere.id}");
        }
    }
    
    public void DiscoverSphere(SphereInteraction sphere)
    {
        if (!discoveredSpheres.Contains(sphere))
        {
            discoveredSpheres.Add(sphere);
            Debug.Log($"Discovered sphere with ID: {sphere.id}. Press E to send AI.");
        }
    }
}