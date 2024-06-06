#pragma once

#include "CoreMinimal.h"
#include "Components/SceneComponent.h"
#include "Components/SpotLightComponent.h"
#include "FlashlightComponent.generated.h"


UCLASS(ClassGroup=(Custom), meta=(BlueprintSpawnableComponent))
class COMPONENTS_API UFlashlightComponent : public USceneComponent {
	GENERATED_BODY()

public:
	// Sets default values for this component's properties
	UFlashlightComponent();

	UPROPERTY(BlueprintReadOnly)
	bool bEnabled;

	UPROPERTY(EditDefaultsOnly, BlueprintInternalUseOnly)
	float IntensityMultiplier;

	UPROPERTY(EditDefaultsOnly, Category="Flashlight")
	USoundBase* ToggleSound = nullptr;
	
	UFUNCTION(BlueprintCallable, Category="Flashlight")
	void TurnOn();

	UFUNCTION(BlueprintCallable, Category="Flashlight")
	void TurnOff();

	UFUNCTION(BlueprintCallable, Category="Flashlight")
	void ToggleFlashlight();
protected:
	virtual void OnComponentCreated() override;
	virtual void OnComponentDestroyed(bool bDestroyingHierarchy) override;

	// Components
	UPROPERTY()	USpotLightComponent* SpotLight;
private:
	void UpdateSpotlightIntensity();
};
