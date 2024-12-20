using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatEventGetter : MonoBehaviour
{
    public Dictionary<string,string> mapping = new(){
        {"friend_request","Event1"},
        {"venue_discussion","Event2"},
        {"theme_discussion","Event3"}
    };
    private void OnEnable() {
        try
        {
            if(!mapping.ContainsKey(GameTimeManager.Instance.GetCurrentEvent().eventName)){
                return;
            }
            ChatController.Instance.TriggerChat(mapping[GameTimeManager.Instance.GetCurrentEvent().eventName]);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            ChatController.Instance.TriggerChat("Event1");
        }
        
    }
}
