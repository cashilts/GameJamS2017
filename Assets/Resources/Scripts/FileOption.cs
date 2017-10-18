using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileOption : MonoBehaviour {

    public enum optionType{SAVEFILE, DIRECTORY };
    public optionType setOptionType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onSelect()
    {
        string text = transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text;
        if (setOptionType == optionType.DIRECTORY)
        {
            transform.parent.parent.GetComponent<FileSelect>().loadDirectoryOptions(text);
        }
        else
        {
            transform.parent.parent.GetComponent<FileSelect>().selectFile(text);
        }
    }
}
