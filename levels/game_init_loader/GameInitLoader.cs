namespace Project;
using Godot;

/// Gives the game enough of a break to load all of its assets (engine settings, etc)
public partial class GameInitLoader : ColorRect {
	private Timer loadTimer;
	
	// Exports
	[Export] public PackedScene MainScene;
	
	public override void _Ready() {
		loadTimer = GetNode<Timer>("Timer");
		loadTimer.Timeout += _LoadGame;
	}

	private void _LoadGame() {
		if (GetTree().ChangeSceneToPacked(MainScene) != Error.Ok) {
			OS.Alert("Could not load the main scene!");
		}
	}
}
