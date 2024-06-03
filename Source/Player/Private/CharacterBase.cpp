#include "CharacterBase.h"

// Sets default values
APlayerCharacterBase::APlayerCharacterBase()
{ 
	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
}

// Called when the game starts or when spawned
void APlayerCharacterBase::BeginPlay()
{
	// Default settings
	// TODO: Doesn't work, should be ran in-editor at compile time
	bUseControllerRotationPitch = false;
	// CharacterMovement->bCanCrouch = true;
	if (HeadComp)
		HeadComp->bUsePawnControlRotation = true;

	// Call parent
	Super::BeginPlay();
}

// Called every frame
void APlayerCharacterBase::Tick(float deltaTime)
{
	Super::Tick(deltaTime);
}

// Called to bind functionality to input
void APlayerCharacterBase::SetupPlayerInputComponent(UInputComponent* playerInputComponent)
{
	Super::SetupPlayerInputComponent(playerInputComponent);
}

