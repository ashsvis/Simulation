using Simulator.Model;
using Simulator.Model.Interfaces;
using System;
using System.Data;

namespace Simulator.View
{
    public partial class SelectLinkSourceForm : Form
    {
        private readonly KindLinkSource kind;

        public SelectLinkSourceForm(KindLinkSource kind)
        {
            InitializeComponent();
            this.kind = kind;
        }

        public (Guid, int) Result { get; private set; }

        private void SelectLinkSourceForm_Load(object sender, EventArgs e)
        {
            tvSources.Nodes.Clear();
            switch (kind)
            {
                case KindLinkSource.LogicOutputs:
                    foreach (var module in Project.Modules.OrderBy(x => x.Name))
                    {
                        var moduleNode = new TreeNode(string.IsNullOrWhiteSpace(module.Name) ? $"Задача {module.Index}" : module.Name);
                        tvSources.Nodes.Add(moduleNode);
                        var n = 1;
                        foreach (var item in module.Elements)
                        {
                            if (item.Instance is IFunction func && item.Instance is ILinkSupport link)
                            {

                                var elementName = string.IsNullOrWhiteSpace(func.Name) ? $"L{n}({func.FuncName})" : func.Name;
                                if (func.OutputNames.Length == 1)
                                {
                                    var outputName = string.IsNullOrWhiteSpace(func.OutputNames[0]) ? "Out" : func.OutputNames[0];
                                    var outputNode = new TreeNode($"{elementName}.{outputName}") { Tag = new Tuple<Guid, int>(item.Id, 0) };
                                    moduleNode.Nodes.Add(outputNode);
                                }
                                else if (func.OutputNames.Length > 1)
                                {
                                    var elementNode = new TreeNode($"{elementName}");
                                    moduleNode.Nodes.Add(elementNode);
                                    for (var i = 0; i < func.OutputNames.Length; i++)
                                    {
                                        var outputName = string.IsNullOrWhiteSpace(func.OutputNames[i]) ? "Out" : func.OutputNames[i];
                                        var outputNode = new TreeNode($"{i + 1}. {outputName}") { Tag = new Tuple<Guid, int>(item.Id, i) };
                                        elementNode.Nodes.Add(outputNode);
                                    }
                                }
                            }
                            n++;
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
                                var elementName = string.IsNullOrWhiteSpace(di.Name) ? $"DI{di.Order}" : di.Name;
                                var elementNode = new TreeNode(elementName) { Tag = new Tuple<Guid, int>(item.Id, di.Order) };
                                unitNode.Nodes.Add(elementNode);
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
                                var elementName = string.IsNullOrWhiteSpace(@do.Name) ? $"DO{@do.Order}" : @do.Name;
                                var elementNode = new TreeNode(elementName) { Tag = new Tuple<Guid, int>(item.Id, @do.Order) };
                                unitNode.Nodes.Add(elementNode);
                            }
                        }
                    }
                    break;
            }
        }

        private void tvSources_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnAccept.Enabled = e.Node != null && e.Node.Level > 0 && e.Node.Nodes.Count == 0;
            var n = 0;
            if (e.Node != null && e.Node.Level > 0)
            {
                foreach (var node in e.Node.Parent.Nodes.Cast<TreeNode>())
                {
                    if (node == e.Node)
                    {
                        var tuple = (Tuple<Guid, int>)e.Node.Tag;
                        Result = (tuple.Item1, tuple.Item2);
                        break;
                    }
                    n++;
                }
            }
        }
    }
}
