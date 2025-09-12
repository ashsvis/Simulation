using System.Xml.Linq;

namespace Simulator.Model
{
    public class Module
    {
        public int Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Element> Items { get; set; } = [];

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? $"Модуль {Index}" : Name;
        }

        public void Save(XElement xmodule)
        {
            XElement xitems = new("Items");
            xmodule.Add(xitems);
            foreach (var item in Items)
            {
                XElement xitem = new("Item");
                xitems.Add(xitem);
                item.Save(xitem);
            }
        }

        public void Load(XElement xmodule)
        {
            if (xmodule != null)
            {
                var xmodules = xmodule.Element("Items");
                if (xmodules != null)
                {
                    foreach (XElement xitem in xmodules.Elements("Item"))
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
                    // установление связей
                    foreach (var item in Items)
                    {
                        if (item.Instance is IFunction function)
                        {
                            var n = 0;
                            foreach (var (id, output) in function.InputLinkSources)
                            {
                                if (id != Guid.Empty)
                                {
                                    var sourceItem = Items.FirstOrDefault(x => x.Id == id);
                                    if (sourceItem != null && sourceItem.Instance is IFunction source)
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
}
