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
            this.SuspendLayout();
            // 
            // buttonIdentify
            // 
            this.buttonIdentify.Location = new System.Drawing.Point(122, 12);
            this.buttonIdentify.Name = "buttonIdentify";
            this.buttonIdentify.Size = new System.Drawing.Size(104, 23);
            this.buttonIdentify.TabIndex = 0;
            this.buttonIdentify.Text = "Identify";
            this.buttonIdentify.UseVisualStyleBackColor = true;
            this.buttonIdentify.Click += new System.EventHandler(this.buttonIdentify_Click);
            // 
            // buttonLocateSensor
            // 
            this.buttonLocateSensor.Location = new System.Drawing.Point(12, 12);
            this.buttonLocateSensor.Name = "buttonLocateSensor";
            this.buttonLocateSensor.Size = new System.Drawing.Size(104, 23);
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
            this.richTextBox.Location = new System.Drawing.Point(12, 41);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(576, 283);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(232, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // WinBioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 336);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.buttonLocateSensor);
            this.Controls.Add(this.buttonIdentify);
            this.Name = "WinBioForm";
            this.Text = "WinBioForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonIdentify;
        private System.Windows.Forms.Button buttonLocateSensor;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button buttonCancel;
    }
}

