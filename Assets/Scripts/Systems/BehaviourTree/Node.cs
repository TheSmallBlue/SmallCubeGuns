using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeGuns.BehaviourTree
{
    /// <summary>
    /// A tree consisting of various nodes, each of them either succeeding or failing.
    /// </summary>
    public class BehaviourTree : Node
    {
        /// <summary>
        /// A tree consisting of various nodes, each of them either succeeding or failing.
        /// </summary>
        public BehaviourTree(params Node[] children) : base(children) { }

        public override NodeStatus Process()
        {
            while (currentChildIndex < ChildNodes.Count)
            {
                var currentChildStatus = currentChild.Process();
                if(currentChildStatus != NodeStatus.SUCCEEDED)
                {
                    return currentChildStatus;
                }

                currentChildIndex++;
            }

            return NodeStatus.SUCCEEDED;
        }
    }

    /// <summary>
    /// Executes its children in a sequence. If any of them fail, resets the sequence and starts over. If all of them succeed, succeeds.
    /// </summary>
    public class Sequence : Node
    {
        /// <summary>
        /// Executes its children in a sequence. If any of them fail, resets the sequence and starts over. If all of them succeed, succeeds.
        /// </summary>
        public Sequence(params Node[] children) : base(children) { }

        public override NodeStatus Process()
        {
            if(currentChildIndex < ChildNodes.Count)
            {
                switch (currentChild.Process())
                {
                    case NodeStatus.RUNNING:
                        return NodeStatus.RUNNING;
                    case NodeStatus.FAILED:
                        Reset();
                        return NodeStatus.FAILED;
                    default:
                        currentChildIndex++;
                        return currentChildIndex == ChildNodes.Count ? NodeStatus.SUCCEEDED : NodeStatus.RUNNING;
                }
            }

            Reset();
            return NodeStatus.SUCCEEDED;
        }
    }

    /// <summary>
    /// Succeeds when any of his children succeed. Fails if none of them do.
    /// </summary>
    public class Selector : Node
    {
        /// <summary>
        /// Succeeds when any of his children succeed. Fails if none of them do.
        /// </summary>
        public Selector(params Node[] children) : base(children) { }

        public override NodeStatus Process()
        {
            if(currentChildIndex < ChildNodes.Count)
            {
                switch (currentChild.Process())
                {
                    case NodeStatus.RUNNING:
                        return NodeStatus.RUNNING;
                    case NodeStatus.SUCCEEDED:
                        Reset();
                        return NodeStatus.SUCCEEDED;
                    default:
                        currentChildIndex++;
                        return NodeStatus.RUNNING;
                }
            }

            Reset();
            return NodeStatus.FAILED;
        }
    }

    /// <summary>
    /// Returns the opposite of the result of its first child.
    /// </summary>
    public class Not : Node
    {
        /// <summary>
        /// Returns the opposite of the result of its first child.
        /// </summary>
        public Not(params Node[] children) : base(children) { }

        public override NodeStatus Process()
        {
            switch (ChildNodes[0].Process())
            {
                case NodeStatus.RUNNING:
                    return NodeStatus.RUNNING;
                case NodeStatus.SUCCEEDED:
                    return NodeStatus.FAILED;
                default:
                    return NodeStatus.SUCCEEDED;
            }
        }
    }

    /// <summary>
    /// A behaviour tree node. Meant to be inhereted from to make custom nodes.
    /// </summary>
    public abstract class Node
    {
        public List<Node> ChildNodes {  get; private set; } = new List<Node>();
        protected int currentChildIndex;
        protected Node currentChild => ChildNodes[currentChildIndex];

        /// <summary>
        /// A behaviour tree node. Meant to be inhereted from to make custom nodes.
        /// </summary>
        public Node(params Node[] children)
        {
            if (children == null) return;

            ChildNodes.AddRange(children);
        }

        public void AddChild(Node node)
        {
            ChildNodes.Add(node);
        }

        public virtual void Reset()
        {
            currentChildIndex = 0;
            foreach (var child in ChildNodes)
            {
                child.Reset();
            }
        }

        public virtual NodeStatus Process() => ChildNodes[currentChildIndex].Process();
    }

    public enum NodeStatus
    {
        RUNNING,
        SUCCEEDED,
        FAILED
    }
}


