using Godot;
using System;

public partial class UIManager : Control {

	private static UIManager instance;
	public static UIManager Instance { get { return instance; } }


	[Export] RichTextLabel moneyLabel;
	[Export] RichTextLabel timeLabel;

	public override void _Ready() {
		instance = this;
	}

	public void UpdateMoneyLabel(int money) {
		moneyLabel.Text = "Money: " + money;
	}

	public void UpdateTimeLabel() {

	}
}

