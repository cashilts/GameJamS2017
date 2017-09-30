using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerPC : MonoBehaviour {

    Tile selectedTile;
    LineRenderer trace;
    MeshRenderer previousMesh;
    Tile startTile;
    // Use this for initialization
    void Start () {
        previousMesh = GameObject.Find("tile0,0").GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x > 0 && mousePos.y > 0 && mousePos.x < Screen.width && mousePos.y < Screen.height)
        {
            if (mousePos.x < Screen.width * 0.25)
            {
                transform.Translate(-0.1f, 0, 0);
            }
            else if (mousePos.x > Screen.width * 0.75)
            {
                transform.Translate(0.1f, 0, 0);
            }
            if (mousePos.y < Screen.height * 0.25)
            {
                transform.Translate(0, -0.1f, 0);
            }
            else if (mousePos.y > Screen.height * 0.75)
            {
                transform.Translate(0, 0.1f, 0);
            }
        }
        Vector2 mouseScroll = Input.mouseScrollDelta;
        transform.Translate(0, 0, mouseScroll.y);
        mousePos.Set(mousePos.x, mousePos.y, 1);
        Vector3 mouseTouchPoint = GetComponent<Camera>().ScreenToWorldPoint(mousePos);
        mouseTouchPoint = transform.position - mouseTouchPoint;
        mouseTouchPoint.Normalize();
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, mouseTouchPoint*-1, out hit, 30))
        {

            MeshRenderer currentMesh = hit.collider.GetComponent<MeshRenderer>();
            selectedTile = currentMesh.GetComponent<Tile>();

            //If the currently selected mesh is hit again, don't update the texture
            if (currentMesh != previousMesh)
            {

                //Add on a highlight to the selected tile to show it got hit
                /*Material[] newMaterials = new Material[2];
                newMaterials[0] = currentMesh.material;
                newMaterials[1] = (Material)Resources.Load("Models/Materials/Highlight");
                currentMesh.materials = newMaterials;
                newMaterials = new Material[1];
                newMaterials[0] = previousMesh.materials[0];
                previousMesh.materials = newMaterials; */
                previousMesh = currentMesh;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            startTile = selectedTile;
            startTile.onMouseButtonDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            startTile.onMouseButtonUp(selectedTile);
            
        }
    }
}
