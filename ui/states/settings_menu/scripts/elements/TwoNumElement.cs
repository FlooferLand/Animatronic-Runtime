using System.Collections.Generic;

namespace Project.SettingsMenuParser.Elements;

using System.Reflection;
using Godot;

public class TwoNumElement : BaseElement<Vector2, HBoxContainer> {
    public List<LineEdit> TextBoxes = new();
    public TwoNumElement(Vector2 value, FieldInfo section, PropertyInfo property) : base(value, section, property) {
        var parent = new HBoxContainer();
        foreach (float elem in new[] { Value.X, Value.Y }) {
            var node = new LineEdit();
            node.Text = $"{elem}";
            parent.AddChild(node);
            TextBoxes.Add(node);
        }
        Node = parent;
		
        // TODO: Add safety check for the parses
        float ParseX(string text) {
            if (!float.TryParse(text, out float output))
                output = Value.X;
            return output;
        }
        float ParseY(string text) {
            if (!float.TryParse(text, out float output))
                output = Value.Y;
            return output;
        }
		
        // Connecting to X and Y value stuff
        TextBoxes[0].TextChanged += (newVal) => {
            float x = ParseX(newVal);
            Value.X = x;
            SetEditorSetting(Value);
        };
        TextBoxes[1].TextChanged += (newVal) => {
            float y = ParseY(newVal);
            Value.Y = y;
            SetEditorSetting(Value);
        };
    }
}
