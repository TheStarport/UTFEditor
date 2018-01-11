using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX.Direct3D9;
using System.IO;
using SharpDX;
using System.Reflection;
using System.Collections;

namespace UTFEditor
{
    public partial class ModelViewForm : Form, UTFFormObserver
    {
        /// <summary>
        /// The directx device.
        /// </summary>
        /// 
        Direct3D d3d = null;
        Device device = null;
        SwapChain swap = null;
        PresentParameters presentParams;
        Surface depthStencil = null;
        Format depthFormat;

        SurFile sur = null;

        enum SurDisplay
        {
            Hidden,
            Wireframe,
            Transparent,
            Opaque
        }

        SurDisplay surDisplay = SurDisplay.Hidden;
        bool SurDisplayCenters => centersToolStripMenuItem.Checked;

        const float relativeHardpointScale = 60;
        float distance;

        Color background = Color.Black;

        /// <summary>
        /// The last position of the mouse when the model is being rotated.
        /// </summary>
        System.Drawing.Point lastPosition;

        Vector3 cameraPosition;
        Vector2 cameraYawPitch;
        float cameraZoom = 1.0f;
        Matrix viewMatrix, projMatrix;
        
        DateTime lastClickTime;

        public struct BoundingBox
        {
            Vector3 min, max;
            public BoundingBox(Vector3 mi, Vector3 ma)
            {
                min = mi;
                max = ma;
            }

            public bool Intersect(Vector3 near, Vector3 far)
            {
                Vector3 direction = far - near;
                direction.Normalize();
                float distance = 0f;
                float tmax = float.MaxValue;

                if (Math.Abs(direction.X) < 1e-3f)
                {
                    if (near.X < this.min.X || near.X > this.max.X)
                    {
                        distance = 0f;
                        return false;
                    }
                }
                else
                {
                    float inverse = 1.0f / direction.X;
                    float t1 = (this.min.X - near.X) * inverse;
                    float t2 = (this.max.X - near.X) * inverse;

                    if (t1 > t2)
                    {
                        float temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }

                    distance = Math.Max(t1, distance);
                    tmax = Math.Min(t2, tmax);

                    if (distance > tmax)
                    {
                        distance = 0f;
                        return false;
                    }
                }

                if (Math.Abs(direction.Y) < 1e-3f)
                {
                    if (near.Y < this.min.Y || near.Y > this.max.Y)
                    {
                        distance = 0f;
                        return false;
                    }
                }
                else
                {
                    float inverse = 1.0f / direction.Y;
                    float t1 = (this.min.Y - near.Y) * inverse;
                    float t2 = (this.max.Y - near.Y) * inverse;

                    if (t1 > t2)
                    {
                        float temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }

                    distance = Math.Max(t1, distance);
                    tmax = Math.Min(t2, tmax);

                    if (distance > tmax)
                    {
                        distance = 0f;
                        return false;
                    }
                }

                if (Math.Abs(direction.Z) < 1e-3f)
                {
                    if (near.Z < this.min.Z || near.Z > this.max.Z)
                    {
                        distance = 0f;
                        return false;
                    }
                }
                else
                {
                    float inverse = 1.0f / direction.Z;
                    float t1 = (this.min.Z - near.Z) * inverse;
                    float t2 = (this.max.Z - near.Z) * inverse;

                    if (t1 > t2)
                    {
                        float temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }

                    distance = Math.Max(t1, distance);
                    tmax = Math.Min(t2, tmax);

                    if (distance > tmax)
                    {
                        distance = 0f;
                        return false;
                    }
                }

                return true;
            }
        };

        /// <summary>
        /// Mesh group data.
        /// </summary>
        public class MeshGroup
        {
            public string Name;
            public MeshGroupDisplayInfo DisplayInfo;
            public VMeshRef RefData;
            public VWireData WireData;
            public Matrix Transform;
            public MeshDataBuffer MeshDataBuffer;
            public SimpleMesh<VertexPositionNormalTexture>[] M;
            public BoundingBox[] B;
		};

		public struct MeshGroupDisplayInfo
		{
			public bool Display;
			//public string Name;
			public int Level;
			public ShadingMode Shading;
			public Color Color;
			public TextureMode Texture;
			public bool Wire;
		}

		public enum ShadingMode
		{
			Flat,
			Wireframe,
			Smooth
		}
		
		public enum TextureMode
		{
			TextureColor,
			Texture,
			Color,
			None
		}

        /// <summary>
        /// THe list of mesh groups in the model.
        /// </summary>
        List<MeshGroup> MeshGroups = new List<MeshGroup>();

        /// <summary>
        /// Vertex and index buffers for the mesh
        /// </summary>
        public class MeshDataBuffer
        {
            public uint crc;
            public VMeshData VMeshData;
            public VertexPositionNormalTexture[] V;
            public UInt16[] I;
        }

        /// <summary>
        /// The vertex and index buffers for one or more meshes.
        /// </summary>
        List<MeshDataBuffer> MeshDataBuffers = new List<MeshDataBuffer>();

        /// <summary>
        /// The set of textures used by the model.
        /// </summary>
        HashSet<uint> TexRequired = new HashSet<uint>();

        /// <summary>
        /// The set of textures yet to be found.
        /// </summary>
        HashSet<uint> TexRemaining;

        /// <summary>
        /// The root node of the model we are drawing.
        /// </summary>
        TreeNode rootNode;
        TreeView utf;

        /// <summary>
        /// The parent tree to watch for changes.
        /// </summary>
        UTFForm parent;

		/// <summary>
		/// Map Cmpnd file names to object names.
		/// </summary>
		Dictionary<string, string> mapFileToObj = new Dictionary<string, string>();

        /// <summary>
        /// The directory path to search for sur/mat files.
        /// </summary>
        string directoryPath;

        /// <summary>
        /// The mesh used to indicate hardpoint position.
        /// </summary>
        public class Hardpoint
        {
            public float scale = 1;
            public VertexBuffer display;
            public VertexBuffer revolute;
            public IndexBuffer indices;
            public float max;
            public float min;

            // Define the vertices for the hardpoint display.
            static public VertexPositionColor[] displayvertices =
            {
                // Dummy central vertex
                
                new VertexPositionColor( 0, 0, 0, 0),
                
                // Central white cube
                
                new VertexPositionColor( -0.5f, -0.5f,  -0.5f, 0x666666),
                new VertexPositionColor( -0.5f, 0.5f,  -0.5f, 0x666666),
                new VertexPositionColor( 0.5f, 0.5f,  -0.5f, 0x888888),
                new VertexPositionColor( 0.5f, -0.5f,  -0.5f, 0x888888),
                new VertexPositionColor( -0.5f, -0.5f,  0.5f, 0xaaaaaa),
                new VertexPositionColor( 0.5f, -0.5f,  0.5f, 0xaaaaaa),
                new VertexPositionColor( 0.5f, 0.5f,  0.5f, 0xcccccc),
                new VertexPositionColor( -0.5f, 0.5f,  0.5f, 0xcccccc),
                
                // Y axis
                
                new VertexPositionColor( -0.125f, -0.25f,  -0.125f, 0x00aa00),
                new VertexPositionColor( -0.125f, 2.75f,  -0.125f, 0x00dd00),
                new VertexPositionColor( 0.125f, 2.75f,  -0.125f, 0x00dd00),
                new VertexPositionColor( 0.125f, -0.25f,  -0.125f, 0x00aa00),
                new VertexPositionColor( -0.125f, -0.25f,  0.125f, 0x00aa00),
                new VertexPositionColor( 0.125f, -0.25f,  0.125f, 0x00aa00),
                new VertexPositionColor( 0.125f, 2.75f,  0.125f, 0x00dd00),
                new VertexPositionColor( -0.125f, 2.75f,  0.125f, 0x00dd00),
                
                // X axis
                
                new VertexPositionColor( -0.25f, 0.125f,  -0.125f, 0xaa0000),
                new VertexPositionColor( 2.75f, 0.125f,  -0.125f, 0xdd0000),
                new VertexPositionColor( 2.75f, -0.125f,  -0.125f, 0xdd0000),
                new VertexPositionColor( -0.25f, -0.125f,  -0.125f, 0xaa0000),
                new VertexPositionColor( -0.25f, 0.125f,  0.125f, 0xaa0000),
                new VertexPositionColor( -0.25f, -0.125f,  0.125f, 0xaa0000),
                new VertexPositionColor( 2.75f, -0.125f,  0.125f, 0xdd0000),
                new VertexPositionColor( 2.75f, 0.125f,  0.125f, 0xdd0000),
                
                // Z axis
                
                new VertexPositionColor( 0.125f, 0.125f,  0.25f, 0x0000aa),
                new VertexPositionColor( 0.125f, 0.125f,  -2.75f, 0x0000dd),
                new VertexPositionColor( 0.125f, -0.125f,  -2.75f, 0x0000dd),
                new VertexPositionColor( 0.125f, -0.125f,  0.25f, 0x0000aa),
                new VertexPositionColor( -0.125f, 0.125f,  0.25f, 0x0000aa),
                new VertexPositionColor( -0.125f, -0.125f,  0.25f, 0x0000aa),
                new VertexPositionColor( -0.125f, -0.125f,  -2.75f, 0x0000dd),
                new VertexPositionColor( -0.125f, 0.125f,  -2.75f, 0x0000dd),
            };
            
            static public int[] displayindexes =
            {
                // Central white cube
                
				1, 2, 3, 
				3, 4, 1, 
				5, 6, 7, 
				7, 8, 5, 
				1, 4, 6, 
				6, 5, 1, 
				4, 3, 7, 
				7, 6, 4, 
				3, 2, 8, 
				8, 7, 3, 
				2, 1, 5, 
				5, 8, 2, 
				
				// Y axis
				
				9, 10, 11, 
				11, 12, 9, 
				13, 14, 15, 
				15, 16, 13, 
				9, 12, 14, 
				14, 13, 9, 
				12, 11, 15, 
				15, 14, 12, 
				11, 10, 16, 
				16, 15, 11, 
				10, 9, 13, 
				13, 16, 10, 
                
                // X axis
				
				17, 18, 19, 
				19, 20, 17, 
				21, 22, 23, 
				23, 24, 21, 
				17, 20, 22, 
				22, 21, 17, 
				20, 19, 23, 
				23, 22, 20, 
				19, 18, 24, 
				24, 23, 19, 
				18, 17, 21, 
				21, 24, 18, 
				
				// Z axis
				
				25, 26, 27, 
				27, 28, 25, 
				29, 30, 31, 
				31, 32, 29, 
				25, 28, 30, 
				30, 29, 25, 
				28, 27, 31, 
				31, 30, 28, 
				27, 26, 32, 
				32, 31, 27, 
				26, 25, 29, 
				29, 32, 26, 
            };
        };
        Hardpoint hp = new Hardpoint();
        
        public class HardpointDisplayInfo
        {
			public Matrix Matrix;
			public TreeNode Node;
			public string Name;
			public float Min, Max;
			public bool Revolute;
			
			public MeshGroup MeshGroup;
			public Color Color;
			public bool Display;
        }
		List<HardpointDisplayInfo> otherHardpoints = new List<HardpointDisplayInfo>();

        /// <summary>
        /// Map a filename to its mesh group.
        /// </summary>
        Dictionary<string, int> mapFileToMesh = new Dictionary<string,int>();

        CmpFixData fixData;
        CmpRevData revData, prisData;
        CmpSphereData sphereData;
        Dictionary<string, Matrix> ParentTransform = new Dictionary<string, Matrix>();

        AddHardpoints addHps;

        public ModelViewForm(UTFForm parent, TreeView utf, string directoryPath)
        {
            this.sur = parent.SurFile;
            this.parent = parent;
            this.utf = utf;
            this.rootNode = utf.Nodes[0];
            this.directoryPath = Path.GetDirectoryName(directoryPath);
            InitializeComponent();
            this.Text += " - " + Path.GetFileName(directoryPath);
            this.MouseWheel += new MouseEventHandler(modelView_MouseWheel);
            parent.AddObserver(this);

            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = viewPanelView.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(viewPanelView, true, null);

                dgvType = hardpointPanelView.GetType();
                pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(hardpointPanelView, true, null);
            }
        }

        private void ModelViewForm_Activated(object sender, EventArgs e)
        {
            modelView.Panel1.Focus();
        }

        /// <summary>
        /// Setup directx.
        /// </summary>
        public void InitializeGraphics()
        {
            d3d = new Direct3D();

            presentParams = new PresentParameters();
            presentParams.Windowed = true;
			presentParams.SwapEffect = SwapEffect.Discard;

			Format[] formats = { Format.D32, Format.D24X8, Format.D16 };
			foreach (Format format in formats)
			{
                if (!d3d.CheckDeviceFormat(0, DeviceType.Hardware, Format.X8R8G8B8, Usage.DepthStencil, ResourceType.Surface, format))
                    continue;

				depthFormat = format;
				if (d3d.CheckDepthStencilMatch(0, DeviceType.Hardware, d3d.Adapters[0].CurrentDisplayMode.Format, d3d.Adapters[0].CurrentDisplayMode.Format, depthFormat)) break;
			}

			try
			{
				device = new Device(d3d, 0, DeviceType.Hardware, modelView.Panel1.Handle, CreateFlags.HardwareVertexProcessing, presentParams);
			}
			catch
			{
				device = null;
			}
			if (device == null)
				device = new Device(d3d, 0, DeviceType.Hardware, modelView.Panel1.Handle, CreateFlags.SoftwareVertexProcessing, presentParams);
            
            if (device == null)
                throw new Exception("Unable to initialise DirectX.");

			this.SetupDevice(device);

            cameraZoom = distance;
            hp.scale = distance / relativeHardpointScale;
            ChangeHardpointSize(1);
			ChangeScale(1);
			
			presentParams.BackBufferWidth = modelView.Panel1.Width;
			presentParams.BackBufferHeight = modelView.Panel1.Height;
			presentParams.DeviceWindowHandle = modelView.Panel1.Handle;

			swap = new SwapChain(device, presentParams);

			depthStencil = Surface.CreateDepthStencil(device, modelView.Panel1.Width, modelView.Panel1.Height, depthFormat, MultisampleType.None, 0, true);
        }

        /// <summary>
        /// Load the model into the vertex and index buffers. Make note of the
        /// positions and rotations of the mesh groups we need to render.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void SetupDevice(Device dev)
        {
            dev.SetRenderState(RenderState.ZEnable, true);
            dev.SetRenderState(RenderState.DitherEnable, true);

			if (hp.display != null)
			{
				hp.display.Dispose();
				hp.revolute.Dispose();
			}
			hp.display = new VertexBuffer(dev, Hardpoint.displayvertices.Length * VertexPositionColor.Stride, Usage.WriteOnly, VertexPositionColor.Format, Pool.Default);
            using (DataStream ds = hp.display.Lock(0, 0, LockFlags.None))
                ds.WriteRange(Hardpoint.displayvertices);
            hp.display.Unlock();
			hp.revolute = new VertexBuffer(dev, 26, Usage.WriteOnly, VertexPositionColor.Format, Pool.Default);
			hp.max = Single.MaxValue;
			hp.min = Single.MinValue;

			if (hp.indices != null)
			{
				hp.indices.Dispose();
			}
			hp.indices = new IndexBuffer(device, Hardpoint.displayindexes.Length * sizeof(int), Usage.WriteOnly, Pool.Default, false);
            using (DataStream ds = hp.indices.Lock(0, 0, LockFlags.None))
                ds.WriteRange(Hardpoint.displayindexes);
            hp.indices.Unlock();

            DataChanged(DataChangedType.All);
        }

        /// <summary>
        /// Parse the treeview and build the directx vertex and index buffers.
        /// </summary>
        public void DataChanged(DataChangedType changeType)
        {
            bool changeMesh = (changeType & DataChangedType.Mesh) == DataChangedType.Mesh;
            bool changeHardpoints = (changeType & DataChangedType.Hardpoints) == DataChangedType.Hardpoints;

            hardpointPanelView.SuspendDrawing();
            viewPanelView.SuspendDrawing();

            hardpointPanelView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            viewPanelView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            if (changeMesh)
            {
                foreach (MeshGroup bd in MeshGroups)
                {
                    foreach (var m in bd.M)
                        m.Dispose();
                }

                MeshGroups.Clear();
                MeshDataBuffers.Clear();

                TreeNode VMeshLibrary = rootNode.Nodes["VMeshLibrary"];
                if (VMeshLibrary == null)
                {
                    // If there's a VMeshRef with no VMeshLibrary, assume interface.generic.vms.
                    if (rootNode.Nodes.Find("VMeshRef", true).Length != 0)
                    {
                        int data = directoryPath.IndexOf(@"\DATA\", 0, StringComparison.OrdinalIgnoreCase);
                        if (data != -1)
                        {
                            try
                            {

                                UTFFile generic = new UTFFile();
                                TreeNode genericLib = generic.LoadUTFFile(directoryPath.Remove(data + 6) + @"INTERFACE\interface.generic.vms");
                                VMeshLibrary = genericLib.Nodes["VMeshLibrary"];
                            }
                            catch { }
                        }
                    }
                }
                if (VMeshLibrary == null)
                    throw new Exception("Model not found");

                // Scan for and build the mesh groups and the vertex/index buffers for
                // each mesh group.
                foreach (TreeNode node in VMeshLibrary.Nodes.Find("VMeshData", true))
                {
                    MeshDataBuffer md = new MeshDataBuffer();
                    md.crc = Utilities.FLModelCRC(node.Parent.Name);
                    md.VMeshData = new VMeshData(node.Tag as byte[]);

                    // Convert index and vertex data into appropriate directx formats.
                    List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
                    foreach (VMeshData.TVertex vert in md.VMeshData.Vertices)
                        vertices.Add(new VertexPositionNormalTexture(vert.X, vert.Y, vert.Z, vert.NormalX, vert.NormalY, vert.NormalZ, vert.S, vert.T));

                    List<UInt16> indices = new List<UInt16>();
                    foreach (VMeshData.TTriangle tri in md.VMeshData.Triangles)
                    {
                        indices.Add((UInt16)tri.Vertex1);
                        indices.Add((UInt16)tri.Vertex2);
                        indices.Add((UInt16)tri.Vertex3);
                    }

                    // Copy data into GPU buffers
                    md.I = indices.ToArray();
                    md.V = vertices.ToArray();

                    // Save the group.
                    MeshDataBuffers.Add(md);
                }

                // Find Cons(truct) nodes. They contain data that links each mesh to the
                // root mesh.
                mapFileToObj.Clear();
                mapFileToObj["\\"] = "Model";
                foreach (TreeNode nodeObj in rootNode.Nodes.Find("Object Name", true))
                {
                    foreach (TreeNode nodeFileName in nodeObj.Parent.Nodes.Find("File Name", false))
                    {
                        string objectName = Utilities.GetString(nodeObj);
                        string fileName = Utilities.GetString(nodeFileName);
                        mapFileToObj[fileName] = objectName;
                    }
                }

                // Scan the level 0 VMeshRefs to build mesh group list for each 
                // of the construction nodes identified in the previous search.
                //string levelstr = spinnerLevel.Value.ToString();
                foreach (TreeNode node in rootNode.Nodes.Find("VMeshRef", true))
                {
                    try
                    {
                        // Test for LevelN\VMeshData\VMeshRef.
                        TreeNode fileNode = node.Parent.Parent;
                        string levelName = fileNode.Name;
                        string fileName;
                        bool displayDefault = false;
                        if (levelName.StartsWith("Level", StringComparison.OrdinalIgnoreCase))
                        {
                            // Okay, back up to filename\MultiLevel\LevelN
                            fileNode = fileNode.Parent.Parent;
                            fileName = fileNode.Name;
                            if (levelName.Substring(5) == "0")
                                displayDefault = true;
                        }
                        else
                        {
                            // No, it's directly under the file.
                            fileName = levelName;
                            levelName = "Level0";
                            displayDefault = true;
                        }
                        string objName;
                        if (mapFileToObj.TryGetValue(fileName, out objName))
                        {
                            MeshGroupDisplayInfo mgdi = GetMeshGroupDisplayInfo(displayDefault, objName, levelName);
                            MeshGroup mg = new MeshGroup();
                            mg.Name = objName;
                            mg.DisplayInfo = mgdi;
                            mg.RefData = new VMeshRef(node.Tag as byte[]);
                            mg.Transform = Matrix.Identity;
                            mg.MeshDataBuffer = FindMatchingMeshData(mg.RefData);
                            mg.M = new SimpleMesh<VertexPositionNormalTexture>[mg.RefData.NumMeshes];
                            mg.B = new BoundingBox[mg.RefData.NumMeshes];
                            mapFileToMesh[fileName] = MeshGroups.Count;

                            int endMesh = mg.RefData.StartMesh + mg.RefData.NumMeshes;
                            for (int mn = mg.RefData.StartMesh; mn < endMesh; mn++)
                            {
                                VMeshData.TMeshHeader mesh = mg.MeshDataBuffer.VMeshData.Meshes[mn];

                                ushort[] indicesCurrent = new ushort[mesh.NumRefVertices];
                                for (int a = 0; a < indicesCurrent.Length; a++)
                                    indicesCurrent[a] = mg.MeshDataBuffer.I[mesh.TriangleStart + a];

                                VertexPositionNormalTexture[] verticesCurrent = new VertexPositionNormalTexture[mesh.EndVertex - mesh.StartVertex + 1];
                                for (int a = 0; a < verticesCurrent.Length; a++)
                                    verticesCurrent[a] = mg.MeshDataBuffer.V[mesh.StartVertex + mg.RefData.StartVert + a];

                                var m = new SimpleMesh<VertexPositionNormalTexture>(device, verticesCurrent, indicesCurrent);

                                Vector3 min, max;
                                Utilities.ComputeBoundingBox(verticesCurrent, out min, out max);
                                mg.B[mn - mg.RefData.StartMesh] = new BoundingBox(min, max);
                                mg.M[mn - mg.RefData.StartMesh] = m;
                            }

                            TreeNode WireData = fileNode.Nodes["VMeshWire"];
                            if (WireData != null)
                            {
                                WireData = WireData.Nodes["VWireData"];
                                if (WireData != null)
                                    mg.WireData = new VWireData(WireData.Tag as byte[]);
                            }

                            MeshGroups.Add(mg);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Exception while loading a meshgroup!", "Error");
                    }
                }

                // Find the offset and rotations from the Fix, Rev, Pris and Sphere nodes.
                ParentTransform.Clear();
                fixData = null;
                revData = prisData = null;
                sphereData = null;
                try
                {
                    TreeNode consNode = rootNode.Nodes["Cmpnd"].Nodes["Cons"];
                    foreach (TreeNode node in consNode.Nodes)
                    {
                        switch (node.Name.ToLowerInvariant())
                        {
                            case "fix":
                                fixData = new CmpFixData(node.Tag as byte[]);
                                break;
                            case "rev":
                                revData = new CmpRevData(node.Tag as byte[]);
                                break;
                            case "pris":
                                prisData = new CmpRevData(node.Tag as byte[]);
                                break;
                            case "sphere":
                                sphereData = new CmpSphereData(node.Tag as byte[]);
                                break;
                        }
                    }
                }
                catch { }

                // Find the minima and maxima bounds to determine initial scale.
                float max_x, max_y, max_z;
                float min_x, min_y, min_z;
                max_x = max_y = max_z = float.MinValue;
                min_x = min_y = min_z = float.MaxValue;

                foreach (MeshGroup mg in MeshGroups)
                {
                    mg.Transform = GetTransform(mg.Name);

                    if (mg.RefData.BoundingBoxMaxX + mg.Transform.M41 > max_x)
                        max_x = mg.RefData.BoundingBoxMaxX + mg.Transform.M41;
                    if (mg.RefData.BoundingBoxMaxY + mg.Transform.M42 > max_y)
                        max_y = mg.RefData.BoundingBoxMaxY + mg.Transform.M42;
                    if (mg.RefData.BoundingBoxMaxZ + mg.Transform.M43 > max_z)
                        max_z = mg.RefData.BoundingBoxMaxZ + mg.Transform.M43;
                    if (mg.RefData.BoundingBoxMinX + mg.Transform.M41 < min_x)
                        min_x = mg.RefData.BoundingBoxMinX + mg.Transform.M41;
                    if (mg.RefData.BoundingBoxMinY + mg.Transform.M42 < min_y)
                        min_y = mg.RefData.BoundingBoxMinY + mg.Transform.M42;
                    if (mg.RefData.BoundingBoxMinZ + mg.Transform.M43 < min_z)
                        min_z = mg.RefData.BoundingBoxMinZ + mg.Transform.M43;
                }
                // Now determine the largest distance.
                distance = Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z));

                // Count the number of textures.
                foreach (MeshDataBuffer mdb in MeshDataBuffers)
                {
                    foreach (VMeshData.TMeshHeader mesh in mdb.VMeshData.Meshes)
                    {
                        TexRequired.Add(mesh.MaterialId);
                    }
                }
                TexRemaining = TexRequired;

                // Load those textures. Look for textures in the directory this utf file is in
                // and look in the utf file itself.
                LoadTextures(rootNode);

                try
                {
                    string dir = directoryPath;
                    for (int i = 0; TexRemaining.Count != 0 && i < 3; ++i)
                    {
                        foreach (string matFile in Directory.GetFiles(dir, "*.mat"))
                        {
                            UTFFile matUtfFile = new UTFFile();
                            try
                            {
                                TreeNode matRootNode = matUtfFile.LoadUTFFile(matFile);
                                LoadTextures(matRootNode);
                                if (TexRemaining.Count == 0)
                                    break;
                            }
                            catch { }
                        }
                        dir = Directory.GetParent(dir).ToString();
                    }
                }
                catch { }

                viewPanelView.Sort(viewPanelView.Columns[1], ListSortDirection.Ascending);
            }

            if(changeHardpoints)
            {
                otherHardpoints.Clear();

                // Load hardpoints
                List<DataGridViewRow> hprows = new List<DataGridViewRow>();

                foreach (TreeNode nFixed in rootNode.Nodes.Find("Fixed", true))
                {
                    foreach (TreeNode nLoc in nFixed.Nodes)
                    {
                        TreeNode node = GetHardpointNode(nLoc);
                        if (node == null) continue;

                        Matrix m = GetHardpointMatrix(node);

                        HardpointDisplayInfo hi = new HardpointDisplayInfo();
                        hi.Matrix = m;
                        hi.Name = node.Name;
                        hi.Node = node;
                        hi.Min = hi.Max = 0;

                        try
                        {
                            hi.MeshGroup = MeshGroups[mapFileToMesh[hi.Node.Parent.Parent.Parent.Name]];
                        }
                        catch { hi.MeshGroup = MeshGroups[0]; }

                        hi.Revolute = false;
                        hi.Color = new Color(UTFEditorMain.FindHpColor(node.Name).ToRgba());
                        hi.Display = true;
                        otherHardpoints.Add(hi);

                        CreateHardpointRow(hi, (IList)hprows);

                    }
                }

                foreach (TreeNode nRev in rootNode.Nodes.Find("Revolute", true))
                {
                    foreach (TreeNode nLoc in nRev.Nodes)
                    {
                        TreeNode node = GetHardpointNode(nLoc);
                        if (node == null) continue;

                        Matrix m = GetHardpointMatrix(node);

                        TreeNode n = node.Nodes["Max"];
                        float max = BitConverter.ToSingle(n.Tag as byte[], 0);
                        n = node.Nodes["Min"];
                        float min = BitConverter.ToSingle(n.Tag as byte[], 0);
                        // If max is 360° or min is -360°, set them to ±180°.
                        if (max >= (float)Math.PI * 2 - 0.0001f || min <= 0.0001f - (float)Math.PI * 2)
                        {
                            max = (float)Math.PI;
                            min = -max;
                        }
                        // Axis doesn't seem to be used, so just rotate them to fit.
                        max = -max - (float)Math.PI / 2;
                        min = -min - (float)Math.PI / 2;

                        HardpointDisplayInfo hi = new HardpointDisplayInfo();
                        hi.Matrix = m;
                        hi.Name = node.Name;
                        hi.Node = node;
                        hi.Min = min;
                        hi.Max = max;
                        hi.MeshGroup = MeshGroups[mapFileToMesh[hi.Node.Parent.Parent.Parent.Name]];
                        hi.Revolute = true;
                        hi.Color = new Color(UTFEditorMain.FindHpColor(node.Name).ToRgba());
                        hi.Display = true;
                        otherHardpoints.Add(hi);

                        CreateHardpointRow(hi, hprows);
                    }
                }
                hardpointPanelView.Rows.Clear();
                hardpointPanelView.Rows.AddRange(hprows.ToArray());

                hardpointPanelView.Sort(hardpointPanelView.Columns[1], ListSortDirection.Ascending);
            }

            hardpointPanelView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            viewPanelView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            hardpointPanelView.ResumeDrawing(true);
            viewPanelView.ResumeDrawing(true);
        }

        DataGridViewRow CreateHardpointRow(HardpointDisplayInfo hi, IList rows)
        {
            DataGridViewRow row = new DataGridViewRow();

            row.CreateCells(hardpointPanelView);
            row.Cells[0].Value = true;
            row.Cells[1].Value = hi.Name;
            row.Cells[2].Value = hi.Revolute;
            row.Cells[3].Value = String.Format("#{0:X8}", hi.Color.ToArgb());
            row.Cells[1].Tag = new object[] { hi.MeshGroup, false };

            CreateHardpointPanelMeshGroupRow(rows.Count - 1, hi.MeshGroup, rows);

            rows.Add(row);

            return row;
		}
        
        Matrix GetHardpointMatrix(TreeNode node)
        {
			Matrix m = Matrix.Identity;
			try
			{
				TreeNode n = node.Nodes["Position"];
				byte[] data = n.Tag as byte[];
				int pos = 0;
				m.M41 = Utilities.GetFloat(data, ref pos);
				m.M42 = Utilities.GetFloat(data, ref pos);
				m.M43 = Utilities.GetFloat(data, ref pos);
				m.M44 = 1;
				n = node.Nodes["Orientation"];
				data = n.Tag as byte[];
				pos = 0;
				m.M11 = Utilities.GetFloat(data, ref pos);
				m.M21 = Utilities.GetFloat(data, ref pos);
				m.M31 = Utilities.GetFloat(data, ref pos);
				m.M12 = Utilities.GetFloat(data, ref pos);
				m.M22 = Utilities.GetFloat(data, ref pos);
				m.M32 = Utilities.GetFloat(data, ref pos);
				m.M13 = Utilities.GetFloat(data, ref pos);
				m.M23 = Utilities.GetFloat(data, ref pos);
				m.M33 = Utilities.GetFloat(data, ref pos);
				m.M14 = m.M24 = m.M34 = 0;
			}
			catch (Exception) {}
			return m;
        }

		MeshGroupDisplayInfo GetMeshGroupDisplayInfo(bool def, string name, string level)
        {
			for(int a = 0; a < viewPanelView.Rows.Count; a++)
			{
				DataGridViewTextBoxCell c = (DataGridViewTextBoxCell)viewPanelView[1, a];
				string cellName = c.Value as string;
				int[] cellTag = (int[]) c.Tag;
				if ("Level" + cellTag[0] == level && cellName == name)
				{
					MeshGroupDisplayInfo mgdi = new MeshGroupDisplayInfo();
					mgdi.Display = (bool)((DataGridViewCheckBoxCell)viewPanelView[0, a]).Value;
					mgdi.Shading = (ShadingMode) Enum.Parse(typeof(ShadingMode), ((DataGridViewComboBoxCell)viewPanelView[2, a]).Value as string);
					mgdi.Color = GetColor((string)((DataGridViewTextBoxCell)viewPanelView[3, a]).Value);
					mgdi.Texture = (TextureMode)Enum.Parse(typeof(TextureMode), ((DataGridViewComboBoxCell)viewPanelView[4, a]).Value as string);
					Int32.TryParse(level.Substring(5), out mgdi.Level);
					return mgdi;
				}
			}
			
			MeshGroupDisplayInfo mgdiDef = new MeshGroupDisplayInfo();
			mgdiDef.Display = def;
			mgdiDef.Shading = ShadingMode.Flat;
			mgdiDef.Texture = TextureMode.TextureColor;
			mgdiDef.Color = Color.White;
			Int32.TryParse(level.Substring(5), out mgdiDef.Level);
			
			int rowNum = viewPanelView.Rows.Add();
			CreateModelPanelLevelRow(rowNum, mgdiDef.Level, def);

			viewPanelView[0, rowNum].Value = mgdiDef.Display;
			viewPanelView[1, rowNum].Value = name;
			viewPanelView[2, rowNum].Value = ShadingMode.Flat.ToString();
			viewPanelView[3, rowNum].Value = "#FFFFFFFF";
			viewPanelView[4, rowNum].Value = TextureMode.TextureColor.ToString();
			viewPanelView[1, rowNum].Tag = new int[] { mgdiDef.Level, 0 };
			
			return mgdiDef;
        }
        
        Color GetColor(string text)
        {
			if(text == null) return Color.Black;
			System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^#(?<A>[0-9a-fA-F]{2})?(?<R>[0-9a-fA-F]{2})(?<G>[0-9a-fA-F]{2})(?<B>[0-9a-fA-F]{2})$");
			System.Text.RegularExpressions.Match m = r.Match(text);
			if(m.Success) return new Color(
						Int32.Parse(m.Groups["R"].Value, System.Globalization.NumberStyles.HexNumber),
						Int32.Parse(m.Groups["G"].Value, System.Globalization.NumberStyles.HexNumber),
						Int32.Parse(m.Groups["B"].Value, System.Globalization.NumberStyles.HexNumber),
                        m.Groups["A"].Success ? Int32.Parse(m.Groups["A"].Value, System.Globalization.NumberStyles.HexNumber) : 255
                        );
			
			return new Color(System.Drawing.Color.FromName(text.Replace(" ", "")).ToRgba());
        }
        
        void CreateModelPanelLevelRow(int row, int level, bool def)
        {
			for(int a = row; a > 0; a--)
			{
				int[] data = (int[]) viewPanelView[1, a].Tag;
				if(data == null) continue;
				if(data[0] == level && data[1] == 1) return;
			}
			
			row = viewPanelView.Rows.Add();

			viewPanelView[0, row].Value = def;
			viewPanelView[1, row].Value = "Level" + level;
			viewPanelView[2, row].Value = ShadingMode.Flat.ToString();
			viewPanelView[3, row].Value = "#FFFFFFFF";
			viewPanelView[4, row].Value = TextureMode.TextureColor.ToString();
			viewPanelView[1, row].Tag = new int[] { level, 1 };

			viewPanelView.Rows[row].DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			viewPanelView.Rows[row].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		}

		void CreateHardpointPanelMeshGroupRow(int i, MeshGroup mg, IList rows)
		{
			for (int a = i; a >= 0; a--)
			{
				object[] data = (object[])(rows[a] as DataGridViewRow).Cells[1].Tag;
				if (data == null) continue;
				if ((MeshGroup)data[0] == mg && (bool)data[1]) return;
			}

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(hardpointPanelView);

            row.Cells[0].Value = true;
            row.Cells[1].Value = mg.Name;
            row.Cells[2].Value = false;
            row.Cells[3].Value = "#FFFFFFFF";

            row.Cells[1].Tag = new object[] { mg, true };
            row.Cells[1].ReadOnly = true;
            row.Cells[2].ReadOnly = true;
            row.Cells[2].Style.ForeColor = System.Drawing.Color.DarkGray;
            (row.Cells[2] as DataGridViewCheckBoxCell).FlatStyle = FlatStyle.Flat;

            row.DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlLightLight;

            rows.Add(row);
		}

        /// <summary>
        /// Get the transform matrix for a single part. Recursively load and add 
        /// the transformations for parent parts.
        /// </summary>
        /// <param name="partName">The part name to search for.</param>
        /// <returns>A transform matrix</returns>
        Matrix GetTransform(string partName)
        {
            if (fixData != null)
            {
                foreach (CmpFixData.Part part in fixData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m;
                        m.M11 = part.RotMatXX;
                        m.M12 = part.RotMatYX;
                        m.M13 = part.RotMatZX;
                        m.M14 = 0.0f;
                        m.M21 = part.RotMatXY;
                        m.M22 = part.RotMatYY;
                        m.M23 = part.RotMatZY;
                        m.M24 = 0.0f;
                        m.M31 = part.RotMatXZ;
                        m.M32 = part.RotMatYZ;
                        m.M33 = part.RotMatZZ;
                        m.M34 = 0.0f;
                        m.M41 = part.OriginX;
                        m.M42 = part.OriginY;
                        m.M43 = part.OriginZ;
                        m.M44 = 1.0f;

                        Matrix parentMat;
                        if (!ParentTransform.TryGetValue(part.ParentName, out parentMat))
                        {
                            parentMat = GetTransform(part.ParentName);
                            ParentTransform.Add(part.ParentName, parentMat);
                        }
                        m *= parentMat;
                        return m;
                    }
                }
            }

            if (revData != null)
            {
                foreach (CmpRevData.Part part in revData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m;
                        m.M11 = part.RotMatXX;
                        m.M12 = part.RotMatYX;
                        m.M13 = part.RotMatZX;
                        m.M14 = 0.0f;
                        m.M21 = part.RotMatXY;
                        m.M22 = part.RotMatYY;
                        m.M23 = part.RotMatZY;
                        m.M24 = 0.0f;
                        m.M31 = part.RotMatXZ;
                        m.M32 = part.RotMatYZ;
                        m.M33 = part.RotMatZZ;
                        m.M34 = 0.0f;
                        m.M41 = part.OriginX + part.OffsetX;
                        m.M42 = part.OriginY + part.OffsetY;
                        m.M43 = part.OriginZ + part.OffsetZ;
                        m.M44 = 1.0f;

                        Matrix parentMat;
                        if (!ParentTransform.TryGetValue(part.ParentName, out parentMat))
                        {
                            parentMat = GetTransform(part.ParentName);
                            ParentTransform.Add(part.ParentName, parentMat);
                        }
                        m *= parentMat;
                        return m;
                    }
                }
            }

            if (prisData != null)
            {
                foreach (CmpRevData.Part part in prisData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m;
                        m.M11 = part.RotMatXX;
                        m.M12 = part.RotMatYX;
                        m.M13 = part.RotMatZX;
                        m.M14 = 0.0f;
                        m.M21 = part.RotMatXY;
                        m.M22 = part.RotMatYY;
                        m.M23 = part.RotMatZY;
                        m.M24 = 0.0f;
                        m.M31 = part.RotMatXZ;
                        m.M32 = part.RotMatYZ;
                        m.M33 = part.RotMatZZ;
                        m.M34 = 0.0f;
                        m.M41 = part.OriginX + part.OffsetX;
                        m.M42 = part.OriginY + part.OffsetY;
                        m.M43 = part.OriginZ + part.OffsetZ;
                        m.M44 = 1.0f;

                        Matrix parentMat;
                        if (!ParentTransform.TryGetValue(part.ParentName, out parentMat))
                        {
                            parentMat = GetTransform(part.ParentName);
                            ParentTransform.Add(part.ParentName, parentMat);
                        }
                        m *= parentMat;
                        return m;
                    }
                }
            }

            if (sphereData != null)
            {
                foreach (CmpSphereData.Part part in sphereData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m;
                        m.M11 = part.RotMatXX;
                        m.M12 = part.RotMatYX;
                        m.M13 = part.RotMatZX;
                        m.M14 = 0.0f;
                        m.M21 = part.RotMatXY;
                        m.M22 = part.RotMatYY;
                        m.M23 = part.RotMatZY;
                        m.M24 = 0.0f;
                        m.M31 = part.RotMatXZ;
                        m.M32 = part.RotMatYZ;
                        m.M33 = part.RotMatZZ;
                        m.M34 = 0.0f;
                        m.M41 = part.OriginX + part.OffsetX;
                        m.M42 = part.OriginY + part.OffsetY;
                        m.M43 = part.OriginZ + part.OffsetZ;
                        m.M44 = 1.0f;

                        Matrix parentMat;
                        if (!ParentTransform.TryGetValue(part.ParentName, out parentMat))
                        {
                            parentMat = GetTransform(part.ParentName);
                            ParentTransform.Add(part.ParentName, parentMat);
                        }
                        m *= parentMat;
                        return m;
                    }
                }
            }

            return Matrix.Identity;
        }

        /// <summary>
        /// Build the rotation matrix for the view and the projection matrix.
        /// </summary>
        /// 
        private void SetupMatrices()
        {
            cameraYawPitch.Y = Utilities.Clamp(cameraYawPitch.Y, -(float)Math.PI / 2 * 0.98f, (float)Math.PI / 2 * 0.98f);

            Vector3 pos = Vector3.TransformCoordinate(Vector3.BackwardRH, Matrix.Translation(0, 0, cameraZoom) * Matrix.RotationYawPitchRoll(-cameraYawPitch.X, -cameraYawPitch.Y, 0));
            pos += cameraPosition;

            viewMatrix = Matrix.LookAtRH(pos, cameraPosition, Vector3.Up);
            projMatrix = Matrix.PerspectiveFovRH(45.0f, device.Viewport.Width / (float)device.Viewport.Height, 0.5f, 100000.0f);

            device.SetTransform(TransformState.View, viewMatrix);
            device.SetTransform(TransformState.Projection, projMatrix);
        }

        /// <summary>
        /// Create some lights and brightness based on the world scale.
        /// </summary>
        private void SetupLights()
		{
			device.SetRenderState(RenderState.Lighting, true);
        }

        /// <summary>
        /// Render the image.
        /// </summary>
        private void Render()
        {
            if (device == null || swap == null || swap.IsDisposed || depthStencil == null)
				return;
            
            Surface s = swap.GetBackBuffer(0);
			device.SetRenderTarget(0, s);
			device.DepthStencilSurface = depthStencil;
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, background, 1.0f, 0);
            device.SetRenderState(SharpDX.Direct3D9.RenderState.ZWriteEnable, true);
            device.SetRenderState(SharpDX.Direct3D9.RenderState.ZEnable, true);
            s.Dispose();
            device.BeginScene();

            SetupMatrices();
			SetupLights();
            
            foreach (MeshGroup mg in MeshGroups)
            {
				if (!mg.DisplayInfo.Display) continue;

                device.SetTransform(TransformState.World, mg.Transform);
				
				int endMesh = mg.RefData.StartMesh + mg.RefData.NumMeshes;
				for (int mn = mg.RefData.StartMesh; mn < endMesh; mn++)
				{
					VMeshData.TMeshHeader mesh = mg.MeshDataBuffer.VMeshData.Meshes[mn];

					if (mg.DisplayInfo.Shading == ShadingMode.Flat)
                    {
                        device.SetRenderState(RenderState.CullMode, Cull.Clockwise);
                        device.SetRenderState(RenderState.FillMode, FillMode.Solid);
					}
					else
                    {
                        device.SetRenderState(RenderState.CullMode, Cull.None);
                        device.SetRenderState(RenderState.FillMode, FillMode.Wireframe);
					}

                    device.SetRenderState(RenderState.AlphaBlendEnable, true);
                    device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                    device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
                    device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);

					Texture tex = FindTextureByMaterialID(mesh.MaterialId);

                    Color dc_col = tex != null ? new Color(tex.Dc) : new Color(255);
                    Color final_col = mg.DisplayInfo.Color * dc_col;

                    if (mg.DisplayInfo.Texture == TextureMode.Texture || mg.DisplayInfo.Texture == TextureMode.None || tex == null)
                        device.SetRenderState(RenderState.TextureFactor, final_col.ToRgba());
                    else
                        device.SetRenderState(RenderState.TextureFactor, final_col.ToRgba());

					if (tex != null && (mg.DisplayInfo.Texture == TextureMode.Texture || mg.DisplayInfo.Texture == TextureMode.TextureColor))
					{
						device.SetTexture(0, tex.texture);

                        device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.Modulate);
                        device.SetTextureStageState(0, TextureStage.ColorArg1, TextureArgument.Texture);
                        device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.TFactor);
                        device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.Modulate);
                        device.SetTextureStageState(0, TextureStage.AlphaArg1, TextureArgument.Texture);
                        device.SetTextureStageState(0, TextureStage.AlphaArg2, TextureArgument.TFactor);

                        device.SetSamplerState(0, SamplerState.MipFilter, TextureFilter.Linear);
                        device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
                        device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);
					}
					else
					{
						device.SetTexture(0, null);

                        device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.SelectArg2);
                        device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.TFactor);
                        device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.SelectArg2);
                        device.SetTextureStageState(0, TextureStage.AlphaArg2, TextureArgument.TFactor);
					}

                    /*device.SetTextureStageState(1, TextureStage.ColorOperation, TextureOperation.Modulate);
                    device.SetTextureStageState(1, TextureStage.ColorArg1, TextureArgument.Current);
                    device.SetTextureStageState(1, TextureStage.ColorArg2, TextureArgument.Diffuse);
                    device.SetTextureStageState(1, TextureStage.AlphaOperation, TextureOperation.Modulate);
                    device.SetTextureStageState(1, TextureStage.AlphaArg1, TextureArgument.Current);
                    device.SetTextureStageState(1, TextureStage.AlphaArg2, TextureArgument.Diffuse);*/

                    mg.M[mn - mg.RefData.StartMesh].Draw();
				}
			}

            if (sur != null && surDisplay != SurDisplay.Hidden)
            {
                device.SetRenderState(SharpDX.Direct3D9.RenderState.ZWriteEnable, false);
                device.SetRenderState(SharpDX.Direct3D9.RenderState.ZEnable, true);

                switch (surDisplay)
                {
                    case SurDisplay.Wireframe:
                        device.SetRenderState(SharpDX.Direct3D9.RenderState.CullMode, SharpDX.Direct3D9.Cull.None);
                        device.SetRenderState(SharpDX.Direct3D9.RenderState.FillMode, SharpDX.Direct3D9.FillMode.Wireframe);

                        device.SetRenderState(RenderState.AlphaBlendEnable, false);
                        break;
                    case SurDisplay.Transparent:
                        device.SetRenderState(SharpDX.Direct3D9.RenderState.CullMode, SharpDX.Direct3D9.Cull.Clockwise);
                        device.SetRenderState(SharpDX.Direct3D9.RenderState.FillMode, SharpDX.Direct3D9.FillMode.Solid);

                        device.SetRenderState(RenderState.AlphaBlendEnable, true);
                        device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                        device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                        device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
                        break;
                    case SurDisplay.Opaque:
                        device.SetRenderState(SharpDX.Direct3D9.RenderState.CullMode, SharpDX.Direct3D9.Cull.Clockwise);
                        device.SetRenderState(SharpDX.Direct3D9.RenderState.FillMode, SharpDX.Direct3D9.FillMode.Solid);

                        device.SetRenderState(RenderState.AlphaBlendEnable, false);
                        break;

                }
                sur.RenderSur(device, MeshGroups);

                // Re-render wireframe on top of the transparent SUR
                if(surDisplay == SurDisplay.Transparent)
                {
                    device.SetRenderState(SharpDX.Direct3D9.RenderState.CullMode, SharpDX.Direct3D9.Cull.None);
                    device.SetRenderState(SharpDX.Direct3D9.RenderState.FillMode, SharpDX.Direct3D9.FillMode.Wireframe);

                    device.SetRenderState(RenderState.AlphaBlendEnable, false);

                    sur.RenderSur(device, MeshGroups);
                }
            }
            
            if (sur != null && SurDisplayCenters)
            {
                device.SetRenderState(SharpDX.Direct3D9.RenderState.ZWriteEnable, false);
                device.SetRenderState(SharpDX.Direct3D9.RenderState.ZEnable, false);
                device.SetRenderState(RenderState.AlphaBlendEnable, false);

                Vector3 pos = Vector3.TransformCoordinate(Vector3.BackwardRH, Matrix.Translation(0, 0, cameraZoom) * Matrix.RotationYawPitchRoll(-cameraYawPitch.X, -cameraYawPitch.Y, 0));
                pos += cameraPosition;

                sur.RenderSurCenters(device, MeshGroups, pos);
            }

            ShowHardpoint();

            device.EndScene();
            swap.Present(Present.None);
        }
        
        /// <summary>
        /// Override the form painting event to run the directx render function.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                this.Render();
            }
            catch { }
        }

        private void modelView_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        /// <summary>
        /// Change the scale on mouse wheel turns.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void modelView_MouseWheel(object sender, MouseEventArgs e)
        {
            ChangeScale(e.Delta < 0 ? 1.15f : 1.0f / 1.15f);
        }

        /// <summary>
        /// On mouse down record the current mouse position to prepare for
        /// rotating the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelView_MouseDown(object sender, MouseEventArgs e)
        {
			(sender as Panel).Focus();
            lastPosition = e.Location;
            lastClickTime = DateTime.Now;
		}

		private void modelView_Panel1_MouseUp(object sender, MouseEventArgs e)
		{
			TimeSpan t = DateTime.Now.Subtract(lastClickTime);

            if (t.TotalMilliseconds < 200 && (Control.ModifierKeys & Keys.Control) != Keys.None)
            {
                if(addHps == null || (addHps != null && !addHps.AddingMode))
                {
                    if(GetHardpointNode() != null)
				        PlaceHardpoint(e.X, e.Y);
                }
                else
                {
                    if (GetHardpointNode() != null && addHps.CurrentIsSet)
                        PlaceHardpoint(e.X, e.Y);
                    else
                    {
                        if (MakeHardpoint(e.X, e.Y, addHps.CurrentName, addHps.CurrentRevolute))
                            addHps.HardpointSet();
                    }
                }
            }
		}

        /// <summary>
        /// Rotate the model if the LEFT mouse button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
				HardpointDisplayInfo hi;
				if(GetHardpointFromScreen(e.X, e.Y, out hi))
				{
                    lblHardpointName.Text = hi.Name;
                    lblHardpointName.Location = new System.Drawing.Point(e.X + Cursor.Size.Width / 2, e.Y + Cursor.Size.Height / 2);
                    lblHardpointName.Visible = true;
                }
				else
                    lblHardpointName.Visible = false;
                return;
            }

            int deltaX = e.Location.X - lastPosition.X;
            int deltaY = e.Location.Y - lastPosition.Y;
            if ((e.Button & MouseButtons.Left) != 0)
            {
                // Movement in the left-right direction of the window results 
                // in rotation around the Y axis.
                cameraYawPitch.X += deltaX / 100f;

                // Movement in the top-bottom direction of the window results
                // in rotation around the X axis.
                cameraYawPitch.Y += deltaY / 100f;
            }
            else if ((e.Button & MouseButtons.Right) != 0)
            {
                cameraZoom = (float)Math.Pow(10, Math.Log10(cameraZoom) + deltaY / 100f);
                ChangeScale(1);
            }
            else if((e.Button & MouseButtons.Middle) != 0)
            {
                Matrix view = Matrix.Invert(viewMatrix);

                Vector3 offset = new Vector3(deltaX, -deltaY, 0);
                offset = Vector3.Transform(offset, (Matrix3x3)view);
                cameraPosition -= offset;
            }
            lastPosition = e.Location;
            Invalidate();
        }

        /// <summary>
        /// Redraw the model after changing the solid state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSolid_CheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Stop observing the model when the window closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.DelObserver(this);
            device.Dispose();
            CloseAddHardpoints();
        }

        /// <summary>
        /// Search the mesh buffers for the specified mesh reference.
        /// </summary>
        /// <param name="startVertex"></param>
        /// <returns></returns>
        MeshDataBuffer FindMatchingMeshData(VMeshRef vmr)
        {
            foreach (MeshDataBuffer md in MeshDataBuffers)
            {
                if (md.crc == vmr.VMeshLibId)
                {
                    return md;
                }
            }
            return null;
        }

        /// <summary>
        /// Search the meshs in this model to check if this models uses this
        /// material.
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        private bool IsTextureUsedInModel(uint materialId)
        {
            if (TexRequired.Contains(materialId))
            {
                TexRemaining.Remove(materialId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the texture for the specified materialID.
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        private Texture FindTextureByMaterialID(uint materialID)
        {
            Texture tex;
            if (textures.TryGetValue(materialID, out tex))
                return tex;
            return null;
        }

        class Texture
        {
            public uint matID;
            public string fileName;
            public SharpDX.Direct3D9.Texture texture;
            public int Dc;
        };

        /// <summary>
        /// A dictionary of textures used by this model.
        /// </summary>
        Dictionary<uint, Texture> textures = new Dictionary<uint, Texture>();

        /// <summary>
        /// Look for material information in this utf file.
        /// </summary>
        /// <param name="matRootNode"></param>
        private void LoadTextures(TreeNode matRootNode)
        {
            TreeNode nodeMatLib = matRootNode.Nodes["material library"];
            if (nodeMatLib != null)
            {
                foreach (TreeNode node in nodeMatLib.Nodes)
                {
                    try
                    {
                        string matName = node.Name;
                        uint matID = Utilities.FLModelCRC(matName);

                        if (IsTextureUsedInModel(matID))
                        {
                            Texture tex = new Texture();
                            tex.matID = matID;
                            tex.Dc = Color.White.ToArgb();

                            // Not all textures have files (glass in particular).
                            // This makes them show as black, rather than garbage.
                            try
                            {
                                tex.fileName = Utilities.GetString(node.Nodes["Dt_name"]);
                                tex.texture = MakeTexture(matRootNode, tex.fileName);
							}
							catch { }

							bool Dc_present = false;
							if(node.Nodes.ContainsKey("Dc")) 
							{
                                
                                byte[] Dc = node.Nodes["Dc"].Tag as Byte[];
                                int pos = 0;
                                int r = (int)(Utilities.GetFloat(Dc, ref pos) * 255);
                                int g = (int)(Utilities.GetFloat(Dc, ref pos) * 255);
                                int b = (int)(Utilities.GetFloat(Dc, ref pos) * 255);
                                tex.Dc = (0xFF << 24) + (r << 16) + (g << 8) + (b << 0);
								Dc_present = true;
                            }
                            if (node.Nodes.ContainsKey("Oc"))
                            {
								byte[] Oc = node.Nodes["Oc"].Tag as Byte[];
								int pos = 0;
								int alpha = (int)(Utilities.GetFloat(Oc, ref pos) * 255);
								tex.Dc &= 0xFFFFFF;
								tex.Dc |= alpha << 24;
								Dc_present = true;
							}

							if (tex.fileName == null && Dc_present)
							{
                                tex.texture = new SharpDX.Direct3D9.Texture(device, 1, 1, 1, Usage.None, Format.A8R8G8B8, Pool.Default);
                                tex.texture.Fill((x, s) => Color.White);
							}
                            textures[matID] = tex;
                        }
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Load texture into memory.
        /// </summary>
        /// <param name="matRootNode"></param>
        /// <param name="texFileName"></param>
        /// <returns></returns>
        private SharpDX.Direct3D9.Texture MakeTexture(TreeNode matRootNode, string texFileName)
        {
            TreeNode nodeTexLib = matRootNode.Nodes["texture library"];
            if (nodeTexLib != null)
            {
                foreach (TreeNode node in nodeTexLib.Nodes)
                {
                    if (node.Name == texFileName)
                    {
                        TreeNode mipNode = node.Nodes["MIPS"];
                        if (mipNode == null)
                            mipNode = node.Nodes["MIP0"];
                        if (mipNode != null)
                        {
                            byte[] texture = mipNode.Tag as byte[];
                            if (texture[0] != 'D') // if 'D' assume DDS (DXT), otherwise TGA.
                            {
                                texture = (byte[])texture.Clone();
                                texture[0x11] |= 0x20; // set the origin flag, flipping the texture
                            }
                            return SharpDX.Direct3D9.Texture.FromMemory(device, texture);
                        }
                    }
                }
            }
            return null;
        }

        private void ResetAll()
		{
			viewPanelView.Rows.Clear();

            cameraPosition = Vector3.Zero;
            cameraYawPitch = Vector2.Zero;
            cameraZoom = distance;
            hp.scale = distance / relativeHardpointScale;
            ChangeHardpointSize(1);
            ChangeScale(1);

            SetBackground(false);
            // If the scale is the same, the text box won't change, so explicitly invalidate.
			this.SetupDevice(device);
        }

        private void CenterViewOnHardpoint()
        {
            TreeNode node = utf.SelectedNode;
            try
            {
                if (node.Parent.Text == "Hardpoints")
                {
                    node = rootNode.Nodes.Find(node.Name, true)[0];
                }
                else if (!Utilities.StrIEq(node.Parent.Name, "Fixed", "Revolute"))
                {
                    node = node.Parent;
                    if (!Utilities.StrIEq(node.Parent.Name, "Fixed", "Revolute"))
                        return;
                }
                node = node.Nodes["Position"];
                byte[] data = node.Tag as byte[];
                int pos = 0;
                cameraPosition.X = Utilities.GetFloat(data, ref pos);
                cameraPosition.Y = Utilities.GetFloat(data, ref pos);
                cameraPosition.Z = Utilities.GetFloat(data, ref pos);
                
                MeshGroup mg;
                if (node.Parent.Parent.Parent.Parent.Name=="THN")
                    mg = MeshGroups[0];
                else
                    mg = MeshGroups[mapFileToMesh[node.Parent.Parent.Parent.Parent.Name]];

                cameraPosition = Vector3.TransformCoordinate(cameraPosition, mg.Transform);
                Invalidate();
            }
            catch
            {
                
            }
        }

		private TreeNode GetHardpointNode()
		{
			return GetHardpointNode(utf.SelectedNode);
		}
		
		private TreeNode GetHardpointNode(TreeNode node)
        {
			if(node == null) return null;
			try {
				if (node.Parent?.Text == "Hardpoints" && node.Parent?.Parent == null)
					node = rootNode.Nodes.Find(node.Name, true)[0];
				else if (!Utilities.StrIEq(node.Parent?.Name, "Fixed", "Revolute"))
				{
					node = node.Parent;
					if (!Utilities.StrIEq(node?.Parent?.Name, "Fixed", "Revolute"))
						return null;
				}
				
				return node;
			}
			catch(Exception)
			{
				return null;
			}
        }

        private void ShowHardpoint()
        {
			TreeNode node = GetHardpointNode();
			centerOnHardpointToolStripMenuItem.Enabled = (node != null);
			
			if(otherHardpoints.Count == 0) return;
			
			device.SetTexture(0, null);

            device.SetRenderState(RenderState.Lighting, false);
            device.SetRenderState(RenderState.AlphaBlendEnable, false);
            device.SetRenderState(RenderState.FillMode, FillMode.Solid);
            device.SetRenderState(RenderState.CullMode, Cull.None);
            device.VertexFormat = VertexPositionColor.Format;

            device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.Modulate);
            device.SetTextureStageState(0, TextureStage.ColorArg1, TextureArgument.Diffuse);
            device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.TFactor);
            device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.SelectArg2);
			
			foreach(HardpointDisplayInfo hi in otherHardpoints)
            {
                device.SetRenderState(RenderState.TextureFactor, hi.Color.ToRgba());
                device.SetRenderState(RenderState.BlendFactor, hi.Color.ToRgba());

				try
				{
					bool isSelectedNode = (hi.Node == node);
					if (!isSelectedNode)
						if (splitViewHardpoint.Panel2Collapsed || !hi.Display) continue;
					float scale = hp.scale * (isSelectedNode ? 4f : 1f);
                    device.SetTransform(TransformState.World, Matrix.Scaling(scale, scale, scale) * hi.Matrix * hi.MeshGroup.Transform);

					if (isSelectedNode)
                    {
                        device.SetRenderState(RenderState.TextureFactor, Color.Add(hi.Color, Color.Gray).ToRgba());
                        device.SetRenderState(RenderState.BlendFactor, Color.Add(hi.Color, Color.Gray).ToRgba());
                        
						if (hi.Revolute)
						{
							try
							{
								int pos = 0;
								if (hp.max != hi.Max || hp.min != hi.Min)
								{
									hp.max = hi.Max;
									hp.min = hi.Min;
									
									VertexPositionColor[] rotVert = new VertexPositionColor[26];
									rotVert[0] = new VertexPositionColor(0, 0, 0, 0xffff00);
									pos = 1;
									float delta = (hp.max - hp.min) / 24;
									for (float angle = hp.max; pos < 26; angle -= delta)
										rotVert[pos++] = new VertexPositionColor(2 * (float)Math.Cos(angle), 0, 2 * (float)Math.Sin(angle), 0xffff00);
                                    using (var stream = hp.revolute.Lock(0, 0, LockFlags.None))
                                        stream.WriteRange(rotVert);
                                    hp.revolute.Unlock();
                                }
								device.SetStreamSource(0, hp.revolute, 0, VertexPositionColor.Stride);
								device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 24);
							}
							catch { }
						}
					}
					device.SetStreamSource(0, hp.display, 0, VertexPositionColor.Stride);
					device.Indices = hp.indices;
					if (isSelectedNode)
						device.DrawIndexedPrimitive(PrimitiveType.TriangleList, 0, 0, Hardpoint.displayvertices.Length, 0, Hardpoint.displayindexes.Length / 3);
					else
						device.DrawIndexedPrimitive(PrimitiveType.TriangleList, 0, 0, Hardpoint.displayvertices.Length, 0, 12);
				}
				catch { }
            }
        }

        private void modelView_MouseClick(object sender, MouseEventArgs e)
        {
            modelView.Focus();
        }
        
        private bool GetHitFromScreen(int x, int y, out Vector3 hitLocation, out Vector3 faceNormal, out string nameFinal)
        {
			nameFinal = null;

			float minDist = Single.MaxValue;
			faceNormal = Vector3.Zero;
            Ray rayFinal = new Ray();
			
			hitLocation = Vector3.Zero;

			foreach (MeshGroup mg in MeshGroups)
			{
				if(!mg.DisplayInfo.Display) continue;

                Ray r = Ray.GetPickRay(x, y, new ViewportF(device.Viewport.X, device.Viewport.Y, device.Viewport.Width, device.Viewport.Height, device.Viewport.MinDepth, device.Viewport.MaxDepth), mg.Transform * viewMatrix * projMatrix);

                int mn = 0;
				foreach (var m in mg.M)
				{
                    BoundingBox b = mg.B[mn];
                    //if (b.Intersect(near, far))
                    {
                        IntersectInformation hit;
                        if (m.Intersects(r, out hit) && hit.Dist < minDist)
                        {
                            minDist = hit.Dist;
                            nameFinal = mg.Name;
                            rayFinal = r;

                            VertexPositionNormalTexture[] tempIntersectedVertices = new VertexPositionNormalTexture[3];
                            
                            tempIntersectedVertices[0] = m.Vertices[m.Indices[hit.FaceIndex * 3]];
                            tempIntersectedVertices[1] = m.Vertices[m.Indices[hit.FaceIndex * 3 + 1]];
                            tempIntersectedVertices[2] = m.Vertices[m.Indices[hit.FaceIndex * 3 + 2]];

                            Vector3 v1 = tempIntersectedVertices[1].position - tempIntersectedVertices[0].position;
                            Vector3 v2 = tempIntersectedVertices[2].position - tempIntersectedVertices[0].position;
                            v1.Normalize();
                            v2.Normalize();
                            faceNormal = Vector3.Cross(v1, v2);
                            faceNormal = Vector3.TransformNormal(faceNormal, Matrix.Invert(mg.Transform));
                            faceNormal.Normalize();
                        }
                    }
					mn++;
				}
			}

			if (minDist == Single.MaxValue) return false;
			
			hitLocation = minDist * rayFinal.Direction + rayFinal.Position;

			return true;
        }

        public bool MakeHardpoint(int x, int y, string hpName, bool revolute)
        {
            if (x < 0 || y < 0 || x > device.Viewport.Width || y > device.Viewport.Height) return false;

            Vector3 faceNormal;
            string nameFinal;
            Vector3 loc;
            if (!GetHitFromScreen(x, y, out loc, out faceNormal, out nameFinal)) return false;
            if (nameFinal == null) return false;

            HardpointData hp = new HardpointData(hpName, revolute);

            if (!LinkHardpoint(hp.Node, nameFinal, revolute)) return false;

            HardpointDisplayInfo hi = new HardpointDisplayInfo();
            hi.Matrix = GetHardpointMatrix(hp.Node);
            hi.Name = hp.Name;
            hi.Node = hp.Node;
            hi.MeshGroup = MeshGroups[mapFileToMesh[hi.Node.Parent.Parent.Parent.Name]];
            hi.Color = new Color(UTFEditorMain.FindHpColor(hp.Node.Name).ToRgba());
            hi.Display = true;

            if (revolute)
            {
                TreeNode n = hp.Node.Nodes["Max"];
                float max = BitConverter.ToSingle(n.Tag as byte[], 0);
                n = hp.Node.Nodes["Min"];
                float min = BitConverter.ToSingle(n.Tag as byte[], 0);
                // If max is 360° or min is -360°, set them to ±180°.
                if (max >= (float)Math.PI * 2 - 0.0001f || min <= 0.0001f - (float)Math.PI * 2)
                {
                    max = (float)Math.PI;
                    min = -max;
                }
                // Axis doesn't seem to be used, so just rotate them to fit.
                max = -max - (float)Math.PI / 2;
                min = -min - (float)Math.PI / 2;

                hi.Min = min;
                hi.Max = max;
                hi.Revolute = true;
            }
            else
            {
                hi.Min = hi.Max = 0;
                hi.Revolute = false;
            }

            otherHardpoints.Add(hi);
            DataGridViewRow row = CreateHardpointRow(hi, hardpointPanelView.Rows);
            hardpointPanelView.CurrentCell = row.Cells[0];

            bool made = PlaceHardpoint(x, y, hp, loc, faceNormal, nameFinal);

            if(made) rootNode.TreeView.SelectedNode = hp.Node;

            return made;
        }

        public bool PlaceHardpoint(int x, int y)
        {
            if (x < 0 || y < 0 || x > device.Viewport.Width || y > device.Viewport.Height) return false;

            TreeNode node = GetHardpointNode();
            if (node == null) return false;

            string nameInit = FindGroupName(node);

            Vector3 faceNormal;
            string nameFinal;
            Vector3 loc;
            if (!GetHitFromScreen(x, y, out loc, out faceNormal, out nameFinal)) return false;

            if (nameInit != null && nameFinal != null && nameFinal != nameInit)
            {
                RelinkHardpoint(node, nameFinal);
            }

            HardpointData hpNew = new HardpointData(node);

            return PlaceHardpoint(x, y, hpNew, loc, faceNormal, nameFinal);
        }

        public bool PlaceHardpoint(int x, int y, HardpointData hpNew, Vector3 loc, Vector3 faceNormal, string nameFinal)
        {
			hpNew.PosX = loc.X;
			hpNew.PosY = loc.Y;
			hpNew.PosZ = loc.Z;

			if (Control.ModifierKeys == (Keys.Shift | Keys.Control) || Control.ModifierKeys == (Keys.Control | Keys.Alt))
			{
                Matrix transMat = Matrix.Identity;

                bool alt = (Control.ModifierKeys & Keys.Alt) != Keys.None;
                Vector3 secondVector = Math.Abs(Vector3.Dot(faceNormal, Vector3.Up)) > 0.5f ? Vector3.ForwardRH : Vector3.Up;
                {
                    Matrix3x3 rot = new Matrix3x3();

                    rot.Row1 = alt ? secondVector : faceNormal;
                    rot.Row2 = alt ? faceNormal : secondVector;
                    rot.Row3 = Vector3.Cross(rot.Row1, rot.Row2);
                    rot.Row2 = Vector3.Cross(rot.Row3, rot.Row1);

                    rot.Row1.Normalize();
                    rot.Row2.Normalize();
                    rot.Row3.Normalize();

                    transMat = (Matrix)rot;
                    transMat.TranslationVector = loc;
                    transMat.M14 = transMat.M24 = transMat.M34 = 0;
                    transMat.M44 = 1;
                }

                hpNew.LoadMatrix(transMat);
			}

			hpNew.Write();
			OnHardpointMoved();
            HardpointDisplayInfo hi = null;
            GetHardpointFromName(hpNew.Name, ref hi);
            hi.Matrix = hpNew.ToMatrix();
            try
            {
                hi.MeshGroup = MeshGroups[mapFileToMesh[hi.Node.Parent.Parent.Parent.Name]];
            }
            catch { hi.MeshGroup = MeshGroups[0]; }
			Invalidate();

            utf.SelectedNode = hi.Node;
            foreach (DataGridViewRow row in hardpointPanelView.Rows)
            {
                if ((string)row.Cells[1].Value == hi.Name)
                {
                    hardpointPanelView.CurrentCell = row.Cells[0];
                    break;
                }
            }

            return true;
		}

        private string FindGroupName(TreeNode node)
        {
            string fileName = node.Parent.Parent.Parent.Name;
			string objName;
			if (mapFileToObj.TryGetValue(fileName, out objName))
				return objName;
            return null;
        }

        private void RelinkHardpoint(TreeNode node, string name)
        {
            bool revolute = node.Parent.Text.Equals("Revolute", StringComparison.InvariantCultureIgnoreCase);
            UnlinkHardpoint(node);
            LinkHardpoint(node, name, revolute);
        }

        private bool LinkHardpoint(TreeNode node, string name, bool revolute)
        {
            TreeNode cmpnd = rootNode.Nodes["Cmpnd"];
            if (cmpnd == null)
            {
                cmpnd = rootNode.Nodes["THN"];
                if (cmpnd == null)
                {
                    return false;
                }
            }

			string fileName = null;
            foreach (TreeNode n in cmpnd.Nodes)
            {
				TreeNode objNode = n.Nodes["Object name"];
				if (objNode != null && Utilities.GetString(objNode) == name)
                {
                    fileName = Utilities.GetString(n.Nodes["File name"]);
                    break;
                }
            }
            if (fileName == null) return false;

			foreach (TreeNode n in rootNode.Nodes)
            {
                if (n.Text == fileName)
                {
                    n.TreeView.BeginUpdate();

					TreeNode Hardpoints = n.Nodes["Hardpoints"];
                    if (Hardpoints == null)
                    {
                        Hardpoints = new TreeNode("Hardpoints");
                        Hardpoints.Name = Hardpoints.Text;
                        Hardpoints.Tag = new byte[0];
                        n.Nodes.Add(Hardpoints);
                    }
					TreeNode FixRev = Hardpoints.Nodes[revolute ? "Revolute" : "Fixed"];
                    if (FixRev == null)
                    {
                        FixRev = new TreeNode(revolute ? "Revolute" : "Fixed");
                        FixRev.Name = FixRev.Text;
                        FixRev.Tag = new byte[0];
                        Hardpoints.Nodes.Add(FixRev);
                    }
                    FixRev.Nodes.Add(node);
					n.TreeView.SelectedNode = node;
                    n.TreeView.EndUpdate();
                    return true;
                }
            }

            return false;
        }

        private void UnlinkHardpoint(TreeNode node)
        {
            if (node.Parent.Nodes.Count == 1)
            {
                if (node.Parent.Parent.Nodes.Count == 1)
                    node.Parent.Parent.Remove();
                else
                    node.Parent.Remove();
            }
            else
                node.Remove();
        }

        private void modelView_Panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
				case Keys.Tab:
					return;

				case Keys.Escape:
					Close();
					return;

                case Keys.F1:
                    ShowHelp();
                    break;

                // Bottom view
                case Keys.D1:
                    ResetView(Viewpoint.Defaults.Bottom);
                    break;

                // Top view
                case Keys.D2:
                    ResetView(Viewpoint.Defaults.Top);
                    break;

                // Back view
                case Keys.D3:
                    ResetView(Viewpoint.Defaults.Back);
                    break;

                // Front view
                case Keys.D4:
                    ResetView(Viewpoint.Defaults.Front);
                    break;

                // Right view
                case Keys.D5:
                    ResetView(Viewpoint.Defaults.Right);
                    break;

                // Left view
				case Keys.D6:
					ResetView(Viewpoint.Defaults.Left);
                    break;

                // Toggle background between black and white
                case Keys.B:
                    SetBackground(background == Color.Black);
                    break;

                // Increase the hardpoint display
                case Keys.Multiply:
                    ChangeHardpointSize(1.25f);
                    break;

                // Decrease the hardpoint display
                case Keys.Divide:
                    ChangeHardpointSize(1 / 1.25f);
                    break;

                // Zoom in
                case Keys.Add:
                    ChangeScale(1.25f);
                    break;

                // Zoom out
                case Keys.Subtract:
                    ChangeScale(1 / 1.25f);
                    break;

                // Move up
                case Keys.Up:
                    MoveView(Viewpoint.Move.Up, e.Shift);
                    break;

                // Move down
				case Keys.Down:
					MoveView(Viewpoint.Move.Down, e.Shift);
                    break;

                // Move left
				case Keys.Left:
					MoveView(Viewpoint.Move.Left, e.Shift);
                    break;

                // Move right
				case Keys.Right:
					MoveView(Viewpoint.Move.Right, e.Shift);
                    break;

                // Rotate around the Y axis
                case Keys.PageUp:
                case Keys.PageDown:
                    switch (e.Modifiers & ~Keys.Shift)
                    {
                        case Keys.None:    RotateView(Viewpoint.Rotate.Y, e.KeyCode == Keys.PageUp, e.Shift); break;
						case Keys.Control: RotateView(Viewpoint.Rotate.X, e.KeyCode == Keys.PageUp, e.Shift); break;
                    }
                    break;

                case Keys.F:
                    CenterViewOnHardpoint();
                    break;
                // Reset the view, but keep the origin and scales
                case Keys.Home:
                    ResetPosition();
                    ResetView(Viewpoint.Defaults.Back);
                    break;

				// Toggle the panel.
				case Keys.P:
					modelView.Panel2Collapsed = !modelView.Panel2Collapsed;
					showViewPanelToolStripMenuItem.Checked = modelView.Panel2Collapsed;
					break;
					
				// Toggle hardpoint edit/add mode
				case Keys.Space:
                    if (e.Control)
                        SwitchAddHardpoints();
                    else
                    {
                        if (addHps != null) return;

                        splitViewHardpoint.Panel2Collapsed = !splitViewHardpoint.Panel2Collapsed;
                        editHardpointsToolStripMenuItem1.Checked = splitViewHardpoint.Panel2Collapsed;
                    }
					Invalidate();
                    break;

                // Edit keys (also valid in Add mode)

				case Keys.A:
                    RotateHardpoint(Viewpoint.Rotate.Y, true, e.Shift);
					break;

				case Keys.D:
					RotateHardpoint(Viewpoint.Rotate.Y, false, e.Shift);
					break;

				case Keys.W:
					RotateHardpoint(Viewpoint.Rotate.X, false, e.Shift);
					break;

				case Keys.X:
					RotateHardpoint(Viewpoint.Rotate.X, true, e.Shift);
					break;

				case Keys.Q:
					RotateHardpoint(Viewpoint.Rotate.Z, true, e.Shift);
					break;

				case Keys.E:
					RotateHardpoint(Viewpoint.Rotate.Z, false, e.Shift);
					break;

                case Keys.Z:
                    MinMaxHardpoint((e.Shift ? 1 : 5) * (e.Control ? -1 : 1), 0);
                    break;

                case Keys.C:
                    MinMaxHardpoint(0, (e.Shift ? 1 : 5) * (e.Control ? -1 : 1));
                    break;

                case Keys.NumPad4:
                    MoveHardpoint(new Vector3(-1, 0, 0), e.Shift);
                    break;

                case Keys.NumPad6:
                    MoveHardpoint(new Vector3(1, 0, 0), e.Shift);
                    break;

                case Keys.NumPad2:
                    MoveHardpoint(new Vector3(0, 0, 1), e.Shift);
                    break;

                case Keys.NumPad8:
                    MoveHardpoint(new Vector3(0, 0, -1), e.Shift);
                    break;

                case Keys.NumPad7:
                    MoveHardpoint(new Vector3(0, -1, 0), e.Shift);
                    break;

                case Keys.NumPad9:
                    MoveHardpoint(new Vector3(0, 1, 0), e.Shift);
                    break;

                // Add mode keys

                case Keys.Enter:
                    if (addHps != null && addHps.AddingMode && addHps.CurrentIsSet) addHps.NextHardpoint();
                    break;

                case Keys.N:
                    if (addHps != null && addHps.AddingMode) addHps.ChangeHardpointType(-1);
                    break;

                case Keys.M:
                    if (addHps != null && addHps.AddingMode) addHps.ChangeHardpointType(1);
                    break;

            }
			e.IsInputKey = true;
		}

        private void MoveHardpoint(Vector3 dir, bool fine)
        {
            TreeNode node = GetHardpointNode();
            if (node == null) return;
            HardpointData hpNew = new HardpointData(node);
            HardpointDisplayInfo hi = null;
            GetHardpointFromName(node.Name, ref hi);

            Vector3 p1 = new Vector3(0, 0, 0);
            Vector3 p2 = dir;
            Matrix m = hi.Matrix;

            Vector3.TransformCoordinate(ref p1, ref m, out p1);
            Vector3.TransformCoordinate(ref p2, ref m, out p2);
            dir = p2 - p1;
            dir.Normalize();

            dir *= fine ? 1 : 10;

            hpNew.PosX += dir.X;
            hpNew.PosY += dir.Y;
            hpNew.PosZ += dir.Z;

            hi.Matrix.M41 = hpNew.PosX;
            hi.Matrix.M42 = hpNew.PosY;
            hi.Matrix.M43 = hpNew.PosZ;

            hpNew.Write();
            OnHardpointMoved();

            Invalidate();
        }

        private void RotateHardpoint(Viewpoint.Rotate dir, bool clockwise, bool fine)
        {
            TreeNode node = GetHardpointNode();
            if (node == null) return;
            HardpointData hpNew = new HardpointData(node);
            HardpointDisplayInfo hi = null;
            GetHardpointFromName(node.Name, ref hi);

            Matrix t = Matrix.Identity;

            switch (dir)
            {
                case Viewpoint.Rotate.X:
                    t = Matrix.RotationX((clockwise ? 1 : -1) * (float)Math.PI / (fine ? 180 : 12));
                    break;
                case Viewpoint.Rotate.Y:
                    t = Matrix.RotationY((clockwise ? 1 : -1) * (float)Math.PI / (fine ? 180 : 12));
                    break;
                case Viewpoint.Rotate.Z:
                    t = Matrix.RotationZ((clockwise ? 1 : -1) * (float)Math.PI / (fine ? 180 : 12));
                    break;
            }

            hi.Matrix = t * hi.Matrix;

            hpNew.LoadMatrix(hi.Matrix);

            hpNew.Write();
            OnHardpointMoved();

            Invalidate();
        }

        private void MinMaxHardpoint(int min, int max)
        {

            TreeNode node = GetHardpointNode();
            if (node == null) return;
            if (node.Parent.Name != "Revolute") return;
            HardpointData hpNew = new HardpointData(node);
            HardpointDisplayInfo hi = null;
            GetHardpointFromName(node.Name, ref hi);

            hpNew.Min = (float)Math.Max(-Math.PI, Math.Min(Math.PI, hpNew.Min - min * Math.PI / 180));
            hpNew.Max = (float)Math.Min(Math.PI, Math.Max(-Math.PI, hpNew.Max + max * Math.PI / 180));
            
            if (hpNew.Min > hpNew.Max)
            {
                float t = hpNew.Min;
                hpNew.Min = hpNew.Max;
                hpNew.Max = t;
            }

            hi.Min = hpNew.Min;
            hi.Max = hpNew.Max;

            hpNew.Write();
            OnHardpointMoved();

            Invalidate();
        }
        
        public override void Refresh()
        {
			base.Refresh();
			//Render();
        }
        
        public event EventHandler HardpointMoved;
        
        protected void OnHardpointMoved()
        {
			this.HardpointMoved(this, new EventArgs());
        }
        
        private void ShowHelp()
        {
			new ViewTextForm("Model View Help", Properties.Resources.ModelViewHelp).Show();
        }
        
        private class Viewpoint
        {
			public enum Defaults
			{
				Bottom,
				Top,
				Right,
				Left,
				Front,
				Back
			}
			public enum Move
			{
				Up,
				Down,
				Left,
				Right
			}
			public enum Rotate
			{
				X,
				Y,
				Z
			}
        }
        
        private void ResetView(Viewpoint.Defaults v)
        {
			switch(v)
			{
				case Viewpoint.Defaults.Bottom:
                    cameraYawPitch = new Vector2(-(float)Math.PI / 2, 0);
					break;
				case Viewpoint.Defaults.Top:
                    cameraYawPitch = new Vector2((float)Math.PI / 2, 0);
					break;
				case Viewpoint.Defaults.Back:
                    cameraYawPitch = Vector2.Zero;
					break;
				case Viewpoint.Defaults.Front:
                    cameraYawPitch = new Vector2(0, (float)Math.PI);
					break;
				case Viewpoint.Defaults.Right:
                    cameraYawPitch = new Vector2(0, -(float)Math.PI / 2);
					break;
				case Viewpoint.Defaults.Left:
                    cameraYawPitch = new Vector2(0, (float)Math.PI / 2);
					break;
			}
			Invalidate();
        }

        private void SetBackground(bool white)
        {
            background = white ? Color.White : Color.Black;
            blackToolStripMenuItem.Checked = !white;
            whiteToolStripMenuItem.Checked = white;
            Invalidate();
        }

        private void ChangeHardpointSize(float value)
        {
            hp.scale *= value;
            
            toolStripHardpointSizeSet.Text = (1 / hp.scale).ToString();
            Invalidate();
        }

        private void ChangeScale(float value)
        {
            cameraZoom *= value;
            Invalidate();
        }
        
        private void MoveView(Viewpoint.Move m, bool fine)
        {
            Matrix view = Matrix.Invert(viewMatrix);
            Vector3 offset = Vector3.Zero;

            int mv = fine ? 1 : 10;
			switch(m)
			{
				case Viewpoint.Move.Up:
                    offset.X -= mv;
					break;
				case Viewpoint.Move.Down:
                    offset.Y += mv;
					break;
				case Viewpoint.Move.Left:
                    offset.X += mv;
					break;
				case Viewpoint.Move.Right:
                    offset.X -= mv;
					break;
            }

            offset = Vector3.Transform(offset, (Matrix3x3)view);
            cameraPosition += offset;

            Invalidate();
        }
        
        private void RotateView(Viewpoint.Rotate r, bool clockwise, bool fine)
        {
			float delta = (float)Math.PI / (fine ? 180 : 12);
			if(!clockwise) delta *= -1;
			
			switch(r)
			{
				case Viewpoint.Rotate.X:
					cameraYawPitch.Y += delta;
					break;
				case Viewpoint.Rotate.Y:
                    cameraYawPitch.X -= delta;
					break;
			}
			Invalidate();
        }
        
        private void ResetPosition()
        {
            cameraPosition = Vector3.Zero;
			Invalidate();
        }

		private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetView(Viewpoint.Defaults.Bottom);
		}

		private void topToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetView(Viewpoint.Defaults.Top);
		}

		private void backToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetView(Viewpoint.Defaults.Back);
		}

		private void frontToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetView(Viewpoint.Defaults.Front);
		}

		private void rightToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetView(Viewpoint.Defaults.Right);
		}

		private void leftToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetView(Viewpoint.Defaults.Left);
		}

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBackground(false);
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBackground(true);
        }

        private void decreaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeHardpointSize(1.5f);
        }

        private void increaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeHardpointSize(1 / 1.5f);
        }

        private void toolStripHardpointSizeSet_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float newScale = Single.Parse(toolStripHardpointSizeSet.Text);
                if (newScale <= 0) throw new ArgumentOutOfRangeException();

                hp.scale = 1 / newScale;

				toolStripHardpointSizeSet.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            catch (Exception)
            {
				toolStripHardpointSizeSet.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void inToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeScale(1.25f);
        }

        private void outToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeScale(1 / 1.25f);
        }

        private void leftToolStripMenuItem1_Click(object sender, EventArgs e)
        {
			MoveView(Viewpoint.Move.Left, false);
        }

        private void leftfineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MoveView(Viewpoint.Move.Left, true);
        }
        
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetPosition();
			ResetView(Viewpoint.Defaults.Back);
		}

		private void centerOnHardpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CenterViewOnHardpoint();
		}

		private void shortcutsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowHelp();
		}

		private void resetAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetAll();
		}

		private void viewPanelView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex > viewPanelView.Rows.Count || e.ColumnIndex > viewPanelView.Columns.Count) return;

			DataGridViewCell c = viewPanelView[e.ColumnIndex, e.RowIndex];
			
			string mgName = viewPanelView[1, e.RowIndex].Value as string;
			int[] mgDat = (int[])viewPanelView[1, e.RowIndex].Tag;
			if (mgDat == null || mgName == null) return;
			
			if(mgDat[1] == 1)
			{
				object newVal = viewPanelView[e.ColumnIndex, e.RowIndex].Value;
				this.SuspendLayout();
				viewPanelView.UseWaitCursor = true;
				for(int a = e.RowIndex + 1; a < viewPanelView.Rows.Count; a++)
				{
					int[] mgDat2 = (int[])viewPanelView[1, a].Tag;
					if(mgDat2 != null && mgDat2[1] == 1) break;
					viewPanelView[e.ColumnIndex, a].Value = newVal;
				}
				viewPanelView.UseWaitCursor = false;
				this.ResumeLayout();

				Invalidate();
				return;
			}
			
			
			MeshGroup mg = GetMeshGroupFromName(mgName, mgDat[0]);
			if (mg == null) return;

			switch (e.ColumnIndex)
			{
				case 0:
					mg.DisplayInfo.Display = (bool)c.Value;
					break;
				case 2:
					mg.DisplayInfo.Shading = (ShadingMode)Enum.Parse(typeof(ShadingMode), c.Value as string);
					break;
				case 3:
					mg.DisplayInfo.Color = GetColor((string)c.Value);
					break;
				case 4:
					mg.DisplayInfo.Texture = (TextureMode)Enum.Parse(typeof(TextureMode), c.Value as string);
					break;
			}

			if (!viewPanelView.UseWaitCursor) Invalidate();
		}
		
		private MeshGroup GetMeshGroupFromName(string name, int level)
		{
			foreach(MeshGroup mg in MeshGroups)
				if (mg.DisplayInfo.Level == level && mg.Name == name) return mg;
			return null;
		}

		private void viewPanelView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			viewPanelView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void viewPanelView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			int[] s1 = (int[]) viewPanelView[1, e.RowIndex1].Tag;
			int[] s2 = (int[]) viewPanelView[1, e.RowIndex2].Tag;
			if (s1[0] < s2[0]) e.SortResult = -1;
			else if (s1[0] > s2[0]) e.SortResult = 1;
			else
			{
				if(s1[1] == 1) e.SortResult = -1;
				else if(s2[1] == 1) e.SortResult = 1;
				else
				{
					if (e.CellValue1 as string == "Root") e.SortResult = -1;
					else if (e.CellValue2 as string == "Root") e.SortResult = 1;
					else e.SortResult = ((string)e.CellValue1).CompareTo((string)e.CellValue2);
				} 
			}
			
			e.Handled = true;
		}

		private void showViewPanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			modelView.Panel2Collapsed = !modelView.Panel2Collapsed;
			showViewPanelToolStripMenuItem.Checked = modelView.Panel2Collapsed;
		}

		private void modelView_Panel1_Resize(object sender, EventArgs e)
		{
			if (device == null) return;

			if (swap != null) swap.Dispose();
			if (depthStencil != null) depthStencil.Dispose();

			presentParams.BackBufferWidth = modelView.Panel1.Width;
			presentParams.BackBufferHeight = modelView.Panel1.Height;
			
			if(presentParams.BackBufferWidth == 0 || presentParams.BackBufferHeight == 0) return;
			
			presentParams.DeviceWindowHandle = modelView.Panel1.Handle;
			
			swap = new SwapChain(device, presentParams);
			
			depthStencil = Surface.CreateDepthStencil(device, modelView.Panel1.Width, modelView.Panel1.Height, Format.D24X8, MultisampleType.None, 0, true);
		}

		private void ModelViewForm_Load(object sender, EventArgs e)
		{
			InitializeGraphics();
		}

		private void viewPanelView_DoubleClick(object sender, EventArgs e)
		{
			if(viewPanelView.SelectedCells[0].ColumnIndex == 3)
			{
				colorDiag.Color = System.Drawing.Color.FromArgb(GetColor(viewPanelView.SelectedCells[0].Value as string).ToArgb());
				if(colorDiag.ShowDialog() == DialogResult.OK)
				{
					viewPanelView.SelectedCells[0].Value = String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", colorDiag.Color.A, colorDiag.Color.R, colorDiag.Color.G, colorDiag.Color.B);
					((DataGridViewTextBoxCell)viewPanelView.SelectedCells[0]).ReadOnly = true;
					((DataGridViewTextBoxCell)viewPanelView.SelectedCells[0]).ReadOnly = false;
				}
			}
		}

		private void modelView_Panel1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			HardpointDisplayInfo hi;
            if (GetHardpointFromScreen(e.X, e.Y, out hi))
            {
                utf.SelectedNode = hi.Node;
                foreach (DataGridViewRow row in hardpointPanelView.Rows)
                {
                    if ((string) row.Cells[1].Value == hi.Name)
                    {
                        hardpointPanelView.CurrentCell = row.Cells[0];
                        break;
                    }
                }
            }
		}
		
		private bool GetHardpointFromScreen(int x, int y, out HardpointDisplayInfo foundHardpoint)
		{
			TreeNode selectedHp = GetHardpointNode();
            
            Ray r = Ray.GetPickRay(x, y, new ViewportF(device.Viewport.X, device.Viewport.Y, device.Viewport.Width, device.Viewport.Height, device.Viewport.MinDepth, device.Viewport.MaxDepth), viewMatrix * projMatrix);
			
			foundHardpoint = new HardpointDisplayInfo();
			float minDist = Single.MaxValue;
			foundHardpoint.Node = utf.SelectedNode;
			
			foreach(HardpointDisplayInfo hi in otherHardpoints)
			{
				bool selected = hi.Node == selectedHp;
				if (!selected)
					if (splitViewHardpoint.Panel2Collapsed || !hi.Display) continue;

                Vector3 hpLoc = Vector3.TransformCoordinate(Vector3.Zero, hi.Matrix * hi.MeshGroup.Transform);

                BoundingSphere bs = new BoundingSphere(hpLoc, hp.scale);
                float dist;
                if(r.Intersects(ref bs, out dist) && dist < minDist)
				{
                    minDist = dist;
					foundHardpoint = hi;
				}
			}

			if (minDist == Single.MaxValue)
				return false;
			
			return true;
		}

		private void hardpointPanelView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			object[] s1 = (object[])hardpointPanelView[1, e.RowIndex1].Tag;
			object[] s2 = (object[])hardpointPanelView[1, e.RowIndex2].Tag;
			
			int ssCompare = ((MeshGroup)s1[0]).Name.CompareTo(((MeshGroup)s2[0]).Name);
			
			if (ssCompare != 0) e.SortResult = ssCompare;
			else
			{
				if ((bool)s1[1] == true) e.SortResult = -1;
				else if ((bool)s2[1] == true) e.SortResult = 1;
				else
					e.SortResult = ((string)e.CellValue1).CompareTo((string)e.CellValue2);
			}

			e.Handled = true;
		}

		private void hardpointPanelView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			hardpointPanelView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void hardpointPanelView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex > hardpointPanelView.Rows.Count || e.ColumnIndex > hardpointPanelView.Columns.Count) return;

			DataGridViewCell c = hardpointPanelView[e.ColumnIndex, e.RowIndex];

			string hpName = hardpointPanelView[1, e.RowIndex].Value as string;
			object[] hpDat = (object[])hardpointPanelView[1, e.RowIndex].Tag;
			if (hpDat == null || hpName == null) return;

			if ((bool)hpDat[1])
			{
				object newVal = hardpointPanelView[e.ColumnIndex, e.RowIndex].Value;
				this.SuspendLayout();
				hardpointPanelView.UseWaitCursor = true;
				for (int a = e.RowIndex + 1; a < hardpointPanelView.Rows.Count; a++)
				{
					object[] hpDat2 = (object[])hardpointPanelView[1, a].Tag;
					if (hpDat2 != null && (bool)hpDat2[1]) break;
					hardpointPanelView[e.ColumnIndex, a].Value = newVal;
				}
				hardpointPanelView.UseWaitCursor = false;
				this.ResumeLayout();

				Invalidate();
				return;
			}


            HardpointDisplayInfo hi = null;
            GetHardpointFromName(hpName, ref hi);
			if (hi == null) return;

			switch (e.ColumnIndex)
			{
				case 0:
					hi.Display = (bool)c.Value;
					break;
				case 3:
					hi.Color = GetColor((string)c.Value);
					break;
			}

			if (!hardpointPanelView.UseWaitCursor) Invalidate();
		}
		
		private void GetHardpointFromName(string name, ref HardpointDisplayInfo hdi)
		{
			foreach(HardpointDisplayInfo hi in otherHardpoints)
			{
                if (hi.Name == name)
                {
                    hdi = hi;
                    return;
                }
			}
		}

		private void hardpointPanelView_DoubleClick(object sender, EventArgs e)
		{
			if (hardpointPanelView.SelectedCells[0].ColumnIndex == 3)
			{
				colorDiag.Color = System.Drawing.Color.FromArgb(GetColor(hardpointPanelView.SelectedCells[0].Value as string).ToArgb());
				if (colorDiag.ShowDialog() == DialogResult.OK)
				{
					hardpointPanelView.SelectedCells[0].Value = String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", colorDiag.Color.A, colorDiag.Color.R, colorDiag.Color.G, colorDiag.Color.B);
					((DataGridViewTextBoxCell)hardpointPanelView.SelectedCells[0]).ReadOnly = true;
					((DataGridViewTextBoxCell)hardpointPanelView.SelectedCells[0]).ReadOnly = false;
				}
			}
			else
			{
                HardpointDisplayInfo hi = null;
                GetHardpointFromName(hardpointPanelView[1, hardpointPanelView.SelectedCells[0].RowIndex].Value as string, ref hi);
				if(hi != null)
					utf.SelectedNode = hi.Node;
			}
		}

		private void hardpointEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (addHps != null) CloseAddHardpoints();

			splitViewHardpoint.Panel2Collapsed = !splitViewHardpoint.Panel2Collapsed;
			Invalidate();
            editHardpointsToolStripMenuItem1.Checked = splitViewHardpoint.Panel2Collapsed;
		}

        private void addHardpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchAddHardpoints();
        }

        void addHps_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAddHardpoints(false);
        }

        private void CloseAddHardpoints() { CloseAddHardpoints(true); }
        private void CloseAddHardpoints(bool close)
        {
            if (addHps == null) return;

            if(close) addHps.Close();
            addHps = null;
            splitViewHardpoint.Panel2Collapsed = true;
            Invalidate();
        }

        private void SwitchAddHardpoints()
        {
            if (addHps == null)
            {
                addHps = new AddHardpoints(this.rootNode);
                addHps.FormClosing += new FormClosingEventHandler(addHps_FormClosing);
                addHps.Show();
                splitViewHardpoint.Panel2Collapsed = false;
                Invalidate();
            }
            else
                CloseAddHardpoints();
        }

        private FuseEditor fuse;

        private void fuseComposerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchFuseEditor();
        }

        void fuse_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseFuseEditor(false);
        }

        private void CloseFuseEditor() { CloseFuseEditor(true); }

        private void ResetSurVisibilityCheckboxes()
        {
            hiddenToolStripMenuItem.Checked = surDisplay == SurDisplay.Hidden;
            wireframeToolStripMenuItem.Checked = surDisplay == SurDisplay.Wireframe;
            transparentToolStripMenuItem.Checked = surDisplay == SurDisplay.Transparent;
            opaqueToolStripMenuItem.Checked = surDisplay == SurDisplay.Opaque;
            Refresh();
        }

        private void hiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surDisplay = SurDisplay.Hidden;
            ResetSurVisibilityCheckboxes();
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surDisplay = SurDisplay.Wireframe;
            ResetSurVisibilityCheckboxes();
        }

        private void transparentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surDisplay = SurDisplay.Transparent;
            ResetSurVisibilityCheckboxes();
        }

        private void opaqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surDisplay = SurDisplay.Opaque;
            ResetSurVisibilityCheckboxes();
        }

        private void centersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            centersToolStripMenuItem.Checked = !centersToolStripMenuItem.Checked;
        }

        private void CloseFuseEditor(bool close)
        {
            if (fuse == null) return;

            if (close) fuse.Close();
            fuse = null;
            splitViewHardpoint.Panel2Collapsed = true;
            Invalidate();
        }

        private void SwitchFuseEditor()
        {
            if (fuse == null)
            {
                fuse = new FuseEditor();
                fuse.FormClosing += new FormClosingEventHandler(fuse_FormClosing);
                fuse.Show();
                splitViewHardpoint.Panel2Collapsed = false;
                Invalidate();
            }
            else
                CloseFuseEditor();
        }
    }
}
