using UnityEngine;
using System.Collections.Generic;

public class CharacterGraphMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    private WaypointNode currentNode;
    private WaypointNode targetNode;
    private HashSet<SphereInteraction> visitedSpheres = new HashSet<SphereInteraction>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (targetNode == null) return;

        Vector3 targetPos = new Vector3(targetNode.transform.position.x, rb.position.y, targetNode.transform.position.z);
        Vector3 move = (targetPos - rb.position).normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        if (Vector3.Distance(rb.position, targetPos) < 0.05f)
        {
            currentNode = targetNode;
            HandleSphere();
            ChooseNextNode();
        }
    }

    void HandleSphere()
    {
        if (currentNode.sphere != null && !visitedSpheres.Contains(currentNode.sphere))
        {
            currentNode.sphere.TriggerAction();
            visitedSpheres.Add(currentNode.sphere);
        }
    }

    void ChooseNextNode()
    {
        List<WaypointNode> available = new List<WaypointNode>();
        foreach (var node in currentNode.nextNodes)
        {
            if (node.sphere == null || !visitedSpheres.Contains(node.sphere))
                available.Add(node);
        }

        if (available.Count == 0) return; // no more paths
        targetNode = available[Random.Range(0, available.Count)];
    }

    public void StartAtNode(WaypointNode startNode)
    {
        currentNode = startNode;
        ChooseNextNode();
    }
}
