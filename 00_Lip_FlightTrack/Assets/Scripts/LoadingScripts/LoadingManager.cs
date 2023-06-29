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
    bool loaded;
    
    private double currCamHeight = 1000;

    //airports

    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public Airport[] airports;
    public bool JsonAirports = false;

    // airplanes
    public TextAsset airplaneFile;
    private List<DataItem> airplanes;
    private string jsonStr;
    public Airplane parsedData;


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
            LoadAirplanes();
        }
    }

    public void LoadAirplanes() {
        if (airplaneFile != null)
        {
            jsonStr = airplaneFile.text;
   
        } else
        {
            Debug.Log("Is Null");
        }
    }

    public void LoadJsonAirports()
    {
        Debug.Log("loading Json Airports");
        if (airportsFile != null)
        {
             AirportReader airportReader = new AirportReader();
            airports = airportReader.ReadAirports(airportsFile);
        }
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
        parsedData = JsonUtility.FromJson<Airplane>(jsonStr);
        airplanes = parsedData.data;
        
        foreach(var airplane in airplanes)
        {
            //Setup Globe Anchor
            GameObject location = GameObject.CreatePrimitive(PrimitiveType.Cube);
            location.AddComponent<CesiumGlobeAnchor>();
            location.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);
            location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = new Unity.Mathematics.double3(airplane.geography.longitude, airplane.geography.latitude, airplane.geography.altitude);
            
            //Adjust Sizing of Globe
            location.name = airplane.flight.iataNumber;
            airplane.location = location;

        }
        foreach (var airport in airports)
        {
            //Setup Globe Anchor
            GameObject location = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            location.AddComponent<CesiumGlobeAnchor>();
            location.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);
            location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = new Unity.Mathematics.double3(airport.longitude, airport.latitude, 0);

            //Adjust Sizing of Globe
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

        foreach ( var airplane in airplanes)
        {
            airplane.location.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
