using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int gold = 100;
    public int GPT = 0;
    public bool aiPlayer = false;
    public Color playerColor;
    public Material playerOccupiedTile;

    List<Unit> ownedUnits = new List<Unit>();

	// Use this for initialization
	void Start () {
        playerOccupiedTile = new Material(Resources.Load<Material>("Models/Materials/OccupyMat"));
        Color matColor = playerColor;
        matColor.a = 40f/255f;
        playerOccupiedTile.color = matColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void newTurn() {
        gold += GPT;
        foreach(Unit unit in ownedUnits)
        {
            unit.newTurn();
        }
    }

    public virtual void doTurn(){

    }

    public void giveUnit(Unit unit)
    {
        ownedUnits.Add(unit);
    }
}
