using System;

namespace Project.SettingsMenuParser.Elements;

using System.Reflection;
using Godot;

public abstract class BaseElement<TValue, TNode> : IElement where TNode: Control {
    private FieldInfo section;
    private PropertyInfo property;
	
    public TValue Value;
    public TNode Node {
        get => internalNode;
        set {
            value.FocusMode = Control.FocusModeEnum.None;
            internalNode = value;
        }
    }
    private TNode internalNode;

    protected BaseElement(TValue value, FieldInfo section, PropertyInfo property) {
        this.section = section;
        this.property = property;
        Value = value;
    }
	
    public void SetEditorSetting(TValue value) {
        object settingsData = section.GetValue(EngineSettings.Data);
        object fieldValue = property.GetValue(settingsData);
        try {
            object converted = Convert.ChangeType(value, fieldValue.GetType());
            property.SetValue(settingsData, converted);
        }
        catch {
            // Manual, annoying casting, because the automatic cast failed
            dynamic val = value;
            if (value.GetType() != property.GetType()) {
                if (value is Vector2 vec && fieldValue is Vector2I) {
                    val = new Vector2I((int) vec.X, (int) vec.Y);
                }
            }
            property.SetValue(settingsData, val);
        }
    }
    public Control GetNode() => Node;
}