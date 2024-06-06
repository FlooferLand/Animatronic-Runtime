#include "HandheldCameraEffect.h"


// Sets default values for this component's properties
UHandheldCameraEffect::UHandheldCameraEffect() {
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UHandheldCameraEffect::BeginPlay() {
	Super::BeginPlay();
}


// Called every frame
void UHandheldCameraEffect::TickComponent(float deltaTime, ELevelTick tickType,
                                          FActorComponentTickFunction* thisTickFunction)
{
	Super::TickComponent(deltaTime, tickType, thisTickFunction);
}

void UHandheldCameraEffect::DestroyComponent(bool bPromoteChildren) {
	Super::DestroyComponent(bPromoteChildren);
}

