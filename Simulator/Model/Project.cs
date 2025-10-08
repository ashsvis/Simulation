using Simulator.Model.Interfaces;
using System.Collections;
using System.ComponentModel;
using System.Xml.Linq;

namespace Simulator.Model
{
    public static class Project
    {
        private static readonly Hashtable vals = [];

        //internal static (string, string) GetAddressById(Guid id)
        //{
        //    var k = 1;
        //    foreach (var module in Modules)
        //    {
        //        var n = 1;
        //        foreach (var element in module.Elements)
        //        {
        //            if (id == element.Id)
        //            {
        //                var localName = $"L{n}";
        //                return (!string.IsNullOrEmpty(module.Name) ? module.Name : $"Task{k}", 
        //                        element.Instance is IFunction func ? func.Name ?? localName : localName);
        //            }
        //            n++;
        //        }
        //        k++;
        //    }
        //    return ("", "");
        //}

        internal static (string, string, string) GetInputByElementId(Guid id, int pin)
        {
            var k = 1;
            foreach (var module in Modules.Union(Equipment))
            {
                var moduleName = !string.IsNullOrEmpty(module.Name) ? module.Name : $"Task{k}";
                var n = 1;
                foreach (var element in module.Elements)
                {
                    if (id == element.Id)
                    {
                        var elementName = $"L{n}";
                        var inputName = pin.ToString();
                        if (element.Instance is IFunction func)
                        {
                            if (!string.IsNullOrEmpty(func.Name))
                                elementName = func.Name;
                            if (pin < func.InputNames.Length)
                                inputName = !string.IsNullOrEmpty(func.InputNames[pin]) 
                                    ? func.InputNames[pin] 
                                    : func.InputNames.Length > 1 ? $"In{pin + 1}" : "Inp";
                        }
                        return (moduleName, elementName, inputName);
                    }
                    n++;
                }
                k++;
            }
            return ("", "", "");
        }

        internal static (string, string, string) GetOutputByElementId(Guid id, int pin)
        {
            var k = 1;
            foreach (var module in Modules.Union(Equipment))
            {
                var moduleName = !string.IsNullOrEmpty(module.Name) ? module.Name : $"Task{k}";
                var n = 1;
                foreach (var element in module.Elements)
                {
                    if (id == element.Id)
                    {
                        var elementName = $"L{n}";
                        var inputName = pin.ToString();
                        if (element.Instance is IFunction func)
                        {
                            if (!string.IsNullOrEmpty(func.Name))
                                elementName = func.Name;
                            if (pin < func.OutputNames.Length)
                                inputName = !string.IsNullOrEmpty(func.OutputNames[pin]) 
                                    ? func.OutputNames[pin] 
                                    : func.OutputNames.Length > 1 ? $"Out{pin + 1}" : "Out";
                        }
                        return (moduleName, elementName, inputName);
                    }
                    n++;
                }
                k++;
            }
            return ("", "", "");
        }

        //internal static string GetModuleNameById(Guid id)
        //{
        //    foreach (var module in Modules)
        //    {
        //        var n = 1;
        //        foreach (var element in module.Elements)
        //        {
        //            if (id == element.Id)
        //                return !string.IsNullOrEmpty(module.Name) ? module.Name : $"Task{n}";
        //            n++;
        //        }
        //    }
        //    return string.Empty;
        //}

        //internal static Guid GetModuleIdById(Guid id)
        //{
        //    foreach (var module in Modules)
        //    {
        //        var n = 1;
        //        foreach (var element in module.Elements)
        //        {
        //            if (id == element.Id)
        //                return module.Id;
        //            n++;
        //        }
        //    }
        //    return Guid.Empty;
        //}

        internal static string GetElementById(Guid id)
        {
            foreach (var module in Modules.Union(Equipment))
            {
                var n = 1;
                foreach (var element in module.Elements)
                {
                    if (id == element.Id)
                    {
                        var localName = $"L{n}";
                        return element.Instance is IFunction func ? func.Name ?? localName : localName;
                    }
                    n++;
                }
            }
            return string.Empty;
        }

        internal static Element? GetFieldElementById(Guid id)
        {
            foreach (var field in Fields)
                foreach (var element in field.Elements)
                    if (id == element.Id)
                        return element;
            return null;
        }

        internal static void WriteValue(Guid elementId, int pin, ValueSide side, ValueKind kind, object? value)
        {
            var key = $"{elementId}\t{pin}\t{side}\t{kind}";
            vals[key] = new ValueItem() { ElementId = elementId, Side = side, Pin = pin, Kind = kind, Value = value };
        }

        internal static ValueItem? ReadValue(Guid elementId, int pin, ValueSide side, ValueKind kind)
        {
            var key = $"{elementId}\t{pin}\t{side}\t{kind}";
            if (vals.ContainsKey(key))
                return (ValueItem?)vals[key];
            return null;
        }

        public static string Name { get; set; } = string.Empty;
        public static string Description { get; set; } = string.Empty;

        [Browsable(false)]
        public static List<Module> Modules { get; set; } = [];

        [Browsable(false)]
        public static List<Unit> Equipment { get; set; } = [];

        [Browsable(false)]
        public static List<Field> Fields { get; set; } = [];

        [Browsable(false)]
        public static List<Module> Blocks { get; set; } = [];

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
            XElement xblocks = new("Blocks");
            root.Add(xblocks);
            foreach (var module in Blocks)
            {
                XElement xblock = new("Block");
                xblocks.Add(xblock);
                module.Save(xblock);
            }
            XElement xmodules = new("Modules");
            root.Add(xmodules);
            foreach (var module in Modules)
            {
                XElement xmodule = new("Module");
                xmodules.Add(xmodule);
                module.Save(xmodule);
            }
            XElement xquipment = new("Equipment");
            root.Add(xquipment);
            foreach (var unit in Equipment)
            {
                XElement xunit = new("Unit");
                xquipment.Add(xunit);
                unit.Save(xunit);
            }
            XElement xfields = new("Fields");
            root.Add(xfields);
            foreach (var field in Fields)
            {
                XElement xfield = new("Field");
                xfields.Add(xfield);
                field.Save(xfield);
            }
            try
            {
                doc.Save(filename);
                Changed = false;
            }
            catch { }
        }

        public static void Load(string filename)
        {
            Name = string.Empty;
            Description = string.Empty;
            Modules.Clear();
            Equipment.Clear();
            Fields.Clear();
            vals.Clear();
            Blocks.Clear();
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
                    else
                        Name = "Project";
                    var description = xproject?.Attribute("Description")?.Value;
                    if (description != null)
                        Description = description;
                    var xblocks = xproject?.Element("Blocks");
                    if (xblocks != null)
                    {
                        foreach (XElement xblock in xblocks.Elements("Block"))
                        {
                            var block = new Module();
                            if (Guid.TryParse(xblock.Element("Id")?.Value, out Guid id))
                                block.Id = id;
                            if (block.Id == Guid.Empty)
                                block.Id = Guid.NewGuid();
                            block.Load(xblock);
                            Blocks.Add(block);
                        }
                    }
                    var xmodules = xproject?.Element("Modules");
                    if (xmodules != null)
                    {
                        foreach (XElement xmodule in xmodules.Elements("Module"))
                        {
                            var module = new Module();
                            if (Guid.TryParse(xmodule.Element("Id")?.Value, out Guid id))
                                module.Id = id;
                            if (module.Id == Guid.Empty)
                                module.Id = Guid.NewGuid();
                            module.Load(xmodule);
                            Modules.Add(module);
                            if (string.IsNullOrEmpty(module.Name))
                                module.Name = "Task" + Modules.Count;
                        }
                    }
                    var xqupment = xproject?.Element("Equipment");
                    if (xqupment != null)
                    {
                        foreach (XElement xunit in xqupment.Elements("Unit"))
                        {
                            var unit = new Unit();
                            if (Guid.TryParse(xunit.Element("Id")?.Value, out Guid id))
                                unit.Id = id;
                            if (unit.Id == Guid.Empty)
                                unit.Id = Guid.NewGuid();
                            unit.Load(xunit);
                            Equipment.Add(unit);
                            if (string.IsNullOrEmpty(unit.Name))
                                unit.Name = "Unit" + Modules.Count;
                        }
                    }
                    var xfields = xproject?.Element("Fields");
                    if (xfields != null)
                    {
                        foreach (XElement xfield in xfields.Elements("Field"))
                        {
                            var field = new Field();
                            if (Guid.TryParse(xfield.Element("Id")?.Value, out Guid id))
                                field.Id = id;
                            if (field.Id == Guid.Empty)
                                field.Id = Guid.NewGuid();
                            field.Load(xfield);
                            Fields.Add(field);
                            if (string.IsNullOrEmpty(field.Name))
                                field.Name = "Field" + Modules.Count;
                        }
                    }
                }
                Changed = false;
                OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.Load));
            }
            catch { }
        }

        public static TreeNode[] GetEquipmentTree()
        {
            List<TreeNode> collection = [];
            var rootNode = new TreeNode("Эмуляция");
            collection.Add(rootNode);
            int nunit = 1;
            foreach (var unit in Equipment.OrderBy(x => x.Name))
            {
                unit.Index = nunit++;
                var unitNode = new TreeNode(unit.ToString()) { Tag = unit };
                rootNode.Nodes.Add(unitNode);
            }
            rootNode.ExpandAll();
            return [.. collection];
        }

        public static TreeNode[] GetModulesTree()
        {
            List<TreeNode> collection = [];
            var rootNode = new TreeNode("Проект");
            collection.Add(rootNode);
            int nmodule = 1;
            foreach (var module in Modules.OrderBy(x => x.Name))
            {
                module.Index = nmodule++;
                var moduleNode = new TreeNode(module.ToString()) { Tag = module };
                rootNode.Nodes.Add(moduleNode);
            }
            rootNode.ExpandAll();
            return [.. collection];
        }

        public static TreeNode[] GetFieldTree()
        {
            List<TreeNode> collection = [];
            var rootNode = new TreeNode("Мнемосхемы");
            collection.Add(rootNode);
            int nfield = 1;
            foreach (var field in Fields.OrderBy(x => x.Name))
            {
                field.Index = nfield++;
                var fieldNode = new TreeNode(field.ToString()) { Tag = field };
                rootNode.Nodes.Add(fieldNode);
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
            var mathNode = new TreeNode("Арифметика");
            rootNode.Nodes.Add(mathNode);
            mathNode.Nodes.Add(new TreeNode("Сложение") { Tag = typeof(Mathematic.ADD) });
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
            //var blocksNode = new TreeNode("Блоки") { Tag = Blocks };
            //rootNode.Nodes.Add(blocksNode);
            //foreach (var block in Blocks)
            //    blocksNode.Nodes.Add(new TreeNode(block.Name) { Tag = block });
            var fieldNode = new TreeNode("Поле");
            rootNode.Nodes.Add(fieldNode);
            fieldNode.Nodes.Add(new TreeNode("Клапан") { Tag = typeof(Fields.VALVE) });
            fieldNode.Nodes.Add(new TreeNode("Место под рисунок") { Tag = typeof(Fields.IMAGEHOLDER) });
            rootNode.Expand();
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
            Equipment.Clear();
            Fields.Clear();
            Blocks.Clear();
            Changed = false;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.Clear));
        }

        public static Module AddModuleToProject(Module? newModule = null)
        {
            var module = newModule ?? new Module();
            Modules.Add(module);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.AddModule));
            return module;
        }

        public static Module AddUnitToProject(Unit? newUnit = null)
        {
            var module = newUnit ?? new Unit();
            Equipment.Add(module);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.AddUnit));
            return module;
        }

        public static Module AddFieldToProject(Field? newField = null)
        {
            var field = newField ?? new Field();
            Fields.Add(field);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.AddField));
            return field;
        }

        public static void RemoveModuleFromProject(Module module)
        {
            Modules.Remove(module);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.RemoveModule));
        }

        public static void RemoveUnitFromProject(Unit unit)
        {
            Equipment.Remove(unit);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.RemoveUnit));
        }

        public static void RemoveFieldFromProject(Field unit)
        {
            Fields.Remove(unit);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.RemoveField));
        }

        public static Module AddBlockToProject()
        {
            var module = new Module() { Name = $"Block{Blocks.Count}" };
            Blocks.Add(module);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.AddBlock));
            return module;
        }

        public static void RemoveBlockFromProject(Module module)
        {
            Blocks.Remove(module);
            Changed = true;
            OnChanged?.Invoke(null, new ProjectEventArgs(ProjectChangeKind.RemoveBlock));
        }

        public static event ProjectEventHandler? OnChanged;

        public static bool Running { get; private set; }

        public static void Start()
        {
            vals.Clear();
            foreach (var module in Modules.Union(Equipment)) 
            { 
                foreach (var element in module.Elements)
                {
                    if (element.Instance is IManualChange changer)
                        changer.Init();
                    if (element.Instance is IEmbededMemory memory)
                        memory.Init();
                }
            }
            foreach (var module in Modules.Union(Equipment)) 
            { 
                foreach (var element in module.Elements)
                {
                    element.Selected = false;
                }
                foreach (var link in module.Links)
                {
                    link.SetSelect(false);
                }
            }
            Running = true;
        }

        public static void Stop()
        {
            Running = false;
        }

        public static void UpdateElementAndFunction(Logic.CommonLogic ori, Element element, Logic.CommonLogic func)
        {
            func.SetItemId(ori.ItemId);
            func.Name = ori.Name;
            for (var i = 0; i < ori.InputLinkSources.Length; i++)
            {
                if (ori.LinkedInputs[i])
                {
                    var (id, outpin, ext) = ori.InputLinkSources[i];
                    func.SetValueLinkToInp(i, id, outpin, ext);
                }
            }
            element.Instance = func;
            element.CalculateTargets();
            Project.Changed = true;
        }

        private static Action? action;

        internal static void RefreshPanels(Action? act = null)
        {
            if (act == null)
                action?.Invoke();
            else
                action = act;
        }
    }

    public enum ValueKind
    {
        None,
        Digital,
        Analog,
    }

    public enum ValueSide
    {
        Input,
        Output,
    }

    public class ValueItem
    {
        public ValueKind Kind { get; set; }
        public ValueSide Side { get; set; }
        public object? Value { get; set; }
        public int Pin { get; set; }
        public Guid ElementId { get; set; }

        public override string ToString()
        {
            return Kind switch
            {
                ValueKind.Analog => $"{Value}",
                ValueKind.Digital => $"{Value ?? false}"[..1].ToUpper(),
                _ => string.Empty,
            };
        }
    }

    public enum ProjectChangeKind
    {
        Clear,
        Load,
        AddModule,
        RemoveModule,
        AddUnit,
        RemoveUnit,
        AddField,
        RemoveField,
        AddBlock,
        RemoveBlock,
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
