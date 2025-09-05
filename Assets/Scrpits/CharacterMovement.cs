using UnityEngine;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public List<Transform> waypoints;       // AI karakterin geçeceği waypointler
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
        if (isMoving) return;
        
        waypoints = pathWaypoints; // Use the provided waypoints
        targetSphere = sphere;
        currentWaypointIndex = 0;
        isMoving = true;
        Debug.Log($"AI moving to sphere ID: {targetSphere.id}");
    }
    
    private void MoveToTarget()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            // Move to the current waypoint
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetWaypoint.position, 
                moveSpeed * Time.deltaTime
            );
            
            // Check if reached the waypoint
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Reached the final waypoint (near the sphere)
            isMoving = false;
            if (targetSphere != null)
            {
                targetSphere.Collect();
                targetSphere = null;
            }
        }
<<<<<<< Updated upstream
=======

        // Eğer hiçbir waypoint hedef yönünde değilse, en yakın olanı seç
        if (bestWaypoint == null || bestScore <= 0)
        {
            float closestDistance = float.MaxValue;
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint == null) continue;
                
                float distance = Vector3.Distance(currentPos, waypoint.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestWaypoint = waypoint;
                }
            }
        }

        return bestWaypoint;
    }

public void TeleportInstantly(Vector3 position)
{
    transform.position = position;
}

    public void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        isMoving = false;
        currentTargetSphereID = -1;
        Debug.Log("Hareket durduruldu!");
>>>>>>> Stashed changes
    }
}