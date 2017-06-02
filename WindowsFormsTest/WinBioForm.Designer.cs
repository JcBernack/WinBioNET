namespace WindowsFormsTest
{
    partial class WinBioForm
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
            this.buttonIdentify = new System.Windows.Forms.Button();
            this.buttonLocateSensor = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonEnroll = new System.Windows.Forms.Button();
            this.btnRebuildDatabase = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonIdentify
            // 
            this.buttonIdentify.Location = new System.Drawing.Point(183, 18);
            this.buttonIdentify.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonIdentify.Name = "buttonIdentify";
            this.buttonIdentify.Size = new System.Drawing.Size(156, 35);
            this.buttonIdentify.TabIndex = 0;
            this.buttonIdentify.Text = "Identify";
            this.buttonIdentify.UseVisualStyleBackColor = true;
            this.buttonIdentify.Click += new System.EventHandler(this.buttonIdentify_Click);
            // 
            // buttonLocateSensor
            // 
            this.buttonLocateSensor.Location = new System.Drawing.Point(18, 18);
            this.buttonLocateSensor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonLocateSensor.Name = "buttonLocateSensor";
            this.buttonLocateSensor.Size = new System.Drawing.Size(156, 35);
            this.buttonLocateSensor.TabIndex = 1;
            this.buttonLocateSensor.Text = "LocateSensor";
            this.buttonLocateSensor.UseVisualStyleBackColor = true;
            this.buttonLocateSensor.Click += new System.EventHandler(this.buttonLocateSensor_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(18, 63);
            this.richTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(862, 433);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(768, 14);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 35);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonEnroll
            // 
            this.buttonEnroll.Location = new System.Drawing.Point(348, 18);
            this.buttonEnroll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonEnroll.Name = "buttonEnroll";
            this.buttonEnroll.Size = new System.Drawing.Size(112, 35);
            this.buttonEnroll.TabIndex = 4;
            this.buttonEnroll.Text = "Enroll";
            this.buttonEnroll.UseVisualStyleBackColor = true;
            this.buttonEnroll.Click += new System.EventHandler(this.buttonEnroll_Click);
            // 
            // btnRebuildDatabase
            // 
            this.btnRebuildDatabase.Location = new System.Drawing.Point(468, 18);
            this.btnRebuildDatabase.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRebuildDatabase.Name = "btnRebuildDatabase";
            this.btnRebuildDatabase.Size = new System.Drawing.Size(161, 35);
            this.btnRebuildDatabase.TabIndex = 7;
            this.btnRebuildDatabase.Text = "Rebuild Database";
            this.btnRebuildDatabase.UseVisualStyleBackColor = true;
            this.btnRebuildDatabase.Click += new System.EventHandler(this.btnRebuildDatabase_Click);
            // 
            // WinBioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 517);
            this.Controls.Add(this.btnRebuildDatabase);
            this.Controls.Add(this.buttonEnroll);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.buttonLocateSensor);
            this.Controls.Add(this.buttonIdentify);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WinBioForm";
            this.Text = "WinBioForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinBioForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonIdentify;
        private System.Windows.Forms.Button buttonLocateSensor;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEnroll;
        private System.Windows.Forms.Button btnRebuildDatabase;
    }
}

