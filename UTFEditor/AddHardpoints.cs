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
    public partial class AddHardpoints : Form
    {
        ModelViewForm parent;

        private class Preset
        {

            public List<HardpointType> HardpointTypes;
            public string Name;

            public Preset(string name)
            {
                Name = name;
                HardpointTypes = new List<HardpointType>();
            }

            public override string ToString()
            {
                return Name;
            }

            public string[] Serialize()
            {
                string[] result = new string[HardpointTypes.Count];

                int a = 0;
                foreach (HardpointType t in HardpointTypes)
                {
                    result[a] = t.Serialize();
                    a++;
                }

                return result;
            }
        }

        private class HardpointType
        {
            public bool Revolute;
            public string Name;

            public HardpointType(string name, bool rev)
            {
                Revolute = rev;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }

            public string Serialize()
            {
                return Name + "," + Revolute;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is HardpointType)) return false;
                HardpointType t = (HardpointType)obj;
                return t.Revolute == Revolute && t.Name == Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode() ^ this.Revolute.GetHashCode();
            }
        }

        public AddHardpoints(ModelViewForm parent)
        {
            this.parent = parent;
            InitializeComponent();

            LoadPresets();
        }

        private void comboPresets_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PresetChanged();
        }

        private void comboPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            PresetChanged();
        }

        private void comboPresets_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) PresetChanged();
        }

        private void comboPresets_Leave(object sender, EventArgs e)
        {
            PresetChanged();
        }

        private void PresetChanged()
        {
            if (comboPresets.Text.Trim() == "") return;

            Preset sel = (Preset)comboPresets.SelectedItem;
            if (sel == null)
            {
                sel = new Preset(comboPresets.Text);
                foreach (HardpointType t in lstHps.Items)
                    sel.HardpointTypes.Add(t);

                comboPresets.Items.Add(sel);
            }
            else
            {
                lstHps.Items.Clear();
                foreach (HardpointType t in sel.HardpointTypes)
                {
                    int at = lstHps.Items.Add(t);
                    if (t.Revolute) lstHps.SetItemChecked(at, true);
                }
            }

            btnStart.Enabled = lstHps.Items.Count > 0;
        }

        private void LoadPresets()
        {
            comboPresets.Items.Clear();

            AddDefaultPresets();

            string presetsPath = System.IO.Path.Combine(Application.LocalUserAppDataPath, "HpPresets");
            if (!System.IO.Directory.Exists(presetsPath))
                return;

            string[] files = System.IO.Directory.GetFiles(presetsPath, "*.txt");

            foreach (string f in files)
            {
                string item = System.IO.Path.GetFileNameWithoutExtension(f);
                Preset p = new Preset(item);
                string[] lines = System.IO.File.ReadAllLines(f);
                foreach(string l in lines)
                {
                    string[] ls = l.Split(',');
                    p.HardpointTypes.Add(new HardpointType(ls[0], Boolean.Parse(ls[1])));
                }
                comboPresets.Items.Add(p);
            }
        }

        private void AddDefaultPresets()
        {
            Preset ship = new Preset("Ship");
            ship.HardpointTypes.AddRange(new HardpointType[] {
                new HardpointType("HpWeapon##", true),
                new HardpointType("HpEngine0#", false),
                new HardpointType("HpEngineGlow##", false),
                new HardpointType("HpTractor_Source##", false),
            });

            comboPresets.Items.Add(ship);
        }

        private void btnAddHp_Click(object sender, EventArgs e)
        {
            if (comboPresets.SelectedItem == null) return;

            string input = txtAddHp.Text;
            bool rev = chkRevolute.Checked;

            if (input.Trim() == "" || !System.Text.RegularExpressions.Regex.IsMatch(input, "^[a-zA-Z0-9#_]*$"))
            {
                System.Windows.Forms.MessageBox.Show("Given hardpoint name is invalid. Must only contain alphanumeric characters, # and _.", "Bad Hardpoint Name", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            if (!input.StartsWith("Hp")) input = "Hp" + input;

            Preset p = (Preset)comboPresets.SelectedItem;
            HardpointType t = new HardpointType(input, rev);

            foreach (HardpointType t2 in p.HardpointTypes)
            {
                if (t2.Equals(t)) return;
            }

            p.HardpointTypes.Add(t);
            int at = lstHps.Items.Add(t);
            if (rev) lstHps.SetItemChecked(at, true);
            lstHps.SelectedIndex = at;

            btnStart.Enabled = true;
        }

        private void btnRemoveHp_Click(object sender, EventArgs e)
        {
            if (lstHps.SelectedItem == null) return;

            Preset p = (Preset)comboPresets.SelectedItem;
            int at = lstHps.SelectedIndex;
            p.HardpointTypes.Remove((HardpointType)lstHps.SelectedItem);
            lstHps.Items.Remove(lstHps.SelectedItem);

            if (lstHps.Items.Count > 0)
            {
                at = Math.Max(at-1, 0);
                lstHps.SelectedIndex = at;
            }

            btnStart.Enabled = lstHps.Items.Count > 0;
        }

        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(-1 * ((ModifierKeys == Keys.Shift) ? 5 : 1));
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(1 * ((ModifierKeys == Keys.Shift) ? 5 : 1));
        }

        private void MoveSelectedItem(int dist)
        {
            if (lstHps.SelectedItem == null) return;

            HardpointType t = (HardpointType)lstHps.SelectedItem;
            int at = mod(lstHps.SelectedIndex + dist, lstHps.Items.Count);
            lstHps.Items.Remove(t);
            lstHps.Items.Insert(at, t);
            if (t.Revolute) lstHps.SetItemChecked(at, true);
            lstHps.SelectedIndex = at;
        }

        private void lstHps_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSel = lstHps.SelectedIndex >= 0;
            btnRemoveHp.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled = hasSel;
        }

        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            Preset p = (Preset)comboPresets.SelectedItem;

            string presetsPath = System.IO.Path.Combine(Application.LocalUserAppDataPath, "HpPresets");
            if (!System.IO.Directory.Exists(presetsPath))
                System.IO.Directory.CreateDirectory(presetsPath);

            System.IO.File.WriteAllLines(System.IO.Path.Combine(presetsPath, comboPresets.Text + ".txt"), p.Serialize());
        }

        private bool adding = false;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (lstHps.Items.Count == 0)
            {
                if (!adding) return;
                else
                {
                    adding = false;
                    btnStart.Text = adding ? "Stop" : "Start";
                    grpAddHps.Enabled = adding;
                }
            }
            adding = !adding;
            btnStart.Text = adding ? "Stop" : "Start";
            grpAddHps.Enabled = adding;
            lstHps.SelectedIndex = 0;
        }
    }
}
