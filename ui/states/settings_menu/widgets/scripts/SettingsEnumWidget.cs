#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project;
using Godot;

[Tool]
public partial class SettingsEnumWidget : SettingsBaseWidget {
    // Nodes
    [GetNode("{WidgetControl}/Dropdown")] private OptionButton dropdown;
    
    // Signals
    [Signal] public delegate void ValueChangedEventHandler(int value);
    
    // Variables
    private Type? enumType = null;
    public Dictionary<int, Enum> EnumValues = new();
    private Dictionary<Enum, int> enumIndexes = new();
    private int selected;
    public int Value {
	    get => selected;
	    set {
		    selected = value;
		    UpdateWidgets();
	    }
    }
    
    public override void _Ready() {
    	base._Ready();
    	UpdateWidgets();
    	
    	if (Engine.IsEditorHint()) return;
    	dropdown.ItemSelected += (itemId) => {
		    if (EnumValues.Count == 0) {
			    Value = (int) itemId;
		    } else {
			    Log.Error($"\"{nameof(EnumValues)}\" is null in {nameof(dropdown.ItemSelected)}");
		    }
		    UpdateWidgets();
    		EmitSignal(nameof(ValueChanged), Value);
    	};
    }

    private void UpdateWidgets() {
	    if (enumType == null) return;
	    
	    dropdown.Selected = selected;  // TODO: update this
    }
    
    public void Init<T>() {
	    if (!typeof(T).IsEnum) {
		    Log.Error($"Type {typeof(T).FullName} is not an enum!");
		    return;
	    }
	    enumType = typeof(T);
	    var values = enumType.GetEnumValues();

	    for (int i = 0; i < values.Length; i++) {
		    object? value = values.GetValue(i);
		    if (value is not Enum enumValue) continue;
		    
		    EnumValues.Add(i, enumValue);
		    enumIndexes.Add(enumValue, i);
		    dropdown.AddItem(enumValue.ToString(), id: i);
	    }
    }

    /// Finds the index of the enum and sets the value.
    /// Sets nothing and returns false if the value wasn't set
    public bool SetValueEnum(Enum @enum) {
	    if (EnumValues.Count == 0) return false;

	    if (enumIndexes.TryGetValue(@enum, out int val)) {
		    Value = val;
		    dropdown.Selected = val;
		    return true;
	    }

	    return false;
    }
}

[Tool]
public partial class SettingsEnumWidgetInspectorPlugin : EditorInspectorPlugin {
	public override bool _CanHandle(GodotObject obj) {
		return obj is SettingsEnumWidget;
	}

	public override void _ParseBegin(GodotObject obj) {
		if (obj is not SettingsEnumWidget widget) return;

		var container = new VBoxContainer();

		var showContents = new CheckButton();
		showContents.Text = "Show assigned values";
		showContents.ToggleMode = true;
		showContents.ButtonPressed = false;
		showContents.Toggled += (enabled) => {
			if (enabled) {
				var data = new Godot.Collections.Array<string>(
					widget.EnumValues.Values.Select((s) => s.ToString()).ToArray()
				);

				for (int i = 0; i < data.Count; i++) {
					string str = data[i];
					var label = new Label();
					label.Text = $"{i}: {str}";
					container.AddChild(label);
				}
				container.AddChild(new VSeparator());
			} else {
				foreach (var child in container.GetChildren()) {
					container.RemoveChild(child);
				}
			}
		};
		
		AddCustomControl(showContents);
		AddCustomControl(container);
	}
}
