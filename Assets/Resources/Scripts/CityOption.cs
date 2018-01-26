using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityOption : MonoBehaviour {

    clickOption clickMethod;
    string storedArg;
    public delegate void clickOption(string arg);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void setClickMethod(clickOption method, string argument)
    {
        clickMethod = method;
        storedArg = argument;
    }

    public void onClick()
    {
        clickMethod(storedArg);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.TILESELECT);
        GameObject hud = Instantiate(Resources.Load<GameObject>("Prefabs/MapHUD"));
        hud.name = "MapHUD";
        Destroy(GameObject.Find("CityHUD"));
    }
}
