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
    public Vector3 newSize;
 // Vector3 newSize = new Vector3(10

    public bool autoRunOnAwake = true;
    
    public TextAsset airplaneFile;
    private string jsonStr;
    public Airplane parsedData;
    private List<DataItem> airplanes;

    void Awake()
    {
        if (autoRunOnAwake)
        {
            Load();
        }
    }

    public void Load()
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
