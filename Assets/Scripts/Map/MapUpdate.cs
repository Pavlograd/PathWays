using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NewElement
{
    public string commentary; // Only for visual information won't be used in script
    public string methodToCall; // Will invoke the function with this name
    public float probability;
    public float minSizeMap; // Minimum size of the map for the object to spawn
    public float maxSizeMap; // Max size of the map for the object to spawn
}

public class MapUpdate : MonoBehaviour
{
    Map map;
    bool spawningRoads = false;
    Vector3Int direction;
    [SerializeField] Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Map>();

        InvokeRepeating("SpawnNewElements", 0.0f, GameManager.instance.playerData.timBtnNwElmts);
    }
    void SpawnNewElements()
    {
        foreach (NewElement element in Map.instance.data.newElements)
        {
            if (GameManager.instance.size >= element.minSizeMap && GameManager.instance.size <= element.maxSizeMap && Random.Range(0, 100.0f) <= element.probability)
            {
                Invoke(element.methodToCall, 0.0f);
            }
        }
    }

    void SpawnApartment()
    {
        SpawnNewElement(ElementType.Apartment);
    }

    void SpawnBuilding()
    {
        SpawnNewElement(ElementType.Building);
    }

    void SpawnRoads()
    {
        if (spawningRoads) return; // Prevent spawning multiple roads at the same time (can be removed later)

        spawningRoads = true;

        StartCoroutine(CoroutineRoads());
    }

    void SpawnParking()
    {
        Vector3Int position = GetEmptyPositionInMap();

        if (isNone(position) && isNone(position + Vector3Int.forward) && isNone(position + Vector3Int.right) && isNone(position + Vector3Int.forward + Vector3Int.right))
        { // Place for parking
            if (FreeForRoad(position + (Vector3Int.forward * 2)) && FreeForRoad(position + (Vector3Int.forward * 2) + Vector3Int.right))
            { // Place for path between parking and apart or building
                map.TryPlacingElement(ElementType.Parking, position);
            }
        }
    }

    bool isNone(Vector3Int position)
    {
        return map.GetElementTypeAtPosition(position) == ElementType.None;
    }

    IEnumerator CoroutineRoads()
    {
        int sizeRoad = Random.Range(3, (int)GameManager.instance.size);
        Vector2Int extremities = map.GetExtremities();
        Vector3Int startPosition = RandomPositionInMap(extremities);
        direction = GetRandomDirection();

        for (int i = 0; i < sizeRoad; i++)
        {
            Vector3Int position = startPosition + (direction * i);

            if (map.GetElementTypeAtPosition(position) == ElementType.None && FreeForRoad(position + Vector3Int.forward))
            {
                map.TryPlacingElement(ElementType.Road, position);
            }
            else
            {
                spawningRoads = false;

                break;
            }
            yield return new WaitForSeconds(5.0f);
        }
        spawningRoads = false;
    }

    bool FreeForRoad(Vector3Int position)
    {
        ElementType type = map.GetElementTypeAtPosition(position);

        return type != ElementType.Building && type != ElementType.Apartment;
    }

    public float GetRotationY()
    {
        return (direction == Vector3Int.left || direction == Vector3Int.right) ? 90.0f : 0.0f;
    }

    Vector3Int GetRandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return Vector3Int.left;
            case 1:
                return Vector3Int.forward;
            case 2:
                return Vector3Int.right;
            case 3:
                return Vector3Int.back;
        }

        return Vector3Int.zero;
    }

    void SpawnNewElement(ElementType type)
    {
        Vector3Int position = GetEmptyPositionInMap();

        if (map.GetElementTypeAtPosition(position) == ElementType.None && FreeForPath(position + Vector3Int.back) && FreeForPath(position + Vector3Int.forward))
        {
            map.TryPlacingElement(type, position); // TryPlacingElement will check if position is empty
        }

        inventory.GiveRandomObjects(); // Give random objects in inventory to prevent player to be able to do nothing
    }

    Vector3Int GetEmptyPositionInMap()
    {
        int tries = 0;
        Vector2Int extremities = map.GetExtremities();
        Vector3Int position = RandomPositionInMap(extremities);

        while (map.GetElementTypeAtPosition(position) != ElementType.None && map.GetElementTypeAtPosition(position) != ElementType.None && tries < 100)
        {
            position = RandomPositionInMap(extremities);
            tries++;
        }

        return position;
    }

    Vector3Int RandomPositionInMap(Vector2Int extremities)
    {
        return new Vector3Int(Random.Range(extremities.x * -1 + 1, extremities.x), 0, Random.Range(extremities.y * -1 + 2, extremities.y + 1));
    }

    bool FreeForPath(Vector3Int position)
    {
        ElementType type = map.GetElementTypeAtPosition(position);

        return type == ElementType.None || type == ElementType.GrassPath || type == ElementType.RoadCross;
    }
}
