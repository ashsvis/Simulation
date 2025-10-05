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
            cmZoomPad = new ContextMenuStrip(components);
            toolStripSeparator1 = new ToolStripSeparator();
            timerInterface = new System.Windows.Forms.Timer(components);
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
            SuspendLayout();
            // 
            // cmZoomPad
            // 
            cmZoomPad.Name = "contextMenuStrip1";
            cmZoomPad.Size = new Size(61, 4);
            cmZoomPad.Opening += cmZoomPad_Opening;
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
            chInput2.Width = 0;
            // 
            // chInput3
            // 
            chInput3.Text = "Вход 3";
            chInput3.TextAlign = HorizontalAlignment.Center;
            chInput3.Width = 0;
            // 
            // chInput4
            // 
            chInput4.Text = "Вход 4";
            chInput4.TextAlign = HorizontalAlignment.Center;
            chInput4.Width = 0;
            // 
            // chInput5
            // 
            chInput5.Text = "Вход 5";
            chInput5.TextAlign = HorizontalAlignment.Center;
            chInput5.Width = 0;
            // 
            // chInput6
            // 
            chInput6.Text = "Вход 6";
            chInput6.TextAlign = HorizontalAlignment.Center;
            chInput6.Width = 0;
            // 
            // chInput7
            // 
            chInput7.Text = "Вход 7";
            chInput7.TextAlign = HorizontalAlignment.Center;
            chInput7.Width = 0;
            // 
            // chInput8
            // 
            chInput8.Text = "Вход 8";
            chInput8.TextAlign = HorizontalAlignment.Center;
            chInput8.Width = 0;
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
            chOutput2.Width = 0;
            // 
            // chOutput3
            // 
            chOutput3.Text = "Выход 3";
            chOutput3.TextAlign = HorizontalAlignment.Center;
            chOutput3.Width = 0;
            // 
            // chOutput4
            // 
            chOutput4.Text = "Выход 4";
            chOutput4.TextAlign = HorizontalAlignment.Center;
            chOutput4.Width = 0;
            // 
            // chOutput5
            // 
            chOutput5.Text = "Выход 5";
            chOutput5.TextAlign = HorizontalAlignment.Center;
            chOutput5.Width = 0;
            // 
            // ModuleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            KeyPreview = true;
            Name = "ModuleForm";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "Форма модуля";
            FormClosing += ModuleForm_FormClosing;
            Load += ModuleForm_Load;
            KeyDown += ModuleForm_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip cmZoomPad;
        private ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Timer timerInterface;
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