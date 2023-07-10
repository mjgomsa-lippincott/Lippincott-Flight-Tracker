using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using CesiumForUnity;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]

public class Path
{
    public double startLat;
    public double startLon;
    public double endLat;
    public double endLon;
    public GameObject curve;
}