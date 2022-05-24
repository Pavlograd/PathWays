using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnApartment : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] GameObject[] icons;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(ApartmentType type) // Will be called when instantiated by the spawner
    {
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material = materials[(int)type];
        }

        icons[(int)type].SetActive(true);

        for (int i = 0; i < icons.Length; i++)
        {
            if (i != (int)type) Destroy(icons[i]);
        }

        Destroy(this);
    }
}
