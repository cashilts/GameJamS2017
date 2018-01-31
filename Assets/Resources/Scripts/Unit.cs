using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : BoardObject {


    //All units must set constant values for their base attack and defence
    public abstract int baseAttack { get; }
    public abstract int baseDefense { get; }
    public abstract int buildCost { get; }
    protected abstract int baseSpeed { get; }
    protected abstract int maintenanceCost { get; }
    public abstract string iconImage { get; }



    public int speed;
    bool onBoat;
    public int ownerIndex = 0;
    public abstract int health { get; set; }
    public Player owner;
    
    Queue<Tile> autoPath;
    public List<Tile.tileType> allowedTiles = new List<Tile.tileType>();

    protected RadialMenu unitMenu;
	// Use this for initialization
	void Start () {
        Player myOwnder = GameObject.Find("Player" + ownerIndex).GetComponent<Player>();
        myOwnder.giveUnit(this);
        owner = myOwnder;
        allowedTiles.Add(Tile.tileType.Grass);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// On unit select bring up menu of unit actions
    /// </summary>
    public override void onSelect()
    {
        base.onSelect();
        //Set the info panel to display information about this unit
        GameObject.Find("InfoPanel").GetComponent<InfoPanel>().setObject(this);

        //Create the radial menu for this unit
        GameObject radialMenuObj = (GameObject)Instantiate(Resources.Load("Prefabs/RadialMenu"));
        radialMenuObj.transform.SetParent(transform.parent, false);
        unitMenu = radialMenuObj.GetComponent<RadialMenu>();
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(startMoveUnit),Resources.Load<Sprite>("Images/move"));
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(deleteUnit), Resources.Load<Sprite>("Images/skull"));

        //Check if boats are near the unit, if so let the unit have the option to get on the boat
        if (!onBoat)
        {
            for (int i = 0; i < 6; i++)
            {
                Tile checkTile = transform.parent.parent.GetComponent<BoardManager>().getNeighborByDirection((BoardManager.tileDirections)i, transform.parent.GetComponent<Tile>());
                if (checkTile.setType == Tile.tileType.ShallowWater)
                {
                    if (checkTile.unitsOnTile.Count != 0)
                    {
                        if (checkTile.unitsOnTile[0].GetComponent<Boat>() != null)
                        {
                            unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(addToNearestBoat), Resources.Load<Sprite>("Images/overhead"));
                            break;
                        }
                    }
                }
            }
        }
    }



    public void addToNearestBoat()
    {
        Destroy(unitMenu.gameObject);
        for (int i = 0; i < 6; i++)
        {
            Tile checkTile = transform.parent.parent.GetComponent<BoardManager>().getNeighborByDirection((BoardManager.tileDirections)i, transform.parent.GetComponent<Tile>());
            if (checkTile.setType == Tile.tileType.ShallowWater)
            {
                if (checkTile.unitsOnTile.Count != 0)
                {
                    if (checkTile.unitsOnTile[0].GetComponent<Boat>() != null)
                    {
                        checkTile.unitsOnTile[0].GetComponent<Boat>().addToBoat(this);
                        onBoat = true;
                    }
                }
            }
        }
    }

    public virtual void startMoveUnit()
    {
        System.String name = transform.parent.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        List<Tile.tileType> bannedList = new List<Tile.tileType>();
        bannedList.Add(Tile.tileType.DeepWater);
        bannedList.Add(Tile.tileType.Ice);
        bannedList.Add(Tile.tileType.ShallowWater);
        transform.parent.parent.GetComponent<BoardManager>().markMovement(bannedList, speed, transform.parent.GetComponent<Tile>(), transform.parent.GetComponent<Tile>());
        if(unitMenu != null)Destroy(unitMenu.gameObject);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.ACTIONTARGET);
        Camera.main.GetComponent<CameraControllerPC>().setActionTarget(new CameraControllerPC.targetSelectMethod(endMoveUnit));
    }

    public virtual void endMoveUnit(Tile endLocation)
    {
        if (endLocation.setType != Tile.tileType.Grass) return;
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

        if (onBoat)
        {
            onBoat = false;
            ((Boat)transform.parent.GetComponent<Tile>().unitsOnTile[0]).removeFromBoat(this);
        }
        transform.parent.GetComponent<Tile>().removeUnit(this);
        endLocation.addUnitsToTile(gameObject.GetComponent<Unit>());
        speed -= distanceMoved;

    }

    public void deleteUnit()
    {
        transform.parent.GetComponent<Tile>().removeUnit(this);
        Destroy(unitMenu.gameObject);
        Destroy(gameObject);
    }

    public virtual void newTurn()
    {
        speed = baseSpeed;
    }
    public override void getInfo()
    {
        GameObject.Find("InfoText").GetComponent<Text>().text = this.GetType().ToString() + ":\nMovement: " + speed + "\nHealth: " + health;
    }

    public virtual XmlElement saveUnit(ref XmlDocument doc)
    {
        XmlElement unit = doc.CreateElement("Unit");
        return unit;
    }

    public virtual void loadUnit(XmlNode unitNode)
    {

    }
}
