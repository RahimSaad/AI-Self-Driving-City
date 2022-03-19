using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AdjacencyModel
{
    public int count;
    public List<NodeData> graph;
    public AdjacencyModel(List<NodeData> graph)
    {
        this.graph = graph;
        this.count = graph.Count;
    }
}
