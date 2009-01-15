namespace AviRecorder
{
    partial class frmRecorder
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
            this.components = new System.ComponentModel.Container();
            this.txtAviFile = new System.Windows.Forms.TextBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdStart = new System.Windows.Forms.Button();
            this.createAviDlg = new System.Windows.Forms.SaveFileDialog();
            this.CaptureScreenTime = new System.Windows.Forms.Timer(this.components);
            this.numScreenSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numScreenSize)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAviFile
            // 
            this.txtAviFile.Location = new System.Drawing.Point(12, 12);
            this.txtAviFile.Name = "txtAviFile";
            this.txtAviFile.Size = new System.Drawing.Size(380, 20);
            this.txtAviFile.TabIndex = 0;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(398, 9);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 1;
            this.cmdSave.Text = "...";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(12, 38);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 2;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // createAviDlg
            // 
            this.createAviDlg.Filter = "AVI Files|*.avi";
            // 
            // CaptureScreenTime
            // 
            this.CaptureScreenTime.Interval = 40;
            this.CaptureScreenTime.Tick += new System.EventHandler(this.CaptureScreenTime_Tick);
            // 
            // numScreenSize
            // 
            this.numScreenSize.Location = new System.Drawing.Point(328, 45);
            this.numScreenSize.Name = "numScreenSize";
            this.numScreenSize.Size = new System.Drawing.Size(120, 20);
            this.numScreenSize.TabIndex = 3;
            this.numScreenSize.ValueChanged += new System.EventHandler(this.numScreenSize_ValueChanged);
            // 
            // frmRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 77);
            this.Controls.Add(this.numScreenSize);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.txtAviFile);
            this.Name = "frmRecorder";
            this.Text = "Screen Recorder";
            ((System.ComponentModel.ISupportInitialize)(this.numScreenSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAviFile;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.SaveFileDialog createAviDlg;
        private System.Windows.Forms.Timer CaptureScreenTime;
        private System.Windows.Forms.NumericUpDown numScreenSize;
    }
}

