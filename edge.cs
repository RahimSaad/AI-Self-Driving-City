using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class edge
{
    public string node;
    public float weight;
    public edge(string node, float weight)
    {
        this.node = node;
        this.weight = weight;
    }
}