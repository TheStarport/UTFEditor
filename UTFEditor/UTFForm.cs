using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    /// <summary>
    /// A list of nodes that can be edited.
    /// </summary>
    public enum Editable
    {
        No,
        VMeshRef,
        Hardpoint,
        Fix,
        Rev,
        Sphere,
        Channel,
        Color,
        String,
        Int,
        IntHex,
        Float
    };

    /// <summary>
    /// A list of nodes that can be viewed (or played).
    /// </summary>
    public enum Viewable
    {
        No,
        VMeshData,
        VMeshRef,
        VWireData,
        Texture,            // node starts with MIP
        Wave,               // data starts with RIFF
    };

    public partial class UTFForm : Form
    {
        /// <summary>
        /// The parent window containing this form.
        /// </summary>
        UTFEditorMain parent;

        /// <summary>
        /// True if there are pending file changes that have not been saved.
        /// </summary>
        private bool fileChangesNotSaved = false;

        /// <summary>
        /// The UTFFile this form is displaying.
        /// </summary>
        UTFFile utfFile = new UTFFile();

        SurFile surFile = null;

        internal SurFile SUR => surFile;

        /// <summary>
        /// The name of the UTF file.
        /// </summary>
        public string fileName;


        /// <summary>
        /// Create an empty form.
        /// </summary>
        public UTFForm(UTFEditorMain parent, string name)
        {
            InitializeComponent();
            fileName = name;
            string path = Path.GetDirectoryName(name);
            this.Text = Path.GetFileName(name);
            if (path.Length != 0)
            {
                int data = path.IndexOf(@"\data\", StringComparison.OrdinalIgnoreCase);
                if (data != -1)
                    path = path.Substring(data + 6);
                this.Text += " - " + path;
            }
            this.parent = parent;
        }

        /// <summary>
        /// Try to load a UTF file. Throw an exception on failure.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        public void LoadUTFFile(string filePath)
        {
            // Add the real root to the treeview
            treeView1.Nodes.Clear();
            TreeNode root = utfFile.LoadUTFFile(filePath);
            // Add the hardpoints first, so we can sort them
            // and still preserve the root order.
            if (utfFile.Hardpoints.Nodes.Count > 0)
                treeView1.Nodes.Add(utfFile.Hardpoints);
            if (utfFile.Parts.Nodes.Count > 0)
                treeView1.Nodes.Add(utfFile.Parts);
            treeView1.TreeViewNodeSorter = new NodeSorter();
            treeView1.Sort();
            treeView1.TreeViewNodeSorter = null;
            treeView1.Sorted = false;
            treeView1.Nodes.Insert(0, root);
            if (utfFile.Hardpoints.Nodes.Count == 0 &&
                utfFile.Parts.Nodes.Count == 0)
                treeView1.Nodes[0].Expand();

            treeView1.Modified += (s, e) => { Modified(); };

            var surpath = Path.ChangeExtension(filePath, ".sur");
            if (File.Exists(surpath))
            {
                try
                {
                    surFile = new SurFile(surpath);
                }
                catch(Exception)
                {
                    surFile = null;
                    MessageBox.Show("Error while loading associated SUR file '" + surpath + "'.", "SUR Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <sumary>
        /// Sort the Hardpoints and Parts nodes.
        /// </sumary>
        public class NodeSorter : IComparer
        {
            private string sx, sy;

            private int compare(string hp)
            {
                bool a = sx.StartsWith(hp);
                bool b = sy.StartsWith(hp);
                if (a && !b)
                    return -1;
                if (!a && b)
                    return 1;
                if (a && b)
                    return String.CompareOrdinal(sx, sy);
                return 0;
            }
            
            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;
                sx = tx.Text.ToLowerInvariant();
                sy = ty.Text.ToLowerInvariant();
                bool a, b;
                int rc;

                //if (tx.FullPath.StartsWith("Parts"))
                if (tx.Parent != null && tx.Parent.Text != "Hardpoints")
                    return String.CompareOrdinal(sx, sy);

                // Place hardpoints before damage points.
                a = (sx[0] == 'h');
                b = (sy[0] == 'h');
                if (a && !b)
                    return -1;
                if (!a && b)
                    return 1;
                if (a && b)
                {
                    // Match the inventory order.
                    // Place weapon hardpoints first.
                    rc = compare("hpweapon");
                    if (rc != 0)
                        return rc;

                    // Then turrets.
                    rc = compare("hpturret");
                    if (rc != 0)
                        return rc;

                    // Then torpedoes/disruptors.
                    rc = compare("hptorpedo");
                    if (rc != 0)
                        return rc;

                    // Then mines.
                    rc = compare("hpmine");
                    if (rc != 0)
                        return rc;

                    // Then countermeasures.
                    rc = compare("hpcm");
                    if (rc != 0)
                        return rc;

                    // Then shields.
                    rc = compare("hpshield");
                    if (rc != 0)
                        return rc;

                    // Finally thrusters.
                    rc = compare("hpthruster");
                    if (rc != 0)
                        return rc;
                }
                return string.CompareOrdinal(sx, sy);
            }
        }

        /// <summary>
        /// Save the data in the treeview displayed by this form back into
        /// the specified file.
        /// </summary>
        /// <param name="filename"></param>
        public void SaveUTFFile(string filePath)
        {
            utfFile.SaveUTFFile(treeView1.Nodes[0], filePath);
            fileName = filePath;
            fileChangesNotSaved = false;
            this.Text = Path.GetFileName(fileName) + " - " + Path.GetDirectoryName(fileName);
        }

        bool doubleClicked = false;

        // The node in the double-click event might not actually be the node that was
        // double-clicked, due to scrolling caused by also expanding or collapsing it.
        // Use the node from the click event, instead.
        TreeNode clickedNode;

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            clickedNode = e.Node;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // The event is also triggered on the plus/minus sign, so make sure
            // it's the selected node.
            if (clickedNode != treeView1.SelectedNode)
                return;

            // Can't do anything with the top-level nodes.
            if (treeView1.SelectedNode.Parent == null)
                return;

            // Double-clicking a list hardpoint will move to its definition.
            if (treeView1.SelectedNode.Parent.Text == "Hardpoints")
            {
                TreeNode node = treeView1.Nodes[0].Nodes.Find(treeView1.SelectedNode.Text, true)[0];
                if (node != treeView1.SelectedNode)
                    treeView1.SelectedNode = node;
            }
            // Edit or view, depending which is available.
            else if (toolStripMenuItemEdit.Enabled)
            {
                EditNode();
            }
            else if (toolStripMenuItemView.Enabled)
            {
                ViewNode();
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if(e.KeyCode == Keys.Enter)
                {
                    if (toolStripMenuItemEdit.Enabled)
                        EditNode();
                    else if (toolStripMenuItemView.Enabled)
                        ViewNode();
                }

                if (e.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            e.Handled = Copy();
                            break;
                        case Keys.X:
                            e.Handled = Cut();
                            break;
                        case Keys.V:
                            if (e.Shift)
                                e.Handled = PasteChild();
                            else
                                e.Handled = Paste();
                            break;
                    }

                    if (e.Handled) e.SuppressKeyPress = true;
                }

                if (e.KeyCode == Keys.Delete && (e.Handled = Delete())) e.SuppressKeyPress = true;
            }
        }

        private void toolStripMenuItemExpandAll_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
                treeView1.SelectedNode.ExpandAll();
        }

        private void toolStripMenuItemCollapseAll_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
                treeView1.SelectedNode.Collapse();
        }

        private void toolStripMenuItemRenameNode_Click(object sender, EventArgs e)
        {
            RenameNode();
        }

        private void toolStripMenuItemAddNode_Click(object sender, EventArgs e)
        {
            AddNode("New");
        }

        private void toolStripMenuItemDeleteNode_Click(object sender, EventArgs e)
        {
            DeleteSelectedNodes();
        }

        public void AddNode(string name)
        {
            TreeNode node = new TreeNode(name);
            node.Name = name;
            node.Tag = new byte[0];

            if (treeView1.SelectedNode == null)
            {
                treeView1.Nodes.Add(node);
            }
            else
            {
                treeView1.SelectedNode.Nodes.Add(node);
                treeView1.SelectedNode.Expand();
            }
            treeView1.SelectedNode = node;
            
            if (name != "\\")
            {
                Modified();
                RenameNode();
            }
        }

        public void DeleteSelectedNodes()
        {
            if (treeView1.SelectedNodes != null)
            {
                treeView1.Delete();
            }
        }

        public void RenameNode()
        {
            if (treeView1.SelectedNode != null)
                treeView1.SelectedNode.BeginEdit();
        }

        private void stringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditString();
        }

        public void EditString()
        {
            if (treeView1.SelectedNode != null)
            {
                EditStringForm edit = new EditStringForm(treeView1.SelectedNode);
                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    parent.SetSelectedNode(treeView1.SelectedNode);
                    Modified();
                }
                edit.Dispose();
                treeView1.Select();
            }
        }

        private void intArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditIntArray(false);
        }

        public void EditIntArray(bool hex)
        {
            if (treeView1.SelectedNode != null &&
                ((treeView1.SelectedNode.Tag as byte[]).Length & 3) == 0)
            {
                EditIntArrayForm edit = new EditIntArrayForm(treeView1.SelectedNode, hex);
                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    parent.SetSelectedNode(treeView1.SelectedNode);
                    Modified();
                }
                edit.Dispose();
                treeView1.Select();
            }
        }

        private void floatArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFloatArray();
        }

        public void EditFloatArray()
        {
            if (treeView1.SelectedNode != null &&
                ((treeView1.SelectedNode.Tag as byte[]).Length & 3) == 0)
            {
                EditFloatArrayForm edit = new EditFloatArrayForm(treeView1.SelectedNode);
                if (edit.ShowDialog() == DialogResult.OK)
                {
                    parent.SetSelectedNode(treeView1.SelectedNode);
                    Modified();
                }
                edit.Dispose();
                treeView1.Select();
            }
        }

        public void EditColor()
        {
            if (treeView1.SelectedNode != null)
            {
                EditColorForm edit = new EditColorForm(treeView1.SelectedNode);
                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    parent.SetSelectedNode(treeView1.SelectedNode);
                    Modified();
                }
                edit.Dispose();
                treeView1.Select();
            }
        }

        private void toolStripMenuItemImportData_Click(object sender, EventArgs e)
        {
            ImportData();
        }

        public void ImportData()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot import data to non-leaf nodes or multiple nodes", "Error");
                return;
            }

            openFileDialog1.Filter = "All Types (*.*)|*.*";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(openFileDialog1.FileName);
                    treeView1.SelectedNode.Tag = data;
                    parent.SetSelectedNode(treeView1.SelectedNode);
                    Modified();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Error");
                }
            }
        }

        private void toolStripMenuItemExportData_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        public void ExportData()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            saveFileDialog1.Filter = "All Types (*.*)|*.*";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    byte[] data = treeView1.SelectedNode.Tag as byte[];
                    File.WriteAllBytes(saveFileDialog1.FileName, data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Error");
                }
            }
        }

        public void ExportAllTextures(string path)
        {
            foreach (TreeNode n in treeView1.Nodes[0].Nodes)
            {
                foreach (TreeNode p in n.Nodes)
                {
                    foreach (TreeNode m in p.Nodes)
                    {
                        try
                        {
                            if (m.Text == "MIPS")
                            {
                                byte[] data = m.Tag as byte[];
                                File.WriteAllBytes(path + "\\" + Path.ChangeExtension(p.Text, ".dds"), data);
                            }
                            else if (m.Text.StartsWith("MIP"))
                            {
                                byte[] data = m.Tag as byte[];
                                File.WriteAllBytes(path + "\\" + Path.ChangeExtension(p.Text, ".tga"), data);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Error " + ex.Message, "Error");
                        }
                    }
                }   
            }
        }

        private void BuildImportedHardpoints(TreeNode parent, THNEditor.ThnParse thn)
        {
            foreach (THNEditor.ThnParse.Entity en in thn.entities)
            {
                HardpointData hp = new HardpointData(en.entity_name, false);

                hp.PosX = en.pos.x;
                hp.PosY = en.pos.y;
                hp.PosZ = en.pos.z;

                hp.RotMatXX = en.rot1.x;
                hp.RotMatXY = en.rot1.y;
                hp.RotMatXZ = en.rot1.z;
                hp.RotMatYX = en.rot2.x;
                hp.RotMatYY = en.rot2.y;
                hp.RotMatYZ = en.rot2.z;
                hp.RotMatZX = en.rot3.x;
                hp.RotMatZY = en.rot3.y;
                hp.RotMatZZ = en.rot3.z;
                hp.Write();

                parent.Nodes.Add(hp.Node);
            }
        }

        THNEditor.ThnParse thn;
            
        public void ImportHardpointsFromTHN(string path)
        {
            thn = new THNEditor.ThnParse();
            thn.Parse(File.ReadAllText(path));

            TreeNode thnnode = new TreeNode("THN");
            thnnode.Name = "THN";

            TreeNode fixnode = new TreeNode("Fixed");
            fixnode.Name = "Fixed";

            TreeNode hpnode = new TreeNode("Hardpoints");
            hpnode.Name = "Hardpoints";

            hpnode.Nodes.Add(fixnode);
            thnnode.Nodes.Add(hpnode);

            treeView1.Nodes[0].Nodes.Add(thnnode);
            BuildImportedHardpoints(fixnode, thn);

            if (treeView1.Nodes.Count <= 1 || treeView1.Nodes[1].Text != "Hardpoints")
            {
                treeView1.Nodes.Insert(1, "Hardpoints");
            }
            else
            {
                treeView1.Nodes[1].Nodes.Clear();
            }

            foreach (THNEditor.ThnParse.Entity en in thn.entities)
            {
                TreeNode hp = new TreeNode(en.entity_name);
                hp.Name = en.entity_name;
                treeView1.Nodes[1].Nodes.Add(hp);
            }
        }

        public void ExportHardpointsToTHN(string path)
        {
            List<THNEditor.ThnParse.Entity> entities = new List<THNEditor.ThnParse.Entity>();
            foreach (TreeNode n in treeView1.Nodes[0].Nodes["THN"].Nodes["Hardpoints"].Nodes["Fixed"].Nodes)
            {
                foreach (THNEditor.ThnParse.Entity e in thn.entities)
                {
                    if (n.Name == e.entity_name)
                    {
                        HardpointData d = new HardpointData(n);
                        e.pos.x = d.PosX;
                        e.pos.y = d.PosY;
                        e.pos.z = d.PosZ;

                        e.rot1.x = d.RotMatXX;
                        e.rot1.y = d.RotMatXY;
                        e.rot1.z = d.RotMatXZ;

                        e.rot2.z = d.RotMatYX;
                        e.rot2.y = d.RotMatYY;
                        e.rot2.z = d.RotMatYZ;

                        e.rot3.x = d.RotMatZX;
                        e.rot3.y = d.RotMatZY;
                        e.rot3.z = d.RotMatZZ;
                        entities.Add(e);
                        break;
                    }
                }
            }
            thn.entities = entities;
            if (!File.Exists(path))
                File.Create(path);
            File.WriteAllText(path, thn.Write());
        }

        /// <summary>
        /// Access hardpoint nodes and generate a file with all hardpoint names
        /// </summary>
        /// <param name="path">Path with filename.ext</param>
        public void ExportHardpointsToFile(string path)
        {
            string hardpointNames = "";
            TreeNode Hardpoints = treeView1.Nodes.Cast<TreeNode>().FirstOrDefault(tn => tn.Text == "Hardpoints");

            foreach (TreeNode hardpointNode in Hardpoints.Nodes)
            {
                if (hardpointNode.Name.StartsWith("Hp"))
                {
                    hardpointNames += hardpointNode.Name + "\n";
                }
            }                            

            File.WriteAllText(path, hardpointNames);
        }

        public void MakeAnimFrames()
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null)
            {
                // If we're not on Channel, assume Header or Frames.
                if (!Utilities.StrIEq(node.Text, "Channel"))
                    node = node.Parent;
                new EditAnimChannel(node).Show(this);
            }
        }

        public void ViewVMeshData()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            try
            {
                byte[] data = treeView1.SelectedNode.Tag as byte[];

                VMeshData decoded = new VMeshData(data);

                StringBuilder st = new StringBuilder(data.Length);
                st.AppendLine("---- HEADER ----");
                st.AppendLine();
                st.AppendFormat("Mesh Type                 = {0}\n", decoded.MeshType);
                st.AppendFormat("Surface Type              = {0}\n", decoded.SurfaceType);
                st.AppendFormat("Number of Meshes          = {0}\n", decoded.NumMeshes);
                st.AppendFormat("Total referenced vertices = {0}\n", decoded.NumRefVertices);
                st.AppendFormat("Flexible Vertex Format    = 0x{0:X}\n", decoded.FlexibleVertexFormat);
                st.AppendFormat("Total number of vertices  = {0}\n", decoded.NumVertices);
                st.AppendLine();

                st.AppendLine("---- MESHES ----");
                st.AppendLine();
                st.AppendLine("Mesh Number  MaterialID  Start Vertex  End Vertex  Start Triangle  NumRefVertex  Padding");
                for (int count = 0; count < decoded.Meshes.Count; count++)
                {
                    st.AppendFormat("{0,11}  0x{1:X8}  {2,12}  {3,10}  {4,14}  {5,12}  0x{6:X2}\n",
                        count,
                        decoded.Meshes[count].MaterialId,
                        decoded.Meshes[count].StartVertex,
                        decoded.Meshes[count].EndVertex,
                        decoded.Meshes[count].TriangleStart/3,
                        decoded.Meshes[count].NumRefVertices,
                        decoded.Meshes[count].Padding);
                }
                st.AppendLine();

                st.AppendLine("---- Triangles ----");
                st.AppendLine();
                st.AppendLine("Triangle  Vertex 1  Vertex 2  Vertex 3");
                for (int count = 0; count < decoded.Triangles.Count; count++)
                {
                    st.AppendFormat("{0,8}  {1,8}  {2,8}  {3,8}\n",
                        count,
                        decoded.Triangles[count].Vertex1,
                        decoded.Triangles[count].Vertex2,
                        decoded.Triangles[count].Vertex3);
                }
                st.AppendLine();

                st.AppendLine("---- Vertices ----");
                st.AppendLine();
                st.Append("Vertex    ----X----,   ----Y----,   ----Z----");
                if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_NORMAL) != 0)
                    st.Append(",    Normal X,    Normal Y,    Normal Z");
                if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_DIFFUSE) != 0)
                    st.Append(", -Diffuse-");
                if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_TEX1) != 0)
                    st.Append(",   ----U----,   ----V----");
                if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_TEX2) != 0)
                    st.Append(",  ----U1----,  ----V1----,  ----U2----,  ----V2----");
                if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_TEX4) != 0)
                    st.Append(",  ----U1----,  ----V1----,   Tangent X,   Tangent Y,   Tangent Z,  Binormal X,  Binormal Y,  Binormal Z,");
                st.AppendLine();
                for (int count = 0; count < decoded.Vertices.Count; count++)
                {
                    st.AppendFormat("{0,6} {1,12:F6},{2,12:F6},{3,12:F6}",
                        count,
                        decoded.Vertices[count].X,
                        decoded.Vertices[count].Y,
                        decoded.Vertices[count].Z);
                    if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_NORMAL) != 0)
                        st.AppendFormat(",{0,12:F6},{1,12:F6},{2,12:F6}",
                            decoded.Vertices[count].NormalX,
                            decoded.Vertices[count].NormalY,
                            decoded.Vertices[count].NormalZ);
                    if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_DIFFUSE) != 0)
                        st.AppendFormat(", 0x{0:X8}", decoded.Vertices[count].Diffuse);
                    if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_TEX1) != 0)
                        st.AppendFormat(",{0,12:F6},{1,12:F6}",
                            decoded.Vertices[count].S,
                            decoded.Vertices[count].T);
                    if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_TEX2) != 0)
                        st.AppendFormat(",{0,12:F6},{1,12:F6},{2,12:F6},{3,12:F6}",
                            decoded.Vertices[count].S,
                            decoded.Vertices[count].T,
                            decoded.Vertices[count].U,
                            decoded.Vertices[count].V);
                    if ((decoded.FlexibleVertexFormat & VMeshData.D3DFVF_TEX4) != 0)
                        st.AppendFormat(",{0,12:F6},{1,12:F6},{2,12:F6},{3,12:F6},{4,12:F6},{5,12:F6},{6,12:F6},{7,12:F6}",
                            decoded.Vertices[count].S,
                            decoded.Vertices[count].T,
                            decoded.Vertices[count].TangentX,
                            decoded.Vertices[count].TangentY,
                            decoded.Vertices[count].TangentZ,
                            decoded.Vertices[count].BinormalX,
                            decoded.Vertices[count].BinormalY,
                            decoded.Vertices[count].BinormalZ);
                    st.AppendLine();
                }

                // Extract the relevant portion from the name.
                string name = treeView1.SelectedNode.Parent.Name;
                // .vms
                int pos = name.LastIndexOf('.');
                // .lod*
                if (pos != -1)
                    pos = name.LastIndexOf('.', pos - 1, pos);
                // .file*
                if (pos != -1)
                    pos = name.LastIndexOf('.', pos - 1, pos);
                new ViewTextForm(name.Substring(pos + 1), st.ToString()).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Error");
            }
        }

        public void ViewVMeshRef()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            try
            {
                byte[] data = treeView1.SelectedNode.Tag as byte[];

                VMeshRef dec = new VMeshRef(data);

                StringBuilder st = new StringBuilder(data.Length);
                st.AppendFormat("Size             = {0}\n", dec.HeaderSize);
                st.AppendFormat("VMeshLibID       = {0}\n", FindVMeshName(dec.VMeshLibId, true));
                st.AppendFormat("StartVert        = {0}\n", dec.StartVert);
                st.AppendFormat("VertQuantity     = {0}\n", dec.NumVert);
                st.AppendFormat("StartIndex       = {0}\n", dec.StartIndex);
                st.AppendFormat("IndexQuantity    = {0}\n", dec.NumIndex);
                st.AppendFormat("StartMesh        = {0}\n", dec.StartMesh);
                st.AppendFormat("MeshQuantity     = {0}\n", dec.NumMeshes);
                st.AppendFormat("BoundingBox.MaxX = {0:F6}\n", dec.BoundingBoxMaxX);
                st.AppendFormat("BoundingBox.MinX = {0:F6}\n", dec.BoundingBoxMinX);
                st.AppendFormat("BoundingBox.MaxY = {0:F6}\n", dec.BoundingBoxMaxY);
                st.AppendFormat("BoundingBox.MinY = {0:F6}\n", dec.BoundingBoxMinY);
                st.AppendFormat("BoundingBox.MaxZ = {0:F6}\n", dec.BoundingBoxMaxZ);
                st.AppendFormat("BoundingBox.MinZ = {0:F6}\n", dec.BoundingBoxMinZ);
                st.AppendFormat("CentreX          = {0:F6}\n", dec.CenterX);
                st.AppendFormat("CentreY          = {0:F6}\n", dec.CenterY);
                st.AppendFormat("CentreZ          = {0:F6}\n", dec.CenterZ);
                st.AppendFormat("Radius           = {0:F6}\n", dec.Radius);

                new ViewTextForm(GetVMeshRefName(treeView1.SelectedNode), st.ToString()).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Error");
            }
        }

        /// <summary>
        /// Get the name associated with the selected VMeshRef node.
        /// </summary>
        /// <returns></returns>
        public string GetVMeshRefName(TreeNode node)
        {
            string name = node.Parent.Parent.Name;
            string level = "";
            if (name.StartsWith("Level", StringComparison.OrdinalIgnoreCase))
            {
                level = "-" + name;
                name = node.Parent.Parent.Parent.Parent.Name;
            }
            return GetName(name) + level + ".vmr";
        }

        public void ViewVWireData()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            try
            {
                byte[] data = treeView1.SelectedNode.Tag as byte[];

                VWireData decoded = new VWireData(data);

                StringBuilder st = new StringBuilder(data.Length);
                st.AppendLine("---- HEADER ----");
                st.AppendLine();
                st.AppendFormat("Structure Size       = {0}\n", decoded.HeaderSize);
                st.AppendFormat("VMeshLibID           = {0}\n", FindVMeshName(decoded.VMeshLibId, true));
                st.AppendFormat("Vertex Base          = {0}\n", decoded.VertexOffset);
                st.AppendFormat("Vertex Quantity      = {0}\n", decoded.NoVertices);
                st.AppendFormat("Ref Vertex Quantity  = {0}\n", decoded.NoRefVertices);
                st.AppendFormat("Vertex Range         = {0}\n", decoded.MaxVertNoPlusOne);
                st.AppendLine();

                st.AppendLine("---- Line Vertex List -----");
                st.AppendLine();
                for (int count = 0; count < decoded.Lines.Count; count++)
                {
                    st.AppendFormat("Line {0,5} = {1,5} to {2,5}\n",
                        count,
                        "v" + decoded.Lines[count].Point1.ToString(),
                        "v" + decoded.Lines[count].Point2.ToString());
                }

                // Find an appropriate name.
                string name = GetName(treeView1.SelectedNode.Parent.Parent.Name);
                new ViewTextForm(name + ".vwd", st.ToString()).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Error");
            }
        }

        private void ViewTexture()
        {
            try
            {
                string name = (treeView1.SelectedNode.Nodes.Count == 0)
                    ? treeView1.SelectedNode.Parent.Name : treeView1.SelectedNode.Name;
                new ViewTextureForm(treeView1.SelectedNode, GetName(name)).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Error");
            }
        }

        /// <summary>
        /// Given the name of a node, strip the timestamp and extension.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetName(string name)
        {
            if (name[0] == '\\')
                return Path.GetFileNameWithoutExtension(fileName);
            
            // Check for an extension.
            int pos = name.LastIndexOf('.');
            
            // Let's assume if the extension is not last,
            // it's the timestamp that follows.
            if (pos != -1)
            {
                if (pos != name.Length - 4)
                    return name.Remove(pos);
                name = name.Remove(pos);
            }

            // If the last twelve characters are digits, assume a timestamp.
            if (name.Length > 12)
            {
                for (pos = 0; pos < 12; ++pos)
                    if (!Char.IsDigit(name[name.Length - 12 + pos]))
                        return name;
                name = name.Remove(name.Length - 12);
            }

            return name;
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            EditNode();
        }

        /// <summary>
        /// Determine if a node contains actual data.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsData(TreeNode node)
        {
            if (node != null && node.Nodes.Count == 0)
            {
                byte[] data = node.Tag as byte[];
                if (data != null && data.Length > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determine if the node is able to be edited.
        /// </summary>
        /// <returns></returns>
        public Editable IsEditable(TreeNode node)
        {
            if (node == null)
                return Editable.No;

            if (FindHardpoint(node) != null)
                return Editable.Hardpoint;

            switch (node.Name.ToLowerInvariant())
            {
                case "vmeshref":
                    return (ContainsData(node)) ? Editable.VMeshRef : Editable.No;

                case "fix":
                case "loose":
                    return Editable.Fix;

                case "rev":
                case "pris":
                    return Editable.Rev;

                case "sphere":
                    return Editable.Sphere;

                case "channel":
                case "header":
                case "frames":
                    return Editable.Channel;

                case "ac":
                case "dc":
                case "ec":
                    return Editable.Color;

                case "constant":
                {
                    // Constant can be either RGB or A.
                    byte[] data = node.Tag as byte[];
                    return (data == null || data.Length == 0 || data.Length == 12)
                           ? Editable.Color : Editable.Float;
                }

                case "bt_name":
                case "child name":
                case "dm0_name":
                case "dm1_name":
                case "dm_name":
                case "dt_name":
                case "et_name":
                case "exporter version":
                case "file name":
                case "m0":
                case "m1":
                case "m2":
                case "m3":
                case "m4":
                case "m5":
                case "m6":
                case "material_name":
                case "name":
                case "object name":
				case "ot_name":
                case "parent name":
                case "type":
                    return Editable.String;

                // single value
                case "count":
                case "edge count":
                case "face count":
                case "flip u":
                case "flip v":
                case "frame count":
                case "image x size":
                case "image y size":
                case "index":
                case "macount":
                case "material count":
                case "material identifier":
                case "material":
                case "normal count":
                case "object vertex count":
                case "sides":
                case "surface normal count":
                case "texture count":
                case "texture vertex count":
                case "uv_bone_id":
                case "uv_vertex_count":
                case "vertex batch count":
                case "vertex count":
                
                // variable array
                case "bone_id_chain":
                case "point_bone_count":
                case "point_indices":
                case "uv0_indices":
                case "uv1_indices":
                    return Editable.Int;

                case "bt_flags":
                case "dm0_flags":
                case "dm1_flags":
                case "dm_flags":
                case "dt_flags":
                case "et_flags":
                case "flags":
                case "maflags":
				case "ot_flags":
                    return Editable.IntHex;
                    
                // single value
                case "alpha":
                case "blend":
                case "bone_x_to_u_scale":
                case "bone_y_to_v_scale":
                case "fade":
                case "fovx":
                case "fovy":
                case "fps":
                case "half x":
                case "half y":
                case "half z":
                case "length":
                case "mass":
                case "max_du":
                case "max_dv":
                case "min_du":
                case "min_dv":
                case "oc":
                case "radius":
                case "root height":
                case "scale":
                case "tilerate":
                case "tilerate0":
                case "tilerate1":
                case "uv_plane_distance":
                case "zfar":
                case "znear":

                // single angular value
                case "max":
                case "min":

                // variable arrays
                case "fractions":
                case "switch2":
                case "bone_weight_chain":
                case "uv0":
                case "uv1":
                case "points":
                case "vertex_normals":
                case "makeys":
                case "edge_angles":
                case "madeltas":

                // matrix
                case "inertia tensor":
                case "orientation":

                // transform
                case "bone to root":
                case "transform":

                // vector
                case "axis":
                case "center of mass":
                case "center":
                case "centroid":
                case "position":
                    return Editable.Float;
            }

            return Editable.No;
        }

        public void EditNode()
        {
            switch (IsEditable(treeView1.SelectedNode))
            {
                case Editable.VMeshRef:  EditVMeshRef();      break;
                case Editable.Fix:       EditFixData(treeView1.SelectedNode.Text); break;
                case Editable.Rev:       EditRevData(treeView1.SelectedNode.Text); break;
                case Editable.Sphere:    EditSphereData();    break;
                case Editable.Channel:   MakeAnimFrames();    break;
                case Editable.Hardpoint: EditHardpoint();     break;
                case Editable.Color:     EditColor();         break;
                case Editable.String:    EditString();        break;
                case Editable.Int:       EditIntArray(false); break;
                case Editable.IntHex:    EditIntArray(true);  break;
                case Editable.Float:     EditFloatArray();    break;
            }
        }

        private void toolStripMenuItemView_Click(object sender, EventArgs e)
        {
            ViewNode();
        }

        /// <summary>
        /// Determine if the node is able to be viewed.
        /// </summary>
        public Viewable IsViewable(TreeNode node)
        {
            if (node.Parent != null && Utilities.StrIEq(node.Parent.Text, "texture library"))
            {
                // Some textures contain animation data, rather than an image.
                return (node.Nodes["MIPS"] == null && node.Nodes["MIP0"] == null)
                       ? Viewable.No : Viewable.Texture;
            }

            if (!ContainsData(node))
                return Viewable.No;

            string text = node.Text;
            
            if (Utilities.StrIEq(text, "VMeshData"))
                return Viewable.VMeshData;

            if (Utilities.StrIEq(text, "VMeshRef"))
                return Viewable.VMeshRef;

            if (Utilities.StrIEq(text, "VWireData"))
                return Viewable.VWireData;

            if (text.StartsWith("MIP", StringComparison.OrdinalIgnoreCase) ||
                Utilities.StrIEq(text, "CUBE"))
                return Viewable.Texture;

            byte[] data = node.Tag as byte[];
            if (data.Length > 16 && data[0] == 'R' && 
                                    data[1] == 'I' && 
                                    data[2] == 'F' && 
                                    data[3] == 'F')
                return Viewable.Wave;

            return Viewable.No;
        }

        public void ViewNode()
        {
            switch (IsViewable(treeView1.SelectedNode))
            {
                case Viewable.VMeshData: ViewVMeshData(); break;
                case Viewable.VMeshRef:  ViewVMeshRef();  break;
                case Viewable.VWireData: ViewVWireData(); break;
                case Viewable.Texture:   ViewTexture();   break;
                case Viewable.Wave:      PlaySound();     break;
            }
        }

        /// <summary>
        /// Play the sound in the selected node. The sound is played in the background.
        /// </summary>
        public void PlaySound()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot play sound from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            byte[] data = treeView1.SelectedNode.Tag as byte[];
            if (data.Length < 16 || !(data[0] == 'R' && data[1] == 'I' && data[2] == 'F' && data[3] == 'F'))
            {
                MessageBox.Show(this, "Not a valid sound leaf", "Error");
                return;
            }

            System.Threading.Thread thread = new System.Threading.Thread(PlaySoundImpl);
            thread.IsBackground = true;
            thread.Start(data);
        }

        [DllImport("WinMM.dll")]
        public static extern bool PlaySound(byte[] wfname, IntPtr hmod, int fuSound);
        public int SND_SYNC = 0x0000; // play synchronously (default)
        public int SND_ASYNC = 0x0001; // play asynchronously
        public int SND_NODEFAULT = 0x0002; // silence (!default) if sound not found
        public int SND_MEMORY = 0x0004; // pszSound points to a memory file

        /// <summary>
        /// Background thread implementation for sound playing.
        /// </summary>
        /// <param name="arg">byte[] to play</param>
        void PlaySoundImpl(object arg)
        {
            byte[] data = arg as byte[];
            try
            {
                PlaySound(data, IntPtr.Zero, SND_SYNC | SND_NODEFAULT | SND_MEMORY);
            }
            catch { }
        }


        /// <summary>
        /// Find the fix or loose node and open an editor for it.
        /// </summary>
        public void EditFixData(string type)
        {
            try
            {
                TreeNode[] nodes = treeView1.Nodes.Find(type, true);
                if (nodes.Length == 0)
                    throw new Exception(type + " node not found");
                treeView1.SelectedNode = nodes[0];
                new EditCmpFixData(this, type, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        /// <summary>
        /// Find the rev or pris node and open an editor for it.
        /// </summary>
        public void EditRevData(string type)
        {
            try
            {
                TreeNode[] nodes = treeView1.Nodes.Find(type, true);
                if (nodes.Length == 0)
                    throw new Exception(type + " node not found");
                treeView1.SelectedNode = nodes[0];
				new EditCmpRevData(this, type, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        /// <summary>
        /// Find the sphere node and open an editor for it.
        /// </summary>
        public void EditSphereData()
        {
            try
            {
                TreeNode[] nodes = treeView1.Nodes.Find("Sphere", true);
                if (nodes.Length == 0)
                    throw new Exception("Sphere node not found");
                treeView1.SelectedNode = nodes[0];
                new EditCmpSphereData(this, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        /// <summary>
        /// stuff for tangent calculation
        /// </summary>
        
        public class Vec3
        {
            public float x, y, z;

            public Vec3(float xx, float yy, float zz)
            {
                x = xx;
                y = yy;
                z = zz;
            }

            public static Vec3 operator +(Vec3 a, Vec3 b)
            {
                a.x += b.x;
                a.y += b.y;
                a.z += b.z;

                return a;
            }

            public static Vec3 operator -(Vec3 a, Vec3 b)
            {
                a.x -= b.x;
                a.y -= b.y;
                a.z -= b.z;

                return a;
            }

            public static Vec3 operator *(Vec3 a, float b)
            {
                a.x *= b;
                a.y *= b;
                a.z *= b;

                return a;
            }

            public static Vec3 operator /(Vec3 a, float b)
            {
                a.x /= b;
                a.y /= b;
                a.z /= b;

                return a;
            }
        };

        public class Vec2
        {
            public float x, y;

            public Vec2(float xx, float yy)
            {
                x = xx;
                y = yy;
            }

            public static Vec2 operator -(Vec2 a, Vec2 b)
            {
                a.x -= b.x;
                a.y -= b.y;

                return a;
            }

        };

        private static void ComputeTangentBasis(
            Vec3 P1, Vec3 P2, Vec3 P3,
            Vec2 UV1, Vec2 UV2, Vec2 UV3,
            ref Vec3 tangent, ref Vec3 binormal)
        {
            Vec3 Edge1 = P2 - P1;
            Vec3 Edge2 = P3 - P1;
            Vec2 Edge1uv = UV2 - UV1;
            Vec2 Edge2uv = UV3 - UV1;

            float cp = Edge1uv.y * Edge2uv.x - Edge1uv.x * Edge2uv.y;

            if (cp != 0.0f)
            {
                float mul = 1.0f / cp;
                tangent = (Edge1 * -Edge2uv.y + Edge2 * Edge1uv.y) * mul;
                binormal = (Edge1 * -Edge2uv.x + Edge2 * Edge1uv.x) * mul;

                normalize(ref tangent, ref tangent);
                normalize(ref binormal, ref binormal);
            }
        }

        private static void normalize(ref Vec3 dest, ref Vec3 src)
        {
            double len = Math.Sqrt(src.x * src.x + src.y * src.y + src.z * src.z);

            if (len == 0)
                return;

            dest = src / (float)len;
        }

        /// <summary>
        /// Calc tangents for model (to use normal mapping)
        /// </summary>
        public void CalcTangents()
        {

            try
            {

                // find vmeshrefs
                foreach (TreeNode node in this.treeView1.Nodes.Find("VMeshRef", true))
                {
                    VMeshRef refdata = new VMeshRef(node.Tag as byte[]);

                    // find vmeshdata
                    foreach (TreeNode meshdata_node in this.treeView1.Nodes.Find("VMeshData", true))
                    {
                        if (Utilities.FLModelCRC(meshdata_node.Parent.Name) == refdata.VMeshLibId)
                        {
                            // found matching meshdata, lets do some work
                            VMeshData meshdata = new VMeshData(meshdata_node.Tag as byte[]);

                            // handle FVF formats

                            switch (meshdata.FlexibleVertexFormat)
                            {
                                case 0x112:
                                    meshdata.FlexibleVertexFormat = 0x412;
                                    break;
                                case 0x212:
                                    meshdata.FlexibleVertexFormat = 0x512;
                                    break;
                                case 0x412:
                                case 0x512:
                                    break;
                                default:
                                    continue;
                            }

                            // iterate meshes
                            for (int iMesh = refdata.StartMesh; iMesh < (refdata.StartMesh+refdata.NumMeshes); iMesh++)
                            {
                                // on every mesh, we calculate tangent data and average it
                                // and then save it back to the VMeshData

                                VMeshData.TMeshHeader mesh = meshdata.Meshes[iMesh];

                                int iVerticeOffset = refdata.StartVert + mesh.StartVertex;
                                int iTriIndexOffset = (mesh.TriangleStart / 3); // refdata.StartIndex is unreliable???!!

                                int iTriangles = (mesh.NumRefVertices / 3);

                                Vec3[] tangentarray = new Vec3[iTriangles];
                                Vec3[] binormalarray = new Vec3[iTriangles];
                                
                                // iterate triangles
                                for (int iTriIndex = iTriIndexOffset; iTriIndex < (iTriIndexOffset + iTriangles); iTriIndex++)
                                {
                                    // first initialize array
                                    int arrindex = iTriIndex - iTriIndexOffset;
                                    tangentarray[arrindex] = new Vec3(1, 0, 0);
                                    binormalarray[arrindex] = new Vec3(0, 1, 0);
                                    

                                    // now get the triangle vertices and calc
                                    VMeshData.TVertex vert1raw = meshdata.Vertices[iVerticeOffset + meshdata.Triangles[iTriIndex].Vertex1];
                                    Vec3 vert1pos = new Vec3(vert1raw.X, vert1raw.Y, vert1raw.Z);
                                    Vec2 vert1uv = new Vec2(vert1raw.S, vert1raw.T);
                                    VMeshData.TVertex vert2raw = meshdata.Vertices[iVerticeOffset + meshdata.Triangles[iTriIndex].Vertex2];
                                    Vec3 vert2pos = new Vec3(vert2raw.X, vert2raw.Y, vert2raw.Z);
                                    Vec2 vert2uv = new Vec2(vert2raw.S, vert2raw.T);
                                    VMeshData.TVertex vert3raw = meshdata.Vertices[iVerticeOffset + meshdata.Triangles[iTriIndex].Vertex3];
                                    Vec3 vert3pos = new Vec3(vert3raw.X, vert3raw.Y, vert3raw.Z);
                                    Vec2 vert3uv = new Vec2(vert3raw.S, vert3raw.T);

                                    ComputeTangentBasis(vert1pos, vert2pos, vert3pos,
                                                        vert1uv, vert2uv, vert3uv,
                                                        ref tangentarray[arrindex], ref binormalarray[arrindex]);

                                }

                                // go through vertices and look up triangle
                                int iEndVertex = iVerticeOffset + (mesh.EndVertex - mesh.StartVertex);
                                for (int iVertIndex = iVerticeOffset; iVertIndex < iEndVertex; iVertIndex++)
                                {
                                    Vec3 tangent = new Vec3(0,0,0);
                                    Vec3 binormal = new Vec3(0,0,0);

                                    int iNumTris = 0;

                                     // iterate triangles
                                    for (int iTriIndex = iTriIndexOffset; iTriIndex < (iTriIndexOffset + iTriangles); iTriIndex++)
                                    {
                                        if (iVerticeOffset + meshdata.Triangles[iTriIndex].Vertex1 == iVertIndex
                                            || iVerticeOffset + meshdata.Triangles[iTriIndex].Vertex2 == iVertIndex
                                            || iVerticeOffset + meshdata.Triangles[iTriIndex].Vertex3 == iVertIndex)
                                        {
                                            // found matching triangle, get tangent/binormal from temporary array if available
                                            int arrindex = iTriIndex - iTriIndexOffset;
                                            tangent += tangentarray[arrindex];
                                            binormal += binormalarray[arrindex];
                                            iNumTris++;                        
                                        }
                                    }

                                    // now average and normalize
                                    tangent /= iNumTris;
                                    binormal /= iNumTris;

                                    normalize(ref tangent, ref tangent);
                                    normalize(ref binormal, ref binormal);

                                    // save
                                    VMeshData.TVertex vertdata = meshdata.Vertices[iVertIndex];
                                    vertdata.TangentX = tangent.x;
                                    vertdata.TangentY = tangent.y;
                                    vertdata.TangentZ = tangent.z;
                                    vertdata.BinormalX = binormal.x;
                                    vertdata.BinormalY = binormal.y;
                                    vertdata.BinormalZ = binormal.z;
                                    meshdata.Vertices[iVertIndex] = vertdata;
                                }
                            }

                            string oldName = meshdata_node.Name;
                            object oldData = meshdata_node.Tag;

                            // save the VMeshData back to the UTF
                            meshdata_node.Tag = meshdata.GetRawData();

                            // communicate change
                            this.NodeChanged(meshdata_node, oldName, oldData);
                        }
                    }
                }

                MessageBox.Show(this, "Tangent/binormal data successfully added!", "Success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }

        }

        

        public void EditVMeshRef()
        {
            try
            {
                new EditVMeshRef(this, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        public void EditHardpoint()
        {
            try
            {
                new EditHardpointData(this, FindHardpoint(treeView1.SelectedNode)).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        /// <summary>
        /// Find the hardpoint associated with a node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeNode FindHardpoint(TreeNode node)
        {
            try
            {
                // In the list, find the actual node.
                if (node.Parent == treeView1.Nodes[1])
                    return treeView1.Nodes[0].Nodes.Find(node.Name, true)[0];
                // On the hardpoint.
                if (Utilities.StrIEq(node.Parent?.Parent?.Name, "Hardpoints"))
                    return node;
                // In the hardpoint.
                if (Utilities.StrIEq(node.Parent?.Parent?.Parent?.Name, "Hardpoints"))
                    return node.Parent;
            }
            catch { }
            return null;
        }
        
        public TreeNode GetSelectedNode()
        {
			return treeView1.SelectedNode;
        }


        /// <summary>
        /// Enable display items in the popup menu depending on the node that
        /// is selected.
        /// </summary>
        /// <param name="node"></param>
        private void UpdateContextMenu(TreeNode node)
        {
            bool is_node = (node != null);
            bool has_data = ContainsData(node);

            // Can only Add nodes to branch nodes.
            toolStripMenuItemAddNode.Enabled = !has_data;

            // Can Rename and Delete any node.
            toolStripMenuItemRenameNode.Enabled =
            toolStripMenuItemDeleteNode.Enabled = is_node;

            // Can Import and Edit As any leaf node.
            toolStripMenuItemImportData.Enabled = 
            stringToolStripMenuItem.Enabled     =
            intArrayToolStripMenuItem.Enabled   =
            floatArrayToolStripMenuItem.Enabled = (is_node && node.Nodes.Count == 0);

            // Can Export any node containing data.
            toolStripMenuItemExportData.Enabled = has_data;

            toolStripMenuItemEdit.Enabled = (IsEditable(node) != Editable.No);

            Viewable view = IsViewable(node);
            toolStripMenuItemView.Enabled = (view != Viewable.No);
            toolStripMenuItemView.Text = (view == Viewable.Wave) ? "Play" : "View";
        }

        bool removeHighlight = false;

        /// <summary>
        /// When a node in the tree is selected update the info box on the parent window.
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Notify the parent to update the node summary area.
            parent.SetSelectedNode(e.Node);
            UpdateContextMenu(e.Node);

            // Redraw the model with the new highlight.
            if (e.Node.FullPath.StartsWith("Parts") || 
                e.Node.FullPath.Contains("Hardpoints"))
            {
                RedrawModel();
                removeHighlight = true;
            }
            else if (removeHighlight)
            {
                RedrawModel();
                removeHighlight = false;
            }
        }

        public void RedrawModel()
        {
            foreach (UTFFormObserver ob in observers)
            {
                (ob as ModelViewForm).Invalidate();
            }
        }

        public bool Cut()
        {
            if (treeView1.SelectedNode != null && !treeView1.SelectedNode.IsEditing)
            {
                treeView1.Cut();

                return true;
            }

            return false;
        }

        public bool Copy()
        {
            if (treeView1.SelectedNode != null && !treeView1.SelectedNode.IsEditing)
            {
                treeView1.Copy();

                return true;
            }

            return false;
        }

        public bool Paste()
        {
            if (!treeView1.SelectedNode.IsEditing)
            {
                treeView1.Paste();

                return true;
            }

            return false;
        }

        public bool PasteChild()
        {
            if (!treeView1.SelectedNode.IsEditing)
            {
                treeView1.PasteChild();

                return true;
            }

            return false;
        }

        public bool Delete()
        {
            if (treeView1.SelectedNode != null && !treeView1.SelectedNode.IsEditing)
            {
                treeView1.Delete();

                return true;
            }

            return false;
        }

        public void ShowModel()
        {
            try
            {
				ModelViewForm modelView = new ModelViewForm(this, treeView1, fileName);
				modelView.Show(this);
				modelView.HardpointMoved += new EventHandler(modelView_HardpointMoved);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

		void modelView_HardpointMoved(object sender, EventArgs e)
		{
            if(treeView1.SelectedNode != null)
			    parent.SetSelectedNode(treeView1.SelectedNode);
			Modified();
		}

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
                return;

            string oldName = e.Node.Name;
            object oldData = e.Node.Tag;
            e.Node.Name = e.Label;
            NodeChanged(e.Node, oldName, oldData);
        }

        private void treeView1_ModifiedNode(object sender, TreeViewEventArgs e)
        {
            NodeChanged(e.Node, null, null);
        }

        /// <summary>
        /// List of data observers.
        /// </summary>
        List<UTFFormObserver> observers = new List<UTFFormObserver>();

        /// <summary>
        /// Register for notifications when node data changes.
        /// </summary>
        /// <param name="ob"></param>
        public void AddObserver(UTFFormObserver ob)
        {
            observers.Add(ob);
        }

        /// <summary>
        /// Remove observing object for node data changes.
        /// </summary>
        /// <param name="ob"></param>
        public void DelObserver(UTFFormObserver ob)
        {
            observers.Remove(ob);
        }

        /// <summary>
        /// Call this function to notify observers when node data changes.
        /// </summary>
        /// <param name="node"></param>
        public void NodeChanged(TreeNode node, string oldName, object oldData)
        {
            bool isHardpoint = Utilities.StrIEq(node?.Parent.Text, "Fixed", "Revolute", "Hardpoints") || Utilities.StrIEq(node.Text, "Fixed", "Revolute", "Hardpoints");

            foreach (UTFFormObserver ob in observers)
                ob.DataChanged(isHardpoint ? DataChangedType.Hardpoints : DataChangedType.Mesh);

            if (node.Parent != null)
            {
                if (Utilities.StrIEq(node.Parent.Name, "VMeshLibrary"))
                {
                    uint oldCRC = Utilities.FLModelCRC(oldName);
                    uint newCRC = Utilities.FLModelCRC(node.Name);

                    TreeNode[] refNodes = FindVMeshRefs(oldCRC);
                    if (refNodes.Length > 0)
                    {
                        if (MessageBox.Show("Automatically update matching VMeshRefLibIDs?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            UpdateVMeshRefs(refNodes, newCRC);
                        }
                    }
                }

                // If this is a hardpoint, rename the other one, too.
                if (Utilities.StrIEq(node.Parent.Text, "Fixed", "Revolute", "Hardpoints"))
                {
                    TreeNode[] onode = treeView1.Nodes.Find(oldName, true);
                    if (onode.Length > 0)
                        onode[0].Text = onode[0].Name = node.Name;
                }
            }
            Modified(node);
        }

        /// <summary>
        /// Find all vmeshref nodes with the matching flmodelcrc.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public TreeNode[] FindVMeshRefs(uint vMeshLibId)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            try
            {
                TreeNode rootNode = treeView1.Nodes[0];
                foreach (TreeNode node in rootNode.Nodes.Find("VMeshRef", true))
                {
                    try
                    {
                        VMeshRef data = new VMeshRef(node.Tag as byte[]);
                        if (data.VMeshLibId == vMeshLibId)
                            nodes.Add(node);
                    }
                    catch { }
                }
            }
            catch { }
            return nodes.ToArray();
        }

        /// <summary>
        /// Update all VMeshRef nodes with newVMeshLibId.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public void UpdateVMeshRefs(TreeNode[] nodes, uint newVMeshLibId)
        {
            foreach (TreeNode node in nodes)
            {
                VMeshRef data = new VMeshRef(node.Tag as byte[]);
                data.VMeshLibId = newVMeshLibId;
                node.Tag = data.GetBytes();
            }

            MessageBox.Show(String.Format("{0} {1} updated.", nodes.Length, (nodes.Length == 1) ? "node" : "nodes"));
        }

        /// <summary>
        /// Search the VMeshData names for a matching crc.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public string FindVMeshName(uint crc, bool code)
        {
            string name = null;
            try
            {
                TreeNode vmesh = treeView1.Nodes[0].Nodes["VMeshLibrary"];
                foreach (TreeNode node in vmesh.Nodes)
                {
                    if (Utilities.FLModelCRC(node.Name) == crc)
                    {
                        name = node.Name;
                        break;
                    }
                }
            }
            catch { }
            if (name == null)
            {
                if (crc == 0xE296602F)
                    name = "interface.generic-2.vms";
                else if (crc == 0x1351B6D4)
                    name = "interface.generic-102.vms";
            }
            if (name == null)
                return String.Format("0x{0:X8}", crc);
            if (code)
                return String.Format("{0} (0x{1:X8})", name, crc);
            return name;
        }


        /// <summary>
        /// Indicate the tree has been modified.
        /// </summary>
        public void Modified()
        {
            if (!fileChangesNotSaved)
            {
                this.Text = this.Text.Insert(0, "*");
                fileChangesNotSaved = true;
            }

			foreach (UTFFormObserver ob in observers)
			{
				ob.Invalidate();
			}
        }

        public void Modified(TreeNode node)
        {
            Modified();
            if (this == parent.ActiveMdiChild &&
                (node == treeView1.SelectedNode || node == treeView1.SelectedNode.Parent))
                parent.SetSelectedNode(treeView1.SelectedNode);
        }
        
        public void SetSelectedNode(TreeNode node)
        {
			treeView1.SelectedNode = node;
        }

        private void UTFForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileChangesNotSaved)
            {
				DialogResult r = MessageBox.Show("Changes not saved. Save now?", "Save Changes for '" + Path.GetFileName(fileName) + "'", MessageBoxButtons.YesNoCancel);
                if (r == DialogResult.Yes)
                {
                    SaveUTFFile(fileName);
                }
                else if (r == DialogResult.Cancel)
                {
					e.Cancel = true;
					return;
				}
            }

            List<UTFFormObserver> observerscopy = new List<UTFFormObserver>(observers);
            foreach (UTFFormObserver ob in observerscopy)
			{
				ob.Close();
			}
        }

        private void UTFForm_Activated(object sender, EventArgs e)
        {
            parent.SetSelectedNode(treeView1.SelectedNode);
        }

        // Can't find a word that combines expand/collapse, so I'll make one up,
        // based on inflate/deflate.
        private void treeView1_BeforeFlate(object sender, TreeViewCancelEventArgs e)
        {
            if (doubleClicked)
            {
                if (e.Node == treeView1.SelectedNode &&
                    (IsEditable(e.Node) != Editable.No ||
                     IsViewable(e.Node) != Viewable.No))
                {
                    e.Cancel = true;
                }
                doubleClicked = false;
            }
        }

        private void treeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                parent.SelectGrid();
            }
        }

        internal void ImportTextures(string[] textures)
        {
            bool appendTga = false, overwrite = false;
            switch (MessageBox.Show("Do you want to include a .tga extension?", "Naming Scheme", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    appendTga = true;
                    break;
                case DialogResult.No:
                    appendTga = false;
                    break;
                case DialogResult.Cancel:
                    return;
            }

            TreeNode rootNode = treeView1.Nodes[0];
            TreeNode[] tlibs = rootNode.Nodes.Find("Texture library", false);

            TreeNode textureLibrary;

            if (tlibs.Length > 0)
            {
                textureLibrary = tlibs[0];

                foreach (string t in textures)
                {
                    string n = Path.GetFileNameWithoutExtension(t);
                    if (appendTga) n += ".tga";

                    TreeNode[] tx = textureLibrary.Nodes.Find(n, false);
                    if (tx.Length > 0)
                    {
                        if (!overwrite)
                        {
                            if (MessageBox.Show("WARNING: Some textures already exist. Overwrite?", "Naming Scheme", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                overwrite = true;
                            else
                                return;
                        }

                        tx[0].Remove();
                        NodeChanged(tx[0], "", null);
                    }
                }
            }
            else
                textureLibrary = rootNode.Nodes.Add("Texture library");

            foreach (string t in textures)
            {
                string n = Path.GetFileNameWithoutExtension(t);
                if (appendTga) n += ".tga";

                string ext = Path.GetExtension(t).ToLower();

                Byte[] contents = File.ReadAllBytes(t);

                TreeNode node = textureLibrary.Nodes.Add(n);
                node.Name = n;
                node.Tag = new byte[0];
                TreeNode texnode = node.Nodes.Add(ext == ".dds" ? "MIPS" : "MIP0");
                texnode.Name = texnode.Text;
                texnode.Tag = contents;
            }
            NodeChanged(textureLibrary, "", null);
        }

        internal void ReplaceAll(string find, string replace, bool whole, bool content, bool name)
        {
            ReplaceNodeAndChildren(treeView1.Nodes[0], find, replace, whole, content, name);

            Modified();
        }

        void ReplaceNodeAndChildren(TreeNode n, string find, string replace, bool whole, bool content, bool name)
        {
            if (name)
            {
                if (whole && n.Name == find)
                    n.Text = n.Name = replace;
                else if (!whole)
                    n.Text = n.Name = n.Name.Replace(find, replace);
            }

            if (content && IsEditable(n) == Editable.String)
            {
                byte[] data = n.Tag as byte[];
                string txt = Encoding.ASCII.GetString(data);

                if (whole && txt == find)
                    txt = replace;
                else if(!whole)
                    txt = txt.Replace(find, replace);

                n.Tag = Encoding.ASCII.GetBytes(txt + "\u0000");
            }

            foreach (TreeNode node in n.Nodes)
                ReplaceNodeAndChildren(node, find, replace, whole, content, name);
        }

        HashSet<string> rescaledHPs = new HashSet<string>();
        internal void RescaleModel(float scaling)
        {
            rescaledHPs.Clear();

            foreach (TreeNode n in treeView1.Nodes)
            {
                if (n.Name == "\\")
                {
                    TraverseRescaleModel(n, scaling);
                    Modified(n);
                }
            }

            rescaledHPs.Clear();

            foreach (UTFFormObserver ob in observers)
                ob.DataChanged(DataChangedType.All);
        }

        void TraverseRescaleModel(TreeNode p, float scaling)
        {
            foreach (TreeNode n in p.Nodes)
            {
                switch (IsEditable(n))
                {
                    case Editable.VMeshRef: RescaleVMeshRef(n, scaling); break;
                    case Editable.Fix: RescaleFixData(n, scaling); break;
                    case Editable.Rev: RescaleRevData(n, scaling); break;
                    case Editable.Hardpoint: RescaleHardpoint(n, scaling); break;
                }

                if (n.Name.ToLowerInvariant() == "vmeshdata")
                    RescaleVMeshData(n, scaling);

                TraverseRescaleModel(n, scaling);
            }
        }

        private void RescaleVMeshData(TreeNode n, float scaling)
        {
            byte[] data = n.Tag as byte[];

            VMeshData s = new VMeshData(data);

            s.Vertices = s.Vertices.Select(v =>
            {
                v.X *= scaling;
                v.Y *= scaling;
                v.Z *= scaling;

                return v;
            }).ToList();

            n.Tag = s.GetRawData();
        }

        private void RescaleHardpoint(TreeNode n, float scaling)
        {
            var hp = FindHardpoint(n);
            if (hp == null || rescaledHPs.Contains(hp.Name))
                return;

            HardpointData s = new HardpointData(hp);

            s.PosX *= scaling;
            s.PosY *= scaling;
            s.PosZ *= scaling;

            s.Write();

            rescaledHPs.Add(s.Name);
        }

        private void RescaleRevData(TreeNode n, float scaling)
        {
            byte[] data = n.Tag as byte[];

            CmpRevData s = new CmpRevData(data);

            foreach(var p in s.Parts)
            {
                p.OffsetX *= scaling;
                p.OffsetY *= scaling;
                p.OffsetZ *= scaling;

                p.OriginX *= scaling;
                p.OriginY *= scaling;
                p.OriginZ *= scaling;
            }

            n.Tag = s.GetBytes();
        }

        private void RescaleFixData(TreeNode n, float scaling)
        {
            byte[] data = n.Tag as byte[];

            CmpFixData s = new CmpFixData(data);

            foreach (var p in s.Parts)
            {
                p.OriginX *= scaling;
                p.OriginY *= scaling;
                p.OriginZ *= scaling;
            }

            n.Tag = s.GetBytes();
        }

        private void RescaleVMeshRef(TreeNode n, float scaling)
        {
            byte[] data = n.Tag as byte[];

            VMeshRef s = new VMeshRef(data);

            s.BoundingBoxMaxX *= scaling;
            s.BoundingBoxMaxY *= scaling;
            s.BoundingBoxMaxZ *= scaling;
            s.BoundingBoxMinX *= scaling;
            s.BoundingBoxMinY *= scaling;
            s.BoundingBoxMinZ *= scaling;

            s.CenterX *= scaling;
            s.CenterY *= scaling;
            s.CenterZ *= scaling;

            s.Radius *= scaling;

            n.Tag = s.GetBytes();
        }
    }
}
