using Simulator.Model.Interfaces;
using System.Text;
using System.Xml.Linq;

namespace Simulator.Model
{
    public class Field : Module
    {
        public static Unit MakeDuplicate(Unit module)
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
            var dublicate = new Model.Unit();
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
