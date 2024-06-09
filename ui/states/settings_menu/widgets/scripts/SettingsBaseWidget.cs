namespace Project;
using Godot;

[Tool]
public partial class SettingsBaseWidget : HSplitContainer {
	// Nodes
	[GetNode("Label")] private Label widgetLabelControl;
	[GetNode("CenterContainer")] protected Control WidgetControl;
	
	// Settings
	private string widgetName = "Placeholder";
	[Export] public string WidgetName {
		get => widgetName;
		set {
			// if (!Engine.IsEditorHint()) return;  // This line stops the widget name from showing.. for some reason..
			widgetName = value;
			if (widgetLabelControl != null)
				widgetLabelControl.Text = value;
		}
	}
	
	public override void _Ready() {
		widgetLabelControl.Text = widgetName;
	}
}
