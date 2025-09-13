using Simulator.Model;
using Simulator.View;

namespace Simulator
{
    public partial class PanelForm : Form
    {
        public HostForm Host { get; }
        public bool IsPrimary { get; }
        public new Rectangle Bounds { get; }

        public PanelForm(HostForm host, bool isPrimary, Rectangle bounds)
        {
            InitializeComponent();
            tsmiOneScreenMode.Visible = host.MultiScreensMode;
            tsmiMultiScreensMode.Visible = !host.MultiScreensMode;
            tslScreenNumber.Visible = !host.MultiScreensMode;
            tslScreenNumber.Text = $"{host.OneScreenIndex + 1}";
            tsbScreenToLeft.Visible = !host.MultiScreensMode && host.OneScreenIndex > 0; ;
            tsbScreenToRight.Visible = !host.MultiScreensMode && host.OneScreenIndex < Screen.AllScreens.Length - 1;

            // �������������� ��������� ���� � ������ ������ ������
            //var location = Properties.Settings.Default.MainFormLocation;
            //var size = Properties.Settings.Default.MainFormSize;
            //if (location.IsEmpty || size.IsEmpty)
            //    CenterToScreen();
            //else
            //{
            //    Location = location;
            //    Size = size;
            //    WindowState = Properties.Settings.Default.MainFormMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
            //}

            Host = host;
            IsPrimary = isPrimary;
            Bounds = bounds;
            Project.OnChanged += Project_OnChanged;
        }

        private void Project_OnChanged(object? sender, ProjectEventArgs e)
        {
            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());
            if (e.ChangeKind == ProjectChangeKind.Clear || e.ChangeKind == ProjectChangeKind.Load)
            {
                pgProps.SelectedObject = null;
                MdiChildren.ToList().ForEach(x => x.Close());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tvLibrary.Nodes.Clear();
            tvLibrary.Nodes.AddRange(Project.GetLibraryTree());

            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());

            timerInterface.Enabled = true;
            timerSimulation.Enabled = true;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            // ���������� ��������� ���� � ����� ������ ������
            //Properties.Settings.Default.MainFormMaximized = WindowState == FormWindowState.Maximized;
            //WindowState = FormWindowState.Normal;
            //Properties.Settings.Default.MainFormLocation = Location;
            //Properties.Settings.Default.MainFormSize = Size;
            //Properties.Settings.Default.Save();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Host.Close();
        }

        private void tsmiCreate_Click(object sender, EventArgs e)
        {
            var notSaved = Project.Modules.Any(x => x.Changed);
            if (notSaved)
            {
                var result = MessageBox.Show("������� ��������� �� ���������! ��������?",
                    "�������� ������ �������", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    Project.Save();
                if (result == DialogResult.Cancel)
                    return;
            }
            Project.Clear();
            pgProps.SelectedObject = null;
            AddModuleToProject();
        }

        public void AddModuleToProject()
        {
            var module = Project.AddModuleToProject();

            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());

            var node = tvModules.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == module);
            if (node != null)
            {
                tvModules.SelectedNode = node;
                node.EnsureVisible();
                pgProps.SelectedObject = module;
            }

            CreateNewModuleForm(module);
        }

        private void tsmiAddModule_Click(object sender, EventArgs e)
        {
            AddModuleToProject();
        }

        private void CreateNewModuleForm(Module module)
        {
            var childForm = new ModuleForm(this, module) { MdiParent = this, WindowState = FormWindowState.Maximized, Text = module.ToString() };
            childForm.ElementSelected += ChildForm_ElementSelected;
            childForm.FormClosed += ChildForm_FormClosed;
            childForm.Show();
        }

        private void ChildForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            pgProps.SelectedObject = null;
        }

        private void ChildForm_ElementSelected(object? sender, EventArgs e)
        {
            pgProps.SelectedObject = sender;
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

        public event EventHandler? SimulationTick;

        /// <summary>
        /// ������ ������������� (100 ��)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSimulation_Tick(object sender, EventArgs e)
        {
            SimulationTick?.Invoke(this, EventArgs.Empty);
            //if (!pgProps.Focused) pgProps.Refresh();
            tsmiSave.Enabled = Project.Changed || Project.Modules.Any(x => x.Changed);
        }

        private void tvLibrary_MouseDown(object sender, MouseEventArgs e)
        {
            var node = tvLibrary.GetNodeAt(e.X, e.Y);
            if (node != null && e.Button == MouseButtons.Left)
            {
                tvLibrary.SelectedNode = null;
                tvLibrary.SelectedNode = node;
                if (node.Tag is Type type)
                {
                    try
                    {
                        var module = new Element() { Type = type };
                        if (module != null)
                        {
                            var ret = tvLibrary.DoDragDrop(module, DragDropEffects.Copy);
                            if (ret == DragDropEffects.None)
                            {
                                Cursor = Cursors.Default;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "������� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void pgProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            foreach (var form in MdiChildren)
            {
                if (form is IUpdateView view)
                    view.UpdateView();
            }
            pgProps.Refresh();
        }

        private void tsbHostClose_Click(object sender, EventArgs e)
        {
            Host.Close();
        }

        private void PanelForm_Leave(object sender, EventArgs e)
        {
            toolStripCaption.BackColor = SystemColors.InactiveCaption;
            toolStripCaption.ForeColor = SystemColors.InactiveCaptionText;
        }

        private void PanelForm_Enter(object sender, EventArgs e)
        {
            toolStripCaption.BackColor = SystemColors.ActiveCaption;
            toolStripCaption.ForeColor = SystemColors.ActiveCaptionText;
        }

        private void pnLeftSize_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var r = ClientRectangle;
            g.DrawLine(SystemPens.ControlDarkDark, new PointF(r.Left, r.Top), new PointF(r.Left, r.Bottom));
            g.DrawLine(SystemPens.ControlDark, new PointF(r.Left + 1, r.Top), new PointF(r.Left + 1, r.Bottom));
            g.DrawLine(SystemPens.ControlLight, new PointF(r.Left + 2, r.Top), new PointF(r.Left + 2, r.Bottom));
            g.DrawLine(SystemPens.ControlLightLight, new PointF(r.Left + 3, r.Top), new PointF(r.Left + 3, r.Bottom));
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            Project.Save();
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            var notSaved = Project.Modules.Any(x => x.Changed);
            if (notSaved)
            {
                var result = MessageBox.Show("������� ��������� �� ���������! ��������?", "�������� �������", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    Project.Save();
                if (result == DialogResult.Cancel)
                    return;
            }
            var dlg = new OpenFileDialog() { DefaultExt = "xml", Filter = "*.xml|*.xml" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pgProps.SelectedObject = null;
                MdiChildren.ToList().ForEach(x => x.Close());
                Project.Load(dlg.FileName);
                tvModules.Nodes.Clear();
                tvModules.Nodes.AddRange(Project.GetModulesTree());
                if (tvModules.Nodes[0].Nodes.Count > 0)
                {
                    var node = tvModules.Nodes[0].Nodes[0];
                    if (node.Tag is Module module)
                    {
                        tvModules.SelectedNode = node;
                        pgProps.SelectedObject = module;
                        CreateNewModuleForm(module);
                    }
                }
            }
        }

        private void tvModules_DoubleClick(object sender, EventArgs e)
        {
            if (tvModules.SelectedNode is TreeNode node)
            {
                if (node.Tag is Module module)
                {
                    var form = MdiChildren.OfType<ModuleForm>().FirstOrDefault(x => x.Module == module);
                    if (form != null)
                    {
                        form.WindowState = FormWindowState.Maximized;
                        form.BringToFront();
                        form.Show();
                    }
                    else
                        CreateNewModuleForm(module);
                }
            }
        }

        private void PanelForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (ActiveMdiChild is ModuleForm form)
            {
                tslPanelCaption.Text = $"{form.Text}";
                var node = tvModules.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == form.Module);
                if (node != null)
                {
                    tvModules.SelectedNode = node;
                    node.EnsureVisible();
                }
            }
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                FileName = Project.FileName,
                DefaultExt = "xml",
                Filter = "*.xml|*.xml"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Project.Save(dlg.FileName);
            }
        }

        public void RemoveModuleFromProject(Module module)
        {
            var node = tvModules.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == module);
            if (node != null)
                tvModules.Nodes[0].Nodes.Remove(node);
            var form = MdiChildren.OfType<ModuleForm>().FirstOrDefault(x => x.Module == module);
            form?.Close();
            Project.RemoveModuleFromProject(module);
        }

        private void tvModules_MouseDown(object sender, MouseEventArgs e)
        {
            var node = tvModules.GetNodeAt(e.X, e.Y);
            if (node != null && e.Button == MouseButtons.Left)
            {
                tvModules.SelectedNode = null;
                tvModules.SelectedNode = node;
                if (node.Tag is ProjectProxy project)
                {
                    pgProps.SelectedObject = project;
                }
                else if (node.Tag is Module module)
                {
                    pgProps.SelectedObject = module;
                }
            }
        }

        private void tsbMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void tsmiMultiScreensMode_Click(object sender, EventArgs e)
        {
            Host.MultiScreensMode = true;
            Application.Restart();
        }

        private void tsmiOneScreenMode_Click(object sender, EventArgs e)
        {
            Host.MultiScreensMode = false;
            Application.Restart();
        }

        private void tsmiScreenMoveRight_Click(object sender, EventArgs e)
        {
            if (Host.OneScreenIndex == Screen.AllScreens.Length - 1) return;
            Host.OneScreenIndex++;
            Location = Screen.AllScreens[Host.OneScreenIndex].Bounds.Location;
            tsbScreenToLeft.Visible = !Host.MultiScreensMode && Host.OneScreenIndex > 0; ;
            tsbScreenToRight.Visible = !Host.MultiScreensMode && Host.OneScreenIndex < Screen.AllScreens.Length - 1;
            tslScreenNumber.Text = $"{Host.OneScreenIndex + 1}";
        }

        private void tsmiScreenMoveLeft_Click(object sender, EventArgs e)
        {
            if (Host.OneScreenIndex == 0) return;
            Host.OneScreenIndex--;
            Location = Screen.AllScreens[Host.OneScreenIndex].Bounds.Location;
            tsbScreenToLeft.Visible = !Host.MultiScreensMode && Host.OneScreenIndex > 0; ;
            tsbScreenToRight.Visible = !Host.MultiScreensMode && Host.OneScreenIndex < Screen.AllScreens.Length - 1;
            tslScreenNumber.Text = $"{Host.OneScreenIndex + 1}";
        }
    }
}
