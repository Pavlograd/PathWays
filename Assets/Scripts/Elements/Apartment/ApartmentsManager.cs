using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentsManager : MonoBehaviour
{
    Dictionary<ApartmentType, int> apartmentsByTypes = new Dictionary<ApartmentType, int>(); // Number of apartments of one type
    List<Apartment> apartments = new List<Apartment>(); // All apartments on the map

    // Start is called before the first frame update
    void Start()
    {
        foreach (ApartmentType aptType in System.Enum.GetValues(typeof(ApartmentType))) // Dynamic creation of apartments by types in case enum change
        {
            apartmentsByTypes.Add(aptType, 0);
        }
    }

    public int GetApartmentsOfType(ApartmentType type)
    {
        return apartmentsByTypes[type];
    }

    public void UpdatePathFinding() // Update futurs roads but not old passants yet
    {
        foreach (Apartment apartment in apartments)
        {
            apartment.FindRoad();
        }
    }

    public int addApartment(Apartment apartment)
    {
        apartments.Add(apartment);

        apartmentsByTypes[apartment.GetApartmentType()]++;

        return GetApartmentsOfType(apartment.GetApartmentType());
    }

    public int removeApartment(Apartment apartment)
    {
        apartments.Remove(apartment);

        apartmentsByTypes[apartment.GetApartmentType()]--;

        return GetApartmentsOfType(apartment.GetApartmentType());
    }
}
