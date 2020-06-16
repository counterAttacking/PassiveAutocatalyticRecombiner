using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAR
{
    public class Simulation
    {
        private int nTime, nSpace, sequence;
        private double Vk, A, Dt, dx, pressure = 1, tempInf;
        private double[] time, space, Wk;
        private double[,] density, u, temperature;
        private double[,,] compCTR, wDot, Yk;

        private double[,] K1;
        private double[,] K2;
        private double[,] K3;
        private double[,] K4;

        private double[] b1;
        private double[] b2;
        private double[] b3;
        private double[] b4;

        public Simulation(int nTime, int nSpace, double Dt, double dx)
        {
            time = Enumerable.Repeat<double>(0.0, nTime + 1).ToArray<double>();
            space = Enumerable.Repeat<double>(0.0, nTime + 1).ToArray<double>();
            Wk = Enumerable.Repeat<double>(0.0, nTime + 1).ToArray<double>();
            density = new double[nTime + 1, nSpace + 1];
            u = new double[nTime + 1, nSpace + 1];
            temperature = new double[nTime + 1, nSpace + 1];
            compCTR = new double[nTime + 1, nSpace + 1, 15];
            wDot = new double[nTime + 1, nSpace + 1, 15];
            Yk = new double[nTime + 1, nSpace + 1, 15];

            Vk = 0;
            this.nTime = nTime;
            this.nSpace = nSpace;
            A = 1;
            this.Dt = Dt;
            this.dx = dx;
            sequence = 1;
        }

        public void InitSetting(double density, double temperature, double H2Rate, double steamCTR)
        {
            tempInf = temperature;
            double r = 0.000082, p = 1;

            // MolecularWeight.csv 파일에서
            // H, H2, O, O2, OH, H2O, HO2, N2, H2O2, M 순으로 분자량을 읽는다.
            var molecularWeightFileStream = new MemoryStream(Properties.Resources.MolecularWeight);
            using (StreamReader streamReader = new StreamReader(molecularWeightFileStream, Encoding.UTF8, false))
            {
                streamReader.ReadLine(); // 첫 줄은 표제이므로
                var idx = 0;
                while (!streamReader.EndOfStream)
                {
                    var lineValues = streamReader.ReadLine().Split(',');
                    var weight = Convert.ToDouble(lineValues[1]);
                    Wk[idx] = weight;
                    idx += 1;
                }
            }

            for (int i = 0; i < nTime + 1; i++)
            {
                time[i] = 0;
                /*this.compCTR.Add(new List<List<double>>());
                this.Yk.Add(new List<List<double>>());
                this.wDot.Add(new List<List<double>>());*/

                for (int j = 0; j < nSpace + 1; j++)
                {
                    this.density[i, j] = density;
                    this.u[i, j] = 0.0;
                    this.temperature[i, j] = temperature;
                    //this.wDot[i].Add(new List<double>());
                    Vk = 0;

                    if (i == 0)
                    {
                        space[j] = 0;
                    }

                    /*this.compCTR[i].Add(new List<double>());
                    this.Yk[i].Add(new List<double>());*/
                    for (int k = 0; k < 10; k++)
                    {
                        if (k == 1)
                        {
                            var tmp = H2Rate * p / r / this.temperature[i, j] / 100;
                            //this.compCTR[i][j].Add(tmp);
                            this.compCTR[i, j, k] = tmp;
                        }
                        else if (k == 3)
                        {
                            var tmp = (100 - H2Rate - steamCTR) * 0.23 * p / r / this.temperature[i, j] / 100;
                            //this.compCTR[i][j].Add(tmp);
                            this.compCTR[i, j, k] = tmp;
                        }
                        else if (k == 5)
                        {
                            var tmp = steamCTR * p / r / this.temperature[i, j] / 100;
                            //this.compCTR[i][j].Add(tmp);
                            this.compCTR[i, j, k] = tmp;
                        }
                        else if (k == 7)
                        {
                            var tmp = (100 - H2Rate - steamCTR) * 0.77 * p / r / this.temperature[i, j] / 100;
                            //this.compCTR[i][j].Add(tmp);
                            this.compCTR[i, j, k] = tmp;
                        }
                        else
                        {
                            //this.compCTR[i][j].Add(0);
                            this.compCTR[i, j, k] = 0.0;
                        }
                        /*this.Yk[i][j].Add(0);
                        this.wDot[i][j].Add(0);*/
                        this.Yk[i, j, k] = 0.0;
                        this.wDot[i, j, k] = 0.0;
                    }
                }
            }

            K1 = new double[nSpace - 1, nSpace - 1];
            K2 = new double[nSpace, nSpace];
            K3 = new double[nSpace, nSpace];
            K4 = new double[nSpace, nSpace];

            for (int i = 0; i < nSpace - 1; i++)
            {
                for (int j = 0; j < nSpace - 1; j++)
                {
                    K1[i, j] = 0.0;
                }
            }
            for (int i = 0; i < nSpace; i++)
            {
                for (int j = 0; j < nSpace; j++)
                {
                    K2[i, j] = 0.0;
                    K3[i, j] = 0.0;
                    K4[i, j] = 0.0;
                }
            }
        }

        public void Run(int targetStep, double Dt)
        {
            this.Dt = Dt;
            var tmpSequence = sequence;
            for (int i = tmpSequence; i < tmpSequence + targetStep; i++, sequence++)
            {
                Thread.Sleep(1);
                Application.DoEvents();
                CalculateGoverningEquation(i);
                Thread.Sleep(1);
                Application.DoEvents();
                CalculateSpecies(i);
            }
            sequence = tmpSequence + sequence;
        }

        private void CalculateGoverningEquation(int timeStep)
        {
            b1 = Enumerable.Repeat<double>(0.0, nSpace - 1).ToArray<double>();
            b2 = Enumerable.Repeat<double>(0.0, nSpace).ToArray<double>();
            b3 = Enumerable.Repeat<double>(0.0, nSpace).ToArray<double>();
            b4 = Enumerable.Repeat<double>(0.0, nSpace).ToArray<double>();

            // Energy equation_BTCS
            for (int i = 0; i < nSpace - 1; i++)
            {
                for (int j = 0; j < nSpace - 1; j++)
                {
                    K1[i, j] = 0.0;
                }
            }

            for (int i = 0; i < nSpace; i++)
            {
                for (int j = 0; j < nSpace; j++)
                {
                    K2[i, j] = 0.0;
                    K3[i, j] = 0.0;
                    K4[i, j] = 0.0;
                }
            }

            K1[0, 0] = density[timeStep - 1, 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, 1) * A / CalculateHeatCapacity(timeStep - 1, 1) / dx / dx;
            K1[0, 1] = density[timeStep - 1, 1] * A * u[timeStep - 1, 1] * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, 1) * A / CalculateHeatCapacity(timeStep - 1, 1) / dx / dx;
            b1[0] = density[timeStep - 1, 1] * A * temperature[timeStep - 1, 1] / Dt - CalculateChemicalReaction(timeStep - 1, 1) - (-density[timeStep - 1, 1] * u[timeStep - 1, 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, 1) * A / CalculateHeatCapacity(timeStep - 1, 1) / dx / dx) * temperature[timeStep - 1, 0];

            K1[nSpace - 2, nSpace - 3] = -density[timeStep - 1, nSpace - 1] * u[timeStep - 1, nSpace - 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, nSpace - 1) * A / CalculateHeatCapacity(timeStep - 1, nSpace - 1) / dx / dx;
            K1[nSpace - 2, nSpace - 2] = density[timeStep - 1, nSpace - 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, nSpace - 1) * A / CalculateHeatCapacity(timeStep - 1, nSpace - 1) / dx / dx;
            b1[nSpace - 2] = density[timeStep - 1, nSpace - 1] * A * temperature[timeStep - 1, nSpace - 1] / Dt - (-density[timeStep - 1, nSpace - 1] * u[timeStep - 1, nSpace - 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, nSpace - 1) * A / CalculateHeatCapacity(timeStep - 1, nSpace - 1) / dx / dx) * temperature[timeStep - 1, nSpace];

            for (int i = 1; i < nSpace / 10; i++)
            {
                K1[i, i - 1] = -density[timeStep - 1, i + 1] * u[timeStep - 1, i + 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / dx / dx;
                K1[i, i] = density[timeStep - 1, i + 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / dx / dx;
                K1[i, i + 1] = density[timeStep - 1, i + 1] * A * u[timeStep - 1, i + 1] * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / dx / dx;
                b1[i] = density[timeStep - 1, i + 1] * A * temperature[timeStep - 1, i + 1] / Dt;
            }

            for (int i = nSpace / 10; i < nSpace - 2; i++)
            {
                K1[i, i - 1] = -density[timeStep - 1, i + 1] * u[timeStep - 1, i + 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / dx / dx;
                K1[i, i] = density[timeStep - 1, i + 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / dx / dx;
                K1[i, i + 1] = density[timeStep - 1, i + 1] * A * u[timeStep - 1, i + 1] * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / dx / dx;
                b1[i] = density[timeStep - 1, i + 1] * A * temperature[timeStep - 1, i + 1] / Dt;
            }

            //온도 업데이트
            var X1 = LUdecomposition(K1, b1, nSpace - 1);
            for (int i = 1; i < nSpace - 1; i++)
            {
                temperature[timeStep, i] = X1[i];
            }

            //Momentum equation 
            //에측자 uprime 구하기_upwind
            K2[0, 0] = 1 / Dt + u[timeStep - 1, 1] / dx;
            b2[0] = 9.81 * (temperature[timeStep, 1] - tempInf) / temperature[timeStep, 1] + u[timeStep - 1, 1] / Dt + (u[timeStep - 1, 1] / dx) * u[timeStep - 1, 0];

            for (int i = 1; i < nSpace; i++)
            {
                K2[i, i - 1] = -u[timeStep - 1, i + 1] / dx;
                K2[i, i] = 1 / Dt + u[timeStep - 1, i + 1] / dx;
                b2[i] = 9.81 * (temperature[timeStep, i + 1] - tempInf) / temperature[timeStep, i + 1] + u[timeStep - 1, i + 1] / Dt;
            }

            var X2 = LUdecomposition(K2, b2, nSpace);

            //수정자 u 구하기_upwind
            K3[0, 0] = 1 / Dt + u[timeStep - 1, 1] / dx;
            b3[0] = 9.81 * (temperature[timeStep, 1] - tempInf) / temperature[timeStep, 1] + (u[timeStep - 1, 1] + X2[0]) * 0.5 / Dt + (u[timeStep - 1, 1] / dx) * u[timeStep - 1, 0];

            for (int i = 1; i < nSpace; i++)
            {
                K3[i, i - 1] = -u[timeStep - 1, i + 1] / dx;
                K3[i, i] = 1 / Dt + u[timeStep - 1, i + 1] / dx;
                b3[i] = 9.81 * (temperature[timeStep, i + 1] - tempInf) / temperature[timeStep, 1 + i] + (u[timeStep - 1, i + 1] + X2[i]) * 0.5 / Dt;
            }

            //속도 업데이트
            var X3 = LUdecomposition(K3, b3, nSpace);
            for (int i = 1; i < nSpace; i++)
            {
                u[timeStep, i] = X3[i];
            }
            u[timeStep, 0] = u[timeStep, 1]; //JY 추가
            u[timeStep, nSpace] = u[timeStep, nSpace - 1]; //JY 추가

            //Continuity_upwind
            K4[0, 0] = 1 / Dt - u[timeStep, 1] / dx;
            b4[0] = density[timeStep - 1, 1] / Dt - (u[timeStep, 0] / Dt) * density[timeStep - 1, 0];

            for (int i = 1; i < nSpace; i++)
            {
                K4[i, i - 1] = -u[timeStep, i] / Dt;
                K4[i, i] = 1 / Dt - u[timeStep, i + 1] / dx;
                b4[i] = density[timeStep - 1, i + 1] / Dt;
            }

            //밀도 업데이트
            var X4 = LUdecomposition(K4, b4, nSpace);
            for (int i = 1; i < nSpace; i++)
            {
                density[timeStep, i] = X4[i];
            }
        }

        private double CalculateChemicalReaction(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            // Wk의 크기만큼 반복문을 도는 이유는 H, H2, O, O2, OH, H2O, HO2, N2, H2O2, M가 존재하기 때문이다.
            for (int i = 0; i < 9; i++)
            {
                tmp = tmp + (density[timeStep, spaceStep] * A * Yk[timeStep, spaceStep, i] * Vk * CalculateEachHeatCapacity(timeStep, spaceStep, i) * (temperature[timeStep, spaceStep] - temperature[timeStep, spaceStep - 1]) / dx) + (wDot[timeStep, spaceStep, i] * CalculateEachHeatCapacity(timeStep, spaceStep, i) * Wk[i]);
            }
            var tmpCp = CalculateHeatCapacity(timeStep, spaceStep);
            tmp = tmp * A / tmpCp;
            return tmp;
        }

        private void CalculateSpecies(int timeStep)
        {
            var reactionConstant = new List<Double>();

            for (int p = 1; p < (nSpace / 10); p++)
            {
                double r = 0.000082;

                double cp_h2 = 29; // J/mol
                double cp_o2 = 35;
                double cp_h2o = 42.44;
                double cp_h2o_l = 75;
                double cp_n2 = 33;
                double cp_oh = 0;
                double cp_ho2 = 0;
                //cp_oh = 32; %unknown
                //cp_ho2 = 32; %unknown
                //cp_h2o2 = 32; %unknown

                // standard enthalpy
                double h0_h2 = 0; //J/mol
                double h0_o2 = 0;
                double h0_n2 = 0;
                double h0_h2o = -244500;
                double h0_oh = 0;
                double h0_ho2 = 0;
                double h0_h2o2 = 0;

                double h_h2 = (h0_h2 + (temperature[timeStep, p] - 298) * cp_h2) * compCTR[timeStep - 1, p, 1];
                double h_o2 = (h0_o2 + (temperature[timeStep, p] - 298) * cp_o2) * compCTR[timeStep - 1, p, 3];
                double h_h2o = (h0_h2o + (temperature[timeStep, p] - 298) * cp_h2o) * compCTR[timeStep - 1, p, 5];
                double h_n2 = (h0_n2 + (temperature[timeStep, p] - 298) * cp_n2) * compCTR[timeStep - 1, p, 7];
                double h_total = (h_h2 + h_o2 + h_h2o + h_n2);

                if (p < (nSpace / 10) + 1)
                {
                    // time variables
                    int iMax = 10000;
                    double dt = Dt / iMax;
                    for (int i = 1; i < iMax + 1; i++)
                    {
                        // ReactionConstant.csv 파일로부터 순서대로 상수값들을 읽는다.
                        var reactionConstantFileStream = new MemoryStream(Properties.Resources.ReactionConstant);
                        using (StreamReader streamReader = new StreamReader(reactionConstantFileStream, Encoding.UTF8, false))
                        {
                            streamReader.ReadLine(); // 첫 줄은 표제이므로
                            while (!streamReader.EndOfStream)
                            {
                                var lineValues = streamReader.ReadLine().Split(',');
                                var aTmpVal = Convert.ToDouble(lineValues[0]);
                                var nTmpVal = Convert.ToDouble(lineValues[1]);
                                var eTmpVal = Convert.ToDouble(lineValues[2]);
                                var calcTmpVal = aTmpVal * Math.Pow(temperature[timeStep, p], nTmpVal) * Math.Exp(-eTmpVal / r / temperature[timeStep, p]) * 10E-06;
                                reactionConstant.Add(calcTmpVal);
                            }
                        }
                        var forwardBackwardReaction = Enumerable.Repeat<double>(0.0, 40).ToArray<double>();
                        if (i == 1)
                        {
                            // forward reaction 27개
                            forwardBackwardReaction[1] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 3] * reactionConstant[0];
                            forwardBackwardReaction[2] = compCTR[timeStep - 1, p - 1, 1] * compCTR[timeStep - 1, p - 1, 2] * reactionConstant[1];
                            forwardBackwardReaction[3] = compCTR[timeStep - 1, p - 1, 1] * compCTR[timeStep - 1, p - 1, 4] * reactionConstant[2];
                            forwardBackwardReaction[4] = compCTR[timeStep - 1, p - 1, 4] * compCTR[timeStep - 1, p - 1, 4] * reactionConstant[3];
                            forwardBackwardReaction[5] = 2 * (compCTR[timeStep - 1, p - 1, 1] * compCTR[timeStep - 1, p - 1, 3] * reactionConstant[4]);
                            forwardBackwardReaction[6] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 9] * reactionConstant[5];
                            forwardBackwardReaction[7] = 0.5 * (compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 1]) * reactionConstant[6];
                            forwardBackwardReaction[8] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 5] * reactionConstant[7];
                            forwardBackwardReaction[9] = compCTR[timeStep - 1, p - 1, 2] * compCTR[timeStep - 1, p - 1, 2] * compCTR[timeStep - 1, p - 1, 9] * reactionConstant[8];
                            forwardBackwardReaction[10] = compCTR[timeStep - 1, p - 1, 2] * compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 9] * reactionConstant[9];
                            forwardBackwardReaction[11] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 4] * compCTR[timeStep - 1, p - 1, 9] * reactionConstant[10];
                            forwardBackwardReaction[12] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 3] * reactionConstant[11];
                            forwardBackwardReaction[13] = compCTR[timeStep - 1, p - 1, 0] * 2 * compCTR[timeStep - 1, p - 1, 3] * reactionConstant[12];
                            forwardBackwardReaction[14] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 3] * compCTR[timeStep - 1, p - 1, 5] * reactionConstant[13];
                            forwardBackwardReaction[15] = compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 3] * compCTR[timeStep - 1, p - 1, 7] * reactionConstant[14];
                            forwardBackwardReaction[16] = compCTR[timeStep - 1, p - 1, 6] * compCTR[timeStep - 1, p - 1, 0] * reactionConstant[15];
                            forwardBackwardReaction[17] = 2 * (compCTR[timeStep - 1, p - 1, 6] * compCTR[timeStep - 1, p - 1, 0] * reactionConstant[16]);
                            forwardBackwardReaction[18] = compCTR[timeStep - 1, p - 1, 6] * compCTR[timeStep - 1, p - 1, 0] * reactionConstant[17];
                            forwardBackwardReaction[19] = compCTR[timeStep - 1, p - 1, 6] * compCTR[timeStep - 1, p - 1, 2] * reactionConstant[18];
                            forwardBackwardReaction[20] = compCTR[timeStep - 1, p - 1, 6] * compCTR[timeStep - 1, p - 1, 4] * reactionConstant[19];
                            forwardBackwardReaction[21] = 2 * compCTR[timeStep - 1, p - 1, 6] * reactionConstant[20];
                            forwardBackwardReaction[22] = 2 * compCTR[timeStep - 1, p - 1, 7] * compCTR[timeStep - 1, p - 1, 9] * reactionConstant[21];
                            forwardBackwardReaction[23] = compCTR[timeStep - 1, p - 1, 4] * compCTR[timeStep - 1, p - 1, 4] * compCTR[timeStep - 1, p - 1, 9] * reactionConstant[22];
                            forwardBackwardReaction[24] = compCTR[timeStep - 1, p - 1, 8] * compCTR[timeStep - 1, p - 1, 0] * reactionConstant[23];
                            forwardBackwardReaction[25] = compCTR[timeStep - 1, p - 1, 8] * compCTR[timeStep - 1, p - 1, 0] * reactionConstant[24];
                            forwardBackwardReaction[26] = compCTR[timeStep - 1, p - 1, 8] * compCTR[timeStep - 1, p - 1, 2] * reactionConstant[25];
                            forwardBackwardReaction[27] = compCTR[timeStep - 1, p - 1, 8] * compCTR[timeStep - 1, p - 1, 4] * reactionConstant[26];
                            // backward reaction 3개
                            forwardBackwardReaction[28] = compCTR[timeStep - 1, p - 1, 2] * compCTR[timeStep - 1, p - 1, 2] * reactionConstant[27];
                            forwardBackwardReaction[29] = 0.5 * (compCTR[timeStep - 1, p - 1, 2] * compCTR[timeStep - 1, p - 1, 5]) * reactionConstant[28];
                            forwardBackwardReaction[30] = 0.5 * (compCTR[timeStep - 1, p - 1, 1] * compCTR[timeStep - 1, p - 1, 9]) * reactionConstant[29];
                        }
                        else
                        {
                            // forward reaction 27개
                            forwardBackwardReaction[1] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 3] * reactionConstant[0];
                            forwardBackwardReaction[2] = compCTR[timeStep, p - 1, 1] * compCTR[timeStep, p - 1, 2] * reactionConstant[1];
                            forwardBackwardReaction[3] = compCTR[timeStep, p - 1, 1] * compCTR[timeStep, p - 1, 4] * reactionConstant[2];
                            forwardBackwardReaction[4] = compCTR[timeStep, p - 1, 4] * compCTR[timeStep, p - 1, 4] * reactionConstant[3];
                            forwardBackwardReaction[5] = 2 * (compCTR[timeStep, p - 1, 1] * compCTR[timeStep, p - 1, 3] * reactionConstant[4]);
                            forwardBackwardReaction[6] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 9] * reactionConstant[5];
                            forwardBackwardReaction[7] = 0.5 * (compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep - 1, p - 1, 0] * compCTR[timeStep, p - 1, 1]) * reactionConstant[6];
                            forwardBackwardReaction[8] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 5] * reactionConstant[7];
                            forwardBackwardReaction[9] = compCTR[timeStep, p - 1, 2] * compCTR[timeStep, p - 1, 2] * compCTR[timeStep, p - 1, 9] * reactionConstant[8];
                            forwardBackwardReaction[10] = compCTR[timeStep, p - 1, 2] * compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 9] * reactionConstant[9];
                            forwardBackwardReaction[11] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 4] * compCTR[timeStep, p - 1, 9] * reactionConstant[10];
                            forwardBackwardReaction[12] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 3] * reactionConstant[11];
                            forwardBackwardReaction[13] = compCTR[timeStep, p - 1, 0] * 2 * compCTR[timeStep, p - 1, 3] * reactionConstant[12];
                            forwardBackwardReaction[14] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 3] * compCTR[timeStep, p - 1, 5] * reactionConstant[13];
                            forwardBackwardReaction[15] = compCTR[timeStep, p - 1, 0] * compCTR[timeStep, p - 1, 3] * compCTR[timeStep, p - 1, 7] * reactionConstant[14];
                            forwardBackwardReaction[16] = compCTR[timeStep, p - 1, 6] * compCTR[timeStep, p - 1, 3] * reactionConstant[15];
                            forwardBackwardReaction[17] = 2 * (compCTR[timeStep, p - 1, 6] * compCTR[timeStep, p - 1, 3] * reactionConstant[16]);
                            forwardBackwardReaction[18] = compCTR[timeStep, p - 1, 6] * compCTR[timeStep, p - 1, 3] * reactionConstant[17];
                            forwardBackwardReaction[19] = compCTR[timeStep, p - 1, 6] * compCTR[timeStep, p - 1, 2] * reactionConstant[18];
                            forwardBackwardReaction[20] = compCTR[timeStep, p - 1, 6] * compCTR[timeStep, p - 1, 4] * reactionConstant[19];
                            forwardBackwardReaction[21] = 2 * compCTR[timeStep, p - 1, 6] * reactionConstant[20];
                            forwardBackwardReaction[22] = 2 * compCTR[timeStep, p - 1, 7] * compCTR[timeStep, p - 1, 9] * reactionConstant[21];
                            forwardBackwardReaction[23] = compCTR[timeStep, p - 1, 4] * compCTR[timeStep, p - 1, 4] * compCTR[timeStep, p - 1, 9] * reactionConstant[22];
                            forwardBackwardReaction[24] = compCTR[timeStep, p - 1, 8] * compCTR[timeStep, p - 1, 0] * reactionConstant[23];
                            forwardBackwardReaction[25] = compCTR[timeStep, p - 1, 8] * compCTR[timeStep, p - 1, 0] * reactionConstant[24];
                            forwardBackwardReaction[26] = compCTR[timeStep, p - 1, 8] * compCTR[timeStep, p - 1, 2] * reactionConstant[25];
                            forwardBackwardReaction[27] = compCTR[timeStep, p - 1, 8] * compCTR[timeStep, p - 1, 4] * reactionConstant[26];
                            // backward reaction 3개
                            forwardBackwardReaction[28] = compCTR[timeStep, p - 1, 2] * compCTR[timeStep - 1, p - 1, 2] * reactionConstant[27];
                            forwardBackwardReaction[29] = 0.5 * (compCTR[timeStep, p - 1, 2] * compCTR[timeStep, p - 1, 5]) * reactionConstant[28];
                            forwardBackwardReaction[30] = 0.5 * (compCTR[timeStep, p - 1, 1] * compCTR[timeStep, p - 1, 9]) * reactionConstant[29];
                        }

                        // Production Rate by species
                        double p_c_h = forwardBackwardReaction[2] + forwardBackwardReaction[3] + forwardBackwardReaction[28] + forwardBackwardReaction[30];
                        double p_c_h2 = forwardBackwardReaction[6] + forwardBackwardReaction[7] + forwardBackwardReaction[8] + forwardBackwardReaction[16] + forwardBackwardReaction[25];
                        double p_c_o = forwardBackwardReaction[1] + forwardBackwardReaction[4] + forwardBackwardReaction[18];
                        double p_c_o2 = forwardBackwardReaction[9] + forwardBackwardReaction[13] + forwardBackwardReaction[16] + forwardBackwardReaction[19] + forwardBackwardReaction[20] + forwardBackwardReaction[21] + forwardBackwardReaction[28];
                        double p_c_oh = forwardBackwardReaction[1] + forwardBackwardReaction[2] + forwardBackwardReaction[5] + forwardBackwardReaction[10] + forwardBackwardReaction[17] + forwardBackwardReaction[19] + forwardBackwardReaction[22] + forwardBackwardReaction[24] + forwardBackwardReaction[26];
                        double p_c_h2o = forwardBackwardReaction[3] + forwardBackwardReaction[4] + forwardBackwardReaction[8] + forwardBackwardReaction[11] + forwardBackwardReaction[14] + forwardBackwardReaction[18] + forwardBackwardReaction[20] + forwardBackwardReaction[21] + forwardBackwardReaction[24] + forwardBackwardReaction[29];
                        double p_c_ho2 = forwardBackwardReaction[12] + forwardBackwardReaction[13] + forwardBackwardReaction[14] + forwardBackwardReaction[15] + forwardBackwardReaction[25] + forwardBackwardReaction[26] + forwardBackwardReaction[27];
                        double p_c_h2o2 = forwardBackwardReaction[23];
                        double p_c_n2 = forwardBackwardReaction[15];

                        // Reductaion Rate by species
                        double r_c_h = forwardBackwardReaction[1] + forwardBackwardReaction[6] + forwardBackwardReaction[7] + forwardBackwardReaction[8] + forwardBackwardReaction[10] + forwardBackwardReaction[11] + forwardBackwardReaction[12] + forwardBackwardReaction[13] + forwardBackwardReaction[14] + forwardBackwardReaction[15] + forwardBackwardReaction[16] + forwardBackwardReaction[17] + forwardBackwardReaction[18] + forwardBackwardReaction[24] + forwardBackwardReaction[25];
                        double r_c_h2 = forwardBackwardReaction[2] + forwardBackwardReaction[3] + forwardBackwardReaction[5] + forwardBackwardReaction[30];
                        double r_c_o = forwardBackwardReaction[2] + forwardBackwardReaction[9] + forwardBackwardReaction[10] + forwardBackwardReaction[19] + forwardBackwardReaction[26] + forwardBackwardReaction[28];
                        double r_c_o2 = forwardBackwardReaction[5] + forwardBackwardReaction[12] + forwardBackwardReaction[13] + forwardBackwardReaction[14] + forwardBackwardReaction[15];
                        double r_c_oh = forwardBackwardReaction[4] + forwardBackwardReaction[11] + forwardBackwardReaction[20] + forwardBackwardReaction[23] + forwardBackwardReaction[27] + forwardBackwardReaction[28];
                        double r_c_h2o = forwardBackwardReaction[8] + forwardBackwardReaction[14] + forwardBackwardReaction[29];
                        double r_c_ho2 = forwardBackwardReaction[16] + forwardBackwardReaction[17] + forwardBackwardReaction[18] + forwardBackwardReaction[19] + forwardBackwardReaction[20] + forwardBackwardReaction[21];
                        double r_c_h2o2 = forwardBackwardReaction[22] + forwardBackwardReaction[24] + forwardBackwardReaction[25] + forwardBackwardReaction[26] + forwardBackwardReaction[27];
                        double r_c_n2 = forwardBackwardReaction[15];

                        //Transient states
                        if (i == 1)
                        {
                            compCTR[timeStep, p, 2] = (p_c_o - r_c_o) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 2];
                            compCTR[timeStep, p, 1] = (p_c_h2 - r_c_h2) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 1];
                            compCTR[timeStep, p, 3] = (p_c_o2 - r_c_o2) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 3];
                            compCTR[timeStep, p, 0] = (p_c_h - r_c_h) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 0];
                            compCTR[timeStep, p, 4] = (p_c_oh - r_c_oh) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 4];
                            compCTR[timeStep, p, 6] = (p_c_ho2 - r_c_ho2) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 6];
                            compCTR[timeStep, p, 5] = (p_c_h2o - r_c_h2o) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep - 1, p - 1, 5];

                        }
                        else
                        {
                            compCTR[timeStep, p, 2] = (p_c_o - r_c_o) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 2];
                            compCTR[timeStep, p, 1] = (p_c_h2 - r_c_h2) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 1];
                            compCTR[timeStep, p, 3] = (p_c_o2 - r_c_o2) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 3];
                            compCTR[timeStep, p, 0] = (p_c_h - r_c_h) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 0];
                            compCTR[timeStep, p, 4] = (p_c_oh - r_c_oh) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 4];
                            compCTR[timeStep, p, 6] = (p_c_ho2 - r_c_ho2) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 6];
                            compCTR[timeStep, p, 5] = (p_c_h2o - r_c_h2o) / ((1 / dt) + (u[timeStep, p] / dx)) + compCTR[timeStep, p, 5];
                        }

                        double cp_n = (cp_h2 * compCTR[timeStep, p, 1] + cp_h2o * compCTR[timeStep, p, 5] + cp_n2 * compCTR[timeStep, p, 7] + cp_o2 * compCTR[timeStep, p, 3]) / (compCTR[timeStep, p, 1] + compCTR[timeStep, p, 5] + compCTR[timeStep, p, 7] + compCTR[timeStep, p, 3]);
                        temperature[timeStep, p] = temperature[timeStep, p] + 2 * (-h0_h2o) * forwardBackwardReaction[4] / (cp_n * (compCTR[timeStep, p, 1] + compCTR[timeStep, p, 5] + compCTR[timeStep, p, 7] + compCTR[timeStep, p, 3])) * dt;
                        // the hydrogen-air burning rate...ref
                        // temperature[timeStep+1][p];
                        // JY 추가

                        // H,H2,O,O2,OH,H2O,HO2,N2
                        double c_total = compCTR[timeStep, p, 1] + compCTR[timeStep, p, 0] + compCTR[timeStep, p, 5] + compCTR[timeStep, p, 3] + compCTR[timeStep, p, 4] + compCTR[timeStep, p, 6] + compCTR[timeStep, p, 2] + compCTR[timeStep, p, 7];

                        Yk[timeStep, p, 0] = compCTR[timeStep, p, 0] / c_total;
                        Yk[timeStep, p, 1] = compCTR[timeStep, p, 1] / c_total;
                        Yk[timeStep, p, 2] = compCTR[timeStep, p, 2] / c_total;
                        Yk[timeStep, p, 3] = compCTR[timeStep, p, 3] / c_total;
                        Yk[timeStep, p, 4] = compCTR[timeStep, p, 4] / c_total;
                        Yk[timeStep, p, 5] = compCTR[timeStep, p, 5] / c_total;
                        Yk[timeStep, p, 6] = compCTR[timeStep, p, 6] / c_total;

                        wDot[timeStep, p, 0] = p_c_h - r_c_h;
                        wDot[timeStep, p, 1] = p_c_h2 - r_c_h2;
                        wDot[timeStep, p, 2] = p_c_o - r_c_o;
                        wDot[timeStep, p, 3] = p_c_o2 - r_c_o2;
                        wDot[timeStep, p, 4] = p_c_oh - r_c_oh;
                        wDot[timeStep, p, 5] = p_c_h2o - r_c_h2o;
                        wDot[timeStep, p, 6] = p_c_ho2 - r_c_ho2;
                    }
                }
                else
                {
                    compCTR[timeStep, p, 2] = compCTR[timeStep - 1, p - 1, 2];
                    compCTR[timeStep, p, 1] = compCTR[timeStep - 1, p - 1, 1];
                    compCTR[timeStep, p, 3] = compCTR[timeStep - 1, p - 1, 3];
                    compCTR[timeStep, p, 0] = compCTR[timeStep - 1, p - 1, 0];
                    compCTR[timeStep, p, 4] = compCTR[timeStep - 1, p - 1, 4];
                    compCTR[timeStep, p, 6] = compCTR[timeStep - 1, p - 1, 6];
                    compCTR[timeStep, p, 5] = compCTR[timeStep - 1, p - 1, 5];

                    double c_total = compCTR[timeStep, p, 1] + compCTR[timeStep, p, 0] + compCTR[timeStep, p, 5] + compCTR[timeStep, p, 3] + compCTR[timeStep, p, 4] + compCTR[timeStep, p, 6] + compCTR[timeStep, p, 2] + compCTR[timeStep, p, 7];

                    Yk[timeStep, p, 0] = compCTR[timeStep, p, 0] / c_total;
                    Yk[timeStep, p, 1] = compCTR[timeStep, p, 1] / c_total;
                    Yk[timeStep, p, 2] = compCTR[timeStep, p, 2] / c_total;
                    Yk[timeStep, p, 3] = compCTR[timeStep, p, 3] / c_total;
                    Yk[timeStep, p, 4] = compCTR[timeStep, p, 4] / c_total;
                    Yk[timeStep, p, 5] = compCTR[timeStep, p, 5] / c_total;
                    Yk[timeStep, p, 6] = compCTR[timeStep, p, 6] / c_total;

                    wDot[timeStep, p, 0] = 0;
                    wDot[timeStep, p, 1] = 0;
                    wDot[timeStep, p, 2] = 0;
                    wDot[timeStep, p, 3] = 0;
                    wDot[timeStep, p, 4] = 0;
                    wDot[timeStep, p, 5] = 0;
                    wDot[timeStep, p, 6] = 0;
                }
            }
            time[timeStep] = time[timeStep - 1] + Dt;
        }

        private double CalculateHeatCapacity(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            // Wk의 크기만큼 반복문을 도는 이유는 H, H2, O, O2, OH, H2O, HO2, N2, H2O2, M가 존재하기 때문이다.
            for (int i = 0; i < 9; i++)
            {
                tmp += compCTR[timeStep, spaceStep, i] * CalculateEachHeatCapacity(timeStep, spaceStep, i);
            }
            return tmp;
        }

        private double CalculateEachHeatCapacity(int timeStep, int spaceStep, int speciesNo)
        {
            if (speciesNo == 1)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 29.189, 29.256, 29.33, 29.46, 29.653, 29.908, 30.222, 30.58, 30.99, 31.42, 31.86, 32.3 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 5)
            {
                double[] xSrc = { 400, 449.33, 500.62, 599.79, 700.42, 799.58, 900.21, 1000.8, 1100, 1200.6, 1300, 1400, 1500 };
                double[] ySrc = { 36.198, 35.597, 35.703, 36.513, 37.598, 38.774, 40.027, 41.304, 42.554, 43.789, 44.94, 46.06, 47.11 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 3)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 30.132, 31.108, 32.103, 32.992, 33.742, 34.363, 34.873, 35.29, 35.66, 35.99, 36.28, 36.55 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 4)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 29.65, 29.51, 29.52, 29.67, 29.92, 30.27, 30.67, 31.12, 31.59, 32.05, 32.5, 32.95 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 8)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 48.65, 52.51, 55.5, 57.92, 59.9, 61.55, 62.95, 64.17, 65.27, 66.3, 67.33, 68.42 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 6)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 37.43, 39.68, 41.68, 43.45, 45.03, 46.42, 47.66, 48.75, 49.73, 50.6, 51.39, 52.11 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 7)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 29.273, 29.594, 30.118, 30.761, 31.439, 32.096, 32.703, 33.248, 33.729, 34.152, 34.522, 34.846 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else
            {
                return 0;
            }
        }

        private double CalculateThermalConductivity(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            // Wk의 크기만큼 반복문을 도는 이유는 H, H2, O, O2, OH, H2O, HO2, N2, H2O2, M가 존재하기 때문이다.
            for (int i = 0; i < 9; i++)
            {
                tmp += compCTR[timeStep, spaceStep, i] * CalculateEachThermalConductivity(timeStep, spaceStep, i);
            }
            return tmp;
        }

        private double CalculateEachThermalConductivity(int timeStep, int spaceStep, int speciesNo)
        {
            if (speciesNo == 1)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 0.23406, 0.2805, 0.32809, 0.37631, 0.42517, 0.47467, 0.5248, 0.57556, 0.62695, 0.67897, 0.73162, 0.7849 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 5)
            {
                double[] xSrc = { 400, 449.33, 500.62, 599.79, 700.42, 799.58, 900.21, 1000.8, 1100, 1200.6, 1300, 1400, 1500 };
                double[] ySrc = { 0.02702, 0.031115, 0.035926, 0.046345, 0.058015, 0.070332, 0.083494, 0.097202, 0.11115, 0.12567, 0.14019, 0.15471, 0.16923 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 3)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 0.034689, 0.04275, 0.050738, 0.058499, 0.065933, 0.072997, 0.079685, 0.086003, 0.091951, 0.097529, 0.102737, 0.107575 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 4)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 0.02702, 0.035926, 0.046345, 0.058015, 0.070332, 0.083494, 0.097202, 0.11115, 0.12567, 0.14019, 0.15471, 0.16923 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 8)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 0.034689, 0.04275, 0.050738, 0.058499, 0.065933, 0.072997, 0.079685, 0.086003, 0.091951, 0.097529, 0.102737, 0.107575 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 6)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 0.034689, 0.04275, 0.050738, 0.058499, 0.065933, 0.072997, 0.079685, 0.086003, 0.091951, 0.097529, 0.102737, 0.107575 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 7)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 0.032205, 0.038143, 0.043917, 0.049605, 0.055197, 0.060666, 0.065991, 0.07116, 0.076173, 0.081038, 0.085763, 0.090361 };
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else
            {
                return 0;
            }
        }

        private double CalculateEnthalpy(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            for (int i = 0; i < 9; i++)
            {
                tmp += compCTR[timeStep, spaceStep, i] * CalculateEachEnthalpy(timeStep, spaceStep, i);
            }
            return tmp;
        }

        private double CalculateEachEnthalpy(int timeStep, int spaceStep, int speciesNo)
        {
            if (speciesNo == 1)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 10.886, 13.809, 16.738, 19.677, 22.632, 25.61, 28.616, 31.656, 34.736, 37.856, 41.016, 44.226 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 5)
            {
                double[] xSrc = { 400, 449.33, 500.62, 599.79, 700.42, 799.58, 900.21, 1000.8, 1100, 1200.6, 1300, 1400, 1500 };
                double[] ySrc = { 49.187, 50.953, 52.78, 56.357, 60.085, 63.871, 67.835, 71.927, 76.085, 80.43, 84.86, 89.41, 94.07 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 3)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 11.701, 14.762, 17.923, 21.179, 24.517, 27.923, 31.386, 34.896, 38.446, 42.026, 45.646, 49.286 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 4)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 42.02706, 44.97706, 47.92706, 50.88706, 53.86706, 56.87706, 59.92706, 63.00706, 66.14706, 69.32706, 72.54706, 75.82706 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 8)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 18.5736, 23.6336, 29.0436, 34.7236, 40.6136, 46.6836, 52.9136, 59.2736, 65.7436, 72.3236, 79.0036, 85.7936 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 6)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 5.772001, 9.632001, 13.702001, 17.952001, 22.382001, 26.952001, 31.662001, 36.482001, 41.412001, 46.422001, 51.522001, 56.702001 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else if (speciesNo == 7)
            {
                double[] xSrc = { 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 };
                double[] ySrc = { 11.638, 14.58, 17.564, 20.607, 23.717, 26.894, 30.135, 33.433, 36.782, 40.177, 43.611, 47.08 };
                for (var i = 0; i < ySrc.Length; i++)
                {
                    ySrc[i] = ySrc[i] * 1000;
                }
                return MonotonicCubicHermiteSpline(temperature[timeStep, spaceStep], xSrc, ySrc);
            }
            else
            {
                return 0;
            }
        }

        private double Polynomial(double x, double p1, double p2, double p3, double p4, double p5, double p6, double p7, double p8, double p9)
        {
            var tmp = p1 * Math.Pow(x, 8) + p2 * Math.Pow(x, 7) + p3 * Math.Pow(x, 6) + p4 * Math.Pow(x, 5) + p5 * Math.Pow(x, 4) + p6 * Math.Pow(x, 3) + p7 * Math.Pow(x, 2) + p8 * Math.Pow(x, 1) + p9;
            return tmp;
        }

        private double[] LUdecomposition(double[,] K, double[] P, int size)
        {
            var a = 0.0;
            for (int i = 1; i < size; i++)
            {
                K[0, i] = K[0, i] / K[0, 0];
            }

            for (int i = 1; i < size - 1; i++)
            {
                for (int j = i; j < size; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        a += K[j, k] * K[k, i];
                    }
                    K[j, i] = K[j, i] - a;
                    a = 0.0;
                }
                for (int j = i + 1; j < size; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        a += K[i, k] * K[k, j];
                    }
                    K[i, j] = (K[i, j] - a) / K[i, i];
                    a = 0;
                }
            }

            for (int i = 0; i < size - 1; i++)
            {
                a += K[size - 1, i] * K[i, size - 1];
            }

            K[size - 1, size - 1] = K[size - 1, size - 1] - a;
            a = 0.0;

            var C = Enumerable.Repeat<double>(0.0, size).ToArray<double>();
            C[0] = P[0] / K[0, 0];
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    a += K[i, j] * C[j];
                }
                C[i] = (P[i] - a) / K[i, i];
                a = 0.0;
            }

            var X = Enumerable.Repeat<double>(0.0, size).ToArray<double>();
            X[size - 1] = C[size - 1];
            for (int i = size - 2; i >= 0; i--)
            {
                for (int j = size - 1; j >= i + 1; j--)
                {
                    a += K[i, j] * X[j];
                }
                X[i] = C[i] - a;
                a = 0.0;
            }

            return X;
        }

        public double[,,] GetH2 => compCTR;
        public double[,] GetU => u;
        public double[,] GetTemperature => temperature;

        private double MonotonicCubicHermiteSpline(double x, double[] xSrc, double[] ySrc)
        {
            var n = xSrc.Length;
            double[] m = Enumerable.Repeat<double>(0.0, n).ToArray<double>();

            m[0] = (ySrc[1] - ySrc[0]) / (xSrc[1] - xSrc[0]);
            m[n - 1] = (ySrc[n - 1] - ySrc[n - 2]) / (xSrc[n - 1] - xSrc[n - 2]);

            for (int k = 1; k < n - 1; k++)
            {
                m[k] = (ySrc[k] - ySrc[k - 1]) / (2 * (xSrc[k] - xSrc[k - 1])) + (ySrc[k + 1] - ySrc[k]) / (2 * (xSrc[k + 1] - xSrc[k]));
            }

            double deltaK1 = 0;
            for (int k = 0; k < n - 1; k++)
            {
                var deltaK = (ySrc[k + 1] - ySrc[k]) / (xSrc[k + 1] - xSrc[k]);

                if (Math.Abs(deltaK) <= 1e-15)
                {
                    m[k] = m[k + 1] = 0;
                }
                else if (deltaK * deltaK1 < 0)
                {
                    m[k] = 0;
                }
                else
                {
                    double ak = m[k] / deltaK;
                    double bk = m[k + 1] / deltaK;

                    if (ak * ak + bk * bk > 9)
                    {
                        m[k] = 3 / (Math.Sqrt(ak * ak + bk * bk)) * ak * deltaK;
                        m[k + 1] = 3 / (Math.Sqrt(ak * ak + bk * bk)) * bk * deltaK;
                    }
                }
                deltaK1 = deltaK;
            }

            int idx = 0;

            // x가 있는 xSrc구간을 구해서 해당하는 xSrc를 cur_x, next_x로 두어야 합니다. 이를통해 y를 구하고 return 합니다.
            for (int k = 0; k < n - 1; k++)
            {
                if (xSrc[k] < x)
                {
                    idx += 1;
                }
            }

            var curX = xSrc[idx - 1];
            var nextX = xSrc[idx];
            var curY = ySrc[idx - 1];
            var nextY = ySrc[idx];
            var h = nextX - curX;
            var t = (x - curX) / h;
            var y = curY * H00(t) + h * m[idx - 1] * H10(t) + nextY * H01(t) + h * m[idx] * H11(t);

            return y;
        }

        private double H00(double t)
        {
            return 2 * t * t * t - 3 * t * t + 1;
        }

        private double H10(double t)
        {
            return t * (1 - t) * (1 - t);
        }

        private double H01(double t)
        {
            return t * t * (3 - 2 * t);
        }

        private double H11(double t)
        {
            return t * t * (t - 1);
        }
    }
}
