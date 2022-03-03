using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehavior : MonoBehaviour
{
    GameObject camObj;//This is your camera with the free look component on it

    CinemachineFreeLook freeLook;// this reference the free look component in your camera

    CinemachineComposer comp;//I named this variable comp for "Composer", you can name it however you like. This is the cinemachine component with all the aiming stuff on it

    // Start is called before the first frame update
    void Start()
    {
        camObj = GameObject.FindWithTag("MainCamera");

    freeLook = camObj.GetComponent<CinemachineFreeLook>();

    comp = freeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        comp.m_TrackedObjectOffset.x+= 1f;
        comp.m_TrackedObjectOffset.z+= 1f;
    }
}
