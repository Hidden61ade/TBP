using UnityEngine;

[System.Serializable]
public class NarrationEvent
{
    public string eventID;
    [TextArea(3, 10)]
    public string subtitleText;
    public AudioClip audioClip;
    public float delayAfterNarration = 1f;
    public NarrationCondition[] conditions;
    [TextArea(2, 5)]
    public string editorNotes;

    public bool ConditionsMet()
    {
        if (conditions == null || conditions.Length == 0)
            return true;

        foreach (var condition in conditions)
        {
            if (!condition.IsMet())
                return false;
        }
        return true;
    }
}