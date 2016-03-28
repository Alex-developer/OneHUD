namespace AGServer
{
    partial class AGServer
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxHTTPServer = new System.Windows.Forms.GroupBox();
            this.btnHTTPServerRestart = new System.Windows.Forms.Button();
            this.lblWebServerStatus = new System.Windows.Forms.Label();
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPortLabel = new System.Windows.Forms.Label();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.lblIPAddressLabel = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.groupBoxHTTPServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip.Location = new System.Drawing.Point(0, 231);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(510, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(63, 17);
            this.Status.Text = "Waiting ....";
            // 
            // groupBoxHTTPServer
            // 
            this.groupBoxHTTPServer.Controls.Add(this.btnHTTPServerRestart);
            this.groupBoxHTTPServer.Controls.Add(this.lblWebServerStatus);
            this.groupBoxHTTPServer.Controls.Add(this.lblStatusLabel);
            this.groupBoxHTTPServer.Controls.Add(this.txtPort);
            this.groupBoxHTTPServer.Controls.Add(this.lblPortLabel);
            this.groupBoxHTTPServer.Controls.Add(this.lblIPAddress);
            this.groupBoxHTTPServer.Controls.Add(this.lblIPAddressLabel);
            this.groupBoxHTTPServer.Location = new System.Drawing.Point(0, 12);
            this.groupBoxHTTPServer.Name = "groupBoxHTTPServer";
            this.groupBoxHTTPServer.Size = new System.Drawing.Size(499, 166);
            this.groupBoxHTTPServer.TabIndex = 6;
            this.groupBoxHTTPServer.TabStop = false;
            this.groupBoxHTTPServer.Text = "HTTP Server ";
            // 
            // btnHTTPServerRestart
            // 
            this.btnHTTPServerRestart.Location = new System.Drawing.Point(381, 125);
            this.btnHTTPServerRestart.Name = "btnHTTPServerRestart";
            this.btnHTTPServerRestart.Size = new System.Drawing.Size(94, 28);
            this.btnHTTPServerRestart.TabIndex = 6;
            this.btnHTTPServerRestart.Text = "Restart";
            this.btnHTTPServerRestart.UseVisualStyleBackColor = true;
            this.btnHTTPServerRestart.Click += new System.EventHandler(this.btnHTTPServerRestart_Click);
            // 
            // lblWebServerStatus
            // 
            this.lblWebServerStatus.AutoSize = true;
            this.lblWebServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebServerStatus.Location = new System.Drawing.Point(179, 122);
            this.lblWebServerStatus.Name = "lblWebServerStatus";
            this.lblWebServerStatus.Size = new System.Drawing.Size(115, 31);
            this.lblWebServerStatus.TabIndex = 5;
            this.lblWebServerStatus.Text = "Stopped";
            // 
            // lblStatusLabel
            // 
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusLabel.Location = new System.Drawing.Point(69, 122);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(92, 31);
            this.lblStatusLabel.TabIndex = 4;
            this.lblStatusLabel.Text = "Status";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(184, 72);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(82, 38);
            this.txtPort.TabIndex = 3;
            this.txtPort.TabStop = false;
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // lblPortLabel
            // 
            this.lblPortLabel.AutoSize = true;
            this.lblPortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortLabel.Location = new System.Drawing.Point(97, 71);
            this.lblPortLabel.Name = "lblPortLabel";
            this.lblPortLabel.Size = new System.Drawing.Size(64, 31);
            this.lblPortLabel.TabIndex = 2;
            this.lblPortLabel.Text = "Port";
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAddress.Location = new System.Drawing.Point(179, 26);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(218, 31);
            this.lblIPAddress.TabIndex = 1;
            this.lblIPAddress.Text = "999.999.999.999";
            // 
            // lblIPAddressLabel
            // 
            this.lblIPAddressLabel.AutoSize = true;
            this.lblIPAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAddressLabel.Location = new System.Drawing.Point(14, 26);
            this.lblIPAddressLabel.Name = "lblIPAddressLabel";
            this.lblIPAddressLabel.Size = new System.Drawing.Size(147, 31);
            this.lblIPAddressLabel.TabIndex = 0;
            this.lblIPAddressLabel.Text = "IP Address";
            // 
            // AGServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 253);
            this.Controls.Add(this.groupBoxHTTPServer);
            this.Controls.Add(this.statusStrip);
            this.Name = "AGServer";
            this.Text = "AG Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AGServer_FormClosed);
            this.Load += new System.EventHandler(this.AGServer_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBoxHTTPServer.ResumeLayout(false);
            this.groupBoxHTTPServer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.GroupBox groupBoxHTTPServer;
        private System.Windows.Forms.Button btnHTTPServerRestart;
        private System.Windows.Forms.Label lblWebServerStatus;
        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPortLabel;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.Label lblIPAddressLabel;
    }
}

