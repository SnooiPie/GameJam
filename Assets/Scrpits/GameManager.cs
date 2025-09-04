using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GhostInteraction ghost;
    public CharacterMovement character;

    private List<Transform> allSpheres = new List<Transform>();

    void Start()
    {
        SphereInteraction[] spheres = Object.FindObjectsByType<SphereInteraction>(FindObjectsSortMode.None);
        foreach (var s in spheres)
        {
            allSpheres.Add(s.transform);
        }
    }

    void Update()
    {
        if (ghost.CurrentSphere != null && Input.GetKeyDown(KeyCode.E))
        {
            // Interact with the sphere
            ghost.CurrentSphere.Interact();

            // Only move character if interaction requirement met
            if (ghost.CurrentSphere.CanActivate)
            {
                character.MoveToSphere(ghost.CurrentSphere.transform, ghost.CurrentSphere);
            }
        }
    }
}
