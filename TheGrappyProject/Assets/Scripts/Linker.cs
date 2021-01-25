using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LinkerSO", menuName = "Game/Linker")]

public class Linker : ScriptableObject {
    public InputProxy inputProxy;
    public VoidEventChannelSO playerDeath;

}