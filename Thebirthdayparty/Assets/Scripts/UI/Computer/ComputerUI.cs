using System.Collections;
using Interactable;
using JetBrains.Annotations;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ComputerUI : MonoSingleton<ComputerUI>
{
    private GameObject m_collections;
    private GameObject m_chat;
    private CanvasGroup m_canvasGroup;
    [Header("Cursor style")]
    public Texture2D arrow;
    public Texture2D cursor_3;
    [Header("Post Processing")]
    public Volume volume;
    void Start()
    {
        Cursor.SetCursor(arrow,new Vector2(10,10),CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_collections = transform.Find("Collections").gameObject;
        m_chat = transform.Find("Chat").gameObject;

        transform.Find("Desktop/CollectionIcon").GetComponent<AppIcon>().SetAction(()=>{
            Debug.Log("打开收集菜单");
            
            StartCoroutine(IEOpenCollections());
            //TODO: 
        });
        transform.Find("Desktop/ChatIcon").GetComponent<AppIcon>().SetAction(()=>{
            Debug.Log("打开聊天菜单");

            StartCoroutine(IEOpenChat());
            //TODO: 
        });

        m_collections.SetActive(false);
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
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
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
    IEnumerator IEOpenCollections(){
        Cursor.SetCursor(cursor_3,Vector2.zero,CursorMode.ForceSoftware);
        yield return new WaitForSeconds(0.25f);
        Cursor.SetCursor(arrow,new Vector2(10,10),CursorMode.ForceSoftware);
        m_collections.SetActive(true);
        yield return null;
    }
    IEnumerator IEOpenChat(){
        Cursor.SetCursor(cursor_3,Vector2.zero,CursorMode.ForceSoftware);
        yield return new WaitForSeconds(0.25f);
        Cursor.SetCursor(arrow,new Vector2(10,10),CursorMode.ForceSoftware);
        m_chat.SetActive(true);
        yield return null;
    }
}
