using System.Collections.Generic;

namespace Project;
using Godot;

public partial class SettingsNumberWidget : SettingsBaseWidget<float> {
	// Nodes
	[GetNode("{WidgetControl}/Container/Tabbed")] private TabContainer tabber;
	[GetNode("{WidgetControl}/Container/Slider")] private Slider slider;
	[GetNode("{tabber}/SpinBox")] private SpinBox spinBox;
	[GetNode("{tabber}/OverrideLabel")] private Label overrideLabel;
	
	// Signals
	[Signal] public delegate void ValueChangedEventHandler(float value, bool isMin, bool isMax);
	
	// Settings
	public string StringValueOverride = null;
	[Export] public float Min = 0f;
	[Export] public float Max = 100.0f;
	[Export] public float Step = 0.1f;
	[Export] public string Postfix = "";
	
	#region internal
	public override void set_Value(float value) {
		base.set_Value(value);
		InternalValue = value;
		UpdateWidgets();
	}
	
	protected override void UpdateWidgets() {
		foreach (var range in new Range[] {spinBox, slider}) {
			if (range == null) continue;
			range.Value = InternalValue;
		}
		if (spinBox != null) spinBox.Suffix = Postfix;

		if (tabber != null) {
			if (StringValueOverride == null) {
				tabber.CurrentTab = 0; // SpinBox
			}
			else {
				overrideLabel.Text = StringValueOverride;
				tabber.CurrentTab = 1; // Override text
			}
		}
	}
	#endregion
	
	public override void _Ready() {
		base._Ready();
		tabber.CurrentTab = 0;  // SpinBox
		foreach (var range in new Range[] {spinBox, slider}) {
			if (range == null) continue;
			range.MinValue = Min;
			range.MaxValue = Max;
			range.Value = InternalValue;
			range.Step = Step;
		}
		
		// Connecting signals
		if (Engine.IsEditorHint()) return;
		foreach (var range in new Range[] { spinBox, slider }) {
			range.ValueChanged += newValue => {
				Value = (float) newValue;
				EmitSignal(
					nameof(ValueChanged),
					/* Value */ Value,
					/* isMin */ Mathf.RoundToInt(Value) == Mathf.RoundToInt(Min), // TODO: Write better float comparison
					/* isMax */ Mathf.RoundToInt(Value) == Mathf.RoundToInt(Max)  // TODO: Write better float comparison
				);
			};
		}
	}
}
