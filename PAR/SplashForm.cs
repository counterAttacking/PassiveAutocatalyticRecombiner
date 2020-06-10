using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAR
{
    public partial class SplashForm : Form
    {
        delegate void LoadingProgressDelegate(int i);
        delegate void CloseDelegate();

        public SplashForm()
        {
            InitializeComponent();
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            Thread loadingThread = new Thread(LoadingThread);
            loadingThread.Start();
        }

        private void LoadingThread()
        {
            for (int i = 0; i <= 100; i++)
            {
                this.Invoke(new LoadingProgressDelegate(LoadingStep), i);
                Thread.Sleep(50);
            }
            Thread.Sleep(1000);
            this.Invoke(new CloseDelegate(SplashFormClose));
        }

        private void LoadingStep(int step)
        {
            progressBar1.Value = step;
        }

        private void SplashFormClose()
        {
            this.Close();
        }
    }
}
