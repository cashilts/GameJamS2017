using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButton : MonoBehaviour {

    passDelegate functionToCall;
    int valueToPass;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public delegate void passDelegate(int i);

    public void createButton(passDelegate functionOnPress,int passValue)
    {
        functionToCall = functionOnPress;
        valueToPass = passValue;
    }

    public void onClick()
    {
        functionToCall(valueToPass);
    }
}
