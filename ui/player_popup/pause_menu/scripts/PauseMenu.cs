namespace Project;
using Godot;

public partial class PauseMenu : PlayerPopupUi {
	[GetNode("SettingsMenu")] private SettingsMenu settingsMenu;
	[GetNode("Buttons/Continue")] private PauseMenuButton continueButton;
	[GetNode("Buttons/Settings")] private PauseMenuButton settingsButton;
	[GetNode("Buttons/ExitToMenu")] private PauseMenuButton exitToMenuButton;
	
	public new void PopUp() {
		base.PopUp();
		GetTree().Paused = true;
	}
	public new void PopDown() {
		base.PopDown();
		GetTree().Paused = false;
		settingsMenu.Visible = false;
	}
	
	#region Signaled
	public void _On_Continue() {
		PopDown();
	}
	public void _On_Settings() {
		settingsMenu.Visible = !settingsMenu.Visible;
	}
	public void _On_ExitToMenu() {
		// TODO: Implement "ExitToMenu" on the pause menu
		OS.Alert("Not implemented!");
	}
	#endregion

	public override void _Ready() {
		base._Ready();
		continueButton.Pressed += _On_Continue;
		settingsButton.Pressed += _On_Settings;
		exitToMenuButton.Pressed += _On_ExitToMenu;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (Input.IsActionJustPressed("escape")) Toggle();
	}
}
