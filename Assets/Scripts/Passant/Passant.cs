using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Passant : MonoBehaviour
{
    ApartmentType type;
    List<Vector3Int> path;
    int indexPath = 0;
    [SerializeField] GameObject[] models;

    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, models.Length);

        for (int i = 0; i < models.Length; i++)
        {
            if (i != index) Destroy(models[i]);
            else models[i].SetActive(true);
        }
    }

    public void Init(ApartmentType _type, List<Vector3Int> _path)
    {
        path = _path;
        type = _type;

        transform.LookAt((Vector3)(path[indexPath]));

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, (Vector3)(path[indexPath]), Time.deltaTime * 0.5f);

        if (transform.position == (Vector3)(path[indexPath]))
        {
            indexPath++;

            if (indexPath == path.Count)
            {
                Building building = Map.instance.GetElementAtPosition(path.Last()) as Building;

                building.PassantArrived();

                // Arrived at destination
                DestroyImmediate(gameObject);
                DestroyImmediate(this);
            }
            else
            {
                transform.LookAt((Vector3)(path[indexPath]));
            }
        }
    }
}
