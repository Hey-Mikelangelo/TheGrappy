using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSceneLoadingManagerHolder : MonoBehaviour
{
    public SceneLoadingManagerSO sceneLoadingManager;
    private void Awake()
    {
        sceneLoadingManager.SetPersistentCourotineCaller(this);
    }
}
