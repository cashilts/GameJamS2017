using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : BoardObject {

    int production = 1;
    public Unit unitInProduction;
    int commitedProduction = 0;
    public Player owner;
    public int ownerIndex = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onSelect()
    {
        GameObject.Find("InfoPanel").GetComponent<InfoPanel>().setObject(this);
        GameObject newCityMenu = (GameObject)Instantiate(Resources.Load("Prefabs/CityPanel"));
        newCityMenu.name = "CityMenu";
        newCityMenu.transform.SetParent(GameObject.Find("Canvas").transform, false);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.INMENU);
        newCityMenu.GetComponent<CityMenu>().addOptionToMenu(new CityOption.clickOption(startProductionOnUnit), "Settler","Settler: " + (int)(new Settler().buildCost/production) + " turns");
        newCityMenu.GetComponent<CityMenu>().addOptionToMenu(new CityOption.clickOption(startProductionOnUnit), "Warrior", "Warrior: " + (int)(new Warrior().buildCost/production) + " turns");
    }

    public void spawnUnit()
    {
        Unit unitScript = unitInProduction.GetComponent<Unit>();
        unitInProduction.newTurn();
        unitInProduction.gameObject.SetActive(true);
        transform.parent.GetComponent<Tile>().addUnitsToTile(unitScript);
        owner.giveUnit(unitScript);
        unitScript.ownerIndex = ownerIndex;
        unitScript.transform.Find("Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", owner.playerColor);

    }

    public void startProductionOnUnit(string unitName)
    {
        if (unitInProduction != null) Destroy(unitInProduction.gameObject);
        GameObject newUnit = (GameObject)Instantiate(Resources.Load("Prefabs/" + unitName));
        newUnit.transform.SetParent(transform.parent, false);
        newUnit.SetActive(false);
        unitInProduction = newUnit.GetComponent<Unit>();
    }

    public void newTurn()
    {
        commitedProduction += production;
        if(unitInProduction != null && commitedProduction >= unitInProduction.buildCost)
        {
            spawnUnit();
            commitedProduction -= unitInProduction.buildCost;
            unitInProduction = null;
        }
        
    }

    public override void getInfo()
    {
        if (unitInProduction != null)
        {
            GameObject.Find("InfoText").GetComponent<Text>().text = this.GetType().ToString() + ":\nIn Production: " + unitInProduction.GetType().ToString() + "\nTurns Left: " + (unitInProduction.buildCost - commitedProduction) / production;
        }
        else
        {
            GameObject.Find("InfoText").GetComponent<Text>().text = this.GetType().ToString() + ":\nIn Production: Nothing";
        }
    }
}
