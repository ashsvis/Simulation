namespace Simulator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // �������������� ��������� ���� � ������ ������ ������
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
            timerInterface.Enabled = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            // ���������� ��������� ���� � ����� ������ ������
            Properties.Settings.Default.MainFormMaximized = WindowState == FormWindowState.Maximized;
            WindowState = FormWindowState.Normal;
            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MainFormSize = Size;
            Properties.Settings.Default.Save();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewChildForm();
        }

        private void CreateNewChildForm()
        {
            var childForm = new ChildForm() { MdiParent = this, WindowState = FormWindowState.Maximized };
            childForm.Show();
        }

        /// <summary>
        /// ���������� ��������� ��������� ���������� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerInterface_Tick(object sender, EventArgs e)
        {
            ����ToolStripMenuItem.Visible = MdiChildren.Length > 0;
        }

        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void ��������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }
    }
}
