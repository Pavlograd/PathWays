using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public enum ApartmentType
{
    Shopping,
    Business,
    Nature,
    None,
}

public class Apartment : Element
{
    ApartmentType apartmentType;
    Vector3Int tile;
    List<List<Vector3Int>> nodes;
    [SerializeField] GameObject exclamationMark;

    public void Init(ApartmentType type)
    {
        SetApartmentType(type);
        GetComponent<SpawnApartment>().Init(type);
        FindRoad();
        InvokeRepeating("SpawnPassant", 0.0f, GameManager.instance.playerData.timBtnNwPssnt);
    }

    void SpawnPassant()
    {
        if (nodes.Count == 0)
        { // No path join the apartment and a building of the same type
            exclamationMark.SetActive(true);
            return;
        }

        exclamationMark.SetActive(false);

        Vector3Int frontTile = Vector3Int.FloorToInt(transform.position) + Vector3Int.back;

        if (IsWalkable(frontTile))
        {
            GameObject newObject = Instantiate(Map.instance.data.passant, transform.position, Quaternion.identity);

            Passant passant = newObject.GetComponent<Passant>();

            passant.Init(apartmentType, nodes[Random.Range(0, nodes.Count)]); // Passant will follow one of the path
        }
    }

    public void FindRoad()
    {
        nodes = new List<List<Vector3Int>>();

        Vector3Int position = Vector3Int.FloorToInt(transform.position);

        Vector3Int frontTile = Vector3Int.FloorToInt(transform.position) + Vector3Int.back;

        if (IsWalkable(frontTile))
        {
            List<Vector3Int> node = new List<Vector3Int>();
            node.Add(frontTile);

            ContinueNode(node);
        }
    }

    bool IsWalkable(Vector3Int position)
    {
        ElementType typeTarget = Map.instance.GetElementTypeAtPosition(position);
        return typeTarget == ElementType.GrassPath || typeTarget == ElementType.RoadCross;
    }

    void ContinueNode(List<Vector3Int> node) // Try to find a path in every Direction
    {
        TryCreateNode(node.Last() + Vector3Int.left, new List<Vector3Int>(node), false);
        TryCreateNode(node.Last() + Vector3Int.right, new List<Vector3Int>(node), false);
        TryCreateNode(node.Last() + Vector3Int.forward, new List<Vector3Int>(node), true); // Check if a building is forward
        TryCreateNode(node.Last() + Vector3Int.back, new List<Vector3Int>(node), false);
    }

    void TryCreateNode(Vector3Int targetPosition, List<Vector3Int> node, bool checkBuilding)
    {
        if (IsWalkable(targetPosition) && !node.Contains(targetPosition)) // Prevent road's loop or reversing
        {
            node.Add(targetPosition);
            ContinueNode(node);
        }
        else if (checkBuilding && Map.instance.GetElementTypeAtPosition(targetPosition) == ElementType.Building)
        {
            Building building = Map.instance.GetElementAtPosition(targetPosition) as Building;

            if (building.GetApartmentType() == apartmentType)
            {
                // End of node

                node.Add(targetPosition);

                AddNewNode(node);
            }
        }
    }

    void AddNewNode(List<Vector3Int> newNode) // Multiple targets is possible but only one path to a target
    {
        foreach (List<Vector3Int> node in nodes)
        {
            if (node.Contains(newNode.Last())) // new Node has same goal as another
            {
                if (node.Count >= newNode.Count)
                {
                    nodes.Remove(node);
                    nodes.Add(newNode);
                }
                return; // Useless to continue a double node has been found
            }
        }

        nodes.Add(newNode); // No double has been found
    }

    public ApartmentType GetApartmentType()
    {
        return apartmentType;
    }

    public void SetApartmentType(ApartmentType _apartmentType)
    {
        apartmentType = _apartmentType;
    }
}
