using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Security.Cryptography.X509Certificates;
using CesiumForUnity;

public class LoadingManager : MonoBehaviour
{
    public bool autoRunOnAwake = true;
    public bool JsonAirports = false;
    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public Airport[] airports;
    public Vector3 newSize;

    public bool autoRunOnAwake = true;
    
    public TextAsset airplaneFile;
    private string jsonStr;
    public Airplane parsedData;
    private List<DataItem> airplanes;

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

        LoadAirplanes();
    }

    public void LoadAirplanes() 
    {
        if (airplaneFile != null)
        {
            //parse file into json str
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
        parsedData = JsonUtility.FromJson<Airplane>(jsonStr);
        airplanes = parsedData.data;
        
        foreach(var airplane in airplanes)
        {
            //Debug.Log("iataCode: " + airplane.aircraft.iataCode);
            GameObject location = GameObject.CreatePrimitive(PrimitiveType.Cube);
            location.AddComponent<CesiumGlobeAnchor>();
            location.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);
            location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = new Unity.Mathematics.double3(airplane.geography.longitude, airplane.geography.latitude, airplane.geography.altitude);

            location.name = airplane.flight.iataNumber;

            // Modify the size of the location GameObject
            location.transform.localScale = newSize;
        }
    }
}
