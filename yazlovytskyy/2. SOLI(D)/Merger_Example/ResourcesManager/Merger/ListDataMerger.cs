using System;
using System.Xml;
using System.Collections.Generic;


namespace ResourceMerger.Merger
{
    public class ListDataMerger
    {
        public List<DataItem> MergingSource { get; set; }
        public List<DataItem> MergingDestination { get; set; }
        public MergingResultLog MergingResult { get; set; }

        public static MergingResultLog MergeNodes(List<DataItem> source, List<DataItem> destination)
        {
            var merger = new Merger(source, destination);
            merger.Merge();
            return merger.MergingResult;
        }

        public ListDataMerger(List<DataItem> source, List<DataItem> destination)
        {
            if (source == null || destination == null)
            {
                throw new ArgumentNullException();
            }
            MergingSource = source;
            MergingDestination = destination;
        }

        public void Merge()
        {
            MergingResult = new MergingResultLog();

            if (SourcesAreIdentical() || MergingSource.Count == 0)
            {
                return;
            }

            for (var i = 0; i < MergingSource.Count; i++)
            {
                ProcessSourceItem(i);
            }
        }

        private void ProcessSourceItem(int itemNumber) {
            var item = MergingSource[itemNumber];
            CopyItemIfNotExist(item, itemNumber);
        }

        private void CopyItemIfNotExist(DataItem item, int insertPos)
        {
            var correspondingItem = MergingDestination.Find((t) => t == item);
            if (correspondingItem != null)
            {
                return;
            }
            CopyItem(item, MergingDestination, insertPos);
            MergingResult.InsertedLines.Add(item.ToString());
        }

        private void CopyItem(DataItem item, List<DataItem> destination, int pos)
        {
            var itemToInsert = item.Clone();
            destination.Insert(pos, itemToInsert);
        }

        private bool SourcesAreIdentical()
        {
            return MergingSource.Equals(MergingDestination);
        }
    }
}