#pragma once

#include <fbxsdk/scene/geometry/fbxlayer.h>
#include "Defines.h"
#include "FBXTypes.h"
#include "FBXLayerElementArray.h"

using namespace ArcManagedFBX::Types;

namespace ArcManagedFBX
{
	public ref class FBXLayerElementVector4
	{
	public:
		ARC_DEFAULT_CONSTRUCTORS(FBXLayerElementVector4)
			FBXLayerElementVector4(fbxsdk_2015_1::FbxLayerElementTemplate<FbxVector4>* instance);

		FBXLayerElementArrayVector4^ GetDirectArray()
		{
			return gcnew FBXLayerElementArrayVector4(mInstance->GetDirectArray());
		}

		FBXLayerElementArrayInt^ GetIndexArray()
		{
			return gcnew FBXLayerElementArrayInt(mInstance->GetIndexArray());
		}

	private:
		fbxsdk_2015_1::FbxLayerElementTemplate<FbxVector4>* mInstance;
	};
}