using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class MessageSend : MonoBehaviour
{
    public string currentCharacter = "Adam";
    public Transform contentParent;
    public GameObject MsgPrefab;
    public string contToSend;
    [InspectorButton("Send")]
    void Send()
    {
        var temp = Instantiate(MsgPrefab, contentParent);
        temp.GetComponent<ComputerMsg.Message>().SetText(contToSend);
    }
    public float Send(string str)
    {
        Debug.Log("Sender Get: " + str);
        //Proceed Control
        #region Control Functions
        if (str.ToCharArray()[0] == '#')
        {
            switch (str)
            {
                case "#SYS:PASS":
                    break;
                case "#SYS:FP":
                    TypeEventSystem.Global.Send<OnChose>(new(0));
                    break;
                case "#SYS:ETD":
                    Debug.LogError("Error Text Defult");
                    break;
                case "#SYS:SCE1":
                    Debug.Log("here");
                    StartCoroutine(SCE1());
                    break;
                case "#NP":
                    GameTimeManager.Instance.GoToNextPeriod();
                    break;
                case "#LQ":
                    Debug.Log("Lock quit");
                    TypeEventSystem.Global.Send<LockQuitButton>();
                    break;
                case "#RQ":
                    Debug.Log("Unlock quit");
                    TypeEventSystem.Global.Send<UnlockQuitButton>();
                    break;
                default:
                    try
                    {
                        string command = str.Substring(1, 2); // 获取#后面两位字符
                        string argument = str.Substring(str.IndexOf('(') + 1, str.IndexOf(')') - str.IndexOf('(') - 1);
                        // 调用相应的函数
                        switch (command)
                        {
                            case "ST":
                                ParseString.SetVariableTrue(argument);
                                break;
                            case "SF":
                                ParseString.SetVariableFalse(argument);
                                break;
                            case "AF":
                                GameSaveManager.Instance.UpdateAffinity(currentCharacter, int.Parse(argument));
                                break;

                            default:
                                Debug.LogWarning("Unknown command");
                                break;
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        break;
                    }
            }
            return 0.1f;
        }
        #endregion
        var temp = Instantiate(MsgPrefab, contentParent);
        temp.GetComponent<ComputerMsg.Message>().SetText(str);
        return 1.5f;
    }
    IEnumerator SCE1()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1.5f);
        GetComponent<ExternalConsoleLauncher>().LaunchExternalConsole();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
