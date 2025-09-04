using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GhostInteraction ghost;
    public CharacterMovement character;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SphereID sphere = ghost.GetCurrentSphere();
            if (sphere != null)
            {
                character.MoveToSphere(sphere.transform);
            }
        }
    }
}
