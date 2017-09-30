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

    public void addOptionToMenu(RadialButton.passDelegate methodToCall, int valueToGive,Sprite imageForButton)
    {
        if (currentObjects != maxObjects)
        {
            currentObjects++;
            GameObject newButton = (GameObject)Instantiate(Resources.Load("Prefabs/RadialButton"));
            newButton.transform.SetParent(transform, false);

            float xCoord = Mathf.Cos(Mathf.PI * 2 * ((float)currentObjects / (float)maxObjects));
            float yCoord = Mathf.Sin(Mathf.PI * 2 * ((float)currentObjects / (float)maxObjects));

            newButton.GetComponent<RadialButton>().createButton(methodToCall, valueToGive);
            newButton.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = imageForButton;
            newButton.transform.localPosition = new Vector3(xCoord, 0, yCoord);

            Camera.main.GetComponent<CameraControllerPC>().menuOpened(this);
        }
    }

    public void addOptionToMenu(RadialButton.passDelegateNoValue methodToCall, Sprite imageForButton)
    {
        if (currentObjects != maxObjects)
        {
            currentObjects++;
            GameObject newButton = (GameObject)Instantiate(Resources.Load("Prefabs/RadialButton"));
            newButton.transform.SetParent(transform, false);

            float xCoord = Mathf.Cos(Mathf.PI * 2 * ((float)currentObjects / (float)maxObjects));
            float yCoord = Mathf.Sin(Mathf.PI * 2 * ((float)currentObjects / (float)maxObjects));

            newButton.GetComponent<RadialButton>().createButton(methodToCall);
            newButton.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = imageForButton;
            newButton.transform.localPosition = new Vector3(xCoord, 0, yCoord);

            Camera.main.GetComponent<CameraControllerPC>().menuOpened(this);
        }
    }
}
