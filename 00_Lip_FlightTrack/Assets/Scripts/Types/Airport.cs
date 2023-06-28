using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using CesiumForUnity;
using UnityEngine.UI;

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
    public CesiumForUnity.CesiumGeoreference location;
}


/*"icao": "00AK",
        "iata": "",
        "name": "Lowell Field",
        "city": "Anchor Point",
        "state": "Alaska",
        "country": "US",
        "elevation": 450,
        "lat": 59.94919968,
        "lon": -151.695999146,
        "tz": "America\/Anchorage"*/