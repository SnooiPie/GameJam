using UnityEngine;
using System.Collections.Generic;

public class WaypointNode : MonoBehaviour
{
    public List<WaypointNode> nextNodes = new List<WaypointNode>();
    public SphereInteraction sphere; // opsiyonel, eğer waypoint sphere ile ilişkiliyse
}
