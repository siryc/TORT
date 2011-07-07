using System;
using System.Xml;
using System.Collections.Generic;

namespace ResourceMerger.Merger
{
    public class TreeDataMerger
    {
        public TreeDataItem MergingSource { get; set; }
        public TreeDataItem MergingDestination { get; set; }
        public MergingResultLog MergingResult { get; set; }
        public ListDataMerger ListDataMerger { get; set; }

        public static MergingResultLog MergeNodes(TreeDataItem source, TreeDataItem destination, ListDataMerger listDataMerger)
        {
            var merger = new TreeDataMerger(source, destination, listDataMerger);
            merger.Merge();
            return merger.MergingResult;
        }

        public TreeDataMerger(TreeDataItem mergingSource, TreeDataItem mergingDestination, ListDataMerger listDataMerger)
        {
            if (mergingSource == null || mergingDestination == null || listDataMerger == null)
            {
                throw new ArgumentNullException();
            }
            MergingSource = mergingSource;
            MergingDestination = mergingDestination;
        }

        public void Merge()
        {
            MergingResult = new MergingResultLog();

            if (SourcesAreIdentical() || MergingSource.ChildItems.Count == 0)
            {
                return;
            }

            for (var i = 0; i < MergingSource.ChildItems.Count; i++)
            {
                var item = MergingSource.ChildItems[i];
                InsertItemIfNotExist(item, i);
            }
        }

        private void InsertItemIfNotExist(TreeDataItem item, int insertPos)
        {
            var correspondingItem = MergingDestination.ChildItems.Find((t) => t == item);
            if (correspondingItem == null)
            {
                CopyItem(item, MergingDestination.ChildItems, insertPos);
                MergingResult.InsertedLines.Add(item.ToString());
            }
            else
            {
                var result = MergeNodes(item, correspondingItem, ListDataMerger);
                MergingResult.InsertedLines.AddRange(result.InsertedLines);
            }
        }
        private void CopyItem(TreeDataItem item, List<TreeDataItem> destination, int pos)
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
