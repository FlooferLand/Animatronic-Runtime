#if TOOLS
namespace Project.Editor;
using Godot;

[Tool]
public partial class MainEditorPlugin : EditorPlugin {
	private SettingsEnumWidgetInspectorPlugin settingsEnumWidgetInspectorPlugin = new();
    
	public override void _EnterTree() {
		AddInspectorPlugin(settingsEnumWidgetInspectorPlugin);
	}

	public override void _ExitTree() {
		RemoveInspectorPlugin(settingsEnumWidgetInspectorPlugin);
	}
}
#endif
