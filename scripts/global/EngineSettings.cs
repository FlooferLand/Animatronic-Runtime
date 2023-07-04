namespace Project;
using Godot;

// Class for storing things like the game settings

public static class EngineSettings {
	// Loading all the settings
	public static void LoadAll() {}
	
	// Tells the engine to expect a setting of `key` and `value`
	// Letting other functions easily retrieve stuff after its called
	public static void RequestProperty(string key, Variant value) {}
}

