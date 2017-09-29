using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWrapper : MonoBehaviour {
    Vector3 gridCenter = new Vector3(0, 0, 0);
    int gridOffsetX = 0;
    int gridOffsetY = 0;
    List<List<GameObject>> boardReferance = new List<List<GameObject>>();

	// Use this for initialization
	void Start () {
        GameObject boardSource = GameObject.Find("BoardGenerator");
        for (int i = 0; i < BoardManager.boardSize; i++) {
            List<GameObject> tempList = new List<GameObject>();
            for (int j = 0; j < BoardManager.boardSize; j++) {
                GameObject tempTile = boardSource.transform.Find("tile" + i +","+j).gameObject;
                tempList.Add(tempTile);
            }
            boardReferance.Add(tempList);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.x > ((BoardManager.tileHeight * BoardManager.boardSize) / 2))
        {
            transform.SetPositionAndRotation(new Vector3((BoardManager.tileHeight * BoardManager.boardSize) / 2, transform.position.y, transform.position.z), transform.rotation);
        }
        if ((transform.position - gridCenter).z > BoardManager.tileHeight)
        {
            gridCenter.z += BoardManager.tileHeight;
            gridOffsetY += 1;
            ShiftTilesY(1);
        }
        else if ((transform.position - gridCenter).z < BoardManager.tileHeight * -1)
        {
            gridCenter.z -= BoardManager.tileHeight;
            gridOffsetY -= 1;
            ShiftTilesY(-1);
        }
    }

    void ShiftTilesX(int direction) {
        int actualShift = (gridOffsetX % BoardManager.boardSize);
        int tileSelect = (direction == 1) ? 1 : 0;
        if (actualShift > 0){
            for (int i = 0; i < BoardManager.boardSize; i++) {
                GameObject tileToShift = boardReferance[actualShift - tileSelect][i];
                tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
            }
        }
        else if (actualShift < 0){
            for (int i = 0; i < BoardManager.boardSize; i++){
                GameObject tileToShift = boardReferance[BoardManager.boardSize + actualShift - tileSelect][i];
                tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
            }
        }
        else{
            if (direction < 0){
                for (int i = 0; i < BoardManager.boardSize; i++){
                    GameObject tileToShift = boardReferance[0][i];
                    tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
                }
            }
            else{
                for (int i = 0; i < BoardManager.boardSize; i++)    {
                    GameObject tileToShift = boardReferance[BoardManager.boardSize - 1][i];
                    tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
                }
            }
        }
    }

    void ShiftTilesY(int direction) {
        int actualShift = gridOffsetY % BoardManager.boardSize;
        int tileSelect = (direction == 1) ? 1 : 0;
        if (actualShift > 0)
        {
            for (int i = 0; i < BoardManager.boardSize; i++)
            {
                GameObject tileToShift = boardReferance[i][actualShift - tileSelect];
                tileToShift.transform.Translate(0,0, BoardManager.boardSize * BoardManager.tileHeight*direction);
            }
        }
        else if (actualShift < 0)
        {
            for (int i = 0; i < BoardManager.boardSize; i++)
            {
                GameObject tileToShift = boardReferance[i][BoardManager.boardSize + actualShift - tileSelect];
                tileToShift.transform.Translate(0,0, BoardManager.boardSize * BoardManager.tileHeight*direction);
            }
        }
        else
        {
            if (direction < 0)
            {
                for (int i = 0; i < BoardManager.boardSize; i++)
                {
                    GameObject tileToShift = boardReferance[i][0];
                    tileToShift.transform.Translate(0, 0,BoardManager.boardSize * BoardManager.tileHeight*direction);
                }
            }
            else
            {
                for (int i = 0; i < BoardManager.boardSize; i++)
                {
                    GameObject tileToShift = boardReferance[BoardManager.boardSize - 1][i];
                    tileToShift.transform.Translate(0, 0, BoardManager.boardSize * BoardManager.tileHeight*direction);
                }
            }
        }
    }
}
