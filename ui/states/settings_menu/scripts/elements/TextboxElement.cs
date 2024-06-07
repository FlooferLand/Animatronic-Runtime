namespace Project.SettingsMenuParser.Elements;

using System.Reflection;
using Godot;

public class TextboxElement : BaseElement<string, LineEdit> {
    public TextboxElement(string value, FieldInfo section, PropertyInfo property) : base(value, section, property) {
        var node = new LineEdit();
        node.Text = Value;
        Node = node;
		
        Node.TextChanged += (str) => {
            Value = str;
            SetEditorSetting(Value);
        };
    }
}
