using UnrealBuildTool;
using System.Collections.Generic;

public class AnimatronicRuntimeTarget : TargetRules
{
	public AnimatronicRuntimeTarget(TargetInfo Target) : base(Target)
	{
		Type = TargetType.Game;
		DefaultBuildSettings = BuildSettingsVersion.V5;
		IncludeOrderVersion = EngineIncludeOrderVersion.Unreal5_4;
		ExtraModuleNames.Add("PrimaryGameModule");
		RegisterModulesCreatedByRider();
	}

	private void RegisterModulesCreatedByRider()
	{
		ExtraModuleNames.AddRange(new[] {
			"Player", "Utility", "Components", "Animatronic", "Data"
		});
	}
}
