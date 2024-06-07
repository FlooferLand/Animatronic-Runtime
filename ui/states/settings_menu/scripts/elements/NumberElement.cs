namespace Project.SettingsMenuParser.Elements;

using System.Reflection;
using Godot;

public class NumberElement : BaseElement<float, HBoxContainer> {
    public float Min, Max=10f, Step=0.5f;
    public NumberElement(float value, FieldInfo section, PropertyInfo property) : base(value, section, property) {
        var parent = new HBoxContainer();
        var slider = new HSlider();
        var spinBox = new SpinBox();
        
        // Attributes
        foreach (var attribute in property.CustomAttributes) {
            if (attribute.ConstructorArguments.Count != 2) continue;
            if (attribute.ConstructorArguments[0].ArgumentType == typeof(PropertyHint)) {
               string[] rangeInfo = (attribute.ConstructorArguments[1].Value as string).Split(",");
               Min = float.Parse(rangeInfo[0]);
               Max = float.Parse(rangeInfo[1]);
               if (rangeInfo.Length > 2) Step = float.Parse(rangeInfo[2]);
            }
        }

        // Slider
        slider.MinValue = Min;
        slider.MaxValue = Max;
        slider.Step = Step;
        slider.Value = Value;
        slider.ValueChanged += (val) => {
            Value = (float) val;
            spinBox.Value = val;
            SetEditorSetting(Value);
        };
        slider.SetCustomMinimumSize(new Vector2(192f, 8f));
        parent.AddChild(slider);
        
        // Number edit thingy
        spinBox.MinValue = Min;
        spinBox.MaxValue = Max;
        spinBox.Step = Step;
        spinBox.Value = Value;
        spinBox.ValueChanged += (val) => {
            Value = (float) val;
            slider.Value = val;
            SetEditorSetting(Value);
        };
        parent.AddChild(spinBox);
        
        Node = parent;
    }
}