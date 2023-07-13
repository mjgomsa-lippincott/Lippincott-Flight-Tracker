using UnityEngine;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Serialization;

public class APIDataManager : MonoBehaviour
{
    private string apiKey;
    public string jsonData;
    private string apiURL;
    void Awake()
    {
        FetchDataFromAPI();
       
    }

    /*private bool ShouldFetchData()
    {
        string lastRequestTimeStr = PlayerPrefs.GetString(LastRequestTimeKey);
        if (!string.IsNullOrEmpty(lastRequestTimeStr))
        {
            DateTime lastRequestTime = DateTime.Parse(lastRequestTimeStr);
            TimeSpan timeSinceLastRequest = DateTime.Now - lastRequestTime;
            //Debug.Log(lastRequestTimeStr);
            return timeSinceLastRequest.TotalHours >= 1;
        }
        return true; // Fetch data if no previous request recorded
    }*/

    public void FetchDataFromAPI()
    {
        string apiKeyFilePath = Path.Combine(Application.dataPath, "APIKey.txt");
        apiKey = File.ReadAllText(apiKeyFilePath);
        apiURL = "https://app.goflightlabs.com/flights?access_key=" + apiKey;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());

        if (sr != null)
        {
            jsonData = sr.ReadToEnd();
        } else {
            Debug.Log("Is Null!");
        }
    }

    public bool ShouldFetchData() { 
   
        return jsonData != null; 
    }
}


