namespace PAR
{
    partial class SimulationForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiInputValues = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRunSimulation = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlSection1 = new System.Windows.Forms.Panel();
            this.dgvU = new System.Windows.Forms.DataGridView();
            this.pnlSection1Header = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSection2 = new System.Windows.Forms.Panel();
            this.dgvTemperature = new System.Windows.Forms.DataGridView();
            this.pnlSection2Header = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlSection3 = new System.Windows.Forms.Panel();
            this.dgvH2Rate = new System.Windows.Forms.DataGridView();
            this.pnlSection3Header = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tslblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlSection1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvU)).BeginInit();
            this.pnlSection1Header.SuspendLayout();
            this.pnlSection2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemperature)).BeginInit();
            this.pnlSection2Header.SuspendLayout();
            this.pnlSection3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvH2Rate)).BeginInit();
            this.pnlSection3Header.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.menuStrip1.Font = new System.Drawing.Font("맑은 고딕", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiInputValues,
            this.tsmiRunSimulation});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(788, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiInputValues
            // 
            this.tsmiInputValues.ForeColor = System.Drawing.Color.White;
            this.tsmiInputValues.Name = "tsmiInputValues";
            this.tsmiInputValues.Size = new System.Drawing.Size(139, 29);
            this.tsmiInputValues.Text = "Input Values";
            this.tsmiInputValues.Click += new System.EventHandler(this.TsmiInputValues_Click);
            // 
            // tsmiRunSimulation
            // 
            this.tsmiRunSimulation.ForeColor = System.Drawing.Color.White;
            this.tsmiRunSimulation.Name = "tsmiRunSimulation";
            this.tsmiRunSimulation.Size = new System.Drawing.Size(164, 29);
            this.tsmiRunSimulation.Text = "Run Simulation";
            this.tsmiRunSimulation.Click += new System.EventHandler(this.TsmiRunSimulation_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 578);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(788, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.pnlSection3);
            this.pnlMain.Controls.Add(this.pnlSection2);
            this.pnlMain.Controls.Add(this.pnlSection1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 33);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(788, 545);
            this.pnlMain.TabIndex = 2;
            // 
            // pnlSection1
            // 
            this.pnlSection1.Controls.Add(this.dgvU);
            this.pnlSection1.Controls.Add(this.pnlSection1Header);
            this.pnlSection1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection1.Location = new System.Drawing.Point(0, 0);
            this.pnlSection1.Name = "pnlSection1";
            this.pnlSection1.Size = new System.Drawing.Size(771, 300);
            this.pnlSection1.TabIndex = 0;
            // 
            // dgvU
            // 
            this.dgvU.AllowUserToAddRows = false;
            this.dgvU.AllowUserToDeleteRows = false;
            this.dgvU.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvU.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvU.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvU.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvU.Location = new System.Drawing.Point(0, 50);
            this.dgvU.Name = "dgvU";
            this.dgvU.ReadOnly = true;
            this.dgvU.RowHeadersVisible = false;
            this.dgvU.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold);
            this.dgvU.RowsDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvU.RowTemplate.Height = 23;
            this.dgvU.Size = new System.Drawing.Size(771, 250);
            this.dgvU.TabIndex = 1;
            // 
            // pnlSection1Header
            // 
            this.pnlSection1Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlSection1Header.Controls.Add(this.label1);
            this.pnlSection1Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection1Header.Location = new System.Drawing.Point(0, 0);
            this.pnlSection1Header.Name = "pnlSection1Header";
            this.pnlSection1Header.Size = new System.Drawing.Size(771, 50);
            this.pnlSection1Header.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "The Result of U";
            // 
            // pnlSection2
            // 
            this.pnlSection2.Controls.Add(this.dgvTemperature);
            this.pnlSection2.Controls.Add(this.pnlSection2Header);
            this.pnlSection2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection2.Location = new System.Drawing.Point(0, 300);
            this.pnlSection2.Name = "pnlSection2";
            this.pnlSection2.Size = new System.Drawing.Size(771, 300);
            this.pnlSection2.TabIndex = 1;
            // 
            // dgvTemperature
            // 
            this.dgvTemperature.AllowUserToAddRows = false;
            this.dgvTemperature.AllowUserToDeleteRows = false;
            this.dgvTemperature.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTemperature.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTemperature.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvTemperature.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTemperature.Location = new System.Drawing.Point(0, 50);
            this.dgvTemperature.Name = "dgvTemperature";
            this.dgvTemperature.ReadOnly = true;
            this.dgvTemperature.RowHeadersVisible = false;
            this.dgvTemperature.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold);
            this.dgvTemperature.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvTemperature.RowTemplate.Height = 23;
            this.dgvTemperature.Size = new System.Drawing.Size(771, 250);
            this.dgvTemperature.TabIndex = 1;
            // 
            // pnlSection2Header
            // 
            this.pnlSection2Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlSection2Header.Controls.Add(this.label2);
            this.pnlSection2Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection2Header.Location = new System.Drawing.Point(0, 0);
            this.pnlSection2Header.Name = "pnlSection2Header";
            this.pnlSection2Header.Size = new System.Drawing.Size(771, 50);
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
            // pnlSection3
            // 
            this.pnlSection3.Controls.Add(this.dgvH2Rate);
            this.pnlSection3.Controls.Add(this.pnlSection3Header);
            this.pnlSection3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection3.Location = new System.Drawing.Point(0, 600);
            this.pnlSection3.Name = "pnlSection3";
            this.pnlSection3.Size = new System.Drawing.Size(771, 300);
            this.pnlSection3.TabIndex = 2;
            // 
            // dgvH2Rate
            // 
            this.dgvH2Rate.AllowUserToAddRows = false;
            this.dgvH2Rate.AllowUserToDeleteRows = false;
            this.dgvH2Rate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvH2Rate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvH2Rate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvH2Rate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvH2Rate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvH2Rate.Location = new System.Drawing.Point(0, 50);
            this.dgvH2Rate.Name = "dgvH2Rate";
            this.dgvH2Rate.ReadOnly = true;
            this.dgvH2Rate.RowHeadersVisible = false;
            this.dgvH2Rate.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold);
            this.dgvH2Rate.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.dgvH2Rate.RowTemplate.Height = 23;
            this.dgvH2Rate.Size = new System.Drawing.Size(771, 250);
            this.dgvH2Rate.TabIndex = 1;
            // 
            // pnlSection3Header
            // 
            this.pnlSection3Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.pnlSection3Header.Controls.Add(this.label3);
            this.pnlSection3Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection3Header.Location = new System.Drawing.Point(0, 0);
            this.pnlSection3Header.Name = "pnlSection3Header";
            this.pnlSection3Header.Size = new System.Drawing.Size(771, 50);
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
            // tslblStatus
            // 
            this.tslblStatus.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tslblStatus.Name = "tslblStatus";
            this.tslblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // SimulationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 600);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimulationForm";
            this.Text = "SimulationForm";
            this.Load += new System.EventHandler(this.SimulationForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlSection1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvU)).EndInit();
            this.pnlSection1Header.ResumeLayout(false);
            this.pnlSection1Header.PerformLayout();
            this.pnlSection2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemperature)).EndInit();
            this.pnlSection2Header.ResumeLayout(false);
            this.pnlSection2Header.PerformLayout();
            this.pnlSection3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvH2Rate)).EndInit();
            this.pnlSection3Header.ResumeLayout(false);
            this.pnlSection3Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiInputValues;
        private System.Windows.Forms.ToolStripMenuItem tsmiRunSimulation;
        private System.Windows.Forms.Panel pnlSection1;
        private System.Windows.Forms.DataGridView dgvU;
        private System.Windows.Forms.Panel pnlSection1Header;
        private System.Windows.Forms.Panel pnlSection3;
        private System.Windows.Forms.DataGridView dgvH2Rate;
        private System.Windows.Forms.Panel pnlSection3Header;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlSection2;
        private System.Windows.Forms.DataGridView dgvTemperature;
        private System.Windows.Forms.Panel pnlSection2Header;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatus;
    }
}