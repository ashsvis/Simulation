
using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class OR : CommonLogic, IContextMenu { public OR() : base(LogicFunction.Or, 2) { }

        public void AddMenuItems(ContextMenuStrip contextMenu)
        {
            ToolStripMenuItem item;
            item = new ToolStripMenuItem() { Text = "Добавить вход", Tag = this };
            item.Click += (s, e) =>
            {
                var menuItem = (ToolStripMenuItem?)s;
                if (menuItem?.Tag is Element element)
                {
                    //or.AddInput();
                    //Project.Changed = true;
                    //zoomPad.Invalidate();
                }
            };
            contextMenu.Items.Add(item);
        }

        public void ClearContextMenu(ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Clear();
        }
    }
    public class OR3 : CommonLogic { public OR3() : base(LogicFunction.Or, 3) { } }
    public class OR4 : CommonLogic { public OR4() : base(LogicFunction.Or, 4) { } }
    public class OR5 : CommonLogic { public OR5() : base(LogicFunction.Or, 5) { } }
    public class OR6 : CommonLogic { public OR6() : base(LogicFunction.Or, 6) { } }
    public class OR7 : CommonLogic { public OR7() : base(LogicFunction.Or, 7) { } }
    public class OR8 : CommonLogic { public OR8() : base(LogicFunction.Or, 8) { } }
}
