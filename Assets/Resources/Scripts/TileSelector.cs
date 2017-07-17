using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour {
    LineRenderer trace;
	// Use this for initialization
	void Start () {
        trace = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd,out hit, 30)) {
            Debug.DrawLine(transform.position, hit.point);
            trace.SetPosition(0, transform.position);
            trace.SetPosition(1, hit.point);
            MeshRenderer currentMesh = hit.collider.GetComponent<MeshRenderer>();
            Material[] newMaterials = new Material[2];
            newMaterials[0] = currentMesh.material;
            newMaterials[1] = (Material)Resources.Load("Models/Materials/Highlight");
            currentMesh.materials = newMaterials;
        }
        //int tileX = Random.Range(0, 99);
        //int tileY = Random.Range(0, 99);

    }
}
