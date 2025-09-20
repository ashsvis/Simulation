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
            tsmiAddModule = new ToolStripButton();
            tsbDeleteModule = new ToolStripButton();
            tsbSave = new ToolStripButton();
            печатьToolStripButton = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            tsbCut = new ToolStripButton();
            tsbCopy = new ToolStripButton();
            tsbPaste = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            справкаToolStripButton = new ToolStripButton();
            timerInterface = new System.Windows.Forms.Timer(components);
            toolStripModule.SuspendLayout();
            SuspendLayout();
            // 
            // zoomPad
            // 
            zoomPad.AllowDrop = true;
            zoomPad.BackColor = Color.FromArgb(64, 64, 64);
            zoomPad.ContextMenuStrip = cmsContextMenu;
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
            // cmsContextMenu
            // 
            cmsContextMenu.Name = "contextMenuStrip1";
            cmsContextMenu.Size = new Size(61, 4);
            // 
            // toolStripModule
            // 
            toolStripModule.GripStyle = ToolStripGripStyle.Hidden;
            toolStripModule.Items.AddRange(new ToolStripItem[] { tsmiAddModule, tsbDeleteModule, tsbSave, печатьToolStripButton, toolStripSeparator, tsbCut, tsbCopy, tsbPaste, toolStripSeparator1, справкаToolStripButton });
            toolStripModule.Location = new Point(0, 0);
            toolStripModule.Name = "toolStripModule";
            toolStripModule.RenderMode = ToolStripRenderMode.System;
            toolStripModule.Size = new Size(800, 25);
            toolStripModule.TabIndex = 1;
            toolStripModule.Text = "toolStrip1";
            // 
            // tsmiAddModule
            // 
            tsmiAddModule.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsmiAddModule.Image = (Image)resources.GetObject("tsmiAddModule.Image");
            tsmiAddModule.ImageTransparentColor = Color.Magenta;
            tsmiAddModule.Name = "tsmiAddModule";
            tsmiAddModule.Size = new Size(23, 22);
            tsmiAddModule.Text = "&Добавить модуль";
            tsmiAddModule.ToolTipText = "Добавить модуль в проект";
            tsmiAddModule.Click += tsmiAddModule_Click;
            // 
            // tsbDeleteModule
            // 
            tsbDeleteModule.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDeleteModule.Image = (Image)resources.GetObject("tsbDeleteModule.Image");
            tsbDeleteModule.ImageTransparentColor = Color.Magenta;
            tsbDeleteModule.Name = "tsbDeleteModule";
            tsbDeleteModule.Size = new Size(23, 22);
            tsbDeleteModule.Text = "&Удалить этот модуль";
            tsbDeleteModule.ToolTipText = "Удалить этот модуль из проекта";
            tsbDeleteModule.Click += tsbDeleteModule_Click;
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
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
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
            // timerInterface
            // 
            timerInterface.Tick += timerInterface_Tick;
            // 
            // ModuleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(zoomPad);
            Controls.Add(toolStripModule);
            KeyPreview = true;
            Name = "ModuleForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Форма модуля";
            FormClosing += ModuleForm_FormClosing;
            Load += ModuleForm_Load;
            KeyDown += ModuleForm_KeyDown;
            toolStripModule.ResumeLayout(false);
            toolStripModule.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private View.ZoomControl zoomPad;
        private ContextMenuStrip cmsContextMenu;
        private ToolStrip toolStripModule;
        private ToolStripButton tsmiAddModule;
        private ToolStripButton tsbDeleteModule;
        private ToolStripButton tsbSave;
        private ToolStripButton печатьToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton tsbCut;
        private ToolStripButton tsbCopy;
        private ToolStripButton tsbPaste;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton справкаToolStripButton;
        private System.Windows.Forms.Timer timerInterface;
    }
}