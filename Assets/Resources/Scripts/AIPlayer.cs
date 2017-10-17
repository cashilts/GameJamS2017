using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player {


	// Use this for initialization
	void Start () {
        aiPlayer = true;
        playerOccupiedTile = new Material(Resources.Load<Material>("Models/Materials/OccupyMat"));
        Color matColor = playerColor;
        matColor.a = 40f / 255f;
        playerOccupiedTile.color = matColor;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void doTurn()
    {
        for(int i = 0; i<ownedUnits.Count; i++)
        {
            if (ownedUnits[i].GetType() == typeof(Settler))
            {
                Settler currentSettler = (Settler)ownedUnits[i];
                currentSettler.settle();
                i--;
                continue;
            }
            else if(ownedUnits[i].GetType() == typeof(Warrior))
            {
                while(ownedUnits[i].speed != 0)
                {
                    ownedUnits[i].startMoveUnit();
                    ownedUnits[i].endMoveUnit(ownedUnits[i].transform.parent.parent.GetComponent<BoardManager>().getNeighborByDirection((BoardManager.tileDirections)Random.Range(0, 6), ownedUnits[i].transform.parent.GetComponent<Tile>()));
                }
            }
        }
        for(int i = 0; i < ownedCities.Count; i++)
        {
            if (ownedCities[i].unitInProduction == null)
            {
                ownedCities[i].startProductionOnUnit("Warrior");
            }
        }
    }
}
