extends Button

@onready var square := $"../Square"

func _pressed():
	square.visible = not square.visible

func _unhandled_input(event):
	if event is InputEventMouseButton:
		event = event as InputEventMouseButton;
		if event.button_index == MOUSE_BUTTON_RIGHT:
			square.visible = not square.visible
			position.x -= 25;
