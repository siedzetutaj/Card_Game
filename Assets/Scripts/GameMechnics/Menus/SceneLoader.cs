using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviourSingletonPersistent<SceneLoader>
{
    //Constant variables
    private const float TIME_FOR_TRANSITION = 1f;

    //Editor-assigned variables
    [SerializeField] private GameObject _transitionGO;
    [SerializeField] private Animator _transitionAnim;
    [SerializeField] private bool _isLoading = false;

    //Other variables
    [SerializeField] private string _nextSceneName;

    //Properties
    public string CurrentSceneName => SceneManager.GetActiveScene().name;

    private void Start()
    {
        DontDestroyOnLoad(this);
        HandleMenuTransition(false);
    }

    private void HandleMenuTransition(bool isHidingScreen, System.Action nextAction = null)
    {
        StartCoroutine(WaitForMenuTransition(isHidingScreen, nextAction));
    }   

    private IEnumerator WaitForMenuTransition(bool isHidingScreen, System.Action nextAction)
    {
        _transitionGO.SetActive(true);

        if (isHidingScreen)
        {
            _transitionAnim.SetBool("Check", isHidingScreen);
            yield return new WaitForSecondsRealtime(TIME_FOR_TRANSITION);
            nextAction?.Invoke();
        }
        else
        {
            nextAction?.Invoke();
            
            yield return new WaitForSecondsRealtime(TIME_FOR_TRANSITION);
            _transitionAnim.SetBool("Check", isHidingScreen);
        }
    }

    private void HandleLoadingAsynchronously()
    {
        StartCoroutine(LoadAsynchronously());
    }

    public void LoadScene(string newSceneName)
    {
        if (_isLoading) return;

        _isLoading = true;

        _nextSceneName = newSceneName;

        HandleMenuTransition(true, HandleLoadingAsynchronously);
    }

    private IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_nextSceneName);

        while (!operation.isDone)
            yield return null;
            
        

        HandleMenuTransition(false);

        _isLoading = false;                   
    }
}



