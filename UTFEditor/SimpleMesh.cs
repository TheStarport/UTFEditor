using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;

namespace UTFEditor
{
    public struct IntersectInformation
    {
        public float Dist;
        public uint FaceIndex;
    }

    public class SimpleMesh<V> : IDisposable where V : struct, IVertexFormat
    {
        public ushort[] Indices { get; private set; }
        public V[] Vertices { get; private set; }

        private VertexBuffer vb;
        private IndexBuffer ib;
        private Device dev;

        public SimpleMesh(Device dev, V[] vertices, ushort[] indices)
        {
            this.dev = dev;

            if (vertices.Length == 0)
                return;

            vb = new VertexBuffer(dev, vertices.Length * vertices[0].StrideV, Usage.WriteOnly, vertices[0].FormatV, Pool.Default);
            using (DataStream ds = vb.Lock(0, 0, LockFlags.None))
                ds.WriteRange(vertices);
            vb.Unlock();

            Vertices = new V[vertices.Length];
            Array.Copy(vertices, Vertices, vertices.Length);

            if (indices.Length == 0)
                return;

            ib = new IndexBuffer(dev, indices.Length * sizeof(ushort), Usage.None, Pool.Default, true);
            using (DataStream ds = ib.Lock(0, 0, LockFlags.None))
                ds.WriteRange(indices);
            ib.Unlock();

            Indices = new ushort[indices.Length];
            Array.Copy(indices, Indices, indices.Length);
        }

        public void Draw()
        {
            dev.VertexFormat = Vertices[0].FormatV;
            dev.SetStreamSource(0, vb, 0, Vertices[0].StrideV);
            dev.Indices = ib;

            if (ib == null)
                dev.DrawPrimitives(PrimitiveType.TriangleList, 0, Vertices.Length / 3);
            else
                dev.DrawIndexedPrimitive(PrimitiveType.TriangleList, 0, 0, Vertices.Length, 0, Indices.Length / 3);
        }

        public void Dispose()
        {
            vb.Dispose();
            ib.Dispose();

            Vertices = null;
            Indices = null;
        }

        public bool Intersects(Ray r, out IntersectInformation hit)
        {
            hit = new IntersectInformation();
            hit.Dist = Single.MaxValue;
            hit.FaceIndex = 0;

            for(uint i = 0; i < Indices.Length; i += 3)
            {
                Vector3 v1 = Vertices[Indices[i]].GetPosition();
                Vector3 v2 = Vertices[Indices[i + 1]].GetPosition();
                Vector3 v3 = Vertices[Indices[i + 2]].GetPosition();
                float dist;
                if(r.Intersects(ref v1, ref v2, ref v3, out dist) && dist < hit.Dist)
                {
                    hit.Dist = dist;
                    hit.FaceIndex = i / 3;
                }
            }

            return hit.Dist < Single.MaxValue;
        }
    }
}
