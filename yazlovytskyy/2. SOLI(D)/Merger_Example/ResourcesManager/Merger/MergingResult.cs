using System.Collections.Generic;

namespace ResourceMerger.Merger
{
    public class MergingResultLog
    {
        public List<string> InsertedLines { get; set; }
        public MergingResultLog()
        {
            InsertedLines = new List<string>();
        }
    }
}
