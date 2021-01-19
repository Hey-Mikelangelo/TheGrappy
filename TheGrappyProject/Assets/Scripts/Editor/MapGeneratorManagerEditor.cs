using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGeneratorManager))]
public class MapGeneratorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGeneratorManager script = (MapGeneratorManager)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("generate"))
        {
            //script.GenerateTexture();
            script.generateOnChange = false;
            script.GenerateChunk(0, 0);

        }
        if (GUILayout.Button("spawn"))
        {

        }
    }
}
