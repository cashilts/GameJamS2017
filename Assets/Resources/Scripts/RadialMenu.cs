using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {

    int radius = 1;
    int maxObjects = 6;
    int currentObjects = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void addOptionToMenu(RadialButton.passDelegate methodToCall, int valueToGive)
    {
        if (currentObjects != maxObjects)
        {
            GameObject newButton = (GameObject)Instantiate(Resources.Load("Prefabs/RadialButton"));
            newButton.transform.SetParent(transform, false);
            newButton.GetComponent<RadialButton>().createButton(methodToCall, valueToGive);
        }
    }
}
