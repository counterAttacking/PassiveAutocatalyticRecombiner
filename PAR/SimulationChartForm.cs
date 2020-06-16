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
    public partial class SimulationChartForm : Form
    {
        private double[,] u;
        private double[,] temperature;
        private double[,,] compCTR;
        private int inputTimeStep;
        private int inputSpaceStep;
        private double inputDt;

        private SimulationChartInputForm frmSimulationChartInput = new SimulationChartInputForm();
        public double timeStep;

        public SimulationChartForm()
        {
            InitializeComponent();
        }

        public SimulationChartForm(double[,] u, double[,] temperature, double[,,] compCTR, int inputTimeStep, int inputSpaceStep, double inputDt)
        {
            InitializeComponent();
            this.u = u;
            this.temperature = temperature;
            this.compCTR = compCTR;
            this.inputTimeStep = inputTimeStep;
            this.inputSpaceStep = inputSpaceStep;
            this.inputDt = inputDt;
        }

        private void TsmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TsmiInputValues_Click(object sender, EventArgs e)
        {
            frmSimulationChartInput.Owner = this;
            frmSimulationChartInput.ShowDialog();
        }

        public void ShowResult()
        {
            gphVelocity.Series["Series1"].Points.Clear();
            gphTemperature.Series["Series1"].Points.Clear();
            gphH2Rate.Series["Series1"].Points.Clear();

            try
            {
                for (int i = 0; i < (inputTimeStep + 1); i++)
                {
                    if ((i + 1) * inputDt == timeStep)
                    {
                        for (int j = 1; j < (inputSpaceStep + 1); j++)
                        {
                            gphVelocity.Series["Series1"].Points.AddXY(string.Format("{0:0.00E+0}", timeStep), string.Format("{0:0.00E+0}", u[i, j]));
                            gphTemperature.Series["Series1"].Points.AddXY(string.Format("{0:0.00E+0}", timeStep), string.Format("{0:0.00E+0}", temperature[i, j]));
                            gphH2Rate.Series["Series1"].Points.AddXY(string.Format("{0:0.00E+0}", timeStep), string.Format("{0:0.00E+0}", compCTR[i, j, 1]));
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
