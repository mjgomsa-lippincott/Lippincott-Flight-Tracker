using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LoadingManager : MonoBehaviour
{
    public bool autoRunOnAwake = true;
    public TextAsset airportsFile;
    Airport[] airports;

    bool loaded;

    void Awake()
    {
        if (autoRunOnAwake)
        {
            Load();
        }
    }

    public void Load()
    {
        Debug.Log("load");
        if (airportsFile != null)
        {
            AirportReader airportReader = new AirportReader();
            airports = airportReader.ReadAirports(airportsFile);
        } else
        {
            Debug.Log("Is Null");
        }
    }

    private void Start()
    {
        foreach ( var airport in airports)
        {
            Debug.Log(airport.name);
        }
    }
}
