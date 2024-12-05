using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoadedEvent
{

}
public class OnDayEventTriggered
{
    public OnDayEventTriggered(EventStatus Event)
    {
        this.Event = Event;
    }
    public EventStatus Event;
}