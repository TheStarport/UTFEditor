using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Collections;

namespace UTFEditor
{
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
        /// This object encapsulates treenodes to copy from one treeview to another.
        /// </summary>
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
            this.Text = name;
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
            treeView1.Nodes.Add(utfFile.LoadUTFFile(filePath));
            treeView1.Nodes[0].Expand();
        }

        /// <summary>
        /// Save the data in the treeview displayed by this form back into
        /// the specified file.
        /// </summary>
        /// <param name="filename"></param>
        public void SaveUTFFile(string filePath)
        {
            utfFile.SaveUTFFile(treeView1.Nodes[0], filePath);
        }

        /// <summary>
        /// On mouse down select the treenode under the pointer
        /// </summary>
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = new Point(e.X, e.Y);
                treeView1.PointToClient(pt);
                treeView1.SelectedNode = treeView1.GetNodeAt(pt);
            }
        }

        /// <summary>
        /// On mouse up show the context menu.
        /// </summary>
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(treeView1, new Point(e.X, e.Y));
        }

        /// <summary>
        /// Start dragging the selected treenode
        /// </summary>
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            CopyNodesObject obj = new CopyNodesObject();
            obj.Nodes.Add(treeView1.SelectedNode);
            DoDragDrop(obj, DragDropEffects.Copy);
        }

        /// <summary>
        /// If a treenode is being dragged into the treeview then set
        /// the drag state.
        /// </summary>
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(CopyNodesObjectName, true))
            {
                e.Effect = DragDropEffects.Copy;
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
            }
        }

        /// <summary>
        /// Copy the tree node into the this treeview
        /// </summary>
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(CopyNodesObjectName, true))
            {
                CopyNodesObject obj = e.Data.GetData(CopyNodesObjectName, true) as CopyNodesObject;
                foreach (TreeNode node in obj.Nodes)
                {
                    if (node == treeView1.SelectedNode)
                        continue;

                    if (treeView1.SelectedNode == null)
                    {
                        treeView1.Nodes.Add((TreeNode)node.Clone());
                    }
                    else
                    {
                        treeView1.SelectedNode.Nodes.Add((TreeNode)node.Clone());
                        treeView1.SelectedNode.Expand();
                    }
                }
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
        }

        public void DeleteSelectedNodes()
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode node = treeView1.SelectedNode;
                treeView1.SelectedNode = node.Parent;
                node.Remove();
            }
        }

        public void RenameNode()
        {
            if (treeView1.SelectedNode != null)
                new RenameNodeForm(this, treeView1.SelectedNode).Show(this);
        }

        private void stringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditString();
        }

        public void EditString()
        {
            if (treeView1.SelectedNode != null)
            {
                new EditStringForm(treeView1.SelectedNode).Show(this);
                parent.SetSelectedNode(treeView1.SelectedNode);
            }
        }

        private void intArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditIntArray();
        }

        public void EditIntArray()
        {
            if (treeView1.SelectedNode != null)
            {
                new EditIntArrayForm(treeView1.SelectedNode).Show(this);
                parent.SetSelectedNode(treeView1.SelectedNode);
            }

        }

        private void floatArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFloatArray();

        }

        public void EditFloatArray()
        {
            if (treeView1.SelectedNode != null)
            {
                new EditFloatArrayForm(treeView1.SelectedNode).Show(this);
                parent.SetSelectedNode(treeView1.SelectedNode);
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
            if (treeView1.SelectedNode != null)
            {
                new EditAnimChannel(treeView1.SelectedNode).Show(this);
                parent.SetSelectedNode(treeView1.SelectedNode);
            }
        }

        public void ExportVMeshData()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            saveFileDialog1.Filter = "Text (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    byte[] data = treeView1.SelectedNode.Tag as byte[];

                    VMeshData decoded = new VMeshData(data);

                    using (StreamWriter st = new StreamWriter(saveFileDialog1.FileName))
                    {
                        st.WriteLine("---- HEADER ----");
                        st.WriteLine();
                        st.WriteLine("Mesh Type                 = {0}", decoded.MeshType);
                        st.WriteLine("Surface Type              = {0}", decoded.SurfaceType);
                        st.WriteLine("Number of Meshes          = {0}", decoded.NumMeshes);
                        st.WriteLine("Total referenced vertices = {0}", decoded.NumRefVertices);
                        st.WriteLine("Flexible Vertex Format    = 0x{0:X}", decoded.FlexibleVertexFormat);
                        st.WriteLine("Total number of vertices  = {0}", decoded.NumVertices);
                        st.WriteLine();

                        st.WriteLine("---- MESHES ----");
                        st.WriteLine();
                        st.WriteLine("Mesh Number\tMaterialID\tStart Vertex\tEnd Vertex\tQtyRefVertex\tPadding");
                        for (int count = 0; count < decoded.Meshes.Count; count++)
                        {
                            st.WriteLine("{0:00000000}\t0x{1:X8}\t{2:00000000}\t{3:00000000}\t{4:00000000}\t0x{5:X2}",
                                count,
                                decoded.Meshes[count].MaterialId,
                                decoded.Meshes[count].StartVertex,
                                decoded.Meshes[count].EndVertex,
                                decoded.Meshes[count].NumRefVertices,
                                decoded.Meshes[count].Padding);
                        }
                        st.WriteLine();

                        st.WriteLine("---- Triangles ----");
                        st.WriteLine();
                        st.WriteLine("Triangle\tVertex 1\tVertex 2\tVertex 3");
                        for (int count = 0; count < decoded.Triangles.Count; count++)
                        {
                            st.WriteLine("{0:00000000}\t{1:00000000}\t{2:00000000}\t{3:00000000}",
                                count,
                                decoded.Triangles[count].Vertex1,
                                decoded.Triangles[count].Vertex2,
                                decoded.Triangles[count].Vertex3);
                        }
                        st.WriteLine();

                        st.WriteLine("---- Vertices ----");
                        st.WriteLine();
                        st.WriteLine("Vertex\t   ----X----,   ----Y----,   ----Z----,    Normal X,   Normal Y,   Normal Z,   ----U----,   ----V----");
                        for (int count = 0; count < decoded.Vertices.Count; count++)
                        {
                            st.WriteLine("{0:00000000}\t{1,12:0.000000},{2,12:0.000000},{3,12:0.000000},{4,12:0.000000},{5,12:0.000000},{6,12:0.000000},{7,12:0.000000}, {8,12:0.000000}",
                                count,
                                decoded.Vertices[count].X,
                                decoded.Vertices[count].Y,
                                decoded.Vertices[count].Z,
                                decoded.Vertices[count].NormalX,
                                decoded.Vertices[count].NormalY,
                                decoded.Vertices[count].NormalZ,
                                decoded.Vertices[count].U,
                                decoded.Vertices[count].V);
                        }

                        st.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Error");
                }
            }
        }

        public void ExportVMeshRef()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            saveFileDialog1.Filter = "Text (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    byte[] data = treeView1.SelectedNode.Tag as byte[];

                    VMeshRef dec = new VMeshRef(data);

                    using (StreamWriter st = new StreamWriter(saveFileDialog1.FileName))
                    {
                        st.WriteLine("Size             = {0}", dec.HeaderSize);
                        st.WriteLine("VMeshLibID       = 0x{0:X8}", dec.VMeshLibId);
                        st.WriteLine("StartVert        = {0}", dec.StartVert);
                        st.WriteLine("VertQuantity     = {0}", dec.NumVert);
                        st.WriteLine("StartRefVert     = {0}", dec.StartRefVert);
                        st.WriteLine("RefVertQuantity  = {0}", dec.NumRefVert);
                        st.WriteLine("MeshNumber       = {0}", dec.StartMesh);
                        st.WriteLine("NumberOfMeshes   = {0}", dec.NumMeshes);
                        st.WriteLine("BoundingBox.maxx = {0:0.000000}", dec.BoundingBoxMaxX);
                        st.WriteLine("BoundingBox.minx = {0:0.000000}", dec.BoundingBoxMinX);
                        st.WriteLine("BoundingBox.maxy = {0:0.000000}", dec.BoundingBoxMaxY);
                        st.WriteLine("BoundingBox.miny = {0:0.000000}", dec.BoundingBoxMinY);
                        st.WriteLine("BoundingBox.maxz = {0:0.000000}", dec.BoundingBoxMaxZ);
                        st.WriteLine("BoundingBox.minz = {0:0.000000}", dec.BoundingBoxMinZ);
                        st.WriteLine("CentreX          = {0:0000000}", dec.CenterX);
                        st.WriteLine("CentreY          = {0:0.000000}", dec.CenterY);
                        st.WriteLine("CentreZ          = {0:0.000000}", dec.CenterZ);
                        st.WriteLine("Radius           = {0:0.000000}", dec.Radius);
                        st.WriteLine();
                        st.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Error");
                }
            }
        }

        public void ExportVWireData()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot export data from non-leaf nodes or multiple nodes", "Error");
                return;
            }

            saveFileDialog1.Filter = "Text (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    byte[] data = treeView1.SelectedNode.Tag as byte[];

                    VWireData decoded = new VWireData(data);

                    using (StreamWriter st = new StreamWriter(saveFileDialog1.FileName))
                    {
                        st.WriteLine("---- HEADER ----");
                        st.WriteLine();
                        st.WriteLine("Structure Size       = {0}", decoded.HeaderSize);
                        st.WriteLine("VMeshLibID           = 0x{0:X8}", decoded.VMeshLibId);
                        st.WriteLine("Buffer Offset        = 0x{0:X4}", decoded.VertexOffset);
                        st.WriteLine("Vertex Quantity      = {0}", decoded.NoVertices);
                        st.WriteLine("Ref Vertex Quantity  = {0}", decoded.NoRefVertices);
                        st.WriteLine("Vertex Range         = {0}", decoded.MaxVertNoPlusOne);
                        st.WriteLine();

                        st.WriteLine("---- Line Vertex List -----");
                        st.WriteLine();
                        for (int count = 0; count < decoded.Lines.Count; count++)
                        {
                            st.WriteLine("Line {0} = v{1} to v{2}",
                                count,
                                decoded.Lines[count].Point1,
                                decoded.Lines[count].Point2);
                        }
                        st.WriteLine();

                        st.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Error");
                }
            }
        }

        private void vMeshDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportVMeshData();
        }

        private void vMeshRefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportVMeshRef();
        }

        private void vWireDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportVWireData();
        }

        private void toolStripMenuItemPlaySound_Click(object sender, EventArgs e)
        {
            PlaySound();
        }

        /// <summary>
        /// Play the sound in the selected node. The sound is played in the background.
        /// </summary>
        public void PlaySound()
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show(this, "Cannot play sound from non-leaf nodes or multipe nodes", "Error");
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
        /// Find the fix node and open an editor for it.
        /// </summary>
        public void EditFixData()
        {
            try
            {
                TreeNode[] nodes = treeView1.Nodes.Find("Fix", true);
                if (nodes.Length != 1)
                    throw new Exception("Fix node not found");
                treeView1.SelectedNode = nodes[0];
                new EditCmpFixData(this, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        /// <summary>
        /// Find the pris node and open an editor for it.
        /// </summary>
        public void EditPrisData()
        {
            try
            {
                TreeNode[] nodes = treeView1.Nodes.Find("Pris", true);
                if (nodes.Length != 1)
                    throw new Exception("Pris node not found");
                treeView1.SelectedNode = nodes[0];
                new EditCmpPrisData(this, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }

        /// <summary>
        /// Find the rev node and open an editor for it.
        /// </summary>
        public void EditRevData()
        {
            try
            {
                TreeNode[] nodes = treeView1.Nodes.Find("Rev", true);
                if (nodes.Length != 1)
                    throw new Exception("Rev node not found");
                treeView1.SelectedNode = nodes[0];
                new EditCmpRevData(this, treeView1.SelectedNode).Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error '" + ex.Message + "'", "Error");
            }
        }


        /// <summary>
        /// Enable display items in the popup menu depending on the node that
        /// is selected.
        /// </summary>
        /// <param name="node"></param>
        private void UpdateContextMenu(TreeNode node)
        {
            toolStripMenuItemAddNode.Enabled = true;
            toolStripMenuItemDeleteNode.Enabled = false;

            toolStripMenuItemExportData.Enabled = false;
            toolStripMenuItemImportData.Enabled = false;

            toolStripMenuItemPlaySound.Enabled = false;

            intArrayToolStripMenuItem.Enabled = false;
            floatArrayToolStripMenuItem.Enabled = false;
            stringToolStripMenuItem.Enabled = false;

            vMeshDataToolStripMenuItem.Enabled = false;
            vMeshRefToolStripMenuItem.Enabled = false;
            vWireDataToolStripMenuItem.Enabled = false;

            if (node != null)
            {
                byte[] data = node.Tag as byte[];
                if (data != null)
                {
                    if (node.Text == "VMeshData" && data.Length > 0)
                    {
                        vMeshDataToolStripMenuItem.Enabled = true;
                    }

                    if (node.Text == "VMeshRef" && data.Length > 0)
                    {
                        vMeshRefToolStripMenuItem.Enabled = true;
                    }

                    if (node.Text == "VWireData" && data.Length > 0)
                    {
                        vWireDataToolStripMenuItem.Enabled = true;
                    }

                    if (node.Text.StartsWith("0x") && data.Length > 0 && data[0] == 'R')
                    {
                        toolStripMenuItemPlaySound.Enabled = true;
                    }

                    if (node.Nodes.Count > 0)
                    {
                        toolStripMenuItemDeleteNode.Enabled = true;
                    }
                    else
                    {
                        toolStripMenuItemDeleteNode.Enabled = true;

                        toolStripMenuItemImportData.Enabled = true;
                        toolStripMenuItemExportData.Enabled = true;

                        stringToolStripMenuItem.Enabled = true;
                        intArrayToolStripMenuItem.Enabled = true;
                        floatArrayToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// When a node in the tree is selected update the info box on the parent window.
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Notify the parent to update the node summary area.
            parent.SetSelectedNode(e.Node);
            UpdateContextMenu(e.Node);
        }

        public void Cut()
        {
            if (treeView1.SelectedNode != null)
            {
                CopyNodesObject obj = new CopyNodesObject();
                TreeNode node = treeView1.SelectedNode;
                obj.Nodes.Add(node);
                node.Remove();
                Clipboard.SetData(CopyNodesObjectName, obj);
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
                foreach (TreeNode node in obj.Nodes)
                {
                    if (node == treeView1.SelectedNode)
                        continue;

                    if (treeView1.SelectedNode == null)
                    {
                        treeView1.Nodes.Add((TreeNode)node.Clone());
                    }
                    else
                    {
                        treeView1.SelectedNode.Nodes.Add((TreeNode)node.Clone());
                        treeView1.SelectedNode.Expand();
                    }
                }
            }
        }

        public void ShowModel()
        {
            try
            {

                new ModelViewForm(this, treeView1.Nodes[0], Path.GetDirectoryName(utfFile.FilePath)).Show(this);
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

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
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

            if (node.Parent != null && node.Parent.Name == "VMeshLibrary")
            {
                uint oldCRC = Utilities.FLModelCRC(oldName);
                uint newCRC = Utilities.FLModelCRC(node.Name);
                
                TreeNode[] refNodes = FindVMeshRefs(oldCRC);
                if (refNodes.Length > 0)
                {
                    if (MessageBox.Show("Automatically update matching VMeshRefLibIDs?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        UpdateVMeshRefs(oldCRC, newCRC);
                    }
                }
            }

            fileChangesNotSaved = true;
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
        /// Update all vmeshref nodes with the matching newVMeshLibId.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public void UpdateVMeshRefs(uint oldVMeshLibId, uint newVMeshLibId)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            try
            {
                int changesMade = 0;

                TreeNode rootNode = treeView1.Nodes[0];
                foreach (TreeNode node in rootNode.Nodes.Find("VMeshRef", true))
                {
                    try
                    {
                        VMeshRef data = new VMeshRef(node.Tag as byte[]);
                        if (data.VMeshLibId == oldVMeshLibId)
                        {
                            data.VMeshLibId = newVMeshLibId;
                            node.Tag = data.GetBytes();
                            changesMade++;
                        }
                    }
                    catch { }
                }

                MessageBox.Show(String.Format("{0} nodes updated", changesMade));
            }
            catch { }
        }

        private void UTFForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileChangesNotSaved)
            {
                if (MessageBox.Show("Changes not saved. Save now?", "Save Changes for '" + Path.GetFileName(utfFile.FilePath) + "'", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveUTFFile(utfFile.FilePath);
                }
            }
        }
    }
}

