using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class City : BoardObject {
   
    public Unit unitInProduction;
    public Player owner;
    Tile parentTile;
    const int NUMMODELS = 2;
    int production = 1;
    int commitedProduction = 0;
    

	// Use this for initialization
	void Start () {
        //Get tile city is located on
        parentTile = transform.parent.GetComponent<Tile>();

        //Set random buildings layout
        for (int i = 0; i < 3; i++)
        {
            int houseModel = Random.Range(1, NUMMODELS+1);
            Debug.Log(houseModel);
            GameObject house = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Models/house"+houseModel.ToString()));
            house.transform.SetPositionAndRotation(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-2f, 0f)), house.transform.rotation);
            house.transform.SetParent(transform, false);
        }
       
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onSelect()
    {
        //Change Camera mode for menu
        GameObject.Find("Camera").GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.CITYMENU);

        //Display city options HUD and remove basic HUD
        Destroy(GameObject.Find("MapHUD"));
        GameObject cityHud = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/CityHUD"));
        cityHud.name = "CityHUD";

        //Determine which options for menu
        GameObject newCityMenu = cityHud.transform.Find("CityPanel").gameObject;
        newCityMenu.GetComponent<CityMenu>().addOptionToMenu(new CityOption.clickOption(startProductionOnUnit), "Settler","Settler: " + (int)(new Settler().buildCost/production) + " turns");
        newCityMenu.GetComponent<CityMenu>().addOptionToMenu(new CityOption.clickOption(startProductionOnUnit), "Warrior", "Warrior: " + (int)(new Warrior().buildCost/production) + " turns");
        for (int i = 0; i < 6; i++)
        {
            Tile checkTile = BoardManager.Instance.getNeighborByDirection((BoardManager.tileDirections)i,parentTile);
            if (checkTile.setType == Tile.tileType.ShallowWater)
            {
                newCityMenu.GetComponent<CityMenu>().addOptionToMenu(new CityOption.clickOption(startProductionOnUnit), "boat", "Boat: " + (int)(new Boat().buildCost / production) + " turns");
                break;
            }
        }
    }

    public void spawnUnit()
    {
        //Get the hidden inactive unit and make it active and add it to the tile 
        Unit unitScript = unitInProduction.GetComponent<Unit>();
        unitInProduction.newTurn();
        unitInProduction.gameObject.SetActive(true);
        parentTile.addUnitsToTile(unitScript);
        owner.giveUnit(unitScript);
        unitScript.owner = owner;
        unitScript.ownerIndex = owner.id;
        if (unitScript.transform.Find("Cube").GetComponent<SkinnedMeshRenderer>() == null)
        {
            unitScript.transform.Find("Cube").GetComponent<MeshRenderer>().materials[0].SetColor("_Color", owner.playerColor);
        }
        else
        {
            unitScript.transform.Find("Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", owner.playerColor);
        }

    }

    public void startProductionOnUnit(string unitName)
    {
        //Create new unit and hide it
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

    public XmlElement saveCity(ref XmlDocument doc)
    {
        XmlElement tileElement = doc.CreateElement("City");
        if (unitInProduction) {
            tileElement.SetAttribute("UnitInProduction", unitInProduction.GetType().ToString());
            tileElement.SetAttribute("Production", commitedProduction.ToString());
        }
        tileElement.SetAttribute("Owner", owner.id.ToString());
        return tileElement;
    }
}
