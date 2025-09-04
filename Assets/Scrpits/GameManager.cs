using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GhostInteraction ghost;
    public CharacterMovement character;

    private List<Transform> allSpheres = new List<Transform>();

    void Start()
    {
        // Find all spheres with SphereID
        SphereID[] spheres = FindObjectsOfType<SphereID>();
        foreach (var s in spheres)
        {
            allSpheres.Add(s.transform);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ghost != null && ghost.CurrentSphere != null)
            {
                // 👇 Call MoveToSphere instead of MoveThroughSpheres
                character.MoveToSphere(ghost.CurrentSphere.transform, ghost.CurrentSphere.GetComponent<SphereInteraction>());
            }
        }
    }
}
