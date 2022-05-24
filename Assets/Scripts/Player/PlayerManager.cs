using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    Plane plane; // Used to detect where user clicks
    [SerializeField] Map map;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Vector3.up, 0);
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3Int worldPosition;
            float distance;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out distance))
            {
                Vector3 position = ray.GetPoint(distance);
                worldPosition = Vector3Int.RoundToInt(position);
                InventoryObject inventoryObject = GetSelectedInventoryObject();

                if (inventoryObject.number > 0 && map.TryPlacingElement(inventoryObject.type, worldPosition))
                {
                    inventory.UseObject();
                }
            }
        }
        if (!EventSystem.current.IsPointerOverGameObject())
        { // Mouse not hover ui
            // Later put a way to show object you will place
        }
    }

    public InventoryObject GetSelectedInventoryObject()
    {
        return inventory.GetSelectedInventoryObject();
    }
}
