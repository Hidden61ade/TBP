using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "new ChatData", menuName = "ChatData/Messages Data")]

public class MessagesData : ScriptableObject
{
    public string EventId;
    public MsgData[] msgDatas;
}
[Serializable]
public class MsgData{
    public enum ChoiceMapping{
        Defult,
        One,
        Two,
        Three
    }
    public bool hasChoice;

    [TextArea]
    public string Content;
    public string[] Choices;
    [Header("选项映射")]
    public bool isSecondary;
    public ChoiceMapping mapping;
}