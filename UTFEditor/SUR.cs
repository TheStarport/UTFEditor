using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace UTFEditor
{
	public class SUR
	{
		public string VersionString;
		public float VersionNumber;
		public string FixedFlag;
		public uint MeshId;
		public uint SurfaceType;
		
		public struct HardpointSection
		{
			public uint MeshIdCount;
			public uint[] MeshIds;
		}
		
		List<HardpointSection> HardpointSections;
		
		public struct SurfaceSection
		{
			public SharpDX.Vector3[] BoundingBox;
			public SharpDX.Vector3 Center;
			public SharpDX.Vector3 Inertia;
			public float Radius;
			public ushort Scale;
			public uint Size;
			public uint BitsSectionOffset;
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
			public short Offset;
			public byte Flag;
		}
		
		public SUR(byte[] data)
		{
			HardpointSections = new List<HardpointSection>();
			int pos = 0;
			
			VersionString = Utilities.GetString(data, ref pos, 4);
			VersionNumber = Utilities.GetFloat(data, ref pos);
			
			if(VersionNumber != 2.0f || VersionString != "vers")
				throw new FormatException();

			MeshId = Utilities.GetDWord(data, ref pos);
			SurfaceType = Utilities.GetDWord(data, ref pos);

			SurfaceSection ss = new SurfaceSection();
			
			while(pos < data.Length)
			{
				string nextType = Utilities.GetString(data, ref pos, 4);
				if(nextType == "hpid")
				{
					HardpointSection hps =  new HardpointSection();
					hps.MeshIdCount = Utilities.GetDWord(data, ref pos);
					hps.MeshIds = new uint[hps.MeshIdCount];
					for (int a = 0; a < hps.MeshIdCount; a++)
						hps.MeshIds[a] = Utilities.GetDWord(data, ref pos);
					HardpointSections.Add(hps);
				}
				else if(nextType == "!fxd")
					FixedFlag = nextType;
				else if(nextType == "exts")
				{
					ss.BoundingBox = new SharpDX.Vector3[2];
					for (int a = 0; a < 2; a++)
						ss.BoundingBox[a] = new SharpDX.Vector3(
													Utilities.GetFloat(data, ref pos),
													Utilities.GetFloat(data, ref pos),
													Utilities.GetFloat(data, ref pos));
				}
				else if(nextType == "surf")
				{
					uint unk = Utilities.GetDWord(data, ref pos);
					ss.Center = new SharpDX.Vector3(
										Utilities.GetFloat(data, ref pos),
										Utilities.GetFloat(data, ref pos),
										Utilities.GetFloat(data, ref pos));
					ss.Inertia = new SharpDX.Vector3(
										Utilities.GetFloat(data, ref pos),
										Utilities.GetFloat(data, ref pos),
										Utilities.GetFloat(data, ref pos));
					ss.Radius = Utilities.GetFloat(data, ref pos);
					ss.Scale = data[pos++];
					ss.Size = Utilities.GetDWord(data, ref pos) & 0x00FFFFFF; pos--;
					ss.BitsSectionOffset = Utilities.GetDWord(data, ref pos);
					
					// padding
					pos += 3*sizeof(uint);

					int maxPos = data.Length;
					while(pos < maxPos)
					{
						TriangleGroup tgh = new TriangleGroup();

						tgh.VertexOffset = Utilities.GetDWord(data, ref pos);
						tgh.MeshId = Utilities.GetDWord(data, ref pos);
						tgh.Type = data[pos++];
						tgh.RefVertexCount = Utilities.GetDWord(data, ref pos) & 0x00FFFFFF; pos--;
						tgh.TriangleCount = Utilities.GetShort(data, ref pos);
						tgh.Triangles = new Triangle[tgh.TriangleCount];
						
						maxPos = pos + (int) tgh.VertexOffset;
						
						for(int a = 0; a < tgh.TriangleCount; a++)
						{
							Triangle t = new Triangle();
							byte[] tempData = new byte[20];
							Array.Copy(data, pos, tempData, 0, 20);
							BitArray br = new BitArray( tempData );
							int bitpos = 0;
							bitpos += 13;
							t.TriangleNumber = (ushort)Advance(br, 12, ref bitpos);
							t.TriangleOpp = (ushort)Advance(br, 12, ref bitpos);
							bitpos += 7;
							t.Flag = (byte)Advance(br, 1, ref bitpos);
							t.Indices = new Index[3];
							for (int b = 0; b < 3; b++)
							{
								Index i = new Index();
								i.VertexId = (ushort)Advance(br, sizeof(ushort)*8, ref bitpos);
								i.Offset = (short)Advance(br, 15, ref bitpos);
								i.Flag = (byte)Advance(br, 1, ref bitpos);
								t.Indices[b] = i;
							}
							tgh.Triangles[a] = t;
							
							PrintBits(br);
							
							pos += 20;
						}
					}
					
				} else throw new FormatException();
			}
		}
		
		public static SUR LoadFromFile(string cmpPath)
		{
			string surPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(cmpPath), System.IO.Path.GetFileNameWithoutExtension(cmpPath)) + ".sur";
			if(System.IO.File.Exists(surPath))
			{
				return new SUR(System.IO.File.ReadAllBytes(surPath));
			}
			
			return null;
		}
		
		private static UInt64 Advance(BitArray br, int length, ref int pos)
		{
			UInt64 output = 0;
			for(int a = 0; a < length; a++)
			{
				output <<= 1;
				output += (UInt64) (br[pos + a] ? 1 : 0);
			}
			PrintBits(br, pos, length);
			pos += length;
			return output;
		}

		private static void PrintBits(BitArray br)
		{
			foreach (bool a in br)
			{
				System.Diagnostics.Debug.Write((a ? 1 : 0));
			}
			System.Diagnostics.Debug.WriteLine(";");
		}

		private static void PrintBits(BitArray br, int pos, int length)
		{
			for (int a = 0; a < br.Length; a++)
			{
				if (a >= pos && a < pos + length)
					System.Diagnostics.Debug.Write((br[a] ? 1 : 0));
				else
					System.Diagnostics.Debug.Write(" ");
			}
			System.Diagnostics.Debug.WriteLine(";");
		}
	}
}
