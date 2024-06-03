#include "ControllerBase.h"
#include "CharacterBase.h"


// Dumb shortcut so code won't get past my screen's horizontal axis
#define BIND_ACTION APlayerControllerBase::BindAction


#pragma region Input handlers
void APlayerControllerBase::HandleMove(const FInputActionValue& value)
{
	// TODO: Might have to multiply by delta?
	const FVector2D movementVector = value.Get<FVector2D>();
	PlayerCharacter->AddMovementInput(PlayerCharacter->GetActorForwardVector(), movementVector.Y);
	PlayerCharacter->AddMovementInput(PlayerCharacter->GetActorRightVector(), movementVector.X);
}

void APlayerControllerBase::HandleLook(const FInputActionValue& value)
{
	// TODO: Might have to multiply by delta?
	const FVector2D lookAxisVector = value.Get<FVector2D>();
	AddYawInput(lookAxisVector.X * MouseSensitivity);
	AddPitchInput(lookAxisVector.Y * MouseSensitivity);
}

#pragma region Crouching
void APlayerControllerBase::HandleCrouch(const FInputActionValue& value)
	{ PlayerCharacter->Crouch(); }
void APlayerControllerBase::HandleUnCrouch(const FInputActionValue& value)
	{ PlayerCharacter->UnCrouch(); }
#pragma endregion

#pragma region Sprinting
void APlayerControllerBase::HandleSprint(const FInputActionValue& value)
{
	CharacterMovement->MaxWalkSpeed = DefaultMaxSpeed * SprintSpeedMultiplier;
	PlayerCharacter->bIsSprinting = true;
}
void APlayerControllerBase::HandleUnSprint(const FInputActionValue& value)
{
	CharacterMovement->MaxWalkSpeed = DefaultMaxSpeed;
	PlayerCharacter->bIsSprinting = false;
}
#pragma endregion

#pragma region Interaction
void APlayerControllerBase::HandleInteract(const FInputActionValue& value)
{
	// TODO: Implement this
}
#pragma endregion

void APlayerControllerBase::HandleEscape(const FInputActionValue& value)
{
	// FGenericPlatformMisc::RequestExit(false);
	// TODO: Pop up pause menu
}

void APlayerControllerBase::HandleFlashlight(const FInputActionValue& value)
{
	
}
#pragma endregion


#pragma region Initialization + deinit
void APlayerControllerBase::OnPossess(APawn* pawn)
{
	Super::OnPossess(pawn);

	// Get a ref to the player pawn
	PlayerCharacter = Cast<APlayerCharacterBase>(pawn);
	checkf(PlayerCharacter, TEXT("APlayerControllerBase-derived classes should only possess APlayerCharacterBase-derived pawns"));
	
#pragma region Input
	// Get a ref to the EnhancedInputComponent
	EnhancedInputComponent = Cast<UEnhancedInputComponent>(InputComponent);
	checkf(EnhancedInputComponent, TEXT("Unable to get a reference to the EnhancedInputComponent"));

	// Getting the local player subsystem
	auto* InputSubsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(GetLocalPlayer());
	checkf(EnhancedInputComponent, TEXT("Unable to get a reference to the UEnhancedInputLocalPlayerSubsystem"));

	// Adding the mapping
	InputSubsystem->AddMappingContext(InputMappingContext, 0);
	
	// Bind the input actions	
	BIND_ACTION(ActionMove, ETriggerEvent::Triggered, &APlayerControllerBase::HandleMove);
	BIND_ACTION(ActionLook, ETriggerEvent::Triggered, &APlayerControllerBase::HandleLook);
	
	BIND_ACTION(ActionCrouch, ETriggerEvent::Triggered, &APlayerControllerBase::HandleCrouch);
	BIND_ACTION(ActionCrouch, ETriggerEvent::Completed, &APlayerControllerBase::HandleUnCrouch);

	BIND_ACTION(ActionSprint, ETriggerEvent::Triggered, &APlayerControllerBase::HandleSprint);
	BIND_ACTION(ActionSprint, ETriggerEvent::Completed, &APlayerControllerBase::HandleUnSprint);

	BIND_ACTION(ActionInteract, ETriggerEvent::Started, &APlayerControllerBase::HandleInteract);
	BIND_ACTION(ActionEscape, ETriggerEvent::Started, &APlayerControllerBase::HandleEscape);
	BIND_ACTION(ActionFlashlight, ETriggerEvent::Started, &APlayerControllerBase::HandleFlashlight);
#pragma endregion

#pragma region General init
	// Getting a ref to the character movement component
	CharacterMovement = PlayerCharacter->GetCharacterMovement();
	checkf(CharacterMovement, TEXT("Could not get the CharacterMovement controller in the player controller"));
	
	// Setting defaults
	DefaultMaxSpeed = CharacterMovement->MaxWalkSpeed;
	PlayerCameraManager->ViewPitchMin = -55;
	PlayerCameraManager->ViewPitchMax = 55;
#pragma endregion
}

void APlayerControllerBase::OnUnPossess()
{
	// Unbinding input
	EnhancedInputComponent->ClearActionBindings();

	// Call the parent method
	Super::OnUnPossess();
}
#pragma endregion


void APlayerControllerBase::Tick(float delta)
{
	Super::Tick(delta);
}


#pragma region Utility
template <class T>
void APlayerControllerBase::BindAction(const UInputAction* action, ETriggerEvent type, T function)
{
	checkf(action, TEXT("A `UInputAction` object reference is null!"));
	EnhancedInputComponent->BindAction(action, type, this, function);
}
#pragma endregion
