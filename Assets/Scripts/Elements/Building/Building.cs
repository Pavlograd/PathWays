using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Element
{
    ApartmentType apartmentType;
    float timer = 0.0f;
    [SerializeField] GameObject exclamationMark;

    public void Init(ApartmentsManager apartmentsManager)
    {
        apartmentType = ApartmentType.None;
        timer = 0.0f;

        while (apartmentsManager.GetApartmentsOfType(apartmentType) <= 0) // Only create a building type for available apartments to prevent stupid game over
        {
            apartmentType = (ApartmentType)Random.Range(0, (int)ApartmentType.None);
        }

        GetComponent<SpawnBuilding>().Init(apartmentType);
    }

    public ApartmentType GetApartmentType()
    {
        return apartmentType;
    }

    public void SetApartmentType(ApartmentType _apartmentType)
    {
        apartmentType = _apartmentType;
    }

    public void PassantArrived()
    {
        GameManager.instance.IncreaseScore();
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= GameManager.instance.playerData.timerBuilding)
        {
            exclamationMark.SetActive(true);

            // Lose
            GameManager.instance.GameOver();
        }
        else if (timer >= GameManager.instance.playerData.timerBuilding / 2.0f) // At half time show the exlamation mark
        {
            exclamationMark.SetActive(true);
        }
        else
        {
            exclamationMark.SetActive(false);
        }
    }
}
