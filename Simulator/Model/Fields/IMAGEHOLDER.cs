using Simulator.Model.Interfaces;
using System.Xml.Linq;

namespace Simulator.Model.Fields
{
    public class IMAGEHOLDER : CommonFields, ICustomDraw
    {
        private float width = Element.Step * 100;
        private float height = Element.Step * 100;

        private bool busy;

        public float Width 
        { 
            get => width;
            set
            {
                if (width == value) return;
                if (width <= 0) return;
                width = value;
                if (busy) return;
                var element = Project.GetFieldElementById(ItemId);
                element?.CalculateTargets();
            }
        }

        public float Height 
        { 
            get => height;
            set
            {
                if (height == value) return;
                if (height <= 0) return;
                height = value;
                if (busy) return;
                var element = Project.GetFieldElementById(ItemId);
                element?.CalculateTargets();
            }
        }

        public override void CalculateTargets(PointF location, ref SizeF size,
            Dictionary<int, RectangleF> itargets, Dictionary<int, PointF> ipins, Dictionary<int, RectangleF> otargets, Dictionary<int, PointF> opins)
        {
            size = new SizeF(Width, Height);
            itargets.Clear();
            ipins.Clear();
            otargets.Clear();
            opins.Clear();
        }



        public void CustomDraw(Graphics graphics, RectangleF rect, Pen pen, Brush brush, Font font, Brush fontbrush, int index, bool selected)
        {
            graphics.DrawRectangles(pen, [rect]);
        }

        public override void Save(XElement xtance)
        {
            base.Save(xtance);
            XElement xsource = new("Place");
            xsource.Add(new XAttribute("Width", (int)Width));
            xsource.Add(new XAttribute("Height", (int)Height));
            xtance.Add(xsource);
        }

        public override void Load(XElement? xtance)
        {
            base.Load(xtance);
            var xsource = xtance?.Element("Place");
            if (xsource != null)
            {
                busy = true;
                if (int.TryParse(xsource.Attribute("Width")?.Value, out int width))
                    Width = width;
                if (int.TryParse(xsource.Attribute("Height")?.Value, out int height))
                    Height = height;
                busy = false;
            }
        }
    }
}
