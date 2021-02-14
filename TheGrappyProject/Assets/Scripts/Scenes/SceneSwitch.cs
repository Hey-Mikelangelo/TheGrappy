using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    public SceneLoadingChannelSO channel;
    public bool showLoadingScreen;
    public static bool loaded;

    public List<SceneInfoSO> scenes;

    public void LoadScenes()
    {
        channel.Load(scenes, showLoadingScreen);
    }
    public void Quit()
    {
        channel.Quit();
    }
    public void Awake()
    {
        channel.LoadInitScene();
    }
}
