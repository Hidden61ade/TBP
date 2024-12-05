using System;
using QFramework;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
public class DayEventRegister : MonoBehaviour
{
    public string EventName;
    public UnityEvent OnTriggered;
    public UnityEvent? OnRepeated;
    public UnityEvent OnOtherTriggered;
    private void Start()
    {
        TypeEventSystem.Global.Register<OnDayEventTriggered>(e =>
        {
            if (!(EventName == e.Event.eventName))
            {
                OnOtherTriggered.Invoke();
                return;
            }
            if (e.Event.completed)
            {
                OnRepeated?.Invoke();
                return;
            }
            OnTriggered.Invoke();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
}
