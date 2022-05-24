using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance { get; private set; }
    public ElementsData data;
    Element[][] mapElements; // All elements on the map
    int offset; // Used to put elements in mapElements depending of their worldPosition
    float rotationY = 0.0f;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] ApartmentsManager apartmentsManager;
    [SerializeField] MapUpdate mapUpdate;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitDoubleArray(data.mapSize); // Will init mapElements
        offset = mapElements[0].Length / 2;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool TryPlacingElement(ElementType element, Vector3Int position)
    {
        if (GetElementTypeAtPosition(position) == ElementType.None) // Check if place is free
        {
            switch (element)
            {
                case ElementType.GrassPath:
                    PlaceGrassPath(position);
                    break;
                case ElementType.Apartment:
                    PlaceApartment(position);
                    break;
                case ElementType.Building:
                    PlaceBuilding(position);
                    break;
                case ElementType.Road:
                    PlaceRoad(position);
                    break;
                case ElementType.Parking:
                    PlaceParking(position);
                    break;
                case ElementType.RoadCross: // Can't place roadcross on nothing
                    return false;
                default:
                    break;
            }

            // Refresh all elements around new element
            RefreshAround(position);
            return true;
        }
        else if (GetElementTypeAtPosition(position) == ElementType.Road && element == ElementType.RoadCross) // RoadCross placement is only on road
        {
            PlaceRoadCross(position);
            RefreshAround(position);
            return true;
        }

        return false;
    }

    void RefreshAround(Vector3Int position)
    {
        // Refresh all elements around new element
        RefreshElement(position + Vector3Int.left);
        RefreshElement(position + Vector3Int.right);
        RefreshElement(position + Vector3Int.forward);
        RefreshElement(position + Vector3Int.back);
    }

    GameObject PlaceElement(Vector3Int position, GameObject element)
    {
        GameObject newObject = Instantiate(element, position, Quaternion.identity);
        mapElements[position.z + offset][position.x + offset] = newObject.GetComponent<Element>();

        return newObject;
    }

    void RefreshElement(Vector3Int position)
    {
        ElementType currentType = GetElementTypeAtPosition(position);

        switch (currentType)
        {
            case ElementType.GrassPath:
                GrassPath grassPath = GetElementAtPosition(position) as GrassPath;

                if (grassPath.GetGrassPathType() != GetNeededGrassPathType(position) || grassPath.GetRotationY() != rotationY)
                {
                    DestroyImmediate(GetElementAtPosition(position).gameObject);
                    PlaceGrassPath(position);
                }
                break;
            default:
                break;
        }
    }

    void PlaceApartment(Vector3Int position)
    {
        rotationY = 0.0f;

        GameObject newObject = PlaceElement(position, data.apartment);

        Apartment apartment = GetElementAtPosition(position) as Apartment;

        apartment.Init((ApartmentType)Random.Range(0, (int)ApartmentType.None));

        apartmentsManager.addApartment(apartment);
        newObject.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
    }

    void PlaceRoad(Vector3Int position)
    {
        rotationY = mapUpdate.GetRotationY();

        GameObject newObject = PlaceElement(position, data.road);
        newObject.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
    }

    void PlaceRoadCross(Vector3Int position)
    {
        Element element = GetElementAtPosition(position);

        rotationY = element.transform.rotation.y == 0.0f ? 90.0f : 0.0f; // Set rotation of roadCross to the inverse of the current road

        DestroyImmediate(element.gameObject); // Destroy old road

        GameObject newObject = PlaceElement(position, data.roadCross);
        newObject.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
    }

    void PlaceParking(Vector3Int position)
    {
        rotationY = Random.Range(0, 2) == 0 ? 0.0f : 90.0f;

        GameObject newObject = PlaceElement(position, data.parking);

        Element elementObject = newObject.GetComponent<Element>();

        // Parking take 4 tiles and PlaceElement set only the first
        SetElementAtPosition(position + Vector3Int.back, elementObject);
        SetElementAtPosition(position + Vector3Int.right, elementObject);
        SetElementAtPosition(position + Vector3Int.back + Vector3Int.right, elementObject);

        newObject.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
    }

    void PlaceBuilding(Vector3Int position)
    {
        rotationY = 0.0f;

        GameObject newObject = PlaceElement(position, data.building);

        Building building = GetElementAtPosition(position) as Building;

        building.Init(apartmentsManager);
        newObject.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
    }

    void PlaceGrassPath(Vector3Int position)
    {
        rotationY = 0.0f;

        GrassPathType grassPathType = GetNeededGrassPathType(position);
        GameObject newObject = PlaceElement(position, data.grassPaths[(int)grassPathType]);

        GrassPath grassPath = GetElementAtPosition(position) as GrassPath;

        grassPath.SetGrassPathType(grassPathType);

        newObject.transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

        apartmentsManager.UpdatePathFinding(); // Update Path finding
    }

    GrassPathType GetNeededGrassPathType(Vector3Int position)
    {
        GrassPathType GrassPathType = GrassPathType.Straight; // Default value in switch

        bool left = IsWalkable(position + Vector3Int.left);
        bool right = IsWalkable(position + Vector3Int.right);
        bool up = IsPathable(position + Vector3Int.forward);
        bool down = IsWalkable(position + Vector3Int.back);

        switch (up, down, left, right)
        {
            case (true, true, true, true):
                GrassPathType = GrassPathType.Junction;
                break;
            case (true, false, true, true):
                GrassPathType = GrassPathType.TIntersections;
                //rotationY = -90.0f;
                break;
            case (false, true, true, true):
                GrassPathType = GrassPathType.TIntersections;
                rotationY = 180.0f;
                break;
            case (true, true, true, false):
                GrassPathType = GrassPathType.TIntersections;
                rotationY = -90.0f;
                break;
            case (true, true, false, true):
                GrassPathType = GrassPathType.TIntersections;
                rotationY = 90.0f;
                break;
            case (false, false, true, true):
                GrassPathType = GrassPathType.Straight;
                rotationY = 90.0f;
                break;
            case (true, false, true, false):
                GrassPathType = GrassPathType.Corner;
                break;
            case (true, false, false, true):
                GrassPathType = GrassPathType.Corner;
                rotationY = 90.0f;
                break;
            case (false, true, true, false):
                GrassPathType = GrassPathType.Corner;
                rotationY = -90.0f;
                break;
            case (false, true, false, true):
                GrassPathType = GrassPathType.Corner;
                rotationY = 180.0f;
                break;
            case (false, false, true, false):
                rotationY = 90.0f;
                break;
            case (false, false, false, true):
                rotationY = 90.0f;
                break;
            default:
                break;
        }

        return GrassPathType;
    }

    bool IsWalkable(Vector3Int position)
    {
        ElementType type = GetElementTypeAtPosition(position);
        return type == ElementType.GrassPath || type == ElementType.RoadCross;
    }

    bool IsPathable(Vector3Int position)
    {
        ElementType type = GetElementTypeAtPosition(position);

        return IsWalkable(position) || type == ElementType.Building || type == ElementType.Apartment;
    }


    //
    // ALL FUNCTION BELOW ARE FOR UI
    //


    public void SwitchSelectedType()
    {
        //selectedType = selectedType == ElementType.GrassPath ? ElementType.Building : ElementType.GrassPath;
    }


    //
    // ALL FUNCTION BELOW ARE GETTERS OR SETTERS
    //

    public void SetElementAtPosition(Vector3Int position, Element element)
    {
        mapElements[position.z + offset][position.x + offset] = element;
    }


    public Element GetElementAtPosition(Vector3Int position)
    {
        return mapElements[position.z + offset][position.x + offset];
    }

    public ElementType GetElementTypeAtPosition(Vector3Int position)
    {
        return GetElementAtPosition(position) != null ? GetElementAtPosition(position).type : ElementType.None;
    }

    void InitDoubleArray(int size)
    {
        mapElements = new Element[size][];

        for (int i = 0; i < size; i++)
        {
            mapElements[i] = new Element[size];
        }
    }

    public Vector2Int GetExtremities()
    {
        Vector2Int vector = new Vector2Int(0, 0);

        // It,s not the real extremeties, I remove 2 to prevent buildings in corners or complete bottom
        vector.x = (int)(GameManager.instance.size - 2);
        vector.y = (int)((GameManager.instance.size - 2) / 2 + 1);

        return vector;
    }
}
