using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    /// <summary>
    /// A VMeshData Decoder
    /// </summary>
    public class VMeshData
    {
        // repeated <no_meshes> times in segment - 12 bytes
        public struct TMeshHeader
        {
            public uint MaterialId;         // crc of texture name for mesh
            public int StartVertex;
            public int EndVertex;
            public int NumRefVertices;
            public int Padding;             // 0x00CC

            public int BaseVertex;
            public int TriangleStart;
        };

        // triangle definition - 6 bytes
        public struct TTriangle
        {
            public int Vertex1;
            public int Vertex2;
            public int Vertex3;
        };

        // vertex definition - 32 bytes
        public struct TVertex
        {
            public uint FVF;
            public float X;
            public float Y;
            public float Z;
            public float NormalX;
            public float NormalY;
            public float NormalZ;
            public uint Diffuse;
            public float S;
            public float T;
            public float U;
            public float V;
        };

        // Data header - 16 bytes long
        public UInt32 MeshType;                 // 0x00000001
        public UInt32 SurfaceType;              // 0x00000004
        public UInt16 NumMeshes;
        public UInt16 NumRefVertices;
        public UInt16 FlexibleVertexFormat;     // 0x0112
        public UInt16 NumVertices;

        /// <summary>
        /// A list of meshes in the mesh data
        /// </summary>
        public List<TMeshHeader> Meshes = new List<TMeshHeader>();

        /// <summary>
        /// A list of triangles in the mesh data
        /// </summary>
        public List<TTriangle> Triangles = new List<TTriangle>();

        /// <summary>
        /// A list of Vertices in the mesh data
        /// </summary>
        public List<TVertex> Vertices = new List<TVertex>();

        public const uint D3DFVF_RESERVED0      = 0x001;
        public const uint D3DFVF_XYZ = 0x002;
        public const uint D3DFVF_XYZRHW = 0x004;
        public const uint D3DFVF_XYZB1 = 0x006;
        public const uint D3DFVF_XYZB2 = 0x008;
        public const uint D3DFVF_XYZB3 = 0x00a;
        public const uint D3DFVF_XYZB4 = 0x00c;
        public const uint D3DFVF_XYZB5 = 0x00e;

        public const uint D3DFVF_NORMAL = 0x010;
        public const uint D3DFVF_RESERVED1 = 0x020;
        public const uint D3DFVF_DIFFUSE = 0x040;
        public const uint D3DFVF_SPECULAR = 0x080;

        public const uint D3DFVF_TEXCOUNT_MASK = 0xf00;
        public const uint D3DFVF_TEX0 = 0x000;
        public const uint D3DFVF_TEX1 = 0x100;
        public const uint D3DFVF_TEX2 = 0x200;
        public const uint D3DFVF_TEX3 = 0x300;
        public const uint D3DFVF_TEX4 = 0x400;
        public const uint D3DFVF_TEX5 = 0x500;
        public const uint D3DFVF_TEX6 = 0x600;
        public const uint D3DFVF_TEX7 = 0x700;
        public const uint D3DFVF_TEX8 = 0x800;

        /// <summary>
        /// Decode the VMeshData
        /// </summary>
        /// <param name="data"></param>
        public VMeshData(byte[] data)
        {
            int pos = 0;

            // Read the data header.
            MeshType = BitConverter.ToUInt32(data, pos); pos += 4;
            SurfaceType = BitConverter.ToUInt32(data, pos); pos += 4;
            NumMeshes = BitConverter.ToUInt16(data, pos); pos += 2;
            NumRefVertices = BitConverter.ToUInt16(data, pos); pos += 2;
            FlexibleVertexFormat = BitConverter.ToUInt16(data, pos); pos += 2;
            NumVertices = BitConverter.ToUInt16(data, pos); pos += 2;

            // The FVF defines what fields are included for each vertex.
            switch (FlexibleVertexFormat)
            {
                case 0x12:
                case 0x102:
                case 0x112:
                case 0x142:
                case 0x152:
                case 0x212:
                case 0x252:
                    break;
                default:
                    throw new Exception(String.Format("FVF 0x{0:X} not supported.", FlexibleVertexFormat));
            }

            // Read the mesh headers.
            int triangleStartOffset = 0;
            int vertexBaseOffset = 0;
            for (int count = 0; count < NumMeshes; count++)
            {
                TMeshHeader item = new TMeshHeader();
                item.MaterialId = BitConverter.ToUInt32(data, pos); pos += 4;
                item.StartVertex = (int)BitConverter.ToUInt16(data, pos); pos += 2;
                item.EndVertex = (int)BitConverter.ToUInt16(data, pos); pos += 2;
                item.NumRefVertices = (int)BitConverter.ToUInt16(data, pos); pos += 2;
                item.Padding = (int)BitConverter.ToUInt16(data, pos); pos += 2;
               
                item.TriangleStart = triangleStartOffset;
                triangleStartOffset += item.NumRefVertices;

                item.BaseVertex = vertexBaseOffset;
                vertexBaseOffset += item.EndVertex - item.StartVertex + 1;

                Meshes.Add(item);
            }

            // Read the triangle data
            int num_triangles = NumRefVertices / 3;
            for (int count = 0; count < num_triangles; count++)
            {
                TTriangle item = new TTriangle();
                item.Vertex1 = BitConverter.ToUInt16(data, pos); pos += 2;
                item.Vertex2 = BitConverter.ToUInt16(data, pos); pos += 2;
                item.Vertex3 = BitConverter.ToUInt16(data, pos); pos += 2;
                Triangles.Add(item);
            }

            // Read the vertex data.
            try
            {
                for (int count = 0; count < NumVertices; count++)
                {                 
                    TVertex item = new TVertex();
                    item.FVF = FlexibleVertexFormat;
                    item.X = BitConverter.ToSingle(data, pos); pos += 4;
                    item.Y = BitConverter.ToSingle(data, pos); pos += 4;
                    item.Z = BitConverter.ToSingle(data, pos); pos += 4;
                    if ((FlexibleVertexFormat & D3DFVF_NORMAL) == D3DFVF_NORMAL)
                    {
                        item.NormalX = BitConverter.ToSingle(data, pos); pos += 4;
                        item.NormalY = BitConverter.ToSingle(data, pos); pos += 4;
                        item.NormalZ = BitConverter.ToSingle(data, pos); pos += 4;
                    }
                    if ((FlexibleVertexFormat & D3DFVF_DIFFUSE) == D3DFVF_DIFFUSE)
                    {
                        item.Diffuse = BitConverter.ToUInt32(data, pos); pos += 4;
                    }
                    if ((FlexibleVertexFormat & D3DFVF_TEX1) == D3DFVF_TEX1)
                    {
                        item.S = BitConverter.ToSingle(data, pos); pos += 4;
                        item.T = BitConverter.ToSingle(data, pos); pos += 4;
                    }
                    if ((FlexibleVertexFormat & D3DFVF_TEX2) == D3DFVF_TEX2)
                    {
                        item.S = BitConverter.ToSingle(data, pos); pos += 4;
                        item.T = BitConverter.ToSingle(data, pos); pos += 4;
                        item.U = BitConverter.ToSingle(data, pos); pos += 4;
                        item.V = BitConverter.ToSingle(data, pos); pos += 4;
                    }
                    Vertices.Add(item);
                }
            }
            catch
            {
                MessageBox.Show("Header has more vertices then data", "Error");
            }
        }
    }
}
