using Godot;
using System;
using System.Linq;
using System.Reflection;
using Project.SettingsMenuParser.Elements;

namespace Project.SettingsMenuParser; 

// TODO: This file should be reformatted. Code is very messy, imported directly from the old project
// TODO: I should implement a way to specify default values, so controls like LineEdit show the default value when no value is entered.

// TODO: I should maybe make the `Connect`s return a reference to the current object instead of the value
// TODO: I should make `Value` have a setter of `SetEditorSetting` and a getter of `GetEditorSetting` so it syncs up with EngineSettings

// Main parser
public static class Parser {
	/// Can return null. Return value should be cast to some form of BaseElement
	public static IElement ParseFieldAttributes(FieldInfo sectionField, PropertyInfo field) {
		// if (sectionField.ReflectedType is null) return null;
		
		// Getting the core attrib
		object settingsData = sectionField.GetValue(EngineSettings.Data);
		var element = field.GetValue(settingsData) switch {
			// Numbers
			float num => new NumberElement(num, sectionField, field),
			int num => new NumberElement(num, sectionField, field),

			// Vectors
			Vector2 vector => new TwoNumElement(vector, sectionField, field),
			Vector2I vector => new TwoNumElement(vector, sectionField, field),

			// Misc
			bool ticked => new CheckboxElement(ticked, sectionField, field),
			string text => new TextboxElement(text, sectionField, field),
			{} other when other.GetType().IsEnum => new EnumElement(other, sectionField, field),

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
					if (element is NumberElement sliderElement) {
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
