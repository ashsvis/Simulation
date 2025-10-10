using Simulator.Model;
using Simulator.Model.Interfaces;
using System.Data;

namespace Simulator.View
{
    public partial class SelectLinkSourceForm : Form
    {
        private readonly KindLinkSource kind;
        private readonly (Guid, int, bool)? linkSource;
        private readonly Module? module;

        public SelectLinkSourceForm(KindLinkSource kind, (Guid, int, bool)? linkSource = null, Module? module = null)
        {
            InitializeComponent();
            if (Properties.Settings.Default.DarkMode)
                ThemeManager.ApplyDarkTheme(this);
            this.kind = kind;
            this.linkSource = linkSource;
            this.module = module;
        }

        public (Guid, int) Result { get; private set; }

        private void SelectLinkSourceForm_Load(object sender, EventArgs e)
        {
            tvSources.Nodes.Clear();
            switch (kind)
            {
                case KindLinkSource.LogicOutputs:
                    if (module != null && module.GetType() == typeof(Module))
                    {
                        var modules = new TreeNode("Проект");
                        tvSources.Nodes.Add(modules);
                        foreach (var module in Project.Modules.OrderBy(x => x.Name))
                        {
                            var moduleNode = new TreeNode(string.IsNullOrWhiteSpace(module.Name) ? $"Задача {module.Index}" : module.Name);
                            modules.Nodes.Add(moduleNode);
                            var n = 1;
                            foreach (var item in module.Elements)
                            {
                                if (item.Instance is IFunction func/* item.Instance is ILinkSupport link*/)
                                {

                                    var elementName = string.IsNullOrWhiteSpace(func.Name) ? $"L{n}({func.FuncName})" : func.Name;
                                    if (func.Outputs.Length == 1)
                                    {
                                        var outputName = string.IsNullOrWhiteSpace(func.Outputs[0].Name) ? "Out" : func.Outputs[0].Name;
                                        var outputNode = new TreeNode($"{elementName}.{outputName}") { Tag = new Tuple<Guid, int>(item.Id, 0) };
                                        moduleNode.Nodes.Add(outputNode);
                                        if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                            tvSources.SelectedNode = outputNode;
                                    }
                                    else if (func.Outputs.Length > 1)
                                    {
                                        var elementNode = new TreeNode($"{elementName}");
                                        moduleNode.Nodes.Add(elementNode);
                                        for (var i = 0; i < func.Outputs.Length; i++)
                                        {
                                            var outputName = string.IsNullOrWhiteSpace(func.Outputs[i].Name) ? "Out" : func.Outputs[i].Name;
                                            var outputNode = new TreeNode($"{i + 1}. {outputName}") { Tag = new Tuple<Guid, int>(item.Id, i) };
                                            elementNode.Nodes.Add(outputNode);
                                            if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                                tvSources.SelectedNode = outputNode;
                                        }
                                    }
                                }
                                n++;
                            }
                        }
                    }
                    if (module != null && module.GetType() == typeof(Unit))
                    {
                        var units = new TreeNode("Эмуляция");
                        tvSources.Nodes.Add(units);
                        foreach (var module in Project.Equipment.OrderBy(x => x.Name))
                        {
                            var moduleNode = new TreeNode(string.IsNullOrWhiteSpace(module.Name) ? $"Юнит {module.Index}" : module.Name);
                            units.Nodes.Add(moduleNode);
                            var n = 1;
                            foreach (var item in module.Elements)
                            {
                                if (item.Instance is IFunction func && item.Instance is ILinkSupport link)
                                {

                                    var elementName = string.IsNullOrWhiteSpace(func.Name) ? $"L{n}({func.FuncName})" : func.Name;
                                    if (func.Outputs.Length == 1)
                                    {
                                        var outputName = string.IsNullOrWhiteSpace(func.Outputs[0].Name) ? "Out" : func.Outputs[0].Name;
                                        var outputNode = new TreeNode($"{elementName}.{outputName}") { Tag = new Tuple<Guid, int>(item.Id, 0) };
                                        moduleNode.Nodes.Add(outputNode);
                                        if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                            tvSources.SelectedNode = outputNode;
                                    }
                                    else if (func.Outputs.Length > 1)
                                    {
                                        var elementNode = new TreeNode($"{elementName}");
                                        moduleNode.Nodes.Add(elementNode);
                                        for (var i = 0; i < func.Outputs.Length; i++)
                                        {
                                            var outputName = string.IsNullOrWhiteSpace(func.Outputs[i].Name) ? "Out" : func.Outputs[i].Name;
                                            var outputNode = new TreeNode($"{i + 1}. {outputName}") { Tag = new Tuple<Guid, int>(item.Id, i) };
                                            elementNode.Nodes.Add(outputNode);
                                            if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                                tvSources.SelectedNode = outputNode;
                                        }
                                    }
                                }
                                n++;
                            }
                        }
                    }
                    break;
                case KindLinkSource.EquipmentInputs:
                    foreach (var unit in Project.Equipment.OrderBy(x => x.Name))
                    {
                        var unitNode = new TreeNode(string.IsNullOrWhiteSpace(unit.Name) ? $"Юнит {unit.Index}" : unit.Name);
                        tvSources.Nodes.Add(unitNode);
                        foreach (var item in unit.Elements)
                        {
                            if (item.Instance is Model.Inputs.DI di)
                            {
                                var elementName = (string.IsNullOrWhiteSpace(di.Name) ? $"DI{di.Order}" : di.Name) + " " + di.Description;
                                var elementNode = new TreeNode(elementName) { Tag = new Tuple<Guid, int>(item.Id, di.Order) };
                                unitNode.Nodes.Add(elementNode);
                                if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                    tvSources.SelectedNode = elementNode;
                            }
                            if (item.Instance is Model.Inputs.AI ai)
                            {
                                var elementName = (string.IsNullOrWhiteSpace(ai.Name) ? $"AI{ai.Order}" : ai.Name) + " " + ai.Description;
                                var elementNode = new TreeNode(elementName) { Tag = new Tuple<Guid, int>(item.Id, ai.Order) };
                                unitNode.Nodes.Add(elementNode);
                                if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                    tvSources.SelectedNode = elementNode;
                            }
                        }
                    }
                    break;
                case KindLinkSource.EquipmentOutputs:
                    foreach (var unit in Project.Equipment.OrderBy(x => x.Name))
                    {
                        var unitNode = new TreeNode(string.IsNullOrWhiteSpace(unit.Name) ? $"Юнит {unit.Index}" : unit.Name);
                        tvSources.Nodes.Add(unitNode);
                        foreach (var item in unit.Elements)
                        {
                            if (item.Instance is Model.Outputs.DO @do)
                            {
                                var elementName = (string.IsNullOrWhiteSpace(@do.Name) ? $"DO{@do.Order}" : @do.Name) +" " + @do.Description;
                                var elementNode = new TreeNode(elementName) { Tag = new Tuple<Guid, int>(item.Id, @do.Order) };
                                unitNode.Nodes.Add(elementNode);
                                if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                    tvSources.SelectedNode = elementNode;
                            }
                            if (item.Instance is Model.Outputs.AO ao)
                            {
                                var elementName = (string.IsNullOrWhiteSpace(ao.Name) ? $"AO{ao.Order}" : ao.Name) + " " + ao.Description;
                                var elementNode = new TreeNode(elementName) { Tag = new Tuple<Guid, int>(item.Id, ao.Order) };
                                unitNode.Nodes.Add(elementNode);
                                if (linkSource != null && linkSource.Value.Item1 == item.Id)
                                    tvSources.SelectedNode = elementNode;
                            }
                        }
                    }
                    break;
            }
        }

        private void tvSources_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnAccept.Enabled = false;
            var n = 0;
            if (e.Node != null && e.Node.Level > 0)
            {
                foreach (var node in e.Node.Parent.Nodes.Cast<TreeNode>())
                {
                    if (node == e.Node && e.Node.Tag is Tuple<Guid, int> tuple)
                    {
                        Result = (tuple.Item1, tuple.Item2);
                        btnAccept.Enabled = true;
                        break;
                    }
                    n++;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Result = (Guid.Empty, 0);
            DialogResult = DialogResult.OK;
        }
    }
}
