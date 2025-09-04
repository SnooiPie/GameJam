using System;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform target;              // Takip edilecek karakter
    public Transform[] cameraPositions;   // 4 farklı kamera noktası
    public float smoothSpeed = 0.125f;    // Kameranın ne kadar yumuşak hareket edeceği

    void LateUpdate()
    {
        if (target == null || cameraPositions.Length == 0) return;

        // En yakın kamera pozisyonunu bul
        Transform closestCamPos = cameraPositions[0];
        float closestDistance = Vector3.Distance(target.position, closestCamPos.position);

        foreach (Transform camPos in cameraPositions)
        {
            float distance = Vector3.Distance(target.position, camPos.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCamPos = camPos;
            }
        }

        // Kameranın hedef pozisyonu
        Vector3 desiredPosition = closestCamPos.position;

        // Smooth hareket
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
