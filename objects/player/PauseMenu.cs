namespace Project;
using Godot;

public partial class PauseMenu : ColorRect {
	// Nodes
	private SettingsMenu settingsMenu;
	private Button continueButton;
	private Button settingsButton;
	private Button quitButton;

	// Private methods
	public override void _Ready() {
		settingsMenu   = GetNode<SettingsMenu>("SettingsMenu");
		continueButton = GetNode<Button>("Buttons/Continue");
		settingsButton = GetNode<Button>("Buttons/Settings");
		quitButton     = GetNode<Button>("Buttons/Quit");
        
		// Callbacks
		continueButton.Pressed += Unpause;
		settingsButton.Pressed += () => {
			settingsMenu.Visible = !settingsMenu.Visible;
		};
		quitButton.Pressed += () => {
			// TODO: Implement "quit to main menu" button
			OS.Alert("Not implemented!");
		};
	}
	
	// Public methods
	public void Toggle() {
		if (Visible)
			Unpause();
		else
			Pause();
	}

	public void Pause() {
		Visible = true;
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		Engine.TimeScale = 0f;
	}

	public void Unpause() {
		Visible = false;
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Engine.TimeScale = 1f;
	}
}

