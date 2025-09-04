using UnityEngine;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    public float moveSpeed = 3f;
    
    private SphereInteraction targetSphere;
    private bool isMoving = false;
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    public void MoveToSphere(List<Transform> pathWaypoints, SphereInteraction sphere)
    {
        if (isMoving) 
        {
            Debug.Log("AI is already moving!");
            return;
        }
        
        waypoints = pathWaypoints;
        targetSphere = sphere;
        currentWaypointIndex = 0;
        isMoving = true;
        Debug.Log($"AI moving to sphere ID: {targetSphere.id}");
    }

    private void MoveToTarget()
    {
        if (currentWaypointIndex < waypoints.Count && waypoints[currentWaypointIndex] != null)
        {
            // Mevcut waypoint'e hareket et
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetWaypoint.position, 
                moveSpeed * Time.deltaTime
            );
            
            // Waypoint'e doğru dönüş
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
            
            // Waypoint'e ulaşıp ulaşmadığını kontrol et
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
            {
                currentWaypointIndex++;
            }
        }
        else if (currentWaypointIndex >= waypoints.Count)
        {
            // Tüm waypoint'ler tamamlandı
            isMoving = false;
            if (targetSphere != null)
            {
                targetSphere.Collect();
                targetSphere = null;
            }
        }
    }

    // Editor'da waypoint'leri görselleştirme
    void OnDrawGizmos()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawSphere(waypoints[i].position, 0.3f);
                    if (i < waypoints.Count - 1 && waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    }
                }
            }
        }
    }
}