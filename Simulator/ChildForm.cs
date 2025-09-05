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
            module.Items.ForEach(item => item.ResultChanged += Item_OutputChanged);

            rs.S = true;
            not.Inp = rs.Q;
            or.Inp1 = not.Out;
        }

        private void Item_OutputChanged(object sender, Model.ResultEventArgs args)
        {
            Debug.WriteLine($"{Name}.{sender.GetType().Name}.{args.Propname} is {args.Result}");
        }

        private void ChildForm_Load(object sender, EventArgs e)
        {

        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            module.Items.ForEach(item => item.ResultChanged -= Item_OutputChanged);
            mainForm.SimulationTick -= MainForm_SimulationTick;
        }

        private void MainForm_SimulationTick(object? sender, EventArgs e)
        {
            module.Items.ForEach(item => item.Calculate());
        }
    }
}
