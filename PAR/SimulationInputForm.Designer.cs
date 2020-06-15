namespace PAR
{
    partial class SimulationInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationInputForm));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSpaceStep = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTimeStep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtH2Rate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTemperature = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input for PAR Simulation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDt);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtSpaceStep);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTimeStep);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtH2Rate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTemperature);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(10, 61);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(383, 223);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtDt
            // 
            this.txtDt.Location = new System.Drawing.Point(198, 182);
            this.txtDt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDt.Name = "txtDt";
            this.txtDt.Size = new System.Drawing.Size(147, 21);
            this.txtDt.TabIndex = 4;
            this.txtDt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "Dt";
            // 
            // txtSpaceStep
            // 
            this.txtSpaceStep.Location = new System.Drawing.Point(198, 144);
            this.txtSpaceStep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSpaceStep.Name = "txtSpaceStep";
            this.txtSpaceStep.Size = new System.Drawing.Size(147, 21);
            this.txtSpaceStep.TabIndex = 3;
            this.txtSpaceStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(64, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "Space Step";
            // 
            // txtTimeStep
            // 
            this.txtTimeStep.Location = new System.Drawing.Point(198, 105);
            this.txtTimeStep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTimeStep.Name = "txtTimeStep";
            this.txtTimeStep.Size = new System.Drawing.Size(147, 21);
            this.txtTimeStep.TabIndex = 2;
            this.txtTimeStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Time Step";
            // 
            // txtH2Rate
            // 
            this.txtH2Rate.Location = new System.Drawing.Point(198, 61);
            this.txtH2Rate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtH2Rate.Name = "txtH2Rate";
            this.txtH2Rate.Size = new System.Drawing.Size(147, 21);
            this.txtH2Rate.TabIndex = 1;
            this.txtH2Rate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "H₂ Rate";
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(198, 18);
            this.txtTemperature.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(147, 21);
            this.txtTemperature.TabIndex = 0;
            this.txtTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Temperature";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(57, 301);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 50);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(233, 301);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 50);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SimulationInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 362);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimulationInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtDt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSpaceStep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTimeStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtH2Rate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.Label label2;
    }
}