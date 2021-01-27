#if UNITY_EDITOR
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneInfoUpdater : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        for (int i = 0; i < movedAssets.Length; i++)
        {
            if (movedAssets[i].Contains(".unity"))
            {
                SceneInfoAssetCreatorSO.CallUpdateAssets();
            }
        }
    }
}
#endif