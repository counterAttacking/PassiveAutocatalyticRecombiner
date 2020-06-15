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
    public partial class MainForm : Form
    {
        private int simulationIdx = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void TsmiNewSimulation_Click(object sender, EventArgs e)
        {
            simulationIdx += 1;
            var tabPageText = string.Format("Simulation {0}", simulationIdx);
            var frm = new SimulationForm
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
            };
            frm.Show();
            tabControl1.TabPages.Add(tabPageText);
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm);
            tabControl1.SelectedTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];
        }
    }
}
