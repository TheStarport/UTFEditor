#include "Stdafx.h"
#include "FBXStatus.h"
#include "FBXGeometryBase.h"

using namespace ArcManagedFBX;
using namespace ArcManagedFBX::Types;
using namespace ArcManagedFBX::Utility;
using namespace ArcManagedFBX::Exceptions;

ARC_DEFAULT_CONSTRUCTORS_IMPL(FBXGeometryBase)

FBXGeometryBase::FBXGeometryBase(FbxGeometryBase* instance)
{
	this->m_NativeObject = instance;
}

FBXVector FBXGeometryBase::BBoxMax::get()
{
	return FBXVector(this->GetFBXGeometryBase()->BBoxMax.Get());
}

FBXVector FBXGeometryBase::BBoxMin::get()
{
	return FBXVector(this->GetFBXGeometryBase()->BBoxMin.Get());
}

bool FBXGeometryBase::PrimaryVisibility::get()
{
	return this->GetFBXGeometryBase()->PrimaryVisibility.Get();
}

bool FBXGeometryBase::ReceiveShadow::get()
{
	ARC_CHECK_AND_THROW(this->GetFBXGeometryBase() == nullptr, "")

	return this->GetFBXGeometryBase()->ReceiveShadow.Get();
}

bool FBXGeometryBase::CastShadow::get()
{
	ARC_CHECK_AND_THROW(this->GetFBXGeometryBase() == nullptr, "")

	return this->GetFBXGeometryBase()->CastShadow.Get();
}

void ArcManagedFBX::FBXGeometryBase::InitBinormals(int32 count, const int32 layerindex, String^ name)
{
	ARC_CHECK_AND_THROW(this->GetFBXGeometryBase() == nullptr, "")

	const char* nativeName = StringHelper::ToNative(name);
	this->GetFBXGeometryBase()->InitBinormals(count,layerindex,nativeName);
}

void ArcManagedFBX::FBXGeometryBase::InitTangents(FBXGeometryBase^ source, const int32 layerindex)
{
	ARC_CHECK_AND_THROW(this->GetFBXGeometryBase() == nullptr, "")

	this->GetFBXGeometryBase()->InitTangents(source->GetFBXGeometryBase(),layerindex);
}

void ArcManagedFBX::FBXGeometryBase::InitTangents(int32 count, int32 layerindex, String^ name)
{
	ARC_CHECK_AND_THROW(this->GetFBXGeometryBase() == nullptr, "")

	this->GetFBXGeometryBase()->InitTangents(count,layerindex,StringHelper::ToNative(name));
}

void ArcManagedFBX::FBXGeometryBase::InitNormals(int32 count)
{
	this->GetFBXGeometryBase()->InitNormals(count);
}

void ArcManagedFBX::FBXGeometryBase::InitNormals(FBXGeometryBase^ source)
{
	this->GetFBXGeometryBase()->InitNormals(source->GetFBXGeometryBase());
}

ArcManagedFBX::FBXVector ArcManagedFBX::FBXGeometryBase::GetControlPointAt(int32 index)
{
	return FBXVector(this->GetFBXGeometryBase()->GetControlPointAt(index));
}

int32 ArcManagedFBX::FBXGeometryBase::GetElementPolygonGroupCount()
{
	ARC_CHECK_AND_THROW(this->GetFBXGeometryBase() == nullptr,"The element polygon group count is null or empty");

	return this->GetFBXGeometryBase()->GetElementPolygonGroupCount();
}

int32 ArcManagedFBX::FBXGeometryBase::GetControlPointsCount()
{
	return this->GetFBXGeometryBase()->GetControlPointsCount();
}

FBXLayerElementNormal^ ArcManagedFBX::FBXGeometryBase::GetElementNormal()
{
	return gcnew FBXLayerElementNormal(this->GetFBXGeometryBase()->GetElementNormal());
}

int ArcManagedFBX::FBXGeometryBase::GetElementNormalCount()
{
	return this->GetFBXGeometryBase()->GetElementNormalCount();
}

FBXLayerElementTangent^ ArcManagedFBX::FBXGeometryBase::GetElementTangent()
{
	return gcnew FBXLayerElementTangent(this->GetFBXGeometryBase()->GetElementTangent());
}

int ArcManagedFBX::FBXGeometryBase::GetElementTangentCount()
{
	return this->GetFBXGeometryBase()->GetElementTangentCount();
}

FBXLayerElementBinormal^ ArcManagedFBX::FBXGeometryBase::GetElementBinormal()
{
	return gcnew FBXLayerElementBinormal(this->GetFBXGeometryBase()->GetElementBinormal());
}

int ArcManagedFBX::FBXGeometryBase::GetElementBinormalCount()
{
	return this->GetFBXGeometryBase()->GetElementBinormalCount();
}

void ArcManagedFBX::FBXGeometryBase::ComputeBBox()
{
	this->GetFBXGeometryBase()->ComputeBBox();
}

array<FBXVector>^ ArcManagedFBX::FBXGeometryBase::GetControlPoints(FBXStatus^ status)
{
	// Retrieve the total count of control points that exist.
	int32 controlPointsCount  = this->GetFBXGeometryBase()->GetControlPointsCount();
	array<FBXVector>^ outputVectors = gcnew array<FBXVector>(controlPointsCount);

	// Retrieve the control points
	FbxVector4* controlPointsHandle = 0;

	if (status != nullptr)
		controlPointsHandle = this->GetFBXGeometryBase()->GetControlPoints(status->GetInstance());
	else
		controlPointsHandle = this->GetFBXGeometryBase()->GetControlPoints(0);

	// Iterate over the control points and store them into the array
	for(uint32 i = 0; i < (uint32)controlPointsCount; i++)
		outputVectors[i] = *FBXVector::ConvertVector4(&controlPointsHandle[i]);

	return outputVectors;
}

void ArcManagedFBX::FBXGeometryBase::InitControlPoints(int32 count)
{
	this->GetFBXGeometryBase()->InitControlPoints(count);
}

void ArcManagedFBX::FBXGeometryBase::SetControlPointAt(FBXVector^ controlPoint, int32 index)
{
	FbxVector4 generatedVector = controlPoint->GenerateVector4();
	this->GetFBXGeometryBase()->SetControlPointAt(generatedVector,index);
}

void ArcManagedFBX::FBXGeometryBase::SetControlPointCount(int32 count)
{
	this->GetFBXGeometryBase()->SetControlPointCount(count);
}
