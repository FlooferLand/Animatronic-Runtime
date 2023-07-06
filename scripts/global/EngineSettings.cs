namespace Project; using Settings;
using Newtonsoft.Json;
using System;
using Godot;

/// Class for storing things like the game settings
public static class EngineSettings {
	private const string PATH = "user://settings.dict";
	public static EngineSettingsData Get = new();

	/// Loading all the settings
	public static void LoadAll() {
		try {
			if (FileAccess.FileExists(PATH)) {
				FileAccess file = FileAccess.Open(PATH, FileAccess.ModeFlags.Read);
				Get = JsonConvert.DeserializeObject<EngineSettingsData>(file.GetAsText());
				file.Close();
			} else {
				// Save the default settings
				SaveAll();
			}
		}
		catch (Exception err) {
			OS.Alert(
				$"Failed to load engine settings. Reason:\n\"{err.Message}\"",
				"Error!"
			);
		}
	}

	/// Save all the settings
	public static void SaveAll() {
		var settings = new JsonSerializerSettings {
			Formatting = Formatting.Indented
		};
		
		// Writing data to the settings file
		FileAccess file = FileAccess.Open(PATH, FileAccess.ModeFlags.Write);
		file.StoreString(JsonConvert.SerializeObject(Get, settings));
		file.Close();
	}
}

