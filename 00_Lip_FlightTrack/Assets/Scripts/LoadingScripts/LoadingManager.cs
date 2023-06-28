using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LoadingManager : MonoBehaviour
{
    public bool autoRunOnAwake = true;
    
    public TextAsset airplaneFile;
    private string jsonStr;
    public Airplane parsedData;

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
       
        foreach(var dataItem in parsedData.data)
        {
            Debug.Log("iataCode: " + dataItem.aircraft.iataCode);
        }
    }
}
