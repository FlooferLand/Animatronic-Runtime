using System.Collections.Generic;

namespace Project;
using Godot;

[Tool]
public partial class SettingsNumberWidget : SettingsBaseWidget {
	// Nodes
	[GetNode("{WidgetControl}/Container/Tabbed")] private TabContainer tabber;
	[GetNode("{WidgetControl}/Container/Slider")] private Slider slider;
	[GetNode("{tabber}/SpinBox")] private SpinBox spinBox;
	[GetNode("{tabber}/OverrideLabel")] private Label overrideLabel;
	
	// Signals
	[Signal] public delegate void ValueChangedEventHandler(float value, bool isMin, bool isMax);
	
	// Settings
	public string StringValueOverride = null;
	private float min = 0f;
	[Export] public float Min {
		get => min;
		set {
			min = value;
			UpdateWidgets();
		}
	}
	private float max = 100.0f;
	[Export] public float Max {
		get => max;
		set {
			max = value;
			UpdateWidgets();
		}
	}
	
	// Variables
	private float value;
	public float Value {
		get => value;
		set {
			this.value = value;
			UpdateWidgets();
		}
	}
	
	public override void _Ready() {
		base._Ready();
		tabber.CurrentTab = 0;  // SpinBox
		
		// Connecting signals
		if (Engine.IsEditorHint()) return;
		foreach (var range in new Range[] { spinBox, slider }) {
			range.ValueChanged += newValue => {
				Value = (float) newValue;
				UpdateWidgets();
				EmitSignal(
					nameof(ValueChanged),
					/* Value */ Value,
					/* isMin */ Mathf.RoundToInt(Value) == Mathf.RoundToInt(Min), // TODO: Write better float comparison
					/* isMax */ Mathf.RoundToInt(Value) == Mathf.RoundToInt(Max)  // TODO: Write better float comparison
				);
			};
		}
	}

	private void UpdateWidgets() {
		foreach (var range in new Range[] {spinBox, slider}) {
			if (range == null) continue;
			range.MinValue = min;
			range.MaxValue = max;
			range.Value = value;
		}

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
}
