using System;
using System.Linq;
using Godot.Collections;

namespace Project;
using Godot;

[Tool]
public partial class SettingsEnumWidget : SettingsBaseWidget {
    // Nodes
    [GetNode("{WidgetControl}/Dropdown")] private OptionButton dropdown;
    
    // Signals ? (not really)
    public delegate void OnItemSelected<in T>(T value) where T: struct, Enum;
    
    // Variables
    private Type enumType = null;
    public System.Collections.Generic.Dictionary<int, Enum> EnumValues = new();
    private System.Collections.Generic.Dictionary<Enum, int> enumIndexes = new();
    private int selected;
    public int Value {
	    get => selected;
	    set {
		    selected = value;
		    UpdateWidgets();
	    }
    }

    private void UpdateWidgets() {
	    if (enumType == null || dropdown == null) return;
	    
	    dropdown.Selected = selected;  // TODO: update this
    }

    // _Ready but better
    public void Init<T>(OnItemSelected<T> onItemSelected) where T: struct, Enum {
	    if (dropdown == null) {
		    Log.Error($"The dropdown enum widget for the setting \"{WidgetName}\" is null");
		    return;
	    }
	    
	    // Enum stuff
	    enumType = typeof(T);
	    var values = enumType.GetEnumValues();

	    for (int i = 0; i < values.Length; i++) {
		    object value = values.GetValue(i);
		    if (value is not Enum enumValue) continue;
		    
		    EnumValues.Add(i, enumValue);
		    enumIndexes.Add(enumValue, i);
			dropdown.AddItem(enumValue.ToString(), id: i);
	    }
	    
	    // Connecting signals
	    if (Engine.IsEditorHint()) return;
	    dropdown.ItemSelected += itemId => {
		    if (EnumValues.Count != 0) {
			    Value = (int) itemId;
		    } else {
			    Log.Error($"\"{nameof(EnumValues)}\" has 0 values in {nameof(dropdown.ItemSelected)}");
		    }
		    UpdateWidgets();
		    if (EnumValues[Value] is T heeHeeHaw)
				onItemSelected(heeHeeHaw);
	    };
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

// TODO: Fix the error about EditorInspectorPlugin not existing
/*[Tool]
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
		showContents.Toggled += enabled => {
			if (enabled) {
				var data = new Array<string>(
					widget.EnumValues.Values.Select(s => s.ToString()).ToArray()
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
}*/
