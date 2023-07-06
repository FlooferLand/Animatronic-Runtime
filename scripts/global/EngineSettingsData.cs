namespace Project.Settings;
using Godot;

// TODO: Add graphical and brightness stuff

public class Display {
	public Vector2I Resolution = DisplayServer.ScreenGetSize();
	
	[Export(PropertyHint.Range, hintString:"0,144,1")]
	public float RefreshRate = DisplayServer.ScreenGetRefreshRate();
	
	public bool VSync = true;
}

public class Graphics {}

public class EngineSettingsData {
	public Display Display = new();
	public Graphics Graphics = new();
}
