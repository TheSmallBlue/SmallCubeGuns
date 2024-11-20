using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CubeGuns.BehaviourTree;
using CubeGuns.Pathfinding;
using CubeGuns.Sight;
using UnityEngine;

public class PathfindingLeaf : ILeaf
{
    MonoBehaviour source;
    CreatureMovement movement;
    NodeGraph graph;

    Func<Vector3> from, to;

    List<NodeComponent> path = new();

    int pathIndex = 0;
    NodeComponent currentNode => path[pathIndex];

    public PathfindingLeaf(MonoBehaviour source, CreatureMovement movement, NodeGraph graph, Func<Vector3> from, Func<Vector3> to)
    {
        this.source = source;
        this.movement = movement;
        this.graph = graph;

        this.from = from;
        this.to = to;
    }

    void ResetPath()
    {
        path = new();
        pathIndex = 0;
    }

    bool ran = false;

    public NodeStatus Process()
    {
        if(!ran)
        {
            NodeComponent point1 = graph.GetClosestNodeTo(from());
            NodeComponent point2 = graph.GetClosestNodeTo(to());

            path = Pathfinding.ThetaStar(point1, point2, 1 << 0).ToList();

            ran = true;
        }

        Debug.Log(path.Count);

        return NodeStatus.RUNNING;
    }
}
