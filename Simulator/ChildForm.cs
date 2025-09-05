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
            module.Items.Add(new Model.Logic.NOT());
            module.Items.Add(new Model.Logic.AND());
            module.Items.Add(new Model.Logic.OR());
            var rs = new Model.Trigger.RS();
            module.Items.Add(rs);
            module.Items.ForEach(item => item.ResultChanged += Item_OutputChanged);

            rs.S = true;
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
