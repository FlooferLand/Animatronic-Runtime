extends Node3D

# Nodes
@export var player: CharacterBody3D

# Variables
@onready var startPos := position
var amplitude := 0.1
var frequency := 1.5
var time := 0.0

func _process(delta):
	var i: Vector3 = player.velocity.abs().normalized()
	var intensity: float = remap(
		(i.x + i.y + i.z),
		0, 1.5,
		0, 2.0
	)
	intensity = intensity if player.is_on_floor() else 0
	
	position = startPos + footstep_motion(delta, intensity)
	rotation.z = sin(time * frequency) * (amplitude / 6)

# Code from https://youtu.be/5MbR2qJK8Tc
func footstep_motion(delta: float, intensity: float) -> Vector3:	
	if intensity < 0.5:
		time = delta * 2.5 # lerp(time, 0.0, 1.5 * delta)
	else:
		time += delta * 5
	
	var sine = sin(time * frequency)
	var bob: float = abs(sine) * (amplitude / 2)		
	if bob == 0.000:
		time = 0.0
	
	return (Vector3.UP * bob) * intensity
