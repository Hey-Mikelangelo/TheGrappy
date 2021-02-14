using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeathPopUpScreen))]
public class popUpScreenEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DeathPopUpScreen script = (DeathPopUpScreen)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Open"))
        {
            script.Open();

        }
        if (GUILayout.Button("Close"))
        {
            script.Close();
        }
    }
}
