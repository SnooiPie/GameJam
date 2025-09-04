using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public CharacterMovement character;
    public List<Transform> waypoints;
    public int ghostInteractedSphereID = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ghostInteractedSphereID >= 0)
        {
            SphereInteraction target = FindSphereByID(ghostInteractedSphereID);
            if (target != null)
            {
                character.MoveToSphere(waypoints, target);
            }
        }
    }

    SphereInteraction FindSphereByID(int id)
    {
        SphereInteraction[] spheres = FindObjectsOfType<SphereInteraction>();
        foreach (var s in spheres)
        {
            if (s.id == id) return s;
        }
        return null;
    }
}