namespace Project;
using Godot;

// This script is a mess, but it works and its just head bobbing
public partial class BobManager : Node3D {
	// Nodes
	[Export] private FootstepManager footstepManager;
	
	// Settings
	public float HeadbobIntensity = 0.2f;
	public float HeadbobSpeed = 10f;
	
	// Variables
	private Vector3 startPos;
	private float old;
	private float current;
	private bool swaySwitch;
	private float swayMod;
	
	public override void _Ready() {
        startPos = Position;
        footstepManager.Step += _OnStep;
	}

	private void _OnStep(float force) {
		current = (force / 2f) * HeadbobIntensity;
		swaySwitch = !swaySwitch;
	}

	public override void _Process(double delta) {
		// Moving upwards
		old = Mathf.Lerp(old, current, (HeadbobSpeed) * (float)delta);
		
		// Moving downwards
		current = Mathf.Lerp(current, 0f, (HeadbobSpeed * 0.6f) * (float)delta);
		
		// Setting the position
		Position = Position.WithY(startPos.Y - old);
		
		// Setting the rotation (sway)
		swayMod = Mathf.Lerp(swayMod, (swaySwitch ? -1f : 1f), 5 * (float) delta);
		Rotation = Rotation.WithZ(Rotation.Z + old * swayMod * 0.015f);
	}
}
