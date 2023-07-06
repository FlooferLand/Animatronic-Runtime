namespace Project; using Settings;
using Godot;

public partial class SettingsMenu : Control {
	private TabContainer tabContainer;
	public override void _Ready() {
		tabContainer = GetNode<TabContainer>("Tabs");
		
		// Procedurally making the settings menu
		var sectionFields = EngineSettings.Get.GetType().GetFields();
		foreach (var sectionField in sectionFields) {
			// Creating a section
			var section = new VBoxContainer();
			section.Name = sectionField.Name;
			tabContainer.AddChild(section);
			
			// Getting the settings for that section
			var settingFields = sectionField.FieldType.GetFields();
			foreach (var settingField in settingFields) {
				var setting = new HSplitContainer();

				// Creating the setting name
				var name = new Label();
				name.Text = settingField.Name;
				setting.AddChild(name);
                
				// Creating the setting value
				GD.Print($"Attributes: {settingField.CustomAttributes}");
				// var value = new Label();
				// value.Text = "null";
				// setting.AddChild(value);
				
				// Adding the setting to the section
				section.AddChild(setting);
			}
		}
	}
}

