using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GrassPathType
{
    Corner,
    Junction,
    Straight,
    TIntersections,
    None,
}

public class GrassPath : Element
{
    GrassPathType grassType;

    public GrassPathType GetGrassPathType()
    {
        return grassType;
    }

    public void SetGrassPathType(GrassPathType _grassType)
    {
        grassType = _grassType;
    }

    public float GetRotationY()
    {
        return transform.rotation.y;
    }
}
