using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTFEditor
{
    public enum DataChangedType
    {
        Mesh = 1,
        Hardpoints = 2,
        All = Mesh | Hardpoints
    }

    public interface UTFFormObserver
    {
        void DataChanged(DataChangedType changedType);
        void Invalidate();
        void Close();
    }
}
