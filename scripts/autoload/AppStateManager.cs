namespace Project;
using Godot;

/// Handles triggering certain events based on the engine state.
/// -- Do not use this for global storage
public partial class AppStateManager : Node {
	public override void _EnterTree() {
		EngineSettings.LoadAll();
		
		// DEBUG
		// OS.Crash("ohh ye this e fuckin erroh innit thislcrashtegame innit cont believ it ahbsolu'ely splendid");
	}

	public override void _Notification(int what) {
		long id = what;
		switch (id) {
			// TODO: Modify engine code once 4.3 releases to allow for graceful crash handling
			case NotificationCrash:
				// .. stop the game from closing here
				Log.FatalError("ENGINE CRASHED FOR AN UNKNOWN REASON");
				EngineSettings.SaveAll();
				break;
			
			case NotificationWMCloseRequest:
				EngineSettings.SaveAll();
				break;
		}
	}

	public override void _Process(double delta) {
		if (EngineSettings.Data.Performance.UpdateTitle) {
			GetTree().Root.SetTitle($"Animatronic Runtime - {Mathf.RoundToInt(Engine.GetFramesPerSecond())} FPS");
		}
	}
}

