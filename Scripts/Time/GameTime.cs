using Godot;
using System;

public partial class GameTime : Node {

    //singleton
    private static GameTime instance;
    public static GameTime Instance { get { return instance; } }

    [Export] Clock gameTime;
    [Export] Timer timer;
    [Export] float timeUntilNextTime = 30;
    [Export] int timeIncrease = 30;

    [Signal] public delegate void OnTimeIncreaseEventHandler(Clock gameTime);

    public override void _Ready() {
        instance = this;
        timer.WaitTime = timeUntilNextTime;
    }

    public void OnTimerTimeout() {
        gameTime.IncreaseTime(timeIncrease);
        UIManager.Instance.UpdateTimeLabel(gameTime);
        EmitSignal(SignalName.OnTimeIncrease, gameTime);
    }


}
