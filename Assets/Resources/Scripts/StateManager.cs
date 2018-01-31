using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateManager : Singleton<StateManager>
{

    bool loadStart = false;
    string loadFile = "";

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.LoadScene("StartMenu");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BeginGame()
    {
        SceneManager.LoadScene("BoardLoader");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("BoardGen");
    }

    public void LoadBoard(string filename)
    {
        loadFile = filename;
        loadStart = true;
        SceneManager.LoadScene("BoardLoader");
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == 2) GameLoader.Instance.load(loadStart, loadFile);
    }



    public void displayLoadSettings()
    {

        GameObject fileSelect = Instantiate(Resources.Load<GameObject>("Prefabs/FileSelect"));
        fileSelect.name = "FileSelect";
        fileSelect.transform.SetParent(GameObject.Find("Canvas").transform, false);
        FileSelect.fileSubmit submitCallback = new FileSelect.fileSubmit(Instance.LoadBoard);
        fileSelect.GetComponent<FileSelect>().setFileSelectFunction(submitCallback);
        fileSelect.GetComponent<FileSelect>().loadDirectoryOptions(System.IO.Directory.GetCurrentDirectory());
        Destroy(GameObject.Find("Canvas").transform.Find("Single Player Begin").gameObject);
        Destroy(GameObject.Find("Canvas").transform.Find("Load Game Begin").gameObject);
    }
}
