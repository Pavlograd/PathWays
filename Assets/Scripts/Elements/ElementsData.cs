using UnityEngine;

[CreateAssetMenu(fileName = "ElementsData", menuName = "ElementsData/ElementsData")]
public class ElementsData : ScriptableObject
{
    public GameObject[] grassPaths; // all prefab grasspaths
    public GameObject apartment;
    public GameObject building;
    public GameObject passant;
    public GameObject road;
    public GameObject parking;
    public GameObject roadCross;
    public NewElement[] newElements;
    public int mapSize = 100;
}
