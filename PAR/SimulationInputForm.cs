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
    public partial class SimulationInputForm : Form
    {
        private double inputTemperature;
        private double inputH2Rate;
        private int inputTimeStep;
        private int inputSpaceStep;
        private double inputDt;
        private bool isReady = false;

        public SimulationInputForm()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var temperature = txtTemperature.Text;
            var H2Rate = txtH2Rate.Text;
            var timeStep = txtTimeStep.Text;
            var spaceStep = txtSpaceStep.Text;
            var Dt = txtDt.Text;

            if (String.IsNullOrEmpty(temperature) || String.IsNullOrEmpty(H2Rate) || String.IsNullOrEmpty(timeStep) || String.IsNullOrEmpty(spaceStep) || String.IsNullOrEmpty(Dt))
            {
                isReady = false;
                ((SimulationForm)(this.Owner)).isReady = isReady;
                MessageBox.Show("입력 값을 모두 입력하지 않았습니다!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            inputTemperature = Convert.ToDouble(temperature);
            inputH2Rate = Convert.ToDouble(H2Rate);
            inputTimeStep = Convert.ToInt32(timeStep);
            inputSpaceStep = Convert.ToInt32(spaceStep);
            inputDt = Convert.ToDouble(Dt);
            isReady = true;

            ((SimulationForm)(this.Owner)).inputTemperature = inputTemperature;
            ((SimulationForm)(this.Owner)).inputH2Rate = inputH2Rate;
            ((SimulationForm)(this.Owner)).inputTimeStep = inputTimeStep;
            ((SimulationForm)(this.Owner)).inputSpaceStep = inputSpaceStep;
            ((SimulationForm)(this.Owner)).inputDt = inputDt;
            ((SimulationForm)(this.Owner)).isReady = isReady;
            ((SimulationForm)(this.Owner)).IsReadyDone();

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var temperature = txtTemperature.Text;
            var H2Rate = txtH2Rate.Text;
            var timeStep = txtTimeStep.Text;
            var spaceStep = txtSpaceStep.Text;
            var Dt = txtDt.Text;

            if (String.IsNullOrEmpty(temperature) || String.IsNullOrEmpty(H2Rate) || String.IsNullOrEmpty(timeStep) || String.IsNullOrEmpty(spaceStep) || String.IsNullOrEmpty(Dt))
            {
                isReady = false;
                ((SimulationForm)(this.Owner)).isReady = isReady;
            }

            ((SimulationForm)(this.Owner)).IsReadyDone();
            this.Close();
        }
    }
}
