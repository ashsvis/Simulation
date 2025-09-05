using Simulator.Model;
using System.Diagnostics;
using Module = Simulator.Model.Module;

namespace Simulator
{
    public partial class ChildForm : Form
    {
        private readonly MainForm mainForm;

        private readonly Module module = new();

        public ChildForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            mainForm.SimulationTick += MainForm_SimulationTick;

            module.Items.Clear();
            var not = new Model.Logic.NOT();
            module.Items.Add(not);
            var and = new Model.Logic.AND();
            module.Items.Add(and);
            var or = new Model.Logic.OR();
            module.Items.Add(or);
            var rs = new Model.Trigger.RS();
            module.Items.Add(rs);

            module.Items.ForEach(item => item.ResultChanged += Item_ResultChanged);

            not.SetValueLinkToInp(rs.GetResultLink());
            or.SetValueLinkToInp2(not.GetResultLink());
        }

        private void Item_ResultChanged(object sender, Model.ResultCalculateEventArgs args)
        {
            Debug.WriteLine($"{Name}.{sender.GetType().Name}.{args.Propname} is {args.Result}");
        }

        private void ChildForm_Load(object sender, EventArgs e)
        {
            //var rs = module.Items.OfType<Model.Trigger.RS>().FirstOrDefault();
            //var not = module.Items.OfType<Model.Logic.NOT>().FirstOrDefault();
            //if (rs != null && not != null)
            //{
            //    not.ResultChanged += (o, e) => 
            //    {
            //        checkBox1.Checked = not.Out; 
            //    };
            //    button1.Click += (o, e) => { rs.S = true; };
            //    button2.Click += (o, e) => { rs.R = true; };
            //}
            //treeView1.Nodes.Clear();
            //var rootNode = new TreeNode("Библиотека");
            //treeView1.Nodes.Add(rootNode);
            //var logicaNode = new TreeNode("Логика");
            //logicaNode.Nodes.Add(new TreeNode("NOT") { Tag = typeof(Model.Logic.NOT) });
            //logicaNode.Nodes.Add(new TreeNode("AND") { Tag = typeof(Model.Logic.AND) });
            //logicaNode.Nodes.Add(new TreeNode("OR") { Tag = typeof(Model.Logic.OR) });
            //rootNode.Nodes.Add(logicaNode);
            //var triggerNode = new TreeNode("Триггеры");
            //rootNode.Nodes.Add(triggerNode);
            //triggerNode.Nodes.Add(new TreeNode("RS-триггер") { Tag = typeof(Model.Trigger.RS) });
            //rootNode.ExpandAll();
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            module.Items.ForEach(item => item.ResultChanged -= Item_ResultChanged);
            mainForm.SimulationTick -= MainForm_SimulationTick;
        }

        private void MainForm_SimulationTick(object? sender, EventArgs e)
        {
            module.Items.ForEach(item =>
            {
                try
                {
                    item.Calculate();
                }
                catch 
                { 

                }
            });
        }
    }
}
