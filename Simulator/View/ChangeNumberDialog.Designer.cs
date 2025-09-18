namespace Simulator.View
{
    partial class ChangeNumberDialog
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
            tbValue = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // tbValue
            // 
            tbValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbValue.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tbValue.Location = new Point(12, 12);
            tbValue.Name = "tbValue";
            tbValue.Size = new Size(241, 23);
            tbValue.TabIndex = 0;
            tbValue.Text = "0";
            tbValue.TextAlign = HorizontalAlignment.Right;
            tbValue.TextChanged += tbValue_TextChanged;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.DialogResult = DialogResult.OK;
            button1.Enabled = false;
            button1.Location = new Point(98, 41);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Ввод";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.DialogResult = DialogResult.Cancel;
            button2.Location = new Point(179, 41);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "Отмена";
            button2.UseVisualStyleBackColor = true;
            // 
            // ChangeNumberDialog
            // 
            AcceptButton = button1;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = button2;
            ClientSize = new Size(266, 71);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(tbValue);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ChangeNumberDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Введите значение";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbValue;
        private Button button1;
        private Button button2;
    }
}