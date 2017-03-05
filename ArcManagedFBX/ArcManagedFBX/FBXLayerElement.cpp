#include "Stdafx.h"
#include "FBXLayerElement.h"

namespace ArcManagedFBX
{
	ARC_DEFAULT_CONSTRUCTORS_IMPL(FBXLayerElement)

	ArcManagedFBX::FBXLayerElement::FBXLayerElement(fbxsdk_2015_1::FbxLayerElement* instance)
		: mInstance(instance)
	{
	}

	EMappingMode FBXLayerElement::GetMappingMode()
	{
		return (EMappingMode)mInstance->GetMappingMode();
	}

	EReferenceMode FBXLayerElement::GetReferenceMode()
	{
		return EReferenceMode(mInstance->GetReferenceMode());
	}

	ARC_DEFAULT_CONSTRUCTORS_IMPL(FBXLayerElementVector4)

	ArcManagedFBX::FBXLayerElementVector4::FBXLayerElementVector4(fbxsdk_2015_1::FbxLayerElementTemplate<FbxVector4>* instance)
		: FBXLayerElement(instance), mInstance(instance)
	{
	}

}