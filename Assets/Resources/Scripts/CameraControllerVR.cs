using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CameraControllerVR : CameraController {
    SteamVR_TrackedController leftController;
    float scrollRate = 0.2f;
	// Use this for initialization
	void Start () {
        leftController = transform.Find("Controller (left)").GetComponent<SteamVR_TrackedController>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(leftController.controllerState.rAxis0.y * scrollRate, 0, -leftController.controllerState.rAxis0.x * scrollRate);
	}

    public override XmlElement saveCamera(ref XmlDocument doc)
    {
        XmlElement cameraElement = doc.CreateElement("Camera");
        cameraElement.SetAttribute("XPos", transform.position.x.ToString());
        cameraElement.SetAttribute("YPos", transform.position.y.ToString());
        cameraElement.SetAttribute("ZPos", transform.position.z.ToString());
        return cameraElement;
    }
}
