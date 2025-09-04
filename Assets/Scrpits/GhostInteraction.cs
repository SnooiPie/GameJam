using UnityEngine;
using System.Collections.Generic;

public class GhostInteraction : MonoBehaviour
{
    public CharacterMovement aiCharacter;
    private List<SphereInteraction> discoveredSpheres = new List<SphereInteraction>();
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && discoveredSpheres.Count > 0)
        {
            // En son keşfedilen sphere'i hedefle
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
            sphere.OnDiscovered();
            Debug.Log($"Discovered sphere with ID: {sphere.id}. Press E to send AI.");
            
            // Tüm keşfedilen sphere'leri göster
            string available = "Discovered spheres: ";
            foreach (SphereInteraction s in discoveredSpheres)
            {
                available += s.id + " ";
            }
            Debug.Log(available);
        }
    }
}