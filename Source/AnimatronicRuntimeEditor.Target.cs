using UnrealBuildTool;
using System.Collections.Generic;

public class AnimatronicRuntimeEditorTarget : TargetRules
{
	public AnimatronicRuntimeEditorTarget( TargetInfo Target) : base(Target)
	{
		Type = TargetType.Editor;
		DefaultBuildSettings = BuildSettingsVersion.V5;
		IncludeOrderVersion = EngineIncludeOrderVersion.Unreal5_4;
		ExtraModuleNames.Add("AnimatronicRuntime");
		RegisterModulesCreatedByRider();
	}

	private void RegisterModulesCreatedByRider()
	{
		ExtraModuleNames.AddRange(new[] {
			"Player", "Utility", "Components"
		});
	}
}
