using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInput : MonoBehaviour {
    stringFunction callback;

    public delegate void stringFunction(string textInput);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCallback(stringFunction newCallback)
    {
        callback = newCallback;
    }

    public void onSubmit()
    {
        GameObject.Find("Camera").GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.TILESELECT);
        string text = transform.Find("Input").Find("Text").GetComponent<UnityEngine.UI.Text>().text;
        callback(text);
        Destroy(GameObject.Find("InputPanel"));
    }
}
