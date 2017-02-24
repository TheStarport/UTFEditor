using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using SharpDX.Direct3D9;

namespace UTFEditor
{
    class SurFile
    {
        public string VersionString;
        public float VersionNumber;
        public string FixedFlag;

        public struct HardpointSection
        {
            public uint MeshIdCount;
            public uint[] MeshIds;
        }
        
        public struct TriangleGroup
        {
            public uint VertexOffset;
            public uint MeshId;
            public byte Type;
            public uint RefVertexCount;
            public short TriangleCount;
            public Triangle[] Triangles;
        }

        public struct SurfaceSection
        {
            public SharpDX.Vector3 Center;
            public SharpDX.Vector3 Inertia;
            public float Radius;
            public float Scale;
            public uint Size;
            public uint BitsSectionOffset;

            public List<TriangleGroup> TriangleGroups;
            public List<Vertex> Vertices;
        }

        public struct Triangle
        {
            public ushort TriangleNumber;
            public ushort TriangleOpp;
            public byte Flag;
            public Index[] Indices;
        }

        public struct Index
        {
            public ushort VertexId;
            public short Offset; // Offset / 16 -> triangle ID      (Offset & 15) >> 2 -> index ID (1, 2 or 3) within triangle
            public byte Flag;
        }

        public struct Vertex
        {
            public float X;
            public float Y;
            public float Z;
            public UInt32 MeshId; // Reference to the Triangle Group Header Section (MeshId)
        };

        public struct Mesh
        {
            public uint MeshId;

            public SharpDX.Vector3[] BoundingBox;
            public List<HardpointSection> HardpointSections;
            public List<SurfaceSection> SurfaceSections;
        }

        List<Mesh> Meshes = new List<Mesh>();

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

            VersionString = Utilities.GetString(data, ref pos, 4);
            VersionNumber = Utilities.GetFloat(data, ref pos);

            if (VersionNumber != 2.0f || VersionString != "vers")
                throw new FormatException();

            while (pos < data.Length)
            {
                Mesh m = new Mesh();
                m.HardpointSections = new List<HardpointSection>();
                m.SurfaceSections = new List<SurfaceSection>();
                m.MeshId = Utilities.GetDWord(data, ref pos);
                uint blockCount = Utilities.GetDWord(data, ref pos);

                while (blockCount-- > 0)
                {
                    string nextType = Utilities.GetString(data, ref pos, 4);
                    if (nextType == "hpid")
                    {
                        HardpointSection hps = new HardpointSection();
                        hps.MeshIdCount = Utilities.GetDWord(data, ref pos);
                        hps.MeshIds = new uint[hps.MeshIdCount];
                        for (int a = 0; a < hps.MeshIdCount; a++)
                            hps.MeshIds[a] = Utilities.GetDWord(data, ref pos);
                        m.HardpointSections.Add(hps);
                    }
                    else if (nextType == "!fxd")
                        FixedFlag = nextType;
                    else if (nextType == "exts")
                    {
                        m.BoundingBox = new SharpDX.Vector3[2];
                        for (int a = 0; a < 2; a++)
                            m.BoundingBox[a] = new SharpDX.Vector3(
                                                        Utilities.GetFloat(data, ref pos),
                                                        Utilities.GetFloat(data, ref pos),
                                                        Utilities.GetFloat(data, ref pos));
                    }
                    else if (nextType == "surf")
                    {
                        SurfaceSection ss = new SurfaceSection();

                        uint size = Utilities.GetDWord(data, ref pos);
                        uint header_pos = (uint)pos;
                        ss.Center = new SharpDX.Vector3(
                                            Utilities.GetFloat(data, ref pos),
                                            Utilities.GetFloat(data, ref pos),
                                            Utilities.GetFloat(data, ref pos));
                        ss.Inertia = new SharpDX.Vector3(
                                            Utilities.GetFloat(data, ref pos),
                                            Utilities.GetFloat(data, ref pos),
                                            Utilities.GetFloat(data, ref pos));
                        ss.Radius = Utilities.GetFloat(data, ref pos);
                        ss.Scale = data[pos] / 250.0f;
                        ss.Size = Utilities.GetDWord(data, ref pos) >> 8;
                        ss.BitsSectionOffset = Utilities.GetDWord(data, ref pos);
                        ss.TriangleGroups = new List<TriangleGroup>();
                        ss.Vertices = new List<Vertex>();

                        uint bits_beg = header_pos + ss.BitsSectionOffset;
                        uint bits_end = header_pos + ss.Size;

                        // padding
                        pos += 3 * sizeof(uint);

                        int vertBufStart = data.Length;
                        while (pos < vertBufStart)
                        {
                            TriangleGroup tgh = new TriangleGroup();

                            int group_pos = pos;
                            tgh.VertexOffset = Utilities.GetDWord(data, ref pos);
                            tgh.MeshId = Utilities.GetDWord(data, ref pos);
                            tgh.Type = data[pos];
                            tgh.RefVertexCount = Utilities.GetDWord(data, ref pos) >> 8;
                            tgh.TriangleCount = Utilities.GetShort(data, ref pos); pos += 2;
                            tgh.Triangles = new Triangle[tgh.TriangleCount];

                            vertBufStart = group_pos + (int)tgh.VertexOffset;

                            for (int a = 0; a < tgh.TriangleCount; a++)
                            {
                                Triangle t = new Triangle();
                                byte[] tempData = new byte[(12 + 12 + 7 + 1 + 3 * (16 + 15 + 1)) / 8];
                                Array.Copy(data, pos, tempData, 0, tempData.Length);
                                pos += tempData.Length;
                                BitArray br = new BitArray(tempData);

                                int bitpos = 0;
                                t.TriangleNumber = (ushort)Advance(br, 12, ref bitpos);
                                t.TriangleOpp = (ushort)Advance(br, 12, ref bitpos);
                                bitpos += 7;
                                t.Flag = (byte)Advance(br, 1, ref bitpos);
                                t.Indices = new Index[3];
                                for (int b = 0; b < 3; b++)
                                {
                                    Index i = new Index();
                                    i.VertexId = (ushort)Advance(br, sizeof(ushort) * 8, ref bitpos);
                                    i.Offset = (short)Advance(br, 15, ref bitpos);
                                    i.Flag = (byte)Advance(br, 1, ref bitpos);
                                    t.Indices[b] = i;
                                }
                                tgh.Triangles[a] = t;
                            }

                            ss.TriangleGroups.Add(tgh);
                        }

                        // Vertices
                        while (pos < bits_beg)
                        {
                            Vertex vertex = new Vertex();
                            vertex.X = Utilities.GetFloat(data, ref pos);
                            vertex.Y = Utilities.GetFloat(data, ref pos);
                            vertex.Z = Utilities.GetFloat(data, ref pos);
                            vertex.MeshId = Utilities.GetDWord(data, ref pos);
                            ss.Vertices.Add(vertex);
                        }

                        m.SurfaceSections.Add(ss);

                        pos = (int)bits_end;
                    }
                    else throw new FormatException();
                }
                
                Meshes.Add(m);
            }
        }

        public void RenderSur(SharpDX.Direct3D9.Device device)
        {
            var colors = new SharpDX.Color[]
            {
                new SharpDX.Color(180, 180, 180, 50),
                new SharpDX.Color(180,148,62, 50),
                new SharpDX.Color(114,124,206, 50),
                new SharpDX.Color(96,168,98, 50),
                new SharpDX.Color(193,92,165, 50),
                new SharpDX.Color(203,90,76, 50)
            };

            device.SetTexture(0, null);
            device.SetTransform(TransformState.World, SharpDX.Matrix.Identity);

            device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.SelectArg2);
            device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.TFactor);
            device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.SelectArg2);
            device.SetTextureStageState(0, TextureStage.AlphaArg2, TextureArgument.TFactor);

            int c = 0;
            foreach (var m in Meshes)
            {
                List<SharpDX.Vector3> tmpVertices = new List<SharpDX.Vector3>();
                List<int> tmpTri = new List<int>();

                foreach (var ss in m.SurfaceSections)
                {
                    foreach (var vert in ss.Vertices)
                    {
                        tmpVertices.Add(new SharpDX.Vector3(vert.X, vert.Y, vert.Z));
                    }

                    foreach (var tg in ss.TriangleGroups)
                    {
                        foreach (var tri in tg.Triangles)
                        {
                            foreach (var i in tri.Indices)
                            {
                                tmpTri.Add(i.VertexId);
                                System.Diagnostics.Debug.Assert(i.VertexId < tmpVertices.Count);
                            }
                        }
                    }
                }

                var color = colors[c % colors.Length];
                device.SetRenderState(RenderState.TextureFactor, color.ToRgba());

                device.DrawIndexedUserPrimitives(SharpDX.Direct3D9.PrimitiveType.TriangleList,
                    0, tmpVertices.Count, tmpTri.Count / 3, tmpTri.ToArray(), SharpDX.Direct3D9.Format.Index32, tmpVertices.ToArray());

                c++;
            }
        }

        private static UInt64 Advance(BitArray br, int length, ref int pos)
        {
            UInt64 output = 0;
            for (int a = length - 1; a >= 0; a--)
            {
                output <<= 1;
                output += (UInt64)(br[pos + a] ? 1 : 0);
            }
            pos += length;
            return output;
        }
    }
}
