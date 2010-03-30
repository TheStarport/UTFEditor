using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UTFEditor
{
    class UTFFile
    {
        /// <summary>
        /// The file this form is editing.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Try to load a UTF file. Throw an exception on failure.
        /// </summary>
        /// <param name="filePath">The file to load.</param>
        public TreeNode LoadUTFFile(string filePath)
        {
            this.FilePath = filePath;
            byte[] buf;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                buf = new byte[fs.Length];
                fs.Read(buf, 0, (int)fs.Length);
                fs.Close();
            }

            int pos = 0;
            int sig = BitConverter.ToInt32(buf, pos); pos += 4;
            int ver = BitConverter.ToInt32(buf, pos); pos += 4;
            if (sig != 0x20465455 || ver != 0x101)
                throw new Exception("Unsupported");

            // get node chunk info
            int nodeBlockOffset = BitConverter.ToInt32(buf, pos); pos += 4;
            int nodeSize = BitConverter.ToInt32(buf, pos); pos += 4;

            int unknown1 = BitConverter.ToInt32(buf, pos); pos += 4;
            int header_size = BitConverter.ToInt32(buf, pos); pos += 4;

            // get string chunk info
            int stringBlockOffset = BitConverter.ToInt32(buf, pos); pos += 4;
            int stringBlockSize = BitConverter.ToInt32(buf, pos); pos += 8;

            // get data chunk info
            int dataBlockOffset = BitConverter.ToInt32(buf, pos); pos += 4;

            // A dummy root node that we throw away.
            TreeNode dummyRoot = new TreeNode();
            dummyRoot.Text = "DUMMYROOT";

            // Load the nodes recursively.
            parseNode(buf, nodeBlockOffset, 0, stringBlockOffset, dataBlockOffset, dummyRoot);

            return dummyRoot.Nodes[0];
        }

        /// <summary>
        /// Decode a node from the UTF file.
        /// </summary>
        /// <param name="buf">The byte array from the file.</param>
        /// <param name="nodeBlockStart">The offset in bytes to the start of the node block in buf</param>
        /// <param name="nodeStart">The offset to the current parent node in the node block</param>
        /// <param name="stringBlockOffset">The offset in bytes to the start of the string block in buf</param>
        /// <param name="dataBlockOffset">The offset in bytes to the start of the data block in buf</param>
        /// <param name="parent">The parent TreeNode</param>
        private void parseNode(byte[] buf, int nodeBlockStart, int nodeStart, int stringBlockOffset, int dataBlockOffset, TreeNode parent)
        {
            int offset = nodeBlockStart + nodeStart;

            while (true)
            {
                int nodeOffset = offset;

                int peerOffset = BitConverter.ToInt32(buf, offset); offset += 4;        // next node on same level
                int nameOffset = BitConverter.ToInt32(buf, offset); offset += 4;        // string for this node
                int flags = BitConverter.ToInt32(buf, offset); offset += 4;             // bit 4 set = intermediate, bit 7 set = leaf
                int zero = BitConverter.ToInt32(buf, offset); offset += 4;              // always seems to be zero
                int childOffset = BitConverter.ToInt32(buf, offset); offset += 4;       // next node in if intermediate, offset to data if leaf
                int allocatedSize = BitConverter.ToInt32(buf, offset); offset += 4;     // leaf node only, 0 for intermediate
                int size = BitConverter.ToInt32(buf, offset); offset += 4;              // leaf node only, 0 for intermediate
                int size2 = BitConverter.ToInt32(buf, offset); offset += 4;             // leaf node only, 0 for intermediate
                int u1 = BitConverter.ToInt32(buf, offset); offset += 4;                // seems to be timestamps. can be zero
                int u2 = BitConverter.ToInt32(buf, offset); offset += 4;
                int u3 = BitConverter.ToInt32(buf, offset); offset += 4;

                // Extract the node name
                int len = 0;
                for (int i = stringBlockOffset + nameOffset; i < buf.Length && buf[i] != 0; i++, len++) ;
                string name = System.Text.Encoding.ASCII.GetString(buf, stringBlockOffset + nameOffset, len);

                // Extract data if this is a leaf.
                byte[] data;
                if ((flags & 0xFF) == 0x80)
                {
                    if (size != size2)
                        MessageBox.Show("Possible compression being used on " + name, "Warning");

                    data = new byte[allocatedSize];
                    Buffer.BlockCopy(buf, childOffset + dataBlockOffset, data, 0, allocatedSize);
                }
                else
                {
                    data = new byte[0];
                }

                TreeNode node = new TreeNode();
                node.Text = name;
                node.Tag = data;
                node.Name = name;
                parent.Nodes.Add(node);

                if (childOffset > 0 && flags == 0x10)
                    parseNode(buf, nodeBlockStart, childOffset, stringBlockOffset, dataBlockOffset, node);

                if (peerOffset > 0)
                    parseNode(buf, nodeBlockStart, peerOffset, stringBlockOffset, dataBlockOffset, parent);

                if ((flags & 0x80) == 0x80)
                    break;

                break;
            }
        }

        /// <summary>
        /// Save the data in the treeview displayed by this form back into
        /// the specified file.
        /// </summary>
        /// <param name="filename"></param>
        public void SaveUTFFile(TreeNode root, string filename)
        {
            List<byte> stringBlock = new List<byte>();
            List<byte> nodeBlock = new List<byte>();
            List<byte> dataBlock = new List<byte>();

            // Build the string block with duplicate strings removed to
            // reduce the file size.
            Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
            BuildStringBlock(nameOffsets, stringBlock, root);

            // A dummy root node that we throw away.
            TreeNode dummyRoot = new TreeNode();
            dummyRoot.Text = "DUMMYROOT";
            dummyRoot.Nodes.Add((TreeNode)(root.Clone()));

            // Built the nodes and data blocks
            BuildNode(nodeBlock, dataBlock, nameOffsets, dummyRoot);

            // Write the file.
            using (FileStream fs = File.Create(filename))
            {
                BinaryWriter w = new BinaryWriter(fs);

                int node_offset = 0x38;			// right after the header
                int string_offset = node_offset + nodeBlock.Count;
                int data_offset = string_offset + stringBlock.Count;

                int sig = 0x20465455;
                int ver = 0x101;
                w.Write(sig);
                w.Write(ver);

                w.Write(node_offset);
                w.Write(nodeBlock.Count);
                w.Write((int)0);
                w.Write((int)0x2c);
                w.Write(string_offset);
                w.Write(stringBlock.Count);
                w.Write(stringBlock.Count);
                w.Write((int)data_offset);
                w.Write((int)0);
                w.Write((int)0);
                w.Write((int)0);
                w.Write((int)0);

                w.Write(nodeBlock.ToArray());
                w.Write(stringBlock.ToArray());
                w.Write(dataBlock.ToArray());

                fs.Close();
            }
        }

        /// <summary>
        /// Add the children of a node to the relevant blocks.
        /// </summary>
        /// <param name="nodeBlock"></param>
        /// <param name="dataBlock"></param>
        /// <param name="nameOffsets">Array of string-index offsets in the string block</param>
        /// <param name="anode">The node to parse.</param>
        /// <returns>Offset to first byte in node block of the first node added to the node block</returns>
        private int BuildNode(List<byte> nodeBlock,
            List<byte> dataBlock,
            Dictionary<string, int> nameOffsets,
            TreeNode anode)
        {
            int firstNodeOffset = nodeBlock.Count;

            TreeNode childNode = anode.FirstNode;
            do
            {
                string name = childNode.Text;
                byte[] data = childNode.Tag as byte[];

                if (childNode.Nodes.Count == 0)
                {
                    int thisNodeStart = nodeBlock.Count();

                    nodeBlock.AddRange(BitConverter.GetBytes((int)0)); // peer offset
                    nodeBlock.AddRange(BitConverter.GetBytes(nameOffsets[name]));
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0x80)); // flags
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0)); // zero
                    nodeBlock.AddRange(BitConverter.GetBytes(dataBlock.Count)); // data offset
                    dataBlock.AddRange(data);
                    nodeBlock.AddRange(BitConverter.GetBytes(data.Length)); // data size
                    nodeBlock.AddRange(BitConverter.GetBytes(data.Length)); // data size2
                    nodeBlock.AddRange(BitConverter.GetBytes(data.Length)); // allocated data size
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0));
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0));
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0));

                    if (childNode.NextNode != null)
                    {
                        int peerOffset = nodeBlock.Count;
                        nodeBlock.RemoveRange(thisNodeStart + 0, 4);
                        nodeBlock.InsertRange(thisNodeStart + 0, BitConverter.GetBytes((int)peerOffset));
                    }
                }
                else
                {
                    int thisNodeStart = nodeBlock.Count();

                    nodeBlock.AddRange(BitConverter.GetBytes((int)0)); // peer offset
                    nodeBlock.AddRange(BitConverter.GetBytes(nameOffsets[childNode.Text]));
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0x10)); // flags
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0)); // zero
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0)); // child offset
                    nodeBlock.AddRange(BitConverter.GetBytes(0)); // data size
                    nodeBlock.AddRange(BitConverter.GetBytes(0)); // data size2
                    nodeBlock.AddRange(BitConverter.GetBytes(0)); // allocated data size
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0));
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0));
                    nodeBlock.AddRange(BitConverter.GetBytes((int)0));

                    int childNodeOffset = BuildNode(nodeBlock, dataBlock, nameOffsets, childNode);

                    if (childNode.NextNode != null)
                    {
                        int peerOffset = nodeBlock.Count;
                        nodeBlock.RemoveRange(thisNodeStart + 0, 4);
                        nodeBlock.InsertRange(thisNodeStart + 0, BitConverter.GetBytes((int)peerOffset));
                    }

                    nodeBlock.RemoveRange(thisNodeStart + 16, 4);
                    nodeBlock.InsertRange(thisNodeStart + 16, BitConverter.GetBytes((int)childNodeOffset));
                }

                childNode = childNode.NextNode;
            }
            while (childNode != null);

            return firstNodeOffset;
        }

        /// <summary>
        /// Scan the node names and produce a unique list of string and encode this into the string
        /// array in the UTF file. Keep a record of string name to offets into the array.
        /// </summary>
        private void BuildStringBlock(Dictionary<string, int> names, List<byte> stringBlock, TreeNode parent)
        {
            if (!names.ContainsKey(parent.Text))
            {
                names[parent.Text] = stringBlock.Count;
                stringBlock.AddRange(ASCIIEncoding.ASCII.GetBytes(parent.Text));
                stringBlock.Add(0);
            }

            foreach (TreeNode node in parent.Nodes)
            {
                // If this node has children then check the children
                if (node.Nodes.Count > 0)
                {
                    BuildStringBlock(names, stringBlock, node);
                }
                // Otherwise add the node name to the string list.
                else
                {
                    if (!names.ContainsKey(node.Text))
                    {
                        names[node.Text] = stringBlock.Count;
                        stringBlock.AddRange(ASCIIEncoding.ASCII.GetBytes(node.Text));
                        stringBlock.Add(0);
                    }
                }
            }
        }
    }
}
