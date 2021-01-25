using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneInfoAssetCreatorSO))]
public class SceneInfoAssetCreatorEditor : Editor
{
    SceneInfoAssetCreatorSO script;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        script = (SceneInfoAssetCreatorSO)target;
        if (GUILayout.Button("UpdateAssets"))
        {
            script.UpdateAssets();
        }
    }
}
