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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tvLibrary.Nodes.Clear();
            var rootNode = new TreeNode("����������");
            tvLibrary.Nodes.Add(rootNode);
            var logicaNode = new TreeNode("������");
            logicaNode.Nodes.Add(new TreeNode("NOT") { Tag = typeof(Model.Logic.NOT) });
            var andNode = new TreeNode("AND") { Tag = typeof(Model.Logic.AND) };
            logicaNode.Nodes.Add(andNode);
            andNode.Nodes.Add(new TreeNode("AND3") { Tag = typeof(Model.Logic.AND3) });
            andNode.Nodes.Add(new TreeNode("AND4") { Tag = typeof(Model.Logic.AND4) });
            andNode.Nodes.Add(new TreeNode("AND5") { Tag = typeof(Model.Logic.AND5) });
            andNode.Nodes.Add(new TreeNode("AND6") { Tag = typeof(Model.Logic.AND6) });
            andNode.Nodes.Add(new TreeNode("AND7") { Tag = typeof(Model.Logic.AND7) });
            andNode.Nodes.Add(new TreeNode("AND8") { Tag = typeof(Model.Logic.AND8) });
            var orNode = new TreeNode("OR") { Tag = typeof(Model.Logic.OR) };
            logicaNode.Nodes.Add(orNode);
            orNode.Nodes.Add(new TreeNode("OR3") { Tag = typeof(Model.Logic.OR3) });
            orNode.Nodes.Add(new TreeNode("OR4") { Tag = typeof(Model.Logic.OR4) });
            orNode.Nodes.Add(new TreeNode("OR5") { Tag = typeof(Model.Logic.OR5) });
            orNode.Nodes.Add(new TreeNode("OR6") { Tag = typeof(Model.Logic.OR6) });
            orNode.Nodes.Add(new TreeNode("OR7") { Tag = typeof(Model.Logic.OR7) });
            orNode.Nodes.Add(new TreeNode("OR8") { Tag = typeof(Model.Logic.OR8) });
            var xorNode = new TreeNode("XOR") { Tag = typeof(Model.Logic.XOR) };
            logicaNode.Nodes.Add(xorNode);
            xorNode.Nodes.Add(new TreeNode("XOR3") { Tag = typeof(Model.Logic.XOR3) });
            xorNode.Nodes.Add(new TreeNode("XOR4") { Tag = typeof(Model.Logic.XOR4) });
            xorNode.Nodes.Add(new TreeNode("XOR5") { Tag = typeof(Model.Logic.XOR5) });
            xorNode.Nodes.Add(new TreeNode("XOR6") { Tag = typeof(Model.Logic.XOR6) });
            xorNode.Nodes.Add(new TreeNode("XOR7") { Tag = typeof(Model.Logic.XOR7) });
            xorNode.Nodes.Add(new TreeNode("XOR8") { Tag = typeof(Model.Logic.XOR8) });
            rootNode.Nodes.Add(logicaNode);
            var triggerNode = new TreeNode("��������");
            rootNode.Nodes.Add(triggerNode);
            triggerNode.Nodes.Add(new TreeNode("RS-�������") { Tag = typeof(Model.Trigger.RS) });
            var generatorNode = new TreeNode("����������");
            rootNode.Nodes.Add(generatorNode);
            generatorNode.Nodes.Add(new TreeNode("������������") { Tag = typeof(Model.Generator.PULSE) });
            generatorNode.Nodes.Add(new TreeNode("�������� ������") { Tag = typeof(Model.Generator.ONDLY) });
            generatorNode.Nodes.Add(new TreeNode("�������� �����") { Tag = typeof(Model.Generator.OFFDLY) });
            rootNode.ExpandAll();
            andNode.Collapse();
            orNode.Collapse();
            xorNode.Collapse();

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

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewChildForm();
        }

        private void CreateNewChildForm()
        {
            var childForm = new ModuleForm(this) { MdiParent = this, WindowState = FormWindowState.Maximized };
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
            Host.Save();
        }
    }
}
