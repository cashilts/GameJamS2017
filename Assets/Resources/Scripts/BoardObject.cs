using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardObject : MonoBehaviour {

    //Board objects are objects stored on tiles, these could be things such as units or cities. 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void onSelect() { }
    public virtual void getInfo() { }
}
