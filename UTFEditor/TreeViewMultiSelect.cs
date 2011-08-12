using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

// Heavily based off code by Stephane Rodriguez
// http://www.arstdesign.com/articles/treeviewms.html

namespace UTFEditor
{
    class TreeViewMultiSelect : System.Windows.Forms.TreeView
    {
        protected ArrayList selectedItems;
        protected TreeNode lastNode, firstNode;

        public TreeViewMultiSelect() : base()
        {
            selectedItems = new ArrayList();
        }

        public ArrayList SelectedNodes
        {
            get
            {
                return selectedItems;
            }
            set
            {
                removePaintFromNodes();
                selectedItems.Clear();
                selectedItems = value;
                if (!selectedItems.Contains(this.SelectedNode))
                    this.SelectedNode = (TreeNode) selectedItems[0];
                paintSelectedNodes();
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);

            bool ctrl = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;

            if(ctrl && selectedItems.Contains(e.Node))
            {
                e.Cancel = true;

                removePaintFromNodes();
                selectedItems.Remove(e.Node);
                paintSelectedNodes();
                return;
            }

            lastNode = e.Node;
            if(!shift) firstNode = e.Node;
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if(isDragging) return;

            base.OnAfterSelect(e);

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
                }

                paintSelectedNodes();
            }
            else if (shift)
            {
                Queue newItems = new Queue();

                TreeNode startNode = firstNode;
                TreeNode endNode = e.Node;

                // See whether nodes are direct descendants of one another
                bool parent = isParent(startNode, endNode);
                if (!parent)
                {
                    parent = isParent(endNode, startNode);

                    // See whether nodes are reversed
                    if (parent)
                    {
                        TreeNode temp = startNode;
                        startNode = endNode;
                        endNode = temp;
                    }
                }

                if (parent)
                {
                    TreeNode n = endNode;
                    while (n != startNode)
                    {

                        if (!selectedItems.Contains(n))
                            newItems.Enqueue(n);

                        while (n.PrevNode != null)
                        {
                            n = n.PrevNode;

                            EnqueueNodeAndChildren(n, newItems);
                        }

                        n = n.Parent;
                    }

                    if (!selectedItems.Contains(startNode))
                        newItems.Enqueue(startNode);
                }
                else
                {
                    int indexStart = -1, indexEnd = -1;

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
                    // See whether nodes are reversed
                    if (indexStart > indexEnd)
                    {
                        TreeNode temp = startNode;
                        startNode = endNode;
                        endNode = temp;

                        indexStart = startNode.Index;
                        indexEnd = endNode.Index;
                    }

                    TreeNode n = startNode;
                    while (n != endNode)
                    {
                        if (!selectedItems.Contains(n))
                            newItems.Enqueue(n);

                        if (n.Nodes.Count > 0)
                            n = n.FirstNode;
                        else if (n.NextNode != null)
                            n = n.NextNode;
                        else
                        {
                            while (n.NextNode == null) n = n.Parent;
                            n = n.NextNode;
                        }
                    }

                    if (!selectedItems.Contains(endNode))
                        newItems.Enqueue(endNode);
                }

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

            TreeNode n0 = (TreeNode)selectedItems[0];
            if (n0.TreeView == null) return;

            Color back = n0.TreeView.BackColor;
            Color fore = n0.TreeView.ForeColor;

            foreach (TreeNode n in selectedItems)
            {
                n.BackColor = back;
                n.ForeColor = fore;
            }
        }

        protected bool isParent(TreeNode parent, TreeNode child)
        {
            TreeNode n = child;
            while (n.Parent != null)
            {
                if (n.Parent == parent) return true;
                else n = n.Parent;
            }

            return false;
        }

        protected bool isDragging = false;

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            isDragging = true;

            foreach (TreeNode n in this.selectedItems)
                System.Diagnostics.Debug.WriteLine(n.Name);

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

            ArrayList lst = new ArrayList();
            lst.Add(this.GetNodeAt(tgt));

            this.SelectedNodes = lst;

            base.OnDragOver(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            isDragging = false;

            Point tgt = this.PointToClient(new Point(e.X, e.Y));

            TreeNode targetNode = this.GetNodeAt(tgt);

            if (e.Data.GetDataPresent(typeof(ArrayList)))
            {
                ArrayList nodes = (ArrayList)e.Data.GetData(typeof(ArrayList));

                if (!ContainsNode(nodes, targetNode))
                {
                    if (e.Effect == DragDropEffects.Move)
                    {
                        foreach (TreeNode n in nodes)
                        {
                            n.Remove();
                            targetNode.Nodes.Add(n);
                        }
                    }
                    else if (e.Effect == DragDropEffects.Copy)
                    {
                        foreach (TreeNode n in nodes)
                            targetNode.Nodes.Add((TreeNode)n.Clone());
                    }

                    targetNode.Expand();
                }
            }

            base.OnDragDrop(e);
        }

        private bool ContainsNode(ArrayList nodes, TreeNode node)
        {
            if (nodes.Contains(node)) return true;
            if (node.Parent == null) return false;

            return ContainsNode(nodes, node.Parent);
        }

        private void EnqueueNodeAndChildren(TreeNode node, Queue queue)
        {
            if (node == null || queue == null) return;

            if (!queue.Contains(node))
                queue.Enqueue(node);

            foreach (TreeNode n in node.Nodes)
                EnqueueNodeAndChildren(n, queue);
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
