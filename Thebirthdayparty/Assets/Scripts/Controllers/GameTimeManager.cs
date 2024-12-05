using QFramework;

public class GameTimeManager : MonoSingleton<GameTimeManager>
{
    public int GetCurrentCycle()
    {
        return GameSaveManager.Instance.currentSave.currentCycle;
    }
    public int GetCurrentDay()
    {
        return GameSaveManager.Instance.currentSave.currentDay;
    }
    public GameTime GetCurrentPeriod()
    {
        return GameManager.TimeFromString(GameSaveManager.Instance.currentSave.currentPeriod);
    }
    public GameTime GetNext(out bool isNextDay)
    {
        var currentTime = GetCurrentPeriod();
        isNextDay = currentTime == GameTime.Evening;
        return currentTime switch
        {
            GameTime.Morning => GameTime.Afternoon,
            GameTime.Afternoon => GameTime.Evening,
            GameTime.Evening => GameTime.Morning,
            _ => GameTime.Invalid
        };
    }
    [InspectorButton("下一时段")]
    public void GoToNextPeriod()
    {
        var temp = GetNext(out bool isNextDay);
        if (isNextDay)
        {
            GameSaveManager.Instance.AdvanceDay();
        }
        GameSaveManager.Instance.SetPeriod(GameManager.TimeName[temp]);
        TypeEventSystem.Global.Send(new OnDayEventTriggered(GetCurrentEvent()));
    }
    public DayEvents GetDayEvents()
    {
        return GameSaveManager.Instance.currentSave.dailyEvents[GetCurrentDay()];
    }
    public EventStatus GetCurrentEvent()
    {
        return GetDayEvents().periods[GameManager.TimeName[GetCurrentPeriod()]];
    }
    private void Start()
    {
        
    }
    // IEnumerator Tester()
    // {
    //     int i = 0;
    //     while (i < 10)
    //     {
    //         Debug.Log("Now, Day: " + GetCurrentDay()
    //         + "\nPeriod: " + GameManager.TimeName[GetCurrentPeriod()]
    //         + "\nEvent: " + GetCurrentEvent().eventName
    //         );
    //         GoToNextPeriod();
    //         i++;
    //         yield return new WaitForSeconds(1);
    //     }
    // }
}