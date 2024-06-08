namespace Project;
using Godot;

[Tool]
public partial class SettingsMenu : Control {
    #region Tabs
    [GetNode("Tabs/Display/List")]       private VBoxContainer displayTab;
    [GetNode("Tabs/Graphics/List")]      private VBoxContainer graphicsTab;
    [GetNode("Tabs/CPU/List")]           private VBoxContainer cpuTab;
    [GetNode("Tabs/Gameplay/List")]      private VBoxContainer gameplayTab;
    #endregion
    
    #region Widgets
    [GetNode($"{{{nameof(displayTab)}}}/Resolution")]
    private SettingsTwoNumWidget resolutionWidget;
    [GetNode($"{{{nameof(displayTab)}}}/VSync")]
    private SettingsEnumWidget vsyncWidget;
    #endregion

    public override void _Ready() {
        if (!Engine.IsEditorHint()) return;
        
        // Inits
        vsyncWidget.Init<DisplayServer.VSyncMode>();
        
        // Setting the default values
        vsyncWidget.SetValueEnum(EngineSettings.Data.Display.VSync);
    }
}