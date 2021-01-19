using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (SceneManager))]
public class SceneManagerEditor : Editor {
   SceneManager sceneManager;
   public override void OnInspectorGUI () {
      base.OnInspectorGUI ();
      sceneManager = (SceneManager) target;
      if(GUILayout.Button("Refresh")){
            sceneManager.RefreshScenes();
        }
   }

   [UnityEditor.Callbacks.DidReloadScripts]
   public static void OnReload () {
      Debug.Log ("reload");
   }

}