#pragma once

#include "CoreMinimal.h"
#include "CharacterBase.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Components/SpotLightComponent.h"
#include "GameFramework/PlayerController.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "ControllerBase.generated.h"


UCLASS(Abstract)
class PLAYER_API APlayerControllerBase : public APlayerController
{
public:
	// New post-UE5 input component
	UPROPERTY()
	UEnhancedInputComponent* EnhancedInputComponent = nullptr;

	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Object refs")
	USpotLightComponent* Flashlight = nullptr;
	
#pragma region Input mappings
	// The Input Action for movement
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionMove = nullptr;
	
	// The Input Action for looking
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionLook = nullptr;
	
	// The Input Action for crouching
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionCrouch = nullptr;
	
	// The Input Action for sprinting
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionSprint = nullptr;
	
	// The Input Action for interacting
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionInteract = nullptr;
	
	// The Input Action for toggling the flashlight
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionFlashlight = nullptr;
	
	// The Input Action for toggling the pause menu
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	UInputAction* ActionEscape = nullptr;

	// The Input Mapping Context
	UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Player Input")
	TObjectPtr<UInputMappingContext> InputMappingContext = nullptr;
#pragma endregion

#pragma region Settings
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category="Player Input")
	double MouseSensitivity = 0.5;
	
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category="Player Input")
	double SprintSpeedMultiplier = 2.0;
#pragma endregion
	
protected:
	// Input action event handlers
	void HandleMove(const FInputActionValue& value);
	void HandleLook(const FInputActionValue& value);
	void HandleCrouch(const FInputActionValue& value);
	void HandleUnCrouch(const FInputActionValue& value);
	void HandleSprint(const FInputActionValue& value);
	void HandleUnSprint(const FInputActionValue& value);
	void HandleInteract(const FInputActionValue& value);
	void HandleEscape(const FInputActionValue& value);
	void HandleFlashlight(const FInputActionValue& value);

	// Built-in
	virtual void OnPossess(APawn* pawn) override;
	virtual void OnUnPossess() override;
	virtual void Tick(float delta) override;

	// Utility
	template<class T>
	void BindAction(const UInputAction* action, ETriggerEvent type, T function);

	// Events (removed)
	// FIXME: Doesn't show up still in blueprints, idk why. "BlueprintCallable" isn't the right one, shouldn't do that
	// UFUNCTION(BlueprintImplementableEvent)
	// void FlashlightStateChanged(bool IsOn);
	
	// Variables
	float DefaultMaxSpeed;
private:
	// Ref to the current controlled pawn
	UPROPERTY()	APlayerCharacterBase* PlayerCharacter = nullptr;

	// Ref to the character movement component
	UPROPERTY() UCharacterMovementComponent* CharacterMovement = nullptr;
	
	GENERATED_BODY()
};
