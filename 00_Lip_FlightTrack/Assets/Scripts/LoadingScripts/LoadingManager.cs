using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;
using System.Net;
using System.IO;

public class LoadingManager : MonoBehaviour
{
    // global
    public bool autoRunOnAwake = true;
    private double currCamHeight = 1000;

    //airports
    public TextAsset airportsFile;
    public TextAsset airportsCSVFile;
    public Airport[] airports;
    public bool JsonAirports = false;

    // airplanes
    private List<DataItem> airplanes;
    private string jsonStr;
    private Airplane parsedData;

    APIDataManager apiMan;


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
            LoadJsonAirplanes();
        }
    }

    public void LoadJsonAirplanes() {
        /*
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://app.goflightlabs.com/flights?access_key=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI0IiwianRpIjoiYTc3OWE5MWU0YmQ0ZGE1MzRkZWIzNzkyNzBkYzllZDg0OTdiMWYxYWQyMWNiZTRiM2QwOTBjNjg5MDE2ZmRhNGE2Yzk1NGFmNWYwMzAzZDQiLCJpYXQiOjE2ODc4MTM2MzMsIm5iZiI6MTY4NzgxMzYzMywiZXhwIjoxNzE5NDM2MDMzLCJzdWIiOiIyMTI2NiIsInNjb3BlcyI6W119.tNUVzLBmTNre_7YFvnWlf0u1AisxVV0TA-tm6N1RpAhrO2OwERMQBfk55klKZR5sxQWjEDKAYs5x9WKoYG2T7w");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        */

        GameObject gameObject = new GameObject("APIDataManager");
        apiMan = gameObject.AddComponent<APIDataManager>();
        jsonStr = apiMan.jsonData;
        
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
        float airplaneScale = airportScale/5;

        foreach ( var airport in airports)
        {
            airport.location.transform.localScale = new Vector3(airportScale, airportScale, airportScale);
        }

        foreach ( var airplane in airplanes)
        {
            airplane.location.transform.localScale = new Vector3(airplaneScale, airplaneScale, airplaneScale);
        }
    }
}
