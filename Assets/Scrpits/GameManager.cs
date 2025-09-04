using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GhostInteraction ghost;
    public CharacterMovement character;

    // Sahnedeki tüm sphere’ler
    private List<Transform> allSpheres = new List<Transform>();

    void Start()
    {
        // SphereID componenti olan tüm objeleri bul
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
                character.MoveThroughSpheres(allSpheres, ghost.CurrentSphere.transform);
            }
        }
    }
}
