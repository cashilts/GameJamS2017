using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLoader : Singleton<GameLoader> {

    bool loadFromFile = false;
    string fileName;
    Coroutine currentLoad = null;


    // Use this for initialization
	void Start () {   
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void load(bool fileload, string filename)
    {
        loadFromFile = fileload;
        fileName = filename;
        currentLoad=  StartCoroutine(Load());
    }



    IEnumerator Load()
    {
        DontDestroyOnLoad(GameManager.Instance);
        GameManager.Instance.setUpPlayers();
        GameObject boardGen = GameObject.Find("BoardGenerator");
        BoardManager genScript = boardGen.GetComponent<BoardManager>();
        GameObject canvas = GameObject.Find("Canvas");
        if (!loadFromFile)
        {
            Slider slider = GameObject.Find("Canvas").transform.Find("Slider").GetComponent<Slider>();
            Text stateText = GameObject.Find("Canvas").transform.Find("State").GetComponent<Text>();
            genScript.initBoard();
            slider.value = 0.2f;
            stateText.text = "Generating Poles";
            genScript.GenerateIcePoles();
            yield return null;
            slider.value = 0.4f;
            stateText.text = "Generating Continents";
            genScript.GenerateContinents();
            yield return null;
            slider.value = 0.6f;
            stateText.text = "Making Shallower Water";
            genScript.PlaceShallowWater();
            yield return null;
            slider.value = 0.8f;
            stateText.text = "Placing Rivers";
            genScript.PlaceRivers();
            yield return null;

            genScript.spawnStartUnits();
            genScript.initializeResources();
            slider.value = 1;
            stateText.text = "Complete";
        }
        else
        {
            genScript.loadBoardState(fileName);
        }
        DontDestroyOnLoad(boardGen.transform.gameObject);
        StateManager stateMan = GameObject.Find("SceneManager").GetComponent<StateManager>();
        stateMan.LoadScene();
        StopCoroutine(currentLoad);
    }
}
