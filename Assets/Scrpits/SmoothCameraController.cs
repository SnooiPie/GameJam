using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform target;          // Takip edilecek karakter
    public Vector3 offset = new Vector3(0f, 5f, -7f); // Kameran�n konumu
    public float smoothSpeed = 0.125f; // Ne kadar yumu�ak takip etsin

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon
        Vector3 desiredPosition = target.position + offset;

        // Yumu�ak ge�i�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Kameray� hareket ettir
        transform.position = smoothedPosition;

        // Her zaman karaktere bak
        transform.LookAt(target);
    }
}
