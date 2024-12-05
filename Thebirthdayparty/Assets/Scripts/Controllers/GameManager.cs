using System.Collections;
using System.Collections.Generic;
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