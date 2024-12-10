using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public bool firstPlayed;
    public int currentCycle;
    public int currentDay;
    public string currentPeriod;
    public Dictionary<string, float> affinity;
    public HashSet<string> interactedItems;
    public Dictionary<int, DayEvents> dailyEvents;
    public Collections collections;

    public GameSaveData()
    {
        firstPlayed = true;
        currentCycle = 1;
        currentDay = 1;
        currentPeriod = "morning";
        affinity = new Dictionary<string, float>
        {
            { "Adam", 0f },
            { "George", 0f }
        };
        interactedItems = new HashSet<string>();
        dailyEvents = DailyEventsInitializer.InitializeDailyEvents();
        collections = new Collections();
    }
    public void ResetObjectsInfo()
    {
        interactedItems = new();
        collections = new();
        firstPlayed = true;
    }
}