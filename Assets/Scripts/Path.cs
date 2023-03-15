using System;
using UnityEngine;

[System.Serializable]
public class LinePath
{
    public SpawnPoint startPoint;
    public SpawnPoint endPoint;
    public float variationOffset = 1.25f;

    // get a random start point and corresponding end point
    public Tuple<SpawnPoint, SpawnPoint> GetStartAndEndPoint()
    {
        return UnityEngine.Random.Range(0, 2) == 0 ? Tuple.Create(startPoint, endPoint) : Tuple.Create(endPoint, startPoint);
    }

    public bool IsValid()
    {
        return startPoint.IsValid() && endPoint.IsValid();
    }
}

[System.Serializable]
public class SpawnPoint
{
    // the position of the point
    public GameObject point;

    // the direction the point is supposed to be facing
    public Vector3 startDirection;

    public bool IsValid()
    {
        return point != null;
    }
}