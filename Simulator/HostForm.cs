using Simulator.Model;

namespace Simulator
{
    public partial class HostForm : Form
    {
        private readonly List<PanelForm> panels = [];
        private bool multiScreensMode;
        private int oneScreenIndex;

        public bool MultiScreensMode
        {
            get => multiScreensMode;
            set
            {
                if (multiScreensMode == value) return;
                multiScreensMode = value;
                Properties.Settings.Default.MultiScreensMode = multiScreensMode;
                Properties.Settings.Default.Save();
            }
        }

        public int OneScreenIndex
        {
            get => oneScreenIndex;
            set
            {
                if (oneScreenIndex == value) return;
                oneScreenIndex = value;
                Properties.Settings.Default.OneScreenIndex = oneScreenIndex;
                Properties.Settings.Default.Save();
            }
        }

        public HostForm()
        {
            InitializeComponent();
            Width = 0;
            Height = 1;
            MultiScreensMode = Properties.Settings.Default.MultiScreensMode;
            OneScreenIndex = Properties.Settings.Default.OneScreenIndex;
            var monitors = Screen.AllScreens;
            for (var i = 0; i < monitors.Length; i++)
            {
                panels.Add(new PanelForm(this, i, monitors[i].Primary, monitors[i].WorkingArea)
                {
                    Visible = false,
                    WindowState = FormWindowState.Normal,
                    StartPosition = FormStartPosition.Manual,
                    FormBorderStyle = FormBorderStyle.None,
                    Location = monitors[i].WorkingArea.Location,
                    Size = monitors[i].WorkingArea.Size,
                    MinimizeBox = false,
                    MaximizeBox = false,
                });
                if (MultiScreensMode || !MultiScreensMode && i == OneScreenIndex)
                {
                    panels[i].Show(this);
                    panels[i].Refresh();
                }
            }
        }

        public void HideAllBut(PanelForm panel)
        {
            panels.ForEach(x =>
            {
                if (x != panel) x.Hide();
            });
        }

        public void ShowAllBut(PanelForm panel)
        {
            panels.ForEach(x =>
            {
                if (x != panel)
                {
                    x.Show(this);
                    x.Refresh();
                }
            });
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
            timerSimulation.Enabled = true;
        }

        public void SwapPanels(int prev, int current)
        {
            if (prev < current)
            {
                OneScreenIndex = current;
                var prevIndex = panels[prev].PanelIndex;
                var currentIndex = panels[current].PanelIndex;
                panels[prev].PanelIndex = currentIndex;
                panels[current].PanelIndex = prevIndex;
                var prevLocation = panels[prev].Location;
                var currentLocation = panels[current].Location;
                panels[prev].Location = currentLocation;
                panels[current].Location = prevLocation;
                var panel = panels[prev];
                panels.RemoveAt(prev);
                if (current + 1 < panels.Count)
                    panels.Insert(current + 1, panel);
                else
                    panels.Add(panel);
            }
            else if (prev > current)
            {
                OneScreenIndex = current;
                var prevIndex = panels[prev].PanelIndex;
                var currentIndex = panels[current].PanelIndex;
                panels[prev].PanelIndex = currentIndex;
                panels[current].PanelIndex = prevIndex;
                var prevLocation = panels[prev].Location;
                var currentLocation = panels[current].Location;
                panels[prev].Location = currentLocation;
                panels[current].Location = prevLocation;
                var panel = panels[prev];
                panels.RemoveAt(prev);
                panels.Insert(current, panel);
            }
        }

        public event EventHandler? SimulationTick;

        /// <summary>
        /// Таймер моделирования (100 мс)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSimulation_Tick(object sender, EventArgs e)
        {
            try
            {
                Project.Modules.ForEach(module => module.Calculate());
                Project.Equipment.ForEach(unit => unit.Calculate());
                SimulationTick?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        public void RemoveModuleChildWindowFromPanels(Model.Module module)
        {
            panels.ForEach(frm => 
            {
                frm.RemoveModuleChildFormFromPanel(module);
            });
        }
    }
}
