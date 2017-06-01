using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public const float tileWidth = 1.7f;
    public const float tileHeight = 1.5f;
    public const int boardSize = 100;

	// Use this for initialization
	void Start () {
        float startX = (boardSize / 2) * tileWidth * -1;
        float startY = (boardSize / 2) * tileHeight * -1;
        for (int i = 0; i < boardSize; i++) {
            for (int j = 0; j < boardSize; j++){
                GameObject newTile = GameObject.Instantiate(newTile = (GameObject)Resources.Load("Prefabs/DeepWaterTex"));
                newTile.name = "tile" + j + "," + i;
                newTile.transform.position = new Vector3(startX + (tileWidth * j), 0, startY + (tileHeight * i));
                newTile.transform.parent = this.transform;
            }
            if (i % 2 == 0){
                startX -= tileWidth / 2;
            }
            else {
                startX += tileWidth / 2;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
