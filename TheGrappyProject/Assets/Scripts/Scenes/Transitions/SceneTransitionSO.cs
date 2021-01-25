using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SceneTransitionSO", menuName = "Scene Data/Scene Transition")]
public class SceneTransitionSO : ScriptableObject
{
    public static SceneTransitionSO currentTransition { get; private set; }
    public static bool isTransition { get; private set; }

    public GameObject transitionCanvas;
    public UnityAction onStartTransitionCompleted;
    public UnityAction onEndTransitionCompleted;
    public bool hasLoadingBar = false;

    private GameObject _transitionCanvasInstance;
    private Slider _loadingProgressSlider;
    private Animator _animator;
    private RuntimeAnimatorController _animatorController;
    private MonoBehaviour _coroutineCaller;
    private bool _hasAnimator = false;
    private Scene _persistentScene;
    static void SetAsCurrentTransition(SceneTransitionSO transition)
    {
        currentTransition = transition;
        isTransition = true;
    }
    static void OnTransitionEnded()
    {
        isTransition = false;
    }

    /// <summary>
    /// In most cases pass "this" as "coroutineCaller". 
    /// Instantiates transitionCanvas in persistent scene, waits for end of start-transition 
    /// animation and fires the "onStartTransitionCompleted" event (UnityAction).
    /// Call "EndTransition" to play end-transition animation. Fires "onEndTransitionCompleted" 
    /// event (UnityAction) on end-transition animation completion.
    /// </summary>
    /// <param name="coroutineCaller">in most cases just pass "this"</param>
    public void StartTransition(MonoBehaviour coroutineCaller, Scene persistentScene)
    {
        _persistentScene = persistentScene;
        Debug.Log("StartTransition");
        SceneTransitionSO.SetAsCurrentTransition(this);
        Scene oldActiveScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(_persistentScene);
        _transitionCanvasInstance = Instantiate(transitionCanvas);
        SceneManager.SetActiveScene(oldActiveScene);
        _transitionCanvasInstance.GetComponent<Canvas>().sortingOrder = 255;
        _coroutineCaller = coroutineCaller;
        CheckComponents();
        if (!_hasAnimator)
        {
            return;
        }
        if (hasLoadingBar)
        {
            _loadingProgressSlider.value = 0;
        }
        _animatorController = _animator.runtimeAnimatorController;
        float transitionTime = _animatorController.animationClips[0].length;
        _animator.ResetTrigger("end");
        _animator.SetTrigger("start");
        _coroutineCaller.StartCoroutine(StartTransitionCompletedCaller(transitionTime));
    }
    IEnumerator StartTransitionCompletedCaller(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        onStartTransitionCompleted?.Invoke();
    }
    /// <summary>
    /// Plays end-transition animation. Fires "onEndTransitionCompleted" 
    /// event (UnityAction) on end-transition animation completion.
    /// </summary>
    public void EndTransition()
    {
        Debug.Log("End transition");
        float transitionTime = _animatorController.animationClips[1].length;
        _animator.ResetTrigger("start");
        _animator.SetTrigger("end");
        _coroutineCaller.StartCoroutine(EndTransitionCompletedCaller(transitionTime));
    }
    IEnumerator EndTransitionCompletedCaller(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneTransitionSO.OnTransitionEnded();
        onEndTransitionCompleted?.Invoke();
        Destroy(_transitionCanvasInstance);
    }
    public void SetProgressValue(float value)
    {
        if (hasLoadingBar)
        {
            _loadingProgressSlider.value = value;
        }
    }
    void CheckComponents()
    {
        GetAnimator();
        GetSlider();
    }
    void GetAnimator()
    {
        _animator = _transitionCanvasInstance.GetComponentInChildren<Animator>();
        if(_animator != null)
        {
            _hasAnimator = true;
        }
        else
        {
            Debug.LogError(_transitionCanvasInstance + " does not have an Animator component");
            _hasAnimator = false;
        }
    }
    void GetSlider()
    {
        _loadingProgressSlider = _transitionCanvasInstance.GetComponentInChildren<Slider>();
        if(_loadingProgressSlider != null)
        {
            hasLoadingBar = true;
        }
        else
        {
            hasLoadingBar = false;
        }
    }
    private void OnEnable()
    {
    }
}
