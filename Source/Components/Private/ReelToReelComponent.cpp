#include "ReelToReelComponent.h"


// Sets default values for this component's properties
UReelToReelComponent::UReelToReelComponent() {
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UReelToReelComponent::BeginPlay() {
	Super::BeginPlay();
}


// Called every frame
void UReelToReelComponent::TickComponent(float deltaTime, ELevelTick tickType,
                                         FActorComponentTickFunction* thisTickFunction) {
	Super::TickComponent(deltaTime, tickType, thisTickFunction);
}

