using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CubeGuns.Pathfinding;
using UnityEngine;

public class Guard : Creature
{
    [SerializeField] NodeGraph graph;

    [SerializeField] Vector3 goalPosition;

    List<NodeComponent> path = new();

    void GoTo(Vector3 position)
    {
        StartCoroutine(Pathfinding.ThetaStar(graph.GetClosestNodeTo(transform.position), graph.GetClosestNodeTo(position), 1 << 0, (o) => path = o.ToList()));
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GoTo(goalPosition);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawSphere(goalPosition, 1f);

        if(path.Count == 0) return;

        Gizmos.color = Color.blue;
        foreach (var node in path)
        {
            Gizmos.DrawWireSphere(node.transform.position, 1f);
        }
    }
}
