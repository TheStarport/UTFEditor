using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    internal partial class EditSurMeshIdsForm : Form
    {
        class ListItem : INotifyPropertyChanged
        {
            public string Name
            {
                get => _name; set
                {
                    _name = value;
                    _crc = Utilities.FLModelCRC(_name);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
                }
            }
            public uint CRC
            {
                get => _crc; set
                {
                    _crc = value;
                    _crcs.TryGetValue(_crc, out string newName);
                    _name = newName;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
                }
            }
            public uint OriginalCRC { get; }

            public ListItem(string name, uint crc, Dictionary<uint, string> crcs)
            {
                _crcs = crcs;
                _name = name;
                _crc = OriginalCRC = crc;
            }

            private string _name;
            private uint _crc;
            private Dictionary<uint, string> _crcs;

            public string DisplayName => (string.IsNullOrEmpty(Name) ? "<unknown>" : Name) + $" - 0x{CRC.ToString("X")}";

            public event PropertyChangedEventHandler PropertyChanged;
        }

        SurFile sur = null;
        Dictionary<uint, string> meshGroupCRCs = new Dictionary<uint, string>();

        BindingList<ListItem> items = new BindingList<ListItem>();

        public EditSurMeshIdsForm(SurFile sur, List<string> meshGroups)
        {
            this.sur = sur;
            foreach(var mg in meshGroups)
                meshGroupCRCs[Utilities.FLModelCRC(mg)] = mg;

            InitializeComponent();

            foreach (var m in this.sur.Meshes)
            {
                if (meshGroupCRCs.ContainsKey(m.MeshId))
                    items.Add(new ListItem(meshGroupCRCs[m.MeshId], m.MeshId, meshGroupCRCs));
                else
                    items.Add(new ListItem(null, m.MeshId, meshGroupCRCs));
            }

            listBox1.DataSource = items;
            listBox1.DisplayMember = "DisplayName";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.OK;

                for(int i = 0; i < sur.Meshes.Count; i++)
                {
                    var m = sur.Meshes[i];
                    foreach(var l in items)
                    {
                        if (l.OriginalCRC == m.MeshId)
                        {
                            m.MeshId = l.CRC;
                            break;
                        }
                    }

                    sur.Meshes[i] = m;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error " + ex.Message, "Invalid data");
            }
        }

        bool programmaticallyChanging = false;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (programmaticallyChanging)
                return;

            programmaticallyChanging = true;

            var li = listBox1.SelectedItem as ListItem;
            if (li == null)
                return;

            textBox1.Text = $"0x{li.CRC.ToString("X")}";
            textBox2.Text = li.Name;

            programmaticallyChanging = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (programmaticallyChanging)
                return;

            if (!UInt32.TryParse(textBox1.Text.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint crc))
                return;

            programmaticallyChanging = true;

            if (meshGroupCRCs.ContainsKey(crc))
                textBox2.Text = meshGroupCRCs[crc];
            else
                textBox2.Text = "";
            
            var li = listBox1.SelectedItem as ListItem;
            li.CRC = crc;
            li.Name = textBox2.Text;

            programmaticallyChanging = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (programmaticallyChanging)
                return;

            programmaticallyChanging = true;

            uint crc = Utilities.FLModelCRC(textBox2.Text.Trim());
            textBox1.Text = $"0x{crc.ToString("X")}";

            var li = listBox1.SelectedItem as ListItem;
            li.Name = textBox2.Text;
            li.CRC = crc;

            programmaticallyChanging = false;
        }
    }
}
