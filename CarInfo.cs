using System;
using UnityEngine;

[Serializable]
public class CarInfo
{
    public string carID;
    
    // x , y and z for the current CarPosition in the city
    // pos describes the position of the car between the current two points as a ration
    // pos = [(distance from start point to car)\(distance from start point to end point)]
    // startPoint ,endPoint are the current two nodes between them the car located
    public float x, y, z, pos;
    public string startPoint, endPoint;
    public CarInfo(string startPoint, string endPoint, Vector3 carPos)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.x = carPos.x;
        this.y = carPos.y;
        this.z = carPos.z;
        this.pos = getPos(startPoint, endPoint, carPos);
    }
    // calculate a ration that describe the position of car between two points
    public static float getPos(string startPoint, string endPoint, Vector3 carPos)
    {
        return Vector3.Distance(GameManager.instance.xNodes[startPoint].position, carPos) /
        Vector3.Distance(GameManager.instance.xNodes[startPoint].position, GameManager.instance.xNodes[endPoint].position);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

