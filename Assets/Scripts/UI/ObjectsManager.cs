using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour
{
    List<ButtonObject> objects = new List<ButtonObject>();
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject prefabObject;
    int offset = 200;

    public void UpdateFromInventory()
    {
        InventoryObject[] inventoryObjects = inventory.GetInventoryObjects();

        int xSize = ((inventoryObjects.Length - 1) * -offset) / 2; // Size needed for the farest button on the left "/2" is for left and right

        for (int i = 0; i < objects.Count; i++) // Clear list of buttons
        {
            Destroy(objects[i].gameObject);
        }
        objects.Clear();

        for (int i = 0; i < inventoryObjects.Length; i++)
        {
            GameObject newObject = Instantiate(prefabObject, Vector3.zero, Quaternion.identity, this.transform);

            newObject.transform.localPosition = new Vector3(xSize + (offset * i), 0.0f, 0.0f);

            ButtonObject buttonObject = newObject.GetComponent<ButtonObject>();

            buttonObject.Init(inventoryObjects[i], this);

            objects.Add(buttonObject);
        }

        UpdateSelectedObject(objects[inventory.GetIndexObjectSelected()]);
    }

    public void UpdateSelectedObject(ButtonObject selectedObject)
    {
        foreach (ButtonObject item in objects)
        {
            item.SetActiveImage(item == selectedObject);
        }

        inventory.SetSelectedElementType(selectedObject.type);
    }
}
