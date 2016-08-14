namespace OneHUD
{
    partial class OneHUD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneHUD));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.tabServers = new System.Windows.Forms.TabControl();
            this.tabPageHTTP = new System.Windows.Forms.TabPage();
            this.btnHTTPServerRestart = new System.Windows.Forms.Button();
            this.lblWebServerStatus = new System.Windows.Forms.Label();
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPortLabel = new System.Windows.Forms.Label();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.lblIPAddressLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lsvPlugins = new System.Windows.Forms.ListView();
            this.PluginName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PluginVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginIcon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListPlugins = new System.Windows.Forms.ImageList(this.components);
            this.PluginAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureConnected = new System.Windows.Forms.PictureBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.statusStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            this.tabServers.SuspendLayout();
            this.tabPageHTTP.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureConnected)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(810, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(63, 17);
            this.Status.Text = "Waiting ....";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(810, 407);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBoxStatus);
            this.tabPage1.Controls.Add(this.tabServers);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(802, 381);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.pictureConnected);
            this.groupBoxStatus.Controls.Add(this.labelStatus);
            this.groupBoxStatus.Location = new System.Drawing.Point(12, 30);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(390, 72);
            this.groupBoxStatus.TabIndex = 10;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Status";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(79, 25);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(196, 31);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Not Connected";
            this.labelStatus.Click += new System.EventHandler(this.labelStatus_Click);
            // 
            // tabServers
            // 
            this.tabServers.Controls.Add(this.tabPageHTTP);
            this.tabServers.Controls.Add(this.tabPage3);
            this.tabServers.Location = new System.Drawing.Point(8, 127);
            this.tabServers.Name = "tabServers";
            this.tabServers.SelectedIndex = 0;
            this.tabServers.Size = new System.Drawing.Size(398, 236);
            this.tabServers.TabIndex = 9;
            // 
            // tabPageHTTP
            // 
            this.tabPageHTTP.Controls.Add(this.btnHTTPServerRestart);
            this.tabPageHTTP.Controls.Add(this.lblWebServerStatus);
            this.tabPageHTTP.Controls.Add(this.lblStatusLabel);
            this.tabPageHTTP.Controls.Add(this.txtPort);
            this.tabPageHTTP.Controls.Add(this.lblPortLabel);
            this.tabPageHTTP.Controls.Add(this.lblIPAddress);
            this.tabPageHTTP.Controls.Add(this.lblIPAddressLabel);
            this.tabPageHTTP.Location = new System.Drawing.Point(4, 22);
            this.tabPageHTTP.Name = "tabPageHTTP";
            this.tabPageHTTP.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHTTP.Size = new System.Drawing.Size(390, 210);
            this.tabPageHTTP.TabIndex = 0;
            this.tabPageHTTP.Text = "HTTP Server";
            this.tabPageHTTP.UseVisualStyleBackColor = true;
            // 
            // btnHTTPServerRestart
            // 
            this.btnHTTPServerRestart.Location = new System.Drawing.Point(271, 147);
            this.btnHTTPServerRestart.Name = "btnHTTPServerRestart";
            this.btnHTTPServerRestart.Size = new System.Drawing.Size(94, 28);
            this.btnHTTPServerRestart.TabIndex = 13;
            this.btnHTTPServerRestart.Text = "Restart";
            this.btnHTTPServerRestart.UseVisualStyleBackColor = true;
            // 
            // lblWebServerStatus
            // 
            this.lblWebServerStatus.AutoSize = true;
            this.lblWebServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebServerStatus.Location = new System.Drawing.Point(150, 147);
            this.lblWebServerStatus.Name = "lblWebServerStatus";
            this.lblWebServerStatus.Size = new System.Drawing.Size(115, 31);
            this.lblWebServerStatus.TabIndex = 12;
            this.lblWebServerStatus.Text = "Stopped";
            // 
            // lblStatusLabel
            // 
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusLabel.Location = new System.Drawing.Point(51, 147);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(92, 31);
            this.lblStatusLabel.TabIndex = 11;
            this.lblStatusLabel.Text = "Status";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(155, 93);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(82, 38);
            this.txtPort.TabIndex = 10;
            this.txtPort.TabStop = false;
            // 
            // lblPortLabel
            // 
            this.lblPortLabel.AutoSize = true;
            this.lblPortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortLabel.Location = new System.Drawing.Point(79, 91);
            this.lblPortLabel.Name = "lblPortLabel";
            this.lblPortLabel.Size = new System.Drawing.Size(64, 31);
            this.lblPortLabel.TabIndex = 9;
            this.lblPortLabel.Text = "Port";
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAddress.Location = new System.Drawing.Point(149, 38);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(218, 31);
            this.lblIPAddress.TabIndex = 8;
            this.lblIPAddress.Text = "999.999.999.999";
            // 
            // lblIPAddressLabel
            // 
            this.lblIPAddressLabel.AutoSize = true;
            this.lblIPAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAddressLabel.Location = new System.Drawing.Point(5, 38);
            this.lblIPAddressLabel.Name = "lblIPAddressLabel";
            this.lblIPAddressLabel.Size = new System.Drawing.Size(147, 31);
            this.lblIPAddressLabel.TabIndex = 7;
            this.lblIPAddressLabel.Text = "IP Address";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lsvPlugins);
            this.groupBox1.Location = new System.Drawing.Point(412, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 349);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Plugins";
            // 
            // lsvPlugins
            // 
            this.lsvPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pluginIcon,
            this.PluginName,
            this.PluginVersion,
            this.PluginAuthor});
            this.lsvPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvPlugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvPlugins.FullRowSelect = true;
            this.lsvPlugins.LargeImageList = this.imageListPlugins;
            this.lsvPlugins.Location = new System.Drawing.Point(3, 16);
            this.lsvPlugins.Name = "lsvPlugins";
            this.lsvPlugins.Size = new System.Drawing.Size(368, 330);
            this.lsvPlugins.SmallImageList = this.imageListPlugins;
            this.lsvPlugins.TabIndex = 0;
            this.lsvPlugins.UseCompatibleStateImageBehavior = false;
            this.lsvPlugins.View = System.Windows.Forms.View.Details;
            // 
            // PluginName
            // 
            this.PluginName.Text = "Name";
            this.PluginName.Width = 200;
            // 
            // PluginVersion
            // 
            this.PluginVersion.Text = "Version";
            this.PluginVersion.Width = 100;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(802, 381);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Applications";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(810, 24);
            this.mainMenu.TabIndex = 8;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // pluginIcon
            // 
            this.pluginIcon.Text = " ";
            this.pluginIcon.Width = 40;
            // 
            // imageListPlugins
            // 
            this.imageListPlugins.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPlugins.ImageStream")));
            this.imageListPlugins.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPlugins.Images.SetKeyName(0, "missing");
            // 
            // PluginAuthor
            // 
            this.PluginAuthor.Text = "Author";
            this.PluginAuthor.Width = 200;
            // 
            // pictureConnected
            // 
            this.pictureConnected.Location = new System.Drawing.Point(27, 24);
            this.pictureConnected.Name = "pictureConnected";
            this.pictureConnected.Size = new System.Drawing.Size(32, 32);
            this.pictureConnected.TabIndex = 1;
            this.pictureConnected.TabStop = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(390, 210);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "IOT Server";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // OneHUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 453);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "OneHUD";
            this.Text = "OneHUD";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AGServer_FormClosed);
            this.Load += new System.EventHandler(this.AGServer_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.tabServers.ResumeLayout(false);
            this.tabPageHTTP.ResumeLayout(false);
            this.tabPageHTTP.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureConnected)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lsvPlugins;
        private System.Windows.Forms.ColumnHeader PluginName;
        private System.Windows.Forms.ColumnHeader PluginVersion;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.TabControl tabServers;
        private System.Windows.Forms.TabPage tabPageHTTP;
        private System.Windows.Forms.Button btnHTTPServerRestart;
        private System.Windows.Forms.Label lblWebServerStatus;
        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPortLabel;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.Label lblIPAddressLabel;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ColumnHeader pluginIcon;
        private System.Windows.Forms.ImageList imageListPlugins;
        private System.Windows.Forms.ColumnHeader PluginAuthor;
        private System.Windows.Forms.PictureBox pictureConnected;
        private System.Windows.Forms.TabPage tabPage3;
    }
}

