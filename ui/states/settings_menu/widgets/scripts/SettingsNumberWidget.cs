using System.Collections.Generic;

namespace Project;
using Godot;

[Tool]
public partial class SettingsNumberWidget : SettingsBaseWidget {
	// Nodes
	[GetNode("{WidgetControl}/Container/SpinBox")] private SpinBox spinBox;
	[GetNode("{WidgetControl}/Container/Slider")] private Slider slider;
	
	// Signals
	[Signal] public delegate void ValueChangedEventHandler(float value);
	
	// Settings
	private float min = 0.0f;
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
		UpdateWidgets();
		
		if (Engine.IsEditorHint()) return;
		foreach (var range in new Range[] { spinBox, slider }) {
			range.ValueChanged += (newValue) => {
				Value = (float) newValue;
				UpdateWidgets();
				EmitSignal(nameof(ValueChanged), Value);
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
	}
}
