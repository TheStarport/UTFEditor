#include "Stdafx.h"
#include "FBXLayerElement.h"

namespace ArcManagedFBX
{
	ARC_DEFAULT_CONSTRUCTORS_IMPL(FBXLayerElementVector4)

	ArcManagedFBX::FBXLayerElementVector4::FBXLayerElementVector4(fbxsdk_2015_1::FbxLayerElementTemplate<FbxVector4>* instance)
		: mInstance(instance)
	{
	}

}