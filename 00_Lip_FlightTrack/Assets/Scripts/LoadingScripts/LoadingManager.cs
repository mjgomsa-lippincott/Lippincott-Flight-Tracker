using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;
using Unity.Mathematics;
using TMPro.Examples;
using System;
using System.Net;
using System.IO;
using System;
using System.Collections;
using static UnityEditor.FilePathAttribute;

public class LoadingManager : MonoBehaviour
{
    // global
    public bool autoRunOnAwake = true;
    private double currCamHeight = 1000;
    public bool onlySouthwest = false;

    //airports
    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public List<Airport> airports;
    public bool JsonAirports = false;

    // airplanes
    private List<DataItem> airplanes;
    private string jsonStr;
    private Airplane parsedData;

    public Material mat;

    //API
    private string apiKey;
    private string apiURL;
    private const float RequestCooldown = 1.5f * 60 * 60; //Cooldown time in seconds, 1.5 hours
    private const string LastRequestKey = "LastAPIRequest";


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
            //LoadJsonAirplanes();
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
        StartCoroutine(GetJSON());
        parsedData = JsonUtility.FromJson<Airplane>(jsonStr);
        airplanes = parsedData.data;

        //Optionally Only Filtering Southwest Planes
        if (onlySouthwest)
        {
            List<DataItem> filteredAirplanes = new List<DataItem>();
            foreach (var airplane in airplanes)
            {
                if (airplane.airline.iataCode == "WN")
                {
                    filteredAirplanes.Add(airplane);
                }
            }
            airplanes = filteredAirplanes;
        }

        foreach (var airplane in airplanes)
        {
            //Setup Globe Anchor
            GameObject location = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (airplane.airline.iataCode == "WN") //Identifying South West Airplanes
            {
                location.GetComponent<Renderer>().material = mat;
            }

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

    IEnumerator GetJSON()
    {
        DateTime lastRequestTime = GetLastRequestTime();
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSinceLastRequest = currentTime - lastRequestTime;

        Debug.Log(lastRequestTime.ToString());

        if (timeSinceLastRequest.TotalSeconds >= RequestCooldown)
        {
            Debug.Log("Cooldown time has passed, make a new API request");
            // Cooldown time has passed, make a new API request
            jsonStr = RequestDataFromAPI();

        }
        else
        {
            Debug.Log("Cooldown time has NOT passed, send old request JSON stored string");
            // Cooldown time has not passed, send old request JSON stored string
            jsonStr = PlayerPrefs.GetString("APIResponse");
        }
        yield return jsonStr;
    }

    private string RequestDataFromAPI()
    {
        string apiKeyFilePath = Path.Combine(Application.dataPath, "APIKey.txt");
        apiKey = File.ReadAllText(apiKeyFilePath);
        apiURL = "https://app.goflightlabs.com/flights?access_key=" + apiKey;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());

        string toReturn = sr.ReadToEnd(); //  Actual API request code
        PlayerPrefs.SetString(LastRequestKey, DateTime.Now.ToString());
        PlayerPrefs.SetString("APIResponse", toReturn);

        return toReturn;
    }

    private DateTime GetLastRequestTime()
    {
        string lastRequestString = PlayerPrefs.GetString(LastRequestKey);
        if (!string.IsNullOrEmpty(lastRequestString))
        {
            DateTime lastRequestTime;
            if (DateTime.TryParse(lastRequestString, out lastRequestTime))
            {
                return lastRequestTime;
            }
        }
        return DateTime.MinValue;
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

            //Making Southwest Planes Larger
            if (airplane.airline.iataCode == "WN")
            {
                airplane.location.transform.localScale = new Vector3(airplaneScale * 2, airplaneScale * 2, airplaneScale * 2);
            } else
            {
                airplane.location.transform.localScale = new Vector3(airplaneScale, airplaneScale, airplaneScale);
            }
        }
    }
}