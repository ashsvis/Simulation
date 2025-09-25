using Simulator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator.View
{
    public partial class SelectLinkSourceForm : Form
    {
        public SelectLinkSourceForm()
        {
            InitializeComponent();
        }

        public (GetLinkValueMethod?, int) Result { get; private set; }

        private void SelectLinkSourceForm_Load(object sender, EventArgs e)
        {
            tvSources.Nodes.Clear();
            foreach (var module in Project.Modules)
            {
                var moduleNode = new TreeNode(string.IsNullOrWhiteSpace(module.Name) ? $"Модуль {module.Index}" : module.Name);
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
                            var outputNode = new TreeNode($"{elementName}.{outputName}") { Tag = new Tuple<ILinkSupport, int>(link, 0) };
                            moduleNode.Nodes.Add(outputNode);
                        }
                        else if (func.OutputNames.Length > 1)
                        {
                            var elementNode = new TreeNode($"{elementName}");
                            moduleNode.Nodes.Add(elementNode);
                            for (var i = 0; i < func.OutputNames.Length; i++)
                            {
                                var outputName = string.IsNullOrWhiteSpace(func.OutputNames[i]) ? "Out" : func.OutputNames[i];
                                var outputNode = new TreeNode($"{i + 1}. {outputName}") { Tag = new Tuple<ILinkSupport, int>(link, i) };
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
                        var tuple = (Tuple<ILinkSupport, int>)e.Node.Tag;
                        Result = (tuple.Item1.GetResultLink(tuple.Item2), tuple.Item2);
                        break;
                    }
                    n++;
                }
            }
        }
    }
}
