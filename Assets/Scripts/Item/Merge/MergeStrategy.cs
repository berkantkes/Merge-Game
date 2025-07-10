public class MergeStrategy
{
    public bool CheckMerge(ItemController selectedItem, SingleGridController targetGrid)
    {
        ItemController targetItem = targetGrid.GetItem();

        if (targetItem.GetBoardItemFamilyType() == selectedItem.GetBoardItemFamilyType())
        {
            if (targetItem.GetLevel() == selectedItem.GetLevel())
            {
                return true;
            }
        }

        return false;
    }
}
