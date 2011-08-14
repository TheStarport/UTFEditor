using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

// Inspired by code from Stephane Rodriguez
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
                if (selectedItems.Count == 0) this.SelectedNode = null;
                else if (!selectedItems.Contains(this.SelectedNode))
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
                nodes.Reverse();

                if (!ContainsNode(nodes, targetNode))
                {
                    if (e.Effect == DragDropEffects.Move)
                    {
                        foreach (TreeNode n in nodes)
                        {
                            if (n.Parent != null && nodes.Contains(n.Parent)) continue;

                            n.Remove();
                            targetNode.Nodes.Insert(0, n);
                        }
                    }
                    else if (e.Effect == DragDropEffects.Copy)
                    {
                        ArrayList newSelectedNodes = new ArrayList();

                        foreach (TreeNode n in nodes)
                        {
                            if (n.Parent != null && nodes.Contains(n.Parent)) continue;

                            TreeNode newNode = (TreeNode)n.Clone();
                            if (n.IsExpanded) newNode.Expand();
                            newSelectedNodes.Add(newNode);

                            TreeNode n2 = newNode.FirstNode;
                            TreeNode n2o = n.FirstNode;

                            while(n2 != null)
                            {
                                TreeNode next = n2.NextNode;
                                TreeNode nexto = n2o.NextNode;

                                if (!nodes.Contains(n2o))
                                    n2.Remove();
                                else if (newNode.IsExpanded)
                                    newSelectedNodes.Add(n2);

                                n2 = next;
                                n2o = nexto;
                            }

                            targetNode.Nodes.Insert(0, newNode);
                        }

                        newSelectedNodes.Add(targetNode);

                        this.SelectedNode = targetNode;
                        this.SelectedNodes = newSelectedNodes;
                    }

                    targetNode.Expand();
                }
            }

            isDragging = false;

            // Leaving this enabled causes the node that is actually dragged
            // to be duplicated and appended to the end of the children
            // of the target node.
            //      base.OnDragDrop(e);
        }

        private bool ContainsNode(ArrayList nodes, TreeNode node)
        {
            if (nodes.Contains(node)) return true;
            if (node.Parent == null) return false;

            return ContainsNode(nodes, node.Parent);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // The ItemDrag event does not get fired on right-click; known bug still not fixed.
            // First diagnosis dates from 2007. Work around it by force-calling it.
            if(e.Button == MouseButtons.Right)
                OnItemDrag(new ItemDragEventArgs(MouseButtons.Right));

            base.OnMouseDown(e);
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
