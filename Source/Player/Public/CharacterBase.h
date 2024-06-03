#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "GameFramework/SpringArmComponent.h"
#include "CharacterBase.generated.h"

UCLASS(Abstract)
class PLAYER_API APlayerCharacterBase : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	APlayerCharacterBase();
	
	// Getting the head component
	// FIXME: Doesn't show up in Blueprint > Defaults
	UPROPERTY(EditDefaultsOnly, Category="Component reference")
	USpringArmComponent* HeadComp = nullptr;

	// States
	UPROPERTY(BlueprintReadOnly)
	bool bIsSprinting = false;
protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void Tick(float deltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(UInputComponent* playerInputComponent) override;
};
