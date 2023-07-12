namespace header_tool
{
    partial class Mainform
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        partial class ProcessFilePanel
        {
            public ProcessFilePanel()
            {
            }
            public ProcessFilePanel(int tabIndex, ProcessFileConfig processFileConfig)
            {
                InitFileProcessPanel(tabIndex, processFileConfig);
            }

            public void InitFileProcessPanel(int tabIndex, ProcessFileConfig processFileConfig)
            {
                PanelAttributeInit(processFileConfig);
                
                this.Panel = new System.Windows.Forms.Panel();
                this.prgDownload = new System.Windows.Forms.ProgressBar();
                this.lblDownload = new System.Windows.Forms.Label();
                this.UrlTextBox = new System.Windows.Forms.TextBox();
                this.checkBoxUrl = new System.Windows.Forms.CheckBox();
                this.FileName = new System.Windows.Forms.Label();
                this.Panel.SuspendLayout();
                // 
                // Panel
                // 
                this.Panel.AutoSize = true;
                this.Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.Panel.BackColor = System.Drawing.SystemColors.Control;
                this.Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.Panel.Controls.Add(this.FileName);
                this.Panel.Controls.Add(this.UrlTextBox);
                this.Panel.Controls.Add(this.checkBoxUrl);
                this.Panel.Controls.Add(this.prgDownload);
                this.Panel.Controls.Add(this.lblDownload);
                this.Panel.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
                this.Panel.Name = "Panel";
                this.Panel.Size = new System.Drawing.Size(478, 95);
                this.Panel.TabIndex = tabIndex;
                // 
                // FileName
                // 
                this.FileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.FileName.AutoSize = true;
                this.FileName.Margin = new System.Windows.Forms.Padding(3);
                this.FileName.Name = "FileName";
                this.FileName.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
                this.FileName.TabIndex = 0;
                this.FileName.Text = this.srcFileName;
                // 
                // OutputTextBox
                // 
                this.UrlTextBox.Location = new System.Drawing.Point(3, 20);
                this.UrlTextBox.Multiline = true;
                this.UrlTextBox.Name = "OutputTextBox";
                this.UrlTextBox.Size = new System.Drawing.Size(358, 70);
                this.UrlTextBox.TabIndex = 1;
                this.UrlTextBox.Text = "";
                this.UrlTextBox.TextChanged += new System.EventHandler(this.UrlTextBox_TextChanged);
                // 
                // checkBoxUrl
                // 
                this.checkBoxUrl.AutoCheck = false;
                this.checkBoxUrl.AutoSize = true;
                this.checkBoxUrl.Enabled = false;
                this.checkBoxUrl.Checked = false;
                this.checkBoxUrl.Location = new System.Drawing.Point(419, 23);
                this.checkBoxUrl.Name = "checkBoxUrl";
                this.checkBoxUrl.Size = new System.Drawing.Size(15, 14);
                this.checkBoxUrl.TabIndex = 2;
                this.checkBoxUrl.UseVisualStyleBackColor = true;
                // 
                // prgDownload
                // 
                this.prgDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                this.prgDownload.Location = new System.Drawing.Point(367, 43);
                this.prgDownload.Name = "prgDownload";
                this.prgDownload.Size = new System.Drawing.Size(106, 22);
                this.prgDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                this.prgDownload.TabIndex = 3;
                // 
                // lblDownload
                // 
                this.lblDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                this.lblDownload.AutoSize = true;
                this.lblDownload.Location = new System.Drawing.Point(374, 68);
                this.lblDownload.Name = "lblDownload";
                this.lblDownload.Size = new System.Drawing.Size(89, 12);
                this.lblDownload.TabIndex = 4;
                this.lblDownload.Text = "Downloading 0%";
            }

            public System.Windows.Forms.Panel Panel;
            private System.Windows.Forms.Label FileName;
            private System.Windows.Forms.CheckBox checkBoxUrl;
            private System.Windows.Forms.TextBox UrlTextBox;
            private System.Windows.Forms.Label lblDownload;
            private System.Windows.Forms.ProgressBar prgDownload;
        }

        partial class OutputPanel
        {
            public void InitOutputPanel(int tabIndex)
            {
                this.Panel = new System.Windows.Forms.Panel();
                this.OutputTextBox = new System.Windows.Forms.TextBox();
                this.PanelName = new System.Windows.Forms.Label();
                this.Panel.SuspendLayout();
                // 
                // Panel
                // 
                this.Panel.AutoSize = true;
                this.Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.Panel.BackColor = System.Drawing.SystemColors.Control;
                this.Panel.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.Panel.Controls.Add(this.PanelName);
                this.Panel.Controls.Add(this.OutputTextBox);
                //this.Panel.Location = new System.Drawing.Point(10, 3);
                this.Panel.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
                this.Panel.Name = "Panel";
                this.Panel.TabIndex = tabIndex;
                // 
                // FileName
                // 
                this.PanelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
                this.PanelName.AutoSize = true;
                //this.FileName.Location = new System.Drawing.Point(202, -1);
                this.PanelName.Margin = new System.Windows.Forms.Padding(3);
                this.PanelName.Name = "Output";
                this.PanelName.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
                //this.FileName.Size = new System.Drawing.Size(71, 15);
                this.PanelName.TabIndex = 0;
                this.PanelName.Text = "log output";
                // 
                // OutputTextBox
                // 
                this.OutputTextBox.Location = new System.Drawing.Point(3, 20);
                this.OutputTextBox.Multiline = true;
                this.OutputTextBox.Name = "OutputTextBox";
                this.OutputTextBox.Size = new System.Drawing.Size(450, 100);
                this.OutputTextBox.TabIndex = 1;
                this.OutputTextBox.ReadOnly = true;
                this.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                this.OutputTextBox.HideSelection = false;
            }

            public void TextOutputLn(string outputBuffer)
            {
                this.TextOutput(outputBuffer + "\r\n");
            }

            public void TextOutput(string outputBuffer)
            {
                this.OutputTextBox.AppendText(outputBuffer);
                this.OutputTextBox.ScrollToCaret();
                // 用于焦点去除
                this.OutputTextBox.Enabled = false;
                this.OutputTextBox.Enabled = true;
            }

            public System.Windows.Forms.Panel Panel;
            private System.Windows.Forms.Label PanelName;
            public System.Windows.Forms.TextBox OutputTextBox;
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            int tableIndex;

            this.btStart = new System.Windows.Forms.Button();
            this.mainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();

            this.subPanel = new ProcessFilePanel[this.config.Length];
            for (tableIndex = 0; tableIndex < subPanel.Length && tableIndex < 6; tableIndex++)
            {
                this.subPanel[tableIndex] = new ProcessFilePanel(tableIndex, this.config[tableIndex]);
            }

            // 
            // btStart
            // 
            this.btStart.AutoSize = true;
            this.btStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btStart.Margin = new System.Windows.Forms.Padding(150, 3, 150, 3);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(198, 22);
            this.btStart.TabIndex = tableIndex++;
            this.btStart.Text = "START";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);

            this.outputPanel = new OutputPanel();
            this.outputPanel.InitOutputPanel(tableIndex++);

            foreach (ProcessFilePanel Panel_tmp in this.subPanel)
            {
                this.mainLayoutPanel.Controls.Add(Panel_tmp.Panel);
            }

            this.mainLayoutPanel.Controls.Add(this.btStart);
            this.mainLayoutPanel.Controls.Add(this.outputPanel.Panel);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // Mainform
            // 

            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, this.mainLayoutPanel.Height * tableIndex);
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "Mainform";
            this.Text = "Header Tool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Mainform_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ProcessFilePanel[] subPanel;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.FlowLayoutPanel mainLayoutPanel;
        private OutputPanel outputPanel;
    }
}

