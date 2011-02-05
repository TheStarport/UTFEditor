using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        UTFEditor parent;

        /// <summary>
        /// THe name of the object used for drag-down operations.
        /// </summary>
        private const string CopyNodesObjectName = "UTFEditor.UTFForm+CopyNodesObject";

        /// <summary>
        /// True if there are pending file changes that have not been saved.
        /// </summary>
        private bool fileChangesNotSaved = false;

        /// <summary>
        /// The UTFFile this form is displaying.
        /// </summary>
        UTFFile utfFile = new UTFFile();

        /// <summary>
        /// The name of the UTF file.
        /// </summary>
        public string fileName;

        /// <summary>
        /// This object encapsulates treenodes to copy from one treeview to another.
        /// </summary>
        [Serializable]
        private class CopyNodesObject
        {
            public List<TreeNode> Nodes = new List<TreeNode>();
        };


        /// <summary>
        /// Create an empty form.
        /// </summary>
        public UTFForm(UTFEditor parent, string name)
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

        /// <summary>
        /// On mouse down select the treenode under the pointer
        /// </summary>
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
            }
            doubleClicked = (e.Clicks > 1);
        }

        /// <summary>
        /// Start dragging the selected treenode
        /// </summary>
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            CopyNodesObject obj = new CopyNodesObject();
            obj.Nodes.Add((TreeNode)e.Item);
            DoDragDrop(obj, DragDropEffects.Copy | DragDropEffects.Move);
        }

        /// <summary>
        /// If a treenode is being dragged into the treeview then set
        /// the drag state.
        /// </summary>
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(CopyNodesObjectName, true))
            {
                e.Effect = e.AllowedEffect;
                treeView1.Focus();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Select the treenode under the point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(CopyNodesObjectName, true))
            {
                Point pt = new Point(e.X, e.Y);
                pt = treeView1.PointToClient(pt);
                treeView1.SelectedNode = treeView1.GetNodeAt(pt);
                
                // Shift to move, control to copy.
                // Default is copy, unless the parents are the same, then move.
                if ((e.KeyState & 4) != 0)
                    e.Effect = DragDropEffects.Move;
                else if ((e.KeyState & 8) != 0)
                    e.Effect = DragDropEffects.Copy;
                else
                {
                    try
                    {
                        CopyNodesObject obj = e.Data.GetData(CopyNodesObjectName, true) as CopyNodesObject;
                        TreeNode node = obj.Nodes[0];
                        if (node.Parent.FullPath == treeView1.SelectedNode.Parent.FullPath)
                            e.Effect = DragDropEffects.Move;
                        else
                            e.Effect = DragDropEffects.Copy;
                    }
                    catch 
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        /// <summary>
        /// Copy the tree node into this treeview
        /// </summary>
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(CopyNodesObjectName, true))
            {
                CopyNodesObject obj = e.Data.GetData(CopyNodesObjectName, true) as CopyNodesObject;
                CopyNodes(obj, e.Effect);
            }
        }

        private void CopyNodes(CopyNodesObject obj, DragDropEffects effect)
        {
            foreach (TreeNode node in obj.Nodes)
            {
                if (node == treeView1.SelectedNode)
                    continue;

                TreeNode newNode;
                bool move = false;
                if (effect == DragDropEffects.Move)
                {
                    try
                    {
                        // If the nodes have the same parent, shift the order,
                        // rather than making it a child.
                        if (node.Parent.FullPath == treeView1.SelectedNode.Parent.FullPath)
                            move = true;
                    }
                    catch { }
                    (node.TreeView.FindForm() as UTFForm).Modified();
                    node.Remove();
                    newNode = node;
                }
                else
                {
                    newNode = (TreeNode)node.Clone();
                }
                if (treeView1.SelectedNode == null)
                {
                    treeView1.Nodes.Add(newNode);
                }
                else
                {
                    if (move)
                        treeView1.SelectedNode.Parent.Nodes.Insert(treeView1.SelectedNode.Index, newNode);
                    else
                        treeView1.SelectedNode.Nodes.Add(newNode);
                    treeView1.SelectedNode = newNode;
                }
                Modified();
            }
        }

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
            if (treeView1.SelectedNode != null && e.KeyCode == Keys.Enter)
            {
                if (toolStripMenuItemEdit.Enabled)
                    EditNode();
                else if (toolStripMenuItemView.Enabled)
                    ViewNode();
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
            if (treeView1.SelectedNode != null)
            {
                TreeNode node = treeView1.SelectedNode;
                treeView1.SelectedNode = node.Parent;
                node.Remove();
                Modified();
            }
        }

        public void RenameNode()
        {
            if (treeView1.SelectedNode != null)
            {
                if (new RenameNodeForm(this, treeView1.SelectedNode).ShowDialog(this) == DialogResult.OK)
                {
                    parent.SetSelectedNode(treeView1.SelectedNode);
                    Modified();
                }
                treeView1.Select();
            }
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
                st.AppendLine("Mesh Number  MaterialID  Start Vertex  End Vertex  QtyRefVertex  Padding");
                for (int count = 0; count < decoded.Meshes.Count; count++)
                {
                    st.AppendFormat("{0,11}  0x{1:X8}  {2,12}  {3,10}  {4,12}  0x{5:X2}\n",
                        count,
                        decoded.Meshes[count].MaterialId,
                        decoded.Meshes[count].StartVertex,
                        decoded.Meshes[count].EndVertex,
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
                        st.AppendFormat(",{0,12:F6},{1,12:F6},{2,12:F6}, {3,12:F6}",
                            decoded.Vertices[count].S,
                            decoded.Vertices[count].T,
                            decoded.Vertices[count].U,
                            decoded.Vertices[count].V);
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
        private TreeNode FindHardpoint(TreeNode node)
        {
            try
            {
                // In the list, find the actual node.
                if (node.Parent == treeView1.Nodes[1])
                    return treeView1.Nodes[0].Nodes.Find(node.Name, true)[0];
                // On the hardpoint.
                if (Utilities.StrIEq(node.Parent.Parent.Name, "Hardpoints"))
                    return node;
                // In the hardpoint.
                if (Utilities.StrIEq(node.Parent.Parent.Parent.Name, "Hardpoints"))
                    return node.Parent;
            }
            catch { }
            return null;
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

        public void Cut()
        {
            if (treeView1.SelectedNode != null)
            {
                Copy();
                treeView1.SelectedNode.Remove();
                Modified();
            }
        }

        public void Copy()
        {
            if (treeView1.SelectedNode != null)
            {
                CopyNodesObject obj = new CopyNodesObject();
                TreeNode node = treeView1.SelectedNode;
                obj.Nodes.Add(node);
                Clipboard.SetData(CopyNodesObjectName, obj);
            }
        }

        public void Paste()
        {
            if (Clipboard.ContainsData(CopyNodesObjectName))
            {
                CopyNodesObject obj = Clipboard.GetData(CopyNodesObjectName) as CopyNodesObject;
                CopyNodes(obj, DragDropEffects.Copy);
            }
        }

        public void ShowModel()
        {
            try
            {
                new ModelViewForm(this, treeView1, fileName).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
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
            foreach (UTFFormObserver ob in observers)
            {
                ob.DataChanged(node, oldName, oldData);
            }

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
        }

        public void Modified(TreeNode node)
        {
            Modified();
            if (this == parent.ActiveMdiChild &&
                (node == treeView1.SelectedNode || node == treeView1.SelectedNode.Parent))
                parent.SetSelectedNode(treeView1.SelectedNode);
        }

        private void UTFForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileChangesNotSaved)
            {
                if (MessageBox.Show("Changes not saved. Save now?", "Save Changes for '" + Path.GetFileName(fileName) + "'", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveUTFFile(fileName);
                }
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
    }
}