namespace Project;
using Godot;

public partial class SettingsTwoNumWidget : SettingsBaseWidget<Vector2> {
	// Nodes
	[GetNode("{WidgetControl}/Container/NumOne")] private SpinBox numOne;
	[GetNode("{WidgetControl}/Container/NumTwo")] private SpinBox numTwo;
	
	// Signals
	[Signal] public delegate void ValueChangedEventHandler(Vector2 value);
	
	#region internal
	protected override void UpdateWidgets() {
		if (numOne != null && numTwo != null) {
			numOne.Value = Value.X;
			numTwo.Value = Value.Y;
		}
	}
	#endregion
	
	public override void _Ready() {
		base._Ready();
		
		// Connecting signals
		if (Engine.IsEditorHint()) return;
		numOne.ValueChanged += newValue => {
			Value = Value.WithX(newValue);
			EmitSignal(nameof(ValueChanged), Value);
		};
		numTwo.ValueChanged += newValue => {
			Value = Value.WithY(newValue);
			EmitSignal(nameof(ValueChanged), Value);
		};
	}
}
