#include "stdafx.h"
#include "FBXLayerElementArray.h"

namespace ArcManagedFBX
{
	FBXLayerElementArrayVector4::FBXLayerElementArrayVector4(fbxsdk_2015_1::FbxLayerElementArrayTemplate<FbxVector4>& instance)
		: mInstance(instance)
	{

	}

	FBXLayerElementArrayInt::FBXLayerElementArrayInt(fbxsdk_2015_1::FbxLayerElementArrayTemplate<int>& instance)
		: mInstance(instance)
	{

	}

}