using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator
{
    public partial class ChildForm : Form
    {
        private readonly MainForm mainForm;

        public ChildForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            mainForm.SimulationTick += MainForm_SimulationTick;
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.SimulationTick -= MainForm_SimulationTick;
        }

        private void MainForm_SimulationTick(object? sender, EventArgs e)
        {

        }
    }
}
