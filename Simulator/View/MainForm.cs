using Simulator.Model;
using Simulator.View;

namespace Simulator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // восстановление состояния окна в начале сеанса работы
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
            tvLibrary.Nodes.Clear();
            var rootNode = new TreeNode("Библиотека");
            tvLibrary.Nodes.Add(rootNode);
            var logicaNode = new TreeNode("Логика");
            logicaNode.Nodes.Add(new TreeNode("NOT") { Tag = typeof(Model.Logic.NOT) });
            logicaNode.Nodes.Add(new TreeNode("AND") { Tag = typeof(Model.Logic.AND) });
            logicaNode.Nodes.Add(new TreeNode("OR") { Tag = typeof(Model.Logic.OR) });
            logicaNode.Nodes.Add(new TreeNode("XOR") { Tag = typeof(Model.Logic.XOR) });
            logicaNode.Nodes.Add(new TreeNode("Логика") { Tag = typeof(Model.Logic.LogicFunction) });
            rootNode.Nodes.Add(logicaNode);
            var triggerNode = new TreeNode("Триггеры");
            rootNode.Nodes.Add(triggerNode);
            triggerNode.Nodes.Add(new TreeNode("RS-триггер") { Tag = typeof(Model.Trigger.RS) });
            rootNode.ExpandAll();

            timerInterface.Enabled = true;
            timerSimulation.Enabled = true;

            CreateNewChildForm();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            // сохранение состояния окна в конце сеанса работы
            Properties.Settings.Default.MainFormMaximized = WindowState == FormWindowState.Maximized;
            WindowState = FormWindowState.Normal;
            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MainFormSize = Size;
            Properties.Settings.Default.Save();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewChildForm();
        }

        private void CreateNewChildForm()
        {
            var childForm = new ChildForm(this) { MdiParent = this, WindowState = FormWindowState.Maximized };
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
        }
    }
}
