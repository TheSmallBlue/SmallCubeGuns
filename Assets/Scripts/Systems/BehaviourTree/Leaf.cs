using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeGuns.BehaviourTree
{
    /// <summary>
    /// Interface that every leaf must implement.
    /// </summary>
    public interface ILeaf
    {
        public NodeStatus Process();
        public void Reset() { }
    }

    /// <summary>
    /// A node with no child nodes.
    /// </summary>
    public class Leaf : Node
    {
        // Here, instead of making classes inherit from node, we make them implement the interface ILeaf, thus implementing strategy.
        // Why? We simply don't want to carry the baggage that comes with inheriting from a full on node.
        // Only things that route logic should inherit from nodes, stuff like selectors and sequences.
        public ILeaf strategy { get; private set; }

        public Leaf(ILeaf leaf)
        {
            strategy = leaf;
        }

        public override NodeStatus Process() => strategy.Process();
        public override void Reset() => strategy.Reset();
    }

    /// <summary>
    /// A leaf through which you can add a conditional function through a delegate.
    /// <para> If the function returns true, it succeeds. Otherwise, it fails. </para>
    /// </summary>
    public class ConditionalLeaf : ILeaf
    {
        Func<bool> condition;

        public ConditionalLeaf(Func<bool> condition)
        {
            this.condition = condition;
        }

        public virtual NodeStatus Process() => condition() ? NodeStatus.SUCCEEDED : NodeStatus.FAILED;
    }

    /// <summary>
    /// A leaf through which you can add any function through a delegate. Always succeeds.
    /// </summary>
    public class ActionLeaf : ILeaf
    {
        public Action action { get; private set; }

        public ActionLeaf(Action action)
        {
            this.action = action;
        }

        public virtual NodeStatus Process()
        {
            action();
            return NodeStatus.SUCCEEDED;
        }
    }

    /// <summary>
    /// A leaf that lets you wait an X amount of seconds.
    /// </summary>
    public class DelayLeaf : ILeaf
    {
        float length;
        float passedTime;

        Func<bool> interruption;

        public DelayLeaf(float length, Func<bool> interruption = null)
        {
            this.length = length;
            this.interruption = interruption;
        }

        public NodeStatus Process()
        {
            passedTime += Time.deltaTime;

            if(interruption != null && interruption())
                return NodeStatus.FAILED;

            if(passedTime > length)
                return NodeStatus.SUCCEEDED;
            
            return NodeStatus.RUNNING;
        }

        public void Reset()
        {
            passedTime = 0;
        }
    }
}

