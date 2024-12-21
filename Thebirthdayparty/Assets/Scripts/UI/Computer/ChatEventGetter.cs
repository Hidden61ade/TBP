using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatEventGetter : MonoBehaviour
{
    public Dictionary<string,string> mapping = new(){
        {"friend_request","Event1"},
        {"venue_discussion","Event2"},
        {"theme_discussion","Event3"},
        {"food_discussion","Event4"},
        {"program_discussion","Event5"},
        {"george_chat","Event6"},
        {"terminal_delivery","Event7"}
    };
    private void OnEnable() {
        try
        {
            if(!mapping.ContainsKey(GameTimeManager.Instance.GetCurrentEvent().eventName)){
                return;
            }
            string eventIdx = mapping[GameTimeManager.Instance.GetCurrentEvent().eventName];
            if(eventIdx.Equals("Event6")||eventIdx.Equals("Event7")){
                MessageSend.currentCharacter = "George";
            }else{
                MessageSend.currentCharacter = "Adam";
            }
            ChatController.Instance.TriggerChat(eventIdx);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            ChatController.Instance.TriggerChat("Event1");
        }
        
    }
}
