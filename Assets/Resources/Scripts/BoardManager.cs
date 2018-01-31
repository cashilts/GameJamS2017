
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Priority_Queue;

public class BoardManager : Singleton<BoardManager> {
   
    public const float tileWidth = 1.7f;
    public const float tileHeight = 1.5f;
    public const int boardSize = 100;


    int grassTiles = 0;
    Tile[,] board = new Tile[boardSize,boardSize];  
    public enum tileDirections { TL,TR,R,BR,BL,L};

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// Returns neighboring tile to the tile at (x,y) based on the direction
    /// </summary>
    /// <param name="direction">Direction of neighbor to return</param>
    /// <param name="x">X coord of current tile</param>
    /// <param name="y">Y coord of current tile</param>
    /// <returns></returns>
    public Tile getNeighborByDirection(tileDirections direction, int x, int y)
    {
        int xPlus = (x + 1 < boardSize) ? (x + 1) : 0;
        int xMinus = (x - 1 >= 0) ? (x - 1) : (boardSize - 1);
        int yPlus = (y + 1 < boardSize) ? (y + 1) : 0;
        int yMinus = (y - 1 >= 0) ? (y - 1) : (boardSize - 1);

        if (x % 2 == 0)
        {
            if(direction == tileDirections.TL)
            {
                return board[xMinus, y];
            }
            else if(direction == tileDirections.TR)
            {
                return board[xMinus, yPlus];
            }
            else if(direction == tileDirections.R)
            {
                return board[x, yPlus];
            }
            else if(direction == tileDirections.BR)
            {
                return board[xPlus, yPlus];
            }
            else if(direction == tileDirections.BL)
            {
                return board[xPlus, y];
            }
            else if (direction == tileDirections.L)
            {
                return board[x, yMinus];
            }
        }
        else
        {
            if (direction == tileDirections.TL)
            {
                return board[xMinus, yMinus];
            }
            else if (direction == tileDirections.TR)
            {
                return board[xMinus, y];
            }
            else if (direction == tileDirections.R)
            {
                return board[x, yPlus];
            }
            else if (direction == tileDirections.BR)
            {
                return board[xPlus, y];
            }
            else if (direction == tileDirections.BL)
            {
                return board[xPlus, yMinus];
            }
            else if (direction == tileDirections.L)
            {
                return board[x, yMinus];
            }
        }
        return board[x, y];
    }


    /// <summary>
    /// Returns neighbor based on the direction and the current tile
    /// </summary>
    /// <param name="direction">Direction of neighbor to return</param>
    /// <param name="tile">Current tile</param>
    /// <returns></returns>
    public Tile getNeighborByDirection(tileDirections direction, Tile tile)
    {
        System.String name = tile.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x1 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y1 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        return getNeighborByDirection(direction,y1,x1);
    }

    /// <summary>
    /// Loads board state from an XML file
    /// </summary>
    /// <param name="fileName">Filename of a valid XML save</param>
    public void loadBoardState(string fileName)
    {
        initBoard();

        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);
        XmlNode gameNode = doc.FirstChild;
        XmlNode focusNode = gameNode.FirstChild;

        CameraController.Instance.loadCamera(focusNode);
        focusNode = focusNode.NextSibling;

        GameManager.Instance.loadState(focusNode);
        focusNode = focusNode.NextSibling;
        for(int i = 0; i<GameManager.Instance.numPlayers; i++)
        {
            GameManager.Instance.getPlayer(i).loadPlayer(focusNode);
            focusNode = focusNode.NextSibling;
        }
        XmlNode tileNode = focusNode.FirstChild;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i, j].loadTile(tileNode);
                tileNode = tileNode.NextSibling;
            }
        }
    }
    

    /// <summary>
    /// Saves board state into an XML file
    /// </summary>
    public void saveBoardState(string filename)
    {
        
        string directory = System.IO.Directory.GetCurrentDirectory();
        if (!System.IO.Directory.Exists(directory + "\\LocalSaves"))
        {
            System.IO.Directory.CreateDirectory(directory + "\\LocalSaves");
        }



        XmlDocument doc = new XmlDocument();
        XmlElement game = doc.CreateElement("Game");
        game.AppendChild(CameraController.Instance.saveCamera(ref doc));
        game.AppendChild(GameManager.Instance.saveState(ref doc));

        Player[] allPlayers = GameObject.FindObjectsOfType<Player>();
        foreach(Player p in allPlayers)
        {
            game.AppendChild(p.savePlayer(ref doc));
        }

        

        XmlElement boardElement = doc.CreateElement("Board");
        for(int i = 0; i < boardSize; i++)
        {
            for(int j = 0; j< boardSize; j++)
            { 
                boardElement.AppendChild(board[i,j].saveTile(ref doc));
            }
        }
        game.AppendChild(boardElement);
        doc.AppendChild(game);
        doc.Save(directory + "\\LocalSaves\\" + filename + ".save");
        
    }


    /// <summary>
    /// Generates two ice poles on the top and bottom of the map
    /// </summary>
    public void GenerateIcePoles()
    {
        int iceX = (int)Random.Range(0.4f * boardSize, 0.6f * boardSize);
        GenerateIce(iceX, 0, 100);
        iceX = (int)Random.Range(0.4f * boardSize, 0.6f * boardSize);
        GenerateIce(iceX, boardSize - 1, 100);
    }

    public void initBoard()
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
    }


    /// <summary>
    /// Generates continents on the map, favouring a large land mass and multiple smaller ones
    /// </summary>
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


    /// <summary>
    /// Surrounds all land with shallow water
    /// </summary>
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
                        board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (board[xPlus, y].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (board[x, yPlus].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (board[x, yMinus].setType == Tile.tileType.Grass)
                    {
                        board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                        board[x, y].setType = Tile.tileType.ShallowWater;
                    }
                    else if (x % 2 == 1)
                    {
                        if (board[xMinus, yMinus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                        else if (board[xPlus, yMinus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                    }
                    else
                    {
                        if (board[xPlus, yPlus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                        else if (board[xMinus, yPlus].setType == Tile.tileType.Grass)
                        {
                            board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/SWater");
                            board[x, y].setType = Tile.tileType.ShallowWater;
                        }
                    }

                }
            }
        }
    }


    /// <summary>
    /// Places rivers on the map
    /// </summary>
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

    /// <summary>
    /// Function for handling generating a river
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="start"></param>
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
        board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/GrassTex");
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
        board[x, y].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Models/Materials/IceTex");
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
        GameManager manager = GameManager.Instance;
        for(int i = 0; i<2; i++)
        {
            int spawnX = Random.Range(0, boardSize);
            int spawnY = Random.Range(0, boardSize);
            while (board[spawnX, spawnY].setType != Tile.tileType.Grass) {
                spawnX = Random.Range(0, boardSize);
                spawnY = Random.Range(0, boardSize);
            }
            GameObject newSettler = Instantiate(Resources.Load<GameObject>("Prefabs/Settler"));
            newSettler.name = "Settler" + i;
            newSettler.GetComponent<Settler>().ownerIndex = i;
            if(i == 0)
            {
                newSettler.transform.Find("Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", new Color(255, 0, 0));
            }
            else
            {
                newSettler.transform.Find("Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", new Color(0, 255, 0));
            }
            DontDestroyOnLoad(newSettler);
            board[spawnX, spawnY].addUnitsToTile(newSettler.GetComponent<Settler>());
        }
    }

    public void initializeResources()
    {
        for(int i =0; i<boardSize; i++)
        {
            for(int j = 0; j<boardSize; j++)
            {
                if(board[i,j].setType == Tile.tileType.Ice)
                {
                    board[i, j].wealth = 0;
                    board[i, j].food = 0;
                }
                else if(board[i,j].setType == Tile.tileType.Grass)
                {
                    int wealthChance = Random.Range(0, 100);
                    if(wealthChance < 25)
                    {
                        board[i, j].wealth = 1;
                    }
                    else if(wealthChance < 75)
                    {
                        board[i, j].wealth = 2;
                    }
                    else
                    {
                        board[i, j].wealth = 3;
                    }
                    int foodChance = Random.Range(0, 100);
                    if (foodChance < 25)
                    {
                        board[i, j].food = 0;
                    }
                    else if(foodChance < 75)
                    {
                        board[i, j].food = 1;
                    }
                    else
                    {
                        board[i, j].food = 2;
                    }
                }
                else if(board[i,j].setType == Tile.tileType.ShallowWater)
                {
                    board[i, j].wealth = 0;
                    board[i, j].food = 0;
                }
                else
                {
                    board[i, j].wealth = 0;
                    board[i, j].food = 0;
                }
            }
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

    public void unmarkTilesInRadius(int radius, int centerX, int centerY)
    {
        MeshRenderer currentMesh = board[centerX, centerY].GetComponent<MeshRenderer>();
        if (radius < 0) return;
        int materialCount = currentMesh.materials.Length;
        List<Material> materialList = new List<Material>();
        for (int i = 0; i < materialCount; i++)
        {
            if(currentMesh.materials[i].name != "Highlight (Instance)")
            {
                materialList.Add(currentMesh.materials[i]);
            }
        }
        currentMesh.materials = materialList.ToArray();
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

        public void claimTile(Tile toClaim,Player owner, int radius)
    {

        System.String name = toClaim.gameObject.name;
        int commaBreak = name.IndexOf(',');
        int x1 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
        int y1 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
        claimTile(y1, x1, owner, radius);
        
    }

    public void claimTile(int x, int y, Player owner, int radius)
    {
        if (board[x, y].owner == null && radius > 0)
        {
            board[x, y].owner = owner;
            owner.GPT += board[x, y].wealth;
            int materialCount = board[x,y].GetComponent<MeshRenderer>().materials.Length;
            List<Material> materialList = new List<Material>();
            for (int i = 0; i < materialCount; i++)
            {
                materialList.Add(board[x,y].GetComponent<MeshRenderer>().materials[i]);
            }
            materialList.Add(owner.playerOccupiedTile);
            board[x,y].GetComponent<MeshRenderer>().materials = materialList.ToArray();

            int xPlus = (x + 1 < boardSize) ? (x + 1) : 0;
            int xMinus = (x - 1 >= 0) ? (x - 1) : (boardSize - 1);
            int yPlus = (y + 1 < boardSize) ? (y + 1) : 0;
            int yMinus = (y - 1 >= 0) ? (y - 1) : (boardSize - 1);

            if (x % 2 == 0)
            {
                claimTile(xPlus, y,owner, radius - 1);
                claimTile(xPlus, yPlus,owner,radius - 1);
                claimTile(x, yPlus,owner, radius - 1);
                claimTile(xMinus, yPlus,owner, radius - 1);
                claimTile(xMinus, y,owner, radius - 1);
                claimTile(x, yMinus,owner, radius - 1);
            }
            else
            {
                claimTile(xPlus, yMinus,owner, radius - 1);
                claimTile(xPlus, y,owner, radius - 1);
                claimTile(x, yPlus,owner, radius - 1);
                claimTile(xMinus, y,owner, radius - 1);
                claimTile(xMinus, yMinus,owner, radius - 1);
                claimTile(x, yMinus,owner, radius - 1);
            }
        }

    }

    public void markMovement(List<Tile.tileType> blockType, int speed, Tile unitTile, Tile currentTile)
    {
        if (speed < 0 || (unitTile != currentTile && blockType.Contains(currentTile.setType))) return;

        for (int i = 0; i < 6; i++)
        {
            markMovement(blockType, speed - 1, unitTile, getNeighborByDirection((tileDirections)i, currentTile));
        }
        if (unitTile != currentTile)
        {
            System.String name = currentTile.gameObject.name;
            int commaBreak = name.IndexOf(',');
            int x1 = System.Convert.ToInt32(name.Substring(4, commaBreak - 4));
            int y1 = System.Convert.ToInt32(name.Substring(commaBreak + 1, name.Length - 1 - commaBreak));
            
            MeshRenderer currentMesh = board[y1, x1].GetComponent<MeshRenderer>();
            int materialCount = currentMesh.materials.Length;
            List<Material> materialList = new List<Material>();
            for (int i = 0; i < materialCount; i++)
            {
                if (currentMesh.materials[i].name == "Highlight (Instance)") return;
                materialList.Add(currentMesh.materials[i]);
            }
            materialList.Add(Resources.Load<Material>("Models/Materials/Highlight"));
            currentMesh.materials = materialList.ToArray();
        }
       
    }
    /// <summary>
    /// Get the path from start to end
    /// </summary>
    /// <param name="start">Tile to start path from</param>
    /// <param name="end">Tile to end path at</param>
    /// <returns></returns>
    public Stack<Tile> getPath(Tile start, Tile end)
    {
        SimplePriorityQueue<Tile> frontier = new SimplePriorityQueue<Tile>();
        frontier.Enqueue(start, 0);
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> costSoFar = new Dictionary<Tile, int>();
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);

        while(!(frontier.Count == 0)){
            Tile current = frontier.Dequeue();
            if (current == end) break;

            for(int i =0; i<6; i++)
            {
                Tile next = getNeighborByDirection((tileDirections)i, current);
                int tileCost = 1;
                if (next.setType == Tile.tileType.Ice || next.setType == Tile.tileType.ShallowWater || next.setType == Tile.tileType.DeepWater) tileCost = 100;



                int newCost = costSoFar[current] + tileCost;
                if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    
                    if (costSoFar.ContainsKey(next)) costSoFar[next] = newCost;
                    else costSoFar.Add(next, newCost);

                    int priority = newCost + distanceBetweenTiles(end, next);
                    if (frontier.Contains(next)) frontier.UpdatePriority(next, priority);
                    else frontier.Enqueue(next, priority);

                    if (cameFrom.ContainsKey(next)) cameFrom[next] = current;
                    else cameFrom.Add(next, current);
                }
            }
        }

        Stack<Tile> path = new Stack<Tile>();
        Tile nextInPath = end;
        path.Push(nextInPath);
        while(nextInPath != start)
        {
            nextInPath = cameFrom[nextInPath];
            path.Push(nextInPath);
        }
        return path;
    }


    /// <summary>
    /// Checks the lowest cost path from start to end and sees if it is within the max movement
    /// </summary>
    /// <param name="start">The tile the path begins from</param>
    /// <param name="end">The tile the path ends on</param>
    /// <param name="maxMove">Maximum expendible movement</param>
    /// <returns></returns>
    public bool isMovePossible(Tile start, Tile end, Unit currentUnit)
    {
        
        SimplePriorityQueue<Tile> frontier = new SimplePriorityQueue<Tile>();
        frontier.Enqueue(start, 0);
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> costSoFar = new Dictionary<Tile, int>();
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);

        while (!(frontier.Count == 0))
        {
            Tile current = frontier.Dequeue();
            if (current == end) break;

            for (int i = 0; i < 6; i++)
            {
                Tile next = getNeighborByDirection((tileDirections)i, current);
                int tileCost = 1;
                if (!currentUnit.allowedTiles.Contains(next.setType)) tileCost = 100;



                int newCost = costSoFar[current] + tileCost;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {

                    if (costSoFar.ContainsKey(next)) costSoFar[next] = newCost;
                    else costSoFar.Add(next, newCost);

                    int priority = newCost + distanceBetweenTiles(end, next);
                    if (frontier.Contains(next)) frontier.UpdatePriority(next, priority);
                    else frontier.Enqueue(next, priority);

                    if (cameFrom.ContainsKey(next)) cameFrom[next] = current;
                    else cameFrom.Add(next, current);
                }
            }
        }

        return (currentUnit.speed < costSoFar[end]);
    }
}
