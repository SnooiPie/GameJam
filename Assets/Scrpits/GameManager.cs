using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GhostInteraction ghost;
    public CharacterMovement character;
    public GameObject door; // Kapı objesi

    void Update()
    {
        if (ghost.currentSphere != null && ghost.currentSphere.CanActivate && Input.GetKeyDown(KeyCode.E))
        {
            // Karakter sphere’a hareket ediyor
            character.MoveToSphere(ghost.currentSphere.transform, ghost.currentSphere);

            // Kapı açıldı durumu
            door.SetActive(false); // Kapıyı gizle / açıldı mantığı
            Debug.Log("Kapı açıldı!");
        }
    }
}
