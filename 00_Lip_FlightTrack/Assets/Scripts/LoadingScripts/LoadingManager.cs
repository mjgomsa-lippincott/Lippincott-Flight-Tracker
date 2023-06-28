using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LoadingManager : MonoBehaviour
{
    public bool autoRunOnAwake = true;
    public bool JsonAirports = false;
    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public Airport[] airports;

    bool loaded;

    void Awake()
    {
        if (autoRunOnAwake)
        {
            if (JsonAirports)
            {
                LoadJsonAirports();
            } else
            {
                LoadCsvAirports();
            }
        }
    }

    public void LoadJsonAirports()
    {
        Debug.Log("loading Json Airports");
        if (airportsFile != null)
        {
            AirportReader airportReader = new AirportReader();
            airports = airportReader.ReadAirports(airportsFile);
        } else
        {
            Debug.Log("Is Null");
        }
        Debug.Log("Finished Loading Json Airports");
    }

    public void LoadCsvAirports()
    {
        Debug.Log("loading CSV Airports");
        if (airportsCSVFile != null)
        {
            AirportCsvReader airportReader = new AirportCsvReader();
            airports = airportReader.ReadAirports(airportsCSVFile);
        }
    }

    private void Start()
    {
        foreach ( var airport in airports)
        {
            //Debug.Log(airport.latitude);
        }
    }
}
