using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject boardGen = GameObject.Find("BoardGenerator");
        BoardManager genScript = boardGen.GetComponent<BoardManager>();
        GameObject canvas = GameObject.Find("Canvas");
        Slider slider = GameObject.Find("Canvas").transform.Find("Slider").GetComponent<Slider>();
        Text stateText = GameObject.Find("Canvas").transform.Find("State").GetComponent<Text>();

        slider.value = 0.2f;
        stateText.text = "Generating Poles";
        genScript.GenerateIcePoles();

        slider.value = 0.4f;
        stateText.text = "Generating Continents";
        genScript.GenerateContinents();

        slider.value = 0.6f;
        stateText.text = "Making Shallower Water";
        genScript.PlaceShallowWater();

        slider.value = 0.8f;
        stateText.text = "Placing Rivers";
        genScript.PlaceRivers();

        

        DontDestroyOnLoad(boardGen.transform.gameObject);
        slider.value = 1;
        stateText.text = "Complete";

        GameObject gameMan = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/GameManager"));
        gameMan.name = "GameManager";
        DontDestroyOnLoad(gameMan);
        
        StateManager stateMan = GameObject.Find("SceneManager").GetComponent<StateManager>();
        stateMan.LoadScene();
        genScript.spawnStartUnits();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
