using CesiumForUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Environment Variables
    public LoadingManager loader;
    public CesiumGeoreference geo;
    public GameObject cam;

    //Plane
    private DataItem plane;
    private double3 planePosition;
    bool setPlane = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!setPlane) { setNewPlane(); }

    }

    void setNewPlane()
    {
        plane = loader.airplanes[UnityEngine.Random.Range(0, loader.airplanes.Count)];
        planePosition = plane.location.GetComponent<CesiumGlobeAnchor>().longitudeLatitudeHeight;
        setPlane = true;
    }
}
