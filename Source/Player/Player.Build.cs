using UnrealBuildTool;

public class Player : ModuleRules
{
    public Player(ReadOnlyTargetRules target) : base(target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(
            new[]
            {
                "Core",
                "EnhancedInput",
                "Engine"
            }
        );

        PrivateDependencyModuleNames.AddRange(
            new[]
            {
                "CoreUObject",
                "Engine",
                "Slate",
                "SlateCore"
            }
        );
    }
}