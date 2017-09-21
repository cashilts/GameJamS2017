using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {
    bool generated = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!generated) {
            BoardManager boardMan = GameObject.Find("GameObject").GetComponent<BoardManager>();
            Texture2D myNewText = new Texture2D(100 * 3, 100 * 3);
            for(int i = 0; i<100; i++)
            {
                for (int j = 0; j < 100; j++) { 

                    Color32 setColor = new Color(0,0,0,0);
                    Tile currTile = boardMan.getTile(j, i);
                    if (currTile.setType == Tile.tileType.DeepWater)
                    {
                        setColor = new Color32(63, 72, 204,255);
                    }
                    else if (currTile.setType == Tile.tileType.Grass)
                    {
                        setColor = new Color32(34, 177, 76,255);
                    }
                    else if(currTile.setType == Tile.tileType.ShallowWater)
                    {
                        setColor = new Color32(0, 162, 232,255);
                    }
                    else if(currTile.setType == Tile.tileType.Ice)
                    {
                        setColor = new Color32(191, 241, 255,255);
                        
                    }
                    Debug.Log(setColor);
                    myNewText.SetPixel(i * 3, j * 3, setColor);
                    myNewText.SetPixel(i * 3 + 1, j * 3, setColor);
                    myNewText.SetPixel(i * 3 + 2, j * 3, setColor);
                    myNewText.SetPixel(i * 3, j * 3 + 1, setColor);
                    myNewText.SetPixel(i * 3, j * 3 + 2, setColor);
                    myNewText.SetPixel(i * 3 + 1, j * 3 + 1, setColor);
                    myNewText.SetPixel(i * 3 + 1, j * 3 + 2, setColor);
                    myNewText.SetPixel(i * 3 + 2, j * 3 + 1, setColor);
                    myNewText.SetPixel(i * 3 + 2, j * 3 + 2, setColor);
                    
                }
            }
            myNewText.Apply();
            GetComponent<MeshRenderer>().material.SetTexture(Shader.PropertyToID("_MainTex"), myNewText);
            generated = true;
        }
        		
	}
}
