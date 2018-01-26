using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class CameraControllerPC : CameraController {

    public enum inputModes { TILESELECT, ACTIONTARGET, INMENU, TEXTENTRY, CITYMENU};
    inputModes currentInputMode = inputModes.TILESELECT;

    Tile selectedTile;
    GameObject previouslySelected;
    Tile startTile;
    RadialMenu openMenu = null;
    RadialButton selectedButton;
   
    targetSelectMethod onActionTarget;
    Canvas tileInfoTab;
    int hoverCount = 0;
    int zoom = 1;
    public delegate void targetSelectMethod(Tile tile);


    // Use this for initialization
    void Start () {
        previouslySelected = GameObject.Find("tile0,0");
        Camera.main.transform.position = GameObject.Find("Settler0").transform.position;
        Camera.main.transform.Translate(0, 0, -5);
        tileInfoTab = ((GameObject)Instantiate(Resources.Load("Prefabs/TileInfo"))).GetComponent<Canvas>();
        tileInfoTab.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = Input.mousePosition;
        if (currentInputMode == inputModes.TILESELECT || currentInputMode == inputModes.ACTIONTARGET)
        {
            
            if (mousePos.x > 0 && mousePos.y > 0 && mousePos.x < Screen.width && mousePos.y < Screen.height)
            {
                if (mousePos.x < Screen.width * 0.05)
                {
                    transform.Translate(-0.1f, 0, 0);
                }
                else if (mousePos.x > Screen.width * 0.95)
                {
                    transform.Translate(0.1f, 0, 0);
                }
                if (mousePos.y < Screen.height * 0.05)
                {
                    transform.Translate(0, -0.1f, 0);
                }
                else if (mousePos.y > Screen.height * 0.95)
                {
                    transform.Translate(0, 0.1f, 0);
                }
            }
        }
        if (!(currentInputMode == inputModes.TEXTENTRY)){
            Vector2 mouseScroll = Input.mouseScrollDelta;
            transform.Translate(0, 0, mouseScroll.y);
            mousePos.Set(mousePos.x, mousePos.y, 1);
            mousePos = GetComponent<Camera>().ScreenToWorldPoint(mousePos);
            Vector3 mouseTouchPoint = mousePos;
            mouseTouchPoint = transform.position - mouseTouchPoint;
            mouseTouchPoint.Normalize();
            RaycastHit hit;

            if (Physics.Raycast(transform.position, mouseTouchPoint * -1, out hit, 50))
            {
                GameObject currentlySelected = hit.collider.gameObject;
                Tile newSelectedTile = currentlySelected.GetComponent<Tile>();
                if (newSelectedTile == null)
                {
                    selectedTile = null;
                    tileInfoTab.gameObject.SetActive(false);
                    selectedButton = currentlySelected.GetComponent<RadialButton>();
                }
                else
                {
                    if (newSelectedTile == selectedTile)
                    {
                        hoverCount++;
                        if (hoverCount > 45)
                        {
                            tileInfoTab.transform.position = mousePos;
                            tileInfoTab.transform.Find("TileStats").GetComponent<Text>().text = "Wealth: " + selectedTile.wealth + "\n" + "Food: " + selectedTile.food; ;
                            tileInfoTab.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        selectedTile = newSelectedTile;
                        tileInfoTab.gameObject.SetActive(false);
                        hoverCount = 0;
                    }


                }
                //If the currently selected mesh is hit again, don't update the texture
                if (currentlySelected != previouslySelected)
                {

                    //Add on a highlight to the selected tile to show it got hit
                    /*Material[] newMaterials = new Material[2];
                    newMaterials[0] = currentMesh.material;
                    newMaterials[1] = (Material)Resources.Load("Models/Materials/Highlight");
                    currentMesh.materials = newMaterials;
                    newMaterials = new Material[1];
                    newMaterials[0] = previousMesh.materials[0];
                    previousMesh.materials = newMaterials; */
                    previouslySelected = currentlySelected;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (currentInputMode == inputModes.TILESELECT)
                {
                    if (selectedTile != null && openMenu != null)
                    {
                        Destroy(openMenu.gameObject);
                        selectedButton = null;
                    }
                    else if (selectedTile == null && selectedButton != null)
                    {
                        selectedButton.onClick();
                    }
                    else
                    {
                        startTile = selectedTile;
                        startTile.onMouseButtonDown();
                    }
                }
                else if (currentInputMode == inputModes.ACTIONTARGET)
                {
                    if (selectedTile != null && onActionTarget != null)
                    {
                        onActionTarget(selectedTile);
                    }
                }
                else if (currentInputMode == inputModes.INMENU)
                {
                    if (Input.mousePosition.x < Screen.width / 2)
                    {
                        Destroy(GameObject.Find("CityMenu"));
                        changeMode(inputModes.TILESELECT);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {

            }
            if (Input.GetKeyDown("s"))
            {
                GameObject InputPanel = Instantiate((GameObject)Resources.Load("Prefabs/InputPanel"));
                InputPanel.name = "InputPanel";
                InputPanel.transform.SetParent(GameObject.Find("MapHUD").transform, false);
                TextInput.stringFunction saveCallback = new TextInput.stringFunction(BoardManager.Instance.saveBoardState);
                InputPanel.GetComponent<TextInput>().setCallback(saveCallback);
                changeMode(inputModes.TEXTENTRY);
            }
            else if (Input.GetKeyDown("l"))
            {
                GameObject fileSelect = Instantiate((GameObject)Resources.Load("Prefabs/FileSelect"));
                fileSelect.name = "FileSelect";
                fileSelect.transform.SetParent(GameObject.Find("Canvas").transform, false);
                FileSelect.fileSubmit submitCallback = new FileSelect.fileSubmit(GameObject.Find("BoardGenerator").GetComponent<BoardManager>().loadBoardState);
                fileSelect.GetComponent<FileSelect>().setFileSelectFunction(submitCallback);
                fileSelect.GetComponent<FileSelect>().loadDirectoryOptions(System.IO.Directory.GetCurrentDirectory());
                changeMode(inputModes.TEXTENTRY);
            }
            if (Input.GetKeyDown("-"))
            {

            }
            else if (Input.GetKeyDown("+"))
            {

            }

        }
    }

    public void menuOpened(RadialMenu menu)
    {
        openMenu = menu;
    }

    public void changeMode(inputModes newMode)
    {
        currentInputMode = newMode;
    }

    public void setActionTarget(targetSelectMethod onSelectMethod)
    {
        onActionTarget = onSelectMethod;   
    }

    public void inputNextTurn()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().endCurrentTurn();
    }

    public override XmlElement saveCamera(ref XmlDocument doc)
    {
        XmlElement cameraElement = doc.CreateElement("Camera");
        cameraElement.SetAttribute("XPos", transform.position.x.ToString());
        cameraElement.SetAttribute("YPos", transform.position.y.ToString());
        cameraElement.SetAttribute("ZPos", transform.position.z.ToString());
        return cameraElement;
    }
}
