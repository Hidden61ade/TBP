using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoadedEvent
{
    public readonly string name = "[NOTMENTIONED]";
    public OnSceneLoadedEvent(){}
    public OnSceneLoadedEvent(string name){
        this.name = name;
    }
}
public class OnDayEventTriggered
{
    public OnDayEventTriggered(EventStatus Event)
    {
        this.Event = Event;
    }
    public EventStatus Event;
}
public class NextPeriodEvent{

}
public class RequireSaveGame{

}