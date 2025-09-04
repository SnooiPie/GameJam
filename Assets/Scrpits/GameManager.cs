using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public CharacterMovement character;
    public List<SphereInteraction> allSpheres; // Inspector’dan atanacak
    public GameObject door; // Kapı objesi

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var sphere in allSpheres)
            {
                if (sphere.CanActivate && sphere.id == 0) // Örnek: ID 0 ile başlıyoruz
                {
                    sphere.Interact();
                    character.MoveToSphere(sphere.transform, sphere);

                    // Kapı açıldı mantığı
                    door.SetActive(false);
                    Debug.Log("Kapı açıldı!");
                    break; // İlk uygun sphere ile işlemi yap
                }
            }
        }
    }
}
