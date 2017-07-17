using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour {
    LineRenderer trace;
    MeshRenderer previousMesh;
	// Use this for initialization
	void Start () {
        trace = GetComponent<LineRenderer>();
        previousMesh = GameObject.Find("tile0,0").GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd,out hit, 30)) {
            trace.SetPosition(0, transform.position);
            trace.SetPosition(1, hit.point);
            MeshRenderer currentMesh = hit.collider.GetComponent<MeshRenderer>();
            if (currentMesh != previousMesh)
            {

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
        //int tileX = Random.Range(0, 99);
        //int tileY = Random.Range(0, 99);

    }
}
