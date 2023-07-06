namespace Project;
using Godot;

/// Handles triggering certain events based on the engine state.
/// -- Do not use for global storage
public partial class AppStateManager : Node {
	public override void _EnterTree() {
		EngineSettings.LoadAll();
	}

	public override void _Notification(int what) {
		long id = what;
		switch (id) {
			case NotificationCrash:
			case NotificationWMCloseRequest:
				EngineSettings.SaveAll();
				break;
		}
	}
}

