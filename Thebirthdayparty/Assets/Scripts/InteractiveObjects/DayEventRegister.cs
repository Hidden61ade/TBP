using System;
using Interactable;
using QFramework;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
[RequireComponent(typeof(InteractableObject))]
public class DayEventRegister : MonoBehaviour
{
    public string EventName = "[DEFAULT NAME]";
    public UnityEvent? OnTriggered;
    public UnityEvent? OnRepeated;
    public UnityEvent? OnOtherTriggered;
    private string? m_InteractName;
    private void Start()
    {
        m_InteractName = GetComponent<InteractableObject>().DataName;
        TypeEventSystem.Global.Register<OnDayEventTriggered>(e =>
        {
            if (!(EventName == e.Event.eventName))
            {
                OnOtherTriggered?.Invoke();
                return;
            }
            if (GameSaveManager.Instance.HasInteracted(m_InteractName))
            {
                OnRepeated?.Invoke();
                return;
            }
            OnTriggered?.Invoke();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
}
