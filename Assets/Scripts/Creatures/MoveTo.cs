namespace CubeGuns.BehaviourTree
{
    using System;
    using System.Collections.Generic;
    using CubeGuns.Pathfinding;
    using CubeGuns.Sight;
    using UnityEngine;
    

    public class MoveTo : ILeaf
    {
        AIMovement movement;
        NodeGraph graph;
        Func<Vector3> source, destination;
        Func<bool> interruption;

        Queue<NodeComponent> path = new();
        PathStatus pathStatus = PathStatus.None;

        public MoveTo(AIMovement movement, NodeGraph graph, Func<Vector3> from, Func<Vector3> to, Func<bool> interruption = null)
        {
            this.movement = movement;
            this.graph = graph;

            source = from;
            destination = to;

            this.interruption = interruption;
        }

        public NodeStatus Process()
        {
            if(interruption != null && interruption())
                return NodeStatus.FAILED;

            switch (pathStatus)
            {
                case PathStatus.None:
                    // A path hasn't been made! Let's make one.
                    Pathfind(source(), destination());
                    return NodeStatus.RUNNING;

                case PathStatus.Running:
                    // We're still calculating the path...
                    return NodeStatus.RUNNING;

                case PathStatus.Succeeded:
                    // We found a path to our destination! Now we just need to move through it...

                    if(MoveThroughPath(path))
                    {
                        // We've reached the end of the path!
                        pathStatus = PathStatus.None;
                        return NodeStatus.SUCCEEDED;
                    }

                    return NodeStatus.RUNNING;

                default:
                    // We couldn't find a path there...
                    return NodeStatus.FAILED;
            }
        }

        bool MoveThroughPath(Queue<NodeComponent> path)
        {
            Vector3 target = CanSee(destination()) ? destination() : path.Peek().GetPosition();
            Vector3 dirToTarget = (target - movement.transform.position).CollapseAxis(VectorAxis.Y);
            Vector3 movementForward = dirToTarget + movement.WallAvoidance();

            movement.transform.forward = movementForward.normalized;
            movement.HorizontalMovement(movementForward.normalized, movement.WalkingMovement);

            if(dirToTarget.magnitude < 1.5f)
            {
                if(target != path.Peek().GetPosition())
                    return true;
                
                path.Dequeue();

                if(path.Count == 0)
                    return true;
            }

            return false;
        }

        async void Pathfind(Vector3 point1, Vector3 point2)
        {
            pathStatus = PathStatus.Running;

            if (CanSee(point2))
            {
                // We can go straight from the first point to the second, so there's no need to pathfind!
                pathStatus = PathStatus.Succeeded;
            }

            try 
            {
                var listPath = await Pathfinding.ThetaStar(graph.GetClosestNodeTo(point1), graph.GetClosestNodeTo(point2), 1 << 0);
                path = new Queue<NodeComponent>(listPath);
                pathStatus = PathStatus.Succeeded;
            } catch
            {
                pathStatus = PathStatus.Failed;
            }
        }

        bool CanSee(Vector3 point) => SightHelpers.InLineOfSight(movement.transform.position, point, 1 << 0);

        enum PathStatus
        {
            None,
            Running,
            Succeeded,
            Failed
        }
    }
}


