using System.Collections;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class MessageSend : MonoBehaviour
{
    public Transform contentParent;
    public GameObject MsgPrefab;
    private void Start()
    {

    }
    public string contToSend;
    [InspectorButton("Send")]
    void Send()
    {
        var temp = Instantiate(MsgPrefab, contentParent);
        temp.GetComponent<ComputerMsg.Message>().SetText(contToSend);
    }
    public void Send(string str)
    {
        //Proceed Control
        if(str.ToCharArray()[0]=='#'){
            switch(str){
                case "#SYS:PASS":
                    break;
                case "#SYS:ETD":
                    Debug.LogError("Error Text Defult");
                    break;
            }
            return;
        }
        var temp = Instantiate(MsgPrefab, contentParent);
        temp.GetComponent<ComputerMsg.Message>().SetText(str);
    }
}
