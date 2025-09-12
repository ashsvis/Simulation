using System.Xml.Linq;

namespace Simulator.Model
{
    public static class Project
    {
        public static string Name { get; set; } = string.Empty;
        public static string Description { get; set; } = string.Empty;
        public static List<Module> Modules { get; set; } = [];

        private static string file = string.Empty;
        public static string FileName => file;

        public static void Save()
        {
            if (file == string.Empty) return;
            Save(file);
        }

        public static void Save(string filename)
        {
            file = filename;
            var root = new XElement("Project");
            XDocument doc = new(new XComment(Description), root);
            XElement xmodules = new("Modules");
            root.Add(xmodules);
            foreach (var module in Modules)
            {
                XElement xmodule = new("Module");
                xmodules.Add(xmodule);
                module.Save(xmodule);
            }
            try
            {
                doc.Save(filename);
                Modules.ForEach(module => module.Changed = false);
            }
            catch { }
        }

        public static void Load(string filename) 
        {
            file = filename;
            Modules.Clear();
            var xdoc = XDocument.Load(filename);
            try
            {
                var xproject = xdoc.Element("Project");
                if (xproject != null)
                {
                    var xmodules = xproject.Element("Modules");
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
            catch { }
        }

        public static TreeNode[] GetModulesTree()
        {
            List<TreeNode> collection = [];
            var rootNode = new TreeNode("Проект");
            collection.Add(rootNode);
            int nmodule = 1;
            foreach(var module in Modules)
            {
                module.Index = nmodule++;
                var moduleNode = new TreeNode(module.ToString()) { Tag = module };
                rootNode.Nodes.Add(moduleNode);
            }
            rootNode.ExpandAll();
            return [.. collection];
        }

        public static TreeNode[] GetLibraryTree()
        {
            List<TreeNode> collection = [];
            var rootNode = new TreeNode("Библиотека");
            collection.Add(rootNode);
            var logicaNode = new TreeNode("Логика");
            logicaNode.Nodes.Add(new TreeNode("NOT") { Tag = typeof(Model.Logic.NOT) });
            var andNode = new TreeNode("AND") { Tag = typeof(Model.Logic.AND) };
            logicaNode.Nodes.Add(andNode);
            andNode.Nodes.Add(new TreeNode("AND3") { Tag = typeof(Model.Logic.AND3) });
            andNode.Nodes.Add(new TreeNode("AND4") { Tag = typeof(Model.Logic.AND4) });
            andNode.Nodes.Add(new TreeNode("AND5") { Tag = typeof(Model.Logic.AND5) });
            andNode.Nodes.Add(new TreeNode("AND6") { Tag = typeof(Model.Logic.AND6) });
            andNode.Nodes.Add(new TreeNode("AND7") { Tag = typeof(Model.Logic.AND7) });
            andNode.Nodes.Add(new TreeNode("AND8") { Tag = typeof(Model.Logic.AND8) });
            var orNode = new TreeNode("OR") { Tag = typeof(Model.Logic.OR) };
            logicaNode.Nodes.Add(orNode);
            orNode.Nodes.Add(new TreeNode("OR3") { Tag = typeof(Model.Logic.OR3) });
            orNode.Nodes.Add(new TreeNode("OR4") { Tag = typeof(Model.Logic.OR4) });
            orNode.Nodes.Add(new TreeNode("OR5") { Tag = typeof(Model.Logic.OR5) });
            orNode.Nodes.Add(new TreeNode("OR6") { Tag = typeof(Model.Logic.OR6) });
            orNode.Nodes.Add(new TreeNode("OR7") { Tag = typeof(Model.Logic.OR7) });
            orNode.Nodes.Add(new TreeNode("OR8") { Tag = typeof(Model.Logic.OR8) });
            var xorNode = new TreeNode("XOR") { Tag = typeof(Model.Logic.XOR) };
            logicaNode.Nodes.Add(xorNode);
            xorNode.Nodes.Add(new TreeNode("XOR3") { Tag = typeof(Model.Logic.XOR3) });
            xorNode.Nodes.Add(new TreeNode("XOR4") { Tag = typeof(Model.Logic.XOR4) });
            xorNode.Nodes.Add(new TreeNode("XOR5") { Tag = typeof(Model.Logic.XOR5) });
            xorNode.Nodes.Add(new TreeNode("XOR6") { Tag = typeof(Model.Logic.XOR6) });
            xorNode.Nodes.Add(new TreeNode("XOR7") { Tag = typeof(Model.Logic.XOR7) });
            xorNode.Nodes.Add(new TreeNode("XOR8") { Tag = typeof(Model.Logic.XOR8) });
            rootNode.Nodes.Add(logicaNode);
            var triggerNode = new TreeNode("Триггеры");
            rootNode.Nodes.Add(triggerNode);
            triggerNode.Nodes.Add(new TreeNode("RS-триггер") { Tag = typeof(Model.Trigger.RS) });
            var generatorNode = new TreeNode("Генераторы");
            rootNode.Nodes.Add(generatorNode);
            generatorNode.Nodes.Add(new TreeNode("Одновибратор") { Tag = typeof(Model.Generator.PULSE) });
            generatorNode.Nodes.Add(new TreeNode("Задержка фронта") { Tag = typeof(Model.Generator.ONDLY) });
            generatorNode.Nodes.Add(new TreeNode("Задержка спада") { Tag = typeof(Model.Generator.OFFDLY) });
            rootNode.ExpandAll();
            andNode.Collapse();
            orNode.Collapse();
            xorNode.Collapse();
            return [.. collection];
        }

        public static void Clear()
        {
            file = string.Empty;
            Modules.Clear();
        }
    }
}
