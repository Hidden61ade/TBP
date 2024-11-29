using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrationCondition
{
    public enum ConditionType
    {
        PlayerAction,
        GameState,
        TimeElapsed,
        VariableCheck,
        Custom
    }

    public ConditionType conditionType;
    public string conditionName;
    public string expectedValue;
    public bool invertCondition;

    public bool IsMet()
    {
        bool result = false;
        
        switch (conditionType)
        {
            case ConditionType.PlayerAction:
                result = GameManager.Instance.HasPlayerPerformedAction(conditionName);
                break;
            case ConditionType.GameState:
                result = GameManager.Instance.GetGameState().ToString() == expectedValue;
                break;
            case ConditionType.TimeElapsed:
                result = float.TryParse(expectedValue, out float expectedTime) && 
                        Time.time >= expectedTime;
                break;
            case ConditionType.VariableCheck:
                result = GameManager.Instance.CheckVariable(conditionName, expectedValue);
                break;
            case ConditionType.Custom:
                result = GameManager.Instance.CheckCustomCondition(conditionName, expectedValue);
                break;
        }

        return invertCondition ? !result : result;
    }
}