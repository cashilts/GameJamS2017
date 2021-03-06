﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using UnityEngine;

public class Tile : MonoBehaviour {

    //Types of tiles
    public static readonly IList<string> typeNames = new ReadOnlyCollection<string>(new List<string> { "DeepWater", "ShallowWater", "Ice", "Grass" });
    public enum tileType { DeepWater, ShallowWater, Ice, Grass };
    public tileType setType = tileType.DeepWater;

    //Water properties of rivers that permeate simple tiles, directions determines which directions a river moves through the tile
    static readonly string[] directionNames = { "TL", "TR", "R", "BR", "BL", "L" };
    public bool[] waterDirections = new bool[6];
    public bool hasWater = false;


    public List<Unit> unitsOnTile = new List<Unit>(5);
    public Unit selectedUnit;
    public City cityOnTile;
    public Player owner;

    public int wealth;
    public int food;

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


    public void addUnitsToTile(Unit unit)
    {
        
        unit.transform.SetParent(transform,false);
        unitsOnTile.Add(unit);
        if (unitsOnTile.Count > 1) unit.gameObject.SetActive(false);
        else selectedUnit = unit;
    }



    public void onMouseButtonDown()
    {
        if (cityOnTile != null)
        {
            if (unitsOnTile.Count == 0 && cityOnTile.owner.id == 0) cityOnTile.onSelect();
            else if(cityOnTile.owner.id == 0)
            {
                GameObject newMenu = Instantiate(Resources.Load<GameObject>("Prefabs/RadialMenu"));
                newMenu.transform.SetParent(transform, false);
                newMenu.GetComponent<RadialMenu>().addOptionToMenu(new RadialButton.passDelegate(SelectUnit), -1, Resources.Load<Sprite>("Images/village"));
                for(int i = 0; i< unitsOnTile.Count; i++)
                {
                    newMenu.GetComponent<RadialMenu>().addOptionToMenu(new RadialButton.passDelegate(SelectUnit), i, Resources.Load<Sprite>("Images/" + unitsOnTile[i].iconImage));
                }
                tileMenu = newMenu.GetComponent<RadialMenu>();
            }
        }
        else
        {

            if (unitsOnTile.Count == 0) return;
            if (unitsOnTile.Count == 1 && unitsOnTile[0].ownerIndex == 0)
            {
                /*System.String name = gameObject.name;
                int commaBreak = name.IndexOf(',');
                int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
                int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

                transform.parent.GetComponent<BoardManager>().markTilesInRadius(unitsOnTile[0].baseSpeed, y, x);*/

                unitsOnTile[0].onSelect();
            }
            else if(unitsOnTile[0].ownerIndex == 0)
            {
                GameObject newMenu = (GameObject)Instantiate(Resources.Load("Prefabs/RadialMenu"));
                newMenu.transform.SetParent(transform, false);
                for (int i = 0; i < unitsOnTile.Count; i++)
                {
                    newMenu.GetComponent<RadialMenu>().addOptionToMenu(new RadialButton.passDelegate(SelectUnit), i, Resources.Load<Sprite>("Images/" + unitsOnTile[i].iconImage));
                }
                tileMenu = newMenu.GetComponent<RadialMenu>();
            }
        }
        
    }

    public void SelectUnit(int unitToSelect)
    {
        
        Destroy(tileMenu.gameObject);
        if (unitToSelect == -1)
        {
            cityOnTile.onSelect();
            return;
        }
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
        if (unitsOnTile.Count != 0)
        {
            selectedUnit = unitsOnTile[0];
            selectedUnit.gameObject.SetActive(true);
        }
    }

    public void claimTile(Player tileOwner)
    {
        owner = tileOwner;
        tileOwner.GPT += wealth;
        int materialCount = GetComponent<MeshRenderer>().materials.Length;
        List<Material> materialList = new List<Material>();
        for (int i = 0; i < materialCount; i++)
        {
            materialList.Add(GetComponent<MeshRenderer>().materials[i]);
        }
        materialList.Add(owner.playerOccupiedTile);
       GetComponent<MeshRenderer>().materials = materialList.ToArray();
    }


    public XmlElement saveTile(ref XmlDocument doc)
    {
        XmlElement tileElement = doc.CreateElement("Tile");

        tileElement.SetAttribute("type", setType.ToString());
        tileElement.SetAttribute("wealth", wealth.ToString());
        tileElement.SetAttribute("food", food.ToString());
        int waterMask = 0;
        for (int k = 0; k < 6; k++)
        {
            waterMask += System.Convert.ToInt32(waterDirections[k]);
            waterMask = waterMask << 1;
        }
        tileElement.SetAttribute("RiverOnTile", waterMask.ToString());

        if(owner != null)
        {
            tileElement.SetAttribute("Owner", owner.id.ToString());
        }
        else
        {
            tileElement.SetAttribute("Owner", "-1");
        }
        if (cityOnTile)
        {
            tileElement.SetAttribute("City", true.ToString());
            tileElement.AppendChild(cityOnTile.saveCity(ref doc));
        }
        else
        {
            tileElement.SetAttribute("City", false.ToString());
        }
       

        foreach(Unit u in unitsOnTile)
        {
            tileElement.AppendChild(u.saveUnit(ref doc));
        }

        return tileElement;
    }

    public void loadTile(XmlNode tileNode)
    {
        XmlAttributeCollection tileAttributes = tileNode.Attributes;

        setType = (tileType)typeNames.IndexOf(tileAttributes.GetNamedItem("type").Value);
        wealth = Convert.ToInt32(tileAttributes.GetNamedItem("wealth").Value);
        food = Convert.ToInt32(tileAttributes.GetNamedItem("food").Value);
        int waterMask = Convert.ToInt32(tileAttributes.GetNamedItem("RiverOnTile").Value);
        for(int i = 0; i<6; i++)
        {
            bool water = Convert.ToBoolean(waterMask % 2);
            waterDirections[5 - i] = water;
            if (water) hasWater = true;
            waterMask = waterMask >> 1;
        }
        int ownerId = Convert.ToInt32(tileAttributes.GetNamedItem("Owner").Value);
        if (ownerId != -1)
        {
            owner = GameObject.Find("Player" + ownerId).GetComponent<Player>();
        }
        XmlNode tileChildren = tileNode.FirstChild;
        bool city = Convert.ToBoolean(tileAttributes.GetNamedItem("City").Value);
        if (city)
        {
            GameObject newCity = Instantiate(Resources.Load<GameObject>("Prefabs/City"));
            newCity.transform.SetParent(transform, false);
            cityOnTile = newCity.GetComponent<City>();
            cityOnTile.loadCity(tileChildren);
            owner.giveCity(cityOnTile);
            tileChildren = tileChildren.NextSibling;
        }

        while(tileChildren != null)
        {
            string unitType = tileChildren.Name;
            GameObject newUnit = (GameObject)Instantiate(Resources.Load("Prefabs/" + unitType));
            newUnit.GetComponent<Unit>().loadUnit(tileChildren);
            tileChildren = tileChildren.NextSibling;
        }
        updateTileGraphics();
    }

    void updateTileGraphics()
    {
        List<Material> materials = new List<Material>();

        if (setType == tileType.Ice) materials.Add(Resources.Load<Material>("Models/Materials/IceTex"));
        else if (setType == tileType.DeepWater) materials.Add(Resources.Load<Material>("Models/Materials/PlainTex"));
        else if (setType == tileType.ShallowWater) materials.Add(Resources.Load<Material>("Models/Materials/SWater"));
        else if (setType == tileType.Grass) materials.Add(Resources.Load<Material>("Models/Materials/GrassTex"));

        if(owner != null)
        {
            materials.Add(owner.playerOccupiedTile);
        }
        GetComponent<MeshRenderer>().materials = materials.ToArray();
        if (hasWater) getWaterTexture();
    }



}
