namespace Project.SettingsMenuParser.Elements;

using System.Reflection;
using Godot;

public class EnumElement : BaseElement<object, OptionButton> {
    public EnumElement(object value, FieldInfo section, PropertyInfo property) : base(value, section, property) {
        var node = new OptionButton();
        foreach (object enumValue in value.GetType().GetEnumValues()) {
            node.AddItem(enumValue.ToString());
        }
        Node = node;
		
        Node.ItemSelected += (id) => {
            // Node.GetItemText()
            // Value = str;
            SetEditorSetting(Value);
        };
    }
}
