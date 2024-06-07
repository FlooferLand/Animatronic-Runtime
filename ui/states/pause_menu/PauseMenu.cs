namespace Project;
using Godot;

public partial class PauseMenu : PanelContainer {
	#region State (show, hide, etc)
	public void PopUp() {
		Show();
	}
	public void PopDown() {
		Hide();
	}
	public void Toggle() {
		if (Visible)  PopDown();
		if (!Visible) PopUp();
	}
	#endregion

	public override void _Ready() {
		Hide();
	}
}
