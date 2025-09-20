using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model
{
    public static class Project
    {
        public static string Name { get; set; } = string.Empty;
        public static string Description { get; set; } = string.Empty;

        [Browsable(false)]
        public static List<Module> Modules { get; set; } = [];

        private static string file = string.Empty;

        [Browsable(false)]
        public static string FileName => file;

        [Browsable(false)]
        public static bool Changed { get; set; }

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
            if (!string.IsNullOrWhiteSpace(Name))
                root.Add(new XAttribute("Name", Name));
            if (!string.IsNullOrWhiteSpace(Description))
                root.Add(new XAttribute("Description", Description));
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
                Changed = false;
            }
            catch { }
        }

        public static void Load(string filename) 
        {
            Name = string.Empty;
            Description = string.Empty;
            Modules.Clear();
            file = filename;
            var xdoc = XDocument.Load(filename);
            try
            {
                var xproject = xdoc.Element("Project");
                if (xproject != null)
                {
                    var name = xproject?.Attribute("Name")?.Value;
                    if (name != null)
                        Name = name;
                    var description = xproject?.Attribute("Description")?.Value;
                    if (description != null)
                        Description = description;
                    var xmodules = xproject?.Element("Modules");
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
                Modules.ForEach(module => module.Changed = false);
                Changed = false;
                OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.Load));
            }
            catch { }
        }

        public static TreeNode[] GetModulesTree()
        {
            List<TreeNode> collection = [];
            var rootNode = new TreeNode("Проект") { Tag = new ProjectProxy() };
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
            var inputNode = new TreeNode("Входные сигналы");
            rootNode.Nodes.Add(inputNode);
            inputNode.Nodes.Add(new TreeNode("Дискретный вход") { Tag = typeof(Inputs.DI) });
            var logicaNode = new TreeNode("Логика");
            logicaNode.Nodes.Add(new TreeNode("Инвертор") { Tag = typeof(Logic.NOT) });
            var orNode = new TreeNode("Дизъюнкция (\"ИЛИ\")");
            logicaNode.Nodes.Add(orNode);
            orNode.Nodes.Add(new TreeNode("2x") { Tag = typeof(Logic.OR) });
            orNode.Nodes.Add(new TreeNode("3x") { Tag = typeof(Logic.OR3) });
            orNode.Nodes.Add(new TreeNode("4x") { Tag = typeof(Logic.OR4) });
            orNode.Nodes.Add(new TreeNode("5x") { Tag = typeof(Logic.OR5) });
            orNode.Nodes.Add(new TreeNode("6x") { Tag = typeof(Logic.OR6) });
            orNode.Nodes.Add(new TreeNode("7x") { Tag = typeof(Logic.OR7) });
            orNode.Nodes.Add(new TreeNode("8x") { Tag = typeof(Logic.OR8) });
            var andNode = new TreeNode("Конъюнкция (\"И\")");
            logicaNode.Nodes.Add(andNode);
            andNode.Nodes.Add(new TreeNode("2x") { Tag = typeof(Logic.AND) });
            andNode.Nodes.Add(new TreeNode("3x") { Tag = typeof(Logic.AND3) });
            andNode.Nodes.Add(new TreeNode("4x") { Tag = typeof(Logic.AND4) });
            andNode.Nodes.Add(new TreeNode("5x") { Tag = typeof(Logic.AND5) });
            andNode.Nodes.Add(new TreeNode("6x") { Tag = typeof(Logic.AND6) });
            andNode.Nodes.Add(new TreeNode("7x") { Tag = typeof(Logic.AND7) });
            andNode.Nodes.Add(new TreeNode("8x") { Tag = typeof(Logic.AND8) });
            var xorNode = new TreeNode("Исключающее \"ИЛИ\"");
            logicaNode.Nodes.Add(xorNode);
            xorNode.Nodes.Add(new TreeNode("2x") { Tag = typeof(Logic.XOR) });
            xorNode.Nodes.Add(new TreeNode("3x") { Tag = typeof(Logic.XOR3) });
            xorNode.Nodes.Add(new TreeNode("4x") { Tag = typeof(Logic.XOR4) });
            xorNode.Nodes.Add(new TreeNode("5x") { Tag = typeof(Logic.XOR5) });
            xorNode.Nodes.Add(new TreeNode("6x") { Tag = typeof(Logic.XOR6) });
            xorNode.Nodes.Add(new TreeNode("7x") { Tag = typeof(Logic.XOR7) });
            xorNode.Nodes.Add(new TreeNode("8x") { Tag = typeof(Logic.XOR8) });
            rootNode.Nodes.Add(logicaNode);
            var triggerNode = new TreeNode("Триггеры");
            rootNode.Nodes.Add(triggerNode);
            triggerNode.Nodes.Add(new TreeNode("RS-триггер") { Tag = typeof(Trigger.RS) });
            triggerNode.Nodes.Add(new TreeNode("SR-триггер") { Tag = typeof(Trigger.SR) });
            var frontEdgeNode = new TreeNode("Детектор фронта") { Tag = typeof(Logic.FE) };
            logicaNode.Nodes.Add(frontEdgeNode);
            var generatorNode = new TreeNode("Таймеры");
            rootNode.Nodes.Add(generatorNode);
            generatorNode.Nodes.Add(new TreeNode("Задержка включения") { Tag = typeof(Timer.ONDLY) });
            generatorNode.Nodes.Add(new TreeNode("Задержка выключения") { Tag = typeof(Timer.OFFDLY) });
            generatorNode.Nodes.Add(new TreeNode("Формирователь импульса") { Tag = typeof(Timer.PULSE) });
            var outputNode = new TreeNode("Выходные сигналы");
            rootNode.Nodes.Add(outputNode);
            outputNode.Nodes.Add(new TreeNode("Лампа") { Tag = typeof(Outputs.LAMP) });
            outputNode.Nodes.Add(new TreeNode("Дискретный выход") { Tag = typeof(Outputs.DO) });
            var diagramNode = new TreeNode("Диаграмма");
            rootNode.Nodes.Add(diagramNode);
            diagramNode.Nodes.Add(new TreeNode("Начало") { Tag = typeof(Diagram.START) });
            diagramNode.Nodes.Add(new TreeNode("Конец") { Tag = typeof(Diagram.FINISH) });
            var assemblyNode = new TreeNode("Сборки");
            rootNode.Nodes.Add(assemblyNode);
            assemblyNode.Nodes.Add(new TreeNode("Сборка 4х4") { Tag = typeof(Logic.ASMBLY) });

            rootNode.ExpandAll();
            andNode.Collapse();
            orNode.Collapse();
            xorNode.Collapse();
            return [.. collection];
        }

        public static void Clear()
        {
            Name = string.Empty;
            Description = string.Empty;
            file = string.Empty;
            Modules.Clear();
            Changed = false;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.Clear));
        }

        public static Module AddModuleToProject()
        {
            var module = new Module();
            Modules.Add(module);
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.AddModule));
            return module;
        }

        public static void RemoveModuleFromProject(Module module)
        {
            Modules.Remove(module);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.RemoveModule));
        }

        public static event ProjectEventHandler? OnChanged;
    }

    public enum ProjectChangeKind
    {
        Clear,
        Load,
        AddModule,
        RemoveModule,
    }

    public delegate void ProjectEventHandler(object? sender, ProjectEventArgs args);

    public class ProjectEventArgs : EventArgs
    {
        public ProjectChangeKind ChangeKind;

        public ProjectEventArgs(ProjectChangeKind changeKind)
        {
            ChangeKind = changeKind;
        }
    }
}
