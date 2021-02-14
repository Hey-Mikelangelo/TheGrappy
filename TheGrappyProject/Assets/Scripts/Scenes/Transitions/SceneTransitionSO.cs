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
    public bool onlyDisable = false;

    private GameObject _transitionCanvasInstance;
    private Canvas _mainCanvas;
    private Slider _loadingProgressSlider;
    private Animator _animator;
    private RuntimeAnimatorController _animatorController;
    private MonoBehaviour _coroutineCaller;
    private bool _hasAnimator = false;
    private Scene _persistentScene;
    private Coroutine StartCoroutine, EndCoroutine;
    private static bool _isInstantiated = false;
    private bool started;
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
    /// event (UnityAction) on end-transition animation completion. Returns instance of transition canvas
    /// </summary>
    /// <param name="coroutineCaller">in most cases just pass "this"</param>
    public GameObject StartTransition(MonoBehaviour coroutineCaller, Scene persistentScene)
    {

        if (StartCoroutine != null)
            coroutineCaller.StopCoroutine(StartCoroutine);
        if (EndCoroutine != null)
            coroutineCaller.StopCoroutine(EndCoroutine);

        _persistentScene = persistentScene;
        SceneTransitionSO.SetAsCurrentTransition(this);
        Scene oldActiveScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(_persistentScene);
        if (_transitionCanvasInstance == null)
        {
            _transitionCanvasInstance = Instantiate(transitionCanvas);
        }
        else
        {
            _mainCanvas.enabled = true;
        }
        _isInstantiated = true;
        if (SceneManager.GetSceneByBuildIndex(oldActiveScene.buildIndex).isLoaded)
        {
            SceneManager.SetActiveScene(oldActiveScene);
        }
        else
        {

        }
        _coroutineCaller = coroutineCaller;
        CheckComponents();
        if (!_hasAnimator)
        {
            onStartTransitionCompleted?.Invoke();
            return _transitionCanvasInstance;
        }
        if (hasLoadingBar)
        {
            _loadingProgressSlider.value = 0;
        }
        _animatorController = _animator.runtimeAnimatorController;
        float transitionTime = _animatorController.animationClips[0].length;
        _animator.ResetTrigger("end");
        _animator.SetTrigger("start");
        StartCoroutine = _coroutineCaller.StartCoroutine(StartTransitionCompletedCaller(transitionTime));
        started = true;
        return _transitionCanvasInstance;
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
        if (!started)
        {
            return;
        }
        started = false;
        if (StartCoroutine != null)
            _coroutineCaller.StopCoroutine(StartCoroutine);
        if (EndCoroutine != null)
            _coroutineCaller.StopCoroutine(EndCoroutine);

        float transitionTime = _animatorController.animationClips[1].length;
        _animator.ResetTrigger("start");
        _animator.SetTrigger("end");
        EndCoroutine = _coroutineCaller.StartCoroutine(EndTransitionCompletedCaller(transitionTime));
    }
    IEnumerator EndTransitionCompletedCaller(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneTransitionSO.OnTransitionEnded();
        if (onlyDisable)
        {
            _mainCanvas.enabled = false;
        }
        else
        {            
            Destroy(_transitionCanvasInstance);
            _isInstantiated = false;
            _transitionCanvasInstance = null;
        }

        onEndTransitionCompleted?.Invoke();
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
        GetCanvas();
    }
    void GetAnimator()
    {
        _animator = _transitionCanvasInstance.GetComponentInChildren<Animator>();
        if (_animator != null)
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
        if (_loadingProgressSlider != null)
        {
            hasLoadingBar = true;
        }
        else
        {
            hasLoadingBar = false;
        }
    }
    void GetCanvas()
    {
        _mainCanvas = _transitionCanvasInstance.GetComponent<Canvas>();
        if (_mainCanvas == null)
        {
            Debug.LogWarning("No canvas in top object " + _transitionCanvasInstance);
        }
    }
    private void OnEnable()
    {
    }
}
