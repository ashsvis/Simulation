using System.Xml.Linq;

namespace Simulator.Model
{
    public class Element
    {
        public static float Step = 6f;

        public Element() 
        { 
        }

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

        public void Save(XElement xtem)
        {
            xtem.Add(new XElement("Id", Id));
            xtem.Add(new XElement("Type", Instance?.GetType()));
            xtem.Add(new XAttribute("X", Location.X));
            xtem.Add(new XAttribute("Y", Location.Y));
            if (Instance is IFunction instance)
                instance.Save(xtem);
        }

        public void Load(XElement item, Type type)
        {
            if (!int.TryParse(item.Attribute("X")?.Value, out int x)) return;
            if (!int.TryParse(item.Attribute("Y")?.Value, out int y)) return;
            Instance = Activator.CreateInstance(type);
            Location = new Point(x, y);
            if (Instance is IFunction inst) 
            {
                inst.Load(item.Element("Instance"));
            }
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
            using var brush = new SolidBrush(Color.FromArgb(255, backColor));
            using var pen = new Pen(foreColor, 1f);
            using var font = new Font("Consolas", 8f);
            using var fontbrush = new SolidBrush(foreColor);
            if (Instance is IFunction instance)
            {
                var max = Math.Max(instance.InverseInputs.Length, instance.InverseOutputs.Length);
                var step = 6f;
                var height = step + max * step * 4 + step;
                var width = step + 1 * step * 4 + step;
                var rect = new RectangleF(location, Size);
                if (Selected)
                {
                    for (var i = 5; i >= 3; i -= 2)
                    {
                        using var selpen = new Pen(Color.FromArgb(110, Color.Yellow), i);
                        graphics.DrawRectangles(selpen, [rect]);
                    }
                }
                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangles(pen, [rect]);
                // обозначение функции, текст по-центру, в верхней части рамки элемента
                using var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                var msn = graphics.MeasureString(instance.Name, font);
                graphics.DrawString(instance.Name, font, fontbrush, new PointF(location.X + width / 2, location.Y - msn.Height), format);
                graphics.DrawString(instance.FuncSymbol, font, fontbrush, new PointF(location.X + width / 2, location.Y), format);
                customDraw?.Invoke(graphics, rect, pen, brush, font, fontbrush);
                // входы
                var y = step + location.Y;
                var x = -step + location.X;
                for (var i = 0; i < instance.InverseInputs.Length; i++)
                {
                    y += step * 2;
                    // горизонтальная риска слева, напротив входа
                    graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                    if (instance.InverseInputs[i])
                    {
                        var r = new RectangleF(x + step / 2, y - step / 2, step, step);
                        // рисуем кружок инверсии
                        graphics.FillEllipse(brush, r);
                        graphics.DrawEllipse(pen, r);
                    }
                    // наименование входа
                    if (!string.IsNullOrEmpty(instance.InputNames[i]))
                    {
                        var ms = graphics.MeasureString(instance.InputNames[i], font);
                        graphics.DrawString(instance.InputNames[i], font, fontbrush, new PointF(x + step, y - ms.Height / 2));
                    }
                    // значение входа - отображаются только не связанные (свободные) входы
                    if (instance.VisibleValues && !instance.LinkedInputs[i])
                    {
                        var value = instance.InputValues[i];
                        var text = value != null && value.GetType() == typeof(bool) ? (bool)value ? "T" : "F" : $"{value}";
                        var ms = graphics.MeasureString(text, font);
                        using var iformat = new StringFormat();
                        iformat.Alignment = StringAlignment.Near;
                        graphics.DrawString(text, font, fontbrush, new PointF(x - ms.Width + step, y - ms.Height), iformat);
                    }
                    y += step * 2;
                }
                // выходы
                y = step + location.Y;
                x = width + location.X;
                for (var i = 0; i < instance.InverseOutputs.Length; i++)
                {
                    if (instance.InverseOutputs.Length == 1)
                        y = height / 2 + location.Y;
                    else
                        y += step * 2;
                    // горизонтальная риска справа, напротив выхода
                    graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                    if (instance.InverseOutputs[i])
                    {
                        var r = new RectangleF(x - step / 2, y - step / 2, step, step);
                        // рисуем кружок инверсии
                        graphics.FillEllipse(brush, r);
                        graphics.DrawEllipse(pen, r);
                    }
                    // наименование выхода
                    if (!string.IsNullOrEmpty(instance.OutputNames[i]))
                    {
                        var ms = graphics.MeasureString(instance.OutputNames[i], font);
                        graphics.DrawString(instance.OutputNames[i], font, fontbrush, new PointF(x - ms.Width, y - ms.Height / 2));
                    }
                    // значение выхода
                    if (instance.VisibleValues)
                    {
                        var value = instance.OutputValues[i];
                        var text = value != null && value.GetType() == typeof(bool) ? (bool)value ? "T" : "F" : $"{value}";
                        var ms = graphics.MeasureString(text, font);
                        graphics.DrawString(text, font, fontbrush, new PointF(x, y - ms.Height));
                    }
                    y += step * 2;
                }
                // области выбора
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
                // точки привязки входов и выходов
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
            }
        }

    }
}
