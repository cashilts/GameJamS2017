using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Science : ScriptableObject {

    public string name;
    public string iconPath;
    public int scienceCost;
    [SerializeField]
    List<int> requiredSciences = new List<int>();

    public void addRequiredScience(int num)
    {
        requiredSciences.Add(num);
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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
