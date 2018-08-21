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
            this.fingerprintPictureBox = new System.Windows.Forms.PictureBox();
            this.buttonCaptureSample = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fingerprintPictureBox)).BeginInit();
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
            this.richTextBox.Size = new System.Drawing.Size(488, 283);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(568, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonEnroll
            // 
            this.buttonEnroll.Location = new System.Drawing.Point(232, 12);
            this.buttonEnroll.Name = "buttonEnroll";
            this.buttonEnroll.Size = new System.Drawing.Size(75, 23);
            this.buttonEnroll.TabIndex = 4;
            this.buttonEnroll.Text = "Enroll";
            this.buttonEnroll.UseVisualStyleBackColor = true;
            this.buttonEnroll.Click += new System.EventHandler(this.buttonEnroll_Click);
            // 
            // btnRebuildDatabase
            // 
            this.btnRebuildDatabase.Location = new System.Drawing.Point(312, 12);
            this.btnRebuildDatabase.Name = "btnRebuildDatabase";
            this.btnRebuildDatabase.Size = new System.Drawing.Size(107, 23);
            this.btnRebuildDatabase.TabIndex = 7;
            this.btnRebuildDatabase.Text = "Rebuild Database";
            this.btnRebuildDatabase.UseVisualStyleBackColor = true;
            this.btnRebuildDatabase.Click += new System.EventHandler(this.btnRebuildDatabase_Click);
            // 
            // fingerprintPictureBox
            // 
            this.fingerprintPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fingerprintPictureBox.Location = new System.Drawing.Point(506, 41);
            this.fingerprintPictureBox.Name = "fingerprintPictureBox";
            this.fingerprintPictureBox.Size = new System.Drawing.Size(137, 283);
            this.fingerprintPictureBox.TabIndex = 8;
            this.fingerprintPictureBox.TabStop = false;
            // 
            // buttonCaptureSample
            // 
            this.buttonCaptureSample.Location = new System.Drawing.Point(425, 12);
            this.buttonCaptureSample.Name = "buttonCaptureSample";
            this.buttonCaptureSample.Size = new System.Drawing.Size(101, 23);
            this.buttonCaptureSample.TabIndex = 9;
            this.buttonCaptureSample.Text = "CaptureSample";
            this.buttonCaptureSample.UseVisualStyleBackColor = true;
            this.buttonCaptureSample.Click += new System.EventHandler(this.buttonCaptureSample_Click);
            // 
            // WinBioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 336);
            this.Controls.Add(this.buttonCaptureSample);
            this.Controls.Add(this.fingerprintPictureBox);
            this.Controls.Add(this.btnRebuildDatabase);
            this.Controls.Add(this.buttonEnroll);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.buttonLocateSensor);
            this.Controls.Add(this.buttonIdentify);
            this.Name = "WinBioForm";
            this.Text = "WinBioForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinBioForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.fingerprintPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonIdentify;
        private System.Windows.Forms.Button buttonLocateSensor;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEnroll;
        private System.Windows.Forms.Button btnRebuildDatabase;
        private System.Windows.Forms.PictureBox fingerprintPictureBox;
        private System.Windows.Forms.Button buttonCaptureSample;
    }
}

