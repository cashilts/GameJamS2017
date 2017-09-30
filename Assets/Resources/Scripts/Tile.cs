﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum tileType {DeepWater, ShallowWater,Ice, Grass};
    public tileType setType = tileType.DeepWater;

    public bool[] waterDirections = new bool[6];
    public bool hasWater = false;
    static readonly string[] directionNames = { "TL", "TR", "R", "BR", "BL", "L" };

    public List<Unit> unitsOnTile = new List<Unit>(5);
    public Unit selectedUnit;
    RadialMenu tileMenu;


	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void getWaterTexture() {
        string waterTexturePath = "Models/Textures/newRiver";
        for (int i = 0; i < 6; i++) {
            if (waterDirections[i]) {
                waterTexturePath += directionNames[i];
            }
        }
        waterTexturePath += "1";
        GetComponent<MeshRenderer>().material.mainTexture = (Texture)Resources.Load(waterTexturePath);
    }


    public void addUnitsToTile(GameObject unit)
    {
        
        unit.transform.SetParent(transform,false);
        unitsOnTile.Add(unit.GetComponent<Settler>());
        if (unitsOnTile.Count > 1) unit.SetActive(false);
        else selectedUnit = unit.GetComponent<Settler>();
    }



    public void onMouseButtonDown()
    {
        if (unitsOnTile.Count == 0) return;
        if(unitsOnTile.Count == 1)
        {
            /*System.String name = gameObject.name;
            int commaBreak = name.IndexOf(',');
            int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
            int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

            transform.parent.GetComponent<BoardManager>().markTilesInRadius(unitsOnTile[0].baseSpeed, y, x);*/

            unitsOnTile[0].onSelect();
        }
        else
        {
            GameObject newMenu = (GameObject)Instantiate(Resources.Load("Prefabs/RadialMenu"));
            newMenu.transform.SetParent(transform,false);
            for(int i = 0; i<unitsOnTile.Count; i++)
            {
                newMenu.GetComponent<RadialMenu>().addOptionToMenu(new RadialButton.passDelegate(SelectUnit), i, Resources.Load<Sprite>("Images/flag"));
            }
            tileMenu = newMenu.GetComponent<RadialMenu>();
        }
        
    }

    public void SelectUnit(int unitToSelect)
    {
        Destroy(tileMenu.gameObject);
        System.String name = gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        selectedUnit.gameObject.SetActive(false);
        unitsOnTile[unitToSelect].gameObject.SetActive(true);
        selectedUnit = unitsOnTile[unitToSelect];
        selectedUnit.onSelect();
        
    }

    public void onMouseButtonUp(Tile endPoint)
    {
        if (unitsOnTile.Count == 0) return;
        /*
        System.String name = gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        transform.parent.GetComponent<BoardManager>().unmarkTilesInRadius(unitsOnTile[0].baseSpeed, y, x); 
        if (transform.parent.GetComponent<BoardManager>().distanceBetweenTiles(this, endPoint) > unitsOnTile[0].baseSpeed)
        {
            name = endPoint.gameObject.name;
            commaBreak = name.IndexOf(',');
            int x2 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
            int y2 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

            if (x < unitsOnTile[0].baseSpeed)
            {
                int testX = x + 100;
                int newDistance = transform.parent.GetComponent<BoardManager>().distanceBetweenTiles(testX, y, x2, y2);
                if(newDistance <= unitsOnTile[0].baseSpeed)
                {
                    endPoint.addUnitsToTile(unitsOnTile[0].gameObject);
                    unitsOnTile.Remove(unitsOnTile[0]);
                }
            }
            if(x2 < unitsOnTile[0].baseSpeed)
            {
                int testX = x + 100;
                int newDistance = transform.parent.GetComponent<BoardManager>().distanceBetweenTiles(x, y, testX, y2);
                if (newDistance <= unitsOnTile[0].baseSpeed)
                {
                    endPoint.addUnitsToTile(unitsOnTile[0].gameObject);
                    unitsOnTile.Remove(unitsOnTile[0]);
                }
            }
            //return;
        }

        endPoint.addUnitsToTile(unitsOnTile[0].gameObject);
        unitsOnTile.Remove(unitsOnTile[0]);
        */
        
    }

    public void removeUnit(Unit removedUnit)
    {
        unitsOnTile.Remove(removedUnit);
        selectedUnit = unitsOnTile[0];
        selectedUnit.gameObject.SetActive(true);
    }

}
