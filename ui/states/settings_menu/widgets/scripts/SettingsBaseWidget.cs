namespace Project;
using Godot;

public abstract partial class SettingsBaseWidget<TValue> : HSplitContainer {
	// Nodes
	[GetNode("Label")] private Label widgetLabelControl;
	[GetNode("CenterContainer")] protected Control WidgetControl;
	
	// Variables
	protected TValue InternalValue;
	protected virtual TValue Value {
		get => InternalValue;
		set {
			InternalValue = value;
			UpdateWidgets();
		}
	}
	
	// Settings
	private string widgetName = "Placeholder";
	[Export] public string WidgetName {
		get => widgetName;
		set {
			widgetName = value;
			if (widgetLabelControl != null)
				widgetLabelControl.Text = value;
		}
	}
	
	public override void _Ready() {
		widgetLabelControl.Text = widgetName;
	}
	
	public virtual void LoadFrom(TValue value) {
		Value = value;
	}

	protected abstract void UpdateWidgets();
}
