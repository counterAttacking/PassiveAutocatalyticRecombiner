using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAR
{
    public partial class SimulationChartInputForm : Form
    {
        private double timeStep;

        public SimulationChartInputForm()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var timeStep = txtTimeStep.Text;

            if (String.IsNullOrEmpty(timeStep))
            {
                MessageBox.Show("입력 값을 모두 입력하지 않았습니다!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                this.timeStep = Convert.ToDouble(timeStep);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ((SimulationChartForm)(this.Owner)).timeStep = this.timeStep;
            ((SimulationChartForm)(this.Owner)).ShowResult();

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
