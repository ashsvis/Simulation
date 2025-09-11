using System.Diagnostics;
using System.Reflection;
using System.Security.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Simulator
{
    public partial class RootForm : Form
    {
        private MainForm[] panels;

        public RootForm()
        {
            InitializeComponent();
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
            Width = 0;
            Height = 1;
            var monitors = Screen.AllScreens;
            panels = new MainForm[monitors.Length];
            for (var i = 0; i < monitors.Length; i++)
            {
                panels[i] = new MainForm(this, monitors[i].Primary, monitors[i].Bounds)
                {
                    Visible = false,
                    Location = monitors[i].Bounds.Location,
                    Size = monitors[i].Bounds.Size,
                    WindowState = FormWindowState.Maximized,
                    MinimizeBox = false,
                    MaximizeBox = false,
                }; //monitors[i].WorkingArea
                panels[i].Show(this);
                panels[i].Refresh();
            }
        }

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
