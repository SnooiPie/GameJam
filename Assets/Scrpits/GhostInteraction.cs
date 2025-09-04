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
            
            string sphereType = sphere.isCorrectSphere ? "Doğru" : "Yanlış";
            Debug.Log($"Keşfedildi: Sphere {sphere.id} ({sphereType}). E tuşuna basarak AI gönder.");
            
            // Tüm keşfedilen sphere'leri göster
            ShowDiscoveredSpheres();
        }
    }
    
    void ShowDiscoveredSpheres()
    {
        string available = "Keşfedilen Sphere'ler: ";
        foreach (SphereInteraction s in discoveredSpheres)
        {
            available += $"{s.id}({(s.isCorrectSphere ? "✓" : "✗")}) ";
        }
        Debug.Log(available);
    }
}