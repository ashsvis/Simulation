using Simulator.Model;
using Simulator.View;
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

        private void zoomPad_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.GetDataPresent(typeof(Element)))
                    e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void zoomPad_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
            {
                zoomPad.Invalidate();
            }
        }

        private void zoomPad_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Effect == DragDropEffects.Copy)
            {
                if (e.Data.GetData(typeof(Element)) is Element item && item.Type != null)
                {
                    item.Instance = Activator.CreateInstance(item.Type);
                    item.Location = zoomPad.PointToClient(new Point(e.X, e.Y));
                    zoomPad.Invalidate();
                }
            }
        }
    }
}
