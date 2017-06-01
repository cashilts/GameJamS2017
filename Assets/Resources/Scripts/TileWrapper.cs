using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWrapper : MonoBehaviour {
    Vector3 gridCenter = new Vector3(0, 0, 0);
    int gridOffsetX = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((GetComponent<Camera>().transform.position - gridCenter).x > BoardManager.tileWidth)
        {
            gridCenter.x += BoardManager.tileWidth;
            gridOffsetX += 1;
            ShiftTiles(1);
        }
        else if ((GetComponent<Camera>().transform.position - gridCenter).x < BoardManager.tileWidth * -1)
        {
            gridCenter.x -= BoardManager.tileWidth;
            gridOffsetX -= 1;
            ShiftTiles(-1);
        }
	}

    void ShiftTiles(int direction) {
        int actualShift = (gridOffsetX % BoardManager.boardSize);
        int tileSelect = (direction == 1) ? 1 : 0;
        if (actualShift > 0)
        {
            for (int i = 0; i < BoardManager.boardSize; i++)
            {
                GameObject tileToShift = GameObject.Find("tile" + (actualShift - tileSelect) + "," + i);
                tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
            }
        }
        else if (actualShift < 0)
        {
            for (int i = 0; i < BoardManager.boardSize; i++)
            {
                GameObject tileToShift = GameObject.Find("tile" + (BoardManager.boardSize + actualShift - tileSelect) + "," + i);
                tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
            }
        }
        else
        {
            if (direction < 0)
            {
                for (int i = 0; i < BoardManager.boardSize; i++)
                {
                    GameObject tileToShift = GameObject.Find("tile0" + "," + i);
                    tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
                }
            }
            else
            {
                for (int i = 0; i < BoardManager.boardSize; i++)
                {
                    GameObject tileToShift = GameObject.Find("tile" + (BoardManager.boardSize - 1) + "," + i);
                    tileToShift.transform.Translate(BoardManager.boardSize * BoardManager.tileWidth * direction, 0, 0);
                }
            }
        }
    }
}
