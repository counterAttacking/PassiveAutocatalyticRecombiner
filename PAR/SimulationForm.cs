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
            simulation.Run(inputTimeStep, inputDt);
            ShowResult();
        }

        private void ShowResult()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < (inputTimeStep + 1); i++)
            {
                stringBuilder.Append((i + 1) * inputDt);
                stringBuilder.Append(" ");
                for (int j = 1; j < (inputSpaceStep + 1); j++)
                {
                    stringBuilder.Append(simulation.GetU(i, j));
                    stringBuilder.Append(" ");
                }
                stringBuilder.AppendLine();
            }
            textBox1.Text = stringBuilder.ToString();
        }
    }
}
