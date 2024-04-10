namespace Project.Settings;
using Godot;

// TODO: Add graphical and brightness stuff

public class Display {
	public Vector2I Resolution = DisplayServer.ScreenGetSize();
	
	[Export(PropertyHint.Range, hintString:"20,999,1")]
	public float RefreshRate = DisplayServer.ScreenGetRefreshRate();
	
	public bool VSync = true;
}

public class Graphics {}

/// <para>This stores all of the editor settings. </para>
/// <para>This class procedurally generates UI using <see cref="SettingsMenuParser.Parser.ParseFieldAttributes"/></para>
public class EngineSettingsData {
	public Display Display = new();
	public Graphics Graphics = new();
}
