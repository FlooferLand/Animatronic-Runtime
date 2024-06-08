namespace Project;
using Godot;

[Tool]
public partial class SettingsBaseWidget : HSplitContainer {
	// Nodes
	[GetNode("Label")] private Label widgetLabelControl;
	
	// Settings
	private string widgetText;
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
