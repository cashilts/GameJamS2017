using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : BoardObject {


    //All units must set constant values for their base attack and defence
    public abstract int baseAttack { get; }
    public abstract int baseDefense { get; }
    protected abstract int baseSpeed { get; }
    public int speed;
    public int ownerIndex = 0;
    protected Player owner;
    protected abstract int maintenanceCost { get; }

    protected RadialMenu unitMenu;
	// Use this for initialization
	void Start () {
        Player myOwnder = GameObject.Find("Player" + ownerIndex).GetComponent<Player>();
        myOwnder.giveUnit(this);
        owner = myOwnder;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onSelect()
    {
        base.onSelect();
        GameObject radialMenuObj = (GameObject)Instantiate(Resources.Load("Prefabs/RadialMenu"));
        radialMenuObj.transform.SetParent(transform.parent, false);
        unitMenu = radialMenuObj.GetComponent<RadialMenu>();
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(startMoveUnit),Resources.Load<Sprite>("Images/walk"));
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(deleteUnit), Resources.Load<Sprite>("Images/remove"));
    }

    public void startMoveUnit()
    {
        System.String name = transform.parent.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        transform.parent.parent.GetComponent<BoardManager>().markTilesInRadius(speed, y, x);
        Destroy(unitMenu.gameObject);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.ACTIONTARGET);
        Camera.main.GetComponent<CameraControllerPC>().setActionTarget(new CameraControllerPC.targetSelectMethod(endMoveUnit));
    }

    public void endMoveUnit(Tile endLocation)
    {
        if (endLocation.setType != Tile.tileType.Grass) return;
       System.String name = transform.parent.gameObject.name;
       int commaBreak = name.IndexOf(',');
       int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
       int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
       transform.parent.parent.GetComponent<BoardManager>().unmarkTilesInRadius(speed, y, x);
       Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.TILESELECT);
        int distanceMoved = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(transform.parent.GetComponent<Tile>(), endLocation);
       if (distanceMoved> speed)
       {
           name = endLocation.gameObject.name;
           commaBreak = name.IndexOf(',');
           int x2 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
           int y2 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

           if (x < speed)
           {
               int testX = x + 100;
               int newDistance = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(testX, y, x2, y2);
               if(newDistance <= baseSpeed)
               {
                   endLocation.addUnitsToTile(gameObject);
                   transform.parent.GetComponent<Tile>().removeUnit(this);
                   speed -= newDistance; 
               }
           }
           if(x2 < speed)
           {
               int testX = x + 100;
               int newDistance = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(x, y, testX, y2);
               if (newDistance <= baseSpeed)
               {
                   endLocation.addUnitsToTile(gameObject);
                    transform.parent.GetComponent<Tile>().removeUnit(this);
                    speed -= newDistance;
               }
           }
           return;
       }
        transform.parent.GetComponent<Tile>().removeUnit(this);
        endLocation.addUnitsToTile(gameObject);
        speed -= distanceMoved;
        
        
    }

    public void deleteUnit()
    {
        Destroy(unitMenu.gameObject);
        Destroy(gameObject);
    }

    public virtual void newTurn()
    {
        speed = baseSpeed;
    }
}
