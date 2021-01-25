using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoadTest))]
public class LoadTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LoadTest script = (LoadTest)target;
        if (GUILayout.Button("Load"))
        {
            script.Load();
        }
        if (GUILayout.Button("FadeStart"))
        {
            script.StartLoadingScreen();
        }
        if (GUILayout.Button("FadeEnd"))
        {
            script.EndLoadingScreen();
        }
    }
}
