using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.LoadScene("StartMenu");
	}
	
	// Update is called once per frame
	void Update() {
	}

    public void BeginGame(){
        SceneManager.LoadScene("BoardLoader");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("BoardGen");
    }
}
