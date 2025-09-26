using Simulator.Model;
using Simulator.Model.Interfaces;
using System.Data;

namespace Simulator.View
{
    public partial class SelectLinkSourceForm : Form
    {
        public SelectLinkSourceForm()
        {
            InitializeComponent();
        }

        public (Guid, int) Result { get; private set; }

        private void SelectLinkSourceForm_Load(object sender, EventArgs e)
        {
            tvSources.Nodes.Clear();
            foreach (var module in Project.Modules)
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
