using System;
using System.Xml;
using System.Collections.Generic;


namespace ResourceMerger.Merger
{
    public abstract class TreeMerger
    {
        public TreeDataItem MergingSource { get; set; }
        public TreeDataItem MergingDestination { get; set; }
        public MergingResultLog MergingResult { get; set; }
        public ListDataMerger ListDataMerger { get; set; }

        public TreeMerger(TreeDataItem mergingSource, TreeDataItem mergingDestination)
        {
            if (mergingSource == null || mergingDestination == null)
            {
                throw new ArgumentNullException();
            }
            MergingSource = mergingSource;
            MergingDestination = mergingDestination;
        }

        public void Merge()
        {
            foreach (var item in MergingSource.ChildItems)
            {
                var missedItems = GetMissedItems();
                CopyItemsToDestination(missedItems);
            }
        }

        private abstract List<DataItem> GetMissedItems() 
        {
            var missdItems = new List<DataItem>();
            if (SourcesAreIdentical())
            {
                return missdItems;
            }
            foreach (var item in MergingSource)
            {
                if (!DestinationContainsItem(item))
                {
                    missdItems.Add(item);
                }
            }
            return missdItems;
        }

        private abstract void CopyItemsToDestination(List<DataItem> items)
        {
            foreach (var item in items)
            {
                CopyItem(item, MergingDestination);
            }
        }

        private abstract void CopyItem(DataItem item, List<DataItem> destination);

        private abstract bool DestinationContainsItem(DataItem item)
        {
            return FindCorrespondingItem(item) != null;
        }

        private abstract DataItem FindCorrespondingItem(DataItem item);

        private bool SourcesAreIdentical()
        {
            return MergingSource.Equals(MergingDestination);
        }
    }
}