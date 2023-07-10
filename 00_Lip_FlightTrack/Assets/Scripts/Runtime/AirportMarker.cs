using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;

public class CityMarker : MonoBehaviour
{
    List<Airport> airports;
    GameObject[] markers;
    void Start()
    {
        airports = gameObject.GetComponent<LoadingManager>().airports;
        foreach (Airport airport in airports)
        {
            //Debug.Log(airport.icao);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
