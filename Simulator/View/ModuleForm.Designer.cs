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
            contextMenuStrip1 = new ContextMenuStrip(components);
            загрузитьToolStripMenuItem = new ToolStripMenuItem();
            сохранитьToolStripMenuItem = new ToolStripMenuItem();
            toolStripModule = new ToolStrip();
            создатьToolStripButton = new ToolStripButton();
            открытьToolStripButton = new ToolStripButton();
            сохранитьToolStripButton = new ToolStripButton();
            печатьToolStripButton = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            вырезатьToolStripButton = new ToolStripButton();
            копироватьToolStripButton = new ToolStripButton();
            вставитьToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            справкаToolStripButton = new ToolStripButton();
            contextMenuStrip1.SuspendLayout();
            toolStripModule.SuspendLayout();
            SuspendLayout();
            // 
            // zoomPad
            // 
            zoomPad.AllowDrop = true;
            zoomPad.BackColor = Color.FromArgb(64, 64, 64);
            zoomPad.ContextMenuStrip = contextMenuStrip1;
            zoomPad.Dock = DockStyle.Fill;
            zoomPad.ForeColor = SystemColors.Window;
            zoomPad.Location = new Point(0, 25);
            zoomPad.MaxZoom = 20F;
            zoomPad.MinZoom = 0.1F;
            zoomPad.Name = "zoomPad";
            zoomPad.Size = new Size(800, 425);
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
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { загрузитьToolStripMenuItem, сохранитьToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(134, 48);
            // 
            // загрузитьToolStripMenuItem
            // 
            загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            загрузитьToolStripMenuItem.Size = new Size(133, 22);
            загрузитьToolStripMenuItem.Text = "Загрузить";
            загрузитьToolStripMenuItem.Click += загрузитьToolStripMenuItem_Click;
            // 
            // сохранитьToolStripMenuItem
            // 
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            сохранитьToolStripMenuItem.Size = new Size(133, 22);
            сохранитьToolStripMenuItem.Text = "Сохранить";
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            // 
            // toolStripModule
            // 
            toolStripModule.GripStyle = ToolStripGripStyle.Hidden;
            toolStripModule.Items.AddRange(new ToolStripItem[] { создатьToolStripButton, открытьToolStripButton, сохранитьToolStripButton, печатьToolStripButton, toolStripSeparator, вырезатьToolStripButton, копироватьToolStripButton, вставитьToolStripButton, toolStripSeparator1, справкаToolStripButton });
            toolStripModule.Location = new Point(0, 0);
            toolStripModule.Name = "toolStripModule";
            toolStripModule.RenderMode = ToolStripRenderMode.System;
            toolStripModule.Size = new Size(800, 25);
            toolStripModule.TabIndex = 1;
            toolStripModule.Text = "toolStrip1";
            // 
            // создатьToolStripButton
            // 
            создатьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            создатьToolStripButton.Image = (Image)resources.GetObject("создатьToolStripButton.Image");
            создатьToolStripButton.ImageTransparentColor = Color.Magenta;
            создатьToolStripButton.Name = "создатьToolStripButton";
            создатьToolStripButton.Size = new Size(23, 22);
            создатьToolStripButton.Text = "&Создать";
            // 
            // открытьToolStripButton
            // 
            открытьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            открытьToolStripButton.Image = (Image)resources.GetObject("открытьToolStripButton.Image");
            открытьToolStripButton.ImageTransparentColor = Color.Magenta;
            открытьToolStripButton.Name = "открытьToolStripButton";
            открытьToolStripButton.Size = new Size(23, 22);
            открытьToolStripButton.Text = "&Открыть";
            // 
            // сохранитьToolStripButton
            // 
            сохранитьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            сохранитьToolStripButton.Image = (Image)resources.GetObject("сохранитьToolStripButton.Image");
            сохранитьToolStripButton.ImageTransparentColor = Color.Magenta;
            сохранитьToolStripButton.Name = "сохранитьToolStripButton";
            сохранитьToolStripButton.Size = new Size(23, 22);
            сохранитьToolStripButton.Text = "&Сохранить";
            // 
            // печатьToolStripButton
            // 
            печатьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
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
            // вырезатьToolStripButton
            // 
            вырезатьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            вырезатьToolStripButton.Image = (Image)resources.GetObject("вырезатьToolStripButton.Image");
            вырезатьToolStripButton.ImageTransparentColor = Color.Magenta;
            вырезатьToolStripButton.Name = "вырезатьToolStripButton";
            вырезатьToolStripButton.Size = new Size(23, 22);
            вырезатьToolStripButton.Text = "Вы&резать";
            // 
            // копироватьToolStripButton
            // 
            копироватьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            копироватьToolStripButton.Image = (Image)resources.GetObject("копироватьToolStripButton.Image");
            копироватьToolStripButton.ImageTransparentColor = Color.Magenta;
            копироватьToolStripButton.Name = "копироватьToolStripButton";
            копироватьToolStripButton.Size = new Size(23, 22);
            копироватьToolStripButton.Text = "&Копировать";
            // 
            // вставитьToolStripButton
            // 
            вставитьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            вставитьToolStripButton.Image = (Image)resources.GetObject("вставитьToolStripButton.Image");
            вставитьToolStripButton.ImageTransparentColor = Color.Magenta;
            вставитьToolStripButton.Name = "вставитьToolStripButton";
            вставитьToolStripButton.Size = new Size(23, 22);
            вставитьToolStripButton.Text = "&Вставить";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // справкаToolStripButton
            // 
            справкаToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            справкаToolStripButton.Image = (Image)resources.GetObject("справкаToolStripButton.Image");
            справкаToolStripButton.ImageTransparentColor = Color.Magenta;
            справкаToolStripButton.Name = "справкаToolStripButton";
            справкаToolStripButton.Size = new Size(23, 22);
            справкаToolStripButton.Text = "С&правка";
            // 
            // ModuleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(zoomPad);
            Controls.Add(toolStripModule);
            Name = "ModuleForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Дочерняя форма";
            FormClosing += ChildForm_FormClosing;
            Load += ChildForm_Load;
            contextMenuStrip1.ResumeLayout(false);
            toolStripModule.ResumeLayout(false);
            toolStripModule.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private View.ZoomControl zoomPad;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem сохранитьToolStripMenuItem;
        private ToolStripMenuItem загрузитьToolStripMenuItem;
        private ToolStrip toolStripModule;
        private ToolStripButton создатьToolStripButton;
        private ToolStripButton открытьToolStripButton;
        private ToolStripButton сохранитьToolStripButton;
        private ToolStripButton печатьToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton вырезатьToolStripButton;
        private ToolStripButton копироватьToolStripButton;
        private ToolStripButton вставитьToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton справкаToolStripButton;
    }
}