using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityMenu : MonoBehaviour {
    int amountOfOptions = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOptionToMenu(CityOption.clickOption methodToAdd, string arg, string optionText)
    {
        GameObject newCityOption = (GameObject)Instantiate(Resources.Load("Prefabs/CityOption"));
        newCityOption.transform.GetChild(0).GetComponent<Text>().text = optionText;
        newCityOption.GetComponent<CityOption>().setClickMethod(methodToAdd,arg);
        newCityOption.transform.SetParent(transform.GetChild(1), false);
        newCityOption.transform.localPosition = new Vector3(90, 235.8f + (-30 * amountOfOptions));
        Debug.Log(newCityOption.transform.localPosition);
        amountOfOptions++;
    }

}
