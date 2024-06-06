#pragma once

#include "DataTypes.generated.h"

UENUM(BlueprintType)
enum BitDrawer {
	Top,
	Bottom
};

USTRUCT(BlueprintType)
struct DATA_API FAnimatronicBitID {
	GENERATED_BODY()
	
	UPROPERTY(BlueprintReadWrite)
	int Number = -1;
	
	UPROPERTY(BlueprintReadWrite)
	TEnumAsByte<BitDrawer> Drawer = BitDrawer::Top;
};
