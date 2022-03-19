using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class moveSettings
{
    public float velocity, acceleration;
    public moveSettings(float velocity, float acceleration)
    {
        this.velocity = velocity;
        this.acceleration = acceleration;
    }
}