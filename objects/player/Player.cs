using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Project;
using Godot;

// Most code here totally not stolen from https://github.com/rbarongr/GodotFirstPersonController

public partial class Player : CharacterBody3D {
	// Nodes
	private Node3D head;
	private SpotLight3D flashlight;
	private FootstepManager footstepManager;
	private BobManager bobManager;
	private RayCast3D interactRay;
	
	#region Settings
	[ExportGroup("Settings")]
	[Export] public bool CameraSmoothing;

	[Export(PropertyHint.Range, "1, 20, 0.1")]
	public float Speed = 2.5f;
	
	[Export(PropertyHint.Range, "5, 40, 0.1")]
	public float Acceleration = 15f;
	
	[Export(PropertyHint.Range, "5, 20, 1")]
	public float JumpHeight = 8f;
	
	[Export(PropertyHint.Range, "0.1, 10, 0.05")]
	public float MouseSensitivity = 7f;
	
	#endregion
	
	// Variables
	private float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private bool jumping;
	private Vector2 rawMouseMotion = Vector2.Zero;
	private Vector2 mouseMotion = Vector2.Zero;
	private float initialSpeed;

	[Pure]
	private Vector3 ProcessCameraMovement(Vector3 startPos, float delta, bool raw = false) {
		Vector3 rotation = startPos;
		const float bound = Mathf.Pi / 2f;

		Vector2 motion = rawMouseMotion;
		if (!raw) {
			var speed = (CameraSmoothing ? 0.8f : 25f);
			mouseMotion = mouseMotion.Lerp(rawMouseMotion, speed * delta);
			motion = mouseMotion;
			rawMouseMotion = Vector2.Zero;
		}

		rotation.Y -= motion.X * MouseSensitivity * delta;
		rotation.X = Mathf.Clamp(rotation.X - motion.Y * MouseSensitivity * delta, -bound, bound);
		return rotation;
	}
	
	public override void _Ready() {
		head = GetNode<Node3D>("Head");
		flashlight = GetNode<SpotLight3D>("Flashlight");
		interactRay = head.GetNode<RayCast3D>("Interaction");
		footstepManager = GetNode<FootstepManager>("FootstepManager");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		bobManager = (BobManager) head;
		initialSpeed = Speed;
		
		// Settings
		SlideOnCeiling = false;
		FloorBlockOnWall = true;
		WallMinSlideAngle = 5f;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
			rawMouseMotion = motion.Relative * 0.01f;
	}

	private ILookDetector lastLookedAt;
	public override void _Process(double delta) {
		#region Cursor lock/unlock
		if (Input.IsActionJustPressed("escape")) {
			switch (Input.MouseMode) {
				case Input.MouseModeEnum.Visible:
					Input.MouseMode = Input.MouseModeEnum.Captured;
					break;
				default:
					Input.MouseMode = Input.MouseModeEnum.Visible;
					break;
			}
		}
		#endregion
		
		// Return if the player isn't focusing on the window!
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
			return;
		
		#region Flashlight + Camera rotation
		if (Input.IsActionJustPressed("flashlight_toggle"))
			flashlight.Visible = !flashlight.Visible;
		
		flashlight.Rotation = ProcessCameraMovement(flashlight.Rotation, (float) delta, true);
		flashlight.Rotation = flashlight.Rotation.Lerp(head.Rotation, 0.5f * (float) delta);
		head.Rotation = ProcessCameraMovement(head.Rotation, (float) delta);
		
		// Camera sway towards look direction
		var sway = mouseMotion.X * 0.03f;
		head.Rotation = head.Rotation.WithZ(
			Mathf.Lerp(head.Rotation.Z, sway, 8f * (float) delta)
		);
		#endregion
		
		#region Look detection + Interaction
        if (interactRay.GetCollider() is Node collider) {
	        var obj = collider.GetOwner<Node3D>();
	        
	        // Look detection
	        // TODO: Not fully finished
	        if (obj is ILookDetector detector) {
		        // var key = obj.GetInstanceId();
		        detector.LookDetector(true, interactRay);
		        lastLookedAt ??= detector;  // Assign if null
		        if (lastLookedAt != detector)
			        lastLookedAt.LookDetector(false, interactRay);
	        } else if (lastLookedAt != null) {
		        lastLookedAt.LookDetector(false, interactRay);
		        lastLookedAt = null;
	        }
	        
	        // Triggering interaction
	        // TODO: Make this mess better
	        if (obj is IBaseInteractable interactable) {
		        var actions = new List<string> { "primary_interact", "secondary_interact" };
		        foreach (string action in actions) {
                    InteractButton button = InteractButton.Primary;
                    if (action == "secondary_interact")
	                    button = InteractButton.Secondary;

                    if (Input.IsActionJustPressed(action))
				        interactable.Interact(interactRay, InteractState.Press, button);
			        else if (Input.IsActionPressed(action))
				        interactable.Interact(interactRay, InteractState.Hold, button);
			        else if (Input.IsActionJustReleased(action))
				        interactable.Interact(interactRay, InteractState.Release, button);
		        }
	        }
        } else if (lastLookedAt != null) {
	        lastLookedAt.LookDetector(false, interactRay);
	        lastLookedAt = null;
        }
		#endregion
		
		#region Footstep sounds
		if (IsOnFloor()) {
			Vector3 i = Velocity.Abs().Normalized();
			float intensity = Mathf.Sign(i.X + i.Y + i.Z);

			if (!footstepManager.IsPlaying() && intensity > 0.5f) {
				footstepManager.Play();
			} else if (footstepManager.IsPlaying() && intensity < 0.5f) {
				footstepManager.Stop();
			}
		}
		#endregion
	}

	// TODO: Should maybe move jumping and gravity to modifying walkDir instead of modifying Velocity directly
	public override void _PhysicsProcess(double delta) {
		Vector2 moveDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 forward = head.Transform.Basis * new Vector3(moveDir.X, 0, moveDir.Y);
		Vector3 walkDir = new Vector3(forward.X, 0, forward.Z).Normalized();
		
		#region Applying gravity
		if (!IsOnFloor()) {
			Velocity = Velocity.WithY(
				Mathf.Lerp(
					Velocity.Y, 
					Velocity.Y - (gravity + Mathf.Abs(Velocity.Y * 0.3)),
					3f * delta
				)
			);
		} else Velocity = Velocity.WithY(0);
		#endregion
		
		#region Jumping
		if (IsOnFloor() && Input.IsActionJustPressed("jump")) {
			Velocity = Velocity.WithY(JumpHeight * 0.15);
			footstepManager.Jump();
			jumping = true;
		}
		if (jumping && !IsOnFloor()) {
			Velocity = Velocity.WithY(Mathf.Lerp(Velocity.Y, Velocity.Y + JumpHeight, 15 * (float) delta));
			if (Velocity.Y >= JumpHeight * 0.9)
				jumping = false;
		}
		#endregion

		#region Sprinting
		// TODO: Make FootstepManager use the player's speed directly as it's frequency (setters/getters)
		if (Input.IsActionJustPressed("sprint")) {
			Speed = initialSpeed * 2;
			footstepManager.VolumeBoost = 2f;
			footstepManager.Frequency = footstepManager.InitialFrequency * 0.6f;
		}
		if (Input.IsActionJustReleased("sprint")) {
			Speed = initialSpeed;
			footstepManager.VolumeBoost = 0f;
			footstepManager.Frequency = footstepManager.InitialFrequency;
		}
		#endregion
		
		// Moving the player
		Velocity = Velocity.MoveToward(walkDir * Speed, Acceleration * (float) delta);
		MoveAndSlide();
	}
}
