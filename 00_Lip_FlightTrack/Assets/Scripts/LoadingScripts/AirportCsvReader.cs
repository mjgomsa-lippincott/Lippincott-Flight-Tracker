using System.Collections.Generic;
using System;
using UnityEngine;
using CesiumForUnity;

public class AirportCsvReader
{
    public List<Airport> ReadAirports(TextAsset airportCSVFile)
    {
        List<Airport> airports = new List<Airport>();

        string[] dataLines = airportCSVFile.text.Split('\n');

        for(int i = 1; i < dataLines.Length; i++)
        {
            string[] fields = dataLines[i].Split(',');

            if (fields.Length > 13)
            {
                if (fields[2] == "\"large_airport\"")
                {
                    Airport airportInfo = new Airport();

                    airportInfo.name = fields[3];
                    airportInfo.icao = fields[1];
                    airportInfo.iata = fields[13];
                    airportInfo.city = fields[10];
                    airportInfo.country = fields[8];
                    airportInfo.state = fields[9];
                    airportInfo.elevation = 0;
                    airportInfo.latitude = Convert.ToDouble(fields[4]);
                    airportInfo.longitude = Convert.ToDouble(fields[5]);
                    
                    airports.Add(airportInfo);
                }
            }
        }
        Debug.Log("Finished Parsing CSV");
        return airports;
    }
}
