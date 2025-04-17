using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class IRInterface: MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public static bool IRState = false;

    // Update is called once per frame
    void Update()
    {
        if (int.Parse(udpReceive.data) == 0)
        {
            IRState = true;
        }
        else
        {
            IRState = false;
        }

    }

    public bool IRStateChange()
    {
        Debug.Log(IRState);
        return IRState;
    }
}