using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int gold = 0;
    public int GPT = 0;
    public bool aiPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void newTurn() {
        gold += GPT;
    }

    public virtual void doTurn(){

    }
}
