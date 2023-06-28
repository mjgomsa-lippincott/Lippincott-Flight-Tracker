using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Security.Cryptography.X509Certificates;

public class LoadingManager : MonoBehaviour
{
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
     // Debug.Log("loading...");

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
            Debug.Log("iataCode: " + airplane.aircraft.iataCode);
        }
    }
}
