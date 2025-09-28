namespace Simulator.View
{
    public partial class ChangeNameDialog : Form
    {
        private string enteredValue = string.Empty;

        public ChangeNameDialog(string enteredValue)
        {
            InitializeComponent();
            EnteredValue = enteredValue;
            tbValue.Text = enteredValue.ToString();
        }

        public string EnteredValue 
        { 
            get => enteredValue; 
            private set => enteredValue = value; 
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            enteredValue = tbValue.Text;
            button1.Enabled = true;
        }
    }
}
