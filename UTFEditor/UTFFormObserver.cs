using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public interface UTFFormObserver
    {
        void DataChanged(TreeNode node, string oldName, object oldData);
    }
}
