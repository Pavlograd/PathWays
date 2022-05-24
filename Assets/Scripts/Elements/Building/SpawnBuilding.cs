using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    public void Init(ApartmentType type) // Will be called when instantiated by the spawner
    {
        Instantiate(prefabs[(int)type], transform.position, Quaternion.identity, this.transform);

        Destroy(this);
    }
}
