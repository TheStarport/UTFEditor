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

        /// <summary>
        /// The model scale.
        /// </summary>
        float scale = 20.0f;
        float distance;

        int background = 0;
        int brightness = 0;

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

        /// <summary>
        /// Mesh group data.
        /// </summary>
        public class MeshGroup
        {
            public string Name;
            public VMeshRef RefData;
            public Matrix Transform;
            public MeshDataBuffer MeshDataBuffer;
        };

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
            public VertexBuffer VertexBuffer;
            public IndexBuffer IndexBuffer;
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
            public float max;
            public float min;

            // Define the vertices for the hardpoint display.
            static public CustomVertex.PositionColored[] displayvertices =
            {
                new CustomVertex.PositionColored( 0, 0,  0, 0xff0000),
                new CustomVertex.PositionColored( 1, 1,  0, 0xff0000),
                new CustomVertex.PositionColored(-1, 1,  0, 0xff0000),

                new CustomVertex.PositionColored( 0, 0,  0, 0x00ff00),
                new CustomVertex.PositionColored(-1, 1,  0, 0x00ff00),
                new CustomVertex.PositionColored( 0, 1, -2, 0x00ff00),

                new CustomVertex.PositionColored( 0, 0,  0, 0x007f00),
                new CustomVertex.PositionColored( 0, 1, -2, 0x007f00),
                new CustomVertex.PositionColored( 1, 1,  0, 0x007f00),

                new CustomVertex.PositionColored( 1, 1,  0, 0x0000ff),
                new CustomVertex.PositionColored( 0, 1, -2, 0x0000ff),
                new CustomVertex.PositionColored(-1, 1,  0, 0x0000ff),
            };
        };
        Hardpoint hp = new Hardpoint();

        /// <summary>
        /// Map a filename to its mesh group.
        /// </summary>
        Dictionary<string, int> mapFileToMesh = new Dictionary<string,int>();

        CmpFixData fixData;
        CmpRevData revData, prisData;
        CmpSphereData sphereData;
        Dictionary<string, Matrix> ParentTransform = new Dictionary<string, Matrix>();

        public ModelViewForm(UTFForm parent, TreeView utf, string directoryPath)
        {
            this.parent = parent;
            this.utf = utf;
            this.rootNode = utf.Nodes[0];
            this.directoryPath = Path.GetDirectoryName(directoryPath);
            InitializeComponent();
            InitializeGraphics();
            this.Text += " - " + Path.GetFileName(directoryPath);
            this.MouseWheel += new MouseEventHandler(modelView_MouseWheel);
            parent.AddObserver(this);
        }

        private void ModelViewForm_Activated(object sender, EventArgs e)
        {
            modelView.Focus();
        }

        /// <summary>
        /// Setup directx.
        /// </summary>
        public void InitializeGraphics()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.BackBufferFormat = Format.Unknown;
            presentParams.EnableAutoDepthStencil = true;

            DepthFormat[] formats = { DepthFormat.D32, DepthFormat.D24X8, DepthFormat.D16 };
            foreach (DepthFormat format in formats)
            {
                try
                {
                    presentParams.AutoDepthStencilFormat = format;
                    device = new Device(0, DeviceType.Hardware, modelView.Panel1, CreateFlags.SoftwareVertexProcessing, presentParams);
                    break;
                }
                catch { }
            }

            if (device == null)
                throw new Exception("Unable to initialise Directx.");

            device.DeviceReset += new System.EventHandler(this.OnResetDevice);
            this.OnResetDevice(device, null);
            scale = (modelView.Panel1.Height - 1) / distance;
            if (scale < 0.001f)
                scale = 0.001f;
            else if (scale > 1000)
                scale = 1000;
            textBoxScale.Text = scale.ToString("0.###");
        }

        /// <summary>
        /// Load the model into the vertex and index buffers. Make note of the
        /// positions and rotations of the mesh groups we need to render.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnResetDevice(object sender, EventArgs e)
        {
            Device dev = (Device)sender;
            dev.RenderState.ZBufferEnable = true;
            dev.RenderState.DitherEnable = true;

            if (hp.display != null)
            {
                hp.display.Dispose();
                hp.revolute.Dispose();
            }
            hp.display = new VertexBuffer(typeof(CustomVertex.PositionColored), 12, dev, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            hp.display.SetData(Hardpoint.displayvertices, 0, LockFlags.None);
            hp.revolute = new VertexBuffer(typeof(CustomVertex.PositionColored), 26, dev, Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            hp.max = Single.MaxValue;
            hp.min = Single.MinValue;
            
            DataChanged(null, "", null);
        }

        /// <summary>
        /// Parse the treeview and build the directx vertex and index buffers.
        /// </summary>
        public void DataChanged(TreeNode changedNode, string oldName, object oldData)
        {
            MeshGroups.Clear();
            foreach (MeshDataBuffer bd in MeshDataBuffers)
            {
                bd.VertexBuffer.Dispose();
                bd.IndexBuffer.Dispose();
            }
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
                md.VertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), vertices.Count, device, Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
                md.VertexBuffer.SetData(vertices.ToArray(), 0, LockFlags.None);
                md.IndexBuffer = new IndexBuffer(typeof(UInt16), indices.Count, device, Usage.WriteOnly, Pool.Default);
                md.IndexBuffer.SetData(indices.ToArray(), 0, LockFlags.None);

                // Save the group.
                MeshDataBuffers.Add(md);
            }

            // Find Cons(truct) nodes. They contain data that links each mesh to the
            // root mesh.
            Dictionary<string, string> mapFileToObj = new Dictionary<string, string>();
            mapFileToObj["\\"] = "";
            foreach (TreeNode nodeObj in rootNode.Nodes.Find("Object Name", true))
            {
                foreach (TreeNode nodeFileName in nodeObj.Parent.Nodes.Find("File Name", false))
                {
                    string objectName = Utilities.GetString(nodeObj);
                    string fileName = Utilities.GetString(nodeFileName);
                    mapFileToObj[fileName] = objectName;
                }
            }

            // Determine if there are any levels.
            TreeNode[] multilevels = rootNode.Nodes.Find("MultiLevel", true);
            if (multilevels.Length == 0)
            {
                labelLevel.Visible = spinnerLevel.Visible = false;
                spinnerLevel.Value = 0;
            }
            else
            {
                labelLevel.Visible = spinnerLevel.Visible = true;
                // Let's assume all parts have the same number of levels.
                // Turns out that's not a valid assumption, so let's assume
                // the root part is first and contains the most levels.
                int level = 0;
                foreach (TreeNode node in multilevels[0].Nodes)
                {
                    try
                    {
                        if (node.Name.StartsWith("Level", StringComparison.OrdinalIgnoreCase))
                        {
                            int lev = int.Parse(node.Name.Substring(5));
                            if (lev > level)
                                level = lev;
                        }
                    }
                    catch { }
                }
                spinnerLevel.Maximum = level;
            }

            // Scan the level 0 VMeshRefs to build mesh group list for each 
            // of the construction nodes identified in the previous search.
            string levelstr = spinnerLevel.Value.ToString();
            foreach (TreeNode node in rootNode.Nodes.Find("VMeshRef", true))
            {
                try
                {
                    string levelName = node.Parent.Parent.Name;
                    string fileName;
                    if (levelName.StartsWith("Level", StringComparison.OrdinalIgnoreCase))
                    {
                        if (levelName.Substring(5) == levelstr)
                            fileName = node.Parent.Parent.Parent.Parent.Name;
                        else
                            continue;
                    }
                    else
                        fileName = levelName;
                    string objName;
                    if (mapFileToObj.TryGetValue(fileName, out objName))
                    {
                        MeshGroup mg = new MeshGroup();
                        mg.Name = objName;
                        mg.RefData = new VMeshRef(node.Tag as byte[]);
                        mg.Transform = Matrix.Identity;
                        mg.MeshDataBuffer = FindMatchingMeshData(mg.RefData);
                        mapFileToMesh[fileName] = MeshGroups.Count;
                        MeshGroups.Add(mg);
                    }
                }
                catch { }
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
            Material mtrl = new Material();
            mtrl.Diffuse = System.Drawing.Color.White;
            mtrl.Ambient = System.Drawing.Color.White;
            device.Material = mtrl;

            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = System.Drawing.Color.White;
            device.Lights[0].DiffuseColor = new ColorValue(scale, scale, scale);
            device.Lights[0].Direction = new Vector3(1.0f, 1.0f, 1.0f);

            device.Lights[1].Type = LightType.Directional;
            device.Lights[1].Diffuse = System.Drawing.Color.White;
            device.Lights[1].DiffuseColor = new ColorValue(scale, scale, scale);
            device.Lights[1].Direction = new Vector3(-1.0f, -1.0f, -1.0f);

            if (textures_Count > 0)
            {
                device.Lights[0].Enabled = false;
                device.Lights[1].Enabled = false;
                device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                device.RenderState.SpecularMaterialSource = ColorSource.Material;
            }
            else
            {
                device.RenderState.Lighting = false;
                device.Lights[0].Enabled = true;
                device.Lights[1].Enabled = true;
                device.RenderState.DiffuseMaterialSource = ColorSource.Color1;
                device.RenderState.SpecularMaterialSource = ColorSource.Color1;
            }

            device.RenderState.LocalViewer = true;
            device.RenderState.SpecularEnable = true;
            device.RenderState.DitherEnable = true;
            device.RenderState.NormalizeNormals = false;
        }

        /// <summary>
        /// Render the image.
        /// </summary>
        private void Render()
        {
            if (device == null)
                return;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, background, 1.0f, 0);
            device.BeginScene();

            if (checkBoxSolid.Checked)
            {
                device.RenderState.CullMode = Cull.Clockwise;
                device.RenderState.FillMode = FillMode.Solid;
            }
            else
            {
                device.RenderState.CullMode = Cull.None;
                device.RenderState.FillMode = FillMode.WireFrame;
            }

            SetupLights();
            SetupMatrices();
            
            foreach (MeshGroup mg in MeshGroups)
            {
                device.SetStreamSource(0, mg.MeshDataBuffer.VertexBuffer, 0);
                device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
                device.Indices = mg.MeshDataBuffer.IndexBuffer;

                device.Transform.World = Matrix.Multiply(mg.Transform, Matrix.Translation(orgX, orgY, orgZ));
                device.RenderState.AmbientColor = (utf.SelectedNode.Text == mg.Name) ? 0x3f3f00 : 0;
                device.RenderState.AmbientColor += brightness;

                int endMesh = mg.RefData.StartMesh + mg.RefData.NumMeshes;
                for (int mn = mg.RefData.StartMesh; mn < endMesh; mn++)
                {
                    VMeshData.TMeshHeader mesh = mg.MeshDataBuffer.VMeshData.Meshes[mn];

                    Texture tex = FindTextureByMaterialID(mesh.MaterialId);
                    if (tex != null)
                    {
                        device.SetTexture(0, tex.texture);

                        device.TextureState[0].ColorOperation = TextureOperation.Add;
                        device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                        device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
                        device.TextureState[0].AlphaOperation = TextureOperation.SelectArg1;
                        
                        device.SamplerState[0].MipFilter = TextureFilter.Linear;
                        device.SamplerState[0].MinFilter = TextureFilter.Linear;
                        device.SamplerState[0].MagFilter = TextureFilter.Linear;
                    }

                    int numVert = mesh.EndVertex - mesh.StartVertex + 1;
                    int numTriangles = mesh.NumRefVertices / 3;
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, mg.RefData.StartVert + mesh.StartVertex, 0, numVert, mesh.TriangleStart, numTriangles);
                }
            }

            ShowHardpoint();

            device.EndScene();
            device.Present(modelView.Panel1);
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

            if (scale < 0.001f)
                scale = 0.001f;
            else if (scale > 1000)
                scale = 1000;

            // Show it in the text box, which also invalidates.
            textBoxScale.Text = scale.ToString("0.###");
        }
        
        /// <summary>
        /// If the scale track bar is updated then re-scale the display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarScale_Scroll(object sender, EventArgs e)
        {
            scale = (float)Math.Pow(10, (double)trackBarScale.Value / 100.0);
            textBoxScale.Text = scale.ToString("0.###");
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
                Invalidate();
            }
        }

        /// <summary>
        /// On mouse down record the current mouse position to prepare for
        /// rotating the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelView_MouseDown(object sender, MouseEventArgs e)
        {
            lastPosition = e.Location;
            right = RightType.RightFirst;
        }

        /// <summary>
        /// Rotate the model if the LEFT mouse button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
                return;

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
                        if (scale < 0.001f)
                            scale = 0.001f;
                        else if (scale > 1000)
                            scale = 1000;
                        textBoxScale.Text = scale.ToString("0.###");
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
        };

        /// <summary>
        /// A dictionary of textures used by this model.
        /// </summary>
        Dictionary<uint, Texture> textures = new Dictionary<uint, Texture>();
        int textures_Count;

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
                            // Not all textures have files (glass in particular).
                            // This makes them show as black, rather than garbage.
                            try
                            {
                                tex.fileName = Utilities.GetString(node.Nodes["Dt_name"]);
                                tex.texture = MakeTexture(matRootNode, tex.fileName);
                                textures_Count++;
                            }
                            catch { }
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
                            using (MemoryStream ms = new MemoryStream(mipNode.Tag as byte[]))
                            {
                                return TextureLoader.FromStream(device, ms);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            rotX = rotY = rotZ = 0;
            posX = posY = 0;
            orgX = orgY = orgZ = 0;
            brightness = 0;
            hp.scale = 1;
            scale = (modelView.Panel1.Height - 1) / distance;
            if (scale < 0.001f)
                scale = 0.001f;
            else if (scale > 1000)
                scale = 1000;
            textBoxScale.Text = scale.ToString("0.###");
            // If the scale is the same, the text box won't change, so explicitly invalidate.
            Invalidate();
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            TreeNode node = utf.SelectedNode;
            try
            {
                if (node.Parent.Text == "Hardpoints")
                    node = rootNode.Nodes.Find(node.Name, true)[0];
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
                Matrix m = Matrix.Multiply(MeshGroups[mapFileToMesh[node.Parent.Parent.Parent.Parent.Name]].Transform, Matrix.Translation(orgX, orgY, orgZ));
                orgX = -m.M41;
                orgY = -m.M42;
                orgZ = -m.M43;
                Invalidate();
            }
            catch { }
        }

        private void ShowHardpoint()
        {
            TreeNode node = utf.SelectedNode;
            try
            {
                if (node.Parent.Text == "Hardpoints")
                    node = rootNode.Nodes.Find(node.Name, true)[0];
                else if (!Utilities.StrIEq(node.Parent.Name, "Fixed", "Revolute"))
                {
                    node = node.Parent;
                    if (!Utilities.StrIEq(node.Parent.Name, "Fixed", "Revolute"))
                        return;
                }
                TreeNode n = node.Nodes["Position"];
                byte[] data = n.Tag as byte[];
                int pos = 0;
                Matrix m;
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

                device.SetTexture(0, null);
                device.Transform.World = Matrix.Multiply(Matrix.Multiply(Matrix.Multiply(
                                Matrix.Scaling(hp.scale, hp.scale, hp.scale),
                                m),
                                MeshGroups[mapFileToMesh[node.Parent.Parent.Parent.Name]].Transform),
                                Matrix.Translation(orgX, orgY, orgZ));
                                
                device.RenderState.Lighting = false;
                device.RenderState.FillMode = FillMode.Solid;
                device.RenderState.CullMode = Cull.None;
                device.VertexFormat = CustomVertex.PositionColored.Format;

                if (Utilities.StrIEq(node.Parent.Name, "Revolute"))
                {
                    try
                    {
                        n = node.Nodes["Max"];
                        float max = BitConverter.ToSingle(n.Tag as byte[], 0);
                        n = node.Nodes["Min"];
                        float min = BitConverter.ToSingle(n.Tag as byte[], 0);
                        if (hp.max != max || hp.min != min)
                        {
                            hp.max = max;
                            hp.min = min;
                            // If max is 360° or min is -360°, set them to ±180°.
                            if (max >= (float)Math.PI * 2 - 0.0001f ||
                                min <= 0.0001f - (float)Math.PI * 2)
                            {
                                max = (float)Math.PI;
                                min = -max;
                            }
                            // Axis doesn't seem to be used, so just rotate them to fit.
                            max = -max - (float)Math.PI / 2;
                            min = -min - (float)Math.PI / 2;
                            CustomVertex.PositionColored[] rotVert = new CustomVertex.PositionColored[26];
                            // Position the rotation just below the top, so the blue takes precedence.
                            rotVert[0] = new CustomVertex.PositionColored(0, 0.999f, 0, 0xffff00);
                            pos = 1;
                            float delta = (max - min) / 24;
                            for (float angle = max; pos < 26; angle -= delta)
                                rotVert[pos++] = new CustomVertex.PositionColored(2 * (float)Math.Cos(angle), 0.999f, 2 * (float)Math.Sin(angle), 0xffff00);
                            hp.revolute.SetData(rotVert, 0, LockFlags.None);
                        }
                        device.SetStreamSource(0, hp.revolute, 0);
                        device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 24);
                    }
                    catch { }
                }
                device.SetStreamSource(0, hp.display, 0);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, 4);

                device.RenderState.Lighting = true;
            }
            catch { }
        }

        private void modelView_MouseClick(object sender, MouseEventArgs e)
        {
            modelView.Focus();
        }

        private void modelView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    new ViewTextForm("Model View Help", Properties.Resources.ModelViewHelp).Show();
                    break;

                // Bottom view
                case Keys.D1:
                    rotY = rotZ = 0;
                    rotX = -(float)Math.PI / 2;
                    Invalidate();
                    break;

                // Top view
                case Keys.D2:
                    rotY = rotZ = 0;
                    rotX = (float)Math.PI / 2;
                    Invalidate();
                    break;

                // Back view
                case Keys.D3:
                    rotX = rotY = rotZ = 0;
                    Invalidate();
                    break;

                // Front view
                case Keys.D4:
                    rotX = rotZ = 0;
                    rotY = (float)Math.PI;
                    Invalidate();
                    break;

                // Right view
                case Keys.D5:
                    rotX = rotZ = 0;
                    rotY = -(float)Math.PI / 2;
                    Invalidate();
                    break;

                // Left view
                case Keys.D6:
                    rotX = rotZ = 0;
                    rotY = (float)Math.PI / 2;
                    Invalidate();
                    break;

                // Increase brightness
                case Keys.A:
                    if (brightness != 0x808080)
                    {
                        if (e.Shift)
                            brightness = 0x808080;
                        else
                            brightness += 0x080808;
                        Invalidate();
                    }
                    break;

                // Decrease brightness
                case Keys.Z:
                    if (brightness != 0)
                    {
                        if (e.Shift)
                            brightness = 0;
                        else
                            brightness -= 0x080808;
                        Invalidate();
                    }
                    break;

                // Toggle background between black and white
                case Keys.B:
                    background ^= 0xFFFFFF;
                    Invalidate();
                    break;

                // Toggle solid and wireframe
                case Keys.W:
                    checkBoxSolid.Checked = !checkBoxSolid.Checked;
                    break;

                // Increase the hardpoint display
                case Keys.Multiply:
                    hp.scale *= 1.25f;
                    Invalidate();
                    break;

                // Decrease the hardpoint display
                case Keys.Divide:
                    hp.scale /= 1.25f;
                    Invalidate();
                    break;

                // Zoom in
                case Keys.Add:
                    scale *= 1.25f;
                    if (scale > 1000)
                        scale = 1000;
                    textBoxScale.Text = scale.ToString("0.###");
                    break;

                // Zoom out
                case Keys.Subtract:
                    scale /= 1.25f;
                    if (scale < 0.001f)
                        scale = 0.001f;
                    textBoxScale.Text = scale.ToString("0.###");
                    break;

                // Move up
                case Keys.Up:
                    posY += (e.Shift) ? 1 : 10;
                    Invalidate();
                    break;

                // Move down
                case Keys.Down:
                    posY -= (e.Shift) ? 1 : 10;
                    Invalidate();
                    break;

                // Move left
                case Keys.Left:
                    posX -= (e.Shift) ? 1 : 10;
                    Invalidate();
                    break;

                // Move right
                case Keys.Right:
                    posX += (e.Shift) ? 1 : 10;
                    Invalidate();
                    break;

                // Rotate around the Y axis
                case Keys.PageUp:
                case Keys.PageDown:
                    float delta = (float)Math.PI / (e.Shift ? 180 : 12);
                    if (e.KeyCode == Keys.PageDown)
                        delta = -delta;
                    switch (e.Modifiers & ~Keys.Shift)
                    {
                        case Keys.None:    rotY += delta; break;
                        case Keys.Control: rotX += delta; break;
                        case Keys.Alt:     rotZ += delta; break;
                    }
                    Invalidate();
                    break;

                // Reset the view, but keep the origin and scales
                case Keys.Home:
                    posX = posY = 0;
                    rotX = rotY = rotZ = 0;
                    Invalidate();
                    break;

                // Close
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void spinnerLevel_ValueChanged(object sender, EventArgs e)
        {
            DataChanged(null, "", null);
            Invalidate();
        }
    }
}
