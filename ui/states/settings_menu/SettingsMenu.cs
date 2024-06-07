using Project.SettingsMenuParser;

namespace Project;
using Godot;

public partial class SettingsMenu : Control {
	private TabContainer tabContainer;
	public override void _Ready() {
		tabContainer = GetNode<TabContainer>("Tabs");
		
		// Procedurally making the settings menu
		var sectionFields = EngineSettings.Data.GetType().GetFields();
		foreach (var sectionField in sectionFields) {
			#region Creating a section
			// Creating the section
			var section = new VBoxContainer();
			
			// Adding a margin container for the section
			var margin = new MarginContainer();
			foreach (string direction in new[] { "top", "left", "bottom", "right" }) {
				margin.AddThemeConstantOverride($"margin_{direction}", 25);
			}
			margin.AddChild(section);
			
			// Adding a scroll container for the section
			var scroll = new ScrollContainer();
			scroll.AddChild(margin);
			
			// Adding the section
			scroll.Name = sectionField.Name;
			tabContainer.AddChild(scroll);
			#endregion
			
			// Getting the settings for that section
			var settingFields = sectionField.FieldType.GetFields();
			foreach (var settingField in settingFields) {
				var setting = new HSplitContainer();

				// Creating the setting name UI
				var name = new Label();
				name.Text = settingField.Name;
				setting.AddChild(name);
				
				// Creating the setting value UI
				var uiType = Parser.ParseFieldAttributes(sectionField, settingField);
				if (uiType != null) {
					setting.AddChild(uiType.GetNode());
				}
				
				// Adding the setting to the section
				section.AddChild(setting);
			}
		}
	}
}
