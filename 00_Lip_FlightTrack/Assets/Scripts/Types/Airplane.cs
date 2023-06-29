using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using CesiumForUnity;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Aircraft
{
    public string iataCode;
    public string icao24;
    public string icaoCode;
    public string regNumber;
}

[System.Serializable]
public class Airline
{
    public string iataCode;
    public string icaoCode;
}

[System.Serializable]
public class ArrivalDeparture
{
    public string iataCode;
    public string icaoCode;
}

[System.Serializable]
public class Flight
{
    public string iataNumber;
    public string icaoNumber;
    public string number;
}

[System.Serializable]
public class Geography
{
    public float altitude;
    public int direction;
    public float latitude;
    public float longitude;
}

[System.Serializable]
public class Speed
{
    public float horizontal;
    public int isGround;
    public int vspeed;
}

[System.Serializable]
public class SystemInfo
{
    public string squawk;
    public int updated;
}

[System.Serializable]
public class DataItem
{
    public Aircraft aircraft;
    public Airline airline;
    public ArrivalDeparture arrival;
    public ArrivalDeparture departure;
    public Flight flight;
    public Geography geography;
    public Speed speed;
    public string status;
    public SystemInfo system;
    public GameObject location;
}

[System.Serializable]
public class Airplane
{
    public bool success;
    public List<DataItem> data;

}
