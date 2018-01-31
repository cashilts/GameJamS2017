using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingsWindow : EditorWindow {
    Building newBuilding;
    List<Building> buildings;
    List<SerializedObject> test;
    List<bool> dependantOn;
    Vector2 verticalScrollPos;
    Vector2 horizontalScollPos;
    [MenuItem ("Window/Building Editor")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(BuildingsWindow));
    }

    private void Awake()
    {
        newBuilding = new Building();
        buildings = new List<Building>();
        test = new List<SerializedObject>();
        dependantOn = new List<bool>();
    }

    // Use this for initialization
    private void OnGUI()
    {
      
        GUILayout.Label("Add New Building", EditorStyles.boldLabel);
        newBuilding.name = EditorGUILayout.TextField("New Name:", newBuilding.name);
        newBuilding.buildCost = EditorGUILayout.IntField("Cost:", newBuilding.buildCost);
        newBuilding.iconPath = EditorGUILayout.TextField("New Icon:", newBuilding.iconPath);
        bool fileSelect = GUILayout.Button("Select New Image");
        if (fileSelect)
        {
            string[] splitPath = EditorUtility.OpenFilePanel("Icon Select", "", "").Split('/');
            string[] fileAndExtension = splitPath[splitPath.Length - 1].Split('.');
            newBuilding.iconPath = fileAndExtension[0];
        }


        GUILayout.Label("Existing Buildings", EditorStyles.boldLabel);
        verticalScrollPos = EditorGUILayout.BeginScrollView(verticalScrollPos, false, true,GUILayout.Width(1000));
        for (int i = 0; i < buildings.Count; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Id " + i.ToString());
            EditorGUILayout.PropertyField(test[i].FindProperty("name"), true);
            EditorGUILayout.PropertyField(test[i].FindProperty("requiredBuildings"), true);
            EditorGUILayout.PropertyField(test[i].FindProperty("requiredSciences"), true);

            dependantOn[i] = GUILayout.Toggle(dependantOn[i], "Requires");
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        bool submitBuilding = GUILayout.Button("Add Building");
        if (submitBuilding)
        {
            for(int i = 0; i<dependantOn.Count; i++)
            {
                newBuilding.addRequiredBuilding(i);
                dependantOn[i] = false;
            }
            dependantOn.Add(false);
            test.Add(new SerializedObject(newBuilding));
            buildings.Add(newBuilding);

            newBuilding = new Building();
            Repaint();
        }
        bool saveBuildings = GUILayout.Button("Save All");
        if (saveBuildings)
        {
            string dest = EditorUtility.SaveFilePanel("Save Building File", "", "Buildings", "bld");

            List<string> fileContents = new List<string>();
            for(int i = 0; i<buildings.Count; i++)
            {
                string buildingLine = "";
                buildingLine += buildings[i].name+":";
                buildingLine += buildings[i].GetRequiredBuildingString() + ":";
                buildingLine += buildings[i].GetRequiredScienceString() + ":";
                buildingLine += buildings[i].iconPath + ":";
                buildingLine += buildings[i].buildCost.ToString() + "";
                fileContents.Add(buildingLine);
            }
            System.IO.File.WriteAllLines(dest, fileContents.ToArray());
        }
        bool loadBuildings = GUILayout.Button("Load All");
        if (loadBuildings)
        {
            buildings.Clear();
            test.Clear();
            dependantOn.Clear();

            string source = EditorUtility.OpenFilePanel("Open Building FIle", "", "bld");
            string[] fileContents = System.IO.File.ReadAllLines(source);
            buildings.Clear();
            for(int i = 0; i<fileContents.Length; i++)
            {
                string currentBuilding = fileContents[i];
                Building newBuilding = new Building();

                string[] buildingData = currentBuilding.Split(':');
                newBuilding.name = buildingData[0];
                string[] requiredBuildingData = buildingData[1].Split(' ');
                for(int j= 0; j<requiredBuildingData.Length; j++)
                {
                    if(requiredBuildingData[j] != "")newBuilding.addRequiredBuilding(int.Parse(requiredBuildingData[j]));
                }
                string[] requiredScienceData = buildingData[2].Split(' ');
                for(int j = 0; j<requiredScienceData.Length; j++)
                {
                    if(requiredScienceData[j]!="")newBuilding.addRequiredScience(int.Parse(requiredScienceData[j]));
                }
                newBuilding.iconPath = buildingData[3];
                newBuilding.buildCost = int.Parse(buildingData[4]);
                SerializedObject serializedBuilding = new SerializedObject(newBuilding);
                buildings.Add(newBuilding);
                test.Add(serializedBuilding);
                dependantOn.Add(false);
            }
            Repaint();

        }

    }
}
