using System.Globalization;

namespace Simulator.View
{
    public partial class ChangeValueDialog : Form
    {
        private double enteredValue;

        public ChangeValueDialog(double enteredValue)
        {
            InitializeComponent();
            if (Properties.Settings.Default.DarkMode)
                ThemeManager.ApplyDarkTheme(this);
            EnteredValue = enteredValue;
            var fp = CultureInfo.GetCultureInfo("en-US");
            tbValue.Text = enteredValue.ToString(fp);
        }

        public double EnteredValue 
        { 
            get => enteredValue; 
            private set => enteredValue = value; 
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            var fp = CultureInfo.GetCultureInfo("en-US");
            if (double.TryParse(tbValue.Text, fp, out double value)) 
            { 
                enteredValue = value; 
                button1.Enabled = true;
            }
            else
                button1.Enabled = false;
        }
    }
}
