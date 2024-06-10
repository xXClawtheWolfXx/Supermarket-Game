using Godot;
using System;

/// <summary>
/// Manages all the HUD UI in the game
/// </summary>
public partial class UIManager : Control {

    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [Export] RichTextLabel moneyLabel;
    [Export] RichTextLabel timeLabel;

    public override void _Ready() {
        instance = this;
    }

    /// <summary>
    /// Prints the specified money to a label on the HUD
    /// </summary>
    /// <param name="money"> the money to be printed</param>
    public void UpdateMoneyLabel(int money) {
        moneyLabel.Text = "Money: " + money;
    }

    public void UpdateTimeLabel(Clock gameTime) {
        timeLabel.Text = "Time: " + gameTime.ToString();

    }

    /// <summary>
    /// Opens the store when the button is pressed
    /// </summary>
    public void OnOpenStoreButtonPressed() {
        GameManager.Instance.OpenStore();
    }
}

