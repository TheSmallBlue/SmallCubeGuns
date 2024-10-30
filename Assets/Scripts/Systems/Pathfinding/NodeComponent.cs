using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CubeGuns.Pathfinding;
using CubeGuns.Sight;
using UnityEngine;
using System;

public class NodeComponent : MonoBehaviour, IPathfindingNode
{
    public NodeGraph Graph { get; private set; }
    public List<NodeComponent> Neighbours { get; private set; }

    [SerializeField] float neighbourCheckRadius = 8f;

    public Vector3 GetPosition() => transform.position;
    public List<IPathfindingNode> GetNeighbours() => Neighbours.Select(x => x as IPathfindingNode).ToList();

    public NodeComponent SetUpParent(NodeGraph graph)
    {
        Graph = graph;
        Graph.OnRecalculate += UpdateNeighbours;

        return this;
    }

    public void UpdateNeighbours(List<NodeComponent> otherNodes)
    {
        List<NodeComponent> neighbours = new();

        foreach (var node in otherNodes)
        {
            if(node == this) continue;
            if(Vector3.Distance(transform.position, node.transform.position) > neighbourCheckRadius) continue;
            if(!SightHelpers.InLineOfSight(transform.position, node.transform.position, 1 << 0)) continue;

            neighbours.Add(node);
        }

        Neighbours = new(neighbours);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, neighbourCheckRadius);
    }
}
