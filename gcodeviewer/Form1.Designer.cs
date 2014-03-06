namespace gcodeparser
{
    partial class MainForm
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
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.CurrentLineTrackbar = new System.Windows.Forms.TrackBar();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.ManualModeButton = new System.Windows.Forms.Button();
            this.agaLabel11 = new AGAControls.AgaLabel();
            this.agaLabel10 = new AGAControls.AgaLabel();
            this.MachineStatusLabel = new AGAControls.AgaLabel();
            this.CancelMachiningButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.ConnectionStatusLabel = new AGAControls.AgaLabel();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.agaLabel9 = new AGAControls.AgaLabel();
            this.EstimatedTimeTextbox = new AGAControls.AgaTextbox();
            this.agaLabel8 = new AGAControls.AgaLabel();
            this.TotalDistanceTextbox = new AGAControls.AgaTextbox();
            this.agaLabel7 = new AGAControls.AgaLabel();
            this.CheckMathLabel = new AGAControls.AgaLabel();
            this.agaLabel6 = new AGAControls.AgaLabel();
            this.SpindleDiameterTextbox = new AGAControls.AgaTextbox();
            this.agaLabel5 = new AGAControls.AgaLabel();
            this.agaTextbox1 = new AGAControls.AgaTextbox();
            this.agaLabel4 = new AGAControls.AgaLabel();
            this.CurrentLineTextbox = new AGAControls.AgaTextbox();
            this.agaLabel3 = new AGAControls.AgaLabel();
            this.agaLabel2 = new AGAControls.AgaLabel();
            this.ZoomTextbox = new AGAControls.AgaTextbox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseLabel = new AGAControls.AgaLabel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.agaLabel1 = new AGAControls.AgaLabel();
            this.agaProgress1 = new AGAControls.AgaProgress();
            this.FileNamePanel = new System.Windows.Forms.Panel();
            this.FileNameLabel = new AGAControls.AgaLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentLineTrackbar)).BeginInit();
            this.RightPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.FileNamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RenderPanel
            // 
            this.RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPanel.Location = new System.Drawing.Point(0, 104);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(641, 462);
            this.RenderPanel.TabIndex = 3;
            // 
            // CurrentLineTrackbar
            // 
            this.CurrentLineTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentLineTrackbar.LargeChange = 10;
            this.CurrentLineTrackbar.Location = new System.Drawing.Point(7, 171);
            this.CurrentLineTrackbar.Maximum = 1;
            this.CurrentLineTrackbar.Minimum = 1;
            this.CurrentLineTrackbar.Name = "CurrentLineTrackbar";
            this.CurrentLineTrackbar.Size = new System.Drawing.Size(221, 45);
            this.CurrentLineTrackbar.TabIndex = 4;
            this.CurrentLineTrackbar.Value = 1;
            this.CurrentLineTrackbar.ValueChanged += new System.EventHandler(this.StepTrackbar_Scroll);
            // 
            // RightPanel
            // 
            this.RightPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.RightPanel.Controls.Add(this.ManualModeButton);
            this.RightPanel.Controls.Add(this.agaLabel11);
            this.RightPanel.Controls.Add(this.agaLabel10);
            this.RightPanel.Controls.Add(this.MachineStatusLabel);
            this.RightPanel.Controls.Add(this.CancelMachiningButton);
            this.RightPanel.Controls.Add(this.PlayButton);
            this.RightPanel.Controls.Add(this.ConnectionStatusLabel);
            this.RightPanel.Controls.Add(this.DisconnectButton);
            this.RightPanel.Controls.Add(this.ConnectButton);
            this.RightPanel.Controls.Add(this.agaLabel9);
            this.RightPanel.Controls.Add(this.EstimatedTimeTextbox);
            this.RightPanel.Controls.Add(this.agaLabel8);
            this.RightPanel.Controls.Add(this.TotalDistanceTextbox);
            this.RightPanel.Controls.Add(this.agaLabel7);
            this.RightPanel.Controls.Add(this.CheckMathLabel);
            this.RightPanel.Controls.Add(this.CurrentLineTrackbar);
            this.RightPanel.Controls.Add(this.agaLabel6);
            this.RightPanel.Controls.Add(this.SpindleDiameterTextbox);
            this.RightPanel.Controls.Add(this.agaLabel5);
            this.RightPanel.Controls.Add(this.agaTextbox1);
            this.RightPanel.Controls.Add(this.agaLabel4);
            this.RightPanel.Controls.Add(this.CurrentLineTextbox);
            this.RightPanel.Controls.Add(this.agaLabel3);
            this.RightPanel.Controls.Add(this.agaLabel2);
            this.RightPanel.Controls.Add(this.ZoomTextbox);
            this.RightPanel.Controls.Add(this.panel1);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(641, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(233, 566);
            this.RightPanel.TabIndex = 8;
            // 
            // ManualModeButton
            // 
            this.ManualModeButton.ForeColor = System.Drawing.Color.Black;
            this.ManualModeButton.Location = new System.Drawing.Point(15, 502);
            this.ManualModeButton.Name = "ManualModeButton";
            this.ManualModeButton.Size = new System.Drawing.Size(206, 23);
            this.ManualModeButton.TabIndex = 30;
            this.ManualModeButton.Text = "Manual mode";
            this.ManualModeButton.UseVisualStyleBackColor = true;
            this.ManualModeButton.Click += new System.EventHandler(this.ManualModeButton_Click);
            // 
            // agaLabel11
            // 
            this.agaLabel11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel11.Location = new System.Drawing.Point(15, 314);
            this.agaLabel11.Name = "agaLabel11";
            this.agaLabel11.Size = new System.Drawing.Size(131, 15);
            this.agaLabel11.TabIndex = 29;
            this.agaLabel11.Text = "Machine connection:";
            this.agaLabel11.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // agaLabel10
            // 
            this.agaLabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel10.Location = new System.Drawing.Point(15, 368);
            this.agaLabel10.Name = "agaLabel10";
            this.agaLabel10.Size = new System.Drawing.Size(131, 15);
            this.agaLabel10.TabIndex = 29;
            this.agaLabel10.Text = "Machine program:";
            this.agaLabel10.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // MachineStatusLabel
            // 
            this.MachineStatusLabel.Location = new System.Drawing.Point(80, 390);
            this.MachineStatusLabel.Name = "MachineStatusLabel";
            this.MachineStatusLabel.Size = new System.Drawing.Size(67, 23);
            this.MachineStatusLabel.TabIndex = 29;
            this.MachineStatusLabel.Text = "Stopped";
            this.MachineStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MachineStatusLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // CancelMachiningButton
            // 
            this.CancelMachiningButton.Enabled = false;
            this.CancelMachiningButton.ForeColor = System.Drawing.Color.Black;
            this.CancelMachiningButton.Location = new System.Drawing.Point(146, 385);
            this.CancelMachiningButton.Name = "CancelMachiningButton";
            this.CancelMachiningButton.Size = new System.Drawing.Size(75, 23);
            this.CancelMachiningButton.TabIndex = 28;
            this.CancelMachiningButton.Text = "CANCEL";
            this.CancelMachiningButton.UseVisualStyleBackColor = true;
            this.CancelMachiningButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.Enabled = false;
            this.PlayButton.ForeColor = System.Drawing.Color.Black;
            this.PlayButton.Location = new System.Drawing.Point(15, 385);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(62, 23);
            this.PlayButton.TabIndex = 27;
            this.PlayButton.Text = "BEGIN";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // ConnectionStatusLabel
            // 
            this.ConnectionStatusLabel.Location = new System.Drawing.Point(79, 336);
            this.ConnectionStatusLabel.Name = "ConnectionStatusLabel";
            this.ConnectionStatusLabel.Size = new System.Drawing.Size(67, 23);
            this.ConnectionStatusLabel.TabIndex = 26;
            this.ConnectionStatusLabel.Text = "Offline";
            this.ConnectionStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ConnectionStatusLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Enabled = false;
            this.DisconnectButton.ForeColor = System.Drawing.Color.Black;
            this.DisconnectButton.Location = new System.Drawing.Point(146, 331);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(75, 23);
            this.DisconnectButton.TabIndex = 25;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.ForeColor = System.Drawing.Color.Black;
            this.ConnectButton.Location = new System.Drawing.Point(15, 331);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(62, 23);
            this.ConnectButton.TabIndex = 24;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // agaLabel9
            // 
            this.agaLabel9.AutoSize = true;
            this.agaLabel9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel9.ForeColor = System.Drawing.Color.White;
            this.agaLabel9.Location = new System.Drawing.Point(15, 278);
            this.agaLabel9.Name = "agaLabel9";
            this.agaLabel9.Size = new System.Drawing.Size(131, 15);
            this.agaLabel9.TabIndex = 23;
            this.agaLabel9.Text = "Estimated time needed:";
            this.agaLabel9.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // EstimatedTimeTextbox
            // 
            this.EstimatedTimeTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.EstimatedTimeTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EstimatedTimeTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EstimatedTimeTextbox.ForeColor = System.Drawing.Color.White;
            this.EstimatedTimeTextbox.Location = new System.Drawing.Point(157, 278);
            this.EstimatedTimeTextbox.Name = "EstimatedTimeTextbox";
            this.EstimatedTimeTextbox.Size = new System.Drawing.Size(69, 16);
            this.EstimatedTimeTextbox.TabIndex = 22;
            this.EstimatedTimeTextbox.Text = "--";
            // 
            // agaLabel8
            // 
            this.agaLabel8.AutoSize = true;
            this.agaLabel8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel8.ForeColor = System.Drawing.Color.White;
            this.agaLabel8.Location = new System.Drawing.Point(15, 257);
            this.agaLabel8.Name = "agaLabel8";
            this.agaLabel8.Size = new System.Drawing.Size(84, 15);
            this.agaLabel8.TabIndex = 21;
            this.agaLabel8.Text = "Total distance:";
            this.agaLabel8.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // TotalDistanceTextbox
            // 
            this.TotalDistanceTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.TotalDistanceTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TotalDistanceTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalDistanceTextbox.ForeColor = System.Drawing.Color.White;
            this.TotalDistanceTextbox.Location = new System.Drawing.Point(157, 257);
            this.TotalDistanceTextbox.Name = "TotalDistanceTextbox";
            this.TotalDistanceTextbox.Size = new System.Drawing.Size(69, 16);
            this.TotalDistanceTextbox.TabIndex = 20;
            this.TotalDistanceTextbox.Text = "--";
            // 
            // agaLabel7
            // 
            this.agaLabel7.AutoSize = true;
            this.agaLabel7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel7.ForeColor = System.Drawing.Color.White;
            this.agaLabel7.Location = new System.Drawing.Point(13, 217);
            this.agaLabel7.Name = "agaLabel7";
            this.agaLabel7.Size = new System.Drawing.Size(44, 25);
            this.agaLabel7.TabIndex = 19;
            this.agaLabel7.Text = "Job";
            this.agaLabel7.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // CheckMathLabel
            // 
            this.CheckMathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckMathLabel.AutoSize = true;
            this.CheckMathLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CheckMathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckMathLabel.ForeColor = System.Drawing.Color.White;
            this.CheckMathLabel.Location = new System.Drawing.Point(12, 542);
            this.CheckMathLabel.Name = "CheckMathLabel";
            this.CheckMathLabel.Size = new System.Drawing.Size(119, 15);
            this.CheckMathLabel.TabIndex = 18;
            this.CheckMathLabel.Text = "Check software math";
            this.CheckMathLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.CheckMathLabel.Click += new System.EventHandler(this.CheckMathLabel_Click);
            // 
            // agaLabel6
            // 
            this.agaLabel6.AutoSize = true;
            this.agaLabel6.Enabled = false;
            this.agaLabel6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel6.ForeColor = System.Drawing.Color.White;
            this.agaLabel6.Location = new System.Drawing.Point(15, 147);
            this.agaLabel6.Name = "agaLabel6";
            this.agaLabel6.Size = new System.Drawing.Size(132, 15);
            this.agaLabel6.TabIndex = 17;
            this.agaLabel6.Text = "Spindle diameter (mm):";
            this.agaLabel6.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // SpindleDiameterTextbox
            // 
            this.SpindleDiameterTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.SpindleDiameterTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SpindleDiameterTextbox.Enabled = false;
            this.SpindleDiameterTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpindleDiameterTextbox.ForeColor = System.Drawing.Color.White;
            this.SpindleDiameterTextbox.Location = new System.Drawing.Point(155, 147);
            this.SpindleDiameterTextbox.Name = "SpindleDiameterTextbox";
            this.SpindleDiameterTextbox.Size = new System.Drawing.Size(69, 16);
            this.SpindleDiameterTextbox.TabIndex = 16;
            this.SpindleDiameterTextbox.Text = "0,1";
            this.SpindleDiameterTextbox.TextChanged += new System.EventHandler(this.SpindleDiameterTextbox_TextChanged);
            // 
            // agaLabel5
            // 
            this.agaLabel5.AutoSize = true;
            this.agaLabel5.Enabled = false;
            this.agaLabel5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel5.ForeColor = System.Drawing.Color.White;
            this.agaLabel5.Location = new System.Drawing.Point(15, 128);
            this.agaLabel5.Name = "agaLabel5";
            this.agaLabel5.Size = new System.Drawing.Size(97, 15);
            this.agaLabel5.TabIndex = 15;
            this.agaLabel5.Text = "Translation (x, y):";
            this.agaLabel5.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // agaTextbox1
            // 
            this.agaTextbox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.agaTextbox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.agaTextbox1.Enabled = false;
            this.agaTextbox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaTextbox1.ForeColor = System.Drawing.Color.White;
            this.agaTextbox1.Location = new System.Drawing.Point(155, 128);
            this.agaTextbox1.Name = "agaTextbox1";
            this.agaTextbox1.Size = new System.Drawing.Size(69, 16);
            this.agaTextbox1.TabIndex = 14;
            this.agaTextbox1.Text = "0, 0";
            // 
            // agaLabel4
            // 
            this.agaLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.agaLabel4.AutoSize = true;
            this.agaLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel4.ForeColor = System.Drawing.Color.White;
            this.agaLabel4.Location = new System.Drawing.Point(16, 417);
            this.agaLabel4.Name = "agaLabel4";
            this.agaLabel4.Size = new System.Drawing.Size(110, 15);
            this.agaLabel4.TabIndex = 13;
            this.agaLabel4.Text = "Current operation:";
            this.agaLabel4.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // CurrentLineTextbox
            // 
            this.CurrentLineTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentLineTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.CurrentLineTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentLineTextbox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentLineTextbox.ForeColor = System.Drawing.Color.White;
            this.CurrentLineTextbox.Location = new System.Drawing.Point(16, 435);
            this.CurrentLineTextbox.Multiline = true;
            this.CurrentLineTextbox.Name = "CurrentLineTextbox";
            this.CurrentLineTextbox.Size = new System.Drawing.Size(205, 61);
            this.CurrentLineTextbox.TabIndex = 12;
            this.CurrentLineTextbox.Text = "Z0 G0";
            // 
            // agaLabel3
            // 
            this.agaLabel3.AutoSize = true;
            this.agaLabel3.Enabled = false;
            this.agaLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel3.ForeColor = System.Drawing.Color.White;
            this.agaLabel3.Location = new System.Drawing.Point(15, 110);
            this.agaLabel3.Name = "agaLabel3";
            this.agaLabel3.Size = new System.Drawing.Size(37, 15);
            this.agaLabel3.TabIndex = 11;
            this.agaLabel3.Text = "Scale:";
            this.agaLabel3.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // agaLabel2
            // 
            this.agaLabel2.AutoSize = true;
            this.agaLabel2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel2.ForeColor = System.Drawing.Color.White;
            this.agaLabel2.Location = new System.Drawing.Point(13, 75);
            this.agaLabel2.Name = "agaLabel2";
            this.agaLabel2.Size = new System.Drawing.Size(64, 25);
            this.agaLabel2.TabIndex = 10;
            this.agaLabel2.Text = "Setup";
            this.agaLabel2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // ZoomTextbox
            // 
            this.ZoomTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.ZoomTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ZoomTextbox.Enabled = false;
            this.ZoomTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZoomTextbox.ForeColor = System.Drawing.Color.White;
            this.ZoomTextbox.Location = new System.Drawing.Point(155, 110);
            this.ZoomTextbox.Name = "ZoomTextbox";
            this.ZoomTextbox.Size = new System.Drawing.Size(69, 16);
            this.ZoomTextbox.TabIndex = 9;
            this.ZoomTextbox.Text = "1.0";
            this.ZoomTextbox.TextChanged += new System.EventHandler(this.ZoomTextbox_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(217)))), ((int)(((byte)(156)))));
            this.panel1.Controls.Add(this.CloseLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(233, 55);
            this.panel1.TabIndex = 8;
            // 
            // CloseLabel
            // 
            this.CloseLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseLabel.Location = new System.Drawing.Point(187, 2);
            this.CloseLabel.Name = "CloseLabel";
            this.CloseLabel.Size = new System.Drawing.Size(44, 33);
            this.CloseLabel.TabIndex = 0;
            this.CloseLabel.Text = "Close";
            this.CloseLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.CloseLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.CloseLabel.Click += new System.EventHandler(this.CloseLabel_Click);
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(187)))), ((int)(((byte)(191)))));
            this.TopPanel.Controls.Add(this.agaLabel1);
            this.TopPanel.Controls.Add(this.agaProgress1);
            this.TopPanel.Controls.Add(this.FileNamePanel);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(641, 104);
            this.TopPanel.TabIndex = 9;
            // 
            // agaLabel1
            // 
            this.agaLabel1.AutoSize = true;
            this.agaLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agaLabel1.ForeColor = System.Drawing.Color.White;
            this.agaLabel1.Location = new System.Drawing.Point(41, 66);
            this.agaLabel1.Name = "agaLabel1";
            this.agaLabel1.Size = new System.Drawing.Size(148, 21);
            this.agaLabel1.TabIndex = 7;
            this.agaLabel1.Text = "Operation progress:";
            this.agaLabel1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // agaProgress1
            // 
            this.agaProgress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.agaProgress1.DataBindings.Add(new System.Windows.Forms.Binding("ProgressColor", global::gcodeparser.Properties.Settings.Default, "ProgressColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.agaProgress1.Location = new System.Drawing.Point(189, 69);
            this.agaProgress1.Maximum = 100;
            this.agaProgress1.Minimum = 0;
            this.agaProgress1.Name = "agaProgress1";
            this.agaProgress1.ProgressBackColor = System.Drawing.Color.White;
            this.agaProgress1.ProgressColor = global::gcodeparser.Properties.Settings.Default.ProgressColor;
            this.agaProgress1.Size = new System.Drawing.Size(445, 19);
            this.agaProgress1.TabIndex = 8;
            this.agaProgress1.Text = "agaProgress1";
            this.agaProgress1.Value = 20;
            // 
            // FileNamePanel
            // 
            this.FileNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(201)))), ((int)(((byte)(112)))));
            this.FileNamePanel.Controls.Add(this.FileNameLabel);
            this.FileNamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FileNamePanel.Location = new System.Drawing.Point(0, 0);
            this.FileNamePanel.Name = "FileNamePanel";
            this.FileNamePanel.Padding = new System.Windows.Forms.Padding(40, 10, 10, 10);
            this.FileNamePanel.Size = new System.Drawing.Size(641, 55);
            this.FileNamePanel.TabIndex = 6;
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FileNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileNameLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileNameLabel.ForeColor = System.Drawing.Color.White;
            this.FileNameLabel.Location = new System.Drawing.Point(40, 10);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(591, 35);
            this.FileNameLabel.TabIndex = 6;
            this.FileNameLabel.Text = "Click here to open CNC file...";
            this.FileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.FileNameLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.FileNameLabel.Click += new System.EventHandler(this.FileNameLabel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.*";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "All files (*.*)|*.*";
            this.openFileDialog1.Title = "Open G-code file";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(118)))), ((int)(((byte)(127)))));
            this.ClientSize = new System.Drawing.Size(874, 566);
            this.Controls.Add(this.RenderPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.RightPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "MainForm";
            this.Text = "Dave\'s G-code emulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CurrentLineTrackbar)).EndInit();
            this.RightPanel.ResumeLayout(false);
            this.RightPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.FileNamePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.TrackBar CurrentLineTrackbar;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.Panel FileNamePanel;
        private System.Windows.Forms.Panel panel1;
        private AGAControls.AgaLabel FileNameLabel;
        private AGAControls.AgaLabel agaLabel1;
        private AGAControls.AgaProgress agaProgress1;
        private AGAControls.AgaLabel agaLabel3;
        private AGAControls.AgaLabel agaLabel2;
        private AGAControls.AgaTextbox ZoomTextbox;
        private AGAControls.AgaTextbox CurrentLineTextbox;
        private AGAControls.AgaLabel agaLabel4;
        private AGAControls.AgaLabel agaLabel5;
        private AGAControls.AgaTextbox agaTextbox1;
        private AGAControls.AgaLabel agaLabel6;
        private AGAControls.AgaTextbox SpindleDiameterTextbox;
        private AGAControls.AgaLabel CloseLabel;
        private AGAControls.AgaLabel CheckMathLabel;
        private AGAControls.AgaLabel agaLabel8;
        private AGAControls.AgaTextbox TotalDistanceTextbox;
        private AGAControls.AgaLabel agaLabel7;
        private AGAControls.AgaLabel agaLabel9;
        private AGAControls.AgaTextbox EstimatedTimeTextbox;
        private AGAControls.AgaLabel ConnectionStatusLabel;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button CancelMachiningButton;
        private AGAControls.AgaLabel agaLabel11;
        private AGAControls.AgaLabel agaLabel10;
        private AGAControls.AgaLabel MachineStatusLabel;
        private System.Windows.Forms.Button ManualModeButton;
    }
}

