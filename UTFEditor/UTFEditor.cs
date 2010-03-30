using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;

using System.Reflection;

namespace UTFEditor
{
    public partial class UTFEditor : Form
    {
        private int childFormNumber = 0;

        public UTFEditor()
        {
            InitializeComponent();
            SetSelectedNode(null);
            LoadRecentFiles();
        }

        /// <summary>
        /// Create a new form with only the root node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowNewForm(object sender, EventArgs e)
        {
            UTFForm childForm = new UTFForm(this, "File " + childFormNumber++);
            childForm.AddNode("\\");
            childForm.MdiParent = this;
            childForm.Show();
        }

        /// <summary>
        /// Display an open dialog file and then open the selected file.
        /// </summary>
        private void OpenFile(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "FL UTF Files|*.dfm;*.cmp;*.utf;*.3db;*.mat;*.txm|All Files|*.*";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    UTFForm childForm = new UTFForm(this, openFileDialog1.FileName);
                    childForm.LoadUTFFile(openFileDialog1.FileName);
                    childForm.MdiParent = this;
                    childForm.Show();

                    UpdateRecentFiles(childForm.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Unable to open file");
                }
            }
        }

        /// <summary>
        /// Open a file selected from the recent items list.
        /// </summary>
        void recentFile_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;

                UTFForm childForm = new UTFForm(this, item.Text);
                childForm.LoadUTFFile(item.Text);
                childForm.MdiParent = this;
                childForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Unable to open file");
            }
        }

        /// <summary>
        /// Save the file that has focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;

                saveFileDialog1.Filter = "FL UTF Files|*.dfm;*.cmp;*.utf;*.3db;*.mat;*.txm|All Files|*.*";
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        childForm.SaveUTFFile(saveFileDialog1.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Error " + ex.Message, "Unable to save file");
                    }
                }
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.Cut();
            }
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.Copy();
            }
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.Paste();
            }
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        public void SetSelectedNode(TreeNode node)
        {
            textBoxCurSelASCII.Text = "";
            textBoxCurSelHex.Text = "";
            textBoxCurSelSize.Text = "";

            buttonAddNode.Enabled = false;
            buttonDelNodes.Enabled = false;
            buttonRenameNode.Enabled = false;

            buttonEditString.Enabled = false;
            buttonEditIntArray.Enabled = false;
            buttonEditFloatArray.Enabled = false;
            buttonPlaySound.Enabled = false;
            buttonEditAnimChannel.Enabled = false;
            buttonEditVMeshRef.Enabled = false;

            buttonImport.Enabled = false;
            buttonExport.Enabled = false;

            buttonExportVMeshData.Enabled = false;
            buttonExportVMeshRef.Enabled = false;
            buttonExportVWireData.Enabled = false;
            
            if (this.ActiveMdiChild is UTFForm)
            {
                buttonAddNode.Enabled = true;
                buttonRenameNode.Enabled = true;
            }

            if (node != null)
            {
                byte[] data = node.Tag as byte[];
                if (data != null)
                {
                    textBoxCurSelASCII.Text = System.Text.Encoding.ASCII.GetString(data, 0, (data.Length < 0x20 ? data.Length : 0x20));
                    textBoxCurSelHex.Text = BitConverter.ToString(data, 0, (data.Length<0x20?data.Length:0x20)).Replace("-", " ");
                    textBoxCurSelSize.Text = data.Length.ToString();

                    if (node.Text == "VMeshData" && data.Length > 0)
                    {
                        buttonExportVMeshData.Enabled = true;
                    }

                    if (node.Text == "VMeshRef" && data.Length > 0)
                    {
                        buttonExportVMeshRef.Enabled = true;
                        buttonEditVMeshRef.Enabled = true;
                    }

                    if (node.Text == "VWireData" && data.Length > 0)
                    {
                        buttonExportVWireData.Enabled = true;
                    }

                    if (node.Text.StartsWith("0x") && data.Length > 0 && data[0] == 'R')
                    {
                        buttonPlaySound.Enabled = true;
                    }

                    if (node.Text == "Channel" && node.Nodes.Count == 2)
                    {
                        buttonEditAnimChannel.Enabled = true;
                    }

                    if (node.Nodes.Count > 0)
                    {
                        buttonDelNodes.Enabled = true;
                    }
                    else
                    {
                        buttonDelNodes.Enabled = true;

                        buttonImport.Enabled = true;
                        buttonExport.Enabled = true;

                        buttonEditString.Enabled = true;
                        buttonEditIntArray.Enabled = true;
                        buttonEditFloatArray.Enabled = true;
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                try
                {
                    childForm.SaveUTFFile(childForm.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Unable to save file");
                }
            }
        }

        private void buttonAddNode_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.AddNode("New");
            }
        }

        private void buttonDelNodes_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.DeleteSelectedNodes();
            }
        }

        private void buttonRenameNode_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.RenameNode();
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ImportData();
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ExportData();
            }
        }

        /// <summary>
        /// Add a file to the recent files list.
        /// </summary>
        /// <param name="filename">The file to add</param>
        void UpdateRecentFiles(string filename)
        {
            string setting = Properties.Settings.Default.RecentFiles;
            List<string> files = new List<string>(setting.Split(';'));
            while (files.Count > 10)
                files.RemoveAt(0);

            if (files.Contains(filename))
                files.Remove(filename);
            files.Add(filename);

            StringBuilder sb = new StringBuilder();
            foreach (string file in files)
            {
                if (file.Trim().Length > 0)
                {
                    sb.Append(file);
                    sb.Append(";");
                }
            }
            Properties.Settings.Default.RecentFiles = sb.ToString();
            Properties.Settings.Default.Save();

            LoadRecentFiles();
        }

        /// <summary>
        /// Read the recently accessed files out of the configuration file and
        /// add them to the menu bar.
        /// </summary>
        void LoadRecentFiles()
        {
            toolStripMenuItemRecentFiles.DropDownItems.Clear();

            string[] files = Properties.Settings.Default.RecentFiles.Split(';');
            for (int i = files.Length - 1; i >= 0; i--)
            {
                string filename = files[i].Trim();
                if (filename.Length > 0)
                {
                    ToolStripMenuItem recentFile = new ToolStripMenuItem(files[i]);
                    recentFile.Click += new EventHandler(recentFile_Click);
                    toolStripMenuItemRecentFiles.DropDownItems.Add(recentFile);
                }
            }
        }

        private void buttonExportVMeshData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ExportVMeshData();
            }
        }

        private void buttonExportVMeshRef_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ExportVMeshRef();
            }
        }

        private void buttonExportVWireData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ExportVWireData();
            }
        }

        private void buttonEditFixData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditFixData();
            }
        }

        private void buttonEditPrisData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditPrisData();
            }
        }


        private void buttonEditRevData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditRevData();
            }
        }

        private void buttonEditString_Click(object sender, EventArgs e)
        {
             if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditString();
            }
        }

        private void buttonEditIntArray_Click(object sender, EventArgs e)
        {
             if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditIntArray();
            }
        }

        private void buttonEditFloatArray_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditFloatArray();
            }
        }
  
        private void buttonEditAnimChannel_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.MakeAnimFrames();
            }
        }

        private void buttonPlaySound_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.PlaySound();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void buttonShowModel_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ShowModel();
            }
        }

        private void buttonEditVMeshRef_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditVMeshRef();
            }
        }

        private void loadSURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "FL SUR Files|*.sur|All Files|*.*";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    SurFile f = new SurFile(openFileDialog1.FileName);
                    /* UTFForm childForm = new UTFForm(this, openFileDialog1.FileName);
                    childForm.LoadUTFFile(openFileDialog1.FileName);
                    childForm.MdiParent = this;
                    childForm.Show(); */

                    // UpdateRecentFiles(childForm.FilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Unable to open file");
                }
            }
        }
    }
}
