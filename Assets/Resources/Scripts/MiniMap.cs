using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {
    public bool generated = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!generated) {
            generateMiniMap();
        }



        		
	}
     public void generateMiniMap() {
        BoardManager boardMan = BoardManager.Instance;
        Texture2D myNewText = new Texture2D(100 * 3, 100 * 3);
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {

                Color32 setColor = new Color(0, 0, 0, 0);
                Tile currTile = boardMan.getTile(j, i);
                if (currTile.setType == Tile.tileType.DeepWater)
                {
                    setColor = new Color32(44, 47, 112, 255);
                }
                else if (currTile.setType == Tile.tileType.Grass)
                {
                    setColor = new Color32(29, 95, 37, 255);
                    if (currTile.hasWater)
                    {
                        Color32 riverColor = new Color32(17, 92, 115, 255);

                        if (currTile.waterDirections[0]) myNewText.SetPixel(i * 3, j * 3, riverColor);
                        else myNewText.SetPixel(i * 3, j * 3, setColor);

                        myNewText.SetPixel(i * 3 + 1, j * 3, riverColor);

                        if (currTile.waterDirections[1]) myNewText.SetPixel(i * 3 + 2, j * 3, riverColor);
                        else myNewText.SetPixel(i * 3 + 2, j * 3, setColor);

                        if (currTile.waterDirections[5]) myNewText.SetPixel(i * 3, j * 3 + 1, riverColor);
                        else myNewText.SetPixel(i * 3, j * 3 + 1, setColor);

                        myNewText.SetPixel(i * 3 + 1, j * 3 + 1, riverColor);

                        if (currTile.waterDirections[2]) myNewText.SetPixel(i * 3 + 2, j * 3 + 1, riverColor);
                        else myNewText.SetPixel(i * 3 + 2, j * 3 + 1, setColor);

                        if (currTile.waterDirections[4]) myNewText.SetPixel(i * 3, j * 3 + 2, riverColor);
                        else myNewText.SetPixel(i * 3, j * 3 + 2, setColor);

                        myNewText.SetPixel(i * 3 + 1, j * 3 + 2, setColor);

                        if (currTile.waterDirections[3]) myNewText.SetPixel(i * 3 + 2, j * 3 + 2, riverColor);
                        else myNewText.SetPixel(i * 3 + 2, j * 3 + 2, setColor);
                        continue;
                    }
                }
                else if (currTile.setType == Tile.tileType.ShallowWater)
                {
                    setColor = new Color32(17, 92, 115, 255);
                }
                else if (currTile.setType == Tile.tileType.Ice)
                {
                    setColor = new Color32(108, 130, 120, 255);

                }
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
        GetComponent<Image>().sprite = Sprite.Create(myNewText, new Rect(0, 0, myNewText.width, myNewText.height), new Vector2(0.5f, 0.5f));
        //GetComponent<MeshRenderer>().material.SetTexture(Shader.PropertyToID("_MainTex"), myNewText);
        generated = true;
    }
}
