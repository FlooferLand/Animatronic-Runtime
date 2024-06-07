namespace Project;
using Godot;

public partial class PlayerPopupUi : Control {
	#region State (show, hide, etc)
	public void PopUp() {
		Show();
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	public void PopDown() {
		Hide();
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public void Toggle() {
		if (Visible)
			PopDown();
		else
			PopUp();
	}
	#endregion

	public override void _Ready() {
		Hide();
	}
	
}
