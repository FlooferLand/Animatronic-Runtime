#include "CoreAnimatronic.h"


// Sets default values
ACoreAnimatronic::ACoreAnimatronic() {
	PrimaryActorTick.bCanEverTick = true;
	
	Mesh = CreateDefaultSubobject<USkeletalMeshComponent>(TEXT("Mesh"));
	Mesh->SetupAttachment(RootComponent);
}

// Called when the game starts or when spawned
void ACoreAnimatronic::BeginPlay() {
	Super::BeginPlay();
}

// Called every frame
void ACoreAnimatronic::Tick(float deltaTime) {
	Super::Tick(deltaTime);
}

void ACoreAnimatronic::Destroyed() {
	Super::Destroyed();
	if (Mesh) {
		Mesh->DestroyComponent();
	}
}

