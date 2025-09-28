namespace Simulator.View
{
    partial class SelectLinkSourceForm
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
            tvSources = new TreeView();
            btnAccept = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // tvSources
            // 
            tvSources.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvSources.Location = new Point(12, 12);
            tvSources.Name = "tvSources";
            tvSources.Size = new Size(200, 241);
            tvSources.TabIndex = 0;
            tvSources.AfterSelect += tvSources_AfterSelect;
            // 
            // btnAccept
            // 
            btnAccept.DialogResult = DialogResult.OK;
            btnAccept.Enabled = false;
            btnAccept.Location = new Point(56, 259);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(75, 23);
            btnAccept.TabIndex = 1;
            btnAccept.Text = "Выбрать";
            btnAccept.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(137, 259);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Отменить";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // SelectLinkSourceForm
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(224, 294);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(tvSources);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectLinkSourceForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Выбор источника для связи";
            Load += SelectLinkSourceForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private TreeView tvSources;
        private Button btnAccept;
        private Button btnCancel;
    }
}