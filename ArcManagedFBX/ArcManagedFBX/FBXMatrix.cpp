#include "stdafx.h"
#include "FBXMatrix.h"

namespace ArcManagedFBX
{
	FBXMatrix::FBXMatrix(FBXMatrix ^ other)
	{
		mData = gcnew array<FBXVector>(4);
		System::Array::Copy(other->mData, mData, other->mData->Length);
	}

	FBXMatrix::FBXMatrix(fbxsdk_2015_1::FbxMatrix matrix)
	{
		mData = gcnew array<FBXVector>(4);
		for (int i = 0; i < 4; i++)
			mData[i] = FBXVector(matrix.mData[i][0], matrix.mData[i][1], matrix.mData[i][2], matrix.mData[i][3]);
	}
}
