namespace Project;
using Godot;

public partial class Flashlight : Node3D {
	[GetNode("Light")] private SpotLight3D light;
	[GetNode("ToggleAudio")] private AudioStreamPlayer toggleAudio;

	private bool active;
	public bool Active {
		get => active;
		set {
			active = value;
			if (light != null) light.Visible = value;
		}
	}
	
	#region State (turn on, turn off, etc)
	public void TurnOn() {
		Active = true;
		toggleAudio?.Play();
	}

	public void TurnOff() {
		Active = false;
		toggleAudio?.Play();
	}
	
	public void Toggle() {
		if (Active)
			TurnOff();
		else
			TurnOn();
	}
	#endregion

	public override void _Ready() {
		Active = false;
	}
}
