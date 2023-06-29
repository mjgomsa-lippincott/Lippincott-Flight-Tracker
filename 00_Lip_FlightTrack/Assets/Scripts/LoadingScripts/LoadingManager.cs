using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using CesiumForUnity;
using static UnityEditor.FilePathAttribute;
using Unity.Mathematics;

public class LoadingManager : MonoBehaviour
{
    public bool autoRunOnAwake = true;
    public bool JsonAirports = false;
    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public Airport[] airports;

    bool loaded;
    private double currCamHeight = 1000;

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
        foreach (var airport in airports)
        {
            //Setup Globe Anchor
            GameObject location = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            location.AddComponent<CesiumGlobeAnchor>();
            location.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);
            location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = new Unity.Mathematics.double3(airport.longitude, airport.latitude, 0);

            //Adjust Sizing of Globe
            //location.GetComponent<SphereCollider>().radius = 10000;
            location.transform.localScale = new Vector3(10000, 10000, 10000);
            location.name = airport.name;
            airport.location = location;
        }
    }

    private void Update()
    {
        currCamHeight = GameObject.Find("CesiumGeoreference").GetComponent<CesiumGeoreference>().height;
        float scale = (float)(currCamHeight / 100 + 2000);

        foreach ( var airport in airports)
        {
            airport.location.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
