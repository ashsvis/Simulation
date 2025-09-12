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
            // восстановление состояния окна в начале сеанса работы
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
            // сохранение состояния окна в конце сеанса работы
            //Properties.Settings.Default.MainFormMaximized = WindowState == FormWindowState.Maximized;
            //WindowState = FormWindowState.Normal;
            //Properties.Settings.Default.MainFormLocation = Location;
            //Properties.Settings.Default.MainFormSize = Size;
            //Properties.Settings.Default.Save();
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
                var result = MessageBox.Show("Текущие изменения не сохранены! Записать?", "Создание нового проекта", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    Project.Save();
                if (result == DialogResult.Cancel)
                    return;
            }
            Project.Clear();
            AddModuleToProject();
        }

        private void AddModuleToProject()
        {
            var module = new Module();
            Project.Modules.Add(module);

            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());

            var node = tvModules.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == module);
            if (node != null)
            {
                tvModules.SelectedNode = node;
                node.EnsureVisible();
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
        /// Обновление состояния элементов интерфейса формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerInterface_Tick(object sender, EventArgs e)
        {
            окноToolStripMenuItem.Visible = MdiChildren.Length > 0;
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

        /// <summary>
        /// Таймер моделирования (100 мс)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSimulation_Tick(object sender, EventArgs e)
        {
            SimulationTick?.Invoke(this, EventArgs.Empty);
            //if (!pgProps.Focused) pgProps.Refresh();
            tsmiSave.Enabled = Project.Modules.Any(x => x.Changed);
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
                MdiChildren.ToList().ForEach(x => x.Close());
                Project.Load(dlg.FileName);
                tvModules.Nodes.Clear();
                tvModules.Nodes.AddRange(Project.GetModulesTree());
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
    }
}
