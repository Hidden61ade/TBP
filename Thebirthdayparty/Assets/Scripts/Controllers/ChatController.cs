using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoSingleton<ChatController>
{
    public Button Choice1;
    public Button Choice2;
    public Button Choice3;
    private void Start()
    {
        TypeEventSystem.Global.Register<OnChose>(e =>
        {
            choice = e.index;
        });
        TypeEventSystem.Global.Register<OnChoiceActivated>(e =>
        {
            Choice1.GetComponentInChildren<TextMeshProUGUI>().SetText(e.choices[0]);
            Choice2.GetComponentInChildren<TextMeshProUGUI>().SetText(e.choices[1]);
            Choice3.GetComponentInChildren<TextMeshProUGUI>().SetText(e.choices[2]);
        });
        Choice1.onClick.AddListener(() =>
        {
            TypeEventSystem.Global.Send<OnChose>(new(1));
            ClearChooseText();
        });
        Choice2.onClick.AddListener(() =>
        {
            TypeEventSystem.Global.Send<OnChose>(new(2));
            ClearChooseText();
        });
        Choice3.onClick.AddListener(() =>
        {
            TypeEventSystem.Global.Send<OnChose>(new(3));
            ClearChooseText();
        });
    }
    void ClearChooseText()
    {
        Choice1.GetComponentInChildren<TextMeshProUGUI>().SetText("");
        Choice2.GetComponentInChildren<TextMeshProUGUI>().SetText("");
        Choice3.GetComponentInChildren<TextMeshProUGUI>().SetText("");
    }
    private void OnEnable()
    {
        //Test only
        TriggerChat("Event1");
    }

    class MsgNode
    {
        public string content;
        public bool hasChoice;
        public string[] choices = new string[3];
        public MsgNode[] linkToBranches = new MsgNode[4];
        int index;
        public MsgNode(MsgData msgData)
        {
            this.content = msgData.Content;
            this.hasChoice = msgData.hasChoice;
        }
    }
    public MessageSend messageSender;
    [SerializeField] private int choice = -1;

    public void TriggerChat(string EventId)
    {
        var data = Resources.Load("Computer/Chats/" + EventId) as MessagesData;
        MsgData[] msgs = data.msgDatas;
        MsgNode head = GenerateChatGraph(msgs);
        StartCoroutine(ProceedSendingChat(head));
    }
    MsgNode GenerateChatGraph(MsgData[] msgs)
    {
        if (msgs == null || msgs.Length == 0)
        {
            return null;
        }
        // 创建头节点
        MsgNode head = new MsgNode(msgs[0]);
        // 当前处理的节点
        MsgNode currentNode = head;
        // 遍历 MsgData 数组来建立树结构
        bool branchesIsEmpty = true;
        MsgNode[] curBranches = new MsgNode[4];
        for (int i = 1; i < msgs.Length; i++)
        {
            MsgNode newNode = new(msgs[i]);
            if (!msgs[i].isSecondary)
            {
                if (msgs[i].hasChoice)
                {
                    newNode.choices = msgs[i].Choices;
                }
                if (branchesIsEmpty)
                {
                    currentNode.linkToBranches[0] = newNode;
                    currentNode = newNode;
                }
                else
                {
                    currentNode.linkToBranches = curBranches;
                    for (int j = 0; j < 4; j++)
                    {
                        if (curBranches[j] is not null)
                        {
                            curBranches[j].linkToBranches[0] = newNode;
                        }
                    }
                    curBranches = new MsgNode[4];
                    branchesIsEmpty = true;
                    currentNode = newNode;
                }
            }
            else
            {
                switch (msgs[i].mapping)
                {
                    case MsgData.ChoiceMapping.Defult:
                        curBranches[0] = newNode;
                        break;
                    case MsgData.ChoiceMapping.One:
                        curBranches[1] = newNode;
                        break;
                    case MsgData.ChoiceMapping.Two:
                        curBranches[2] = newNode;
                        break;
                    case MsgData.ChoiceMapping.Three:
                        curBranches[3] = newNode;
                        break;
                }
                branchesIsEmpty = false;
            }
        }
        return head;
    }
    IEnumerator ProceedSendingChat(MsgNode head)
    {
        MsgNode currentNode = head;
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (currentNode is null)
            {
                break;
            }
            Debug.Log("Try Send" + currentNode.content);
            messageSender.Send(currentNode.content);
            if (currentNode.hasChoice)
            {
                TypeEventSystem.Global.Send<OnChoiceActivated>(new(currentNode.choices));
                this.choice = -1;
                yield return new WaitUntil(() => { return choice != -1; });
                currentNode = currentNode.linkToBranches[choice] ?? currentNode.linkToBranches[0];
                continue;
            }
            else
            {
                if (currentNode.linkToBranches[0] is null)
                {
                    yield break;
                }
                currentNode = currentNode.linkToBranches[0];
            }
        }
    }
}
public class OnChoiceActivated
{
    public string[] choices = new string[3];
    public OnChoiceActivated(string[] choices)
    {
        int temp = choices.Length;
        for (int i = 0; i < temp; i++)
        {
            this.choices[i] = choices[i];
        }
        for (int i = temp; i < 3; i++)
        {
            this.choices[i] = "";
        }
    }
}
public class OnChose
{
    public readonly int index;
    public OnChose(int arg)
    {
        this.index = arg;
    }
}
