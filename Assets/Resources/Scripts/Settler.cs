using System.Collections;
using System.Collections.Generic;
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
    #endregion


    // Use this for initialization
    void Start () {
        Player myOwner = GameObject.Find("Player" + ownerIndex).GetComponent<Player>();
        myOwner.giveUnit(this);
        owner = myOwner;
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
            transform.parent.GetComponent<Tile>().claimTile(owner);
            Destroy(unitMenu.gameObject);
            Destroy(gameObject);
        }
    }

    public override void newTurn()
    {
        speed = baseSpeed;
    }
}
