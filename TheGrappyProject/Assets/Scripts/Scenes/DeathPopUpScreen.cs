using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DeathPopUpScreen : MonoBehaviour
{
    public SceneTransitionSO pupUpScreen;
    public GameEventssSO gameEvents;
    public bool _closing;
    public bool _opening;

    private void OnEnable()
    {
        /*gameEvents.onPlayerDeath += Open;
        gameEvents.onPlayerRespawnTap += Close;
        pupUpScreen.onEndTransitionCompleted += OnEndTransitionCompleted;
        pupUpScreen.onStartTransitionCompleted += OnStartTransitionCompleted;*/
    }
    private void OnDisable()
    {
       /* gameEvents.onPlayerDeath -= Open;
        gameEvents.onPlayerRespawnTap -= Close;
        pupUpScreen.onEndTransitionCompleted -= OnEndTransitionCompleted;
        pupUpScreen.onStartTransitionCompleted -= OnStartTransitionCompleted;*/

    }
    void OnEndTransitionCompleted()
    {
        _closing = false;
    }
    void OnStartTransitionCompleted()
    {
        _opening = false;
    }
    public void Open()
    {
        if (_opening || _closing)
        {
            return;
        }
        Scene persistentScene = gameObject.scene;
        if(pupUpScreen.onlyDisable == false)
        {
            Debug.LogError("Death popUp must be set to only disable");
            return;
        }
        _opening = true;
        pupUpScreen.StartTransition(this, persistentScene);
    }
    public void Close()
    {
        Debug.Log("close");
        if (_closing || _opening)
        {
            return;
        }
        _closing = true;
        pupUpScreen.EndTransition();
    }

}
