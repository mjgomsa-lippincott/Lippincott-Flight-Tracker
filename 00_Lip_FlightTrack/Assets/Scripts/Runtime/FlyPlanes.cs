using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyPlanes : MonoBehaviour
{
    // Start is called before the first frame update
    public LoadingManager loader;
    public List<Path> flightpaths;
    void Start()
    {
        foreach ( var dataItem in loader.airplanes )
        {
            Path path = new Path();
            Airport startPort = loader.airports.FirstOrDefault(a => a.iata == dataItem.departure.iataCode);
            Debug.Log(loader.airports.FirstOrDefault(a => a.iata == dataItem.departure.iataCode));
            Airport endPort = loader.airports.FirstOrDefault(a => a.iata == dataItem.arrival.iataCode);
            path.startLat = startPort.latitude;
            path.endLat = endPort.latitude; 
            path.startLon = endPort.longitude;
            path.endLon = endPort.longitude;
            flightpaths.Add(path);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var counter = 0;
        foreach (var dataItem in loader.airplanes ) 
        {
            dataItem.geography.latitude = (float)((flightpaths[counter].endLat - flightpaths[counter].startLat)/dataItem.speed.horizontal);
            dataItem.geography.longitude = (float)((flightpaths[counter].endLon - flightpaths[counter].startLon) / dataItem.speed.horizontal);
            counter++;
        }
    }
}