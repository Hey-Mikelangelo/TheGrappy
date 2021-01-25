using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionChannelSO : ScriptableObject
{
    
    public List<SceneTransitionSO> Transitions = new List<SceneTransitionSO>();

    public SceneTransitionSO currentTransition { get; private set; }
    
}
