using System.Collections;
namespace tainicom.TreeViewEx
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal interface ISelectionStrategy
    {
        void SelectFromUiAutomation(TreeViewExItem item);

        void SelectPreviousFromKey();

        void SelectNextFromKey();

        void SelectCurrentBySpace();

        void SelectFromProperty(TreeViewExItem item, bool isSelected);

        void SelectFirst();

        void SelectLast();

        void ClearObsoleteItems(IEnumerable items);
    }
}
