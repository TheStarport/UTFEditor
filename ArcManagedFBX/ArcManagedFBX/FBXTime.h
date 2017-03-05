#pragma once
#ifndef FBXTime_h__
#define FBXTime_h__

#include <fbxsdk\core\base\fbxtime.h>
#include "Defines.h"
#include "FBXTypes.h"

using namespace System::Runtime;
using namespace System::Runtime::InteropServices;

namespace ArcManagedFBX
{
	public value struct FBXTime
	{
	public:
		static FBXTime Infinite() { return FBXTime(FBXSDK_TC_INFINITY); }

		FBXTime(const fbxsdk_2015_1::FbxLongLong time)
			: mTime(time)
		{ }

		operator fbxsdk_2015_1::FbxTime()
		{
			return fbxsdk_2015_1::FbxTime(mTime);
		}
	private:
		fbxsdk_2015_1::FbxLongLong mTime;
	};
}
#endif // FBXTime_h__