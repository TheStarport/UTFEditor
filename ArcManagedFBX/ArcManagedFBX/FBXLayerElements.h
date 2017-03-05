#pragma once

#include <fbxsdk/scene/geometry/fbxlayer.h>
#include "Defines.h"
#include "FBXTypes.h"
#include "FBXLayerElement.h"

using namespace System::Runtime;
using namespace System::Runtime::InteropServices;

#define ManagedFBXLayerElement_Begin(type) \
public ref class FBXLayerElement##type : public FBXLayerElementVector4 \
{ \
public: \
	FBXLayerElement##type(fbxsdk_2015_1::FbxLayerElement##type* instance) : mInstance(instance), FBXLayerElementVector4(instance) { } \
private: \
	fbxsdk_2015_1::FbxLayerElement##type* mInstance; \
public:

#define ManagedFBXLayerElement_End() };

namespace ArcManagedFBX
{
	ManagedFBXLayerElement_Begin(Normal);
	ManagedFBXLayerElement_End();

	ManagedFBXLayerElement_Begin(Tangent);
	ManagedFBXLayerElement_End();

	ManagedFBXLayerElement_Begin(Binormal);
	ManagedFBXLayerElement_End();
}