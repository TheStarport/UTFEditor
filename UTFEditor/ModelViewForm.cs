using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D = Microsoft.DirectX.Direct3D;
using System.IO;

namespace UTFEditor
{
    public partial class ModelViewForm : Form, UTFFormObserver
    {
		/// <summary>
        /// The directx device.
        /// </summary>
        Device device = null;
        SwapChain swap = null;
        PresentParameters presentParams = null;
        Surface depthStencil = null;
        DepthFormat depthFormat;

        /// <summary>
        /// The model scale.
        /// </summary>
        float scale = 20.0f;
        float distance;

        int background = 0;

        /// <summary>
        /// The last position of the mouse when the model is being rotated.
        /// </summary>
        Point lastPosition;

        /// <summary>
        /// Is the right button rotating or zooming?
        /// </summary>
        enum RightType { RightFirst, RightZoom, RightRotate };
        RightType right = RightType.RightFirst;

        /// <summary>
        /// The current rotation around the Y axis.
        /// </summary>
        float rotY = 0;

        /// <summary>
        /// The current rotation around the X axis.
        /// </summary>
        float rotX = 0;

        /// <summary>
        /// The current rotation around the Z axis.
        /// </summary>
        float rotZ = 0;

        /// The current X offset.
        float posX = 0;

        /// The current Y offset.
        float posY = 0;

        /// The current X/Y/Z origin.
        float orgX = 0, orgY = 0, orgZ = 0;
        
        /// The thn scene origin if we've imported hardpoints from a thn file.
        float thnX = 0, thnY = 0, thnZ = 0;
        
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
            public Mesh[] M;
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
            public CustomVertex.PositionNormalTextured[] V;
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
            static public CustomVertex.PositionColored[] displayvertices =
            {
                // Dummy central vertex
                
                new CustomVertex.PositionColored( 0, 0, 0, 0),
                
                // Central white cube
                
                new CustomVertex.PositionColored( -0.5f, -0.5f,  -0.5f, 0x666666),
                new CustomVertex.PositionColored( -0.5f, 0.5f,  -0.5f, 0x666666),
                new CustomVertex.PositionColored( 0.5f, 0.5f,  -0.5f, 0x888888),
                new CustomVertex.PositionColored( 0.5f, -0.5f,  -0.5f, 0x888888),
                new CustomVertex.PositionColored( -0.5f, -0.5f,  0.5f, 0xaaaaaa),
                new CustomVertex.PositionColored( 0.5f, -0.5f,  0.5f, 0xaaaaaa),
                new CustomVertex.PositionColored( 0.5f, 0.5f,  0.5f, 0xcccccc),
                new CustomVertex.PositionColored( -0.5f, 0.5f,  0.5f, 0xcccccc),
                
                // Y axis
                
                new CustomVertex.PositionColored( -0.125f, -0.25f,  -0.125f, 0x00aa00),
                new CustomVertex.PositionColored( -0.125f, 2.75f,  -0.125f, 0x00dd00),
                new CustomVertex.PositionColored( 0.125f, 2.75f,  -0.125f, 0x00dd00),
                new CustomVertex.PositionColored( 0.125f, -0.25f,  -0.125f, 0x00aa00),
                new CustomVertex.PositionColored( -0.125f, -0.25f,  0.125f, 0x00aa00),
                new CustomVertex.PositionColored( 0.125f, -0.25f,  0.125f, 0x00aa00),
                new CustomVertex.PositionColored( 0.125f, 2.75f,  0.125f, 0x00dd00),
                new CustomVertex.PositionColored( -0.125f, 2.75f,  0.125f, 0x00dd00),
                
                // X axis
                
                new CustomVertex.PositionColored( -0.25f, 0.125f,  -0.125f, 0xaa0000),
                new CustomVertex.PositionColored( 2.75f, 0.125f,  -0.125f, 0xdd0000),
                new CustomVertex.PositionColored( 2.75f, -0.125f,  -0.125f, 0xdd0000),
                new CustomVertex.PositionColored( -0.25f, -0.125f,  -0.125f, 0xaa0000),
                new CustomVertex.PositionColored( -0.25f, 0.125f,  0.125f, 0xaa0000),
                new CustomVertex.PositionColored( -0.25f, -0.125f,  0.125f, 0xaa0000),
                new CustomVertex.PositionColored( 2.75f, -0.125f,  0.125f, 0xdd0000),
                new CustomVertex.PositionColored( 2.75f, 0.125f,  0.125f, 0xdd0000),
                
                // Z axis
                
                new CustomVertex.PositionColored( 0.125f, 0.125f,  0.25f, 0x0000aa),
                new CustomVertex.PositionColored( 0.125f, 0.125f,  -2.75f, 0x0000dd),
                new CustomVertex.PositionColored( 0.125f, -0.125f,  -2.75f, 0x0000dd),
                new CustomVertex.PositionColored( 0.125f, -0.125f,  0.25f, 0x0000aa),
                new CustomVertex.PositionColored( -0.125f, 0.125f,  0.25f, 0x0000aa),
                new CustomVertex.PositionColored( -0.125f, -0.125f,  0.25f, 0x0000aa),
                new CustomVertex.PositionColored( -0.125f, -0.125f,  -2.75f, 0x0000dd),
                new CustomVertex.PositionColored( -0.125f, 0.125f,  -2.75f, 0x0000dd),
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
            this.parent = parent;
            this.utf = utf;
            this.rootNode = utf.Nodes[0];
            this.directoryPath = Path.GetDirectoryName(directoryPath);
            InitializeComponent();
            this.Text += " - " + Path.GetFileName(directoryPath);
            this.MouseWheel += new MouseEventHandler(modelView_MouseWheel);
            parent.AddObserver(this);
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
            presentParams = new PresentParameters();
            presentParams.Windowed = true;
			presentParams.SwapEffect = SwapEffect.Discard;

			DepthFormat[] formats = { DepthFormat.D32, DepthFormat.D24X8, DepthFormat.D16 };
			foreach (DepthFormat format in formats)
			{
				depthFormat = format;
				if (Manager.CheckDepthStencilMatch(0, DeviceType.Hardware, Manager.Adapters.Default.CurrentDisplayMode.Format, Manager.Adapters.Default.CurrentDisplayMode.Format, depthFormat)) break;
			}

			try
			{
				device = new Device(0, DeviceType.Hardware, modelView.Panel1.Handle, CreateFlags.HardwareVertexProcessing, presentParams);
			}
			catch
			{
				device = null;
			}
			if (device == null)
				device = new Device(0, DeviceType.Hardware, modelView.Panel1.Handle, CreateFlags.SoftwareVertexProcessing, presentParams);
            
            if (device == null)
                throw new Exception("Unable to initialise DirectX.");

			this.SetupDevice(device);
			
            scale = (modelView.Panel1.Height - 1) / distance;
            if (scale < 0.001f)
                scale = 0.001f;
            else if (scale > 1000)
                scale = 1000;
            hp.scale = 25 / scale;
            ChangeHardpointSize(1);
			ChangeScale(1);
			
			presentParams.BackBufferWidth = modelView.Panel1.Width;
			presentParams.BackBufferHeight = modelView.Panel1.Height;
			presentParams.DeviceWindowHandle = modelView.Panel1.Handle;

			swap = new SwapChain(device, presentParams);

			depthStencil = device.CreateDepthStencilSurface(modelView.Panel1.Width, modelView.Panel1.Height, depthFormat, MultiSampleType.None, 0, true);
        }

        /// <summary>
        /// Load the model into the vertex and index buffers. Make note of the
        /// positions and rotations of the mesh groups we need to render.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void SetupDevice(Device dev)
        {
			dev.RenderState.ZBufferEnable = true;
			dev.RenderState.DitherEnable = true;

			if (hp.display != null)
			{
				hp.display.Dispose();
				hp.revolute.Dispose();
			}
			hp.display = new VertexBuffer(typeof(CustomVertex.PositionColored), Hardpoint.displayvertices.Length, dev, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
			hp.display.SetData(Hardpoint.displayvertices, 0, LockFlags.None);
			hp.revolute = new VertexBuffer(typeof(CustomVertex.PositionColored), 26, dev, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
			hp.max = Single.MaxValue;
			hp.min = Single.MinValue;

			if (hp.indices != null)
			{
				hp.indices.Dispose();
			}
			hp.indices = new IndexBuffer(device, Hardpoint.displayindexes.Length * sizeof(int), Usage.WriteOnly, Pool.Default, false);
			hp.indices.SetData(Hardpoint.displayindexes, 0, LockFlags.None);

			DataChanged(null, "", null);
        }

        /// <summary>
        /// Parse the treeview and build the directx vertex and index buffers.
        /// </summary>
        public void DataChanged(TreeNode changedNode, string oldName, object oldData)
        {
			foreach (MeshGroup bd in MeshGroups)
            {
				foreach(Mesh m in bd.M)
					m.Dispose();
			}
			MeshGroups.Clear();
            MeshDataBuffers.Clear();
            otherHardpoints.Clear();
            hardpointPanelView.Rows.Clear();
            viewPanelView.Rows.Clear();

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
                List<CustomVertex.PositionNormalTextured> vertices = new List<CustomVertex.PositionNormalTextured>();
                foreach (VMeshData.TVertex vert in md.VMeshData.Vertices)
                {
                    vertices.Add(new CustomVertex.PositionNormalTextured(vert.X, vert.Y, vert.Z, vert.NormalX, vert.NormalY, vert.NormalZ, vert.S, vert.T));
                }

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
                        mg.M = new Mesh[mg.RefData.NumMeshes];
                        mg.B = new BoundingBox[mg.RefData.NumMeshes];
                        mapFileToMesh[fileName] = MeshGroups.Count;

						int endMesh = mg.RefData.StartMesh + mg.RefData.NumMeshes;
						for (int mn = mg.RefData.StartMesh; mn < endMesh; mn++)
						{
							VMeshData.TMeshHeader mesh = mg.MeshDataBuffer.VMeshData.Meshes[mn];
							Mesh m = new Mesh(mesh.NumRefVertices / 3, mg.MeshDataBuffer.V.Length, 0, CustomVertex.PositionNormalTextured.Format, device);
							
							ushort[] indicesCurrent = new ushort[mesh.NumRefVertices];
							for (int a = 0; a < indicesCurrent.Length; a++)
								indicesCurrent[a] = mg.MeshDataBuffer.I[mesh.TriangleStart + a];
							m.SetIndexBufferData(indicesCurrent, LockFlags.None);
							
							CustomVertex.PositionNormalTextured[] verticesCurrent = new CustomVertex.PositionNormalTextured[mesh.EndVertex - mesh.StartVertex + 1];
							for (int a = 0; a < verticesCurrent.Length; a++)
								verticesCurrent[a] = mg.MeshDataBuffer.V[mesh.StartVertex + mg.RefData.StartVert + a];
							m.SetVertexBufferData(verticesCurrent, LockFlags.None);

                            Vector3 min, max;
                            Geometry.ComputeBoundingBox(verticesCurrent, CustomVertex.PositionNormalTextured.StrideSize, out min, out max);
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
                catch {
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
            
            // Load SUR, if available
            try
            {
				//SUR.LoadFromFile(parent.fileName);
            }
            catch(Exception)
            { }

			// Load hardpoints
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
					hi.Color = UTFEditorMain.FindHpColor(node.Name);
					hi.Display = true;
					otherHardpoints.Add(hi);

					CreateHardpointRow(hi);
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
					hi.Color = UTFEditorMain.FindHpColor(node.Name);
					hi.Display = true;
					otherHardpoints.Add(hi);
					
					CreateHardpointRow(hi);
				}
			}

			viewPanelView.Sort(viewPanelView.Columns[1], ListSortDirection.Ascending);
			hardpointPanelView.Sort(hardpointPanelView.Columns[1], ListSortDirection.Ascending);
        }
        
        int CreateHardpointRow(HardpointDisplayInfo hi)
        {
			int row = hardpointPanelView.Rows.Add();
			CreateHardpointPanelMeshGroupRow(row, hi.MeshGroup);

			hardpointPanelView[0, row].Value = true;
			hardpointPanelView[1, row].Value = hi.Name;
			hardpointPanelView[2, row].Value = hi.Revolute;
			hardpointPanelView[3, row].Value = String.Format("#{0:X8}", hi.Color.ToArgb());
			hardpointPanelView[1, row].Tag = new object[] { hi.MeshGroup, false };

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

        Matrix GetHardpointMatrix(HardpointData hp)
        {
            Matrix m = Matrix.Identity;
            m.M41 = hp.PosX;
            m.M42 = hp.PosY;
            m.M43 = hp.PosZ;
            m.M44 = 1;

            m.M11 = hp.RotMatXX;
            m.M12 = hp.RotMatXY;
            m.M13 = hp.RotMatXZ;

            m.M21 = hp.RotMatYX;
            m.M22 = hp.RotMatYY;
            m.M23 = hp.RotMatYZ;

            m.M31 = hp.RotMatZX;
            m.M32 = hp.RotMatZY;
            m.M33 = hp.RotMatZZ;

            m.M14 = m.M24 = m.M34 = 0;

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
			mgdiDef.Color = Color.FromArgb(255, Color.White);
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
			if(m.Success) return Color.FromArgb(
						m.Groups["A"].Success ? Int32.Parse(m.Groups["A"].Value, System.Globalization.NumberStyles.HexNumber) : 255,
						Int32.Parse(m.Groups["R"].Value, System.Globalization.NumberStyles.HexNumber),
						Int32.Parse(m.Groups["G"].Value, System.Globalization.NumberStyles.HexNumber),
						Int32.Parse(m.Groups["B"].Value, System.Globalization.NumberStyles.HexNumber)
						);
			
			return Color.FromName(text.Replace(" ", ""));
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

			viewPanelView.Rows[row].DefaultCellStyle.BackColor = SystemColors.ControlDarkDark;
			viewPanelView.Rows[row].DefaultCellStyle.ForeColor = SystemColors.ControlLightLight;
		}

		void CreateHardpointPanelMeshGroupRow(int row, MeshGroup mg)
		{
			for (int a = row; a >= 0; a--)
			{
				object[] data = (object[])hardpointPanelView[1, a].Tag;
				if (data == null) continue;
				if ((MeshGroup)data[0] == mg && (bool)data[1]) return;
			}

			row = hardpointPanelView.Rows.Add();

			hardpointPanelView[0, row].Value = true;
			hardpointPanelView[1, row].Value = mg.Name;
			hardpointPanelView[2, row].Value = false;
			hardpointPanelView[3, row].Value = "#FFFFFFFF";
			hardpointPanelView[1, row].Tag = new object[] { mg, true };

			hardpointPanelView.Rows[row].DefaultCellStyle.BackColor = SystemColors.ControlDarkDark;
			hardpointPanelView.Rows[row].DefaultCellStyle.ForeColor = SystemColors.ControlLightLight;
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
                        m.Multiply(parentMat);
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
                        m.Multiply(parentMat);
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
                        m.Multiply(parentMat);
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
                        m.Multiply(parentMat);
                        return m;
                    }
                }
            }

            return Matrix.Identity;
        }

        /// <summary>
        /// Build the rotation matrix for the view and the projection matrix.
        /// </summary>
        private void SetupMatrices()
        {
            Matrix rotation = Matrix.RotationZ(rotZ);
            rotation.Multiply(Matrix.RotationY(rotY));
            rotation.Multiply(Matrix.RotationX(rotX));
            rotation.Multiply(Matrix.Scaling(scale, scale, scale));
            rotation.Multiply(Matrix.Translation(posX, posY, 0));
            device.Transform.View = rotation;
            device.Transform.Projection = Matrix.OrthoRH(modelView.Panel1.Width, modelView.Panel1.Height, -200000.0f, 200000.0f);
        }

        /// <summary>
        /// Create some lights and brightness based on the world scale.
        /// </summary>
        private void SetupLights()
		{
			device.RenderState.Lighting = true;
			/*device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = Color.Blue;
			//device.Lights[0].DiffuseColor = new ColorValue(scale, scale, scale);
			device.Lights[0].Direction = new Vector3(1.0f, 1.0f, 1.0f);
			device.Lights[0].Update();
			device.Lights[0].Enabled = true;
			device.RenderState.Ambient = Color.Gray;
			Material mtrl = new Material();
			mtrl.Ambient = Color.Black;
			mtrl.Diffuse = Color.White;
			device.Material = mtrl;
			
			/*
			device.RenderState.DiffuseMaterialSource = ColorSource.Material;
			device.RenderState.SpecularMaterialSource = ColorSource.Material;
			Material mtrl = new Material();
			mtrl.Ambient = Color.Black;
			mtrl.Diffuse = Color.White;
			device.Material = mtrl;
			
			/*device.RenderState.Lighting = true;
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = Color.Red;
			device.Lights[0].DiffuseColor = new ColorValue(scale, scale, scale);
			device.Lights[0].Direction = new Vector3(1.0f, 1.0f, 1.0f);
			device.Lights[0].Enabled = true;
			/*Material mtrl = new Material();
			mtrl.Ambient = Color.Red;//mgdi.Color;
			device.Material = mtrl;
			
			device.RenderState.Ambient = Color.Red;
			device.RenderState.Lighting = true;
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = Color.Red;
			device.Lights[0].DiffuseColor = new ColorValue(scale, scale, scale);
			device.Lights[0].Direction = new Vector3(1.0f, 1.0f, 1.0f);
			device.Lights[0].Enabled = true;*/
			/*Material mtrl = new Material();
			mtrl.Ambient = Color.Red;//mgdi.Color;
			device.Material = mtrl;
			device.RenderState.Lighting = false;
			device.RenderState.Ambient = Color.Red;*/

			/*device.RenderState.DiffuseMaterialSource = ColorSource.Color1;
			device.RenderState.EmissiveMaterialSource = ColorSource.Color1;
			device.RenderState.AmbientMaterialSource = ColorSource.Color1;

			device.RenderState.Lighting = true;
            device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = mgdi.Color;
            device.Lights[0].DiffuseColor = new ColorValue(scale, scale, scale);
            device.Lights[0].Direction = new Vector3(1.0f, 1.0f, 1.0f);

            device.Lights[1].Type = LightType.Directional;
			device.Lights[1].Diffuse = mgdi.Color;
            device.Lights[1].DiffuseColor = new ColorValue(scale, scale, scale);
            device.Lights[1].Direction = new Vector3(-1.0f, -1.0f, -1.0f);

			if (mgdi.Texture && hasTexture)
			{
				device.RenderState.Lighting = true;
				device.Lights[0].Enabled = true;
				device.Lights[1].Enabled = true;
                device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                device.RenderState.SpecularMaterialSource = ColorSource.Material;
            }
            else
            {
                device.RenderState.Lighting = false;
                device.RenderState.Ambient = mgdi.Color;
                device.Lights[0].Enabled = false;
				device.Lights[1].Enabled = false;
                device.RenderState.DiffuseMaterialSource = ColorSource.Material;
				device.RenderState.SpecularMaterialSource = ColorSource.Material;
            }

            device.RenderState.LocalViewer = true;
            device.RenderState.SpecularEnable = true;
            device.RenderState.DitherEnable = true;
            device.RenderState.NormalizeNormals = false;*/
        }

        /// <summary>
        /// Render the image.
        /// </summary>
        private void Render()
        {
            if (device == null || swap == null || swap.Disposed || depthStencil == null)
				return;
            
            Surface s = swap.GetBackBuffer(0, BackBufferType.Mono);
			device.SetRenderTarget(0, s);
			device.DepthStencilSurface = depthStencil;
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, background, 1.0f, 0);
			s.Dispose();
            device.BeginScene();

            SetupMatrices();
			SetupLights();
            
            foreach (MeshGroup mg in MeshGroups)
            {
				if (!mg.DisplayInfo.Display) continue;
				
				device.Transform.World = mg.Transform * Matrix.Translation(orgX, orgY, orgZ);
				
				int endMesh = mg.RefData.StartMesh + mg.RefData.NumMeshes;
				for (int mn = mg.RefData.StartMesh; mn < endMesh; mn++)
				{
					VMeshData.TMeshHeader mesh = mg.MeshDataBuffer.VMeshData.Meshes[mn];

					if (mg.DisplayInfo.Shading == ShadingMode.Flat)
					{
						device.RenderState.CullMode = Cull.Clockwise;
						device.RenderState.FillMode = FillMode.Solid;
					}
					else
					{
						device.RenderState.CullMode = Cull.None;
						device.RenderState.FillMode = FillMode.WireFrame;
					}
					
					device.RenderState.AlphaBlendEnable = true;
					device.RenderState.SourceBlend = Blend.SourceAlpha;
					device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
					device.RenderState.BlendOperation = BlendOperation.Add;

					Texture tex = FindTextureByMaterialID(mesh.MaterialId);
					if(mg.DisplayInfo.Texture == TextureMode.Texture || mg.DisplayInfo.Texture == TextureMode.None || tex == null)
						device.RenderState.TextureFactor = mg.DisplayInfo.Color.ToArgb();
					else
						device.RenderState.TextureFactor = ColorOperator.Modulate(mg.DisplayInfo.Color, Color.FromArgb(tex.Dc)).ToArgb();

					if (tex != null && (mg.DisplayInfo.Texture == TextureMode.Texture || mg.DisplayInfo.Texture == TextureMode.TextureColor))
					{
						device.SetTexture(0, tex.texture);
						
						device.TextureState[0].ColorOperation = TextureOperation.Modulate;
						device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
						device.TextureState[0].ColorArgument2 = TextureArgument.TFactor;
						device.TextureState[0].AlphaOperation = TextureOperation.Modulate;
						device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
						device.TextureState[0].AlphaArgument2 = TextureArgument.TFactor;

						device.SamplerState[0].MipFilter = TextureFilter.Linear;
						device.SamplerState[0].MinFilter = TextureFilter.Linear;
						device.SamplerState[0].MagFilter = TextureFilter.Linear;
					}
					else
					{
						device.SetTexture(0, null);

						device.TextureState[0].ColorOperation = TextureOperation.SelectArg2;
						device.TextureState[0].ColorArgument2 = TextureArgument.TFactor;
						device.TextureState[0].AlphaOperation = TextureOperation.SelectArg2;
						device.TextureState[0].AlphaArgument2 = TextureArgument.TFactor;
					}

					mg.M[mn - mg.RefData.StartMesh].DrawSubset(0);
				}
			}

            ShowHardpoint();

            device.EndScene();
            swap.Present();
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
            // Adjust the scale.
            if (e.Delta < 0)
                scale *= 1.25f;
            else
                scale /= 1.25f;

            ChangeScale(1);
        }
        
        /// <summary>
        /// If the scale track bar is updated then re-scale the display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarScale_Scroll(object sender, EventArgs e)
        {
            scale = (float)Math.Pow(10, (double)trackBarScale.Value / 100.0);
            ChangeScale(1);
        }

        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            float s;
            if (Single.TryParse(textBoxScale.Text, out s) &&
                s >= 0.001 && s <= 1000)
            {
                scale = s;
                // Map the scale onto the trackbar.
                this.trackBarScale.Scroll -= new System.EventHandler(this.trackBarScale_Scroll);
                trackBarScale.Value = (int)(Math.Log10(scale) * 100);
                this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
                textBoxScale.ForeColor = SystemColors.WindowText;
                Invalidate();
            }
            else textBoxScale.ForeColor = Color.Red;
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
            right = RightType.RightFirst;
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
					if(hardpointNameToolTip.GetToolTip(modelView) != hi.Name)
						hardpointNameToolTip.Show(hi.Name, modelView, e.X + 5, e.Y + 5);
				}
				else
					hardpointNameToolTip.Hide(modelView);
                return;
            }

            int deltaX = e.Location.X - lastPosition.X;
            int deltaY = e.Location.Y - lastPosition.Y;
            if ((e.Button & MouseButtons.Left) != 0)
            {
                // If the shift key is down or the right button is pressed then move.
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift ||
                    (e.Button & MouseButtons.Right) != 0)
                {
                    posX += deltaX;
                    posY -= deltaY;
                }
                // Otherwise rotate.
                else
                {
                    // Movement in the left-right direction of the window results 
                    // in rotation around the Y axis.
                    rotY += deltaX / 100f;

                    // Movement in the top-bottom direction of the window results
                    // in rotation around the X axis.
                    rotX += deltaY / 100f;
                }
            }
            else if ((e.Button & MouseButtons.Right) != 0)
            {
                if (right == RightType.RightFirst)
                    right = (deltaX != 0) ? RightType.RightRotate : RightType.RightZoom;
                switch (right)
                {
                    case RightType.RightZoom:
                        scale = (float)Math.Pow(10, Math.Log10(scale) + deltaY / 100f);
                        ChangeScale(1);
                        break;

                    case RightType.RightRotate:
                        // Movement in the left-right direction of the window results
                        // in rotation around the Z axis.
                        rotZ -= deltaX / 100f;
                        break;
                }
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
            public Direct3D.Texture texture;
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
							try 
							{
                                byte[] Dc = node.Nodes["Dc"].Tag as Byte[];
                                int pos = 0;
                                int r = (int)(Utilities.GetFloat(Dc, ref pos) * 255);
                                int g = (int)(Utilities.GetFloat(Dc, ref pos) * 255);
                                int b = (int)(Utilities.GetFloat(Dc, ref pos) * 255);
                                tex.Dc = (0xFF << 24) + (r << 16) + (g << 8) + (b << 0);
								Dc_present = true;
                            }
                            catch { }
							try
							{
								byte[] Oc = node.Nodes["Oc"].Tag as Byte[];
								int pos = 0;
								int alpha = (int)(Utilities.GetFloat(Oc, ref pos) * 255);
								tex.Dc &= 0xFFFFFF;
								tex.Dc |= alpha << 24;
								Dc_present = true;
							}
							catch { }
							if (tex.fileName == null && Dc_present)
							{
								Bitmap bmp = new Bitmap(1, 1);
								bmp.SetPixel(0, 0, Color.White);
								tex.texture = Direct3D.Texture.FromBitmap(device, bmp, Usage.Dynamic, Pool.Default);
							}
							textures.Add(matID, tex);
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
        private Direct3D.Texture MakeTexture(TreeNode matRootNode, string texFileName)
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
                            using (MemoryStream ms = new MemoryStream(texture))
                            {
                                return TextureLoader.FromStream(device, ms);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void ResetAll()
		{
			viewPanelView.Rows.Clear();
			
            rotX = rotY = rotZ = 0;
            posX = posY = 0;
            orgX = orgY = orgZ = 0;
            scale = (modelView.Panel1.Height - 1) / distance;
            if (scale < 0.001f)
                scale = 0.001f;
            else if (scale > 1000)
                scale = 1000;
            ChangeScale(1);
            hp.scale = 25 / scale;
            ChangeHardpointSize(1);

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
                orgX = Utilities.GetFloat(data, ref pos);
                orgY = Utilities.GetFloat(data, ref pos);
                orgZ = Utilities.GetFloat(data, ref pos);
                
                MeshGroup mg;
                if (node.Parent.Parent.Parent.Parent.Name=="THN")
                    mg = MeshGroups[0];
                else
                    mg = MeshGroups[mapFileToMesh[node.Parent.Parent.Parent.Parent.Name]];
                
                Matrix m = Matrix.Multiply(mg.Transform, Matrix.Translation(orgX, orgY, orgZ));
                orgX = -m.M41;
                orgY = -m.M42;
                orgZ = -m.M43;
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
				if (node.Parent.Text == "Hardpoints" && node.Parent.Parent == null)
					node = rootNode.Nodes.Find(node.Name, true)[0];
				else if (!Utilities.StrIEq(node.Parent.Name, "Fixed", "Revolute"))
				{
					node = node.Parent;
					if (!Utilities.StrIEq(node.Parent.Name, "Fixed", "Revolute"))
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
			
			device.RenderState.Lighting = false;
			device.RenderState.AlphaBlendEnable = true;
			device.RenderState.FillMode = FillMode.Solid;
			device.RenderState.CullMode = Cull.None;
			device.VertexFormat = CustomVertex.PositionColored.Format;
			device.TextureState[0].ColorOperation = TextureOperation.Modulate;
			device.TextureState[0].ColorArgument1 = TextureArgument.Diffuse;
			device.TextureState[0].ColorArgument2 = TextureArgument.TFactor;
			device.TextureState[0].AlphaOperation = TextureOperation.SelectArg2;
			
			foreach(HardpointDisplayInfo hi in otherHardpoints)
			{
				device.RenderState.TextureFactor = hi.Color.ToArgb();
				device.RenderState.BlendFactor = hi.Color;
				try
				{
					bool isSelectedNode = (hi.Node == node);
					if (!isSelectedNode)
						if (splitViewHardpoint.Panel2Collapsed || !hi.Display) continue;
					float scale = hp.scale * (isSelectedNode ? 4f : 1f);
                    device.Transform.World = Matrix.Scaling(scale, scale, scale) * hi.Matrix * hi.MeshGroup.Transform * Matrix.Translation(orgX, orgY, orgZ) * Matrix.Translation(thnX, thnY, thnZ);

					if (isSelectedNode)
					{
						device.RenderState.TextureFactor = ColorOperator.Add(hi.Color, Color.Gray).ToArgb();
						device.RenderState.BlendFactor = ColorOperator.Add(hi.Color, Color.Gray);
						if (hi.Revolute)
						{
							try
							{
								int pos = 0;
								if (hp.max != hi.Max || hp.min != hi.Min)
								{
									hp.max = hi.Max;
									hp.min = hi.Min;
									
									CustomVertex.PositionColored[] rotVert = new CustomVertex.PositionColored[26];
									rotVert[0] = new CustomVertex.PositionColored(0, 0, 0, 0xffff00);
									pos = 1;
									float delta = (hp.max - hp.min) / 24;
									for (float angle = hp.max; pos < 26; angle -= delta)
										rotVert[pos++] = new CustomVertex.PositionColored(2 * (float)Math.Cos(angle), 0, 2 * (float)Math.Sin(angle), 0xffff00);
									hp.revolute.SetData(rotVert, 0, LockFlags.None);
								}
								device.SetStreamSource(0, hp.revolute, 0);
								device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 24);
							}
							catch { }
						}
					}
					device.SetStreamSource(0, hp.display, 0);
					device.Indices = hp.indices;
					if (isSelectedNode)
						device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Hardpoint.displayvertices.Length, 0, Hardpoint.displayindexes.Length / 3);
					else
						device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Hardpoint.displayvertices.Length, 0, 12);
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
			Vector3 nearFinal = new Vector3(x, y, 0);
			Vector3 farFinal = new Vector3(x, y, 1);

			Vector3 nearInit = new Vector3(x, y, 0);
			Vector3 farInit = new Vector3(x, y, 1);
			nameFinal = null;

			float minDist = Single.MaxValue;
			faceNormal = Vector3.Empty;
			
			hitLocation = Vector3.Empty;

			foreach (MeshGroup mg in MeshGroups)
			{
				if(!mg.DisplayInfo.Display) continue;
				
				Vector3 near = new Vector3(nearInit.X, nearInit.Y, nearInit.Z);
				Vector3 far = new Vector3(farInit.X, farInit.Y, farInit.Z);
				near.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, mg.Transform * Matrix.Translation(orgX, orgY, orgZ));
				far.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, mg.Transform * Matrix.Translation(orgX, orgY, orgZ));

				int mn = 0;
				foreach (Mesh m in mg.M)
				{
                    BoundingBox b = mg.B[mn];
                    if (b.Intersect(near, far))
                    {
                        IntersectInformation hit;
                        if (m.Intersect(near, far - near, out hit) && hit.Dist < minDist)
                        {
                            minDist = hit.Dist;
                            farFinal = far;
                            nearFinal = near;
                            nameFinal = mg.Name;

                            ushort[] intersectedIndices = new ushort[3];

                            ushort[] indices = (ushort[])m.LockIndexBuffer(typeof(ushort), LockFlags.ReadOnly, m.NumberFaces * 3);
                            Array.Copy(indices, hit.FaceIndex * 3, intersectedIndices, 0, 3);
                            m.UnlockIndexBuffer();

                            CustomVertex.PositionNormalTextured[] tempIntersectedVertices = new CustomVertex.PositionNormalTextured[3];

                            CustomVertex.PositionNormalTextured[] meshVertices =
                                (CustomVertex.PositionNormalTextured[])m.LockVertexBuffer(typeof(CustomVertex.PositionNormalTextured), LockFlags.ReadOnly, m.NumberVertices);
                            tempIntersectedVertices[0] = meshVertices[intersectedIndices[0]];
                            tempIntersectedVertices[1] = meshVertices[intersectedIndices[1]];
                            tempIntersectedVertices[2] = meshVertices[intersectedIndices[2]];
                            m.UnlockVertexBuffer();

                            Vector3 v1 = tempIntersectedVertices[1].Position - tempIntersectedVertices[0].Position;
                            Vector3 v2 = tempIntersectedVertices[2].Position - tempIntersectedVertices[0].Position;
                            faceNormal = Vector3.Cross(v1, v2);
                            faceNormal.Normalize();
                        }
                    }
					mn++;
				}
			}

			if (minDist == Single.MaxValue) return false;
			
			hitLocation = minDist * (farFinal - nearFinal) + nearFinal;
            hitLocation.X -= thnX;
            hitLocation.Y -= thnY;
            hitLocation.Z -= thnZ;


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

            //DataChanged(null, "", null);

            HardpointDisplayInfo hi = new HardpointDisplayInfo();
            hi.Matrix = GetHardpointMatrix(hp.Node);
            hi.Name = hp.Name;
            hi.Node = hp.Node;
            hi.MeshGroup = MeshGroups[mapFileToMesh[hi.Node.Parent.Parent.Parent.Name]];
            hi.Color = UTFEditorMain.FindHpColor(hp.Node.Name);
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
            int row = CreateHardpointRow(hi);
            hardpointPanelView.CurrentCell = hardpointPanelView.Rows[row].Cells[0];

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
				Matrix transMat = transMat = Matrix.LookAtRH(new Vector3(0, 0, 0), faceNormal, new Vector3(0, 1, 0));
				if (transMat.Determinant == 0)
					transMat = transMat = Matrix.LookAtRH(new Vector3(0, 0, 0), faceNormal, new Vector3(0, 0, 1));
				if ((Control.ModifierKeys & Keys.Alt) != Keys.None)
					transMat *= Matrix.RotationX((float)Math.PI / 2);
				hpNew.RotMatXX = transMat.M11;
				hpNew.RotMatXY = transMat.M12;
				hpNew.RotMatXZ = transMat.M13;

				hpNew.RotMatYX = transMat.M21;
				hpNew.RotMatYY = transMat.M22;
				hpNew.RotMatYZ = transMat.M23;

				hpNew.RotMatZX = transMat.M31;
				hpNew.RotMatZY = transMat.M32;
				hpNew.RotMatZZ = transMat.M33;
			}

			hpNew.Write();
			OnHardpointMoved();
            HardpointDisplayInfo hi = null;
            GetHardpointFromName(hpNew.Name, ref hi);
            hi.Matrix = GetHardpointMatrix(hpNew);
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
                    SetBackground(background == 0);
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
                        case Keys.Alt:     RotateView(Viewpoint.Rotate.Z, e.KeyCode == Keys.PageUp, e.Shift); break;
                    }
                    break;

                // Reset the view, but keep the origin and scales
                case Keys.Home:
					if(e.Shift)
					{
						CenterViewOnHardpoint();
					}
					else
					{
						ResetPosition();
						ResetView(Viewpoint.Defaults.Back);
                    }
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

            p1.TransformCoordinate(m);
            p2.TransformCoordinate(m);
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
                    t.RotateX((clockwise ? 1 : -1) * (float)Math.PI / (fine ? 180 : 12));
                    break;
                case Viewpoint.Rotate.Y:
                    t.RotateY((clockwise ? 1 : -1) * (float)Math.PI / (fine ? 180 : 12));
                    break;
                case Viewpoint.Rotate.Z:
                    t.RotateZ((clockwise ? 1 : -1) * (float)Math.PI / (fine ? 180 : 12));
                    break;
            }

            hi.Matrix = Matrix.Multiply(t, hi.Matrix);

            hpNew.RotMatXX = hi.Matrix.M11;
            hpNew.RotMatXY = hi.Matrix.M12;
            hpNew.RotMatXZ = hi.Matrix.M13;

            hpNew.RotMatYX = hi.Matrix.M21;
            hpNew.RotMatYY = hi.Matrix.M22;
            hpNew.RotMatYZ = hi.Matrix.M23;

            hpNew.RotMatZX = hi.Matrix.M31;
            hpNew.RotMatZY = hi.Matrix.M32;
            hpNew.RotMatZZ = hi.Matrix.M33;

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

            hpNew.Min = (float)Math.Max(-Math.PI, Math.Min(0, hpNew.Min - min * Math.PI / 180));
            hpNew.Max = (float)Math.Min(Math.PI, Math.Max(0, hpNew.Max + max * Math.PI / 180));
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
					rotY = rotZ = 0;
					rotX = -(float)Math.PI / 2;
					break;
				case Viewpoint.Defaults.Top:
					rotY = rotZ = 0;
					rotX = (float)Math.PI / 2;
					break;
				case Viewpoint.Defaults.Back:
					rotX = rotY = rotZ = 0;
					break;
				case Viewpoint.Defaults.Front:
					rotX = rotZ = 0;
					rotY = (float)Math.PI;
					break;
				case Viewpoint.Defaults.Right:
					rotX = rotZ = 0;
					rotY = -(float)Math.PI / 2;
					break;
				case Viewpoint.Defaults.Left:
					rotX = rotZ = 0;
					rotY = (float)Math.PI / 2;
					break;
			}
			Invalidate();
        }

        private void SetBackground(bool white)
        {
            background = white ? 0xFFFFFF : 0;
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
            scale *= value;
            if (scale > 1000)
                scale = 1000;
            if (scale < 0.001f)
                scale = 0.001f;
            textBoxScale.Text = scale.ToString("0.###");
            Invalidate();
        }
        
        private void MoveView(Viewpoint.Move m, bool fine)
        {
			int mv = fine ? 1 : 10;
			switch(m)
			{
				case Viewpoint.Move.Up:
					posY -= mv;
					break;
				case Viewpoint.Move.Down:
					posY += mv;
					break;
				case Viewpoint.Move.Left:
					posX += mv;
					break;
				case Viewpoint.Move.Right:
					posX -= mv;
					break;
			}
			Invalidate();
        }
        
        private void RotateView(Viewpoint.Rotate r, bool clockwise, bool fine)
        {
			float delta = (float)Math.PI / (fine ? 180 : 12);
			if(!clockwise) delta *= -1;
			
			switch(r)
			{
				case Viewpoint.Rotate.X:
					rotX += delta;
					break;
				case Viewpoint.Rotate.Y:
					rotY += delta;
					break;
				case Viewpoint.Rotate.Z:
					rotZ += delta;
					break;
			}
			Invalidate();
        }
        
        private void ResetPosition()
        {
			posX = posY = 0;
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

				toolStripHardpointSizeSet.ForeColor = SystemColors.WindowText;
            }
            catch (Exception)
            {
				toolStripHardpointSizeSet.ForeColor = Color.Red;
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
			if (device == null || presentParams == null) return;

			if (swap != null) swap.Dispose();
			if (depthStencil != null) depthStencil.Dispose();

			presentParams.BackBufferWidth = modelView.Panel1.Width;
			presentParams.BackBufferHeight = modelView.Panel1.Height;
			
			if(presentParams.BackBufferWidth == 0 || presentParams.BackBufferHeight == 0) return;
			
			presentParams.DeviceWindowHandle = modelView.Panel1.Handle;
			
			swap = new SwapChain(device, presentParams);
			
			depthStencil = device.CreateDepthStencilSurface(modelView.Panel1.Width, modelView.Panel1.Height, DepthFormat.D24X8, MultiSampleType.None, 0, true);
		}

		private void ModelViewForm_Load(object sender, EventArgs e)
		{
			InitializeGraphics();
		}

		private void viewPanelView_DoubleClick(object sender, EventArgs e)
		{
			if(viewPanelView.SelectedCells[0].ColumnIndex == 3)
			{
				colorDiag.Color = GetColor(viewPanelView.SelectedCells[0].Value as string);
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
			Vector3 nearInit = new Vector3(x, y, 0);
			Vector3 farInit = new Vector3(x, y, 1);
			
			Vector3 near = new Vector3(nearInit.X, nearInit.Y, nearInit.Z);
			Vector3 far = new Vector3(farInit.X, farInit.Y, farInit.Z);
			near.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, Matrix.Translation(orgX, orgY, orgZ));
			far.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, Matrix.Translation(orgX, orgY, orgZ));
			
			Vector3 direction = far - near;
			far -= direction * 0.47f;
			near += direction * 0.47f;
			direction.Normalize();
			
			foundHardpoint = new HardpointDisplayInfo();
			float minFactor = Single.MaxValue;
			foundHardpoint.Node = utf.SelectedNode;
			
			// Calculating GetHitFromScreen is really slow and doesn't seem to be needed.
			/*
			Vector3 hit;
			Vector3 normal;
			string name;
			bool hitMesh = GetHitFromScreen(x, y, out hit, out normal, out name);
			*/
			
			float hScale = hp.scale / scale;
			
			foreach(HardpointDisplayInfo hi in otherHardpoints)
			{
				bool selected = hi.Node == selectedHp;
				if (!selected)
					if (splitViewHardpoint.Panel2Collapsed || !hi.Display) continue;
				Vector3 hpLoc = new Vector3(hi.Matrix.M41, hi.Matrix.M42, hi.Matrix.M43);
				hpLoc.TransformCoordinate(hi.MeshGroup.Transform * Matrix.Translation(thnX, thnY, thnZ));

				float distanceHardpoint = Vector3.LengthSq(hpLoc - near);
				/*if(hitMesh)
				{
					float distanceMesh = Vector3.LengthSq(hit - near);
					if (distanceHardpoint > distanceMesh*1.002f) continue;
				}*/
				float dot = Vector3.Dot(hpLoc - near, direction);
				float offsetHardpoint = Math.Abs(distanceHardpoint - dot*dot);
				
				//System.Diagnostics.Debug.WriteLine(hi.Name + ": distance = " + distanceHardpoint + ", offset = " + offsetHardpoint + ", dot = " + dot);
				
				//System.Diagnostics.Debug.WriteLine(hi.Name + ": projDirection = " + projDirection + ", distCameraHp = " + distCameraHp + ", cosine = " + cosine + ", theta = " + theta + ", offsetHardpoint = " + offsetHardpoint + ", distanceHardpoint = " + distanceHardpoint);
				
				float precisionFactor = distanceHardpoint * offsetHardpoint * hScale;

				if (precisionFactor < minFactor && offsetHardpoint < hScale * 6 * (selected ? 16 : 1))
				{
					minFactor = precisionFactor;
					foundHardpoint = hi;
					//System.Diagnostics.Debug.WriteLine("New min: " + distanceHardpoint + " by " + hi.Name);
				}
			}

			if (minFactor == Single.MaxValue)
			{
				return false;
			}
			
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
				colorDiag.Color = GetColor(hardpointPanelView.SelectedCells[0].Value as string);
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
