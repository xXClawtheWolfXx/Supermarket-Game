using Godot;
using System;

/// Manages all the HUD UI in the game
public partial class UIManager : Control {

    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [Export] RichTextLabel moneyLabel;
    [Export] RichTextLabel timeLabel;

    public override void _Ready() {
        instance = this;
    }

    /// Prints the specified money to a label on the HUD
    public void UpdateMoneyLabel(int money) {
        moneyLabel.Text = "Money: " + money;
    }

    public void UpdateTimeLabel(string time) {
        timeLabel.Text = time;

    }

    /// Opens the store when the button is pressed
    public void OnOpenStoreButtonPressed() {
        GameManager.Instance.OpenStore();
    }
}

