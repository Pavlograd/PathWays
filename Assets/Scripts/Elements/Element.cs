using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ElementType // Later for prefabs
{
    None,
    GrassPath,
    Road,
    Building,
    Apartment,
    Parking,
    RoadCross,
}

public class Element : MonoBehaviour
{
    public ElementType type;
}
