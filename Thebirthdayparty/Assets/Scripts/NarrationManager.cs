using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private AudioSource narratorVoice;
    [SerializeField] private TextMeshProUGUI subtitleText;

    [Header("Settings")]
    [SerializeField] private float textFadeSpeed = 2f;
    [SerializeField] private float minimumDelayBetweenNarrations = 0.5f;
    [SerializeField] [Range(0, 1)] private float narratorVolume = 1f;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    private Queue<NarrationEvent> narrationQueue = new Queue<NarrationEvent>();
    private bool isNarrating = false;
    private Coroutine currentNarrationCoroutine;
    private Coroutine currentSubtitleCoroutine;

    // 事件系统
    public UnityEvent<NarrationEvent> OnNarrationStart = new UnityEvent<NarrationEvent>();
    public UnityEvent<NarrationEvent> OnNarrationEnd = new UnityEvent<NarrationEvent>();
    public UnityEvent OnNarrationQueueEmpty = new UnityEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeComponents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeComponents()
    {
        // 如果没有手动赋值，尝试自动获取组件
        if (narratorVoice == null)
            narratorVoice = GetComponent<AudioSource>();
        
        if (narratorVoice == null)
            narratorVoice = gameObject.AddComponent<AudioSource>();

        // 配置AudioSource
        narratorVoice.playOnAwake = false;
        narratorVoice.spatialBlend = 0f; // 2D音效
        narratorVoice.volume = narratorVolume;

        if (subtitleText == null)
            Debug.LogWarning("NarrationManager: No subtitle text component assigned!");
    }

    public void TriggerNarration(NarrationEvent[] possibleNarrations)
    {
        if (possibleNarrations == null || possibleNarrations.Length == 0)
        {
            DebugLog("No narrations provided to trigger.");
            return;
        }

        NarrationEvent selectedNarration = ChooseNarration(possibleNarrations);
        if (selectedNarration != null)
        {
            narrationQueue.Enqueue(selectedNarration);
            if (!isNarrating)
            {
                StartNarrationQueue();
            }
        }
    }

    public void StopCurrentNarration()
    {
        if (currentNarrationCoroutine != null)
            StopCoroutine(currentNarrationCoroutine);
        if (currentSubtitleCoroutine != null)
            StopCoroutine(currentSubtitleCoroutine);

        narratorVoice.Stop();
        ClearSubtitle();
        isNarrating = false;
    }

    public void ClearNarrationQueue()
    {
        narrationQueue.Clear();
        StopCurrentNarration();
        OnNarrationQueueEmpty.Invoke();
    }

    private NarrationEvent ChooseNarration(NarrationEvent[] narrations)
    {
        foreach (var narration in narrations)
        {
            if (narration != null && narration.ConditionsMet())
                return narration;
        }
        DebugLog("No suitable narration found among the possibilities.");
        return null;
    }

    private void StartNarrationQueue()
    {
        if (currentNarrationCoroutine != null)
            StopCoroutine(currentNarrationCoroutine);
        currentNarrationCoroutine = StartCoroutine(PlayNarrationQueue());
    }

    private IEnumerator PlayNarrationQueue()
    {
        isNarrating = true;

        while (narrationQueue.Count > 0)
        {
            NarrationEvent currentNarration = narrationQueue.Dequeue();

            // 检查音频文件
            if (currentNarration.audioClip == null)
            {
                DebugLog($"Warning: Narration {currentNarration.eventID} has no audio clip!");
                continue;
            }

            // 触发开始事件
            OnNarrationStart.Invoke(currentNarration);

            // 播放音频
            narratorVoice.clip = currentNarration.audioClip;
            narratorVoice.Play();

            // 显示字幕
            if (currentSubtitleCoroutine != null)
                StopCoroutine(currentSubtitleCoroutine);
            currentSubtitleCoroutine = StartCoroutine(ShowSubtitle(currentNarration.subtitleText));

            // 等待音频播放完成
            float narrationDuration = currentNarration.audioClip.length;
            yield return new WaitForSeconds(narrationDuration);

            // 触发结束事件
            OnNarrationEnd.Invoke(currentNarration);

            // 额外延迟
            yield return new WaitForSeconds(currentNarration.delayAfterNarration);

            // 最小间隔
            yield return new WaitForSeconds(minimumDelayBetweenNarrations);
        }

        isNarrating = false;
        OnNarrationQueueEmpty.Invoke();
    }

    private IEnumerator ShowSubtitle(string text)
    {
        if (subtitleText == null) yield break;

        // 淡入
        subtitleText.text = text;
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * textFadeSpeed;
            subtitleText.alpha = Mathf.Clamp01(alpha);
            yield return null;
        }

        // 等待音频播放
        yield return new WaitForSeconds(narratorVoice.clip.length);

        // 淡出
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * textFadeSpeed;
            subtitleText.alpha = Mathf.Clamp01(alpha);
            yield return null;
        }

        ClearSubtitle();
    }

    private void ClearSubtitle()
    {
        if (subtitleText != null)
        {
            subtitleText.text = "";
            subtitleText.alpha = 0;
        }
    }

    // 属性访问器
    public bool IsNarrating => isNarrating;
    public int QueueCount => narrationQueue.Count;
    public float CurrentNarrationTime => narratorVoice.time;
    public float CurrentNarrationLength => narratorVoice.clip != null ? narratorVoice.clip.length : 0f;

    // 音量控制
    public void SetNarratorVolume(float volume)
    {
        narratorVolume = Mathf.Clamp01(volume);
        narratorVoice.volume = narratorVolume;
    }

    private void DebugLog(string message)
    {
        if (showDebugLogs)
            Debug.Log($"[NarrationManager] {message}");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}