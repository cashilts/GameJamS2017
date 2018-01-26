using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Boat : Unit {

    List<Unit> onBoat = new List<Unit>();

    #region Override 
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
            return 0;
        }
    }

    public override int buildCost
    {
        get
        {
            return 2;
        }
    }
    private int _health = 2;
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

    public override string iconImage
    {
        get
        {
            return "sailboat";
        }
    }
    protected override int baseSpeed
    {
        get
        {
            return 5;
        }
    }

    protected override int maintenanceCost
    {
        get
        {
            return 0;
        }
    }
#endregion
    // Use this for initialization
    void Start () {
        allowedTiles.Add(Tile.tileType.DeepWater);
        allowedTiles.Add(Tile.tileType.ShallowWater);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addToBoat(Unit toAdd)
    {
        onBoat.Add(toAdd);
        toAdd.transform.parent.GetComponent<Tile>().removeUnit(toAdd);
        transform.parent.GetComponent<Tile>().addUnitsToTile(toAdd);
    }

    public void removeFromBoat(Unit toRemove)
    {
        onBoat.Remove(toRemove);
    }

    public override void startMoveUnit()
    {
        System.String name = transform.parent.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        List<Tile.tileType> bannedList = new List<Tile.tileType>();
        bannedList.Add(Tile.tileType.Grass);
        bannedList.Add(Tile.tileType.Ice);
        transform.parent.parent.GetComponent<BoardManager>().markMovement(bannedList, speed, transform.parent.GetComponent<Tile>(), transform.parent.GetComponent<Tile>());
        if (unitMenu != null) Destroy(unitMenu.gameObject);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.ACTIONTARGET);
        Camera.main.GetComponent<CameraControllerPC>().setActionTarget(new CameraControllerPC.targetSelectMethod(endMoveUnit));
    }

    public override void endMoveUnit(Tile endLocation)
    {
        if (endLocation.setType == Tile.tileType.Grass || endLocation.setType == Tile.tileType.Ice) return;
        if (endLocation.unitsOnTile.Count > 0 && endLocation.unitsOnTile[0].owner != owner) return;
        System.String name = transform.parent.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        transform.parent.parent.GetComponent<BoardManager>().unmarkTilesInRadius(speed, y, x);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.TILESELECT);
        int distanceMoved = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(transform.parent.GetComponent<Tile>(), endLocation);
        if (distanceMoved > speed)
        {
            name = endLocation.gameObject.name;
            commaBreak = name.IndexOf(',');
            int x2 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
            int y2 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

            if (x < speed)
            {
                int testX = x + 100;
                int newDistance = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(testX, y, x2, y2);
                if (newDistance <= baseSpeed)
                {
                    endLocation.addUnitsToTile(gameObject.GetComponent<Unit>());
                    transform.parent.GetComponent<Tile>().removeUnit(this);
                    speed -= newDistance;
                }
            }
            if (x2 < speed)
            {
                int testX = x + 100;
                int newDistance = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(x, y, testX, y2);
                if (newDistance <= baseSpeed)
                {
                    endLocation.addUnitsToTile(gameObject.GetComponent<Unit>());
                    transform.parent.GetComponent<Tile>().removeUnit(this);
                    speed -= newDistance;
                }
            }
            return;
        }
        if ((transform.parent.parent.GetComponent<BoardManager>().isMovePossible(transform.parent.GetComponent<Tile>(), endLocation, this)))
        {
            return;
        }   

        transform.parent.GetComponent<Tile>().removeUnit(this);
        endLocation.addUnitsToTile(gameObject.GetComponent<Unit>());
        foreach (Unit unit in onBoat)
        {
            transform.parent.GetComponent<Tile>().removeUnit(unit);
            endLocation.addUnitsToTile(unit);
        }
        speed -= distanceMoved;
    }

    public override void newTurn()
    {
        speed = baseSpeed;
    }

    public override XmlElement saveUnit(ref XmlDocument doc)
    {
        XmlElement newElement = doc.CreateElement("Boat");
        newElement.SetAttribute("speed", speed.ToString());
        newElement.SetAttribute("health", health.ToString());
        newElement.SetAttribute("Owner", owner.id.ToString());
        return newElement;
    }
}
