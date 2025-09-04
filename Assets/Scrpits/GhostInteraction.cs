using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    // Ghost artık sadece E tuşunu basmayı sağlar
    // Sphere seçimi GameManager üzerinden yapılır
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // GameManager logic’ine bağlı
        }
    }
}
