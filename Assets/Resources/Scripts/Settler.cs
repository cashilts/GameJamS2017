using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Settler : Unit {

    #region UnitOverrides
    public override int baseAttack
    {
        get
        {
            return 0;
        }
    }

    public override int baseDefense
    {
        get
        {
            return 1;
        }
    }

    protected override int baseSpeed
    {
        get
        {
            return 2;
        }
    }

    protected override int maintenanceCost
    {
        get
        {
            return 1;
        }
    }

    public override int buildCost
    {
        get
        {
            return 3;
        }
    }

    public override string iconImage
    {
        get
        {
            return "flag";
        }
    }
    int _health = 5;
    public override int health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;
        }
    }
    #endregion


    // Use this for initialization
    void Start () {
        Player myOwner = GameObject.Find("Player" + ownerIndex).GetComponent<Player>();
        myOwner.giveUnit(this);
        owner = myOwner;
        allowedTiles.Add(Tile.tileType.Grass);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onSelect()
    {
        base.onSelect();
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(settle), Resources.Load<Sprite>("Images/flag"));
    }

    public void settle()
    {
        if(transform.parent.GetComponent<Tile>().cityOnTile == null)
        {
            GameObject newCity = (GameObject)Instantiate(Resources.Load("Prefabs/City"));
            newCity.transform.SetParent(transform.parent,false);
            transform.parent.GetComponent<Tile>().cityOnTile = newCity.GetComponent<City>();
            BoardManager.Instance.claimTile(transform.parent.GetComponent<Tile>(), owner, 2);
            transform.parent.GetComponent<Tile>().removeUnit(this);
            owner.giveCity(newCity.GetComponent<City>());
            owner.removeUnit(this);
            newCity.GetComponent<City>().owner = owner;
            if (unitMenu != null)
            {
                Destroy(unitMenu.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public override void newTurn()
    {
        speed = baseSpeed;
    }

    public override XmlElement saveUnit(ref XmlDocument doc)
    {
        XmlElement newElement = doc.CreateElement("Settler");
        newElement.SetAttribute("speed", speed.ToString());
        newElement.SetAttribute("health", health.ToString());
        newElement.SetAttribute("Owner", owner.id.ToString());
        return newElement;
    }

    public override void loadUnit(XmlNode unitNode)
    {
        XmlAttributeCollection unitAttribs = unitNode.Attributes;
        speed = Convert.ToInt32(unitAttribs.GetNamedItem("speed").Value);
        health = Convert.ToInt32(unitAttribs.GetNamedItem("health").Value);
        int ownerId = Convert.ToInt32(unitAttribs.GetNamedItem("Owner").Value);
        owner = GameObject.Find("Player" + ownerId).GetComponent<Player>();
        owner.giveUnit(this);
        //transform.Find("Cube").GetComponent<MeshRenderer>().materials[0].SetColor("_Color", owner.playerColor);
    }
}
