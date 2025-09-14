namespace Simulator
{
    partial class HostForm
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
            timerSimulation = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // timerSimulation
            // 
            timerSimulation.Tick += timerSimulation_Tick;
            // 
            // HostForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(193, 37);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HostForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "RootForm";
            WindowState = FormWindowState.Minimized;
            FormClosing += RootForm_FormClosing;
            Load += RootForm_Load;
            Resize += RootForm_Resize;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timerSimulation;
    }
}