using Simulator.Model.Common;
using Simulator.Model.Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace Simulator.Model
{
    public class Module
    {
        private string name = string.Empty;
        private string description = string.Empty;

        [Browsable(false)]
        public int Index { get; set; }

        [Browsable(false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Category("Задача"), DisplayName("Имя")]
        public string Name 
        { 
            get => name;
            set 
            {
                if (name == value) return;
                name = value;
                Project.Changed = true;
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
                Project.Changed = true;
            }
        }

        [Browsable(false)]
        public List<Element> Elements { get; set; } = [];

        [Browsable(false)]
        public List<Link> Links { get; set; } = [];

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? $"(без имени {Index})" : Name;
        }

        public void Save(XElement xmodule)
        {
            xmodule.Add(new XElement("Id", Id));
            if (!string.IsNullOrWhiteSpace(Name))
                xmodule.Add(new XAttribute("Name", Name));
            if (!string.IsNullOrWhiteSpace(Description))
                xmodule.Add(new XAttribute("Description", Description));
            XElement xitems = new("Elements");
            xmodule.Add(xitems);
            foreach (var item in Elements)
            {
                XElement xitem = new("Element");
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
                vals.Clear();
                var name = xmodule?.Attribute("Name")?.Value;
                if (name != null)
                    Name = name;
                var description = xmodule?.Attribute("Description")?.Value;
                if (description != null)
                    Description = description;
                LoadElements(xmodule, Elements);
                ConnectLinks(Elements);
                LoadVisualLinks(xmodule, Links);
            }
        }

        public void ConnectLinks(List<Element> elements)
        {
            // установление связей
            foreach (var item in elements)
            {
                if (item.Instance is ILinkSupport func)
                {
                    var n = 0;
                    foreach (var (id, output, external) in func.InputLinkSources)
                    {
                        if (id != Guid.Empty)
                        {
                            var sourceItem = elements.FirstOrDefault(x => x.Id == id);
                            if (sourceItem != null && sourceItem.Instance is ILinkSupport source && source.OutputValues.Length > 0)
                                func.SetValueLinkToInp(n, id, output, false);
                        }
                        n++;
                    }
                }
            }
        }

        public static void LoadVisualLinks(XElement? xmodule, List<Link> links)
        {
            var xlinks = xmodule?.Element("Links");
            if (xlinks != null)
            {
                foreach (XElement xlink in xlinks.Elements("Link"))
                {
                    if (!Guid.TryParse(xlink.Attribute("Id")?.Value, out Guid id)) continue;
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
                        var xpa = xpoints.Attribute("Array")?.Value;
                        if (xpa != null)
                        {
                            foreach (string[] xps in xpa.Split(' ').Select(x => x.Split(',')))
                            {
                                if (xps.Length == 2 &&
                                    float.TryParse(xps[0], fp, out float x) &&
                                    float.TryParse(xps[1], fp, out float y))
                                {
                                    points.Add(new PointF(x, y));
                                }

                            }
                        }
                    }
                    if (points.Count > 1)
                    {
                        var link = new Link(id, sourceId, sourcePinIndex, destinationId, destPinIndex, [.. points]);
                        links.Add(link);
                    }
                }
            }
        }

        public void LoadElements(XElement? xmodule, List<Element> elements)
        {
            var xitems = xmodule?.Element("Elements");
            if (xitems != null)
            {
                foreach (XElement xitem in xitems.Elements("Element"))
                {
                    if (!Guid.TryParse(xitem.Attribute("Id")?.Value, out Guid id)) continue;
                    var xtance = xitem.Element("Instance");
                    var xtype = xtance?.Attribute("Type");
                    if (xtype == null) continue;
                    Type? type = Type.GetType(xtype.Value);
                    if (type == null) continue;
                    var element = new Element(Elements, Links) { Id = id, };
                    element.Load(xitem, type);
                    elements.Add(element);
                }
            }
        }

        public void Calculate()
        {
            try
            {
                Elements.ForEach(item =>
                {
                    try
                    {
                        if (item.Instance is ICalculate instance)
                            instance.Calculate();
                    }
                    catch (Exception ex) 
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
            finally
            {
                Elements.ForEach(item =>
                {
                    try
                    {
                        if (item.Instance is Model.Logic.FE frontEdgeDetector)
                            frontEdgeDetector.Reset();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        public Action GetCalculationMethod()
        {
            return new Action(Calculate);
        }

        public Module DeepCopy()
        {
            var root = new XElement("DeepCopy");
            XDocument docSource = new(new XComment("Полное копирование"), root);
            root.Add(new XElement("Id", Id));
            if (!string.IsNullOrWhiteSpace(Name))
                root.Add(new XAttribute("Name", Name));
            if (!string.IsNullOrWhiteSpace(Description))
                root.Add(new XAttribute("Description", Description));
            XElement xitems = new("Elements");
            root.Add(xitems);
            foreach (var item in Elements)
            {
                var holder = new XElement("Element");
                item.Save(holder);
                xitems.Add(holder);
            }
            XElement xlinks = new("Links");
            root.Add(xlinks);
            foreach (var item in Links)
            {
                var holder = new XElement("Link");
                item.Save(holder);
                xlinks.Add(holder);
            }
            string xml = root.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            using var xmlStream = new MemoryStream(bytes);

            var aCopy = new Module
            {
                Id = Id
            };

            List<Element> elements = [];
            List<Link> elementlinks = [];
            XDocument docCopy = XDocument.Load(xmlStream);
            var name = docCopy.Root?.Attribute("Name")?.Value;
            if (name != null)
                aCopy.Name = name;
            var description = docCopy.Root?.Attribute("Description")?.Value;
            if (description != null)
                aCopy.Description = description;
            LoadElements(docCopy.Root, elements);
            // установление связей
            ConnectLinks(elements);
            LoadVisualLinks(docCopy.Root, elementlinks);

            foreach (Element element in elements)
            {
                aCopy.Elements.Add(element);
            }
            foreach (Link link in elementlinks)
            {
                link.SetSourceId(link.SourceId);
                link.SetDestinationId(link.DestinationId);
                aCopy.Links.Add(link);
            }
            return aCopy;
        }

        public void Accept(Module module)
        {
            Id = module.Id;
            Name = module.Name;
            Description = module.Description;
            Elements.Clear();
            Elements.AddRange(module.Elements);
            Links.Clear();
            Links.AddRange(module.Links);
        }

        private readonly ConcurrentDictionary<string, ValueItem> vals = [];

        public void Clear()
        {
            vals.Clear();
        }

        public ValueItem[] GetElementVariablesByIndex(Guid elementId)
        {
            List<ValueItem> result = [];
            var inputkeys = vals.Where(x => x.Value.ElementId == elementId && x.Value.Side == ValueDirect.Input).OrderBy(x => x.Value.Pin).Select(x => x.Key).ToList();
            foreach (var key in inputkeys)
            {
                if (vals.TryGetValue(key, out ValueItem? a))
                    result.Add(a);
            }
            var outputkeys = vals.Where(x => x.Value.ElementId == elementId && x.Value.Side == ValueDirect.Output).OrderBy(x => x.Value.Pin).Select(x => x.Key).ToList();
            foreach (var key in outputkeys)
            {
                if (vals.TryGetValue(key, out ValueItem? a))
                    result.Add(a);
            }
            return [.. result];
        }

        public static Module MakeDuplicate(Module module)
        {
            var root = new XElement("Duplicate");
            //XDocument doc = new(new XComment("Дубликат"), root);
            XElement xitems = new("Elements");
            root.Add(xitems);
            foreach (var item in module.Elements)
            {
                var holder = new XElement("Element");
                item.Save(holder);
                xitems.Add(holder);
            }
            XElement xlinks = new("Links");
            root.Add(xlinks);
            foreach (var item in module.Links)
            {
                var holder = new XElement("Link");
                item.Save(holder);
                xlinks.Add(holder);
            }
            string xml = root.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            using var xmlStream = new MemoryStream(bytes);

            List<Element> elements = [];
            List<Link> elementlinks = [];
            Dictionary<Guid, Guid> guids = [];
            XDocument doc = XDocument.Load(xmlStream);
            var dublicate = new Model.Module();
            dublicate.LoadElements(doc.Root, elements);
            // замена Id на новый, сохранение уникальности Id для копии элемента
            // составление словаря замен
            foreach (Element element in elements)
            {
                var newId = Guid.NewGuid();
                guids.Add(element.Id, newId);
                element.Id = newId;
            }
            // замена SourceId для входный связей из словаря замен
            foreach (Element element in elements)
            {
                if (element.Instance is ILinkSupport link)
                {
                    foreach (var seek in link.InputLinkSources)
                        link.UpdateInputLinkSources(seek,
                            guids.TryGetValue(seek.Item1, out Guid value) ? value : Guid.Empty);
                }
            }
            // установление связей
            dublicate.ConnectLinks(elements);
            LoadVisualLinks(doc.Root, elementlinks);

            foreach (Element element in elements)
                dublicate.Elements.Add(element);
            foreach (Link link in elementlinks)
            {
                if (!guids.ContainsKey(link.SourceId) || !guids.ContainsKey(link.DestinationId)) continue;
                link.SetId(Guid.NewGuid());
                link.SetSourceId(guids[link.SourceId]);
                link.SetDestinationId(guids[link.DestinationId]);
                dublicate.Links.Add(link);
            }
            return dublicate;
        }
    }
}
