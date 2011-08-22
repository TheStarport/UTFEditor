using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace UTFEditor
{
    public partial class AddHardpoints : Form
    {
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

        TreeNode root;
        List<TreeNode> fixRevs = new List<TreeNode>();

        public AddHardpoints(TreeNode root)
        {
            InitializeComponent();

            this.root = root;
            LoadPresets();
            LoadFixRevs();
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
                new HardpointType("HpTractor_Source", false),
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
            btnRemoveHp.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled = lstHps.SelectedIndex >= 0 && !adding;
            txtHp.Text = GetNextHardpoint();
            chkSet.Checked = false;
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
                if (adding)
                {
                    adding = false;
                    btnStart.Text = "Start";
                    grpAddHps.Enabled = false;
                }

                return;
            }
            adding = !adding;
            btnStart.Text = adding ? "Stop" : "Start";
            grpAddHps.Enabled = adding;
            lstHps.SelectedIndex = 0;

            btnRemoveHp.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled = lstHps.SelectedIndex >= 0 && !adding;
            btnAddHp.Enabled = txtAddHp.Enabled = chkRevolute.Enabled = !adding;

            if (adding)
            {
                txtHp.Text = GetNextHardpoint();
                chkSet.Checked = false;
            }
            else
            {
                txtHp.Text = "";
            }
        }

        public bool AddingMode
        {
            get
            {
                return adding;
            }
        }

        public string CurrentName
        {
            get
            {
                return txtHp.Text;
            }
        }

        public bool CurrentRevolute
        {
            get
            {
                return ((HardpointType)lstHps.SelectedItem).Revolute;
            }
        }

        public bool CurrentIsSet
        {
            get
            {
                return chkSet.Checked;
            }
        }

        public void HardpointSet()
        {
            chkSet.Checked = true;
        }

        private string GetNextHardpoint()
        {
            HardpointType hptype = (HardpointType)lstHps.SelectedItem;
            if (hptype == null) return "";

            int mask = GetMaskType(hptype.Name);

            int num = 1;

            string name = MakeHardpointName(hptype.Name, num, mask);

            string staticpattern = hptype.Name.Substring(0, hptype.Name.Length - mask);

            foreach (TreeNode fr in fixRevs)
            {
                foreach (TreeNode h in fr.Nodes)
                {
                    if (h.Text.Substring(0, h.Text.Length - mask).Equals(staticpattern, StringComparison.InvariantCultureIgnoreCase))
                        name = MakeNextHardpointName(hptype.Name, name, h.Text, num, mask);
                }
            }

            return name;
        }

        private void LoadFixRevs()
        {
            fixRevs.Clear();
            foreach(TreeNode n in root.Nodes)
            {
                if (n.Nodes["Hardpoints"] != null)
                {
                    TreeNode hps = n.Nodes["Hardpoints"];
                    if (hps.Nodes["Fixed"] != null)
                        fixRevs.Add(hps.Nodes["Fixed"]);
                    if (hps.Nodes["Revolute"] != null)
                        fixRevs.Add(hps.Nodes["Revolute"]);
                }
                    
            }
        }

        private int GetMaskType(string init)
        {
            if (init.EndsWith("##"))
                return 2;
            else if (init.EndsWith("#"))
                return 1;
            else
                return 0;
        }

        private string MakeHardpointName(string pattern, int num, int mask)
        {
            switch (mask)
            {
                case 1:
                    return pattern.Substring(0, pattern.Length - 1) + num;
                case 2:
                    return pattern.Substring(0, pattern.Length - 2) + num.ToString("00");
                default:
                    return pattern;
            }
        }

        private string MakeNextHardpointName(string pattern, string current, string match, int num, int mask)
        {
            int othernum = 0;
            try
            {
                switch (mask)
                {
                    case 0:
                        return current;
                    case 1:
                        othernum = Int32.Parse(match.Substring(match.Length - 1));
                        break;
                    case 2:
                        othernum = Int32.Parse(match.Substring(match.Length - 2));
                        break;
                }
            }
            catch(FormatException)
            {
                return current;
            }

            if (othernum < num) return current;

            return MakeHardpointName(pattern, othernum + 1, mask);
        }

        public void NextHardpoint()
        {
            txtHp.Text = GetNextHardpoint();
            chkSet.Checked = false;
        }

        public void ChangeHardpointType(int direction)
        {
            lstHps.SelectedIndex = mod(lstHps.SelectedIndex + direction, lstHps.Items.Count);
            NextHardpoint();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            NextHardpoint();
        }

        private void btnNextType_Click(object sender, EventArgs e)
        {
            ChangeHardpointType(1);
        }

        private void btnPrevType_Click(object sender, EventArgs e)
        {
            ChangeHardpointType(-1);
        }

        private void chkSet_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = chkSet.Checked;
        }

        /*private Regex MakeTypePattern(string init)
        {
            if (!init.Contains('#')) return new Regex("^" + init + "$");

            char[] ch = init.ToCharArray();
            StringBuilder sb = new StringBuilder("^");
            int numLen = 0;
            for (int a = 0; a < ch.Length; a++)
            {
                if (ch[a] == '#')
                    numLen++;
                else if (numLen > 0)
                {
                    sb.Append("[0-9]{" + numLen + ",}");
                    numLen = 0;
                }
                else
                    sb.Append(ch[a]);
            }
            sb.Append('$');

            return new Regex(sb.ToString());
        }

        private string MakeNextHardpointName(string pattern, Regex rg, int current, string match)
        {
            int other = GetHardpointNumber(rg, match);

            if (other >= current)
            {
            }
            else
                return current;
        }

        private int GetHardpointNumber(Regex pattern, string hp)
        {
            MatchCollection matches = pattern.Matches(hp);
            StringBuilder sb = new StringBuilder();
            foreach (Match m in matches)
                sb.Append(m.Value);

            return Int32.Parse(sb.ToString());
        }*/
    }
}
