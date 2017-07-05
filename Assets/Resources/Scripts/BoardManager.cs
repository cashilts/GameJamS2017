using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public const float tileWidth = 1.7f;
    public const float tileHeight = 1.5f;
    public const int boardSize = 100;
    Tile[,] board = new Tile[boardSize,boardSize];  

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
                board[i, j] = newTile.GetComponent<Tile>();
            }
            if (i % 2 == 0){
                startX -= tileWidth / 2;
            }
            else {
                startX += tileWidth / 2;
            }
        }
        int iceX = (int)Random.Range(0.4f * boardSize, 0.6f * boardSize);
        GenerateIce(iceX, 0, 100);
        iceX = (int)Random.Range(0.4f * boardSize, 0.6f * boardSize);
        GenerateIce(iceX, boardSize - 1, 100);

        int continentP = 100;
        while (Random.Range(0, 100) <= continentP) {
            int x = (int)Random.Range(boardSize * 0.3f, boardSize * 0.7f);
            int y = (int)Random.Range(0, boardSize-1);
            if (board[x, y].setType == Tile.tileType.DeepWater) {
                GenerateContinent(x, y, continentP);
                continentP -= 21;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    void GenerateContinent(int x, int y, float p){
        board[x, y].setType = Tile.tileType.Grass;
        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/GrassTex");
        if (Random.Range(0, 100) <= p)
        {
            if (x - 1 >= 0)
            {
                if (board[x - 1, y].setType == Tile.tileType.DeepWater ) GenerateContinent(x - 1, y, p - 0.21f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (x + 1 <= boardSize - 1)
            {
                if (board[x + 1, y].setType == Tile.tileType.DeepWater) GenerateContinent(x + 1, y, p - 0.22f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (y + 1 <= boardSize - 1)
            {
                if (board[x, y + 1].setType == Tile.tileType.DeepWater) GenerateContinent(x, y + 1, p - 0.25f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (y - 1 >= 0)
            {
                if (board[x, y - 1].setType == Tile.tileType.DeepWater) GenerateContinent(x, y - 1, p - 0.20f);
            }
        }
    }

    void GenerateIce(int x, int y, float p) {
        board[x, y].setType = Tile.tileType.Ice;
        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/IceTex");
        if (Random.Range(0, 100) <= p)
        {
            if (x - 1 >= 0) {
                if (board[x - 1, y].setType != Tile.tileType.Ice) GenerateIce(x - 1, y, p - 0.19f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (x + 1 <= boardSize-1) {
                if (board[x + 1, y].setType != Tile.tileType.Ice) GenerateIce(x + 1, y, p - 0.19f);
            }
        }
        if (Random.Range(0, 100) <= p) {
            if (y + 1 <= boardSize - 1){
                if (board[x, y + 1].setType != Tile.tileType.Ice) GenerateIce(x, y + 1, p - 10);
            }
        }
        if (Random.Range(0, 100) <= p) {
            if (y - 1 >= 0) {
                if (board[x, y - 1].setType != Tile.tileType.Ice) GenerateIce(x, y - 1, p-5);
            }
        }
    }
}
