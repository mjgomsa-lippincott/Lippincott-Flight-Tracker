using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;
using Unity.Mathematics;
using TMPro.Examples;
using System;

public class LoadingManager : MonoBehaviour
{
    public bool autoRunOnAwake = true;
    bool loaded;
    private double currCamHeight = 1000;

    //airports
    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public List<Airport> airports;
    public bool JsonAirports = false;

    // airplanes
    public TextAsset airplaneFile;
    public List<DataItem> airplanes;
    private string jsonStr;
    public Airplane parsedData;


    void Awake()
    {
        if (autoRunOnAwake)
        {
            if (JsonAirports)
            {
                LoadJsonAirports();
            }
            else
            {
                LoadCsvAirports();
            }
            LoadAirplanes();
        }
    }

    public void LoadAirplanes()
    {
        if (airplaneFile != null)
        {
            jsonStr = airplaneFile.text;

        }
        else
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

        foreach (var airplane in airplanes)
        {
            //Setup Globe Anchor
            GameObject location = GameObject.CreatePrimitive(PrimitiveType.Cube);
            location.AddComponent<CesiumGlobeAnchor>();
            location.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);
            location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = new Unity.Mathematics.double3(airplane.geography.longitude, airplane.geography.latitude, airplane.geography.altitude);
            //location.GetComponent<CesiumGlobeAnchor>().detectTransformChanges = false;

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
        float airportScale = (float)(currCamHeight / 100 + 2000);
        float airplaneScale = airportScale / 5;

        foreach (var airport in airports)
        {
            if (airport.location != null)
            {
                airport.location.transform.localScale = new Vector3(airportScale, airportScale, airportScale);
            }
        }

        foreach (var airplane in airplanes)
        {
            var lonComponent = Mathf.Sin(airplane.geography.direction) * airplane.speed.horizontal / 600000 ; // 0.008 km per degree
            var latComponent = Mathf.Cos(airplane.geography.direction) * airplane.speed.horizontal / 600000 ;
            var prevLatLon = airplane.location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight;
            airplane.location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = prevLatLon + new double3(latComponent, lonComponent, 0);

            airplane.location.transform.localScale = new Vector3(airplaneScale, airplaneScale, airplaneScale);
        }
    }
}