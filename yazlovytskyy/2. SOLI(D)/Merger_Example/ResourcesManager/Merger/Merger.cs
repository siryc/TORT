using System;
using System.Xml;
using System.Collections.Generic;


namespace ResourceMerger.Merger
{
    public abstract class Merger
    {
        public List<DataItem> MergingSource { get; set; }
        public List<DataItem> MergingDestination { get; set; }
        public MergingResultLog MergingResult { get; set; }

        public Merger(List<DataItem> source, List<DataItem> destination)
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
            var missedItems = MergingSource.GetMissedItemsIn(MergingDestination);
            CopyItemsToDestination(missedItems);
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