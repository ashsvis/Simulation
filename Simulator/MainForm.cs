namespace Simulator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var location = Properties.Settings.Default.MainFormLocation;
            var size = Properties.Settings.Default.MainFormSize;
            if (location.IsEmpty || size.IsEmpty)
                CenterToScreen();
            else
            {
                Location = location;
                Size = size;
                WindowState = Properties.Settings.Default.MainFormMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainFormMaximized = WindowState == FormWindowState.Maximized;
            WindowState = FormWindowState.Normal;
            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MainFormSize = Size;
            Properties.Settings.Default.Save();
        }

        private void ‚˚ıÓ‰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
