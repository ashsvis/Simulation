using Simulator.Model.Interfaces;

namespace Simulator.Model.Logic
{
    public class AND : CommonLogic, IContextMenu { public AND() : base(LogicFunction.And, 2) { }

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
    public class AND3 : CommonLogic { public AND3() : base(LogicFunction.And, 3) { } }
    public class AND4 : CommonLogic { public AND4() : base(LogicFunction.And, 4) { } }
    public class AND5 : CommonLogic { public AND5() : base(LogicFunction.And, 5) { } }
    public class AND6 : CommonLogic { public AND6() : base(LogicFunction.And, 6) { } }
    public class AND7 : CommonLogic { public AND7() : base(LogicFunction.And, 7) { } }
    public class AND8 : CommonLogic { public AND8() : base(LogicFunction.And, 8) { } }
}
