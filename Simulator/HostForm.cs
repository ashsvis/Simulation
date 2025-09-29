using Simulator.Model;
using System.ComponentModel;

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
            backWorkerCalc.CancelAsync();
        }

        private void RootForm_Load(object sender, EventArgs e)
        {
            backWorkerCalc.RunWorkerAsync();
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

        public void RemoveModuleChildWindowFromPanels(Model.Module module)
        {
            panels.ForEach(frm =>
            {
                frm.RemoveModuleChildFormFromPanel(module);
            });
        }

        private void backWorkerCalc_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                Thread.Sleep(50);
                try
                {
                    if (Project.Modules.Count != 0)
                        Project.Modules.ToList().ForEach(module => module.Calculate());
                    Thread.Sleep(50);
                    if (Project.Equipment.Count != 0)
                        Project.Equipment.ToList().ForEach(unit => unit.Calculate());
                    worker.ReportProgress(0);
                }
                catch { }
            }
        }

        private void backWorkerCalc_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            
        }

        private void backWorkerCalc_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            SimulationTick?.Invoke(this, EventArgs.Empty);
        }
    }
}
