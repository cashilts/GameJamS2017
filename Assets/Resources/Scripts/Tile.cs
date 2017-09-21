using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum tileType {DeepWater, ShallowWater,Ice, Grass};
    public tileType setType = tileType.DeepWater;

    public bool[] waterDirections = new bool[6];
    public bool hasWater = false;
    static readonly string[] directionNames = { "TL", "TR", "R", "BR", "BL", "L" };

    public List<BoardObject> objectsOnTile;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void getWaterTexture() {
        string waterTexturePath = "Models/Textures/newRiver";
        for (int i = 0; i < 6; i++) {
            if (waterDirections[i]) {
                waterTexturePath += directionNames[i];
            }
        }
        waterTexturePath += "1";
        GetComponent<MeshRenderer>().material.mainTexture = (Texture)Resources.Load(waterTexturePath);
    }
}
