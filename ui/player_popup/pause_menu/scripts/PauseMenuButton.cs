namespace Project;
using Godot;

[Tool]
public partial class PauseMenuButton : Control {
	// Nodes
	[GetNode("Button")] private Button button;
	
	// Signals
	[Signal] public delegate void PressedEventHandler();
	
	// State
	private string text;
	[Export] public string Text {
		get => text;
		set {
			text = value;
			if (button != null)
				button.Text = value;
		}
	}

	public override void _Ready() {
		if (Text != null) Text = text;

		if (!Engine.IsEditorHint()) {
			button.Pressed += () => EmitSignal(nameof(Pressed));
		}
	}
}
