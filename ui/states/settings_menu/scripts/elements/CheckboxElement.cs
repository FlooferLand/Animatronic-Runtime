namespace Project.SettingsMenuParser.Elements;

using System.Reflection;
using Godot;

public class CheckboxElement : BaseElement<bool, CheckBox> {
    public CheckboxElement(bool value, FieldInfo section, PropertyInfo property) : base(value, section, property) {
        var node = new CheckBox();
        node.ButtonPressed = Value;
        Node = node;
		
        Node.Pressed += () => {
            Value = Node.ButtonPressed;
            SetEditorSetting(Value);
        };
    }
}