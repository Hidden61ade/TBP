using System.Collections;
using System.Collections.Generic;
using Interactable;
using JetBrains.Annotations;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ComputerUI : MonoSingleton<ComputerUI>
{
    public GameObject m_files;
    public AppIcon files;
    public GameObject m_chat;
    public AppIcon chat;
    private CanvasGroup m_canvasGroup;
    [Header("Cursor style")]
    public Texture2D arrow;
    public Texture2D cursor_3;
    [Header("Post Processing")]
    public Volume volume;
    void Start()
    {
        Cursor.SetCursor(arrow, new Vector2(10, 10), CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        files.SetAction(() =>
        {
            Debug.Log("打开收集菜单");

            StartCoroutine(IEOpenCollections());
            //TODO: 
        });
        chat.SetAction(() =>
        {
            Debug.Log("打开聊天菜单");

            StartCoroutine(IEOpenChat());
            //TODO: 
        });

        m_files.SetActive(false);
        m_chat.SetActive(false);
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_canvasGroup.alpha = 0;
        StartCoroutine(IEInitialize());
    }
    public void Quit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TypeEventSystem.Global.Send<InteractEnd>();
        SceneController.Instance.SwitchSceneTo("SampleScene");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    IEnumerator IEInitialize()
    {
        float a = 0.3f;
        volume.profile.TryGet<Vignette>(out Vignette vignette);
        volume.profile.TryGet<FilmGrain>(out FilmGrain filmGrain);
        while (a > 0.02)
        {
            m_canvasGroup.alpha += Time.deltaTime * 5f;
            a = a - Time.deltaTime < 0 ? 0 : a - Time.deltaTime;
            vignette.intensity.Override(a);
            filmGrain.intensity.Override(a);

            yield return null;
        }
        vignette.intensity.Override(0);
        filmGrain.intensity.Override(0);
        m_canvasGroup.alpha = 1;
    }
    IEnumerator IEOpenCollections()
    {
        Cursor.SetCursor(cursor_3, Vector2.zero, CursorMode.ForceSoftware);
        yield return new WaitForSeconds(0.25f);
        Cursor.SetCursor(arrow, new Vector2(10, 10), CursorMode.ForceSoftware);
        m_files.SetActive(true);
        AddWindow(m_files);
        yield return null;
    }
    IEnumerator IEOpenChat()
    {
        Cursor.SetCursor(cursor_3, Vector2.zero, CursorMode.ForceSoftware);
        yield return new WaitForSeconds(0.25f);
        Cursor.SetCursor(arrow, new Vector2(10, 10), CursorMode.ForceSoftware);
        m_chat.SetActive(true);
        AddWindow(m_chat);
        yield return null;
    }
    #region Window Apperance
    private List<GameObject> windows = new List<GameObject>();  // 存储所有窗口的列表
    private Dictionary<GameObject, int> windowOrderMap = new Dictionary<GameObject, int>();  // 保存窗口和排序值的映射
    private int nmin = 2;  // 最小的sortingOrder值
    public void AddWindow(GameObject window)
    {
        windows.Add(window);
        InitializeWindowOrder(window);
    }

    // 初始化一个新窗口的sortingOrder
    private void InitializeWindowOrder(GameObject window)
    {
        int order = GetMaxOrder() + 1;  // 获取当前最大排序值 + 1
        SetWindowOrder(window, order);
    }

    // 初始化所有窗口的sortingOrder
    private void InitializeWindowOrders()
    {
        int order = nmin;
        foreach (var window in windows)
        {
            SetWindowOrder(window, order);
            order++;
        }
    }

    // 点击窗口时调用的函数
    public void OnWindowClicked(GameObject clickedWindow)
    {
        // 更新该窗口的sortingOrder，使其位于最前方
        int newOrder = GetMaxOrder() + 1;  // 获取当前最大的sortingOrder，然后+1
        SetWindowOrder(clickedWindow, newOrder);

        // 更新其他窗口的排序值，避免排序冲突
        UpdateOtherWindowsOrder(clickedWindow);
    }
    private int GetMaxOrder()
    {
        int maxOrder = nmin;
        foreach (var order in windowOrderMap.Values)
        {
            if (order > maxOrder)
            {
                maxOrder = order;
            }
        }
        return maxOrder;
    }
    private void SetWindowOrder(GameObject window, int order)
    {
        Canvas canvas = window.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = order;
            windowOrderMap[window] = order;  // 更新映射中的排序值
        }
    }
    private void UpdateOtherWindowsOrder(GameObject clickedWindow)
    {
        int order = nmin;
        foreach (var window in windows)
        {
            if (window != clickedWindow)
            {
                SetWindowOrder(window, order);
                order++;
            }
        }
    }
    public void RemoveWindow(GameObject window)
    {
        if (windows.Contains(window))
        {
            windows.Remove(window);
            windowOrderMap.Remove(window);  // 删除映射
        }
    }
    #endregion
}
