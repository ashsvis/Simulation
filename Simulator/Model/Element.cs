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
                CalculateTargets();
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

        public SizeF Size { get; set; }

        public RectangleF Bounds => new(location, Size);

        private readonly Dictionary<int, RectangleF> itargets = [];
        private readonly Dictionary<int, RectangleF> otargets = [];

        private readonly Dictionary<int, PointF> ipins = [];
        private readonly Dictionary<int, PointF> opins = [];
        protected PointF location;

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

        public void CalculateTargets()
        {
            if (Instance is IFunction instance)
            {
                var max = Math.Max(instance.InverseInputs.Length, instance.InverseOutputs.Length);
                var step = Step;
                var height = step + max * step * 4 + step;
                var width = step + 1 * step * 4 + step;
                Size = new SizeF(width, height);
                // входы
                var y = step + location.Y;
                var x = -step + location.X;
                var n = 0;
                itargets.Clear();
                ipins.Clear();
                for (var i = 0; i < instance.InverseInputs.Length; i++)
                {
                    y += step * 2;
                    // значение входа
                    var ms = new SizeF(step * 2, step * 2);
                    itargets.Add(n, new RectangleF(new PointF(x - ms.Width + step, y - ms.Height), ms));
                    ipins.Add(n, new PointF(x, y));
                    y += step * 2;
                    n++;
                }
                // выходы
                y = step + location.Y;
                x = width + location.X;
                n = 0;
                otargets.Clear();
                opins.Clear();
                for (var i = 0; i < instance.InverseOutputs.Length; i++)
                {
                    if (instance.InverseOutputs.Length == 1)
                        y = height / 2 + location.Y;
                    else
                        y += step * 2;
                    // значение выхода
                    var ms = new SizeF(step * 2, step * 2);
                    otargets.Add(n, new RectangleF(new PointF(x, y - ms.Height), ms));
                    var pt = new PointF(x + step, y);
                    opins.Add(n, pt);
                    y += step * 2;
                    n++;
                }
            }

        }

        public virtual void Draw(Graphics graphics, Color foreColor, Color backColor, CustomDraw? customDraw = null)
        {
            if (Instance is IDraw instance)
            {
                instance.Draw(graphics, foreColor, backColor, location, Size, customDraw);
            }
        }

    }
}
