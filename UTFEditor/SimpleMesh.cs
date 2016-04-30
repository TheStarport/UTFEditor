using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D9;

namespace UTFEditor
{
    public struct IntersectInformation<I>
    {
        public float Dist;
        public I FaceIndex;
    }

    public class SimpleMesh<V, I> : IDisposable where V : struct, IVertexFormat where I : struct
    {
        public I[] Indices { get; private set; }
        public V[] Vertices { get; private set; }

        private VertexBuffer vb;
        private IndexBuffer ib;
        private Device dev;

        public SimpleMesh(Device dev, V[] vertices, I[] indices)
        {
            this.dev = dev;

            if (vertices.Length == 0)
                return;

            vb = new VertexBuffer(dev, vertices.Length * vertices[0].StrideV, Usage.WriteOnly, vertices[0].FormatV, Pool.Default);
            using (DataStream ds = vb.Lock(0, 0, LockFlags.None))
                ds.WriteRange(vertices);

            Vertices = new V[vertices.Length];
            Array.Copy(vertices, Vertices, vertices.Length);

            if (indices.Length == 0)
                return;

            ib = new IndexBuffer(dev, indices.Length * (typeof(I) == typeof(ushort) ? sizeof(ushort) : sizeof(uint)), Usage.None, Pool.Default, typeof(I) == typeof(ushort));
            using (DataStream ds = ib.Lock(0, 0, LockFlags.None))
                ds.WriteRange(indices);

            Indices = new I[indices.Length];
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

        public bool Intersect(Ray r, out IntersectInformation<I> hit)
        {
            hit = new IntersectInformation<I>();
            return false;
        }
    }
}
