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
    public partial class SimulationForm : Form
    {
        private Simulation simulation;

        private SimulationInputForm frmSimulationInput = new SimulationInputForm();

        public double inputTemperature;
        public double inputH2Rate;
        public int inputTimeStep;
        public int inputSpaceStep;
        public double inputDt;

        public SimulationForm()
        {
            InitializeComponent();
        }

        private void TsmiInputValues_Click(object sender, EventArgs e)
        {
            frmSimulationInput.Owner = this;
            frmSimulationInput.ShowDialog();
        }

        private void TsmiRunSimulation_Click(object sender, EventArgs e)
        {
            simulation = new Simulation(inputTimeStep, inputSpaceStep, inputDt, 0.005);
            simulation.InitSetting(1, inputTemperature, inputH2Rate, 0);
        }
    }
}
