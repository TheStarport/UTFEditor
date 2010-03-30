using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UTFEditor
{
    class SurFile
    {
        //SUR File Format
        //Compiled By Lancer Solurus with Adoxa's help
        //All values in HEX unless otherwise noted (Size in decimal)
        //*****************************************
        //Size(d) Type Comment

        //** Header
        //4 string 'vers' - Version
        //4 float 2.0

        //** Section header
        //4 ULONG CRC
        //4 ULONG Surface Type Count (!fxd, exts, surf & hpid)
        //4 string '!fxd' - Part header
        //4 string 'exts' - Tag
        //12 XYZ Box minimum
        //12 XYZ Box maximum

        //** Surface tag
        //4 string 'surf' - Surface
        //** Surface header 0x00000034
        //4 ULONG Surface offset, bytes to next HPID or section (this addr + offset)
        //** Surface base pointer 0x00000038
        //12 XYZ Center
        //12 XYZ Inertial damping
        //4 float Radius for visibility (set this large enough to surround entire object to keep it from 'popping', 1.5*radius is good)
        //1 byte Flag or Scaler?
        //3 byte Duplicate of surface offset above
        //4 ULONG Bits section offset (surface header start addr + offset)
        //4 ULONG 0 unused
        //4 ULONG 0 unused
        //4 ULONG 0 unused

        //** Face list header (size = 16*face lists)
        //4 ULONG Vertex list offset (this addr + offset)
        //4 ULONG Cmpnd/HP CRC or bits offset if 'last face group flag' is set
        //1 byte Type 4-normal face, 5-face linkage (final face list entry)
        //3 byte Referenced vertex count, unused by FL, simply set to 0
        //4 ULONG Face count

        //** Face list (size = 16*Face count)
        //12 bits Face number nn xn xx xx (4 bytes) max 16,384 faces
        //12 bits Opposite face xx nx nn xx
        //1 byte Flag xx xx xx nn
        //2 WORD Vertex index 1 nn nn xx xx
        //15 bits Opposite offset xx xx nn nn (bits 0-14)
        //1 bit Last Face Group Flag xx xx xx nx (bit 15) (only set if more than 1 face group)
        //2 WORD Vertex index 2
        //15 bits Opposite offset
        //1 bit Last Face Group Flag
        //2 WORD Vertex index 3
        //15 bits Opposite offset
        //1 bit Last Face Group Flag

        //** Vertex list (size = 16*Vertex count)
        //12 XYZ Vertex position
        //4 ULONG Cmpnd/HP CRC

        //** Bits section (as in 24 bit bitmap)
        //4 ULONG Sibling offset {Bits base addr + offset)
        //4 ULONG Triangle offset (this bits base addr - face base offset (0-fbo)) (if sibling offset is not 0, this is 0)
        //12 XYZ Center
        //4 float Radius
        //4 ULONG Scale (XX YY ZZ xx) - xx is always 00 since it's unused

        //** HpID section - hardpoint IDs
        //4 ULONG Count
        //4 ULONG Hardpoint CRC list (size = 4*Count)
    

        // SUR File Structs compiled by Twex, Phantom Fox, Free Spirit, Dr Del, CCCP, shsan and Skyshooter
        // From FLModelTool and probably originally from Colin Sanby's SUR exporter/importer.
        public class SurTriangleGroupHeader
        {
            public int OffsetToVertex;  // (number of bytes to first Vertex Section, including this header)
            public UInt32 TriangleID;   // (the reference id used assosiate triangles with vertices)
            public byte Type;           // ('4' = D3DPT_TRIANGLELIST or '5' = D3DPT_TRIANGLESTRIP)
            public UInt16 NumRefVerts;  // number of referenced verticies ?
            public UInt32 NumTriangles; // (number of triangle sections in group (note indexed from 0 to N-1))
        };

        public class SurTriangle
        {
            public UInt32 TriID;
            public UInt16  Vertex1;     // (index of the first vertex in the triangle (vertexes are incrementally numbered from the start of the vertex section)
            public UInt16  Vertex2;     // (index of the second vertex)
            public UInt16  Vertex3;     // (index of the third Vertex)
        };

        List<SurTriangle> Triangles = new List<SurTriangle>();

        public class SurVertex
        {
            public float X;             // (coordinate on the X Axe)
            public float Y;             // (coordinate on the Y Axe)
            public float Z;             // (coordinate on the Z Axe)
            public UInt32 TriID;        // (reference to the Triangle group Header Section (the TriangleID)
        };

        List<SurVertex> Vertices = new List<SurVertex>();

        public string FilePath;

        /// <summary>
        /// WARNING THIS IS NOT FINISHED AND IS DEFINITELY BROKEN
        /// </summary>
        /// <param name="filePath"></param>
        public SurFile(string filePath)
        {
            this.FilePath = filePath;
            byte[] data;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                fs.Close();
            }

            int pos = 0;

            // Read header.
            UInt32 Tag = BitConverter.ToUInt32(data, pos); pos += 4;
            float Version = BitConverter.ToSingle(data, pos); pos += 4;
            if (Tag != 0x73726576 || Version != 2.0f)
            {
                throw new Exception("Unsupported sur header.");
            }

            while (pos < data.Length)
            {
                // ID Header
                UInt32 CRC = BitConverter.ToUInt32(data, pos); pos += 4;
                UInt32 Type = BitConverter.ToUInt32(data, pos); pos += 4;

                // If the next bytes equal the "!fxd" then skip over this.
                // This header exists only when Type is greater than 2
                if (Utilities.GetString(data, pos, 4) == "!fxd")
                {
                     pos += 4;
                }

                // Read the bounding box
                if (Utilities.GetString(data, pos, 4) == "exts")
                {
                    pos += 4;
                    float BoundingBoxMinX = BitConverter.ToSingle(data, pos); pos += 4;
                    float BoundingBoxMinY = BitConverter.ToSingle(data, pos); pos += 4;
                    float BoundingBoxMinZ = BitConverter.ToSingle(data, pos); pos += 4;
                    float BoundingBoxMaxX = BitConverter.ToSingle(data, pos); pos += 4;
                    float BoundingBoxMaxY = BitConverter.ToSingle(data, pos); pos += 4;
                    float BoundingBoxMaxZ = BitConverter.ToSingle(data, pos); pos += 4;
                }

                // Read the surface header 
                if (Utilities.GetString(data, pos, 4) == "surf")
                {
                    uint SurfaceTag = BitConverter.ToUInt32(data, pos); pos += 4;
                    int OffsetToNextComponent = BitConverter.ToInt32(data, pos); pos += 4; OffsetToNextComponent += pos;

                    // Read the surface details
                    float CenterX = BitConverter.ToSingle(data, pos); pos += 4;
                    float CenterY = BitConverter.ToSingle(data, pos); pos += 4;
                    float CenterZ = BitConverter.ToSingle(data, pos); pos += 4;
                    float InertiaMomentX = BitConverter.ToSingle(data, pos); pos += 4;
                    float InertiaMomentY = BitConverter.ToSingle(data, pos); pos += 4;
                    float InertiaMomentZ = BitConverter.ToSingle(data, pos); pos += 4;
                    float Radius = BitConverter.ToSingle(data, pos); pos += 4;
                    ushort Unknown = BitConverter.ToUInt16(data, pos); pos += 2;
                    ushort NumTriangles = BitConverter.ToUInt16(data, pos); pos += 2;
                    int OffsetToBits = BitConverter.ToInt32(data, pos); pos += 4; OffsetToBits += pos;
                    pos += 12;

                    // Triangle data.
                    int vertBufStart = data.Length;
                    while (pos < vertBufStart)
                    {
                        // Read the triangle group header. We ignore data that we don't need.
                        SurTriangleGroupHeader hdrTri = new SurTriangleGroupHeader();
                        hdrTri.OffsetToVertex = BitConverter.ToInt32(data, pos); pos += 4;
                        hdrTri.TriangleID = BitConverter.ToUInt32(data, pos); pos += 4;
                        hdrTri.Type = (byte)(data[pos] >> 4);
                        hdrTri.NumRefVerts = (ushort)(BitConverter.ToUInt16(data, pos) & 0x0FFF); pos += 2;
                        hdrTri.NumTriangles = BitConverter.ToUInt16(data, pos); pos += 2;

                        vertBufStart = pos + (int)hdrTri.OffsetToVertex;

                        for (int i = 0; i < hdrTri.NumTriangles; i++)
                        {
                            // Read a triangle. Note a number of fields are skipped over.
                            SurTriangle tri = new SurTriangle();
                            tri.TriID = (uint)(BitConverter.ToInt16(data, pos) >> 4); pos += 4;
                            tri.Vertex1 = BitConverter.ToUInt16(data, pos); pos += 4;
                            tri.Vertex2 = BitConverter.ToUInt16(data, pos); pos += 4;
                            tri.Vertex3 = BitConverter.ToUInt16(data, pos); pos += 4;
                            Triangles.Add(tri);
                        }
                    }

                    // Vertices
                    while (pos < OffsetToBits)
                    {
                        SurVertex vertex = new SurVertex();
                        vertex.X = BitConverter.ToSingle(data, pos); pos += 4;
                        vertex.Y = BitConverter.ToSingle(data, pos); pos += 4;
                        vertex.Z = BitConverter.ToSingle(data, pos); pos += 4;
                        vertex.TriID = BitConverter.ToUInt32(data, pos); pos += 4;
                        Vertices.Add(vertex);
                    }

                    // bits section
                    while (pos < OffsetToNextComponent)
                    {
                        pos++;
                    }
                }
            }

            foreach (SurTriangle tri in Triangles)
            {
                Console.WriteLine("{0} {1} {2} {3}", tri.TriID, tri.Vertex1, tri.Vertex1, tri.Vertex3);
            }

            foreach (SurVertex vert in Vertices)
            {
                Console.WriteLine("{0} {1} {2} {3}", vert.TriID, vert.X, vert.Y, vert.Z);
            }
        }

        public void RenderSur(Microsoft.DirectX.Direct3D.Device device)
        {
            device.RenderState.CullMode = Microsoft.DirectX.Direct3D.Cull.None;
            device.RenderState.FillMode = Microsoft.DirectX.Direct3D.FillMode.WireFrame;

            List<Microsoft.DirectX.Direct3D.CustomVertex.PositionOnly> tmpVertices
                = new List<Microsoft.DirectX.Direct3D.CustomVertex.PositionOnly>();
            foreach (SurVertex vert in Vertices)
            {
                tmpVertices.Add(new Microsoft.DirectX.Direct3D.CustomVertex.PositionOnly(vert.X,vert.Y,vert.Z));
            }

            List<int> tmpTri = new List<int>();
            foreach (SurTriangle tri in Triangles)
            {
                tmpTri.Add(tri.Vertex1);
                tmpTri.Add(tri.Vertex2);
                tmpTri.Add(tri.Vertex3);
            }

            device.DrawIndexedUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleList,
                0, tmpVertices.Count, tmpTri.Count, tmpTri.ToArray(), false, tmpVertices.ToArray());
        }
    }
}
