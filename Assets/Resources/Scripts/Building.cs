using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building : ScriptableObject {

    public string name = "";
    [SerializeField]
    List<int> requiredBuildings = new List<int>();
    [SerializeField]
    List<int> requiredSciences = new List<int>();
    public string iconPath = "";
    public int buildCost = 0;


    public void addRequiredBuilding(int num)
    {
        requiredBuildings.Add(num);
    }

    public void addRequiredScience(int num)
    {
        requiredSciences.Add(num);
    }

    public string GetRequiredBuildingString()
    {
        string data = "";
        if (requiredBuildings.Count != 0)
        {
            for (int i = 0; i < requiredBuildings.Count; i++)
            {
                data += requiredBuildings[i].ToString() + " ";
            }
            data = data.Substring(0, data.Length - 1);
        }
        return data;
    }

    public string GetRequiredScienceString()
    {
        string data = "";
        if (requiredSciences.Count != 0)
        {
            for (int i = 0; i < requiredSciences.Count; i++)
            {
                data += requiredSciences[i].ToString() + " ";
            }
            data = data.Substring(0, data.Length - 1);
        }
        return data;
    }

}
