namespace PAR
{
    partial class SimulationChartForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationChartForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiInputValues = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlSection3 = new System.Windows.Forms.Panel();
            this.gphH2Rate = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlSection3Header = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSection2 = new System.Windows.Forms.Panel();
            this.gphTemperature = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlSection2Header = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlSection1 = new System.Windows.Forms.Panel();
            this.gphVelocity = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlSection1Header = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlSection3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gphH2Rate)).BeginInit();
            this.pnlSection3Header.SuspendLayout();
            this.pnlSection2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gphTemperature)).BeginInit();
            this.pnlSection2Header.SuspendLayout();
            this.pnlSection1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gphVelocity)).BeginInit();
            this.pnlSection1Header.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.tsmiInputValues});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClose});
            this.fileFToolStripMenuItem.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(67, 25);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(180, 26);
            this.tsmiClose.Text = "Close(&C)";
            this.tsmiClose.Click += new System.EventHandler(this.TsmiClose_Click);
            // 
            // tsmiInputValues
            // 
            this.tsmiInputValues.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.tsmiInputValues.Name = "tsmiInputValues";
            this.tsmiInputValues.Size = new System.Drawing.Size(136, 25);
            this.tsmiInputValues.Text = "Input Values(&I)";
            this.tsmiInputValues.Click += new System.EventHandler(this.TsmiInputValues_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 29);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(800, 421);
            this.pnlMain.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pnlSection3);
            this.panel1.Controls.Add(this.pnlSection2);
            this.panel1.Controls.Add(this.pnlSection1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 421);
            this.panel1.TabIndex = 3;
            // 
            // pnlSection3
            // 
            this.pnlSection3.Controls.Add(this.gphH2Rate);
            this.pnlSection3.Controls.Add(this.pnlSection3Header);
            this.pnlSection3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection3.Location = new System.Drawing.Point(0, 1000);
            this.pnlSection3.Name = "pnlSection3";
            this.pnlSection3.Size = new System.Drawing.Size(783, 500);
            this.pnlSection3.TabIndex = 2;
            // 
            // gphH2Rate
            // 
            chartArea1.Name = "ChartArea1";
            this.gphH2Rate.ChartAreas.Add(chartArea1);
            this.gphH2Rate.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.gphH2Rate.Legends.Add(legend1);
            this.gphH2Rate.Location = new System.Drawing.Point(0, 50);
            this.gphH2Rate.Name = "gphH2Rate";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.gphH2Rate.Series.Add(series1);
            this.gphH2Rate.Size = new System.Drawing.Size(783, 450);
            this.gphH2Rate.TabIndex = 2;
            this.gphH2Rate.Text = "chart1";
            // 
            // pnlSection3Header
            // 
            this.pnlSection3Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlSection3Header.Controls.Add(this.label3);
            this.pnlSection3Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection3Header.Location = new System.Drawing.Point(0, 0);
            this.pnlSection3Header.Name = "pnlSection3Header";
            this.pnlSection3Header.Size = new System.Drawing.Size(783, 50);
            this.pnlSection3Header.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(24, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(269, 31);
            this.label3.TabIndex = 1;
            this.label3.Text = "The Result of H₂ Rate";
            // 
            // pnlSection2
            // 
            this.pnlSection2.Controls.Add(this.gphTemperature);
            this.pnlSection2.Controls.Add(this.pnlSection2Header);
            this.pnlSection2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection2.Location = new System.Drawing.Point(0, 500);
            this.pnlSection2.Name = "pnlSection2";
            this.pnlSection2.Size = new System.Drawing.Size(783, 500);
            this.pnlSection2.TabIndex = 1;
            // 
            // gphTemperature
            // 
            chartArea2.Name = "ChartArea1";
            this.gphTemperature.ChartAreas.Add(chartArea2);
            this.gphTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.gphTemperature.Legends.Add(legend2);
            this.gphTemperature.Location = new System.Drawing.Point(0, 50);
            this.gphTemperature.Name = "gphTemperature";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.gphTemperature.Series.Add(series2);
            this.gphTemperature.Size = new System.Drawing.Size(783, 450);
            this.gphTemperature.TabIndex = 2;
            this.gphTemperature.Text = "chart1";
            // 
            // pnlSection2Header
            // 
            this.pnlSection2Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlSection2Header.Controls.Add(this.label2);
            this.pnlSection2Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection2Header.Location = new System.Drawing.Point(0, 0);
            this.pnlSection2Header.Name = "pnlSection2Header";
            this.pnlSection2Header.Size = new System.Drawing.Size(783, 50);
            this.pnlSection2Header.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(24, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(329, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "The Result of Temperature";
            // 
            // pnlSection1
            // 
            this.pnlSection1.Controls.Add(this.gphVelocity);
            this.pnlSection1.Controls.Add(this.pnlSection1Header);
            this.pnlSection1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection1.Location = new System.Drawing.Point(0, 0);
            this.pnlSection1.Name = "pnlSection1";
            this.pnlSection1.Size = new System.Drawing.Size(783, 500);
            this.pnlSection1.TabIndex = 0;
            // 
            // gphVelocity
            // 
            chartArea3.Name = "ChartArea1";
            this.gphVelocity.ChartAreas.Add(chartArea3);
            this.gphVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.gphVelocity.Legends.Add(legend3);
            this.gphVelocity.Location = new System.Drawing.Point(0, 50);
            this.gphVelocity.Name = "gphVelocity";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.gphVelocity.Series.Add(series3);
            this.gphVelocity.Size = new System.Drawing.Size(783, 450);
            this.gphVelocity.TabIndex = 1;
            this.gphVelocity.Text = "chart1";
            // 
            // pnlSection1Header
            // 
            this.pnlSection1Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlSection1Header.Controls.Add(this.label1);
            this.pnlSection1Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection1Header.Location = new System.Drawing.Point(0, 0);
            this.pnlSection1Header.Name = "pnlSection1Header";
            this.pnlSection1Header.Size = new System.Drawing.Size(783, 50);
            this.pnlSection1Header.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "The Result of Velocity";
            // 
            // SimulationChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SimulationChartForm";
            this.Text = "Chart Page";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlSection3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gphH2Rate)).EndInit();
            this.pnlSection3Header.ResumeLayout(false);
            this.pnlSection3Header.PerformLayout();
            this.pnlSection2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gphTemperature)).EndInit();
            this.pnlSection2Header.ResumeLayout(false);
            this.pnlSection2Header.PerformLayout();
            this.pnlSection1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gphVelocity)).EndInit();
            this.pnlSection1Header.ResumeLayout(false);
            this.pnlSection1Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiInputValues;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlSection3;
        private System.Windows.Forms.DataVisualization.Charting.Chart gphH2Rate;
        private System.Windows.Forms.Panel pnlSection3Header;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlSection2;
        private System.Windows.Forms.DataVisualization.Charting.Chart gphTemperature;
        private System.Windows.Forms.Panel pnlSection2Header;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlSection1;
        private System.Windows.Forms.DataVisualization.Charting.Chart gphVelocity;
        private System.Windows.Forms.Panel pnlSection1Header;
        private System.Windows.Forms.Label label1;
    }
}