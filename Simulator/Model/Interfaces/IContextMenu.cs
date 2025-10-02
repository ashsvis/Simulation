namespace Simulator.Model.Interfaces
{
    public interface IContextMenu
    {
        void ClearContextMenu(ContextMenuStrip contextMenu);
        void AddMenuItems(ContextMenuStrip contextMenu);
    }
}