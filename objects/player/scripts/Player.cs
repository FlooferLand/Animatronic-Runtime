using System.Collections.Generic;

namespace Project;
using Godot;

// TODO: Finish this and uncomment code

public partial class Player : CharacterBody3D {
	#region Nodes
	[GetNode("Head")]				private Node3D head;
	[GetNode("Head/Camera")]		private Camera3D camera;
	[GetNode("Head/InteractRay")]	private RayCast3D interactRay;
	[GetNode("Head/Flashlight")]	private Flashlight flashlight;
	[GetNode("FootstepManager")]	private FootstepManager footstepManager;
	
	// UI
	[GetNode("{camera}/Canvas/PlayerHUD")]	private Control playerHud;
	[GetNode("{playerHud}/PauseMenu")]		private PauseMenu pauseMenu;
	#endregion
	
	#region Settings
	[ExportGroup("Settings")]
	[Export] public bool CameraSmoothing;

	[Export(PropertyHint.Range, "1, 20, 0.1")]
	public float Speed = 3.0f;
	
	[Export(PropertyHint.Range, "5, 40, 0.1")]
	public float Acceleration = 10f;
	
	[Export(PropertyHint.Range, "5, 20, 1")]
	public float JumpHeight = 8f;
	
	[Export(PropertyHint.Range, "0.1, 5.0, 0.05")]
	public float MouseSensitivity = 1.0f;
	#endregion
	
	// Variables
	private ILookDetector lastLookedAt;
	private float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private bool jumping;
	private float initialSpeed;

	public override void _Ready() {
		base._Ready();
		initialSpeed = Speed;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		// Settings
		SlideOnCeiling = false;
		FloorBlockOnWall = true;
		WallMinSlideAngle = 5f;
	}
	
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured) {
			var relative = motion.Relative * 0.08f;
			var rotation = head.RotationDegrees;
			rotation.Y -= relative.X * MouseSensitivity;
			rotation.X -= relative.Y * MouseSensitivity;
			rotation.X = Mathf.Clamp(rotation.X, -90, 90);
			head.RotationDegrees = rotation;
		}
	}

	public override void _Process(double delta) {
		// Return if the player isn't focusing on the window!
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
			return;
		
		#region Flashlight + Camera rotation
		if (Input.IsActionJustPressed("toggle_flashlight"))
			flashlight.Toggle();
		#endregion
		
		#region Look detection + Interaction
        if (interactRay.GetCollider() is Node collider) {
	        var obj = collider.GetOwner<Node3D>();
	        
	        // Look detection
	        // TODO: Not fully finished
	        if (obj is ILookDetector detector) {
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
		        var actions = new List<string> { "interact_primary", "interact_secondary" };
		        foreach (string action in actions) {
                    InteractButton button = InteractButton.Primary;
                    if (action == "interact_secondary")
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
	
	// Player movement & etc
	public override void _PhysicsProcess(double delta) {
		// Return if the player isn't focusing on the window!
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
			return;
		
		Vector2 moveDir = Input.GetVector("walk_left", "walk_right", "walk_forward", "walk_backward");
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
