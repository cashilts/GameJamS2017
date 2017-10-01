using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour {

    BoardObject infoObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(infoObject != null) infoObject.getInfo();
	}

    public void setObject(BoardObject newObject)
    {
        infoObject = newObject;
    }
}
