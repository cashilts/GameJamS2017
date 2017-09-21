using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public const float tileWidth = 1.7f;
    public const float tileHeight = 1.5f;
    public const int boardSize = 100;
    int grassTiles = 0;
    Tile[,] board = new Tile[boardSize,boardSize];  

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void GenerateIcePoles()
    {
        float startX = (boardSize / 2) * tileWidth * -1;
        float startY = (boardSize / 2) * tileHeight * -1;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GameObject newTile = GameObject.Instantiate(newTile = (GameObject)Resources.Load("Prefabs/DeepWaterTex"));
                newTile.name = "tile" + j + "," + i;
                newTile.transform.position = new Vector3(startX + (tileWidth * j), 0, startY + (tileHeight * i));
                newTile.transform.parent = this.transform;
                board[i, j] = newTile.GetComponent<Tile>();
            }
            if (i % 2 == 0)
            {
                startX -= tileWidth / 2;
            }
            else
            {
                startX += tileWidth / 2;
            }
        }
        int iceX = (int)Random.Range(0.4f * boardSize, 0.6f * boardSize);
        GenerateIce(iceX, 0, 100);
        iceX = (int)Random.Range(0.4f * boardSize, 0.6f * boardSize);
        GenerateIce(iceX, boardSize - 1, 100);
    }

    public void GenerateContinents()
    {
        int continentP = 100;
        int continentNum = 1;
        int continentMax = 1;
        Debug.Log("StartGen");
        while (grassTiles < (boardSize * boardSize * 0.35))
        {
            Debug.Log("Not enough Grass");
            while (Random.Range(0, 100) <= continentP)
            {
                Debug.Log("Making " + continentNum + " continents");
                while (continentNum != 0)
                {
                    int x = (int)Random.Range(0, boardSize - 1);
                    int y = (int)Random.Range(0, boardSize - 1);
                    if (board[x, y].setType == Tile.tileType.DeepWater)
                    {
                        GenerateContinent(x, y, continentP);
                        continentNum--;
                    }
                }
                if (Random.Range(0, 100) <= 100 - continentP)
                {
                    continentMax++;
                }
                continentNum = continentMax;
                continentP -= 12;
            }
            continentP += 3;
        }
    }

    public void PlaceShallowWater()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                if (board[x, y].setType == Tile.tileType.DeepWater)
                {
                    int xPlus = (x + 1 < boardSize) ? (x + 1) : 0;
                    int xMinus = (x - 1 >= 0) ? (x - 1) : (boardSize - 1);
                    int yPlus = (y + 1 < boardSize) ? (y + 1) : 0;
                    int yMinus = (y - 1 >= 0) ? (y - 1) : (boardSize - 1);

                    if (board[xMinus, y].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (board[xPlus, y].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (board[x, yPlus].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (board[x, yMinus].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (x % 2 == 1)
                    {
                        if (board[xMinus, yMinus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                        else if (board[xPlus, yMinus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                    }
                    else
                    {
                        if (board[xPlus, yPlus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                        else if (board[xMinus, yPlus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                    }

                }
            }
        }
    }

    public void PlaceRivers()
    {
        for (int i = 0; i < 5; i++)
        {
            int riverX = Random.Range(0, boardSize);
            int riverY = Random.Range(0, boardSize);
            while (board[riverX, riverY].setType != Tile.tileType.ShallowWater)
            {
                riverX = Random.Range(0, boardSize);
                riverY = Random.Range(0, boardSize);
            }
            board[riverX, riverY].hasWater = true;
            GenerateRiver(riverX, riverY);
        }
    }

    void GenerateRiver(int x, int y, bool start=true){
        const int maxTries = 12;

        int tries = 0;

        int xPlus = (x + 1 < boardSize) ? (x + 1) : 0;
        int xMinus = (x - 1 >= 0) ? (x - 1) : (boardSize - 1);
        int yPlus = (y + 1 < boardSize) ? (y + 1) : 0;
        int yMinus = (y - 1 >= 0) ? (y - 1) : (boardSize - 1);

        bool generated = false;
        int startTry = -1;

        int nextTile = Random.Range(0, 6);

        Tile nextRiverTile = board[x,y];
        int nextX=0, nextY=0;
        while (!generated)
        {
            switch (nextTile)
            {
                case 0:
                    if (x % 2 == 0)
                    {
                        nextRiverTile = board[xPlus, y];
                        nextX = xPlus;
                        nextY = y;
                    }
                    else
                    {
                        nextRiverTile = board[xPlus, yMinus];
                        nextX = xPlus;
                        nextY = yMinus;
                        
                    }
                    break;
                case 1:
                    if (x % 2 == 0) {
                        nextRiverTile = board[xPlus, yPlus];
                        nextX = xPlus;
                        nextY = yPlus;
                    }
                    else
                    {
                        nextRiverTile = board[xPlus, y];
                        nextX = xPlus;
                        nextY = y;
                    }
                    break;
                case 2:
                    nextRiverTile = board[x,yPlus];
                    nextX = x;
                    nextY = yPlus;
                    break;
                case 3:
                    if (x % 2 == 0) {
                        nextRiverTile = board[xMinus, yPlus];
                        nextX = xMinus;
                        nextY = yPlus;
                    }
                    else
                    {
                        nextRiverTile = board[xMinus, y];
                        nextX = xMinus;
                        nextY = y;
                    }
                    break;
                case 4:
                    if (x % 2 == 0) {
                        nextRiverTile = board[xMinus, y];
                        nextX = xMinus;
                        nextY = y;
                    }
                    else
                    {
                        nextRiverTile = board[xMinus, yMinus];
                        nextX = xMinus;
                        nextY = yMinus;
                    }
                    break;
                case 5:
                    nextRiverTile = board[x, yMinus];
                    nextX = x;
                    nextY = yMinus;
                    break;
            }
            if (nextRiverTile.setType == Tile.tileType.Grass && !nextRiverTile.hasWater) {
                Debug.Log(nextX+ " " + nextY);
                nextRiverTile.hasWater = true;
                nextRiverTile.waterDirections[(nextTile + 3) % 6] = true;
                board[x, y].waterDirections[nextTile] = true;
                if (!start)
                {   
                    board[x, y].getWaterTexture();
                }
                GenerateRiver(nextX, nextY, false);
                generated = true;
            } else if(nextRiverTile.setType == Tile.tileType.ShallowWater && !start && !nextRiverTile.hasWater)
            {
                board[x, y].waterDirections[nextTile] = true;
                board[x, y].getWaterTexture();
                generated = true;
            }
            else
            {
                nextTile = Random.Range(0, 6);
                if (tries == maxTries) {
                    generated = true;
                }
                tries++;
            }
        }
        
    }

    void GenerateContinent(int x, int y, float p){
        grassTiles++;
        board[x, y].setType = Tile.tileType.Grass;
        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/GrassTex");
        if (Random.Range(0, 100) <= p)
        {
            if (x - 1 >= 0)
            {
                if (board[x - 1, y].setType == Tile.tileType.DeepWater ) GenerateContinent(x - 1, y, p - 0.81f);
            }
            else
            {
                if (board[boardSize-1, y].setType == Tile.tileType.DeepWater) GenerateContinent(boardSize-1, y, p - 0.81f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (x + 1 <= boardSize - 1)
            {
                if (board[x + 1, y].setType == Tile.tileType.DeepWater) GenerateContinent(x + 1, y, p - 0.62f);
            }
            else
            {
                if (board[0, y].setType == Tile.tileType.DeepWater) GenerateContinent(0, y, p - 0.62f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (y + 1 <= boardSize - 1)
            {
                if (board[x, y + 1].setType == Tile.tileType.DeepWater) GenerateContinent(x, y + 1, p - 0.79f);
            }
            else
            {
                if (board[x, 0].setType == Tile.tileType.DeepWater) GenerateContinent(x, 0, p - 0.79f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (y - 1 >= 0)
            {
                if (board[x, y - 1].setType == Tile.tileType.DeepWater) GenerateContinent(x, y - 1, p - 0.70f);
            }
            else
            {
                if (board[x, boardSize-1].setType == Tile.tileType.DeepWater) GenerateContinent(x, boardSize-1, p - 0.70f);
            }
        }
    }

    void GenerateIce(int x, int y, float p) {
        board[x, y].setType = Tile.tileType.Ice;
        board[x, y].GetComponent<MeshRenderer>().material = (Material)Resources.Load("Models/Materials/IceTex");
        if (Random.Range(0, 100) <= p)
        {
            if (x - 1 >= 0) {
                if (board[x - 1, y].setType != Tile.tileType.Ice) GenerateIce(x - 1, y, p - 0.005f);
            }
            else
            {
                if (board[boardSize - 1, y].setType != Tile.tileType.Ice) GenerateIce(boardSize - 1, y, p - 0.005f);
            }
        }
        if (Random.Range(0, 100) <= p)
        {
            if (x + 1 <= boardSize-1 ) {
                if (board[x + 1, y].setType != Tile.tileType.Ice) GenerateIce(x + 1, y, p - 0.005f);
            }
            else
            {
                if (board[0, y].setType != Tile.tileType.Ice) GenerateIce(0, y, p - 0.005f);
            }
        }
        if (Random.Range(0, 100) <= p) {
            if (y + 1 <= boardSize - 1){
                if (board[x, y + 1].setType != Tile.tileType.Ice) GenerateIce(x, y + 1, p - 15);
            }
        }
        if (Random.Range(0, 100) <= p) {
            if (y - 1 >= 0) {
                if (board[x, y - 1].setType != Tile.tileType.Ice) GenerateIce(x, y - 1, p-15);
            }
        }
    }

    public Tile getTile(int x, int y)
    {
        return board[x, y];
    }
}
