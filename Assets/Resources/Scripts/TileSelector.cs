using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour {
    LineRenderer trace;
    MeshRenderer previousMesh;
    SteamVR_TrackedController controller;
    Tile selectedTile;
	// Use this for initialization
	void Start () {

        trace = GetComponent<LineRenderer>();
        previousMesh = GameObject.Find("tile0,0").GetComponent<MeshRenderer>();
        controller = GetComponent<SteamVR_TrackedController>();
	}
	
	// Update is called once per frame
	void Update () {

        //Get direction that the controller is pointing and raycast to the tiles 
        Vector3 fwd = transform.TransformDirection(transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd,out hit, 30)) {

            //Draws a trace line to show the raycast
            trace.SetPosition(0, transform.position);
            trace.SetPosition(1, hit.point);

            //Get the object that was hit and its renderer
            MeshRenderer currentMesh = hit.collider.GetComponent<MeshRenderer>();
            selectedTile = currentMesh.GetComponent<Tile>();

            //If the currently selected mesh is hit again, don't update the texture
            if (currentMesh != previousMesh)
            {

                //Add on a highlight to the selected tile to show it got hit
                Material[] newMaterials = new Material[2];
                newMaterials[0] = currentMesh.material;
                newMaterials[1] = (Material)Resources.Load("Models/Materials/Highlight");
                currentMesh.materials = newMaterials;
                newMaterials = new Material[1];
                newMaterials[0] = previousMesh.materials[0];
                previousMesh.materials = newMaterials;
                previousMesh = currentMesh;
            }
        }
        if (controller.triggerPressed)
        {
        }

    }
}
