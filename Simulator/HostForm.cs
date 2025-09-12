namespace Simulator
{
    public partial class HostForm : Form
    {
        private readonly PanelForm[] panels;

        public HostForm()
        {
            InitializeComponent();
            Width = 0;
            Height = 1;
            var monitors = Screen.AllScreens;
            panels = new PanelForm[monitors.Length];
            for (var i = 0; i < monitors.Length; i++)
            {
                panels[i] = new PanelForm(this, monitors[i].Primary, monitors[i].WorkingArea)
                {
                    Visible = false,
                    WindowState = FormWindowState.Normal,
                    StartPosition = FormStartPosition.Manual,
                    FormBorderStyle = FormBorderStyle.None,
                    Location = monitors[i].WorkingArea.Location,
                    Size = monitors[i].WorkingArea.Size,
                    MinimizeBox = false,
                    MaximizeBox = false,
                };
                panels[i].Show(this);
                panels[i].Refresh();
                // debug
                break;
            }
        }

        private void RootForm_Resize(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void RootForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // финализация
        }

        private void RootForm_Load(object sender, EventArgs e)
        {
            #region Защита от повторного запуска
            //var process = RunningInstance();
            //if (process != null) { Application.Exit(); return; }
            #endregion
        }

        //public void LoadModel()
        //{
        //    Project.Load("project.xml");
        //}

        //[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        //private static Process RunningInstance()
        //{
        //    var current = Process.GetCurrentProcess();
        //    var processes = Process.GetProcessesByName(current.ProcessName);
        //    // Просматриваем все процессы
        //    return processes.Where(process => process.Id != current.Id).
        //        FirstOrDefault(process => Assembly.GetExecutingAssembly().
        //            Location.Replace("/", "\\") == current.MainModule.FileName);
        //    // нет, таких процессов не найдено
        //}
    }
}
