using System.Reflection;
using System.Xml.Linq;

namespace Simulator.Model
{
    public class Unit
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Module> Modules { get; set; } = [];

        public void Save(XElement xunit)
        {
            XElement xmodules = new("Modules");
            xunit.Add(xmodules);
            foreach (var module in Modules)
            {
                XElement xmodule = new("Module");
                xmodules.Add(xmodule);
                module.Save(xmodule);
            }
        }

        public void Load(XElement xunit)
        {
            if (xunit != null)
            {
                var xmodules = xunit.Element("Modules");
                if (xmodules != null)
                {
                    foreach (XElement xmodule in xmodules.Elements("Module"))
                    {
                        var module = new Module();
                        module.Load(xmodule);
                        Modules.Add(module);

                    }
                }
            }
        }
    }
}
