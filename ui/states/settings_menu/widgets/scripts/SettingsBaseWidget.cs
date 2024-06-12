namespace Project;
using Godot;

public abstract partial class SettingsBaseWidget<TValue> : HSplitContainer {
	// Nodes
	[GetNode("Label")] private Label widgetLabelControl;
	[GetNode("CenterContainer")] protected Control WidgetControl;
	
	//
	protected TValue InternalValue;
	public TValue Value;
	
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

	public virtual void set_Value(TValue value) {
		InternalValue = value;
	}
	public virtual TValue get_Value() {
		return InternalValue;
	}
	
	public virtual void LoadFrom(TValue value) {
		Value = value;
	}

	protected abstract void UpdateWidgets();
}
