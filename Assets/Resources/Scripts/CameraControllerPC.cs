using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerPC : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x > 0 && mousePos.y > 0)
        {
            if (mousePos.x < Screen.width * 0.25)
            {
                transform.Translate(-0.1f, 0, 0);
            }
            else if (mousePos.x > Screen.width * 0.75)
            {
                transform.Translate(0.1f, 0, 0);
            }
            if (mousePos.y < Screen.height * 0.25)
            {
                transform.Translate(0, -0.1f, 0);
            }
            else if (mousePos.y > Screen.height * 0.75)
            {
                transform.Translate(0, 0.1f, 0);
            }
        }
	}
}
