#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "HandheldCameraEffect.generated.h"


UCLASS(ClassGroup=(Custom), meta=(BlueprintSpawnableComponent))
class COMPONENTS_API UHandheldCameraEffect : public UActorComponent
{
	GENERATED_BODY()

public:
	// Sets default values for this component's properties
	UHandheldCameraEffect();

	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = "Effect")
	bool bEnabled = false;

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void TickComponent(float deltaTime, ELevelTick tickType,
	                           FActorComponentTickFunction* thisTickFunction) override;
};
