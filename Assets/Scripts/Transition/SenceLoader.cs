using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SenceLoader : MonoBehaviour
{

    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;//场景加载事件SO
    public VoidEventSO afterSceneLoadingEvent;
    public VoidEventSO newGameEvent;

    public Transform playerTransform;//玩家位置

    public Vector3 firstPosition;
    public Vector3 menuPosition;//菜单场景位置
    public GameScenceSO firstLoadScene;//初始加载场景
    public GameScenceSO menuScene;//菜单场景


    private GameScenceSO currentGameScene;//当前场景
    private bool isLoadingScene;//是否正在加载场景

    private GameScenceSO gameScence;//作为的目标场景
    private Vector3 playerPos;//作为传送点的目标位置
    private bool fadeScreen;//作为传送点的目标位置

    public float fadeDuration = 1f; // 淡入淡出持续时间

    private void Awake()
    {
 
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequsetEvent(menuScene, menuPosition, true);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent+= OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        gameScence = firstLoadScene;
        //OnLoadRequestEvent(gameScence,firstPosition, true);
        loadEventSO.RaiseLoadRequsetEvent(gameScence, firstPosition, true);
    }
    private void OnLoadRequestEvent(GameScenceSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoadingScene)
        {
            Debug.LogWarning("场景正在加载中，请稍后再试");
            return;
        }
        isLoadingScene = true;
        gameScence = locationToLoad;
        playerPos = posToGo;
        this.fadeScreen = fadeScreen;
        
        StartCoroutine(UnLoadPreviousScene());
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //TODO场景切换时的淡入淡出效果
        }

        yield return new WaitForSeconds(fadeDuration);
        if (currentGameScene != null)
        {
            yield return currentGameScene.sceneReference.UnLoadScene();
        }

        playerTransform.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = gameScence.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// 场景加载完成后
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentGameScene = gameScence;

        playerTransform.position = playerPos;
        playerTransform.gameObject.SetActive(true);

        if (fadeScreen)
        {
            //TODO场景切换时的淡入淡出效果
        }

        isLoadingScene = false;

        //场景完成后事件
        if(currentGameScene.sceneType != SceneType.Menu)
            afterSceneLoadingEvent.RaiseEvent();
    } 
}
