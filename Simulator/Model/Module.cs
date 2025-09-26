using Simulator.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
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
        public List<Element> Elements { get; set; } = [];

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

        public static void ConnectLinks(List<Element> elements)
        {
            // установление связей
            foreach (var item in elements)
            {
                if (item.Instance is ILinkSupport function)
                {
                    var n = 0;
                    foreach (var (id, output) in function.InputLinkSources)
                    {
                        if (id != Guid.Empty)
                        {
                            var sourceItem = elements.FirstOrDefault(x => x.Id == id);
                            if (sourceItem != null && sourceItem.Instance is ILinkSupport source && source.OutputValues.Length > 0)
                                function.SetValueLinkToInp(n, source.GetResultLink(output), id, output);
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

        public static void LoadElements(XElement? xmodule, List<Element> elements)
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
                    var element = new Element { Id = id, };
                    element.Load(xitem, type);
                    elements.Add(element);
                }
            }
        }

        public Action GetCalculationMethod()
        {
            return new Action(() =>
            {
                Elements.ForEach(item =>
                {
                    try
                    {
                        if (item.Instance is ICalculate instance)
                            instance.Calculate();
                    }
                    catch
                    {

                    }
                });
                Elements.ForEach(item =>
                {
                    try
                    {
                        if (item.Instance is Model.Logic.FE frontEdgeDetector)
                            frontEdgeDetector.Reset();
                    }
                    catch
                    {

                    }
                });
            });
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

                //if (element.Instance is ILinkSupport link)
                //{
                //    foreach (var seek in link.InputLinkSources)
                //        link.UpdateInputLinkSources(seek, element.Id);
                //}

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
    }
}
