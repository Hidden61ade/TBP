using System.Collections;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    Coroutine currentCoroutine;
    private bool hasInited = false;
    private void Start()
    {
        LoadingPanel.Instance.gameObject.SetActive(false);
        StartCoroutine(IEInitialize());
    }
    IEnumerator IEInitialize(){
        if(hasInited) yield break;
        SceneManager.LoadScene("StartScene",LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartScene"));
        hasInited = true;
    }

    // 加载场景
    public void LoadScene(string name)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(IEAsyncLoadScene(name));
    }
    // 加载附加场景（不卸载当前场景）
    public void LoadSceneAdditive(string name, bool autoSwitch = true)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(IELoadSceneAdditive(name, autoSwitch));
    }
    // 切换场景（卸载当前场景后加载新场景）
    public void SwitchScene(string name)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(IEAsyncSwitchScene(name));
    }
    public void SwitchSceneTo(string name)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        StartCoroutine(IEAsyncSwitchSceneTo(name));
        TypeEventSystem.Global.Send<OnSceneLoadedEvent>();
    }
    public void SwitchSceneWithRelease(string name)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(IEAsyncSwitchSceneWithRelease(name));
    }


    private IEnumerator IEAsyncLoadScene(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            yield return null;
        }

        TypeEventSystem.Global.Send<OnSceneLoadedEvent>();
        currentCoroutine = null;
    }
    private IEnumerator IELoadSceneAdditive(string name, bool autoSwitch)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!operation.isDone)
        {
            LoadingPanel.Instance.SetPercentage(operation.progress * 100f);
            yield return null;
        }

        if (autoSwitch)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        }

        TypeEventSystem.Global.Send<OnSceneLoadedEvent>();
        currentCoroutine = null;
    }
    private IEnumerator IEAsyncSwitchSceneTo(string name)
    {
        // 获取当前活动的场景
        Scene currentScene = SceneManager.GetActiveScene();

        // 获取目标场景
        Scene targetScene = SceneManager.GetSceneByName(name);

        if (!targetScene.IsValid() || !targetScene.isLoaded)
        {
            Debug.LogError($"Scene '{name}' is not loaded. Make sure the scene is loaded before calling SwitchSceneTo.");
            yield break;
        }

        // 将目标场景设置为激活场景
        SceneManager.SetActiveScene(targetScene);

        // 卸载当前活动的场景
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        // 发送场景切换完成事件
        TypeEventSystem.Global.Send<OnSceneLoadedEvent>();

        currentCoroutine = null;
    }
    private IEnumerator IEAsyncSwitchScene(string name)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!unloadOperation.isDone)
        {
            yield return null;
        }
        //伪加载
        LoadingPanel.Instance.gameObject.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name,LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;
        float fakeProgress = 0f;
        float realProgress = 0f;

        while (fakeProgress < 100f)
        {
            realProgress = Mathf.Clamp01(loadOperation.progress / 0.9f) * 100f;
            if (fakeProgress < realProgress)
            {
                fakeProgress += Time.deltaTime * 30f;
            }
            else
            {
                fakeProgress += Time.deltaTime * 20f;
            }
            LoadingPanel.Instance.SetPercentage(fakeProgress);
            if (fakeProgress >= 99f && loadOperation.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }
        fakeProgress = 100f;
        LoadingPanel.Instance.SetPercentage(fakeProgress);
        LoadingPanel.Instance.gameObject.SetActive(false);
        loadOperation.allowSceneActivation = true;
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        TypeEventSystem.Global.Send<OnSceneLoadedEvent>();
        currentCoroutine = null;
    }
    private IEnumerator IEAsyncSwitchSceneWithRelease(string name)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name);
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        TypeEventSystem.Global.Send<OnSceneLoadedEvent>();
        currentCoroutine = null;
    }
}
