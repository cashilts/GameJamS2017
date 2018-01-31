using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScienceWindow : EditorWindow {

    Science newScience;
    List<SerializedObject> serializedScience;
    List<Science> sciences;
    List<bool> dependantOn;
    Vector2 verticalScrollPos;

    [MenuItem("Window/Science Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ScienceWindow));
    }

    private void Awake()
    {
        newScience = new Science();
        serializedScience = new List<SerializedObject>();
        sciences = new List<Science>();
        dependantOn = new List<bool>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Add New Science", EditorStyles.boldLabel);
        newScience.name = EditorGUILayout.TextField("New Name", newScience.name);
        newScience.scienceCost = EditorGUILayout.IntField("New Cost", newScience.scienceCost);
        bool fileSelect = GUILayout.Button("Select New Image");
        if (fileSelect) {
            string[] splitPath = EditorUtility.OpenFilePanel("Icon Select", "", "").Split('/');
            string[] fileAndExtension = splitPath[splitPath.Length - 1].Split('.');
            newScience.iconPath = fileAndExtension[0];
        }

        GUILayout.Label("Existing Sciences", EditorStyles.boldLabel);
        verticalScrollPos = EditorGUILayout.BeginScrollView(verticalScrollPos, false, true, GUILayout.Width(1000));
        for(int i = 0; i<sciences.Count; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Id " + i.ToString());
            EditorGUILayout.PropertyField(serializedScience[i].FindProperty("name"), true);
            EditorGUILayout.PropertyField(serializedScience[i].FindProperty("requiredSciences"), true);

            dependantOn[i] = GUILayout.Toggle(dependantOn[i], "Requires");
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        bool submitScience = GUILayout.Button("Add Science");
        if (submitScience)
        {
            for (int i = 0; i < dependantOn.Count; i++)
            {
                newScience.addRequiredScience(i);
                dependantOn[i] = false;
            }
            dependantOn.Add(false);
            serializedScience.Add(new SerializedObject(newScience));
            sciences.Add(newScience);

            newScience = new Science();
            Repaint();
        }

        bool saveBuildings = GUILayout.Button("Save All");
        if (saveBuildings)
        {
            string dest = EditorUtility.SaveFilePanel("Save Science File", "", "Sciences", "sci");

            List<string> fileContents = new List<string>();
            for (int i = 0; i < sciences.Count; i++)
            {
                string buildingLine = "";
                buildingLine += sciences[i].name + ":";
                buildingLine += sciences[i].GetRequiredScienceString() + ":";
                buildingLine += sciences[i].iconPath + ":";
                buildingLine += sciences[i].scienceCost.ToString() + "";
                fileContents.Add(buildingLine);
            }
            System.IO.File.WriteAllLines(dest, fileContents.ToArray());
        }

        bool loadScience = GUILayout.Button("Load All");
        if (loadScience)
        {
            sciences.Clear();
            serializedScience.Clear();
            dependantOn.Clear();

            string source = EditorUtility.OpenFilePanel("Open Science File", "", "sci");
            string[] fileContents = System.IO.File.ReadAllLines(source);
            for (int i = 0; i < fileContents.Length; i++)
            {
                string currentScience = fileContents[i];
                Science tempScience = new Science();

                string[] buildingData = currentScience.Split(':');
                tempScience.name = buildingData[0];
                string[] requiredScienceData = buildingData[1].Split(' ');
                for (int j = 0; j < requiredScienceData.Length; j++)
                {
                    if (requiredScienceData[j] != "") tempScience.addRequiredScience(int.Parse(requiredScienceData[j]));
                }
                tempScience.iconPath = buildingData[2];
                tempScience.scienceCost = int.Parse(buildingData[3]);
                SerializedObject serialized = new SerializedObject(tempScience);
                sciences.Add(tempScience);
                serializedScience.Add(serialized);
                dependantOn.Add(false);
            }
            Repaint();

        }
    }

}
