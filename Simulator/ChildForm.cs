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
            module.Items.Add(new Model.Trigger.RS());
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.SimulationTick -= MainForm_SimulationTick;
        }

        private void MainForm_SimulationTick(object? sender, EventArgs e)
        {
            module.Items.ForEach(item => item.Calculate());
        }
    }
}
