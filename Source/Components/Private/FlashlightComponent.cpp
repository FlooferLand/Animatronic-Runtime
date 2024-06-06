#include "FlashlightComponent.h"
#include "Components/AudioComponent.h"
#include "Kismet/GameplayStatics.h"


// Sets default values for this component's properties
UFlashlightComponent::UFlashlightComponent() {
	PrimaryComponentTick.bCanEverTick = false;
	bEnabled = false;
	IntensityMultiplier = 1.0f;
	
	// Light component setup
	SpotLight = CreateDefaultSubobject<USpotLightComponent>(TEXT("SpotLight"));
	SpotLight->SetupAttachment(this);
	SpotLight->SetVisibility(false);
}

void UFlashlightComponent::TurnOn() {
	if (ToggleSound) {
		UGameplayStatics::PlaySound2D(this, ToggleSound);
	}
	SpotLight->SetVisibility(true);
	bEnabled = true;
}
void UFlashlightComponent::TurnOff() {
	if (ToggleSound) {
		UGameplayStatics::PlaySound2D(this, ToggleSound);
	}
	SpotLight->SetVisibility(false);
	bEnabled = false;
}
void UFlashlightComponent::ToggleFlashlight() {
	if (bEnabled) {
		TurnOff();
	} else {
		TurnOn();
	}
}

void UFlashlightComponent::OnComponentCreated() {
	Super::OnComponentCreated();
	UpdateSpotlightIntensity();
}

void UFlashlightComponent::OnComponentDestroyed(bool bDestroyingHierarchy) {
	Super::OnComponentDestroyed(bDestroyingHierarchy);

	if (SpotLight) {
		SpotLight->DestroyComponent();
	}
}

void UFlashlightComponent::UpdateSpotlightIntensity() {
	if (SpotLight) {
		SpotLight->Intensity = 50000.0 * IntensityMultiplier;
	}
}

