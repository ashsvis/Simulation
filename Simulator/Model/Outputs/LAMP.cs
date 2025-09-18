using Simulator.Model.Logic;

namespace Simulator.Model.Outputs
{
    public class LAMP : CommonLogic, ICustomDraw
    {
        public LAMP() : base(LogicFunction.Lamp, 1, 0) 
        {
        }

        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index)
        {
            if (Out)
            {
                using var fill = new SolidBrush(Color.Yellow);
                graphics.FillEllipse(fill, rect);
            }
            var rleft = new RectangleF(rect.X, rect.Y, rect.Height, rect.Height);
            graphics.DrawArc(pen, rleft, 90f, 180f);
            var rright = new RectangleF(rect.X + rect.Width - rect.Height, rect.Y, rect.Height, rect.Height);
            graphics.DrawArc(pen, rright, -90f, 180f);


            //var cen = new RectangleF(rect.X + rect.Height / 2f, rect.Y, rect.Width - rect.Height, rect.Height);
            //graphics.DrawLines(pen, [cen.Location, new PointF(cen.Right, cen.Top)]);
            //graphics.DrawLines(pen, [new PointF(cen.Left, cen.Bottom), new PointF(cen.Right, cen.Bottom)]);
            //using var sf = new StringFormat();
            //sf.Alignment = StringAlignment.Center;
            //sf.LineAlignment = StringAlignment.Center;

            //graphics.DrawString("Начало", font, fontbrush, rect, sf);
        }
    }
}
