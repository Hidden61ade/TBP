using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatEventGetter : MonoBehaviour
{
    public Dictionary<string,string> mapping = new(){
        {"friend_request","Event1"},
        {"venue_discussion","Event2"}
    };
    private void OnEnable() {
        try
        {
            ChatController.Instance.TriggerChat(mapping[GameTimeManager.Instance.GetCurrentEvent().eventName]);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            ChatController.Instance.TriggerChat("Test");
        }
        
    }
}
