namespace Project;
using Godot;

[Tool]
public partial class SettingsBaseWidget : HSplitContainer {
	// Nodes
	[GetNode("Label")] private Label widgetLabelControl;
	[GetNode("CenterContainer")] protected Control WidgetControl;
	
	// Settings
	private string widgetText = "Placeholder";
	[Export] public string WidgetText {
		get => widgetText;
		set {
			widgetText = value;
			if (widgetLabelControl != null) widgetLabelControl.Text = value;
		}
	}
	
	public override void _Ready() {
		if (Engine.IsEditorHint()) widgetLabelControl.Text = widgetText;
	}
}
