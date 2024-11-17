using System;
using System.Collections;
using System.Collections.Generic;
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

    bool planningPath;

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
        planningPath = false;
    }

    public NodeStatus Process()
    {
        if(path.Count == 0 && !planningPath)
        {
            Debug.Log("Start pathfind");

            // We haven't made a path yet! Let's make one.
            source.StartCoroutine(Pathfinding.ThetaStar(graph.GetClosestNodeTo(from()), graph.GetClosestNodeTo(to()), 1 << 0, p => 
            {
                path = p;
                planningPath = false;
                Debug.Log("Path found!");
            }));

            planningPath = true;
            
            return NodeStatus.RUNNING;
        }

        if(path == null)
        {
            // We could not make a path there...
            ResetPath();
            return NodeStatus.FAILED;
        }

        // If planningPath hasnt been set to false it means we're still planning the path, so we just say we're running.
        if(planningPath) return NodeStatus.RUNNING;

        // We are currently traversing a path!
        bool canSeeTarget = SightHelpers.InLineOfSight(movement.transform.position, to(), 1 << 0);

        if (pathIndex < path.Count || canSeeTarget)
        {
            Vector3 target = canSeeTarget ? to() : currentNode.GetPosition();
            Vector3 dirToTarget = target - movement.transform.position;

            source.transform.forward = dirToTarget.CollapseAxis(VectorAxis.Y).normalized;

            movement.HorizontalMovement(dirToTarget.normalized, movement.WalkingMovement);

            if (Vector3.Distance(movement.transform.position.CollapseAxis(VectorAxis.Y), target.CollapseAxis(VectorAxis.Y)) < 1f)
            {
                if(canSeeTarget)
                {
                    ResetPath();
                    return NodeStatus.SUCCEEDED;
                } else
                    pathIndex++;
            }

            return NodeStatus.RUNNING;
        }

        // We have traversed every node in the path!
        ResetPath();
        return NodeStatus.SUCCEEDED;

        /*
        if(path == null) return NodeStatus.FAILED;

        bool canSeeTarget = SightHelpers.InLineOfSight(movement.transform.position, destination, 1 << 0);

        if (pathIndex < path.Count || canSeeTarget)
        {

            Vector3 target = canSeeTarget ? destination : currentNode.GetPosition();
            Vector3 dirToTarget = (target - movement.transform.position).normalized;

            movement.HorizontalMovement(dirToTarget, movement.WalkingMovement);

            if(Vector3.Distance(movement.transform.position, target) < 1f) pathIndex++;

            return NodeStatus.RUNNING;
        }

        return NodeStatus.SUCCEEDED;
        */
    }
}
