
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
        while (grassTiles < (boardSize * boardSize * 0.35))
        {
            while (Random.Range(0, 100) <= continentP)
            {
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
            while (board[riverX, riverY].setType != Tile.tileType.Grass)
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

    public void spawnStartUnits()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i = 0; i<2; i++)
        {
            int spawnX = Random.Range(0, boardSize);
            int spawnY = Random.Range(0, boardSize);
            while (board[spawnX, spawnY].setType != Tile.tileType.Grass) {
                spawnX = Random.Range(0, boardSize);
                spawnY = Random.Range(0, boardSize);
            }
            GameObject newSettler = Instantiate((GameObject)Resources.Load("Prefabs/Settler"));
            DontDestroyOnLoad(newSettler);
            board[spawnX, spawnY].addUnitsToTile(newSettler);
        }
    }

    public Tile getTile(int x, int y)
    {
        return board[x, y];
    }

    public int distanceBetweenTiles(int col1,int col2,int row1,int row2)
    {

        int x1 = col1 - (row1 + (row1 & 1)) / 2;
        int z1 = row1;
        int y1 = -x1 - z1;

        int x2 = col2 - (row2 + (row2 & 1)) / 2;
        int z2 = row2;
        int y2 = -x2 - z2;

        return (Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2) + Mathf.Abs(z1 - z2)) / 2;
    }

    public int distanceBetweenTiles(Tile t1, Tile t2)
    {
        Debug.Log(t1.gameObject.name + " and " + t2.gameObject.name);

        System.String name = t1.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x1 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y1 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));

        name = t2.gameObject.name;
        commaBreak = name.IndexOf(',');
        int x2 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y2 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        return distanceBetweenTiles(x1, x2, y1, y2);
    }

    public void markTilesInRadius(int radius, int centerX, int centerY)
    {
        MeshRenderer currentMesh = board[centerX, centerY].GetComponent<MeshRenderer>();
        if (radius < 0) return;
        Material[] newMaterials = new Material[2];
        newMaterials[0] = currentMesh.material;
        newMaterials[1] = (Material)Resources.Load("Models/Materials/Highlight");
        currentMesh.materials = newMaterials;


        int xPlus = (centerX + 1 < boardSize) ? (centerX + 1) : 0;
        int xMinus = (centerX - 1 >= 0) ? (centerX - 1) : (boardSize - 1);
        int yPlus = (centerY + 1 < boardSize) ? (centerY + 1) : 0;
        int yMinus = (centerY - 1 >= 0) ? (centerY - 1) : (boardSize - 1);

        if (centerX % 2 == 0)
        {
            markTilesInRadius(radius - 1, xPlus, centerY);
            markTilesInRadius(radius - 1, xPlus, yPlus);
            markTilesInRadius(radius - 1, centerX, yPlus);
            markTilesInRadius(radius - 1, xMinus, yPlus);
            markTilesInRadius(radius - 1, xMinus, centerY);
            markTilesInRadius(radius - 1, centerX, yMinus);
        }
        else
        {
            markTilesInRadius(radius - 1, xPlus, yMinus);
            markTilesInRadius(radius - 1, xPlus, centerY);
            markTilesInRadius(radius - 1, centerX, yPlus);
            markTilesInRadius(radius - 1, xMinus, centerY);
            markTilesInRadius(radius - 1, xMinus, yMinus);
            markTilesInRadius(radius - 1, centerX, yMinus);
        }

    }

    public void unmarkTilesInRadius(int radius, int centerX, int centerY)
    {
        MeshRenderer currentMesh = board[centerX, centerY].GetComponent<MeshRenderer>();
        if (radius < 0) return;
        Material[] newMaterials = new Material[1];
        newMaterials[0] = currentMesh.material;
        currentMesh.materials = newMaterials;

        int xPlus = (centerX + 1 < boardSize) ? (centerX + 1) : 0;
        int xMinus = (centerX - 1 >= 0) ? (centerX - 1) : (boardSize - 1);
        int yPlus = (centerY + 1 < boardSize) ? (centerY + 1) : 0;
        int yMinus = (centerY - 1 >= 0) ? (centerY - 1) : (boardSize - 1);

        if (centerX % 2 == 0)
        {
            unmarkTilesInRadius(radius - 1, xPlus, centerY);
            unmarkTilesInRadius(radius - 1, xPlus, yPlus);
            unmarkTilesInRadius(radius - 1, centerX, yPlus);
            unmarkTilesInRadius(radius - 1, xMinus, yPlus);
            unmarkTilesInRadius(radius - 1, xMinus, centerY);
            unmarkTilesInRadius(radius - 1, centerX, yMinus);
        }
        else
        {
            unmarkTilesInRadius(radius - 1, xPlus, yMinus);
            unmarkTilesInRadius(radius - 1, xPlus, centerY);
            unmarkTilesInRadius(radius - 1, centerX, yPlus);
            unmarkTilesInRadius(radius - 1, xMinus, centerY);
            unmarkTilesInRadius(radius - 1, xMinus, yMinus);
            unmarkTilesInRadius(radius - 1, centerX, yMinus);
        }
    }
}
