using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfoHolderSO", menuName = "Scene Data/SceneInfo Holder")]
public class SceneInfoHolderSO : ScriptableObject
{
    public string sceneInfoSOPath = "Assets/ScriptableObjects/SceneInfo";
    public List<SceneInfoSO> SceneInfoList = new List<SceneInfoSO>();

}
