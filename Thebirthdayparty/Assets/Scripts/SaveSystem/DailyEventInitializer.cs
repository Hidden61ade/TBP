using System.Collections.Generic;

public static class DailyEventsInitializer
{
    public static Dictionary<int, DayEvents> InitializeDailyEvents()
{
    var events = new Dictionary<int, DayEvents>();
    
    // Day 1
    var day1 = new DayEvents();
    day1.periods["morning"] = new EventStatus("friend_request");      // 事件I：添加好友，派对计划初步讨论
    day1.periods["afternoon"] = new EventStatus("free_time");
    day1.periods["evening"] = new EventStatus("free_time");
    events[1] = day1;

    // Day 2
    var day2 = new DayEvents();
    day2.periods["morning"] = new EventStatus("venue_discussion");    // 事件II：场地安排讨论
    day2.periods["afternoon"] = new EventStatus("free_time");
    day2.periods["evening"] = new EventStatus("theme_discussion");    // 事件III：主题安排讨论
    events[2] = day2;

    // Day 3
    var day3 = new DayEvents();
    day3.periods["morning"] = new EventStatus("free_time");
    day3.periods["afternoon"] = new EventStatus("food_discussion");   // 事件IV：讨论准备食物
    day3.periods["evening"] = new EventStatus("free_time");
    events[3] = day3;

    // Day 4
    var day4 = new DayEvents();
    day4.periods["morning"] = new EventStatus("free_time");
    day4.periods["afternoon"] = new EventStatus("program_discussion"); // 事件V：讨论看什么节目
    day4.periods["evening"] = new EventStatus("george_chat");         // 事件VI：George的聊天邀请
    events[4] = day4;

    // Day 5
    var day5 = new DayEvents();
    day5.periods["morning"] = new EventStatus("free_time");
    day5.periods["afternoon"] = new EventStatus("free_time");
    day5.periods["evening"] = new EventStatus("terminal_delivery");   // 事件VII：收到tErminaL终端
    events[5] = day5;

    // Day 6
    var day6 = new DayEvents();
    day6.periods["morning"] = new EventStatus("free_time");
    day6.periods["afternoon"] = new EventStatus("adam_apology");      // 与Adam的道歉对话选项
    day6.periods["evening"] = new EventStatus("free_time");
    events[6] = day6;

    // Day 7 (派对当天)
    var day7 = new DayEvents();
    day7.periods["morning"] = new EventStatus("party_day");          // 派对日
    events[7] = day7;

    return events;
}
}