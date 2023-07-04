namespace Project;
using Godot;

public partial class GameInitLoader : ColorRect {
	[Export] public PackedScene MainScene;
	
	public override void _Ready() {
		// Initializing the config and stuff
		EngineSettings.RequestProperty("test_value", Variant.From(123));
		EngineSettings.LoadAll();
		
		// Loading the game
		if (GetTree().ChangeSceneToPacked(MainScene) != Error.Ok) {
			OS.Alert("Could not load the main scene!");
		}
	}
}
