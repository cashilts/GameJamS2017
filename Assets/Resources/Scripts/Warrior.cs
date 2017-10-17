using System.Collections;
using System.Collections.Generic;
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
            return "attack";
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
        unitMenu.addOptionToMenu(new RadialButton.passDelegateNoValue(attackStart), Resources.Load<Sprite>("Images/attack"));
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
            transform.parent.parent.GetComponent<BoardManager>().markMovement(bannedList, attackRange, transform.parent.GetComponent<Tile>(), transform.parent.GetComponent<Tile>());
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
}
