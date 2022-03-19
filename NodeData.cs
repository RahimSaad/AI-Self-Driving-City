using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeData
{
    public string node;
    public List<edge> neighbours;
    public NodeData(string node, List<edge> neighbours)
    {
        this.node = node;
        this.neighbours = neighbours;
    }
}