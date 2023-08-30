using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using CesiumForUnity;
using UnityEngine;
using System;

public class CameraAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public CesiumGeoreference geo;
    public GameObject cam;

    //Focusing the Globe Scene;
    public double globePauseLen;
    public double3 globePosition;
    public double globeTransitionLen;

    //Focusing Country Scale
    public double countryPauseLen;
    public double3 countryPosition;
    public double countryTransitionLen;

    //Foucsing Plane Scale
    public double planePauseLen;

    //Variables
    private double[] timings;

    public LoadingManager loader;
    private DataItem plane;
    private double3 planePosition;
    bool setPlane = false;

    void Start()
    {
        timings = new double[6];
        timings[0] = 0;
        timings[1] = globePauseLen + timings[0];
        timings[2] = globeTransitionLen + timings[1];
        timings[3] = countryPauseLen + timings[2];
        timings[4] = countryTransitionLen + timings[3];
        timings[5] = planePauseLen + timings[4];
    }

    // Update is called once per frame
    void Update()
    {
        if (!setPlane)
        {
            plane = loader.airplanes[UnityEngine.Random.Range(0, loader.airplanes.Count)];
            planePosition = plane.location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight;
            setPlane = true;
        }

        double currentTime = Time.timeSinceLevelLoadAsDouble;

        int stage = 0;
        for (int i = 0; i < timings.Length; i++)
        {
            if (timings[i] < currentTime )
            {
                stage++;
            }
        }

        double percentage = 0;
        double timingS = 0;
        double timingE = 0;
        if ( stage < timings.Length && stage >= 1 )
        {
            percentage = (currentTime - timings[stage - 1]) / (timings[stage] - timings[stage - 1]);
            timingS = timings[stage - 1];
            timingE = timings[stage];

        }

        if (stage == 1)
        {
            double3 globeTangent = globePosition;
            globeTangent[2] = 0;

            var ecef = CesiumWgs84Ellipsoid.LongitudeLatitudeHeightToEarthCenteredEarthFixed(globeTangent);
            Vector3 look = (float3)geo.TransformEarthCenteredEarthFixedDirectionToUnity(ecef);

            cam.GetComponentInParent<CesiumGlobeAnchor>().longitudeLatitudeHeight = globePosition;
            cam.transform.rotation.Set(90f, 0f, 0f, 0f);
            //cam.transform.LookAt(look);
        }
        else if (stage == 2)
        {
            cam.GetComponentInParent<CesiumGlobeAnchor>().longitudeLatitudeHeight = interDouble3(globePosition, countryPosition, timingS, currentTime, timingE);
        }
        else if (stage == 3)
        {
            cam.GetComponentInParent<CesiumGlobeAnchor>().longitudeLatitudeHeight = countryPosition;
        }
        else if (stage == 4)
        {
            planePosition = plane.location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight;
            cam.GetComponentInParent<CesiumGlobeAnchor>().longitudeLatitudeHeight = interDouble3(countryPosition, planePosition, timingS, currentTime, timingE);
        }
        else if (stage == 5)
        {
            planePosition = plane.location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight + new double3(0, 0, 100);
            cam.GetComponentInParent<CesiumGlobeAnchor>().longitudeLatitudeHeight = planePosition;
        }
        else if (stage == 6)
        {
            GetComponentInParent<SceneController>().ToggleScenes();
        }

        //double3 camPos = cam.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight;
        //cam.transform.LookAt(new Vector3((float) camPos[0], (float)camPos[1], (float) camPos[2]));
    }

    double3 interDouble3 (double3 pos1, double3 pos2, double timingS, double currentTime, double timingE) 
    {
        double x = pos1[0] + ( currentTime - timingS) * (pos2[0] - pos1[0]) / (timingE - timingS);
        double y = pos1[1] + ( currentTime - timingS) * (pos2[1] - pos1[1]) / (timingE - timingS);
        double z = pos1[2] + ( currentTime - timingS) * (pos2[2] - pos1[2]) / (timingE - timingS);

        return new double3(x,y,z);
    }
}