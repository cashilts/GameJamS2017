using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerVR : MonoBehaviour {
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
}
