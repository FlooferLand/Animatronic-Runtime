#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "CoreAnimatronic.generated.h"

UCLASS()
class ANIMATRONIC_API ACoreAnimatronic : public AActor {
	GENERATED_BODY()

public:
	// Sets default values for this actor's properties
	ACoreAnimatronic();

	UPROPERTY(VisibleDefaultsOnly)
	USkeletalMeshComponent* Mesh;
protected:
	virtual void BeginPlay() override;
	virtual void Tick(float deltaTime) override;
	virtual void Destroyed() override;
};
