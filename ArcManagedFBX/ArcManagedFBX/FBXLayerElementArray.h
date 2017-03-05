#pragma once

#include <fbxsdk/scene/geometry/fbxlayer.h>
#include "Defines.h"
#include "FBXTypes.h"
#include "FBXVector.h"

using namespace ArcManagedFBX::Types;

namespace ArcManagedFBX
{
	public ref class FBXLayerElementArrayVector4
	{
	public:
		FBXLayerElementArrayVector4(fbxsdk_2015_1::FbxLayerElementArrayTemplate<FbxVector4>& instance);

		FBXVector GetAt(int index) { return FBXVector(mInstance.GetAt(index)); }

	private:
		fbxsdk_2015_1::FbxLayerElementArrayTemplate<FbxVector4>& mInstance;
	};

	public ref class FBXLayerElementArrayInt
	{
	public:
		FBXLayerElementArrayInt(fbxsdk_2015_1::FbxLayerElementArrayTemplate<int>& instance);

		int GetAt(int index) { return mInstance.GetAt(index); }

	private:
		fbxsdk_2015_1::FbxLayerElementArrayTemplate<int>& mInstance;
	};
}