#pragma once

#include "Defines.h"
#include "FBXObject.h"
#include "FBXTypes.h"
#include <fbxsdk/scene/shading/fbxsurfacematerial.h>

using namespace System;
using namespace System::Text;

using namespace ArcManagedFBX::Types;

namespace ArcManagedFBX
{
	public ref class FBXSurfaceMaterial : public FBXObject
	{
	public:
		ARC_DEFAULT_CONSTRUCTORS(FBXSurfaceMaterial)

		ARC_CHILD_CAST(NativeObject, fbxsdk_2015_1::FbxSurfaceMaterial, FBXSurfaceMaterial);

	protected:
		ARC_DEFAULT_INTERNAL_CONSTRUCTOR(FBXSurfaceMaterial, fbxsdk_2015_1::FbxSurfaceMaterial)

	};
}