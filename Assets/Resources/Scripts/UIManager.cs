using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    Player mainPlayer;
	// Use this for initialization
	void Start () {
        //Get referance to player object
        mainPlayer = GameObject.Find("GameManager").transform.Find("Player0").GetComponent<Player>();

        //Reset event system 
        GameObject eventSystem = Instantiate(Resources.Load<GameObject>("Prefabs/EventSystem"));
        eventSystem.transform.SetParent(transform);

        //Set button click event
        transform.Find("InfoPanel").Find("Button").GetComponent<Button>().onClick.AddListener(GameObject.Find("Camera").GetComponent<CameraControllerPC>().inputNextTurn);
	}
	
	// Update is called once per frame
	void Update () {
        Text goldText = transform.Find("Gold").GetComponent<Text>();
        goldText.text = "Gold: " + mainPlayer.gold + " (";
        if(mainPlayer.GPT > 0)
        {
            goldText.text += "+" + mainPlayer.GPT + ")";
        }
        else
        {
            goldText.text += mainPlayer.GPT + ")";
        }

        Text turnText = transform.Find("Turn").GetComponent<Text>();
        turnText.text = "Turn: " + mainPlayer.transform.parent.GetComponent<GameManager>().turnCount;

	}
}
