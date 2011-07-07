using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceMerger.Merger
{
    public interface DataItem
    {
        DataItem Clone();
        String ToString();
    }
}
