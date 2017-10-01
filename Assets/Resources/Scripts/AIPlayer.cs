using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player {


	// Use this for initialization
	void Start () {
        aiPlayer = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void doTurn()
    {
        foreach(Unit unit in ownedUnits)
        {
            if(unit.GetType() == typeof(Settler))
            {
                Settler currentSettler = (Settler)unit;
                currentSettler.settle();
            }
        }
        foreach(City city in ownedCities)
        {
            if(city.unitInProduction == null)
            {
                city.startProductionOnUnit("Warrior");
            }
        }
    }
}
