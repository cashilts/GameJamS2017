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

    public override int baseSpeed
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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onSelect()
    {
        Debug.Log("Selected");
    }
}
