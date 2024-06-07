using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Project.SettingsMenuParser; 

// TODO: This file should be reformatted. Code is very messy, imported directly from the old project
// TODO: I should implement a way to specify default values, so controls like LineEdit show the default value when no value is entered.

#region Defining the settings elements
// TODO: I should maybe make the `Connect`s return a reference to the current object instead of the value
// TODO: I should make `Value` have a setter of `SetEditorSetting` and a getter of `GetEditorSetting` so it syncs up with EngineSettings
public delegate void ParamFunc<in T>(T value);
public interface IElement {
	public Control GetNode();
}
public abstract class BaseElement<TValue, TNode> : IElement where TNode: Control {
	private FieldInfo section;
	private FieldInfo field;
	
	public TValue Value;
	public TNode Node;

	protected BaseElement(TValue value, FieldInfo section, FieldInfo field) {
		this.section = section;
		this.field = field;
		Value = value;
	}
	
	public void SetEditorSetting(TValue value) {
		object settingsData = section.GetValue(EngineSettings.Data);
		object fieldValue = field.GetValue(settingsData);
		try {
			object converted = Convert.ChangeType(value, fieldValue.GetType());
			field.SetValue(settingsData, converted);
		}
		catch {
			// Manual, annoying casting, because the automatic cast failed
			dynamic val = value;
			if (value.GetType() != field.GetType()) {
				if (value is Vector2 vec && fieldValue is Vector2I) {
					val = new Vector2I((int) vec.X, (int) vec.Y);
				}
			}
			field.SetValue(settingsData, val);
		}
	}
	public Control GetNode() => Node;
}
public class SliderElement : BaseElement<float, HSlider> {
	public float Min, Max=10f, Step=0.5f;
	public SliderElement(float value, FieldInfo section, FieldInfo field) : base(value, section, field) {
		var node = new HSlider();
		node.MinValue = Min;
		node.MaxValue = Max;
		node.Step = Step;
		node.Value = Value;
		Node = node;
		
		Node.ValueChanged += (val) => {
			Value = (float) val;
			SetEditorSetting(Value);
		};
	}
}
public class CheckboxElement : BaseElement<bool, CheckBox> {
	public CheckboxElement(bool value, FieldInfo section, FieldInfo field) : base(value, section, field) {
		var node = new CheckBox();
		node.ButtonPressed = Value;
		Node = node;
		
		Node.Pressed += () => {
			Value = Node.ButtonPressed;
			SetEditorSetting(Value);
		};
	}
}

public class TextBoxElement : BaseElement<string, LineEdit> {
	public TextBoxElement(string value, FieldInfo section, FieldInfo field) : base(value, section, field) {
		var node = new LineEdit();
		node.Text = Value;
		Node = node;
		
		Node.TextChanged += (str) => {
			Value = str;
			SetEditorSetting(Value);
		};
	}
}

public class TwoNumElement : BaseElement<Vector2, HBoxContainer> {
	public List<LineEdit> TextBoxes = new();
	public TwoNumElement(Vector2 value, FieldInfo section, FieldInfo field) : base(value, section, field) {
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
#endregion

// Main parser
public static class Parser {
	/// Can return null. Return value should be casted to some form of BaseElement
	public static IElement ParseFieldAttributes(FieldInfo sectionField, FieldInfo field) {
		// if (sectionField.ReflectedType is null) return null;
		
		// Getting the core attrib
		object settingsData = sectionField.GetValue(EngineSettings.Data);
		var element = field.GetValue(settingsData) switch {
			// Numbers
			float num => new SliderElement(num, sectionField, field),
			int num => new SliderElement(num, sectionField, field),

			// Vectors
			Vector2 vector => new TwoNumElement(vector, sectionField, field),
			Vector2I vector => new TwoNumElement(vector, sectionField, field),

			// Misc
			bool ticked => new CheckboxElement(ticked, sectionField, field),
			string text => new TextBoxElement(text, sectionField, field),

			// Not implemented
			{ } other => ((Func<IElement>)(() => {
				OS.Alert($"Settings menu UI element \"{other.GetType()}\" is not implemented!", "Not implemented!");
				return null;
			}))()
		};
		
		// Parsing the attributes (min/max slider boundaries, etc)
		var attributes = field.CustomAttributes;
		foreach (var attrib in attributes) {
			bool hasPropertyHint = false;
			foreach (var arg in attrib.ConstructorArguments) {
				if (arg.ArgumentType.Name == "PropertyHint") {
					hasPropertyHint = true;
					continue;
				}
				
				// Some safety checks
				if (arg.Value is null || arg.Value.ToString() is null)
					continue;

				// Using the next argument as the hint
				if (hasPropertyHint) {
					var list = arg.Value.ToString()!.Split(",").ToList();
					float[] split = (from x in list select float.Parse(x.Trim())).ToArray();

					(float min, float max, float step) = (split[0], split[1], split[2]);
					if (element is SliderElement sliderElement) {
						sliderElement.Min = min;
						sliderElement.Max = max;
						sliderElement.Step = step;
					}
				}
			}
		}

		return element;
	}
}
