using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    public SceneLoadingChannelSO channel;
    public static bool loaded;

    public List<SceneInfoSO> scenes;

    public void LoadScenes()
    {
        channel.Load(scenes, true);
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
