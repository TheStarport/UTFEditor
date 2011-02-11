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
        const string UTFfilter = "FL UTF Files|*.3db;*.ale;*.anm;*.cmp;*.dfm;*.mat;*.sph;*.txm;*.utf;*.vms|" +
                                 "Model Files (*.3db)|*.3db|" +
                                 "Alchemy Files (*.ale)|*.ale|" +
                                 "Animation Files (*.anm)|*.anm|" +
                                 "Compound Files (*.cmp)|*.cmp|" +
                                 "Deformable Files (*.dfm)|*.dfm|" +
                                 "Material Files (*.mat)|*.mat|" +
                                 "Sphere Files (*.sph)|*.sph|" +
                                 "Texture Files (*.txm)|*.txm|" +
                                 "Audio Files (*.utf)|*.utf|" +
                                 "VMesh Files (*.vms)|*.vms|" +
                                 "All Files|*";

        private int childFormNumber = 0;

        public UTFEditor(string[] args)
        {
            InitializeComponent();
            SetSelectedNode(null);
            LoadRecentFiles();

            foreach (string arg in args)
            {
                try
                {
                    LoadUTFFile(arg);
                }
                catch { }
            }
        }

        /// <summary>
        /// Open and show a UTF file. Throws exception on failure.
        /// </summary>
        /// <param name="name">File to open</param>
        private void LoadUTFFile(string name)
        {
            UTFForm childForm = new UTFForm(this, name);
            childForm.LoadUTFFile(name);
            childForm.MdiParent = this;
            childForm.Show();

            UpdateRecentFiles(childForm.fileName);
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

        private void UTFEditor_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void UTFEditor_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (string file in files)
            {
                try
                {
                    LoadUTFFile(file);
                }
                catch { }
            }
        }

        /// <summary>
        /// Display an open dialog file and then open the selected file.
        /// </summary>
        private void OpenFile(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = UTFfilter;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    LoadUTFFile(openFileDialog1.FileName);
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
                LoadUTFFile(item.Text);
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

                saveFileDialog1.Filter = UTFfilter;
                saveFileDialog1.FileName = childForm.fileName;
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        childForm.SaveUTFFile(saveFileDialog1.FileName);
                        UpdateRecentFiles(saveFileDialog1.FileName);
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                try
                {
                    childForm.SaveUTFFile(childForm.fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Unable to save file");
                }
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


        private void UTFEditor_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
                SetSelectedNode(null);
        }

        public void SelectGrid()
        {
            dataView.Select();
        }

        public void SetSelectedNode(TreeNode node)
        {
            UTFForm childForm;
            Editable edit;
            Viewable view;
            bool     data;
            if (this.ActiveMdiChild is UTFForm)
            {
                childForm = this.ActiveMdiChild as UTFForm;
                edit = childForm.IsEditable(node);
                view = childForm.IsViewable(node);
                data = childForm.ContainsData(node);
                buttonAddNode.Enabled = !data;
                buttonDelNodes.Enabled =
                buttonRenameNode.Enabled = (node != null);
            }
            else
            {
                childForm = null;
                edit = Editable.No;
                view = Viewable.No;
                data = false;
                buttonAddNode.Enabled =
                buttonDelNodes.Enabled =
                buttonRenameNode.Enabled = false;
            }

            dataView.Rows.Clear();
            // Hide the last two columns, show them as needed.
            dataView.Columns[2].Visible = dataView.Columns[3].Visible = false;
            // Restore the style after a hex dump.
            dataView.Columns[1].DefaultCellStyle =
            dataView.Columns[2].DefaultCellStyle = dataView.DefaultCellStyle;
            dataView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dataView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			
			if(edit == Editable.Hardpoint)
				DisplayData(childForm, node, true);
            else if (data)
            {
                byte[] val = node.Tag as byte[];
                DisplayData(childForm, node, true);
                if (dataView.Rows.Count == 1)
					dataView.Rows.Add();
				dataView[0, 1].ValueType = typeof(string);
                dataView[0, 1].Value = String.Format("({0} {1})", val.Length, (val.Length == 1) ? "byte" : "bytes");
                dataView[0, 1].ReadOnly = true;
            }
            else
            {
                if (node != null)
                    foreach (TreeNode n in node.Nodes)
                        DisplayData(childForm, n, false);
            }
            buttonApply.Enabled = false;

            buttonImport.Enabled = (node != null && node.Nodes.Count == 0);
            buttonExport.Enabled = data;

            buttonEdit.Enabled = (edit != Editable.No);

            buttonView.Enabled = (view != Viewable.No);
            buttonView.Text = (view == Viewable.Wave) ? "Play" : "View";
       }

        private void DisplayData(UTFForm childForm, TreeNode node, bool fullHex)
        {
            if (!childForm.ContainsData(node) && childForm.IsEditable(node) != Editable.Hardpoint)
                return;

            Editable edit = childForm.IsEditable(node);
            Viewable view = childForm.IsViewable(node);

            byte[] data = node.Tag as byte[];
			int row = dataView.Rows.Add();
			dataView[0, row].ValueType = typeof(string);
            dataView[0, row].Value = node.Name;
            dataView[0, row].ReadOnly = true;
            int pos = 0;

            if (edit == Editable.String)
			{
				dataView[1, row].ValueType = typeof(string);
                dataView[1, row].Value = Utilities.GetString(node);
                dataView[1, row].ReadOnly = false;
                return;
            }

            if (edit == Editable.Hardpoint)
            {
				// Special case for a hardpoint's min/max nodes, to show degrees rather than radians.
				if(Utilities.StrIEq(node.Name, "Min", "Max"))
				{
					DataGridViewCell cell = dataView[1, row];
					cell.ValueType = typeof(double);
					cell.Style.Format = "0.##";
					cell.Value = Utilities.RadianToDegree(Utilities.GetFloat(data, ref pos));
					cell.ReadOnly = false;
					return;
                }
                else
                {
					HardpointData hpdata = new HardpointData(childForm.FindHardpoint(node));
					dataView[0, 0].Value = "X";
					dataView[0, 0].ReadOnly = true;
					dataView[1, 0].ValueType = typeof(float);
					dataView[1, 0].Style.Format = "g";
					dataView[1, 0].Value = hpdata.PosX;
					dataView[1, 0].ReadOnly = false;
					dataView.Rows.Add();

					dataView[0, 1].Value = "Y";
					dataView[0, 1].ReadOnly = true;
					dataView[1, 1].ValueType = typeof(float);
					dataView[1, 1].Style.Format = "g";
					dataView[1, 1].Value = hpdata.PosY;
					dataView[1, 1].ReadOnly = false;
					dataView.Rows.Add();

					dataView[0, 2].Value = "Z";
					dataView[0, 2].ReadOnly = true;
					dataView[1, 2].ValueType = typeof(float);
					dataView[1, 2].Style.Format = "g";
					dataView[1, 2].Value = hpdata.PosZ;
					dataView[1, 2].ReadOnly = false;
					dataView.Rows.Add();

					float pitch, yaw, roll;
					Utilities.OrientationToRotation(hpdata.RotMatXX,
													hpdata.RotMatXY,
													hpdata.RotMatXZ,
													hpdata.RotMatYX,
													hpdata.RotMatYY,
													hpdata.RotMatYZ,
													hpdata.RotMatZX,
													hpdata.RotMatZY,
													hpdata.RotMatZZ,
													out pitch, out yaw, out roll);

					dataView[0, 3].Value = "RotX";
					dataView[0, 3].ReadOnly = true;
					dataView[1, 3].ValueType = typeof(float);
					dataView[1, 3].Style.Format = "g";
					dataView[1, 3].Value = pitch;
					dataView[1, 3].ReadOnly = false;
					dataView.Rows.Add();

					dataView[0, 4].Value = "RotY";
					dataView[0, 4].ReadOnly = true;
					dataView[1, 4].ValueType = typeof(float);
					dataView[1, 4].Style.Format = "g";
					dataView[1, 4].Value = yaw;
					dataView[1, 4].ReadOnly = false;
					dataView.Rows.Add();

					dataView[0, 5].Value = "RotZ";
					dataView[0, 5].ReadOnly = true;
					dataView[1, 5].ValueType = typeof(float);
					dataView[1, 5].Style.Format = "g";
					dataView[1, 5].Value = roll;
					dataView[1, 5].ReadOnly = false;
					dataView.Rows.Add();

					// TODO: Not display this if not defined
					{
						dataView[0, 6].Value = "Min";
						dataView[0, 6].ReadOnly = true;
						dataView[1, 6].ValueType = typeof(float);
						dataView[1, 6].Style.Format = "g";
						dataView[1, 6].Value = hpdata.Min;
						dataView[1, 6].ReadOnly = false;
						dataView.Rows.Add();

						dataView[0, 7].Value = "Max";
						dataView[0, 7].ReadOnly = true;
						dataView[1, 7].ValueType = typeof(float);
						dataView[1, 7].Style.Format = "g";
						dataView[1, 7].Value = hpdata.Max;
						dataView[1, 7].ReadOnly = false;
					}
					return;
                }
            }

            if (view == Viewable.Texture)
            {
                DDSHeader dds = new DDSHeader();
                if (dds.Read(data, out pos))
				{
					dataView[1, row].ValueType = typeof(string);
                    dataView[1, row].Value = String.Format("{0}x{1}", dds.width, dds.height);
					dataView[1, row].ReadOnly = dataView[2, row].ReadOnly = dataView[3, row].ReadOnly = true;
                    if (dds.pflags == 4)
					{
						dataView[2, row].ValueType = typeof(string);
                        dataView[2, row].Value = dds.FourCC;
                    }
                    else
					{
						dataView[2, row].ValueType = typeof(string);
						dataView[2, row].Value = String.Format("{0}bpp {1}", dds.bpp, (dds.pflags == 0x40) ? "RGB" : "RGBA");
					}
					dataView[3, row].ValueType = typeof(string);
					dataView[3, row].Value = String.Format("{0} {1}", dds.mipmapcount, (dds.mipmapcount == 1) ? "level" : "levels");
                    dataView.Columns[2].Visible = dataView.Columns[3].Visible = true;
                    return;
                }
                TgaHeader tga = new TgaHeader();
                if (tga.Read(data, out pos))
				{
					dataView[1, row].ValueType = typeof(string);
					dataView[1, row].Value = String.Format("{0}x{1}", tga.Image_Width, tga.Image_Height);
					dataView[1, row].ReadOnly = dataView[2, row].ReadOnly = true;
                    string type;
                    switch (tga.Image_Type)
                    {
                        case 1: type = "map"; break;
                        case 2: type = (tga.Pixel_Depth == 32) ? "RGBA" : "RGB"; break;
                        default: type = "unknown"; break;
					}
					dataView[2, row].ValueType = typeof(string);
					dataView[2, row].Value = String.Format("{0}bpp {1}", tga.Pixel_Depth, type);
                    dataView.Columns[2].Visible = true;
                    return;
                }
            }
            else
            {
                string format = null;
                Type type = null;
                switch (edit)
                {
                    case Editable.Float:
                    case Editable.Color:
                    case Editable.Hardpoint:
                        type = typeof(float);
                        format = "g";
                        break;
                    case Editable.Int:
                        type = typeof(int);
                        format = "";
                        break;
                    case Editable.IntHex:
                        type = typeof(uint);
                        format = "0xX";
                        break;
                }
                if (type != null)
                {
                    int col = 1;
                    if (data.Length > 4)
                    {
                        dataView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                        dataView.Columns[2].Visible = true;
                        if (data.Length > 8)
                            dataView.Columns[3].Visible = true;
                    }
                    while (pos < data.Length)
                    {
                        if (col == 4)
                        {
                            col = 1;
                            row = dataView.Rows.Add();
                        }
                        object num;
                        if (type == typeof(float))
                            num = Utilities.GetFloat(data, ref pos);
                        else
                            num = Utilities.GetInt(data, ref pos);
						dataView[col, row].ReadOnly = false;
						dataView[col, row].Style.Format = format;
						dataView[col, row].ValueType = type;
                        dataView[col++, row].Value = num;
                    }
                    return;
                }
            }

            dataView.Columns[2].Visible = true;
            if (fullHex)
            {
                DataGridViewCellStyle style = dataView.DefaultCellStyle.Clone();
                style.Font = new Font(FontFamily.GenericMonospace, style.Font.SizeInPoints);
                dataView.Columns[1].DefaultCellStyle =
                dataView.Columns[2].DefaultCellStyle = style;
                dataView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            }
            int len = Math.Min((fullHex) ? 80 : 8, data.Length);
            for (pos = 0; pos < len; )
            {
                if (pos != 0)
                    row = dataView.Rows.Add();
				int curlen = Math.Min(8, len - pos);
				dataView[1, row].ValueType = typeof(string);
                dataView[1, row].Value = BitConverter.ToString(data, pos, curlen).Replace('-', ' ');
                StringBuilder sb = new StringBuilder(8);
                while (curlen-- != 0)
                {
                    byte b = data[pos++];
                    sb.Append((b >= 32 && b <= 126) ? (char)b : '.');
				}
				dataView[2, row].ValueType = typeof(string);
				dataView[2, row].Value = sb.ToString();
            }
        }

        private void dataView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (tb != null)
            {
                tb.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                tb.TextChanged -= new EventHandler(tb_TextChanged);
                tb.TextChanged += new EventHandler(tb_TextChanged);
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            float val;
            if (tb.TextLength == 0 || Single.TryParse(tb.Text, out val))
            {
                tb.ForeColor = DefaultForeColor;
            }
            else
            {
                tb.ForeColor = Color.Red;
            }
        }

		
        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
			DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)sender;
            // Since we can't seem to get the caret position, if there's an
            // actual selection, just replace it.
			if (tb.SelectionLength == 0) {
				Type t = tb.EditingControlDataGridView.SelectedCells[0].ValueType;
				if(t == typeof(float))
				{
					if(!Utilities.ValidFloatChar(tb.Text, e.KeyChar, tb.SelectionStart)) e.Handled = true;
					return;
				}
				else if (t == typeof(int))
				{
					if (!Utilities.ValidIntChar(tb.Text, e.KeyChar, tb.SelectionStart, false)) e.Handled = true;
					return;
				}
				else if (t == typeof(uint))
				{
					if (!Utilities.ValidIntChar(tb.Text, e.KeyChar, tb.SelectionStart, true)) e.Handled = true;
					return;
				}
			}
        }

        private void dataView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            //string val = e.Value.ToString();
            //if (val.Length == 0)
            //{
            //    e.Value = 0d;
            //    e.ParsingApplied = true;
            //}
            //else
            //{
            //    double num;
            //    if (Double.TryParse(val, out num))
            //    {
            //        e.Value = num;
            //        e.ParsingApplied = true;
            //        dataView[e.ColumnIndex, e.RowIndex].ErrorText = null;
            //    }
            //    else
            //    {
            //        dataView[e.ColumnIndex, e.RowIndex].ErrorText = "Parsing failed";
            //    }
            //}
        }

        private void dataView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (dataView[e.ColumnIndex, e.RowIndex].ValueType == typeof(double))
            //{
            //    double num;
            //    if (Double.TryParse(e.FormattedValue.ToString(), out num))
            //    {
            //        dataView[e.ColumnIndex, e.RowIndex].ToolTipText = null;
            //    }
            //    else
            //    {
            //        dataView[e.ColumnIndex, e.RowIndex].ToolTipText = "Parsing failed";
            //        e.Cancel = true;
            //    }
            //}
        }

        private void dataView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            buttonApply.Enabled = true;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
			// TODO: Make the button actually do something; bind Ctrl+S to this button when focused on the DataGridView
			ApplyChanges();
            buttonApply.Enabled = false;
            // If clicked, focus will shift to the next item,
            // make it go back to the grid.
            dataView.Select();
        }
        
        private void ApplyChanges()
		{	
			Editable edit;
			Viewable view;
			TreeNode node;
			UTFForm childForm;
			bool data;
			if (this.ActiveMdiChild is UTFForm)
			{
				childForm = this.ActiveMdiChild as UTFForm;
				node = childForm.GetSelectedNode();
				edit = childForm.IsEditable(node);
				view = childForm.IsViewable(node);
				data = childForm.ContainsData(node);
			}
			else
			{
				node = null;
				childForm = null;
				edit = Editable.No;
				view = Viewable.No;
				data = false;
				buttonAddNode.Enabled =
				buttonDelNodes.Enabled =
				buttonRenameNode.Enabled = false;
			}
			
			if (edit == Editable.Hardpoint) {
				TreeNode hp = childForm.FindHardpoint(node);
				HardpointData hpdata = new HardpointData(hp);
				
				hpdata.PosX = (float) dataView[1, 0].Value;
				hpdata.PosY = (float)dataView[1, 1].Value;
				hpdata.PosZ = (float)dataView[1, 2].Value;
				
				float[] rot = new float[9];
				
				Utilities.RotationToOrientation(
					(float)dataView[1, 3].Value,
					(float)dataView[1, 4].Value,
					(float)dataView[1, 5].Value,
					out rot[0],
					out rot[1],
					out rot[2],
					out rot[3],
					out rot[4],
					out rot[5],
					out rot[6],
					out rot[7],
					out rot[8]);
				
				hpdata.RotMatXX = rot[0];
				hpdata.RotMatXY = rot[1];
				hpdata.RotMatXZ = rot[2];
				hpdata.RotMatYX = rot[3];
				hpdata.RotMatYY = rot[4];
				hpdata.RotMatYZ = rot[5];
				hpdata.RotMatZX = rot[6];
				hpdata.RotMatZY = rot[7];
				hpdata.RotMatZZ = rot[8];

				if (Utilities.StrIEq(hp.Parent.Name, "Revolute"))
				{
					hpdata.Min = (float)dataView[1, 6].Value;
					hpdata.Max = (float)dataView[1, 7].Value;
				}
				
				hpdata.Write();
			}

			childForm.Modified();
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

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditNode();
            }
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ViewNode();
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

        private void buttonEditFixData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditFixData("Fix");
            }
        }

        private void buttonEditLooseData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditFixData("Loose");
            }
        }

        private void buttonEditRevData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditRevData("Rev");
            }
        }

        private void buttonEditPrisData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditRevData("Pris");
            }
        }

        private void buttonEditSphereData_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.EditSphereData();
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
                childForm.EditIntArray(false);
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
  
        private void buttonShowModel_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is UTFForm)
            {
                UTFForm childForm = this.ActiveMdiChild as UTFForm;
                childForm.ShowModel();
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

                    // UpdateRecentFiles(childForm.fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error " + ex.Message, "Unable to open file");
                }
            }
        }
    }
}
