using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int tileX = Random.Range(0, 99);
        int tileY = Random.Range(0, 99);
        MeshRenderer currentMesh = GameObject.Find("tile" + tileX + "," + tileY).transform.GetChild(0).GetComponent<MeshRenderer>();
        Material[] newMaterials = new Material[2];
        newMaterials[0] = currentMesh.material;
        newMaterials[1] = (Material)Resources.Load("Models/Materials/Highlight");
        currentMesh.materials = newMaterials;
	}
}
