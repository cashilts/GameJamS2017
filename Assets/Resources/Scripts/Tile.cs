using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum tileType {DeepWater, ShallowWater,Ice, Grass};
    public tileType setType = tileType.DeepWater;

    bool[] waterDirections = new bool[6];
    static readonly string[] directionNames = { "TL", "TR", "R", "BR", "BL", "L" };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
