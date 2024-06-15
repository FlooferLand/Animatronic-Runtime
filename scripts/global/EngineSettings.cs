using System.Xml;
using Godot.Collections;
using YamlDotNet.Serialization;
namespace Project;
using YamlDotNet.Serialization.NamingConventions;
using Settings;
using System;
using Godot;

/// Class for storing things like the game settings
public static class EngineSettings {
	private static Dictionary<string, Variant> data = new();
	private static ISerializer serializer = new SerializerBuilder()
		.WithNamingConvention(CamelCaseNamingConvention.Instance)
		.Build();
	private static IDeserializer deserializer = new DeserializerBuilder()
		.WithNamingConvention(CamelCaseNamingConvention.Instance)
		.Build();

	/// Loading all the settings
	public static void LoadAll() {
		try {
			if (FileAccess.FileExists(Paths.SettingsPath)) {
				FileAccess file = FileAccess.Open(Paths.SettingsPath, FileAccess.ModeFlags.Read);
				data = deserializer.Deserialize<Dictionary<string, Variant>>(file.GetAsText());
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
		// Writing data to the settings file
		FileAccess file = FileAccess.Open(Paths.SettingsPath, FileAccess.ModeFlags.Write);
		if (file != null) {
			file.StoreString(serializer.Serialize(data));
			file.Close();
		} else {
			Log.Error(FileAccess.GetOpenError());
		}
	}
}

