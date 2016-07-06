namespace IdeographicCreator
{
    partial class FormSetTopic
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
            this.textBoxSetTopic = new System.Windows.Forms.TextBox();
            this.labelSetTopic = new System.Windows.Forms.Label();
            this.buttonSetTopic = new System.Windows.Forms.Button();
            this.textBoxChangeTopic = new System.Windows.Forms.TextBox();
            this.buttonChangeTopic = new System.Windows.Forms.Button();
            this.labelChangeTopic = new System.Windows.Forms.Label();
            this.buttonDeleteTopic = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxSetTopic
            // 
            this.textBoxSetTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSetTopic.Location = new System.Drawing.Point(12, 217);
            this.textBoxSetTopic.Name = "textBoxSetTopic";
            this.textBoxSetTopic.Size = new System.Drawing.Size(370, 22);
            this.textBoxSetTopic.TabIndex = 0;
            // 
            // labelSetTopic
            // 
            this.labelSetTopic.AutoSize = true;
            this.labelSetTopic.Location = new System.Drawing.Point(16, 188);
            this.labelSetTopic.Name = "labelSetTopic";
            this.labelSetTopic.Size = new System.Drawing.Size(195, 17);
            this.labelSetTopic.TabIndex = 1;
            this.labelSetTopic.Text = "Введите название подтемы:";
            // 
            // buttonSetTopic
            // 
            this.buttonSetTopic.Location = new System.Drawing.Point(12, 254);
            this.buttonSetTopic.Name = "buttonSetTopic";
            this.buttonSetTopic.Size = new System.Drawing.Size(165, 26);
            this.buttonSetTopic.TabIndex = 2;
            this.buttonSetTopic.Text = "Добавить подтему";
            this.buttonSetTopic.UseVisualStyleBackColor = true;
            this.buttonSetTopic.Click += new System.EventHandler(this.buttonSetTopic_Click);
            // 
            // textBoxChangeTopic
            // 
            this.textBoxChangeTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChangeTopic.Location = new System.Drawing.Point(12, 33);
            this.textBoxChangeTopic.Name = "textBoxChangeTopic";
            this.textBoxChangeTopic.Size = new System.Drawing.Size(369, 22);
            this.textBoxChangeTopic.TabIndex = 3;
            // 
            // buttonChangeTopic
            // 
            this.buttonChangeTopic.Location = new System.Drawing.Point(11, 71);
            this.buttonChangeTopic.Name = "buttonChangeTopic";
            this.buttonChangeTopic.Size = new System.Drawing.Size(166, 26);
            this.buttonChangeTopic.TabIndex = 4;
            this.buttonChangeTopic.Text = "Сохранить изменения";
            this.buttonChangeTopic.UseVisualStyleBackColor = true;
            this.buttonChangeTopic.Click += new System.EventHandler(this.buttonChangeTopic_Click);
            // 
            // labelChangeTopic
            // 
            this.labelChangeTopic.AutoSize = true;
            this.labelChangeTopic.Location = new System.Drawing.Point(16, 9);
            this.labelChangeTopic.Name = "labelChangeTopic";
            this.labelChangeTopic.Size = new System.Drawing.Size(216, 17);
            this.labelChangeTopic.TabIndex = 5;
            this.labelChangeTopic.Text = "Редактировать название темы:";
            // 
            // buttonDeleteTopic
            // 
            this.buttonDeleteTopic.Location = new System.Drawing.Point(12, 133);
            this.buttonDeleteTopic.Name = "buttonDeleteTopic";
            this.buttonDeleteTopic.Size = new System.Drawing.Size(164, 26);
            this.buttonDeleteTopic.TabIndex = 6;
            this.buttonDeleteTopic.Text = "Удалить тему";
            this.buttonDeleteTopic.UseVisualStyleBackColor = true;
            this.buttonDeleteTopic.Click += new System.EventHandler(this.buttonDeleteTopic_Click);
            // 
            // FormSetTopic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(397, 309);
            this.Controls.Add(this.buttonDeleteTopic);
            this.Controls.Add(this.labelChangeTopic);
            this.Controls.Add(this.buttonChangeTopic);
            this.Controls.Add(this.textBoxChangeTopic);
            this.Controls.Add(this.buttonSetTopic);
            this.Controls.Add(this.labelSetTopic);
            this.Controls.Add(this.textBoxSetTopic);
            this.Name = "FormSetTopic";
            this.Text = "Set Topic";
            this.Load += new System.EventHandler(this.FormSetTopic_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSetTopic;
        private System.Windows.Forms.Label labelSetTopic;
        private System.Windows.Forms.Button buttonSetTopic;
        private System.Windows.Forms.TextBox textBoxChangeTopic;
        private System.Windows.Forms.Button buttonChangeTopic;
        private System.Windows.Forms.Label labelChangeTopic;
        private System.Windows.Forms.Button buttonDeleteTopic;
    }
}