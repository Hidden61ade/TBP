using System;
using System.Collections.Generic;

[Serializable]
public class DayEvents
{
    public Dictionary<string, EventStatus> periods;

    public DayEvents()
    {
        periods = new Dictionary<string, EventStatus>();
    }
}

[Serializable]
public class EventStatus
{
    public string eventName;
    public bool completed;

    public EventStatus(string name, bool status = false)
    {
        eventName = name;
        completed = status;
    }
}