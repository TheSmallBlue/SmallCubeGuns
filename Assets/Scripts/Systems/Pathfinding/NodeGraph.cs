using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CubeGuns.Pathfinding;
using CubeGuns.Sight;
using UnityEngine;

public class NodeGraph : MonoBehaviour
{
    public List<NodeComponent> Nodes { get; private set; }

    public event Action<List<NodeComponent>> OnRecalculate = delegate { };

    private void Awake() 
    {
        SetupChildNodes();
    }

    void SetupChildNodes()
    {
        Nodes = GetComponentsInChildren<NodeComponent>().Select(x => x.SetUpParent(this)).ToList();

        OnRecalculate(Nodes);
    }

    public NodeComponent GetClosestNodeTo(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        NodeComponent closestNode = null;

        if(Nodes == null) SetupChildNodes();

        for (int i = 0; i < Nodes.Count; i++)
        {
            if(!SightHelpers.InLineOfSight(position, Nodes[i].transform.position, 1 << 0)) continue;

            float newDistance = Vector3.Distance(position, Nodes[i].transform.position);

            if(newDistance < closestDistance)
            {
                closestDistance = newDistance;
                closestNode = Nodes[i];
            }
        }

        return closestNode;
    }

    public void Recalculate()
    {
        OnRecalculate(Nodes);
    }
}
