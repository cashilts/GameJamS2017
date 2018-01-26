using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileOption : MonoBehaviour {

    public enum optionType{SAVEFILE, DIRECTORY };
    public optionType setOptionType;
    string optionText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onSelect()
    {
        if (setOptionType == optionType.DIRECTORY)
        {
            transform.parent.parent.GetComponent<FileSelect>().loadDirectoryOptions(optionText);
        }
        else
        {
            transform.parent.parent.GetComponent<FileSelect>().selectFile(optionText);
        }
    }

    public void setOption(string text)
    {
        optionText = text;
    }
}
