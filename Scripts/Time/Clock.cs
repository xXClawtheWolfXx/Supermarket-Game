using Godot;
using System;

[GlobalClass]
public partial class Clock : Node {
    [Export] int hour;
    [Export] int minute;

    public int GetHour { get { return hour; } }
    public int GetMinute { get { return minute; } }

    public Clock() {
        hour = minute = 0;
    }

    public Clock(int hour, int minute) {
        this.hour = hour;
        this.minute = minute;
    }

    public Clock(Clock clock) {
        hour = clock.hour;
        minute = clock.minute;
    }

    public void IncreaseTime(int min) {
        minute += min;
        hour += minute / 60;
        minute = minute % 60;
        if (hour >= 24)
            hour -= 24;
    }

    public override string ToString() {
        return hour.ToString().PadLeft(2, '0') + ":" + minute.ToString().PadLeft(2, '0');

    }


}
