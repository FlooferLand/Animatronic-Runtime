namespace Project;
using Godot;

[Tool]
public partial class SettingsTwoNumWidget : SettingsBaseWidget {
	// Nodes
	[GetNode("{WidgetControl}/Container/NumOne")] private SpinBox numOne;
	[GetNode("{WidgetControl}/Container/NumTwo")] private SpinBox numTwo;
	
	// Signals
	[Signal] public delegate void ValueChangedEventHandler(Vector2 value);
	
	// Variables
	private Vector2 value;
	public Vector2 Value {
		get => value;
		set {
			this.value = value;
			UpdateWidgets();
		}
	}
	
	public override void _Ready() {
		base._Ready();
		UpdateWidgets();
		
		// Connecting signals
		if (Engine.IsEditorHint()) return;
		numOne.ValueChanged += (newValue) => {
			Value = Value.WithX(newValue);
			UpdateWidgets();
			EmitSignal(nameof(ValueChanged), Value);
		};
		numTwo.ValueChanged += (newValue) => {
			Value = Value.WithY(newValue);
			UpdateWidgets();
			EmitSignal(nameof(ValueChanged), Value);
		};
	}

	private void UpdateWidgets() {
		if (numOne != null && numTwo != null) {
			numOne.Value = Value.X;
			numTwo.Value = Value.Y;
		}
	}
}
