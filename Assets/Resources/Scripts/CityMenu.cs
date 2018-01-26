using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityMenu : MonoBehaviour {
    //Keep track of item count in menu, to be used later to make large menus scroll
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
        newCityOption.transform.SetParent(transform.GetChild(1), true);
        newCityOption.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, gameObject.GetComponent<RectTransform>().rect.height/2 - 15 + (-30 * amountOfOptions));
        amountOfOptions++;
    }

    void test(string empty)
    {
        Debug.Log(empty);
    }

}
