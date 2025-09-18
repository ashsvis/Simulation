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
    public partial class ChangeNumberDialog : Form
    {
        private int enteredValue;

        public ChangeNumberDialog(int enteredValue)
        {
            InitializeComponent();
            EnteredValue = enteredValue;
            tbValue.Text = enteredValue.ToString();
        }

        public int EnteredValue 
        { 
            get => enteredValue; 
            private set => enteredValue = value; 
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbValue.Text, out int value)) 
            { 
                enteredValue = value; 
                button1.Enabled = true;
            }
            else
                button1.Enabled = false;
        }
    }
}
