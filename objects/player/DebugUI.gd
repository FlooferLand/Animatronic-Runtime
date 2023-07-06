extends Control

@onready var framerate := $Framerate

func _process(_delta):
	if Input.is_action_just_pressed("developer_toggle"):
		visible = not visible
	if visible:
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_DISABLED)
	else:
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ENABLED)
	framerate.text = str(Engine.get_frames_per_second()) + " FPS"
