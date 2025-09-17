using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;

namespace Simulator.Model
{
    public class Module
    {
        private string name = string.Empty;
        private string description = string.Empty;

        [Browsable(false)]
        public int Index { get; set; }

        [Category("Задача"), DisplayName("Имя")]
        public string Name 
        { 
            get => name;
            set 
            {
                if (name == value) return;
                name = value;
                Changed = true;
            }
        }

        [Category("Задача"), DisplayName("Описание")]
        public string Description 
        { 
            get => description;
            set
            {
                if (description == value) return;
                description = value;
                Changed = true;
            }
        }

        [Browsable(false)]
        public List<Element> Items { get; set; } = [];

        [Browsable(false)]
        public List<Link> Links { get; set; } = [];

        [Browsable(false)]
        public bool Changed { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? $"Задача {Index}" : Name;
        }

        public void Save(XElement xmodule)
        {
            if (!string.IsNullOrWhiteSpace(Name))
                xmodule.Add(new XAttribute("Name", Name));
            if (!string.IsNullOrWhiteSpace(Description))
                xmodule.Add(new XAttribute("Description", Description));
            XElement xitems = new("Items");
            xmodule.Add(xitems);
            foreach (var item in Items)
            {
                XElement xitem = new("Item");
                xitems.Add(xitem);
                item.Save(xitem);
            }
            XElement xlinks = new("Links");
            xmodule.Add(xlinks);
            foreach (var link in Links)
            {
                XElement xlink = new("Link");
                xlinks.Add(xlink);
                link.Save(xlink);
            }
        }

        public void Load(XElement xmodule)
        {
            if (xmodule != null)
            {
                var name = xmodule?.Attribute("Name")?.Value;
                if (name != null)
                    Name = name;
                var description = xmodule?.Attribute("Description")?.Value;
                if (description != null)
                    Description = description;
                var xitems = xmodule?.Element("Items");
                if (xitems != null)
                {
                    foreach (XElement xitem in xitems.Elements("Item"))
                    {
                        if (!Guid.TryParse(xitem.Element("Id")?.Value, out Guid id)) continue;
                        var xtype = xitem.Element("Type");
                        if (xtype == null) continue;
                        Type? type = Type.GetType(xtype.Value);
                        if (type == null) continue;
                        var element = new Element { Id = id, };
                        element.Load(xitem, type);
                        Items.Add(element);
                    }
                }
                var xlinks = xmodule?.Element("Links");
                if (xlinks != null)
                {
                    foreach (XElement xlink in xlinks.Elements("Link"))
                    {
                        if (!Guid.TryParse(xlink.Element("Id")?.Value, out Guid id)) continue;
                        var xsource = xlink.Element("Source");
                        if (xsource == null) continue;
                        if (!Guid.TryParse(xsource.Attribute("Id")?.Value, out Guid sourceId)) continue;
                        if (!int.TryParse(xsource.Attribute("PinIndex")?.Value, out int sourcePinIndex)) continue;
                        var xdest = xlink.Element("Destination");
                        if (xdest == null) continue;
                        if (!Guid.TryParse(xdest.Attribute("Id")?.Value, out Guid destinationId)) continue;
                        if (!int.TryParse(xdest.Attribute("PinIndex")?.Value, out int destPinIndex)) continue;
                        List<PointF> points = [];
                        var xpoints = xlink.Element("Points");
                        if (xpoints != null)
                        {
                            var fp = CultureInfo.GetCultureInfo("en-US");
                            foreach (XElement xpoint in xpoints.Elements("Point"))
                            {
                                if (float.TryParse(xpoint.Attribute("X")?.Value, fp, out float x) &&
                                    float.TryParse(xpoint.Attribute("Y")?.Value, fp, out float y))
                                {
                                    points.Add(new PointF(x, y));
                                }
                            }
                        }
                        if (points.Count > 1)
                        {
                            var link = new Link(id, sourceId, sourcePinIndex, destinationId, destPinIndex, [..points]);
                            link.Load(xlink);
                            Links.Add(link);
                        }
                    }
                }
                // установление связей
                foreach (var item in Items)
                {
                    if (item.Instance is ILinkSupport function)
                    {
                        var n = 0;
                        foreach (var (id, output) in function.InputLinkSources)
                        {
                            if (id != Guid.Empty)
                            {
                                var sourceItem = Items.FirstOrDefault(x => x.Id == id);
                                if (sourceItem != null && sourceItem.Instance is ILinkSupport source)
                                    function.SetValueLinkToInp(n, source.GetResultLink(output), id, output);
                            }
                            n++;
                        }
                    }
                }
            }
        }
    }
}
