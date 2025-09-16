using System.ComponentModel;
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
                var xmodules = xmodule?.Element("Items");
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
}
