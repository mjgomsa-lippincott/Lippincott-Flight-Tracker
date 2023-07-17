using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        foreach(var device in devices)
        {
            Debug.Log(device.name + device.characteristics);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
