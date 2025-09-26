namespace Simulator
{
    partial class ModuleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModuleForm));
            zoomPad = new Simulator.View.ZoomControl();
            cmsContextMenu = new ContextMenuStrip(components);
            toolStripModule = new ToolStrip();
            tsbSave = new ToolStripButton();
            печатьToolStripButton = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            tsbCut = new ToolStripButton();
            tsbCopy = new ToolStripButton();
            tsbPaste = new ToolStripButton();
            справкаToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            timerInterface = new System.Windows.Forms.Timer(components);
            splitContainer1 = new SplitContainer();
            lvVariables = new ListView();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            tableLayoutPanel1 = new TableLayoutPanel();
            toolStripModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // zoomPad
            // 
            zoomPad.AllowDrop = true;
            zoomPad.BackColor = Color.FromArgb(64, 64, 64);
            zoomPad.ContextMenuStrip = cmsContextMenu;
            zoomPad.Dock = DockStyle.Fill;
            zoomPad.ForeColor = SystemColors.Window;
            zoomPad.Location = new Point(0, 0);
            zoomPad.MaxZoom = 20F;
            zoomPad.MinZoom = 0.1F;
            zoomPad.Name = "zoomPad";
            zoomPad.Size = new Size(794, 289);
            zoomPad.TabIndex = 0;
            zoomPad.ZoomScale = 1D;
            zoomPad.ZoomSensitivity = 0.2F;
            zoomPad.OnDraw += zoomPad_OnDraw;
            zoomPad.DragDrop += zoomPad_DragDrop;
            zoomPad.DragEnter += zoomPad_DragEnter;
            zoomPad.DragOver += zoomPad_DragOver;
            zoomPad.MouseDown += zoomPad_MouseDown;
            zoomPad.MouseMove += zoomPad_MouseMove;
            zoomPad.MouseUp += zoomPad_MouseUp;
            // 
            // cmsContextMenu
            // 
            cmsContextMenu.Name = "contextMenuStrip1";
            cmsContextMenu.Size = new Size(61, 4);
            // 
            // toolStripModule
            // 
            toolStripModule.GripStyle = ToolStripGripStyle.Hidden;
            toolStripModule.Items.AddRange(new ToolStripItem[] { tsbSave, печатьToolStripButton, toolStripSeparator, tsbCut, tsbCopy, tsbPaste, справкаToolStripButton });
            toolStripModule.Location = new Point(0, 0);
            toolStripModule.Name = "toolStripModule";
            toolStripModule.RenderMode = ToolStripRenderMode.System;
            toolStripModule.Size = new Size(800, 25);
            toolStripModule.TabIndex = 1;
            toolStripModule.Text = "toolStrip1";
            // 
            // tsbSave
            // 
            tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSave.Enabled = false;
            tsbSave.Image = (Image)resources.GetObject("tsbSave.Image");
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(23, 22);
            tsbSave.Text = "&Сохранить";
            tsbSave.Click += tsbSave_Click;
            // 
            // печатьToolStripButton
            // 
            печатьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            печатьToolStripButton.Enabled = false;
            печатьToolStripButton.Image = (Image)resources.GetObject("печатьToolStripButton.Image");
            печатьToolStripButton.ImageTransparentColor = Color.Magenta;
            печатьToolStripButton.Name = "печатьToolStripButton";
            печатьToolStripButton.Size = new Size(23, 22);
            печатьToolStripButton.Text = "&Печать";
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 25);
            // 
            // tsbCut
            // 
            tsbCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCut.Enabled = false;
            tsbCut.Image = (Image)resources.GetObject("tsbCut.Image");
            tsbCut.ImageTransparentColor = Color.Magenta;
            tsbCut.Name = "tsbCut";
            tsbCut.Size = new Size(23, 22);
            tsbCut.Text = "Вы&резать";
            tsbCut.Click += tsbCut_Click;
            // 
            // tsbCopy
            // 
            tsbCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCopy.Enabled = false;
            tsbCopy.Image = (Image)resources.GetObject("tsbCopy.Image");
            tsbCopy.ImageTransparentColor = Color.Magenta;
            tsbCopy.Name = "tsbCopy";
            tsbCopy.Size = new Size(23, 22);
            tsbCopy.Text = "&Копировать";
            tsbCopy.Click += tsbCopy_Click;
            // 
            // tsbPaste
            // 
            tsbPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbPaste.Enabled = false;
            tsbPaste.Image = (Image)resources.GetObject("tsbPaste.Image");
            tsbPaste.ImageTransparentColor = Color.Magenta;
            tsbPaste.Name = "tsbPaste";
            tsbPaste.Size = new Size(23, 22);
            tsbPaste.Text = "&Вставить";
            tsbPaste.Click += tsbPaste_Click;
            // 
            // справкаToolStripButton
            // 
            справкаToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            справкаToolStripButton.Enabled = false;
            справкаToolStripButton.Image = (Image)resources.GetObject("справкаToolStripButton.Image");
            справкаToolStripButton.ImageTransparentColor = Color.Magenta;
            справкаToolStripButton.Name = "справкаToolStripButton";
            справкаToolStripButton.Size = new Size(23, 22);
            справкаToolStripButton.Text = "С&правка";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // timerInterface
            // 
            timerInterface.Tick += timerInterface_Tick;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(3, 28);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(zoomPad);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(lvVariables);
            splitContainer1.Size = new Size(794, 419);
            splitContainer1.SplitterDistance = 289;
            splitContainer1.TabIndex = 2;
            // 
            // lvVariables
            // 
            lvVariables.Columns.AddRange(new ColumnHeader[] { columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
            lvVariables.Dock = DockStyle.Fill;
            lvVariables.FullRowSelect = true;
            lvVariables.GridLines = true;
            lvVariables.Location = new Point(0, 0);
            lvVariables.Name = "lvVariables";
            lvVariables.Size = new Size(794, 126);
            lvVariables.TabIndex = 11;
            lvVariables.UseCompatibleStateImageBehavior = false;
            lvVariables.View = System.Windows.Forms.View.Details;
            lvVariables.VirtualMode = true;
            lvVariables.RetrieveVirtualItem += lvVariables_RetrieveVirtualItem;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Элемент";
            columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Направление";
            columnHeader3.TextAlign = HorizontalAlignment.Center;
            columnHeader3.Width = 90;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Индекс";
            columnHeader4.TextAlign = HorizontalAlignment.Center;
            columnHeader4.Width = 55;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Тип";
            columnHeader5.TextAlign = HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Значение";
            columnHeader6.TextAlign = HorizontalAlignment.Center;
            columnHeader6.Width = 70;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(toolStripModule, 0, 0);
            tableLayoutPanel1.Controls.Add(splitContainer1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // ModuleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            KeyPreview = true;
            Name = "ModuleForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Форма модуля";
            FormClosing += ModuleForm_FormClosing;
            Load += ModuleForm_Load;
            KeyDown += ModuleForm_KeyDown;
            toolStripModule.ResumeLayout(false);
            toolStripModule.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private View.ZoomControl zoomPad;
        private ContextMenuStrip cmsContextMenu;
        private ToolStrip toolStripModule;
        private ToolStripButton tsbSave;
        private ToolStripButton печатьToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton tsbCut;
        private ToolStripButton tsbCopy;
        private ToolStripButton tsbPaste;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton справкаToolStripButton;
        private System.Windows.Forms.Timer timerInterface;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel1;
        private ListView lvVariables;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
    }
}