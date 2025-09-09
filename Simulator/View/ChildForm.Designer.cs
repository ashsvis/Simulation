namespace Simulator
{
    partial class ChildForm
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
            zoomPad = new Simulator.View.ZoomControl();
            contextMenuStrip1 = new ContextMenuStrip(components);
            сохранитьToolStripMenuItem = new ToolStripMenuItem();
            загрузитьToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // zoomPad
            // 
            zoomPad.AllowDrop = true;
            zoomPad.BackColor = SystemColors.ControlDarkDark;
            zoomPad.ContextMenuStrip = contextMenuStrip1;
            zoomPad.Dock = DockStyle.Fill;
            zoomPad.ForeColor = SystemColors.Window;
            zoomPad.Location = new Point(0, 0);
            zoomPad.MaxZoom = 20F;
            zoomPad.MinZoom = 0.1F;
            zoomPad.Name = "zoomPad";
            zoomPad.Size = new Size(800, 450);
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
            contextMenuStrip1.Size = new Size(181, 70);
            // 
            // сохранитьToolStripMenuItem
            // 
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            сохранитьToolStripMenuItem.Size = new Size(180, 22);
            сохранитьToolStripMenuItem.Text = "Сохранить";
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            // 
            // загрузитьToolStripMenuItem
            // 
            загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            загрузитьToolStripMenuItem.Size = new Size(180, 22);
            загрузитьToolStripMenuItem.Text = "Загрузить";
            загрузитьToolStripMenuItem.Click += загрузитьToolStripMenuItem_Click;
            // 
            // ChildForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(zoomPad);
            Name = "ChildForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Дочерняя форма";
            FormClosing += ChildForm_FormClosing;
            Load += ChildForm_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private View.ZoomControl zoomPad;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem сохранитьToolStripMenuItem;
        private ToolStripMenuItem загрузитьToolStripMenuItem;
    }
}