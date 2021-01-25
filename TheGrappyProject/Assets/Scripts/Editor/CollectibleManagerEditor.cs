using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CollectibleManager))]
public class CollectibleManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        CollectibleManager script = (CollectibleManager)target;
        serializedObject.Update();
        EditorList.Show(serializedObject.FindProperty("CollectibleNames"), EditorListOption.All);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("collectiblesFolder"));
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("Update"))
        {
            script.GenerateEnums();
        }
    }
}
