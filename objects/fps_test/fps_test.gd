extends Node3D

@onready var test_model := $TestModel
@onready var audio := $Audio
var animating: bool = false
var audio_playhead: float = 0.0

func _ready() -> void:
	hide_model()

func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("fps_test"):
		if test_model.visible:
			hide_model()
		else:
			show_model()
	if Input.is_action_just_pressed("fps_test_anim"):
		animating = not animating
		if animating:
			# audio.play(audio_playhead)
			show_model()
		else:
			audio_playhead = audio.get_playback_position()
			audio.stop()

func _physics_process(delta: float) -> void:
	if not animating:
		return
	var node = test_model.get_node("metarig/")
	if node == null:
		node = test_model
	
	var intensity: float = 1.0;
	animate_recursive(node, delta, intensity)

func animate_recursive(node: Node3D, delta: float, intensity: float) -> void:
	for child in node.get_children():
		if child is Node3D:
			child.rotation_degrees += (Vector3.ONE * 100 * intensity) * delta;
			animate_recursive(child, delta, intensity * 0.9)

func show_model() -> void:
	test_model.visible = true
	test_model.process_mode = Node.PROCESS_MODE_ALWAYS
	if animating:
		audio.play(audio_playhead)

func hide_model() -> void:
	test_model.visible = false
	test_model.process_mode = Node.PROCESS_MODE_DISABLED
	audio_playhead = audio.get_playback_position()
	audio.stop()
