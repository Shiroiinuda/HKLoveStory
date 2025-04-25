using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public static LoadSceneManager instance;
    public GameObject LoadingScreen;
    [SerializeField] private GameObject bufferPannel;
    //public Image LoadingBarFill;

    public Animator animator;

    private int levelToLoad;

    public List<RenderTexture> renderTexList;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        foreach(RenderTexture rt in renderTexList)
        {
            rt.Release();
        }
        StartCoroutine(BufferOff());
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
        bufferPannel.SetActive(true);
    }
    
    private IEnumerator BufferOff()
    {
        bufferPannel.SetActive(true);
        yield return new WaitForSeconds(1);
        bufferPannel.SetActive(false);
    }

    public void OnFadeComplete()
    {
        LoadScene(levelToLoad);
    }

    private void LoadScene(int sceneNumber)
    {
        LoadingScreen.SetActive(true);
// #if UNITY_ANDROID || UNITY_IOS
//         SceneManager.LoadScene(sceneNumber);
//         return;
// #endif
        StartCoroutine(LoadSceneAsync(sceneNumber));
    }

    public Slider loadingSlider;
    //Fade With Loading Screen
    IEnumerator LoadSceneAsync(int sceneID)
    {
        LoadingScreen.SetActive(true);
        animator.SetTrigger("Load");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);



        operation.allowSceneActivation = false;

        while (operation.isDone == false)
        {
            if(loadingSlider != null)
            loadingSlider.value = operation.progress / 0.9f;
            if (operation.progress == 0.9f)
            {
                if(loadingSlider != null)
                loadingSlider.value = 1f;
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    //Fade Without Loading Screen
    public void FadeToLevelWithoutLoadingScreen(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOutNoScreen");
        bufferPannel.SetActive(true);
    }

    public void OnFadeCompleteWithoutLoadingScreen()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    //Fade To Reload Current Scene
    public void GetCurrentSceneThenFade()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }

}
