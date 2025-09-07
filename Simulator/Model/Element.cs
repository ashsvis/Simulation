using System.Drawing;

namespace Simulator.Model
{
    public class Element
    {
        public Element() { }
        public Type? Type { get; set; }
        public object? Instance { get; set; }
        public PointF Location { get; set; }
        public SizeF Size { get; set; }

        public RectangleF Bounds => new(Location, Size);

        private readonly Dictionary<int, RectangleF> targets = [];
        public Dictionary<int, RectangleF> Targets => targets;

        private readonly Dictionary<int, PointF> pins = [];
        public Dictionary<int, PointF> Pins => pins;

        public bool TryGetOutput(PointF point, out int? output, out PointF? pin)
        {
            output = null;
            pin = null;
            foreach (var key in targets.Keys.Where(x => x >= 200 && x < 300))
            {
                var target = targets[key];
                if (target.Contains(point))
                {
                    output = key - 200;
                    pin = pins.TryGetValue(key, out PointF value) ? value : null;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetInput(PointF point, out int? input, out PointF? pin)
        {
            input = null;
            pin = null;
            foreach (var key in targets.Keys.Where(x => x >= 100 && x < 200))
            {
                var target = targets[key];
                if (target.Contains(point))
                {
                    input = key - 100;
                    pin = pins.TryGetValue(key, out PointF value) ? value : null;
                    return true;
                }
            }
            return false;
        }

        private void CalculateTargets(Graphics graphics)
        {
            targets.Clear();
            pins.Clear();
            using var font = new Font("Consolas", 8f);
            if (Instance is IFunction instance)
            {
                var max = Math.Max(instance.InverseInputs.Length, instance.InverseOutputs.Length);
                var step = 6f;
                var height = step + max * step * 4 + step;
                var width = step + 1 * step * 4 + step;
                Size = new SizeF(width, height);
                // входы
                var y = step + Location.Y;
                var x = -step + Location.X;
                var n = 100;
                for (var i = 0; i < instance.InverseInputs.Length; i++)
                {
                    y += step * 2;
                    // значение входа
                    var ms = graphics.MeasureString("W", font);
                    targets.Add(n, new RectangleF(new PointF(x - ms.Width + step, y - ms.Height), ms));
                    pins.Add(n, new PointF(x, y));
                    y += step * 2;
                    n++;
                }
                // выходы
                y = step + Location.Y;
                x = width + Location.X;
                n = 200;
                for (var i = 0; i < instance.InverseOutputs.Length; i++)
                {
                    if (instance.InverseOutputs.Length == 1)
                        y = height / 2 + Location.Y;
                    else
                        y += step * 2;
                    // значение выхода
                    var ms = graphics.MeasureString("W", font);
                    targets.Add(n, new RectangleF(new PointF(x, y - ms.Height), ms));
                    pins.Add(n, new PointF(x + step, y));
                    y += step * 2;
                    n++;
                }
            }

        }

        public void Draw(Graphics graphics, Color foreColor, Color backColor)
        {
            CalculateTargets(graphics);
            using var brush = new SolidBrush(backColor);
            using var pen = new Pen(foreColor, 1f);
            using var font = new Font("Consolas", 8f);
            using var fontbrush = new SolidBrush(foreColor);
            if (Instance is IFunction instance)
            {
                var max = Math.Max(instance.InverseInputs.Length, instance.InverseOutputs.Length);
                var step = 6f;
                var height = step + max * step * 4 + step;
                var width = step + 1 * step * 4 + step;
                var rect = new RectangleF(Location, Size);
                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangles(pen, [rect]);
                // входы
                var y = step + Location.Y;
                var x = -step + Location.X;
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
                y = step + Location.Y;
                x = width + Location.X;
                for (var i = 0; i < instance.InverseOutputs.Length; i++)
                {
                    if (instance.InverseOutputs.Length == 1)
                        y = height / 2 + Location.Y;
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
                // обозначение функции, текст по-центру, в верхней части рамки элемента
                using var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(instance.FuncSymbol, font, fontbrush, new PointF(Location.X + width / 2, Location.Y), format);
                /*
                // области выбора
                using Pen tarpen = new(Color.FromArgb(80, Color.Magenta), 0);
                foreach (var key in targets.Keys)
                {
                    var target = targets[key];
                    graphics.DrawRectangles(tarpen, [target]);
                }
                // точки привязки входов и выходов
                using Pen pinpen = new(Color.FromArgb(255, Color.Black), 0);
                foreach (var key in pins.Keys)
                {
                    var pt = pins[key];
                    var r = new RectangleF(pt.X - 3, pt.Y - 3, 6, 6);
                    graphics.DrawLine(pinpen, new PointF(r.X, r.Y), new PointF(r.X + r.Width, r.Y + r.Height));
                    graphics.DrawLine(pinpen, new PointF(r.X + r.Width, r.Y), new PointF(r.X, r.Y + r.Height));
                }
                */
            }
        }
    }
}
