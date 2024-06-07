namespace Project.Settings;
using Godot;

// TODO: Add graphical and brightness stuff

public class Display {
	public Vector2I Resolution => DisplayServer.ScreenGetSize();

	[Export(PropertyHint.Range, hintString:"30,250,10")]
	public int FramerateCap {
		get => framerateCap;
		set {
			framerateCap = value;
			Engine.SetMaxFps(framerateCap);
		}
	}
	private int framerateCap = 120;
	
	public DisplayServer.VSyncMode VSync => DisplayServer.WindowGetVsyncMode();
}

public class Graphics {}

public class Performance {
	// TODO: Add several performance modes
	public bool LowPerformanceMode {
		get => lowPerfMode || Utils.IsHomebrew();
		set => lowPerfMode = value;
	}
	private bool lowPerfMode = false;
}

/// <para>This stores all the editor settings. </para>
/// <para>This class procedurally generates UI using <see cref="SettingsMenuParser.Parser.ParseFieldAttributes"/></para>
public class EngineSettingsData {
	public Display Display = new();
	public Graphics Graphics = new();
	public Performance Performance = new();
}
