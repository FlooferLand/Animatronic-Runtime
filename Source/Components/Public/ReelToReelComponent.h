#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "ReelToReelComponent.generated.h"


// Reads data from a tape into real-time bit values and manages playback
UCLASS(ClassGroup=(Custom), meta=(BlueprintSpawnableComponent))
class COMPONENTS_API UReelToReelComponent : public UActorComponent {
	GENERATED_BODY()

public:
	// Sets default values for this component's properties
	UReelToReelComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void TickComponent(float deltaTime, ELevelTick tickType,
	                           FActorComponentTickFunction* thisTickFunction) override;
};
