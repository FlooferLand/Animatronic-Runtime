namespace Project; using Settings;
using Newtonsoft.Json;
using System;
using Godot;

/// Class for storing things like the game settings
public static class EngineSettings {
	private const string Path = "user://settings.dict";
	public static EngineSettingsData Data = new();

	/// Loading all the settings
	public static void LoadAll() {
		try {
			if (FileAccess.FileExists(Path)) {
				FileAccess file = FileAccess.Open(Path, FileAccess.ModeFlags.Read);
				Data = JsonConvert.DeserializeObject<EngineSettingsData>(file.GetAsText());
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
		FileAccess file = FileAccess.Open(Path, FileAccess.ModeFlags.Write);
		if (file != null) {
			file.StoreString(JsonConvert.SerializeObject(Data, settings));
			file.Close();
		} else {
			Log.Error(FileAccess.GetOpenError());
		}
	}
}

