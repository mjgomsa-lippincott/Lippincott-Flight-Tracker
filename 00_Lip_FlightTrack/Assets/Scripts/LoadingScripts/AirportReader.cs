using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CesiumForUnity;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Scripting;

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
                    case "iaco":
                        airportInfo.iaco = reader.ReadAsString(); break;
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
                    case "latitude":
                        airportInfo.latitude = (double)reader.ReadAsDouble(); break;
                    case "longitude":
                        airportInfo.longitude = (double)reader.ReadAsDouble(); break;
                    case "tz":
                        airportInfo.tz = reader.ReadAsString(); break;

                }
            }
        }

        airportInfo.location = new CesiumGeoreference(); 
        return airportInfo;
    }
}
