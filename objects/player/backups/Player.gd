extends CharacterBody3D

# Most code here totally not stolen from https://github.com/rbarongr/GodotFirstPersonController

# Nodes
@onready var head: Node3D = $Head
@onready var footstep_manager: FootstepManager = $FootstepManager

# Settings
@export_group("Settings")
@export_range(1, 20, 0.1)    var speed := 3.0
@export_range(5, 40, 0.1)    var acceleration := 15.0
@export_range(5, 20, 1)      var jump_height := 9.0
@export_range(0.1, 10, 0.05) var mouse_sensitivity := 7.0
@export var camera_smoothing := false

# Variables
var gravity: float = ProjectSettings.get_setting("physics/3d/default_gravity")
var jumping := false
var _mouse_motion := Vector2.ZERO
var _mouse_smooth_motion := Vector2.ZERO
var _initial_speed := speed

func _ready():
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _input(event: InputEvent):
	if event is InputEventMouseMotion and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		_mouse_motion = event.relative * (0.01 if camera_smoothing else 0.02)

func _process(delta):
	# Head movement
	var head_rot: Vector3 = head.rotation
	var motion := Vector2.ZERO
	if camera_smoothing:
		_mouse_smooth_motion = _mouse_smooth_motion.lerp(_mouse_motion, 5 * delta)
		motion = _mouse_smooth_motion
	else:
		motion = _mouse_motion
	head_rot.y -= motion.x * mouse_sensitivity * delta
	head_rot.x = clamp(head_rot.x - motion.y * mouse_sensitivity * delta, -(PI/2), (PI/2))
	head.rotation = head_rot
	_mouse_motion = Vector2.ZERO
	
	# Footstep sounds
	if is_on_floor():
		var i: Vector3 = velocity.abs().normalized()
		var intensity = sign(i.x + i.y + i.z)
		
		if not footstep_manager.is_playing() and intensity > 0.5:
			footstep_manager.play()
		elif footstep_manager.is_playing() and intensity < 0.5:
			footstep_manager.stop()
	elif footstep_manager.is_playing():
		footstep_manager.stop()
	
	# Cursor lock/unlock
	if Input.is_action_just_pressed("escape"):
		match Input.mouse_mode:
			Input.MOUSE_MODE_VISIBLE:
				Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
			_:
				Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _physics_process(delta):
	var move_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var forward:  Vector3 = head.transform.basis * Vector3(move_dir.x, 0, move_dir.y)
	var walk_dir: Vector3 = Vector3(forward.x, 0, forward.z).normalized()
	
	# Applying gravity
	if not is_on_floor():
		velocity.y = lerp(velocity.y, velocity.y - (gravity + abs(velocity.y*0.3)), 2.8 * delta)
	else:
		if velocity.y > 0.1:
			footstep_manager.oneshot()
		velocity.y = 0
	
	# Jumping
	if is_on_floor() and Input.is_action_just_pressed("jump"):
		velocity.y = jump_height * 0.15
		footstep_manager.oneshot()
		jumping = true
	if jumping and not is_on_floor():
		velocity.y = lerp(velocity.y, velocity.y + jump_height, 15 * delta)
		if velocity.y >= jump_height * 0.9:
			jumping = false
	
	# Sprinting
	if Input.is_action_just_pressed("sprint"):
		speed = _initial_speed * 2
	if Input.is_action_just_released("sprint"):
		speed = _initial_speed
		
	# Applying movement
	velocity = velocity.move_toward(walk_dir * speed, acceleration * delta)
	move_and_slide()
