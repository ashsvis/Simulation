namespace Simulator.Model
{
    public class Element
    {
        public Element() { }
        public Type? Type { get; set; }
        public object? Instance { get; set; }
        public PointF Location { get; set; }

        public void Draw(Graphics graphics, Color foreColor, Color backColor)
        {
            using var brush = new SolidBrush(backColor);
            using var pen = new Pen(foreColor, 0);
            using var font = new Font("Consolas", 8f);
            using var fontbrush = new SolidBrush(foreColor);
            if (Instance is ICalculate instance)
            {
                var max = Math.Max(instance.InverseInputs.Length, instance.InverseOutputs.Length);
                var step = 6f;
                var height = step + max * step * 4 + step;
                var width = step + 1 * step * 4 + step;
                var rect = new RectangleF(Location, new SizeF(width, height));
                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangles(pen, [rect]);
                // входы
                var y = step + Location.Y;
                var x = -step + Location.X;
                for (var i = 0; i < instance.InverseInputs.Length; i++)
                {
                    y += step * 2;
                    graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                    if (instance.InverseInputs[i])
                    {
                        var r = new RectangleF(x + step / 2, y - step / 2, step, step);
                        graphics.FillEllipse(brush, r);
                        graphics.DrawEllipse(pen, r);
                    }
                    // наименование входа
                    if (!string.IsNullOrEmpty(instance.InputNames[i]))
                    {
                        var ms = graphics.MeasureString(instance.InputNames[i], font);
                        graphics.DrawString(instance.InputNames[i], font, fontbrush, new PointF(x + step, y - ms.Height / 2));
                    }
                    y += step * 2;
                }
                // выходы
                y = step + Location.Y;
                x = width + Location.X;
                for (var i = 0; i < instance.InverseOutputs.Length; i++)
                {
                    y += step * 2;
                    graphics.DrawLine(pen, new PointF(x, y), new PointF(x + step, y));
                    if (instance.InverseOutputs[i])
                    {
                        var r = new RectangleF(x - step / 2, y - step / 2, step, step);
                        graphics.FillEllipse(brush, r);
                        graphics.DrawEllipse(pen, r);
                    }
                    // наименование выхода
                    if (!string.IsNullOrEmpty(instance.OutputNames[i]))
                    {
                        var ms = graphics.MeasureString(instance.OutputNames[i], font);
                        graphics.DrawString(instance.OutputNames[i], font, fontbrush, new PointF(x - ms.Width, y - ms.Height / 2));
                    }
                    y += step * 2;
                }
                // функция
                using var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(instance.FuncSymbol, font, fontbrush, new PointF(Location.X + width / 2, Location.Y), format);
            }
        }
    }
}
