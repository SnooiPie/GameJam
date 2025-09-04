using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform target;          // Takip edilecek karakter
    public Vector3 offset = new Vector3(0f, 5f, -7f); // Kameranýn konumu
    public float smoothSpeed = 0.125f; // Ne kadar yumuþak takip etsin

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon
        Vector3 desiredPosition = target.position + offset;

        // Yumuþak geçiþ
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Kamerayý hareket ettir
        transform.position = smoothedPosition;

        // Her zaman karaktere bak
        transform.LookAt(target);
    }
}
