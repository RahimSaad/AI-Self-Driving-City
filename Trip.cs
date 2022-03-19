using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Trip
{
    // list of string with nodes names that describe the entire car route from start to destination
    public List<string> route;
    public Trip(List<string> route)
    {
        this.route = route;
    }
}
