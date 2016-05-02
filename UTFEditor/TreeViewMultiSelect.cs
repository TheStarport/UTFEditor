using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

// Multiselection code inspired by works from Stephane Rodriguez
// http://www.arstdesign.com/articles/treeviewms.html

namespace UTFEditor
{
    class TreeViewMultiSelect : System.Windows.Forms.TreeView
    {
        protected ArrayList selectedItems;
        protected TreeNode lastNode, firstNode;
        protected bool preventSelectActions = false;

        public TreeViewMultiSelect() : base()
        {
            selectedItems = new ArrayList();
        }

        /***********************
         * MULTISELECTION CODE *
         ***********************/

        public ArrayList SelectedNodes
        {
            get
            {
                return selectedItems;
            }
            set
            {
                removePaintFromNodes();

                if (value.Count == 0) this.SelectedNode = null;
                else if (!value.Contains(this.SelectedNode))
                    this.SelectedNode = (TreeNode) value[0];

                selectedItems = value;
                paintSelectedNodes();
            }
        }

        public new TreeNode SelectedNode
        {
            get
            {
                return base.SelectedNode;
            }
            set
            {
                if (!selectedItems.Contains(value) && value != null)
                {
                    removePaintFromNodes();
                    selectedItems.Clear();
                    selectedItems.Add(value);
                    paintSelectedNodes();
                }

                base.SelectedNode = value;
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);
            if (preventSelectActions)
                return;

            bool ctrl = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;

            lastNode = e.Node;
            if(!shift) firstNode = e.Node;
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if(isDragging) return;

            base.OnAfterSelect(e);
            if (preventSelectActions)
                return;

            bool ctrl = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;

            if (ctrl)
            {
                if (!selectedItems.Contains(e.Node))
                    selectedItems.Add(e.Node);
                else
                {
                    removePaintFromNodes();
                    selectedItems.Remove(e.Node);

                    if (SelectedNode == e.Node)
                        SelectedNode = null;
                }

                paintSelectedNodes();
            }
            else if (shift)
            {
                Queue newItems = new Queue();

                TreeNode startNode = firstNode;
                TreeNode endNode = e.Node;

                int indexStart = -1, indexEnd = -1;

                // See whether nodes are reversed by calculating indices for both.
                // In order to have comparable indices, the nodes are tracked back
                // up as their parents until both resulting nodes are siblings,
                // at which point their indices are compared.
                {
                    int startDepth = startNode.GetDepth();
                    int endDepth = endNode.GetDepth();

                    TreeNode sNode = startNode;
                    TreeNode eNode = endNode;

                    if (startDepth > endDepth)
                        while (startDepth > endDepth)
                        {
                            sNode = sNode.Parent;
                            startDepth--;
                        }
                    else
                        while (startDepth < endDepth)
                        {
                            eNode = eNode.Parent;
                            endDepth--;
                        }

                    while (sNode.Parent != eNode.Parent)
                    {
                        sNode = sNode.Parent;
                        eNode = eNode.Parent;
                    }

                    indexStart = sNode.Index;
                    indexEnd = eNode.Index;
                }

                if (indexStart == indexEnd && startNode != endNode)
                {
                    // Identical indices mean one of the two nodes
                    // is the parent of the other. Find which and
                    // reorder appropriatedly.

                    TreeNode n = startNode;
                    while (n != null)
                    {
                        n = n.Parent;
                        if (n == endNode)
                        {
                            indexStart++;
                            break;
                        }
                    }
                }

                if (indexStart > indexEnd)
                {
                    TreeNode temp = startNode;
                    startNode = endNode;
                    endNode = temp;
                }

                {
                    TreeNode n = startNode;
                    while (n != endNode)
                    {
                        if (!selectedItems.Contains(n))
                            newItems.Enqueue(n);

                        if (n.Nodes.Count > 0 && n.IsExpanded)
                            n = n.FirstNode;
                        else if (n.NextNode != null)
                            n = n.NextNode;
                        else
                        {
                            while (n.NextNode == null) n = n.Parent;
                            n = n.NextNode;
                        }
                    }
                }

                if (!selectedItems.Contains(endNode))
                    newItems.Enqueue(endNode);

                selectedItems.AddRange(newItems);

                paintSelectedNodes();
                firstNode = e.Node;
            }
            else
            {
                if (selectedItems != null && selectedItems.Count > 0)
                {
                    removePaintFromNodes();
                    selectedItems.Clear();
                }
                selectedItems.Add(e.Node);
            }
        }

        protected void paintSelectedNodes()
        {
            foreach (TreeNode n in selectedItems)
            {
                n.BackColor = SystemColors.Highlight;
                n.ForeColor = SystemColors.HighlightText;
            }
        }

        protected void removePaintFromNodes()
        {
            if (selectedItems.Count == 0) return;

            Color back = this.BackColor;
            Color fore = this.ForeColor;

            foreach (TreeNode n in selectedItems)
            {
                n.BackColor = back;
                n.ForeColor = fore;
            }
        }

        protected void resetPaintNodes()
        {
            Color back = this.BackColor;
            Color fore = this.ForeColor;

            TreeNode n = this.Nodes[0];
            while (n != null)
            {
                n.BackColor = back;
                n.ForeColor = fore;
                System.Diagnostics.Debug.WriteLine(n.Name);

                if (n.Nodes.Count > 0)
                    n = n.FirstNode;
                else if (n.NextNode != null)
                    n = n.NextNode;
                else if (n.Parent != null)
                    n = n.Parent.NextNode;
                else
                    n = null;
            }
        }

        /**********************
         * DRAG-AND-DROP CODE *
         **********************/

        public delegate void TreeNodeEventHandler(object sender, TreeViewEventArgs args);

        public event TreeNodeEventHandler ModifiedNode;

        protected bool isDragging = false;

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            isDragging = true;

            if (e.Button == MouseButtons.Left)
                DoDragDrop(this.selectedItems.Clone(), DragDropEffects.Move);
            else if (e.Button == MouseButtons.Right)
                DoDragDrop(this.selectedItems.Clone(), DragDropEffects.Copy);

            base.OnItemDrag(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            isDragging = true;

            e.Effect = e.AllowedEffect;

            base.OnDragEnter(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;

            Point tgt = this.PointToClient(new Point(e.X, e.Y));

            this.SelectedNode = this.GetNodeAt(tgt);

            base.OnDragOver(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            Point tgt = this.PointToClient(new Point(e.X, e.Y));

            TreeNode targetNode = this.GetNodeAt(tgt);

            if (e.Data.GetDataPresent(typeof(ArrayList)))
            {
                ArrayList nodes = (ArrayList)e.Data.GetData(typeof(ArrayList));

                if (!ContainsNode(nodes, targetNode))
                {
                    if (e.Effect == DragDropEffects.Move)
                    {
                        Cut(nodes, false);
                        Paste(nodes, targetNode, true, false);
                    }
                    else if (e.Effect == DragDropEffects.Copy)
                    {
                        Copy(nodes, false);
                        Paste(nodes, targetNode, false, false);
                    }
                }
            }

            // Resume normal Select events
            isDragging = false;
        }

        private bool ContainsNode(ArrayList nodes, TreeNode node)
        {
            if (nodes.Contains(node)) return true;
            if (node.Parent == null) return false;

            return ContainsNode(nodes, node.Parent);
        }

        private bool isRightDragging = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // The ItemDrag event does not get fired on right-click; known bug still not fixed.
            // Work around it by calling it manually.
            if (e.Button == MouseButtons.Right)
                isRightDragging = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && isRightDragging)
            {
                isRightDragging = false;
                OnItemDrag(new ItemDragEventArgs(MouseButtons.Right));
            }

            base.OnMouseMove(e);
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            if (selectedItems.Contains(e.Node) && ModifierKeys == Keys.Control)
                SelectedNode = null;
            else
            {
                if (ModifierKeys == Keys.None)
                {
                    SelectedNodes = new ArrayList();
                    SelectedNode = e.Node;
                }

                base.OnNodeMouseClick(e);
            }
        }

        /***********************
         * CUT/COPY/PASTE CODE *
         ***********************/

        /// <summary>
        /// This object encapsulates treenodes to copy from one treeview to another.
        /// </summary>
        [Serializable]
        private class CopyNodesObject
        {
            public ArrayList Nodes = new ArrayList();

            public CopyNodesObject(ArrayList n)
            {
                Nodes = n;
            }
        };
        
        /// <summary>
        /// THe name of the object used for drag-down operations.
        /// </summary>
        private const string CopyNodesObjectName = "UTFEditor.UTFForm+CopyNodesObject";

        public void Copy()
        {
            Copy((ArrayList)this.selectedItems.Clone(), true);
        }

        public void Copy(ArrayList nodes, bool save)
        {
            if (save)
                Clipboard.SetData(CopyNodesObjectName, new CopyNodesObject(nodes));
        }

        public void Cut()
        {
            Cut((ArrayList)this.selectedItems.Clone(), true);
        }

        public void Cut(ArrayList nodes, bool save)
        {
            Delete(nodes);

            Copy(nodes, save);
        }

        public void Paste()
        {
            if (Clipboard.ContainsData(CopyNodesObjectName) && this.SelectedNode != null)
                Paste(((CopyNodesObject)Clipboard.GetData(CopyNodesObjectName)).Nodes, this.SelectedNode, true, false);
        }

        public void PasteChild()
        {
            TreeNode n = this.SelectedNode;
            if (Clipboard.ContainsData(CopyNodesObjectName) && this.SelectedNode != null && this.SelectedNode.Parent != null)
                Paste(((CopyNodesObject)Clipboard.GetData(CopyNodesObjectName)).Nodes, this.SelectedNode, true, true);
        }

        public void Paste(ArrayList nodes, TreeNode targetNode, bool pasteAllChildren, bool pasteAsChild)
        {
            if (targetNode == null)
                targetNode = this.Nodes[0];

            if (targetNode.Parent == null) // Root node must be unique
                pasteAsChild = true;

            ArrayList newSelectedNodes = new ArrayList();
            int index = pasteAsChild ? targetNode.Nodes.Count : targetNode.Index + 1;

            foreach (TreeNode n in nodes)
            {
                TreeNode newNode = (TreeNode)n.Clone();
                if (n.IsExpanded) newNode.Expand();
                newSelectedNodes.Add(newNode);

                TreeNode n2 = newNode.FirstNode;
                TreeNode n2o = n.FirstNode;

                while (n2 != null)
                {
                    TreeNode next = n2.NextNode;
                    TreeNode nexto = n2o.NextNode;

                    if (!pasteAllChildren && !nodes.Contains(n2o))
                        n2.Remove();
                    else if (newNode.IsExpanded)
                        newSelectedNodes.Add(n2);

                    n2 = next;
                    n2o = nexto;
                }

                // Ensure that pasted nodes cannot have the same name as an existing node
                {
                    string name = newNode.Text;
                    int count = 1;

                    bool found;
                    do
                    {
                        found = false;
                        foreach (TreeNode nn in pasteAsChild ? targetNode.Nodes : targetNode.Parent.Nodes)
                        {
                            if (nn.Text == newNode.Text)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found == true)
                        {
                            Regex r = new Regex(@"^.+(?<num> \([0-9]+\))$");
                            Match m = r.Match(name);
                            if (m.Success)
                                name = name.Substring(0, m.Groups["num"].Index);
                            newNode.Text = name + " (" + count + ")";
                            newNode.Name = name + " (" + count + ")";
                            count++;
                        }

                    } while (found == true);
                }

                if(pasteAsChild)
                    targetNode.Nodes.Insert(index, newNode);
                else
                    targetNode.Parent.Nodes.Insert(index, newNode);

                OnModifiedNode(new TreeViewEventArgs(targetNode));

                index++;
            }

            OnModified(new EventArgs());
            
            preventSelectActions = true;
            this.SelectedNodes = newSelectedNodes;
            preventSelectActions = false;
        }

        public void Delete()
        {
            Delete((ArrayList)this.selectedItems.Clone());
        }

        public void Delete(ArrayList nodes)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.Parent != null && nodes.Contains(n.Parent)) continue;

                var parent = n.Parent;

                n.Remove();
                
                OnModifiedNode(new TreeViewEventArgs(parent));
            }

            OnModified(new EventArgs());
        }

        public event EventHandler Modified;

        protected void OnModified(EventArgs e)
        {
            if (Modified != null)
                Modified(this, e);

            resetPaintNodes();
        }

        protected void OnModifiedNode(TreeViewEventArgs e)
        {
            if (ModifiedNode != null)
                ModifiedNode(this, e);
        }
    }

    public static class TreeNodeExtensions
    {
        public static int GetDepth(this TreeNode node)
        {
            int depth = 0;
            while (node.Parent != null)
            {
                node = node.Parent;
                depth++;
            }

            return depth;
        }
    }
}
