using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButton : MonoBehaviour {

    passDelegate functionToCall;
    passDelegateNoValue functionNoValue;
    int valueToPass;
    bool valueNeeded;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public delegate void passDelegate(int i);
    public delegate void passDelegateNoValue();

    public void createButton(passDelegate functionOnPress,int passValue)
    {
        functionToCall = functionOnPress;
        valueToPass = passValue;
        valueNeeded = true;
    }

    public void createButton(passDelegateNoValue functionOnPress)
    {
        functionNoValue = functionOnPress;
        valueNeeded = false;
    }

    public void onClick()
    {
        if (valueNeeded)
        {
            functionToCall(valueToPass);
        }
        else
        {
            functionNoValue();
        }
        
    }
}
