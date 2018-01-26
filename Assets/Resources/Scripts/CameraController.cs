using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

    //Going to be developed into a general interface to the camera for either VR or normal PC controls


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public virtual XmlElement saveCamera(ref XmlDocument doc)
    {
        XmlElement cameraElement = doc.CreateElement("Camera");
        return cameraElement;
    }
}
