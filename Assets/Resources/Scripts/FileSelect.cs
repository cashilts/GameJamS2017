using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileSelect : MonoBehaviour {

    private int usedHeight = 0;
    GameObject optionHolder;
    public delegate void fileSubmit(string filename);
    fileSubmit submitFunction;

	// Use this for initialization
	void Start () {
        optionHolder = transform.Find("OptionHolder").gameObject;
        string directory = System.IO.Directory.GetCurrentDirectory() + "\\LocalSaves\\";
        loadDirectoryOptions(directory);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadDirectoryOptions(string directory)
    {
        for (int i = 0; i < optionHolder.transform.childCount; i++)
        {
            Destroy(optionHolder.transform.GetChild(i).gameObject);
        }
        usedHeight = 0;
        optionHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        GameObject prevDir = Instantiate((GameObject)Resources.Load("Prefabs/FileOption"));
        prevDir.transform.SetParent(optionHolder.transform, false);
        prevDir.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -15 - usedHeight);
        usedHeight += 30;
        prevDir.transform.Find("Text").GetComponent<Text>().text = directory.Substring(0, directory.LastIndexOf('\\'));
        prevDir.GetComponent<FileOption>().setOptionType = FileOption.optionType.DIRECTORY;

        string[] directoriesInDir = System.IO.Directory.GetDirectories(directory);
        for(int i = 0; i<directoriesInDir.Length; i++)
        {
            GameObject newOption = Instantiate((GameObject)Resources.Load("Prefabs/FileOption"));
            newOption.transform.SetParent(optionHolder.transform, false);
            newOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -15 - usedHeight);
            usedHeight += 30;
            newOption.transform.Find("Text").GetComponent<Text>().text = directoriesInDir[i];
            newOption.GetComponent<FileOption>().setOptionType = FileOption.optionType.DIRECTORY;
        }

        string[] filesInDir = System.IO.Directory.GetFiles(directory);
        
        for(int i = 0; i<filesInDir.Length; i++)
        {
            if (filesInDir[i].Contains(".save"))
            {
                GameObject newOption = Instantiate((GameObject)Resources.Load("Prefabs/FileOption"));
                newOption.transform.SetParent(optionHolder.transform, false);
                newOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -15 - usedHeight);
                usedHeight += 30;
                newOption.transform.Find("Text").GetComponent<Text>().text = filesInDir[i];
                newOption.GetComponent<FileOption>().setOptionType = FileOption.optionType.SAVEFILE;
            }
        }

        transform.Find("Scrollbar").GetComponent<Scrollbar>().size = ((200f / usedHeight) > 1 ? 1 : (200f / usedHeight));
    }

    public void scrollBarMoved()
    {
        optionHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,(usedHeight - 200) * (transform.Find("Scrollbar").GetComponent<Scrollbar>().value));
    }

    public void selectFile(string filename)
    {
        submitFunction(filename);
    }

    public void setFileSelectFunction(fileSubmit function)
    {
        submitFunction = function;
    }
}
