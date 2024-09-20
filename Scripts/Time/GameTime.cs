using Godot;
using System;

public partial class GameTime : Node {

    //singleton
    private static GameTime instance;
    public static GameTime Instance { get { return instance; } }

    //[Export] Clock gameTime;
    [Export] Timer timer;
    [Export] float timeUntilNextTime = 30;
    //[Export] int timeIncrease = 30;
    [Export] int maxTimeSlots = 48; // every 30 mins
    [Export] int time;

    [Signal] public delegate void OnTimeIncreaseEventHandler(int time);

    public int GetMaxTimeSlots { get => maxTimeSlots; }

    public override void _Ready() {
        instance = this;
        timer.WaitTime = timeUntilNextTime;
    }

    public void OnTimerTimeout() {
        // gameTime.IncreaseTime(timeIncrease);
        time++;
        if (time >= maxTimeSlots) time = 0;
        UIManager.Instance.UpdateTimeLabel(timeToString());
        EmitSignal(SignalName.OnTimeIncrease, time);
    }

    private string timeToString() {
        string timeStr = "";
        timeStr += (time / 2).ToString().PadLeft(2, '0') + ":";

        if (time % 2 == 1) timeStr += "30";
        else timeStr += "00";
        return timeStr;
    }
}
