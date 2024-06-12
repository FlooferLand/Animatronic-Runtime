// ReSharper disable UnusedMember.Global, MemberCanBeMadeStatic.Global, FieldCanBeMadeReadOnly.Global
#pragma warning disable CA1822
namespace Project.Settings;
using Godot;

public class Display {
	// TODO: Add brightness settings
	
	// Resolution; Defaults to the screen resolution
	public Vector2I Resolution = DisplayServer.ScreenGetSize();

	// Framerate limiter, for battery-saving reasons
	public int FramerateCap = 240;

	/// Nuh uhh
	public DisplayServer.VSyncMode VSync = DisplayServer.WindowGetVsyncMode();
}

public class Graphics {
	// TODO: Add graphical settings
	
	// LOD distance tweak
	public float LodThreshold = Utils.IsHomebrew() ? 0.3f : 0.8f;
}

public class Performance {
	// Backing fields
	private bool lowPerfMode = false;
	private bool updateTitle = true;

	/// Property for LowPerformanceMode
	/// TODO: Add more variety; make it so the user can enable harsh performance optimizations only for some features
	public bool LowPerformanceMode {
		get => lowPerfMode || Utils.IsHomebrew();
		set => lowPerfMode = value;
	}

	/// Property for UpdateTitle; Has a default window title
	/// TODO: Move to the new settings system (SettingsMenu)
	public bool UpdateTitle {
		get => updateTitle;
		set {
			updateTitle = value;
			var window = (Engine.GetMainLoop() as SceneTree)?.Root;
			if (!updateTitle && window != null) {
				window.SetTitle("Animatronic Runtime");
			}
		}
	}
}

/// <summary>
/// This class stores all the engine settings.
/// </summary>
public class EngineSettingsData {
	public Display Display { get; } = new();
	public Graphics Graphics { get; } = new();
	public Performance Performance { get; } = new();
}