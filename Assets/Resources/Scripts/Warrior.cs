using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Warrior : Unit
{ 
    bool hasAttack = true;
    int attackRange = 1;
    #region UnitOverrides
    public override int baseAttack
    {
        get
        {
            return 2;
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
            return 1;
        }
    }

    public override string iconImage
    {
        get
        {
            return "club";
        }
    }
    int _health = 3;
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
        allowedTiles.Add(Tile.tileType.Grass);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onSelect()
    {
        base.onSelect();
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(attackStart), Resources.Load<Sprite>("Images/club"));
    }

    public void attackStart()
    {
        if (hasAttack)
        {
            System.String name = transform.parent.gameObject.name;
            int commaBreak = name.IndexOf(',');
            int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
            int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
            List<Tile.tileType> bannedList = new List<Tile.tileType>();
            BoardManager.Instance.markMovement(bannedList, attackRange, transform.parent.GetComponent<Tile>(), transform.parent.GetComponent<Tile>());
            Destroy(unitMenu.gameObject);
            Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.ACTIONTARGET);
            Camera.main.GetComponent<CameraControllerPC>().setActionTarget(new CameraControllerPC.targetSelectMethod(attackEnd));
        }
    }

    public void attackEnd(Tile endAttack)
    {
        System.String name = transform.parent.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        transform.parent.parent.GetComponent<BoardManager>().unmarkTilesInRadius(attackRange, y, x);
        if (!(endAttack.unitsOnTile.Count == 0) && !(endAttack.unitsOnTile[0].ownerIndex == ownerIndex))
        {
            endAttack.unitsOnTile[0].health -= baseAttack - endAttack.unitsOnTile[0].baseDefense;
            if (endAttack.unitsOnTile[0].health < 0)
            {
                Unit killed = endAttack.unitsOnTile[0];
                Destroy(endAttack.unitsOnTile[0].gameObject);
                endAttack.removeUnit(killed);
                killed.owner.removeUnit(killed);
            }
            hasAttack = false;
        }
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.TILESELECT);
    }

    public override void newTurn()
    {
        speed = baseSpeed;
        hasAttack = true;
    }

    public override XmlElement saveUnit(ref XmlDocument doc)
    {
        XmlElement newElement = doc.CreateElement("Warrior");
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
