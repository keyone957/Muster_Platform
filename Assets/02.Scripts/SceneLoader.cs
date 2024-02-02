using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 씬 관리, 씬을 스택 형식으로 관리하기 위해
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-12

public enum SceneName
{
    None,
    Title,
    Start,
    Stage
}
public class SceneLoader : MonoBehaviour
{
    // Singleton
    public static SceneLoader _instance;

    // Fade In
    FadeOverlay fadeOverlay;
    [Range(0.5f, 2.0f)]
    [SerializeField] float _fadeDuration = 3.0f;

    // Scene 관련 정보
    [Header("for Debug")]
    [SerializeField] SceneName befScene = SceneName.None;
    [SerializeField] SceneName curScene = SceneName.None;

    private void Awake()
    {
        // Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        fadeOverlay = FindObjectOfType<FadeOverlay>(true);
    }

    public async UniTask LoadScene(SceneName sceneName)
    {
        // Fade In 함수 호출
        fadeOverlay.DoFadeOut(_fadeDuration);
        await UniTask.Delay((int)(_fadeDuration * 1000));

/*
        // 기존의 씬을 언로드
        if (sceneName == SceneName.Start)
        {
            if(curScene == SceneName.Title)
                await SceneManager.UnloadSceneAsync(SceneName.Title.ToString());
            else if(curScene == SceneName.Stage)
                await SceneManager.UnloadSceneAsync(SceneName.Stage.ToString());

        }
        else if (sceneName == SceneName.Stage)
        {
            await SceneManager.UnloadSceneAsync(SceneName.Start.ToString());
        }*/

        // 씬을 로드
        // await SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
        await SceneManager.LoadSceneAsync(sceneName.ToString());
        befScene = curScene;
        curScene = sceneName;

        // Fade Out 함수 호출
        fadeOverlay.DoFadeIn(_fadeDuration*2);
    }
}
