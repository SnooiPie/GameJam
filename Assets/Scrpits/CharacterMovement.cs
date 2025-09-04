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
    }
}