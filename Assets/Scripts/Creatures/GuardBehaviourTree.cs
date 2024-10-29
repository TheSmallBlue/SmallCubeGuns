using CubeGuns.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Guard))]
public class GuardBehaviourTree : MonoBehaviour
{
    BehaviourTree tree;

    private void Awake()
    {
        SetUpTree();
    }

    void SetUpTree()
    {
        tree = new();
    }
}
