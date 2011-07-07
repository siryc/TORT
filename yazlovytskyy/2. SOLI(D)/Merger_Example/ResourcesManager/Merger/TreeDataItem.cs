using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceMerger.Merger
{
    public interface TreeDataItem : DataItem
    {
        TreeDataItem Clone();
        List<TreeDataItem> ChildItems { get; set; }
    }
}
