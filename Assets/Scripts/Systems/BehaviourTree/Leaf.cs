using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeGuns.BehaviourTree
{
    /// <summary>
    /// Interface that every leaf must implement for it to work as a leaf.
    /// </summary>
    public interface ILeaf
    {
        public NodeStatus Process();
        public void Reset() { }
    }

    /// <summary>
    /// A node that works with the given class, as long as it implements ILeaf
    /// </summary>
    public class Leaf : Node
    {
        // We implement strategy mainly simplify the builders of each class.
        // If we made each node a class of Node, then every builder would need to implement node's parameters, lest it causes nasty issues
        // With this, each class can implement any paramaters into their builders!
        public ILeaf strategy { get; private set; }

        public Leaf(ILeaf leaf, params Node[] children) : base(children) 
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

}

