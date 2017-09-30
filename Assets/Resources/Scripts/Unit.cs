using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : BoardObject {


    //All units must set constant values for their base attack and defence
    public abstract int baseAttack { get; }
    public abstract int baseDefense { get; }
    public abstract int baseSpeed { get; }
    protected abstract int maintenanceCost { get; }

    protected RadialMenu unitMenu;
	// Use this for initialization
	void Start () {
		
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
        transform.parent.parent.GetComponent<BoardManager>().markTilesInRadius(baseSpeed, y, x);
        Destroy(unitMenu.gameObject);
        Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.ACTIONTARGET);
        Camera.main.GetComponent<CameraControllerPC>().setActionTarget(new CameraControllerPC.targetSelectMethod(endMoveUnit));
    }

    public void endMoveUnit(Tile endLocation)
    {
       System.String name = transform.parent.gameObject.name;
       int commaBreak = name.IndexOf(',');
       int x = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
       int y = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
       transform.parent.parent.GetComponent<BoardManager>().unmarkTilesInRadius(baseSpeed, y, x);
       Camera.main.GetComponent<CameraControllerPC>().changeMode(CameraControllerPC.inputModes.TILESELECT);
       if (transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(transform.parent.GetComponent<Tile>(), endLocation) > baseSpeed)
       {
           name = endLocation.gameObject.name;
           commaBreak = name.IndexOf(',');
           int x2 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
           int y2 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

           if (x < baseSpeed)
           {
               int testX = x + 100;
               int newDistance = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(testX, y, x2, y2);
               if(newDistance <= baseSpeed)
               {
                   endLocation.addUnitsToTile(gameObject);
                   transform.parent.GetComponent<Tile>().removeUnit(this);
               }
           }
           if(x2 < baseSpeed)
           {
               int testX = x + 100;
               int newDistance = transform.parent.parent.GetComponent<BoardManager>().distanceBetweenTiles(x, y, testX, y2);
               if (newDistance <= baseSpeed)
               {
                   endLocation.addUnitsToTile(gameObject);
                    transform.parent.GetComponent<Tile>().removeUnit(this);
               }
           }
           //return;
       }
        transform.parent.GetComponent<Tile>().removeUnit(this);
        endLocation.addUnitsToTile(gameObject);
        
    }

    public void deleteUnit()
    {
        Destroy(unitMenu.gameObject);
        Destroy(gameObject);
    }
}
