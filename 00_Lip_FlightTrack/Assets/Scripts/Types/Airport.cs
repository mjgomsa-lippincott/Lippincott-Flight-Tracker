using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using CesiumForUnity;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Airport
{
    public string name;
    public string iaco;
    public string iata;
    public string city;
    public string state;
    public string country;
    public double elevation;
    public double latitude;
    public double longitude;
    public string tz;
    public GameObject location;
}
