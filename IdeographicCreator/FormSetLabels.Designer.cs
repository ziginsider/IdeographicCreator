namespace IdeographicCreator
{
    partial class FormSetLabels
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
            this.treeViewSetLabels = new System.Windows.Forms.TreeView();
            this.btnSetLabels = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewSetLabels
            // 
            this.treeViewSetLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewSetLabels.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.treeViewSetLabels.Location = new System.Drawing.Point(0, -1);
            this.treeViewSetLabels.Name = "treeViewSetLabels";
            this.treeViewSetLabels.Size = new System.Drawing.Size(377, 689);
            this.treeViewSetLabels.TabIndex = 0;
            // 
            // btnSetLabels
            // 
            this.btnSetLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetLabels.Location = new System.Drawing.Point(239, 705);
            this.btnSetLabels.Name = "btnSetLabels";
            this.btnSetLabels.Size = new System.Drawing.Size(126, 26);
            this.btnSetLabels.TabIndex = 1;
            this.btnSetLabels.Text = "OK";
            this.btnSetLabels.UseVisualStyleBackColor = true;
            this.btnSetLabels.Click += new System.EventHandler(this.btnSetLabels_Click);
            // 
            // FormSetLabels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 743);
            this.Controls.Add(this.btnSetLabels);
            this.Controls.Add(this.treeViewSetLabels);
            this.Name = "FormSetLabels";
            this.Text = "FormSetLabels";
            this.Load += new System.EventHandler(this.FormSetLabels_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewSetLabels;
        private System.Windows.Forms.Button btnSetLabels;
    }
}