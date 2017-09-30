using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : BoardObject {


    //All units must set constant values for their base attack and defence
    public abstract int baseAttack { get; }
    public abstract int baseDefense { get; }
    public abstract int baseSpeed { get; }
    protected abstract int maintenanceCost { get; }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
