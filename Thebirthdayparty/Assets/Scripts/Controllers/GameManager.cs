using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Dictionary<string, bool> playerActions = new Dictionary<string, bool>();
    private Dictionary<string, string> gameVariables = new Dictionary<string, string>();
    private GameState currentGameState = GameState.None;

    public enum GameState
    {
        None,
        Introduction,
        Playing,
        Paused,
        Ending
    }
    public static Dictionary<GameTime, string> TimeName = new(){
        {GameTime.Morning,"morning"},
        {GameTime.Afternoon,"afternoon"},
        {GameTime.Evening,"evening"}
        };
    public static GameTime TimeFromString(string arg)
    {
        return arg switch
        {
            "morning" => GameTime.Morning,
            "afternoon" => GameTime.Afternoon,
            "evening" => GameTime.Evening,
            _ => GameTime.Invalid,
        };
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GameSaveManager.Instance.currentSave.firstPlayed)
        {
            TypeEventSystem.Global.Register<OnSceneLoadedEvent>(e =>
            {
                if (e.name == "SampleScene")
                {
                    Debug.Log("播放初始动画");
                    var temp = GameObject.FindWithTag("Player");
                    temp.transform.position = new Vector3(22, 2.8f, 1.5f);
                    temp.transform.eulerAngles = new Vector3(-85, -90, 0);
                    temp.AddComponent<StartAnimation>();
                    Unreg.Invoke();
                }
            });
        }
    }
    private static IUnRegister startAnimationRegHandle;
    Action Unreg = () =>
    {
        startAnimationRegHandle?.UnRegister();
    };
    public void RecordPlayerAction(string actionName)
    {
        playerActions[actionName] = true;
    }

    public bool HasPlayerPerformedAction(string actionName)
    {
        return playerActions.ContainsKey(actionName) && playerActions[actionName];
    }

    public void SetVariable(string name, string value)
    {
        gameVariables[name] = value;
    }

    public bool CheckVariable(string name, string expectedValue)
    {
        return gameVariables.ContainsKey(name) && gameVariables[name] == expectedValue;
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
    }

    public bool CheckCustomCondition(string conditionName, string expectedValue)
    {
        // 实现自定义条件检查逻辑
        // 这里可以根据具体需求扩展
        return false;
    }
}
public enum GameTime
{
    Morning,
    Afternoon,
    Evening,
    Invalid
}