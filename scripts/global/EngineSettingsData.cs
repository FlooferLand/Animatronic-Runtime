// ReSharper disable UnusedMember.Global, MemberCanBeMadeStatic.Global, FieldCanBeMadeReadOnly.Global
#pragma warning disable CA1822
namespace Project.Settings;
using Godot;

// TODO: Add graphical and brightness stuff

public class Display {
	// Backing fields
	private int framerateCap = 120;
	
	// Resolution; Defaults to the screen resolution
	public Vector2I Resolution => DisplayServer.ScreenGetSize();

	// Framerate limiter, for battery-saving reasons
	[Export(PropertyHint.Range, hintString: "30,250,10")]
	public int FramerateCap {
		get => framerateCap;
		set {
			framerateCap = value;
			Engine.SetMaxFps(framerateCap);
		}
	}

	/// Nuh uhh
	public DisplayServer.VSyncMode VSync => DisplayServer.WindowGetVsyncMode();
}

public class Graphics {
	// TODO: Add graphical settings
}

public class Performance {
	// Backing fields
	private bool lowPerfMode = false;
	private bool updateTitle = true;

	/// Property for LowPerformanceMode
	public bool LowPerformanceMode {
		get => lowPerfMode || Utils.IsHomebrew();
		set => lowPerfMode = value;
	}

	/// Property for UpdateTitle; Has a default window title
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