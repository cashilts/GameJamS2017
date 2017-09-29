using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    Player mainPlayer;
	// Use this for initialization
	void Start () {
        mainPlayer = GameObject.Find("GameManager").transform.Find("Player0").GetComponent<Player>();
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
