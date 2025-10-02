using Simulator.Model.Interfaces;
using System.Xml.Linq;

namespace Simulator.Model
{
    public class Element: IChangeIndex, IContextMenu
    {
        public const float Step = 8f;

        public Element(List<Element>? items = null) 
        {
            Elements = items;
        }

        public int Index { get; set; }
        public bool Selected { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public Type? Type { get; set; }
        
        public object? Instance { get; set; }

        public PointF Location 
        { 
            get => location;
            set
            {
                if (location == value) return;
                location = value;
                if (Instance is ILinkSupport element)
                    element.CalculateTargets(location, ref size, itargets, ipins, otargets, opins);
            }
        }

        public void CalculateTargets()
        {
            if (Instance is ILinkSupport element)
                element.CalculateTargets(location, ref size, itargets, ipins, otargets, opins);
        }

        public override string? ToString()
        {
            return Instance != null ? Instance.GetType().Name.ToString() : base.ToString();
        }

        public void Save(XElement xtem)
        {
            xtem.Add(new XAttribute("Id", Id));
            xtem.Add(new XAttribute("X", Location.X));
            xtem.Add(new XAttribute("Y", Location.Y));
            if (Instance is ILoadSave instance)
            {
                var xtance = new XElement("Instance");
                xtance.Add(new XAttribute("Type", instance.GetType()));
                xtem.Add(xtance);
                if (!string.IsNullOrWhiteSpace(instance.Name))
                    xtance.Add(new XAttribute("Name", instance.Name));
                instance.Save(xtance);
            }
        }

        public void Load(XElement item, Type type, IVariable manager)
        {
            if (!int.TryParse(item.Attribute("X")?.Value, out int x)) return;
            if (!int.TryParse(item.Attribute("Y")?.Value, out int y)) return;
            Instance = Activator.CreateInstance(type);
            Location = new Point(x, y);
            if (Instance is ILinkSupport link)
            {
                link.SetItemId(Id);
            }
            if (Instance is ILoadSave inst) 
                inst.Load(item.Element("Instance"));
        }

        public SizeF Size { get => size; set => size = value; }

        public RectangleF Bounds => new(location, Size);

        private readonly Dictionary<int, RectangleF> itargets = [];
        private readonly Dictionary<int, RectangleF> otargets = [];
                
        private readonly Dictionary<int, PointF> ipins = [];
        private readonly Dictionary<int, PointF> opins = [];
        protected PointF location;
        private SizeF size;

        public Dictionary<int, PointF> InputPins => ipins;
        public Dictionary<int, PointF> OutputPins => opins;

        public List<Element>? Elements { get; private set; }
        public List<Link>? Links { get; private set; }

        public void Assign(List<Element> elements, List<Link> links)
        {
            Elements = elements;
            Links = links;
        }

        public bool TryGetOutput(PointF point, out int? output, out PointF? pin)
        {
            output = null;
            pin = null;
            foreach (var key in otargets.Keys)
            {
                var target = otargets[key];
                if (target.Contains(point))
                {
                    output = key;
                    pin = opins.TryGetValue(key, out PointF value) ? value : null;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetInput(PointF point, out int? input, out PointF? pin)
        {
            input = null;
            pin = null;
            foreach (var key in itargets.Keys)
            {
                var target = itargets[key];
                if (target.Contains(point))
                {
                    input = key;
                    pin = ipins.TryGetValue(key, out PointF value) ? value : null;
                    return true;
                }
            }
            return false;
        }

        public void Draw(Graphics graphics, Color foreColor, Color backColor, CustomDraw? customDraw = null)
        {
            if (Instance is IDraw instance)
            {
                instance.Draw(graphics, foreColor, backColor, location, Size, Index, Selected, customDraw);
            }

#if DEBUG

            //// области выбора
            //using Pen tarpen = new(Color.FromArgb(80, Color.Magenta), 0);
            //foreach (var key in itargets.Keys)
            //{
            //    var itarget = itargets[key];
            //    graphics.DrawRectangles(tarpen, [itarget]);
            //}
            //foreach (var key in otargets.Keys)
            //{
            //    var otarget = otargets[key];
            //    graphics.DrawRectangles(tarpen, [otarget]);
            //}
            //// точки привязки входов и выходов
            //using Pen pinpen = new(Color.FromArgb(255, Color.Black), 0);
            //foreach (var key in ipins.Keys)
            //{
            //    var pt = ipins[key];
            //    var r = new RectangleF(pt.X - 3, pt.Y - 3, 6, 6);
            //    graphics.DrawLine(pinpen, new PointF(r.X, r.Y), new PointF(r.X + r.Width, r.Y + r.Height));
            //    graphics.DrawLine(pinpen, new PointF(r.X + r.Width, r.Y), new PointF(r.X, r.Y + r.Height));
            //}
            //foreach (var key in opins.Keys)
            //{
            //    var pt = opins[key];
            //    var r = new RectangleF(pt.X - 3, pt.Y - 3, 6, 6);
            //    graphics.DrawLine(pinpen, new PointF(r.X, r.Y), new PointF(r.X + r.Width, r.Y + r.Height));
            //    graphics.DrawLine(pinpen, new PointF(r.X + r.Width, r.Y), new PointF(r.X, r.Y + r.Height));
            //}

#endif
        }

        public void AddMenuItems(ContextMenuStrip contextMenu)
        {
            ToolStripMenuItem item;
            item = new ToolStripMenuItem() { Text = "Изменить номер...", Tag = this };
            item.Click += (s, e) =>
            {
                var menuItem = (ToolStripMenuItem?)s;
                if (menuItem?.Tag is Element element && element is IChangeIndex changer && Elements != null)
                {
                    var dlg = new Simulator.View.ChangeNumberDialog(changer.Index);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.EnteredValue > 0 && dlg.EnteredValue <= Elements.Count)
                        {
                            var tmp = Elements[changer.Index - 1];
                            Elements.Remove(tmp);
                            Elements.Insert(dlg.EnteredValue - 1, tmp);
                            Project.Changed = true;
                        }
                    }
                }
            };
            contextMenu.Items.Add(item);
            contextMenu.Items.Add(new ToolStripSeparator());
            if (this.Instance is IContextMenu context)
            {
                var count = contextMenu.Items.Count;
                context.AddMenuItems(contextMenu);
                if (contextMenu.Items.Count > count)
                    contextMenu.Items.Add(new ToolStripSeparator());
            }
            item = new ToolStripMenuItem() { Text = "Удалить элемент", Tag = this };
            item.Click += (s, e) =>
            {
                var menuItem = (ToolStripMenuItem?)s;
                if (menuItem?.Tag is Element element)
                {
                    if (MessageBox.Show("Не, реально удалить?", "Удаление элемента",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DeleteOneElement(element);
                    }
                }
            };
            contextMenu.Items.Add(item);
        }

        private void DeleteOneElement(Element element)
        {
            if (Elements == null) return;
            foreach (var item in Elements)
            {
                if (item.Instance is ILinkSupport func)
                {
                    var n = 0;
                    foreach (var islinked in func.LinkedInputs)
                    {
                        if (islinked)
                        {
                            var linkSources = func.InputLinkSources;
                            (Guid id, int index, bool external) = linkSources[n];
                            if (Elements.FirstOrDefault(x => x.Id == id) == element)
                            {
                                func.ResetValueLinkToInp(n);
                            }
                        }
                        n++;
                    }
                }
            }
            Links?.RemoveAll(link => link.SourceId == element.Id || link.DestinationId == element.Id);
            Elements.Remove(element);
            Project.Changed = true;
        }

        public void ClearContextMenu(ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Clear();
        }
    }
}
