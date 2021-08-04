using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SceneTransitionSO", menuName = "Scene Data/Scene Transition")]
public class SceneTransitionSO : ScriptableObject
{
    public static SceneTransitionSO currentTransition { get; private set; }
    public static bool isTransition { get; private set; }

    public GameObject transitionCanvas;
    public System.Action onStartTransitionCompleted;
    public System.Action onEndTransitionCompleted;
    public bool hasLoadingBar = false;
    public bool onlyDisable = false;

    private GameObject transitionCanvasInstance;
    private Canvas mainCanvas;
    private Slider loadingProgressSlider;
    private Animator animator;
    private RuntimeAnimatorController animatorController;
    private MonoBehaviour coroutineCaller;
    private bool hasAnimator = false;
    private Scene persistentScene;
    private Coroutine StartCoroutine, EndCoroutine;
    private bool started;

    /// <summary>
    /// In most cases pass "this" as "coroutineCaller". 
    /// Instantiates transitionCanvas in persistent scene, waits for end of start-transition 
    /// animation and fires the "onStartTransitionCompleted" event (System.Action).
    /// Call "EndTransition" to play end-transition animation. Fires "onEndTransitionCompleted" 
    /// event (System.Action) on end-transition animation completion. Returns instance of transition canvas
    /// </summary>
    /// <param name="coroutineCaller">in most cases just pass "this"</param>
    public GameObject StartTransition(MonoBehaviour coroutineCaller, Scene persistentScene)
    {

        if (StartCoroutine != null)
            coroutineCaller.StopCoroutine(StartCoroutine);
        if (EndCoroutine != null)
            coroutineCaller.StopCoroutine(EndCoroutine);

        this.persistentScene = persistentScene;
        SceneTransitionSO.SetAsCurrentTransition(this);
        Scene oldActiveScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(this.persistentScene);
        if (transitionCanvasInstance == null)
        {
            transitionCanvasInstance = Instantiate(transitionCanvas);
        }
        else
        {
            mainCanvas.enabled = true;
        }
        if (SceneManager.GetSceneByBuildIndex(oldActiveScene.buildIndex).isLoaded)
        {
            SceneManager.SetActiveScene(oldActiveScene);
        }
        this.coroutineCaller = coroutineCaller;
        CheckComponents();
        if (!hasAnimator)
        {
            onStartTransitionCompleted?.Invoke();
            return transitionCanvasInstance;
        }
        if (hasLoadingBar)
        {
            loadingProgressSlider.value = 0;
        }
        animatorController = animator.runtimeAnimatorController;
        float transitionTime = animatorController.animationClips[0].length;
        animator.ResetTrigger("end");
        animator.SetTrigger("start");
        StartCoroutine = this.coroutineCaller.StartCoroutine(StartTransitionCompletedCaller(transitionTime));
        started = true;
        return transitionCanvasInstance;
    }

    /// <summary>
    /// Plays end-transition animation. Fires "onEndTransitionCompleted" 
    /// event (System.Action) on end-transition animation completion.
    /// </summary>
    public void EndTransition()
    {
        if (!started)
        {
            return;
        }
        started = false;
        if (StartCoroutine != null)
            coroutineCaller.StopCoroutine(StartCoroutine);
        if (EndCoroutine != null)
            coroutineCaller.StopCoroutine(EndCoroutine);

        float transitionTime = animatorController.animationClips[1].length;
        animator.ResetTrigger("start");
        animator.SetTrigger("end");
        EndCoroutine = coroutineCaller.StartCoroutine(EndTransitionCompletedCaller(transitionTime));
    }

    public void SetProgressValue(float value)
    {
        if (hasLoadingBar)
        {
            loadingProgressSlider.value = value;
        }
    }

    private static void SetAsCurrentTransition(SceneTransitionSO transition)
    {
        currentTransition = transition;
        isTransition = true;
    }
    private static void OnTransitionEnded()
    {
        isTransition = false;
    }
    private IEnumerator StartTransitionCompletedCaller(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        onStartTransitionCompleted?.Invoke();
    }

    private IEnumerator EndTransitionCompletedCaller(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneTransitionSO.OnTransitionEnded();
        if (onlyDisable)
        {
            mainCanvas.enabled = false;
        }
        else
        {
            Destroy(transitionCanvasInstance);
            transitionCanvasInstance = null;
        }

        onEndTransitionCompleted?.Invoke();
    }
    
    private void CheckComponents()
    {
        GetAnimator();
        GetSlider();
        GetCanvas();
    }
    private void GetAnimator()
    {
        animator = transitionCanvasInstance.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            hasAnimator = true;
        }
        else
        {
            Debug.LogError(transitionCanvasInstance + " does not have an Animator component");
            hasAnimator = false;
        }
    }
    private void GetSlider()
    {
        loadingProgressSlider = transitionCanvasInstance.GetComponentInChildren<Slider>();
        if (loadingProgressSlider != null)
        {
            hasLoadingBar = true;
        }
        else
        {
            hasLoadingBar = false;
        }
    }
    private void GetCanvas()
    {
        mainCanvas = transitionCanvasInstance.GetComponent<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogWarning("No canvas in top object " + transitionCanvasInstance);
        }
    }
}
