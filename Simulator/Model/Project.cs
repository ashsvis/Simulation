using System.Reflection;
using System.Xml.Linq;

namespace Simulator.Model
{
    public class Project
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Unit> Units { get; set; } = [];

        public Project() { }

        public void Save(string filename)
        {
            var root = new XElement("Project");
            XDocument doc = new(new XComment(Description), root);
            XElement xunits = new("Units");
            root.Add(xunits);
            foreach (var unit in Units)
            {
                XElement xunit = new("Unit");
                xunits.Add(xunit);
                unit.Save(xunit);
            }
            doc.Save(filename);
        }

        public void Load(string filename) 
        {
            var xdoc = XDocument.Load(filename);
            try
            {
                var xproject = xdoc.Element("Project");
                if (xproject != null)
                {
                    var xunits = xproject.Element("Units");
                    if (xunits != null)
                    {
                        foreach (XElement xunit in xunits.Elements("Unit"))
                        {
                            var unit = new Unit();
                            unit.Load(xunit);
                            Units.Add(unit);

                        }
                    }
                }
            }
            catch { }
        }
    }
}
