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

        /// <summary>
        /// The last position of the mouse when the model is being rotated.
        /// </summary>
        Point lastPosition;

        /// <summary>
        /// The current rotation around the Y axis.
        /// </summary>
        float rotY = 0;

        /// <summary>
        /// The current rotation around the X axis.
        /// </summary>
        float rotX = 0;

        /// The current X offset.
        float posX = 0;

        /// The current Y offset.
        float posY = 0;

        /// The current Z offset.
        float posZ = 0;
        
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
            public VMeshData VMeshData;
            public VertexBuffer VertexBuffer;
            public IndexBuffer IndexBuffer;
        }

        /// <summary>
        /// The vertex and index buffers for one or more meshes.
        /// </summary>
        List<MeshDataBuffer> MeshDataBuffers = new List<MeshDataBuffer>();

        /// <summary>
        /// The root node of the model we are drawing.
        /// </summary>
        TreeNode rootNode;

        /// <summary>
        /// The parent tree to watch for changes.
        /// </summary>
        UTFForm parent;

        /// <summary>
        /// The directory path to search for sur/mat files.
        /// </summary>
        string directoryPath;

        public ModelViewForm(UTFForm parent, TreeNode rootNode, string directoryPath)
        {
            this.parent = parent;
            this.rootNode = rootNode;
            this.directoryPath = directoryPath;
            InitializeComponent();
            InitializeGraphics();
            this.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            parent.AddObserver(this);
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
                    device = new Device(0, DeviceType.Hardware, pictureBox1, CreateFlags.SoftwareVertexProcessing, presentParams);
                    break;
                }
                catch { }
            }

            if (device == null)
                throw new Exception("Unable to initialise Directx.");

            device.DeviceReset += new System.EventHandler(this.OnResetDevice);
            this.OnResetDevice(device, null);
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

            VMeshData rootVMeshData = null;
            foreach (TreeNode node in rootNode.Nodes.Find("VMeshData", true))
            {
                rootVMeshData = new VMeshData(node.Tag as byte[]);
                break;
            }

            if (rootVMeshData == null)
                throw new Exception("Model not found");

            // Scan for a build the mesh groups and the vertex/index buffers for
            // each mesh group.
            foreach (TreeNode node in rootNode.Nodes.Find("VMeshData", true))
            {
                MeshDataBuffer md = new MeshDataBuffer();
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
            mapFileToObj["\\"] = "\\";
            foreach (TreeNode nodeObj in rootNode.Nodes.Find("Object Name", true))
            {
                foreach (TreeNode nodeFileName in nodeObj.Parent.Nodes.Find("File Name", false))
                {
                    string objectName = Utilities.GetString(nodeObj, 0);
                    string fileName = Utilities.GetString(nodeFileName, 0);
                    mapFileToObj[fileName] = objectName;
                }
            }

            // Scan the level 0 VMeshRefs to build mesh group list for each 
            // of the construction nodes identified in the previous search.
            foreach (TreeNode node in rootNode.Nodes.Find("VMeshRef", true))
            {
                try
                {
                    string levelName = node.Parent.Parent.Name;
                    string fileName;
                    if (levelName == "Level0")
                        fileName = node.Parent.Parent.Parent.Parent.Name;
                    else if (levelName != "\\")
                        fileName = node.Parent.Parent.Parent.Name;
                    else
                        fileName = "\\";
                        if (mapFileToObj.ContainsKey(fileName))
                        {
                            MeshGroup mg = new MeshGroup();
                            mg.Name = mapFileToObj[fileName];
                            mg.RefData = new VMeshRef(node.Tag as byte[]);
                            mg.Transform = Matrix.Identity;
                            mg.MeshDataBuffer = FindMatchingMeshDataByStartVert(mg.RefData.StartVert);
                            MeshGroups.Add(mg);
                        }
                    }
                catch { }
            }
            
            // Find the offset and rotations from the Fix, Rev and Pris nodes.
            foreach (MeshGroup mg in MeshGroups)
            {
                mg.Transform = GetTransform(mg.Name);
            }

            // Load those textures. Look for textures in the directory this utf file is in
            // and look in the utf file itself.
            LoadTextures(rootNode);

                try
                {
                for (int i = 0; i < 3; ++i)
                {
            foreach (string matFile in Directory.GetFiles(directoryPath, "*.mat"))
            {
                UTFFile matUtfFile = new UTFFile();               
                try
                {
                    TreeNode matRootNode = matUtfFile.LoadUTFFile(matFile);
                    LoadTextures(matRootNode);
                }
                catch {}
            }
                    directoryPath = Directory.GetParent(directoryPath).ToString();
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
            foreach (TreeNode node in rootNode.Nodes.Find("Fix", true))
            {
                CmpFixData fixData = new CmpFixData(node.Tag as byte[]);
                foreach (CmpFixData.Part part in fixData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m = Matrix.Identity;

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

                        Matrix parentMat = GetTransform(part.ParentName);
                        m.Multiply(parentMat);
                        return m;
                    }
                }
            }

            foreach (TreeNode node in rootNode.Nodes.Find("Rev", true))
            {
                CmpRevData revData = new CmpRevData(node.Tag as byte[]);
                foreach (CmpRevData.Part part in revData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m = Matrix.Identity;
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

                        Matrix parentMat = GetTransform(part.ParentName);
                        m.Multiply(parentMat);
                        return m;
                    }
                }
            }

            foreach (TreeNode node in rootNode.Nodes.Find("Pris", true))
            {
                CmpPrisData prisData = new CmpPrisData(node.Tag as byte[]);
                foreach (CmpPrisData.Part part in prisData.Parts)
                {
                    if (part.ChildName == partName)
                    {
                        Matrix m = Matrix.Identity;
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

                        Matrix parentMat = GetTransform(part.ParentName);
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
            Matrix rotation = Matrix.RotationY(rotY);
            rotation.Multiply(Matrix.RotationX(rotX));
            //rotation.Multiply(Matrix.Translation(posX, posY, 0));
            device.Transform.View = rotation;
            device.Transform.Projection = Matrix.OrthoRH(pictureBox1.Width, pictureBox1.Height, -200000.0f, 200000.0f);
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

            if (textures.Count > 0)
            {
                device.Lights[0].Enabled = false;
                device.Lights[1].Enabled = false;
                device.RenderState.DiffuseMaterialSource = ColorSource.Material;
                device.RenderState.SpecularMaterialSource = ColorSource.Material;
            }
            else
            {
                device.Lights[0].Enabled = true;
                device.Lights[1].Enabled = true;
                device.RenderState.DiffuseMaterialSource = ColorSource.Color1;
                device.RenderState.SpecularMaterialSource = ColorSource.Color1;
            }

            device.RenderState.LocalViewer = true;
            device.RenderState.SpecularEnable = true;
            device.RenderState.DitherEnable = true;
            device.RenderState.NormalizeNormals = false;

            device.RenderState.Ambient = System.Drawing.Color.FromArgb(0x222222);
        }

        /// <summary>
        /// Render the image.
        /// </summary>
        private void Render()
        {
            if (device == null)
                return;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            device.BeginScene();

            if (checkBoxSolid.Checked)
            {
                device.RenderState.CullMode = Cull.Clockwise; 
                device.RenderState.FillMode = FillMode.Solid;
                device.RenderState.Lighting = true;
            }
            else
            {
                device.RenderState.CullMode = Cull.None;
                device.RenderState.FillMode = FillMode.WireFrame;
                device.RenderState.Lighting = false;
            }


            SetupLights();
            SetupMatrices();

            foreach (MeshGroup mg in MeshGroups)
            {
                device.SetStreamSource(0, mg.MeshDataBuffer.VertexBuffer, 0);
                device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
                device.Indices = mg.MeshDataBuffer.IndexBuffer;

                device.Transform.World = Matrix.Multiply(
                    Matrix.Multiply(mg.Transform, Matrix.Translation(posX, posY, -posZ)),
                    Matrix.Scaling(scale, scale, scale));

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
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, mesh.BaseVertex, 0, numVert, mesh.TriangleStart, numTriangles);
                }
            }
            device.EndScene();
            device.Present(pictureBox1);
        }

        /// <summary>
        /// Override the form painting event to run the directx render function.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                this.Render();
            }
            catch { }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        /// <summary>
        /// Change the scale on mouse wheel turns.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            // Adjust the scale.
            if (e.Delta > 0)
                scale *= 1.25f;
            else
                scale *= 0.75f;

            if (scale < 0.001f)
                scale = 0.001f;
            if (scale > 1000)
                scale = 1000;

            // Map the scale onto the trackbar.
            this.trackBarScale.Scroll -= new System.EventHandler(this.trackBarScale_Scroll);
            trackBarScale.Value = (int)(Math.Log10(scale) * 100);
            this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);

            // Show it in the text box.
            textBoxScale.Text = scale.ToString("0.###");

            Invalidate();
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
            Invalidate();
        }

        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            if (textBoxScale.Text.Length != 0)
            {
                try
                {
                    float s = Convert.ToSingle(textBoxScale.Text);
                    if (s >= 0.001 && s <= 1000)
                    {
                        scale = s;
                        Invalidate();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// On mouse down record the current mouse position to prepare for
        /// rotating the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPosition = e.Location;
        }

        /// <summary>
        /// Rotate the model if the LEFT mouse button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float deltaX = (float)(e.Location.X - lastPosition.X);
                float deltaY = (float)(e.Location.Y - lastPosition.Y);
                // If the shift key is down then move
                if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    posX += (deltaX / scale);
                    posY -= (deltaY / scale);
                    this.textBoxPosX.TextChanged -= new System.EventHandler(this.textBoxPosXYZ_TextChanged);
                    this.textBoxPosY.TextChanged -= new System.EventHandler(this.textBoxPosXYZ_TextChanged);
                    textBoxPosX.Text = posX.ToString("0.#");
                    textBoxPosY.Text = posY.ToString("0.#");
                    this.textBoxPosX.TextChanged += new System.EventHandler(this.textBoxPosXYZ_TextChanged);
                    this.textBoxPosY.TextChanged += new System.EventHandler(this.textBoxPosXYZ_TextChanged);
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

                lastPosition = e.Location;
                Invalidate();
            }
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
        /// Search the mesh buffers for the specified vertex. This should probably
        /// be checking for the VMeshLibID too.
        /// </summary>
        /// <param name="startVertex"></param>
        /// <returns></returns>
        MeshDataBuffer FindMatchingMeshDataByStartVert(int startVertex)
        {
            foreach (MeshDataBuffer md in MeshDataBuffers)
            {
                foreach (VMeshData.TMeshHeader mh in md.VMeshData.Meshes)
                {
                    if (mh.BaseVertex == startVertex)
                    {
                        return md;
                    }
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
            foreach (MeshDataBuffer mdb in MeshDataBuffers)
            {
                foreach (VMeshData.TMeshHeader mesh in mdb.VMeshData.Meshes)
                {
                    if (mesh.MaterialId == materialId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  Return the texture if the specified texture library name is in the texture
        ///  list for this model.
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        private Texture FindTextureByFileName(string filename)
        {
            foreach (Texture tex in textures.Values)
            {
                if (tex.fileName == filename)
                {
                    return tex;
                }
            }
            return null;
        }

        /// <summary>
        /// Return the texture for the specified materialID.
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        private Texture FindTextureByMaterialID(uint materialID)
        {
            if (textures.ContainsKey(materialID))
                return textures[materialID];
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

        /// <summary>
        /// Look for material information in this utf file.
        /// </summary>
        /// <param name="matRootNode"></param>
        private void LoadTextures(TreeNode matRootNode)
        {

            foreach (TreeNode nodeMatLib in matRootNode.Nodes.Find("material library", true))
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
                            tex.fileName = Utilities.GetString(node.Nodes["Dt_name"].Tag as byte[], 0);
                            tex.texture = MakeTexture(matRootNode, tex.fileName);
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
            try
            {
                foreach (TreeNode nodeTexLib in matRootNode.Nodes.Find("texture library", true))
                {
                    foreach (TreeNode node in nodeTexLib.Nodes)
                    {
                        if (node.Name == texFileName)
                        {
                            TreeNode mipNode = null;
                            try
                            {
                                mipNode = node.Nodes["MIPS"];
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
                            catch
                            {
                            }                           
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private void textBoxPosXYZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                posX = Convert.ToSingle(textBoxPosX.Text);
                posY = Convert.ToSingle(textBoxPosY.Text);
                posZ = Convert.ToSingle(textBoxPosZ.Text);
                Invalidate();
            }
            catch { }
        }

    }
}
