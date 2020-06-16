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
        public bool isReady = false;

        public SimulationForm()
        {
            InitializeComponent();
        }

        private void SimulationForm_Load(object sender,EventArgs e)
        {
            tslblStatus.Text = "Please Input all values";
        }

        private void TsmiInputValues_Click(object sender, EventArgs e)
        {
            frmSimulationInput.Owner = this;
            frmSimulationInput.ShowDialog();
        }

        private void TsmiRunSimulation_Click(object sender, EventArgs e)
        {
            tsmiInputValues.Enabled = false;
            tsmiRunSimulation.Enabled = false;
            tslblStatus.Text = "Simulation Running!!!";

            simulation = new Simulation(inputTimeStep, inputSpaceStep, inputDt, 0.005);
            simulation.InitSetting(1, inputTemperature, inputH2Rate, 0);
            simulation.Run(inputTimeStep, inputDt);
            ShowResult();

            tsmiInputValues.Enabled = true;
            tsmiRunSimulation.Enabled = true;
            tslblStatus.Text = "Simulation Completed!!!";
        }

        public void IsReadyDone()
        {
            if(isReady==true)
            {
                tslblStatus.Text = "Ready Done!!!";
            }
            else
            {
                tslblStatus.Text = "Not Ready!!!";
            }
        }

        private void ShowResult()
        {
            ShowResultPart1();
            ShowResultPart2();
            ShowResultPart3();
        }

        private void ShowResultPart1()
        {
            dgvU.Columns.Clear();
            dgvU.Rows.Clear();
            dgvU.ColumnCount = (inputSpaceStep + 1) - 1 + 1;
            for (var i = 0; i < dgvU.ColumnCount; i++)
            {
                dgvU.Columns[i].Width = 170;
                if (i == 0)
                {
                    dgvU.Columns[i].Name = "TimeStep";
                }
                else
                {
                    dgvU.Columns[i].Name = string.Format("{0} Space Step", i);
                }
            }
            var u = simulation.GetU;
            for (int i = 0; i < (inputTimeStep + 1); i++)
            {
                dgvU.Rows.Add(string.Format("{0:0.00E+0}", (i + 1) * inputDt));
                for (int j = 1; j < (inputSpaceStep + 1); j++)
                {
                    dgvU[j, i].Value = string.Format("{0:0.00E+0}", u[i, j]);
                }
            }
        }

        private void ShowResultPart2()
        {
            var temperature = simulation.GetTemperature;
            dgvTemperature.Columns.Clear();
            dgvTemperature.Rows.Clear();
            dgvTemperature.ColumnCount = (inputSpaceStep + 1) - 1 + 1;
            for (var i = 0; i < dgvTemperature.ColumnCount; i++)
            {
                dgvTemperature.Columns[i].Width = 170;
                if (i == 0)
                {
                    dgvTemperature.Columns[i].Name = "TimeStep";
                }
                else
                {
                    dgvTemperature.Columns[i].Name = string.Format("{0} Space Step", i);
                }
            }
            for (int i = 0; i < (inputTimeStep + 1); i++)
            {
                dgvTemperature.Rows.Add(string.Format("{0:0.00E+0}", (i + 1) * inputDt));
                for (int j = 1; j < (inputSpaceStep + 1); j++)
                {
                    dgvTemperature[j, i].Value = string.Format("{0:0.00E+0}", temperature[i, j]);
                }
            }
        }

        private void ShowResultPart3()
        {
            var compCTR = simulation.GetH2;
            dgvH2Rate.Columns.Clear();
            dgvH2Rate.Rows.Clear();
            dgvH2Rate.ColumnCount = (inputSpaceStep + 1) - 1 + 1;
            for (var i = 0; i < dgvH2Rate.ColumnCount; i++)
            {
                dgvH2Rate.Columns[i].Width = 170;
                if (i == 0)
                {
                    dgvH2Rate.Columns[i].Name = "TimeStep";
                }
                else
                {
                    dgvH2Rate.Columns[i].Name = string.Format("{0} Space Step", i);
                }
            }
            for (int i = 0; i < (inputTimeStep + 1); i++)
            {
                dgvH2Rate.Rows.Add(string.Format("{0:0.00E+0}", (i + 1) * inputDt));
                for (int j = 1; j < (inputSpaceStep + 1); j++)
                {
                    dgvH2Rate[j, i].Value = string.Format("{0:0.00E+0}", compCTR[i, j, 1]);
                }
            }
        }
    }
}
