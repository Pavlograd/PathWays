using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryObject
{
    public ElementType type;
    public string name;
    public int number;
    public int minSize; // Minimum size of the map for the object to be available
    public int minRandom;
    public int maxRandom;
}


public class Inventory : MonoBehaviour
{
    public InventoryObject[] objects;
    [SerializeField] ObjectsManager objectsManager; // Link with UI
    int indexObjectSelected;

    // Start is called before the first frame update
    void Start()
    {
        indexObjectSelected = 0;
        objects = (InventoryObject[])GameManager.instance.playerData.objects.Clone();
        objectsManager.UpdateFromInventory();
    }

    public int GetIndexObjectSelected()
    {
        return indexObjectSelected;
    }

    public InventoryObject[] GetInventoryObjects()
    {
        return objects;
    }

    public ElementType GetSelectedElementType()
    {
        return objects[indexObjectSelected].type;
    }

    public InventoryObject GetSelectedInventoryObject()
    {
        return objects[indexObjectSelected];
    }

    public void UseObject()
    {
        objects[indexObjectSelected].number--;
        objectsManager.UpdateFromInventory(); // Update UI
    }

    public void SetSelectedElementType(ElementType type)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].type == type)
            {
                indexObjectSelected = i;
                return;
            }
        }
    }

    public void GiveRandomObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].number += Random.Range(objects[i].minRandom, objects[i].maxRandom);
        }
    }
}
