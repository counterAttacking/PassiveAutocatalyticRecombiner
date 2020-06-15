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

        public double inputTemperature;
        public double inputH2Rate;
        public double inputTimeStep;
        public double inputSpaceStep;
        public double inputDt;

        public SimulationForm()
        {
            InitializeComponent();
        }

        private void TsmiInputValues_Click(object sender, EventArgs e)
        {
            var frmsimulationInput = new SimulationInputForm();
            frmsimulationInput.Owner = this;
            frmsimulationInput.Show();
        }

        private void TsmiRunSimulation_Click(object sender, EventArgs e)
        {

        }
    }
}
