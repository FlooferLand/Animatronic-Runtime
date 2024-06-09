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
    [GetNode($"{{{nameof(displayTab)}}}/Resolution")]   private SettingsTwoNumWidget resolutionWidget;
    [GetNode($"{{{nameof(displayTab)}}}/FpsCap")]       private SettingsNumberWidget fpsCapWidget;
    [GetNode($"{{{nameof(displayTab)}}}/VSync")]        private SettingsEnumWidget vsyncWidget;
    
    [GetNode($"{{{nameof(graphicsTab)}}}/LodThreshold")] private SettingsNumberWidget lodThresholdWidget;
    #endregion

    // TODO: Switch the settings to be made at compile-time only (a pain to do because Godot's Tool system sucks)
    
    // TODO: Switch the settings to be completely made through code, because Godot is shit and keeps resetting my nodes
    
    public override void _Ready() {
        // V-Sync
        vsyncWidget.Init<DisplayServer.VSyncMode>(value => {
            EngineSettings.Data.Display.VSync = value;
        });
        vsyncWidget.SetValueEnum(EngineSettings.Data.Display.VSync);
        
        // FPS cap
        fpsCapWidget.Value = EngineSettings.Data.Display.FramerateCap;  // TODO: Remove this!! temporary patch
        fpsCapWidget.ValueChanged += (value, isMin, isMax) => {
            int val;
            if (isMin || isMax) {
                val = 0;
                // ReSharper disable once StringLiteralTypo
                fpsCapWidget.StringValueOverride = "MAXIMUM FPS WOAAAOOOWW";
                Engine.SetMaxFps(0);
            } else {
                val = (int) value;
                fpsCapWidget.StringValueOverride = null;
                Engine.SetMaxFps(val);
            }
            EngineSettings.Data.Display.FramerateCap = val;
        };
        
        // LoD threshold
        lodThresholdWidget.Value = EngineSettings.Data.Graphics.LodThreshold;  // TODO: Remove this!! temporary patch
        lodThresholdWidget.ValueChanged += (value, _, _) => {
            EngineSettings.Data.Graphics.LodThreshold = value;
            GetTree().Root.MeshLodThreshold = value;
        };
    }
}