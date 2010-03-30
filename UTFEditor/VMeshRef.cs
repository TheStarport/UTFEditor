using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTFEditor
{
    // Structures by Mario Brito from FLModelToolby Anton (Xtreme Team Studios).

    public class VMeshRef
    {
        // Header - one per lod for each .3db section of cmp - 60 bytes
        public UInt32 HeaderSize;           // 0x0000003C
        public UInt32 VMeshLibId;              // crc of 3db name
        public UInt16 StartVert;
        public UInt16 NumVert;
        public UInt16 StartRefVert;
        public UInt16 NumRefVert;
        public UInt16 StartMesh;
        public UInt16 NumMeshes;
        public float BoundingBoxMaxX;
        public float BoundingBoxMinX;
        public float BoundingBoxMaxY;
        public float BoundingBoxMinY;
        public float BoundingBoxMaxZ;
        public float BoundingBoxMinZ;
        public float CenterX;
        public float CenterY;
        public float CenterZ;
        public float Radius;

        public VMeshRef(byte[] data)
        {
            int pos = 0;
            HeaderSize = BitConverter.ToUInt32(data, pos); pos += 4;
            VMeshLibId = BitConverter.ToUInt32(data, pos); pos += 4;
            StartVert = BitConverter.ToUInt16(data, pos); pos += 2;
            NumVert = BitConverter.ToUInt16(data, pos); pos += 2;
            StartRefVert = BitConverter.ToUInt16(data, pos); pos += 2;
            NumRefVert = BitConverter.ToUInt16(data, pos); pos += 2;
            StartMesh = BitConverter.ToUInt16(data, pos); pos += 2;
            NumMeshes = BitConverter.ToUInt16(data, pos); pos += 2;
            BoundingBoxMaxX = BitConverter.ToSingle(data, pos); pos += 4;
            BoundingBoxMinX = BitConverter.ToSingle(data, pos); pos += 4;
            BoundingBoxMaxY = BitConverter.ToSingle(data, pos); pos += 4;
            BoundingBoxMinY = BitConverter.ToSingle(data, pos); pos += 4;
            BoundingBoxMaxZ = BitConverter.ToSingle(data, pos); pos += 4;
            BoundingBoxMinZ = BitConverter.ToSingle(data, pos); pos += 4;
            CenterX = BitConverter.ToSingle(data, pos); pos += 4;
            CenterY = BitConverter.ToSingle(data, pos); pos += 4;
            CenterZ = BitConverter.ToSingle(data, pos); pos += 4;
            Radius = BitConverter.ToSingle(data, pos); pos += 4;
        }

        public byte[] GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(HeaderSize));
            data.AddRange(BitConverter.GetBytes(VMeshLibId));
            data.AddRange(BitConverter.GetBytes(StartVert));
            data.AddRange(BitConverter.GetBytes(NumVert));
            data.AddRange(BitConverter.GetBytes(StartRefVert));
            data.AddRange(BitConverter.GetBytes(NumRefVert));
            data.AddRange(BitConverter.GetBytes(StartMesh));
            data.AddRange(BitConverter.GetBytes(NumMeshes));
            data.AddRange(BitConverter.GetBytes(BoundingBoxMaxX));
            data.AddRange(BitConverter.GetBytes(BoundingBoxMinX));
            data.AddRange(BitConverter.GetBytes(BoundingBoxMaxY));
            data.AddRange(BitConverter.GetBytes(BoundingBoxMinY));
            data.AddRange(BitConverter.GetBytes(BoundingBoxMaxZ));
            data.AddRange(BitConverter.GetBytes(BoundingBoxMinZ));
            data.AddRange(BitConverter.GetBytes(CenterX));
            data.AddRange(BitConverter.GetBytes(CenterY));
            data.AddRange(BitConverter.GetBytes(CenterZ));
            data.AddRange(BitConverter.GetBytes(Radius));
            return data.ToArray();
        }
    }
}
