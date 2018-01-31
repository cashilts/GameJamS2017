using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Player : MonoBehaviour {

    public int gold = 100;
    public int GPT = 0;
    public bool aiPlayer = false;
    public int id;
    public Color playerColor;
    public Material playerOccupiedTile;

    protected List<Unit> ownedUnits = new List<Unit>();
    protected List<City> ownedCities = new List<City>();

	// Use this for initialization
	void Start () {
        playerOccupiedTile = new Material(Resources.Load<Material>("Models/Materials/OccupyMat"));
        Color matColor = playerColor;
        matColor.a = 40f/255f;
        playerOccupiedTile.color = matColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void newTurn() {
        gold += GPT;
        foreach(Unit unit in ownedUnits)
        {
            unit.newTurn();
        }
        foreach(City city in ownedCities)
        {
            city.newTurn();
        }
    }

    public virtual void doTurn(){

    }

    public void giveUnit(Unit unit)
    {
        ownedUnits.Add(unit);
    }

    public void removeUnit(Unit unit)
    {
        ownedUnits.Remove(unit);
    }

    public void giveCity(City city)
    {
        ownedCities.Add(city);
    }

    public XmlElement savePlayer(ref XmlDocument doc)
    {
        XmlElement playerElement = doc.CreateElement("Player");
        playerElement.SetAttribute("Gold", gold.ToString());
        playerElement.SetAttribute("GPT", GPT.ToString());
        playerElement.SetAttribute("id", GPT.ToString());
        return playerElement;
    }

    public void loadPlayer(XmlNode playerNode)
    {
        XmlAttributeCollection playerElements = playerNode.Attributes;
        gold = Convert.ToInt32(playerElements.GetNamedItem("Gold").Value);
        GPT = Convert.ToInt32(playerElements.GetNamedItem("GPT").Value);
        id = Convert.ToInt32(playerElements.GetNamedItem("id").Value);
    }
}
