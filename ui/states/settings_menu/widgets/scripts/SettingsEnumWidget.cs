using System;
using System.Linq;
using Godot.Collections;

namespace Project;
using Godot;

public partial class SettingsEnumWidget : SettingsBaseWidget<int> {
    // Nodes
    [GetNode("{WidgetControl}/Dropdown")] private OptionButton dropdown;
    
    // Signals ? (not really)
    public delegate void OnItemSelected<in TEnum>(TEnum value) where TEnum: struct, Enum;
    
    // Variables
    private Type enumType = null;
    public System.Collections.Generic.Dictionary<int, Enum> EnumValues = new();
    private System.Collections.Generic.Dictionary<Enum, int> enumIndexes = new();

    #region internal
    public void LoadFrom<TEnum>(TEnum value) where TEnum: struct, Enum {
	    int index = SetValueEnum(value);
	    if (index >= 0) {
		    Value = index;
		    base.LoadFrom(index);
	    }
    }

    protected override void UpdateWidgets() {
	    if (enumType == null || dropdown == null) return;
	    dropdown.Selected = InternalValue;  // TODO: update this
    }
    #endregion

    // _Ready but better
    public void Init<TEnum>(OnItemSelected<TEnum> onItemSelected) where TEnum: struct, Enum {
	    if (dropdown == null) {
		    Log.Error($"The dropdown enum widget for the setting \"{WidgetName}\" is null");
		    return;
	    }
	    
	    // Enum stuff
	    enumType = typeof(TEnum);
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
		    if (EnumValues[Value] is TEnum heeHeeHaw)
				onItemSelected(heeHeeHaw);
	    };
    }

    /// Finds the index of the enum and sets the value.
    /// Sets nothing and returns -1 if the value wasn't set
    public int SetValueEnum(Enum @enum) {
	    if (EnumValues.Count == 0) return -1;
	    if (enumIndexes.TryGetValue(@enum, out int val)) {
		    Value = val;
		    dropdown.Selected = val;
		    return val;
	    }
	    return -1;
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
