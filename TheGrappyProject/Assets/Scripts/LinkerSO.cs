using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LinkerSO", menuName = "Game/Linker")]

public class LinkerSO : ScriptableObject {
    public SceneLoadingChannelSO sceneLoadingChannel;
    public InputEventsSO inputProxy;
    public GameEventsSO gameEvents;
    public PlayerDataSO playerData;
    public PlayerVarsSO playerVars;
    public Collision2DEventChannelSO playerCollisionChannel;
    public MapDataSO mapData;
    public DataManagerSO dataManager;
}