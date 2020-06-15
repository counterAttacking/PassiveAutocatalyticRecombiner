using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAR
{
    public class Simulation
    {
        private int nTime, nSpace, sequence;
        private double vk, a, beforeDt, dx, p = 1, tempInf;
        private List<double> time, space, wk;
        private List<List<double>> density, u, temperature;
        private List<List<List<double>>> compCTR, wDot, yk;

        public Simulation(int nTime, int nSpace, double beforeDt, double dx)
        {
            time = new List<double>();
            space = new List<double>();
            wk = new List<double>();
            density = new List<List<double>>();
            u = new List<List<double>>();
            temperature = new List<List<double>>();
            compCTR = new List<List<List<double>>>();
            wDot = new List<List<List<double>>>();
            yk = new List<List<List<double>>>();

            vk = 0;
            this.nTime = nTime;
            this.nSpace = nSpace;
            a = 1;
            this.beforeDt = beforeDt;
            this.dx = dx;
            sequence = 1;
        }

        public void Initialize(double density, double temperature, double H2Rate, double steamCTR)
        {
            tempInf = temperature;
            double r1 = 0.000082, p1 = 1;

            wk.Add(1.00794); // H's g/mol
            wk.Add(2.01588); // H2's g/mol
            wk.Add(15.9994); // O's g/mol
            wk.Add(31.9988); // O2's g/mol
            wk.Add(17.00734); // OH's g/mol
            wk.Add(18.01528); // H20's g/mol
            wk.Add(33.00674); // H02's g/mol
            wk.Add(28.0134); // N2's g/mol

            for (int i = 0; i < nTime + 1; i++)
            {
                time.Add(0);
                this.density.Add(new List<double>());
                this.u.Add(new List<double>());
                this.temperature.Add(new List<double>());
                this.compCTR.Add(new List<List<double>>());
                this.yk.Add(new List<List<double>>());

                for (int j = 0; j < nSpace + 1; j++)
                {
                    this.density[i].Add(density);
                    this.u[i].Add(0);
                    this.temperature[i].Add(temperature);
                    vk = 0;

                    if (i == 0)
                    {
                        space.Add(0);
                    }

                    this.compCTR[i].Add(new List<double>());
                    this.yk[i].Add(new List<double>());
                    for (int k = 0; k < 9; k++)
                    {
                        if (k == 1)
                        {
                            var tmp = H2Rate * p1 / r1 / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else if (k == 3)
                        {
                            var tmp = (100 - H2Rate - steamCTR) * 0.23 * p1 / r1 / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else if (k == 5)
                        {
                            var tmp = steamCTR * p1 / r1 / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else if (k == 7)
                        {
                            var tmp = (100 - H2Rate - steamCTR) * 0.77 * p1 / r1 / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else
                        {
                            this.compCTR[i][j].Add(0);
                        }
                        this.yk[i][j].Add(0);
                    }
                }
            }
        }

        public double Polynomial(double x, double p1, double p2, double p3, double p4, double p5, double p6, double p7, double p8, double p9)
        {
            double ans = p1 * Math.Pow(x, 8) + p2 * Math.Pow(x, 7) + p3 * Math.Pow(x, 6) + p4 * Math.Pow(x, 5) + p5 * Math.Pow(x, 4) + p6 * Math.Pow(x, 3) + p7 * Math.Pow(x, 2) + p8 * Math.Pow(x, 1) + p9;
            return ans;
        }
    }
}
