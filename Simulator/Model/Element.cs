using System.Xml.Linq;

namespace Simulator.Model
{
    public class Element
    {
        public const float Step = 8f;//6f;

        public Element() 
        { 
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

        public override string? ToString()
        {
            return Instance != null ? Instance.GetType().Name.ToString() : base.ToString();
        }

        public void Save(XElement xtem)
        {
            xtem.Add(new XElement("Id", Id));
            xtem.Add(new XElement("Type", Instance?.GetType()));
            xtem.Add(new XAttribute("X", Location.X));
            xtem.Add(new XAttribute("Y", Location.Y));
            if (Instance is ILoadSave instance)
                instance.Save(xtem);
        }

        public void Load(XElement item, Type type)
        {
            if (!int.TryParse(item.Attribute("X")?.Value, out int x)) return;
            if (!int.TryParse(item.Attribute("Y")?.Value, out int y)) return;
            Instance = Activator.CreateInstance(type);
            Location = new Point(x, y);
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

            // области выбора
            using Pen tarpen = new(Color.FromArgb(80, Color.Magenta), 0);
            foreach (var key in itargets.Keys)
            {
                var itarget = itargets[key];
                graphics.DrawRectangles(tarpen, [itarget]);
            }
            foreach (var key in otargets.Keys)
            {
                var otarget = otargets[key];
                graphics.DrawRectangles(tarpen, [otarget]);
            }
            // точки привязки входов и выходов
            using Pen pinpen = new(Color.FromArgb(255, Color.Black), 0);
            foreach (var key in ipins.Keys)
            {
                var pt = ipins[key];
                var r = new RectangleF(pt.X - 3, pt.Y - 3, 6, 6);
                graphics.DrawLine(pinpen, new PointF(r.X, r.Y), new PointF(r.X + r.Width, r.Y + r.Height));
                graphics.DrawLine(pinpen, new PointF(r.X + r.Width, r.Y), new PointF(r.X, r.Y + r.Height));
            }
            foreach (var key in opins.Keys)
            {
                var pt = opins[key];
                var r = new RectangleF(pt.X - 3, pt.Y - 3, 6, 6);
                graphics.DrawLine(pinpen, new PointF(r.X, r.Y), new PointF(r.X + r.Width, r.Y + r.Height));
                graphics.DrawLine(pinpen, new PointF(r.X + r.Width, r.Y), new PointF(r.X, r.Y + r.Height));
            }

#endif
        }

    }
}
