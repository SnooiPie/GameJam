using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GhostInteraction ghost;        // drag your Ghost here
    public CharacterMovement character;   // drag your Character here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ghost != null && ghost.CurrentSphere != null)
            {
                character.MoveToSphere(ghost.CurrentSphere.transform);
            }
        }
    }
}
