using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSet : MonoBehaviour {
    [SerializeField]
    List<Building> test;


    public void Awake()
    {
        test = new List<Building>();
        Building building = new Building();
        building.name = "Test";
        building.addRequiredBuilding(0);
        building.addRequiredScience(0);
        test.Add(building);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
