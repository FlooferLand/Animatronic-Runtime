extends Node
class_name FootstepManager

# Nodes
@onready var raycast: RayCast3D = $RayCast
@onready var audio: AudioStreamPlayer = $AudioPlayer
@onready var timer: Timer = $Timer

# Settings
@export_group("Settings")
@export var volume_db := 0.0
@export var frequency := 0.4

# Variables
var sounds := {}

# Node funcs
func _enter_tree():
	const FOOTSTEPS_PATH := "res://sound/footsteps/"
	for surface_type in DirAccess.get_directories_at(FOOTSTEPS_PATH):
		var steps: Array[AudioStream]
		var slips: Array[AudioStream]
		var surface_dir = FOOTSTEPS_PATH.path_join(surface_type)
		for file in DirAccess.get_files_at(surface_dir):
			if file.ends_with("import"):
				continue
			
			var stream = load(surface_dir.path_join(file))
			if file.begins_with("step"):
				steps.push_back(stream)
			elif file.begins_with("slip"):
				slips.push_back(stream)
			else:
				printerr("FOUND FOOTSTEP IS NOT A 'STEP' OR A 'SLIP'")
		
		# Adding the new sound
		sounds[surface_type] = { "steps": steps, "slips": slips }
func _ready():
	timer.wait_time = frequency
	audio.volume_db = volume_db

# Funcs
func play():
	timer.start()

func stop():
	timer.stop()

func is_playing() -> bool:
	return not timer.is_stopped()

func oneshot() -> void:
	var steps: Array[AudioStream] = sounds["porcelain"]["steps"]
	audio.stream = steps[randi_range(0, len(steps)-1)]
	audio.play()
