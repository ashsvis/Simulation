using Simulator.Model;
using Simulator.View;

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
            tvEquipment.Nodes.Clear();
            tvEquipment.Nodes.AddRange(Project.GetEquipmentTree());
            if (e.ChangeKind == ProjectChangeKind.Clear || e.ChangeKind == ProjectChangeKind.Load)
            {
                pgProps.SelectedObject = null;
                MdiChildren.ToList().ForEach(x => x.Close());
            }
        }

        private void PanelForm_Load(object sender, EventArgs e)
        {
            panLeft.Width = Properties.Settings.Default.LeftToolsPanelVisible ? Properties.Settings.Default.LeftToolsPanelWidth : 0;
            panRight.Width = Properties.Settings.Default.RightToolsPanelVisible ? Properties.Settings.Default.RightToolsPanelWidth : 0;

            tvLibrary.Nodes.Clear();
            tvLibrary.Nodes.AddRange(Project.GetLibraryTree());

            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());

            tvEquipment.Nodes.Clear();
            tvEquipment.Nodes.AddRange(Project.GetEquipmentTree());

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

            tvLibrary.Nodes.Clear();
            tvLibrary.Nodes.AddRange(Project.GetLibraryTree());

            tvModules.Nodes.Clear();
            tvModules.Nodes.AddRange(Project.GetModulesTree());

            tvEquipment.Nodes.Clear();
            tvEquipment.Nodes.AddRange(Project.GetEquipmentTree());
        }

        public void AddModuleToProject(Module? newModule = null)
        {
            var module = Project.AddModuleToProject(newModule);

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

        public void AddUnitToProject(Unit? newUnit = null)
        {
            var unit = Project.AddUnitToProject(newUnit);

            tvEquipment.Nodes.Clear();
            tvEquipment.Nodes.AddRange(Project.GetEquipmentTree());

            var node = tvEquipment.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == unit);
            if (node != null)
            {
                tvEquipment.SelectedNode = node;
                node.EnsureVisible();
                pgProps.SelectedObject = unit;
            }

            CreateNewModuleForm(unit);
        }

        private void tsmiAddModule_Click(object sender, EventArgs e)
        {
            switch (tcTools.SelectedIndex)
            {
                case 0:
                    AddModuleToProject();
                    break;
                case 1:
                    AddUnitToProject();
                    break;
            }
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
            pgProps.SelectedObjects = null;
        }

        private void ChildForm_ElementSelected(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                pgProps.SelectedObject = null;
                pgProps.SelectedObjects = null;
            }
            else if (sender is object?[])
                pgProps.SelectedObjects = (object[])sender;
            else
            {
                pgProps.SelectedObjects = null;
                pgProps.SelectedObject = sender;
            }
        }

        /// <summary>
        /// Обновление состояния элементов интерфейса формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerInterface_Tick(object sender, EventArgs e)
        {
            tsbSave.Enabled = tsmiSave.Enabled = Project.Changed || Project.Modules.Any(x => x.Changed);
            окноToolStripMenuItem.Visible = MdiChildren.Length > 0;
            switch (tcTools.SelectedIndex)
            {
                case 0:
                    tsbShowModuleForm.Enabled =
                    tsbDeleteModule.Enabled =
                    tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Module _;
                    break;
                case 1:
                    tsbShowModuleForm.Enabled =
                    tsbDeleteModule.Enabled =
                    tvEquipment.SelectedNode != null && tvEquipment.SelectedNode.Tag is Model.Unit _;
                    break;
            }
            tsmiLeftPanelVisible.Checked = panLeft.Width > 0;
            tsmiRightPanelVisible.Checked = panRight.Width > 0;
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
                        MessageBox.Show(ex.Message, "Вставка элемента", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (node.Tag is Model.Unit block)
                {
                    try
                    {
                        var module = new Element() { Type = typeof(Model.Logic.BLK) };
                        if (module != null)
                        {
                            module.Instance = new Model.Logic.BLK(block.Id);
                            var ret = tvLibrary.DoDragDrop(module, DragDropEffects.Copy);
                            if (ret == DragDropEffects.None)
                            {
                                Cursor = Cursors.Default;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Вставка блока", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (e.Clicks > 1)
                    EnsureShowBlockChildForm();
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
            var notSaved = Project.Modules.Any(x => x.Changed) || Project.Equipment.Any(x => x.Changed);
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

                tvLibrary.Nodes.Clear();
                tvLibrary.Nodes.AddRange(Project.GetLibraryTree());

                tvModules.Nodes.Clear();
                tvModules.Nodes.AddRange(Project.GetModulesTree());

                tvEquipment.Nodes.Clear();
                tvEquipment.Nodes.AddRange(Project.GetEquipmentTree());
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
            var form = MdiChildren.OfType<ModuleForm>().FirstOrDefault(x => x.Module.Id == module.Id);
            form?.Close();
        }

        private void tvModules_MouseDown(object sender, MouseEventArgs e)
        {
            var node = tvModules.GetNodeAt(e.X, e.Y);
            if (node != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    tvModules.SelectedNode = null;
                    tvModules.SelectedNode = node;
                    if (node.Tag is Model.Module module)
                    {
                        pgProps.SelectedObject = module;
                        if (e.Clicks > 1)
                            EnsureShowModuleChildForm(module);
                    }
                    return;
                }
            }
            tvModules.SelectedNode = node;
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

        private void tsbDeleteModule_Click(object sender, EventArgs e)
        {
            switch (tcTools.SelectedIndex)
            {
                case 0:
                    if (tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Module module)
                    {
                        if (MessageBox.Show("Эта задача будет удалена безвозвратно! Удалить?",
                            "Удаление текущей задачи", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            Project.RemoveModuleFromProject(module);
                            Host.RemoveModuleChildWindowFromPanels(module);
                        }
                    }
                    break;
                case 1:
                    if (tvEquipment.SelectedNode != null && tvEquipment.SelectedNode.Tag is Model.Unit unit)
                    {
                        if (MessageBox.Show("Это оборудование будет удалено безвозвратно! Удалить?",
                            "Удаление текущего оборудования", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            Project.RemoveUnitFromProject(unit);
                            Host.RemoveModuleChildWindowFromPanels(unit);
                        }
                    }
                    break;
            }
        }

        private void tsbShowModuleForm_Click(object sender, EventArgs e)
        {
            switch (tcTools.SelectedIndex)
            {
                case 0:
                    if (tvModules.SelectedNode is not TreeNode node) return;
                    if (node.Tag is not Model.Module treeModule) return;
                    EnsureShowModuleChildForm(treeModule);
                    break;
                case 1:
                    if (tvEquipment.SelectedNode is not TreeNode node1) return;
                    if (node1.Tag is not Model.Unit treeModule1) return;
                    EnsureShowModuleChildForm(treeModule1);
                    break;
            }
        }

        public void EnsureShowModuleChildForm(Model.Module? module)
        {
            pgProps.SelectedObject = module;
            var form = MdiChildren.OfType<ModuleForm>().FirstOrDefault(frm => frm.Module == module);
            if (form != null)
            {
                form.WindowState = FormWindowState.Maximized;
                form.BringToFront();
                form.Show();
            }
            else if (module != null)
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

        private void tvLibrary_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvLibrary.SelectedNode != null && tvLibrary.SelectedNode.Tag is Model.Unit block)
            {
                tsbShowBlockForm.Enabled = true;
                tsbDeleteBlock.Enabled = true;
            }
            else
            {
                tsbShowBlockForm.Enabled = false;
                tsbDeleteBlock.Enabled = false;
            }
        }

        public void AddBlockToProject()
        {
            var module = Project.AddBlockToProject();

            var blocksNode = tvLibrary.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == Project.Blocks);
            blocksNode?.Nodes.Clear();
            foreach (var block in Project.Blocks)
                blocksNode?.Nodes.Add(new TreeNode(block.Name) { Tag = block });
            var node = blocksNode?.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Tag == module);
            if (node != null)
            {
                tvLibrary.SelectedNode = node;
                node.EnsureVisible();
                pgProps.SelectedObject = module;
            }

            CreateNewModuleForm(module);
        }

        private void tsbAddBock_Click(object sender, EventArgs e)
        {
            AddBlockToProject();
        }

        private void tsbShowBlockForm_Click(object sender, EventArgs e)
        {
            EnsureShowBlockChildForm();
        }

        public void EnsureShowBlockChildForm(Model.Module? module = null)
        {
            if (module == null)
            {
                if (tvLibrary.SelectedNode is not TreeNode node) return;
                if (node.Tag is not Model.Unit treeModule) return;
                module = treeModule;
            }
            pgProps.SelectedObject = module;
            var form = MdiChildren.OfType<ModuleForm>().FirstOrDefault(x => x.Module.Id == module.Id);
            if (form != null)
            {
                form.WindowState = FormWindowState.Maximized;
                form.BringToFront();
                form.Show();
            }
            else
                CreateNewModuleForm(module);
        }

        private void tsbDeleteBlock_Click(object sender, EventArgs e)
        {
            if (tvLibrary.SelectedNode != null && tvLibrary.SelectedNode.Tag is Model.Unit module)
            {
                if (MessageBox.Show("Этот блок будет удалён безвозвратно! Удалить?",
                    "Удаление текущего блока", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Project.RemoveBlockFromProject(module);
                    Host.RemoveModuleChildWindowFromPanels(module);
                    tvLibrary.SelectedNode.Remove();
                }
            }

        }

        private void cmModules_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var isLinkToModule = tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Unit;
            tsmiModuleDublicate.Visible = isLinkToModule;
            tsmiRenameModule.Visible = isLinkToModule;
            e.Cancel = !isLinkToModule;
        }

        private void tsmiModuleDublicate_Click(object sender, EventArgs e)
        {
            if (tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Unit module)
            {
                var dlg = new ChangeNameDialog(module.Name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(dlg.EnteredValue) && !Project.Modules.Any(x => x.Name.Equals(dlg.EnteredValue)))
                    {
                        var dublicate = Module.MakeDuplicate(module);
                        dublicate.Name = dlg.EnteredValue;
                        AddModuleToProject(dublicate);
                        dublicate.Changed = true;
                    }
                    else
                        MessageBox.Show("Уже есть такое имя задачи!", "Копия задачи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmiRenameModule_Click(object sender, EventArgs e)
        {
            if (tvModules.SelectedNode != null && tvModules.SelectedNode.Tag is Model.Unit module)
            {
                var dlg = new ChangeNameDialog(module.Name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(dlg.EnteredValue) && !Project.Modules.Any(x => x.Name.Equals(dlg.EnteredValue)))
                    {
                        tvModules.SelectedNode.Text = dlg.EnteredValue;
                        module.Name = dlg.EnteredValue;
                        Project.Changed = true;
                    }
                    else
                        MessageBox.Show("Уже есть такое имя задачи!", "Переименование задачи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmiRenameUnit_Click(object sender, EventArgs e)
        {
            if (tvEquipment.SelectedNode != null && tvEquipment.SelectedNode.Tag is Model.Unit module)
            {
                var dlg = new ChangeNameDialog(module.Name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(dlg.EnteredValue) && !Project.Equipment.Any(x => x.Name.Equals(dlg.EnteredValue)))
                    {
                        tvEquipment.SelectedNode.Text = dlg.EnteredValue;
                        module.Name = dlg.EnteredValue;
                        Project.Changed = true;
                    }
                    else
                        MessageBox.Show("Уже есть такое имя оборудования!", "Переименование оборудования", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmiUnitDublicate_Click(object sender, EventArgs e)
        {
            if (tvEquipment.SelectedNode != null && tvEquipment.SelectedNode.Tag is Model.Unit unit)
            {
                var dlg = new ChangeNameDialog(unit.Name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(dlg.EnteredValue) && !Project.Equipment.Any(x => x.Name.Equals(dlg.EnteredValue)))
                    {
                        var dublicate = Unit.MakeDuplicate(unit);
                        dublicate.Name = dlg.EnteredValue;
                        AddUnitToProject(dublicate);
                        dublicate.Changed = true;
                    }
                    else
                        MessageBox.Show("Уже есть такое имя оборудования!", "Копия оборудования", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmEquipment_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var isLinkToUnit = tvEquipment.SelectedNode != null && tvEquipment.SelectedNode.Tag is Model.Unit;
            tsmiUnitDublicate.Visible = isLinkToUnit;
            tsmiRenameUnit.Visible = isLinkToUnit;
            e.Cancel = !isLinkToUnit;
        }

        private void tvEquipment_MouseDown(object sender, MouseEventArgs e)
        {
            var node = tvEquipment.GetNodeAt(e.X, e.Y);
            if (node != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    tvEquipment.SelectedNode = null;
                    tvEquipment.SelectedNode = node;
                    if (node.Tag is Model.Unit unit)
                    {
                        pgProps.SelectedObject = unit;
                        if (e.Clicks > 1)
                            EnsureShowModuleChildForm(unit);
                    }
                    return;
                }
            }
            tvEquipment.SelectedNode = node;

        }

        private void tsbRun_Click(object sender, EventArgs e)
        {
            if (tsbRun.Checked)
            {
                tsbRun.Checked = false;
                Project.Stop();
                MdiChildren.OfType<ModuleForm>().ToList().ForEach(x => x.Refresh());
            }
            else
            {
                tsbRun.Checked = true;
                Project.Start();
            }
        }

        private void tsmiLeftPanelVisible_Click(object sender, EventArgs e)
        {
            tsmiLeftPanelVisible.Checked = !tsmiLeftPanelVisible.Checked;
            Properties.Settings.Default.LeftToolsPanelVisible = tsmiLeftPanelVisible.Checked;
            Properties.Settings.Default.Save();
            panLeft.Width = Properties.Settings.Default.LeftToolsPanelVisible ? Properties.Settings.Default.LeftToolsPanelWidth : 0;
        }

        private void tsmiRightPanelVisible_Click(object sender, EventArgs e)
        {
            tsmiRightPanelVisible.Checked = !tsmiRightPanelVisible.Checked;
            Properties.Settings.Default.RightToolsPanelVisible = tsmiRightPanelVisible.Checked;
            Properties.Settings.Default.Save();
            panRight.Width = Properties.Settings.Default.RightToolsPanelVisible ? Properties.Settings.Default.RightToolsPanelWidth : 0;
        }
    }
}
