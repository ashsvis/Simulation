using Simulator.Model;
using Simulator.View;
using System.Linq.Expressions;
using System.Reflection;

namespace Simulator
{
    public partial class PanelForm : Form
    {
        public HostForm Host { get; }
        public int PanelIndex { get; set; }
        public bool IsPrimary { get; }
        public new Rectangle Bounds { get; }

        private Point firstMouseDown;
        private Point mousePosition;

        public PanelForm(HostForm hostForm, int panelIndex, bool isPrimary, Rectangle bounds)
        {
            InitializeComponent();
            UpdateScreenControls(hostForm);
            Host = hostForm;
            PanelIndex = panelIndex;
            IsPrimary = isPrimary;
            Bounds = bounds;
            Project.OnChanged += Project_OnChanged;
            hostForm.SimulationTick += Panel_SimulationTick;
        }

        private void Panel_SimulationTick(object? sender, EventArgs e)
        {
            SimulationTick?.Invoke(this, EventArgs.Empty);
            tsmiSave.Enabled = Project.Changed || Project.Modules.Any(x => x.Changed);
        }

        private void UpdateScreenControls(HostForm host)
        {
            tsmiOneScreenMode.Visible = host.MultiScreensMode;
            tsmiMultiScreensMode.Visible = !host.MultiScreensMode;
            tslScreenNumber.Visible = !host.MultiScreensMode;
            tslScreenNumber.Text = $"{PanelIndex + 1}";
            tsbScreenToLeft.Visible = !host.MultiScreensMode && PanelIndex > 0; ;
            tsbScreenToRight.Visible = !host.MultiScreensMode && PanelIndex < Screen.AllScreens.Length - 1;
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

        private void PanelForm_Load(object sender, EventArgs e)
        {
            panLeft.Width = Properties.Settings.Default.LeftToolsPanelWidth;
            panRight.Width = Properties.Settings.Default.RightToolsPanelWidth;

            tvLibrary.Nodes.Clear();
            tvLibrary.Nodes.AddRange(Project.GetLibraryTree());

            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());

            timerInterface.Enabled = true;
        }

        private void PanelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Host.Close();
        }

        private void tsmiCreate_Click(object sender, EventArgs e)
        {
            var notSaved = Project.Modules.Any(x => x.Changed);
            if (notSaved)
            {
                var result = MessageBox.Show("Текущие изменения не сохранены! Записать?",
                    "Создание нового проекта", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
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

        private void CreateNewModuleForm(Model.Module module)
        {
            var moduleForm = new ModuleForm(this, module) { MdiParent = this, WindowState = FormWindowState.Maximized, Text = module.ToString() };
            moduleForm.ElementSelected += ChildForm_ElementSelected;
            moduleForm.FormClosed += ChildForm_FormClosed;
            moduleForm.Show();
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
        /// Обновление состояния элементов интерфейса формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerInterface_Tick(object sender, EventArgs e)
        {
            окноToolStripMenuItem.Visible = MdiChildren.Length > 0;
            tsbCascadeLayout.Enabled = MdiChildren.Length > 0;
            tsbHorizontalLayout.Enabled = MdiChildren.Length > 0;
        }

        private void поГоризонталиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void воВертикалиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void упорядочитьСвернутыеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        public event EventHandler? SimulationTick;

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
                        MessageBox.Show(ex.Message, "Вставка модуля", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var result = MessageBox.Show("Текущие изменения не сохранены! Записать?", "Загрузка проекта", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
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
                    if (node.Tag is Model.Module module)
                    {
                        tvModules.SelectedNode = node;
                        pgProps.SelectedObject = module;
                        CreateNewModuleForm(module);
                    }
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

        public void RemoveModuleChildFormFromPanel(Model.Module module)
        {
            var form = MdiChildren.OfType<ModuleForm>().FirstOrDefault(x => x.Module == module);
            form?.Close();
        }

        private void tvModules_MouseDown(object sender, MouseEventArgs e)
        {
            var node = tvModules.GetNodeAt(e.X, e.Y);
            if (node != null && e.Button == MouseButtons.Left)
            {
                tvModules.SelectedNode = null;
                tvModules.SelectedNode = node;
                if (node.Tag is ProjectProxy project)
                    pgProps.SelectedObject = project;
                else if (node.Tag is Model.Module module)
                    pgProps.SelectedObject = module;
                if (e.Clicks > 1)
                    EnsureShowModuleChildForm();
            }
        }

        private void tsbMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void tsmiMultiScreensMode_Click(object sender, EventArgs e)
        {
            Host.MultiScreensMode = true;
            Host.ShowAllBut(this);
            UpdateScreenControls(Host);
        }

        private void tsmiOneScreenMode_Click(object sender, EventArgs e)
        {
            Host.MultiScreensMode = false;
            Host.HideAllBut(this);
            UpdateScreenControls(Host);
        }

        private void tsmiScreenMoveRight_Click(object sender, EventArgs e)
        {
            if (PanelIndex == Screen.AllScreens.Length - 1) return;
            var prev = PanelIndex;
            Host.SwapPanels(prev, PanelIndex + 1);
            UpdateScreenControls(Host);
        }

        private void tsmiScreenMoveLeft_Click(object sender, EventArgs e)
        {
            if (PanelIndex == 0) return;
            var prev = PanelIndex;
            Host.SwapPanels(prev, PanelIndex - 1);
            UpdateScreenControls(Host);
        }

        private void PanelForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                UpdateScreenControls(Host);
        }

        private void panRightSize_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
        }

        private void panRightSize_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var mp = mousePosition;
                var pt = e.Location;
                var delta = new Size(pt.X - mp.X, pt.Y - mp.Y);
                mousePosition = e.Location;
                panRight.SuspendLayout();
                panRight.Width -= delta.Width;
                panRight.ResumeLayout();
            }
        }

        private void panRightSize_MouseUp(object sender, MouseEventArgs e)
        {
            Properties.Settings.Default.RightToolsPanelWidth = panRight.Width;
            Properties.Settings.Default.Save();
        }

        private void pnLeftSize_MouseDown(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
        }

        private void pnLeftSize_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var mp = mousePosition;
                var pt = e.Location;
                var delta = new Size(pt.X - mp.X, pt.Y - mp.Y);
                mousePosition = e.Location;
                panLeft.SuspendLayout();
                panLeft.Width += delta.Width;
                panLeft.ResumeLayout();
            }
        }

        private void pnLeftSize_MouseUp(object sender, MouseEventArgs e)
        {
            Properties.Settings.Default.LeftToolsPanelWidth = panLeft.Width;
            Properties.Settings.Default.Save();
        }

        private void tvModules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tsbCompile.Enabled = 
                tsbShowModuleForm.Enabled = 
                tsbDeleteModule.Enabled = 
                tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Module _;
        }

        private void tsbDeleteModule_Click(object sender, EventArgs e)
        {
            if (tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Module module)
            {
                if (MessageBox.Show("Этот модуль будет удалён безвозвратно! Удалить?",
                    "Удаление текущего модуля", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Host.RemoveModuleChildWindowFromPanels(module);
                }
            }
        }

        private void tsbShowModuleForm_Click(object sender, EventArgs e)
        {
            EnsureShowModuleChildForm();
        }

        public void EnsureShowModuleChildForm(Model.Module? module = null)
        {
            if (module == null)
            {
                if (tvModules.SelectedNode is not TreeNode node) return;
                if (node.Tag is not Model.Module treeModule) return;
                module = treeModule;
            }
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

        private void tsbHorizontalLayout_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tsbCascadeLayout_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tsbCompile_Click(object sender, EventArgs e)
        {
            if (tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Module module)
            {
                var method = module.GetCalculationMethod();
            }
        }
    }
}
