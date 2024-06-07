namespace Project;
using Godot;

public partial class BobManager : Node3D {
	// Nodes
	[Export] private FootstepManager footstepManager;
	
	// Settings
	public float HeadbobIntensity = 0.03f;
	public float HeadbobSpeed = 12f;
	
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
		Rotation = Rotation.WithZ(Mathf.Lerp(Rotation.Z, 0f, 5 * (float) delta));

		// Camera sway towards look direction
		// TODO: Add camera sway towards look direction back in
		// float movementDiff = mouseMotion.X * 0.5f;
		// Rotation = Rotation.WithZ(
		// 	Mathf.Lerp(Rotation.Z, sway, 15f * (float) delta)
		// );
	}
}
