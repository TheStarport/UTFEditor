using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using SharpDX.Direct3D9;
using System.Linq;

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

        public List<Mesh> Meshes { get; private set; } = new List<Mesh>();

        public SharpDX.Vector3 RootCenter => Meshes[0].SurfaceSections[0].Center;

        public void UpdateRootCenter(float x, float y, float z)
        {
            var m = Meshes[0];

            var ss = m.SurfaceSections[0];
            ss.Center = new SharpDX.Vector3(x, y, z);
            m.SurfaceSections[0] = ss;

            Meshes[0] = m;
        }

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
                Mesh m = new Mesh
                {
                    HardpointSections = new List<HardpointSection>(),
                    SurfaceSections = new List<SurfaceSection>(),
                    MeshId = Utilities.GetDWord(data, ref pos)
                };
                uint blockCount = Utilities.GetDWord(data, ref pos);

                while (blockCount-- > 0)
                {
                    string nextType = Utilities.GetString(data, ref pos, 4);
                    if (nextType == "hpid")
                    {
                        HardpointSection hps = new HardpointSection
                        {
                            MeshIdCount = Utilities.GetDWord(data, ref pos)
                        };
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
                            tgh.TriangleCount = Utilities.GetShort(data, ref pos);
                            pos += 2;
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

        public void Save(string filePath = null)
        {
            if (filePath == null)
                filePath = FilePath + ".2.sur";

            int pos = 0;
            byte[] data = new byte[4096];

            Utilities.WriteString(data, VersionString, ref pos);
            Utilities.WriteFloat(data, VersionNumber, ref pos);

            bool first = true;

            foreach(var m in Meshes)
            {
                // Id, block count, !fxd, exts
                int basicSize = 4 + 4 + 4 + 4 + 2 * 3 * 4;
                while (pos + basicSize > data.Length)
                    Array.Resize(ref data, data.Length * 2);

                Utilities.WriteDWord(data, m.MeshId, ref pos);

                int blockCount = m.HardpointSections.Count + (FixedFlag != string.Empty ? 1 : 0) + 1 + m.SurfaceSections.Count;

                Utilities.WriteDWord(data, (uint)blockCount, ref pos);

                if (first && FixedFlag != string.Empty)
                    Utilities.WriteString(data, FixedFlag, ref pos);

                Utilities.WriteString(data, "exts", ref pos);

                foreach(var v in m.BoundingBox)
                {
                    Utilities.WriteFloat(data, v.X, ref pos);
                    Utilities.WriteFloat(data, v.Y, ref pos);
                    Utilities.WriteFloat(data, v.Z, ref pos);
                }

                foreach(var s in m.SurfaceSections)
                {
                    // surf, size, center, inertia, radius, scale, size, bits, padding
                    int ssSize = 4 + 4 + 3 * 4 + 3 * 4 + 4 + 1 + 4 + 4 + 3 * 4;
                    while (pos + ssSize > data.Length)
                        Array.Resize(ref data, data.Length * 2);

                    Utilities.WriteString(data, "surf", ref pos);

                    Utilities.WriteDWord(data, 0, ref pos);

                    int headerPos = pos;

                    Utilities.WriteFloat(data, s.Center.X, ref pos);
                    Utilities.WriteFloat(data, s.Center.Y, ref pos);
                    Utilities.WriteFloat(data, s.Center.Z, ref pos);

                    Utilities.WriteFloat(data, s.Inertia.X, ref pos);
                    Utilities.WriteFloat(data, s.Inertia.Y, ref pos);
                    Utilities.WriteFloat(data, s.Inertia.Z, ref pos);

                    Utilities.WriteFloat(data, s.Radius, ref pos);
                    data[pos] = (byte)(s.Scale * 250.0f);
                    Utilities.WriteDWord(data, s.Size << 8, ref pos);
                    Utilities.WriteDWord(data, s.BitsSectionOffset, ref pos);

                    // padding
                    pos += 3 * sizeof(uint);

                    foreach(var tg in s.TriangleGroups)
                    {
                        int tgSize = 4 + 4 + 1 + 4 + 2 + 2;
                        while (pos + tgSize > data.Length)
                            Array.Resize(ref data, data.Length * 2);

                        int groupPos = pos;

                        Utilities.WriteDWord(data, tg.VertexOffset, ref pos);
                        Utilities.WriteDWord(data, tg.MeshId, ref pos);
                        data[pos] = tg.Type;
                        Utilities.WriteDWord(data, tg.RefVertexCount << 8, ref pos);
                        Utilities.WriteShort(data, tg.TriangleCount, ref pos);
                        pos += 2;

                        foreach(var t in tg.Triangles)
                        {
                            int tSize = (12 + 12 + 7 + 1 + 3 * (16 + 15 + 1)) / 8;
                            while (pos + tSize > data.Length)
                                Array.Resize(ref data, data.Length * 2);

                            BitArray br = new BitArray(tSize * 8);
                            int bitpos = 0;

                            Store(br, t.TriangleNumber, 12, ref bitpos);
                            Store(br, t.TriangleOpp, 12, ref bitpos);
                            bitpos += 7;
                            Store(br, t.Flag, 1, ref bitpos);
                            foreach(var i in t.Indices)
                            {
                                Store(br, i.VertexId, sizeof(ushort) * 8, ref bitpos);
                                Store(br, (ulong)i.Offset, 15, ref bitpos);
                                Store(br, i.Flag, 1, ref bitpos);
                            }

                            br.CopyTo(data, pos);
                            pos += tSize;
                        }
                    }

                    // x, y, z, id
                    int vSize = 4 * 4 * s.Vertices.Count;
                    while (pos + vSize > data.Length)
                        Array.Resize(ref data, data.Length * 2);

                    foreach (var v in s.Vertices)
                    {
                        Utilities.WriteFloat(data, v.X, ref pos);
                        Utilities.WriteFloat(data, v.Y, ref pos);
                        Utilities.WriteFloat(data, v.Z, ref pos);
                        Utilities.WriteDWord(data, v.MeshId, ref pos);
                    }

                    pos = headerPos + (int)s.Size;
                    
                    while (pos > data.Length)
                        Array.Resize(ref data, data.Length * 2);

                    headerPos -= 4;
                    Utilities.WriteDWord(data, (uint)(pos - headerPos - 4), ref headerPos);
                }

                foreach(var h in m.HardpointSections)
                {
                    // hpid, count, list of ids
                    uint hpSize = 4 + 4 + 4 * h.MeshIdCount;
                    if (pos + hpSize > data.Length)
                        Array.Resize(ref data, data.Length * 2);

                    Utilities.WriteString(data, "hpid", ref pos);

                    Utilities.WriteDWord(data, h.MeshIdCount, ref pos);
                    for (int a = 0; a < h.MeshIdCount; a++)
                        Utilities.WriteDWord(data, h.MeshIds[a], ref pos);
                }

                first = false;
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Write(data, 0, pos);
            }
        }

        public void RenderSur(Device device, List<ModelViewForm.MeshGroup> meshgroups)
        {
            var badcolor = new SharpDX.Color(255, 255, 255, 50);
            var colors = new SharpDX.Color[]
            {
                new SharpDX.Color(230, 25, 75, 50),
                new SharpDX.Color(60, 180, 75, 50),
                new SharpDX.Color(255, 225, 25, 50),
                new SharpDX.Color(0, 130, 200, 50),
                new SharpDX.Color(245, 130, 48, 50),
                new SharpDX.Color(145, 30, 180, 50),
                new SharpDX.Color(70, 240, 240, 50),
                new SharpDX.Color(240, 50, 230, 50),
                new SharpDX.Color(210, 245, 60, 50),
                new SharpDX.Color(250, 190, 190, 50),
                new SharpDX.Color(0, 128, 128, 50),
                new SharpDX.Color(230, 190, 255, 50),
                new SharpDX.Color(170, 110, 40, 50),
                new SharpDX.Color(255, 250, 200, 50),
                new SharpDX.Color(128, 0, 0, 50),
                new SharpDX.Color(170, 255, 195, 50),
                new SharpDX.Color(128, 128, 0, 50),
                new SharpDX.Color(255, 215, 180, 50),
                new SharpDX.Color(0, 0, 128, 50),
            };

            device.SetTexture(0, null);
            device.SetTransform(TransformState.World, SharpDX.Matrix.Identity);

            device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.SelectArg2);
            device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.TFactor);
            device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.SelectArg2);
            device.SetTextureStageState(0, TextureStage.AlphaArg2, TextureArgument.TFactor);

            int c = 0;
            Dictionary<uint, SharpDX.Matrix> meshgrouptransforms = new Dictionary<uint, SharpDX.Matrix>();
            HashSet<uint> meshgroupdisplay = new HashSet<uint>();
            foreach(var m in meshgroups)
            {
                var k = Utilities.FLModelCRC(m.Name);
                if (meshgrouptransforms.ContainsKey(k))
                    continue;
                else
                {
                    meshgrouptransforms[k] = m.Transform;

                    if (m.DisplayInfo.Display)
                        meshgroupdisplay.Add(k);
                }

            }

            foreach (var m in Meshes)
            {
                if (!meshgroupdisplay.Contains(m.MeshId) && meshgrouptransforms.ContainsKey(m.MeshId))
                {
                    c++;
                    continue;
                }

                if (meshgrouptransforms.ContainsKey(m.MeshId))
                    device.SetTransform(TransformState.World, meshgrouptransforms[m.MeshId]);
                else // Problematic, need to warn user
                    device.SetTransform(TransformState.World, SharpDX.Matrix.Identity);

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

                var color = meshgrouptransforms.ContainsKey(m.MeshId) ? colors[c % colors.Length] : badcolor;
                device.SetRenderState(RenderState.TextureFactor, color.ToBgra());

                device.DrawIndexedUserPrimitives(SharpDX.Direct3D9.PrimitiveType.TriangleList,
                    0, tmpVertices.Count, tmpTri.Count / 3, tmpTri.ToArray(), SharpDX.Direct3D9.Format.Index32, tmpVertices.ToArray());

                c++;
            }
        }

        public void RenderSurCenters(Device device, List<ModelViewForm.MeshGroup> meshgroups, SharpDX.Vector3 cameraPos)
        {
            var badcolor = new SharpDX.Color(255, 255, 255, 50);
            var colors = new SharpDX.Color[]
            {
                new SharpDX.Color(230, 25, 75, 255),
                new SharpDX.Color(60, 180, 75, 255),
                new SharpDX.Color(255, 225, 25, 255),
                new SharpDX.Color(0, 130, 200, 255),
                new SharpDX.Color(245, 130, 48, 255),
                new SharpDX.Color(145, 30, 180, 255),
                new SharpDX.Color(70, 240, 240, 255),
                new SharpDX.Color(240, 50, 230, 255),
                new SharpDX.Color(210, 245, 60, 255),
                new SharpDX.Color(250, 190, 190, 255),
                new SharpDX.Color(0, 128, 128, 255),
                new SharpDX.Color(230, 190, 255, 255),
                new SharpDX.Color(170, 110, 40, 255),
                new SharpDX.Color(255, 250, 200, 255),
                new SharpDX.Color(128, 0, 0, 255),
                new SharpDX.Color(170, 255, 195, 255),
                new SharpDX.Color(128, 128, 0, 255),
                new SharpDX.Color(255, 215, 180, 255),
                new SharpDX.Color(0, 0, 128, 255),
            };

            device.SetRenderState(RenderState.PointSize, 30.0f);

            device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.SelectArg2);
            device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.TFactor);
            device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.SelectArg2);
            device.SetTextureStageState(0, TextureStage.AlphaArg2, TextureArgument.TFactor);
            device.SetTransform(TransformState.World, SharpDX.Matrix.Identity);

            int c = 0;
            Dictionary<uint, SharpDX.Matrix> meshgrouptransforms = new Dictionary<uint, SharpDX.Matrix>();
            HashSet<uint> meshgroupdisplay = new HashSet<uint>();
            foreach (var m in meshgroups)
            {
                var k = Utilities.FLModelCRC(m.Name);
                if (meshgrouptransforms.ContainsKey(k))
                    continue;
                else
                {
                    meshgrouptransforms[k] = m.Transform;

                    if (m.DisplayInfo.Display)
                        meshgroupdisplay.Add(k);
                }

            }

            List<Tuple<SharpDX.Vector3, int>> points = new List<Tuple<SharpDX.Vector3, int>>();

            foreach (var m in Meshes)
            {
                if (!meshgroupdisplay.Contains(m.MeshId) && meshgrouptransforms.ContainsKey(m.MeshId))
                {
                    c++;
                    continue;
                }

                var mat = SharpDX.Matrix.Identity;

                if (meshgrouptransforms.ContainsKey(m.MeshId))
                    mat = meshgrouptransforms[m.MeshId];

                foreach (var s in m.SurfaceSections)
                {
                    var color = meshgrouptransforms.ContainsKey(m.MeshId) ? colors[c % colors.Length] : badcolor;
                    points.Add(new Tuple<SharpDX.Vector3, int>(SharpDX.Vector3.TransformCoordinate(s.Center, mat), color.ToBgra()));

                }

                c++;
            }

            points.Sort((a, b) =>
            {
                float l1 = (a.Item1 - cameraPos).LengthSquared();
                float l2 = (b.Item1 - cameraPos).LengthSquared();

                return l1 < l2 ? 1 : l1 > l2 ? -1 : 0;
            });

            foreach(var p in points)
            {
                device.SetRenderState(RenderState.TextureFactor, p.Item2);

                device.DrawUserPrimitives(PrimitiveType.PointList, 1, new SharpDX.Vector3[] { p.Item1 });
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

        private static void Store(BitArray br, UInt64 input, int length, ref int pos)
        {
            for (int a = 0; a < length; a++)
            {
                br.Set(pos + a, (input & 1) == 1);
                input >>= 1;
            }
            pos += length;
        }
    }
}
