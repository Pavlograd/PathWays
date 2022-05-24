using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour
{
    public ElementType type;
    [SerializeField] GameObject activeImage;
    [SerializeField] Text nameObject;

    ObjectsManager objectsManager;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeSelectedObject);
    }

    public void Init(InventoryObject inventoryObject, ObjectsManager _objectsManager)
    {
        nameObject.text = inventoryObject.name + " X" + inventoryObject.number;
        type = inventoryObject.type;
        objectsManager = _objectsManager;
    }

    public void ChangeSelectedObject()
    {
        objectsManager.UpdateSelectedObject(this);
    }

    public void SetActiveImage(bool active)
    {
        activeImage.SetActive(active);
    }
}
