using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public partial class VMeshDataNodeNameFixer : Form
    {
        UTFEditorMain parent;

        public VMeshDataNodeNameFixer(UTFEditorMain parent)
        {
            this.parent = parent;
            InitializeComponent();
            listBoxOptions.SelectedItem = "Scan Only";
        }

        private string scanOptions = "Scan Only";
        private BackgroundWorker bgScanMeshNodesWkr = null;

        private void buttonScan_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select directory to scan";
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if (bgScanMeshNodesWkr == null)
                {
                    scanOptions = listBoxOptions.SelectedItem.ToString();

                    bgScanMeshNodesWkr = new BackgroundWorker();
                    bgScanMeshNodesWkr.DoWork += new DoWorkEventHandler(ScanForNonUniqueMeshNodes);
                    bgScanMeshNodesWkr.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanForNonUniqueMeshNodesCompleted);
                    bgScanMeshNodesWkr.WorkerReportsProgress = true;
                    bgScanMeshNodesWkr.WorkerSupportsCancellation = true;
                    bgScanMeshNodesWkr.RunWorkerAsync(folderBrowserDialog1.SelectedPath);
                }
            }
        }

        /// <summary>
        /// Add an error entry to the log file.
        /// </summary>
        /// <param name="details">A human readable description.</param>
        /// <param name="accountID">If the log entry is related to a file operation 
        /// then this parameter contains a path to the directory containing the file.</param>
        public void AddLog(string details)
        {
            this.Invoke(new UpdateUIAddLogDelegate(UpdateUIAddLog), new object[] { details });
        }

        /// <summary>
        /// A delegate that always runs in the UI thread. This updates the database
        /// which in turn updates the log table.
        /// </summary>
        /// <param name="details"></parfam>
        /// <param name="dirPath"></param>
        delegate void UpdateUIAddLogDelegate(string details);
        protected void UpdateUIAddLog(string details)
        {
            richTextBox1.AppendText(details + "\n");
            richTextBox1.ScrollToCaret();
        }

        public void OpenEdit(string file)
        {
            this.Invoke(new OpenEditUIDelegate(OpenEditUI), new object[] { file });
        }

        /// <summary>
        /// A delegate that always runs in the UI thread. This updates the database
        /// which in turn updates the log table.
        /// </summary>
        /// <param name="details"></param>
        /// <param name="dirPath"></param>
        delegate void OpenEditUIDelegate(string details);
        protected void OpenEditUI(string file)
        {
            parent.LoadUTFFile(file);
        }

        struct VMeshNodeInfo
        {
            public string file;
            public string name;
            public uint crc;
        };

        private void ScanForNonUniqueMeshNodesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AddLog("Completed");
            bgScanMeshNodesWkr = null;
        }


        class MaterialInfo
        {
            public string matName;
            public uint matID;
            public uint matID2;
            public string texFileName;
            public uint texCRC;
            public Dictionary<string, string> refs = new Dictionary<string, string>();
            public Dictionary<string, string> errors = new Dictionary<string, string>();
            public Dictionary<string, string> usedBy = new Dictionary<string, string>();
        }
        Dictionary<uint, MaterialInfo> matList = new Dictionary<uint, MaterialInfo>();

        uint GetTextureCRC(string file, TreeNode root, string texFileName)
        {
            foreach (TreeNode texLibNode in root.Nodes.Find("texture library", true))
            {
                foreach (TreeNode node in texLibNode.Nodes)
                {
                    if (node.Name.ToLowerInvariant() == texFileName.ToLowerInvariant())
                    {
                        TreeNode mipNode = node.Nodes["MIPS"];
                        if (mipNode == null)
                            mipNode = node.Nodes["MIP0"];
                        if (mipNode == null)
                        {
                            AddLog("WARNING texture data is not found");
                            return 0;
                        }

                        byte[] texture = mipNode.Tag as byte[];
                        return Utilities.FLModelCRC(texture);
                    }
                }
            }
            return 0;
        }

        void LoadMaterial(string file, TreeNode root, TreeNode node)
        {
            TreeNode[] dtName = node.Nodes.Find("Dt_name", true);
            if (dtName.Length > 0)
            {
                MaterialInfo mi = new MaterialInfo();
                mi.matName = node.Name;
                mi.matID = Utilities.FLModelCRC(mi.matName);
                mi.matID2 = Utilities.FLModelCRC(mi.matName.ToLowerInvariant());
                mi.texFileName = Utilities.GetString(node.Nodes["Dt_name"]).ToLowerInvariant();
                mi.texCRC = GetTextureCRC(file, root, mi.texFileName);
                if (mi.texCRC == 0)
                    mi.errors[file] = String.Format("Error no texture data found for tex1name={0}", mi.texFileName);

                MaterialInfo mi2 = null;
                if (matList.ContainsKey(mi.matID))
                    mi2 = matList[mi.matID];
                //if (matList.ContainsKey(mi.matID2))
                //    mi2 = matList[mi.matID2];

                // If this texture is already present then check it
                if (mi2 != null)
                {
                    mi2.refs[file] = file;
                    if (mi2.texFileName != mi.texFileName)
                    {
                        mi2.errors[file] = String.Format("Error texture file name different for same material matName, tex1name={0} tex2name={1}", mi2.texFileName, mi.texFileName);
                    }
                    if (mi2.texCRC != mi.texCRC)
                    {
                        mi2.errors[file] = String.Format("Error texture file contents different for same texture name, texname={0} tex1crc={1} tex2crc={2}", mi.texFileName, mi.texCRC, mi2.texCRC);
                    }
                }
                else
                {
                    mi.refs[file] = file;
                    matList[mi.matID] = mi;
                    if (mi.matID != mi.matID2)
                    {
                        //matList[mi.matID2] = mi;
                    }
                }
            }
        }

        void LoadMaterials(string file, TreeNode root)
        {
            foreach (TreeNode matLibNode in root.Nodes.Find("material library", true))
            {
                foreach (TreeNode node in matLibNode.Nodes)
                {
                    LoadMaterial(file, root, node);
                }
            }
            foreach (TreeNode matLibNode in root.Nodes.Find("Material library", true))
            {
                foreach (TreeNode node in matLibNode.Nodes)
                {
                    LoadMaterial(file, root, node);
                }
            }
        }

        Dictionary<uint, VMeshNodeInfo> nodelist = new Dictionary<uint, VMeshNodeInfo>();

        /// <summary>
        /// Build the update package in the background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanForNonUniqueMeshNodes(object sender, DoWorkEventArgs e)
        {
            matList.Clear();
            nodelist.Clear();
            string folderpath = (string)e.Argument;
            string[] files = System.IO.Directory.GetFiles(folderpath, "*.*", System.IO.SearchOption.AllDirectories);

            int max = files.Length;

            AddLog("Loading mats");
            int curr = 0;
            foreach (string file in files)
            {
                if (bgScanMeshNodesWkr.CancellationPending)
                    return;

                if ((++curr % 100) == 0)
                    AddLog(String.Format("\nProcessing {0}/{1}", curr, max));

                if (file.EndsWith(".mat"))
                {
                    UTFFile utf = new UTFFile();
                    TreeNode root = utf.LoadUTFFile(file);
                    LoadMaterials(file, root);
                }
            }

            AddLog("Loading cmp/3db");
            curr = 0;
            foreach (string file in files)
            {
                if (file.EndsWith(".3db") || file.EndsWith(".cmp"))
                {
                    try
                    {
                        List<string> files_with_duplicates = new List<string>();
                        UTFFile utf = new UTFFile();
                        TreeNode root = utf.LoadUTFFile(file);

                        LoadMaterials(file, root);

                        foreach (TreeNode node in root.Nodes.Find("VMeshData", true))
                        {
                            VMeshNodeInfo info = new VMeshNodeInfo();
                            info.file = file;
                            info.name = node.Parent.Name;
                            info.crc = Utilities.FLModelCRC(info.name);
                            if (nodelist.ContainsKey(info.crc))
                            {
                                AddLog("\nError duplicate node name=" + info.name);
                                AddLog(" file1=" + info.file);
                                AddLog(" file2=" + nodelist[info.crc].file);
                                if (!files_with_duplicates.Contains(info.file))
                                    files_with_duplicates.Add(info.file);
                                if (!files_with_duplicates.Contains(nodelist[info.crc].file))
                                    files_with_duplicates.Add(nodelist[info.crc].file);
                            }
                            else
                            {
                                nodelist[info.crc] = info;
                            }

                            VMeshData meshData = new VMeshData(node.Tag as byte[]);
                            foreach (VMeshData.TMeshHeader m in meshData.Meshes)
                            {
                                if (matList.ContainsKey(m.MaterialId))
                                {
                                    matList[m.MaterialId].usedBy[file] = node.Name;
                                }
                                else
                                {
                                    AddLog(String.Format("\nError no material entry for name={0} materialID={1}", node.Parent.Parent.Name, m.MaterialId));
                                    AddLog(" file1=" + file);
                                }
                            }
                        }

                        if (root.Nodes.Find("VMeshLibrary", true).Length > 0)
                        {
                            foreach (TreeNode node in root.Nodes.Find("VMeshRef", true))
                            {
                                VMeshRef data = new VMeshRef(node.Tag as byte[]);
                                if (FindVMeshName(root, data.VMeshLibId) == null)
                                {
                                    AddLog("\nError no vmeshlibrary entry");
                                    try { AddLog(" name=" + node.Parent.Parent.Parent.Parent.Name); }
                                    catch { }
                                    AddLog(" file1=" + file);

                                    if (scanOptions == "Open File On Error")
                                    {
                                        parent.LoadUTFFile(file);
                                    }
                                }
                            }

                            foreach (TreeNode node in root.Nodes.Find("VWireData", true))
                            {
                                VWireData data = new VWireData(node.Tag as byte[]);
                                if (FindVMeshName(root, data.VMeshLibId) == null)
                                {
                                    AddLog("\nError no vmeshlibrary entry for name=" + node.Parent.Parent.Name);
                                    AddLog(" file1=" + file);

                                    if (scanOptions == "Open File On Error")
                                    {
                                        OpenEdit(file);
                                    }
                                }
                            }
                        }


                        foreach (string file_to_fix in files_with_duplicates)
                        {
                            if (scanOptions == "Automatically Fix Errors")
                            {
                                FixVMeshNodeNames(file_to_fix);
                            }
                            else if (scanOptions == "Automatically Fix Errors")
                            {
                                DialogResult r = MessageBox.Show(String.Format("Rebuild VMeshData Node Names in {0}?", file_to_fix), "Rebuild?", MessageBoxButtons.YesNoCancel);

                                if (r == DialogResult.Yes)
                                {
                                    FixVMeshNodeNames(file_to_fix);
                                }
                                else if (r == DialogResult.Cancel)
                                {
                                    AddLog("Cancelled");
                                    return;
                                }
                            }
                            else if (scanOptions == "Open File On Error")
                            {
                                parent.LoadUTFFile(file);
                            }
                        }
                    }
                    catch
                    {
                        AddLog("Error loading " + file);
                    }
                }
            }

            foreach (MaterialInfo mi in matList.Values)
            {
                if (mi.errors.Count > 0 && mi.usedBy.Count > 1)
                {
                    AddLog(String.Format("\nError for material={0} id={1} id2={2}", mi.matName, mi.matID, mi.matID2));
                    AddLog(String.Format("Material found in:"));
                    foreach (string file in mi.refs.Keys)
                    {
                        AddLog(" " + file);
                    }
                    AddLog(String.Format("Errors:"));
                    foreach (KeyValuePair<string, string> item in mi.errors)
                    {
                        AddLog(String.Format(" file={0} error={1}", item.Key, item.Value));
                    }
                    AddLog(String.Format("Used By:"));
                    foreach (KeyValuePair<string, string> item in mi.usedBy)
                    {
                        AddLog(String.Format(" model={0} mesh={1}", item.Key, item.Value));
                    }
                }
            }

            // Check for same textures, different names.
            Dictionary<uint, List<MaterialInfo>> textures = new Dictionary<uint, List<MaterialInfo> >();
            foreach (MaterialInfo mi in matList.Values)
            {
                if (!textures.ContainsKey(mi.texCRC))
                    textures[mi.texCRC] = new List<MaterialInfo>();
                textures[mi.texCRC].Add(mi);
            }
            foreach (List<MaterialInfo> ml in textures.Values)
            {
                if (ml.Count > 1)
                {
                    AddLog(String.Format("Same texture/different name:"));
                    foreach (MaterialInfo mi in ml)
                    {
                        AddLog(String.Format(" name={0} texture_file_name={1} matid={2}", mi.matName, mi.texFileName, mi.matID));
                        AddLog(String.Format(" Material found in:"));
                        foreach (string file in mi.refs.Keys)
                        {
                            AddLog("  " + file);
                        }
                    }
                }
            }
        }

        void FixVMeshNodeNames(string file)
        {
            UTFFile utf = new UTFFile();
            TreeNode root = utf.LoadUTFFile(file);

            foreach (TreeNode node in root.Nodes.Find("VMeshData", true))
            {
                string new_name = System.IO.Path.GetFileNameWithoutExtension(file);
                new_name += "." + String.Format("{0:0000000000}", Utilities.FLModelCRC(Guid.NewGuid().ToString()));
                string old_name = node.Parent.Name;
                if (old_name.Contains("wire"))
                {
                    new_name += ".wire.";
                } 
                
                int lodstart = old_name.IndexOf(".lod");
                if (lodstart > 0)
                {
                    new_name += old_name.Substring(lodstart);
                }
                else
                {
                    new_name += ".vms";
                }

                node.Parent.Name = new_name;
                node.Parent.Text = new_name;
                NodeChanged(root, node.Parent, old_name);
            }

            utf.SaveUTFFile(root, file);
        }

        /// <summary>
        /// Call this function to notify observers when node data changes.
        /// </summary>
        /// <param name="node"></param>
        public void NodeChanged(TreeNode root, TreeNode node, string oldName)
        {
            uint oldCRC = Utilities.FLModelCRC(oldName);
            uint newCRC = Utilities.FLModelCRC(node.Name);

            TreeNode[] refNodes = FindVMeshRefs(root, oldCRC);
            if (refNodes.Length > 0)
            {
                AddLog(String.Format("Updating {0} VMeshRef CRCs from {1} to {2}", refNodes.Length, oldName, node.Name));
                UpdateVMeshRefs(refNodes, newCRC);
            }

            TreeNode[] wireNodes = FindVWireData(root, oldCRC);
            if (wireNodes.Length > 0)
            {
                AddLog(String.Format("Updating {0} VMeshWire CRCs from {1} to {2}", wireNodes.Length, oldName, node.Name));
                UpdateVWireData(wireNodes, newCRC);
            }
        }

        /// <summary>
        /// Find all vmeshref nodes with the matching flmodelcrc.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public TreeNode[] FindVMeshRefs(TreeNode root, uint vMeshLibId)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            try
            {
                foreach (TreeNode node in root.Nodes.Find("VMeshRef", true))
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
        }

        /// <summary>
        /// Find all vmeshwire nodes with the matching flmodelcrc.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public TreeNode[] FindVWireData(TreeNode root, uint vMeshLibId)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            try
            {
                foreach (TreeNode node in root.Nodes.Find("VWireData", true))
                {
                    try
                    {
                        VWireData data = new VWireData(node.Tag as byte[]);
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
        /// Update all VMeshWire nodes with newVMeshLibId.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public void UpdateVWireData(TreeNode[] nodes, uint newVMeshLibId)
        {
            foreach (TreeNode node in nodes)
            {
                VWireData data = new VWireData(node.Tag as byte[]);
                data.VMeshLibId = newVMeshLibId;
                node.Tag = data.GetBytes();
            }
        }

        private void buttonFixFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                FixVMeshNodeNames(openFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Search the VMeshData names for a matching crc.
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        public string FindVMeshName(TreeNode root, uint crc)
        {
            string name = null;
            try
            {
                TreeNode vmesh = root.Nodes["VMeshLibrary"];
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
            return name;
        }
    }
}
