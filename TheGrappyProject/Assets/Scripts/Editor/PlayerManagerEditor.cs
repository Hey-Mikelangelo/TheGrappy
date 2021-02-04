using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : Editor{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerManager script = (PlayerManager)target;
        if (GUILayout.Button("GenerateMap"))
        {
            script.GenerateChunks();
        }

    }
}