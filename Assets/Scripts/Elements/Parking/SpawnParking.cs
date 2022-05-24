using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParking : MonoBehaviour
{
    [SerializeField] GameObject[] cars;
    [SerializeField] GameObject[] carsPos;
    [SerializeField] float probability;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject carPos in carsPos) // Spawn a random car in parking's places with a proba
        {
            if (Random.Range(0, 100.0f) <= probability)
            {
                Instantiate(cars[Random.Range(0, cars.Length)], carPos.transform.position, carPos.transform.rotation, carPos.transform);
            }
        }
    }
}
