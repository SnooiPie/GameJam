using UnityEngine;
using UnityEngine.AI;

public class TeleportAgent : MonoBehaviour
{
    [Header("NavMesh Agent to teleport")]
    public NavMeshAgent agent;
    [Header("Target Sphere")]
    public SphereInteraction targetSphere; 

    public void TeleportToSphere()
    {
        if (agent == null || targetSphere == null) return;

        agent.Warp(targetSphere.transform.position);

        agent.transform.rotation = targetSphere.transform.rotation;

        Debug.Log($"Agent {agent.name} teleported to Sphere {targetSphere.id}");
    }
}
