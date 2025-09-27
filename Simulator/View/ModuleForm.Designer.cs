using Simulator.View;

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
            lvVariables = new CustomListView();
            chElement = new ColumnHeader();
            chInput1 = new ColumnHeader();
            chInput2 = new ColumnHeader();
            chInput3 = new ColumnHeader();
            chInput4 = new ColumnHeader();
            chInput5 = new ColumnHeader();
            chInput6 = new ColumnHeader();
            chInput7 = new ColumnHeader();
            chInput8 = new ColumnHeader();
            chOutput1 = new ColumnHeader();
            chOutput2 = new ColumnHeader();
            chOutput3 = new ColumnHeader();
            chOutput4 = new ColumnHeader();
            chOutput5 = new ColumnHeader();
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
            lvVariables.Columns.AddRange(new ColumnHeader[] { chElement, chInput1, chInput2, chInput3, chInput4, chInput5, chInput6, chInput7, chInput8, chOutput1, chOutput2, chOutput3, chOutput4, chOutput5 });
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
            // chElement
            // 
            chElement.Text = "Элемент";
            chElement.Width = 80;
            // 
            // chInput1
            // 
            chInput1.Text = "Вход 1";
            chInput1.TextAlign = HorizontalAlignment.Center;
            chInput1.Width = 56;
            // 
            // chInput2
            // 
            chInput2.Text = "Вход 2";
            chInput2.TextAlign = HorizontalAlignment.Center;
            chInput2.Width = 56;
            // 
            // chInput3
            // 
            chInput3.Text = "Вход 3";
            chInput3.TextAlign = HorizontalAlignment.Center;
            chInput3.Width = 56;
            // 
            // chInput4
            // 
            chInput4.Text = "Вход 4";
            chInput4.TextAlign = HorizontalAlignment.Center;
            chInput4.Width = 56;
            // 
            // chInput5
            // 
            chInput5.Text = "Вход 5";
            chInput5.TextAlign = HorizontalAlignment.Center;
            chInput5.Width = 56;
            // 
            // chInput6
            // 
            chInput6.Text = "Вход 6";
            chInput6.TextAlign = HorizontalAlignment.Center;
            chInput6.Width = 56;
            // 
            // chInput7
            // 
            chInput7.Text = "Вход 7";
            chInput7.TextAlign = HorizontalAlignment.Center;
            chInput7.Width = 56;
            // 
            // chInput8
            // 
            chInput8.Text = "Вход 8";
            chInput8.TextAlign = HorizontalAlignment.Center;
            chInput8.Width = 56;
            // 
            // chOutput1
            // 
            chOutput1.Text = "Выход 1";
            chOutput1.TextAlign = HorizontalAlignment.Center;
            chOutput1.Width = 56;
            // 
            // chOutput2
            // 
            chOutput2.Text = "Выход 2";
            chOutput2.TextAlign = HorizontalAlignment.Center;
            chOutput2.Width = 56;
            // 
            // chOutput3
            // 
            chOutput3.Text = "Выход 3";
            chOutput3.TextAlign = HorizontalAlignment.Center;
            chOutput3.Width = 56;
            // 
            // chOutput4
            // 
            chOutput4.Text = "Выход 4";
            chOutput4.TextAlign = HorizontalAlignment.Center;
            chOutput4.Width = 56;
            // 
            // chOutput5
            // 
            chOutput5.Text = "Выход 5";
            chOutput5.TextAlign = HorizontalAlignment.Center;
            chOutput5.Width = 56;
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
        private CustomListView lvVariables;
        private ColumnHeader chElement;
        private ColumnHeader chInput1;
        private ColumnHeader chInput2;
        private ColumnHeader chInput3;
        private ColumnHeader chInput4;
        private ColumnHeader chInput5;
        private ColumnHeader chInput6;
        private ColumnHeader chInput7;
        private ColumnHeader chInput8;
        private ColumnHeader chOutput1;
        private ColumnHeader chOutput2;
        private ColumnHeader chOutput3;
        private ColumnHeader chOutput4;
        private ColumnHeader chOutput5;
    }
}