#pragma once

#include "FBXVector.h"

namespace ArcManagedFBX
{
	public value struct FBXMatrix
	{
	public:
		FBXMatrix(FBXMatrix^ other);

		array<FBXVector>^ mData;

	internal:
		FBXMatrix(fbxsdk_2015_1::FbxMatrix matrix);
	};

}
