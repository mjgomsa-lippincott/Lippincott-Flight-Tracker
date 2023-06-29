using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using UnityEngine;
using UnityEngine.Timeline;

public class AirportReader
{
    JsonTextReader reader;

    //Read airport json file
    public Airport[] ReadAirports(TextAsset airportFile)
    {
        List<Airport> airports = Read(airportFile);
        return airports.ToArray();
    }

    List<Airport> Read(TextAsset airportFile)
    {
        reader = new JsonTextReader(new System.IO.StringReader(airportFile.text));
        reader.Read();

        List<Airport> airports = new List<Airport>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                airports.Add(ReadAirport());
            }
        }

        return airports;
    }
    // here

    Airport ReadAirport()
    {
        Airport airportInfo = new Airport();

        int startDepth = reader.Depth;

        while (reader.Read() && reader.Depth > startDepth)
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                switch ((string)reader.Value)
                {
                    case "name":
                        airportInfo.name = reader.ReadAsString(); break;
                    case "icao":
                        airportInfo.icao = reader.ReadAsString(); break;
                    case "iata":
                        airportInfo.iata = reader.ReadAsString(); break;
                    case "city":
                        airportInfo.city = reader.ReadAsString(); break;
                    case "state":
                        airportInfo.state = reader.ReadAsString(); break;
                    case "country":
                        airportInfo.country = reader.ReadAsString(); break;
                    case "elevation":
                        airportInfo.elevation = (double)reader.ReadAsDouble(); break;
                    case "lat":
                        airportInfo.latitude = (double)reader.ReadAsDouble(); break;
                    case "lon":
                        airportInfo.longitude = (double)reader.ReadAsDouble(); break;
                }
            }
        }

        //Setup Globe Anchor
        GameObject location = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        location.AddComponent<CesiumGlobeAnchor>();
        location.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);
        location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight = new Unity.Mathematics.double3(airportInfo.longitude, airportInfo.latitude, 0);

        //Adjust Sizing of Globe
        location.GetComponent<SphereCollider>().radius = 10000;
        location.transform.localScale = new Vector3(10000, 10000, 10000);
        location.name = airportInfo.name;

        airportInfo.location = location;
        return airportInfo;
    }
}
