#pragma once

#include "AnimatronicMoveMap.generated.h"

/// Maps bit values to bone movements.
/// Usually indexed by the bit value
USTRUCT(BlueprintType)
struct DATA_API FAnimatronicBoneMapping {
	GENERATED_BODY()

	inline FAnimatronicBoneMapping() {
		BoneAxis = EBoneAxis::BA_X;
		BoneName = "";
	}

	/// The axis the bone should move on
	UPROPERTY(BlueprintReadWrite)
	TEnumAsByte<EBoneAxis> BoneAxis;

	/// The name of the bone
	UPROPERTY(BlueprintReadWrite)
	FName BoneName;
};
