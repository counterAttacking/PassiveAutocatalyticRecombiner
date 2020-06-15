using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAR
{
    public class Simulation
    {
        private int nTime, nSpace, sequence;
        private double Vk, A, Dt, dx, pressure = 1, tempInf;
        private List<double> time, space, Wk;
        private List<List<double>> density, u, temperature;
        private List<List<List<double>>> compCTR, wDot, Yk;

        public Simulation(int nTime, int nSpace, double Dt, double dx)
        {
            time = new List<double>();
            space = new List<double>();
            Wk = new List<double>();
            density = new List<List<double>>();
            u = new List<List<double>>();
            temperature = new List<List<double>>();
            compCTR = new List<List<List<double>>>();
            wDot = new List<List<List<double>>>();
            Yk = new List<List<List<double>>>();

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
                while (!streamReader.EndOfStream)
                {
                    var lineValues = streamReader.ReadLine().Split(',');
                    var weight = Convert.ToDouble(lineValues[1]);
                    Wk.Add(weight);
                }
            }

            for (int i = 0; i < nTime + 1; i++)
            {
                time.Add(0);
                this.density.Add(new List<double>());
                this.u.Add(new List<double>());
                this.temperature.Add(new List<double>());
                this.compCTR.Add(new List<List<double>>());
                this.Yk.Add(new List<List<double>>());
                this.wDot.Add(new List<List<double>>());

                for (int j = 0; j < nSpace + 1; j++)
                {
                    this.density[i].Add(density);
                    this.u[i].Add(0);
                    this.temperature[i].Add(temperature);
                    this.wDot[i].Add(new List<double>());
                    Vk = 0;

                    if (i == 0)
                    {
                        space.Add(0);
                    }

                    this.compCTR[i].Add(new List<double>());
                    this.Yk[i].Add(new List<double>());
                    for (int k = 0; k < 10; k++)
                    {
                        if (k == 1)
                        {
                            var tmp = H2Rate * p / r / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else if (k == 3)
                        {
                            var tmp = (100 - H2Rate - steamCTR) * 0.23 * p / r / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else if (k == 5)
                        {
                            var tmp = steamCTR * p / r / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else if (k == 7)
                        {
                            var tmp = (100 - H2Rate - steamCTR) * 0.77 * p / r / this.temperature[i][j] / 100;
                            this.compCTR[i][j].Add(tmp);
                        }
                        else
                        {
                            this.compCTR[i][j].Add(0);
                        }
                        this.Yk[i][j].Add(0);
                        this.wDot[i][j].Add(0);
                    }
                }
            }
        }

        public void Run(int targetStep, double Dt)
        {
            this.Dt = Dt;
            var tmpSequence = sequence;
            for (int i = tmpSequence; i < tmpSequence + targetStep; i++, sequence++)
            {
                CalculateGoverningEquation(i);
                CalculateSpecies(i);
            }
            sequence = tmpSequence + sequence;
        }

        private void CalculateGoverningEquation(int timeStep)
        {
            var K1 = new List<List<double>>();
            var K2 = new List<List<double>>();
            var K3 = new List<List<double>>();
            var K4 = new List<List<double>>();

            var b1 = Enumerable.Repeat<double>(0.0, nSpace - 1).ToArray<double>();
            var b2 = Enumerable.Repeat<double>(0.0, nSpace).ToArray<double>();
            var b3 = Enumerable.Repeat<double>(0.0, nSpace).ToArray<double>();
            var b4 = Enumerable.Repeat<double>(0.0, nSpace).ToArray<double>();

            // Energy equation_BTCS
            for (int i = 0; i < nSpace - 1; i++)
            {
                K1.Add(new List<double>());
                for (int j = 0; j < nSpace - 1; j++)
                {
                    K1[i].Add(0.0);
                }
            }

            K1[0][0] = density[timeStep - 1][1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, 1) * A / CalculateHeatCapacity(timeStep - 1, 1) / dx / dx;
            K1[0][1] = density[timeStep - 1][1] * A * u[timeStep - 1][1] * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, 1) * A / CalculateHeatCapacity(timeStep - 1, 1) / dx / dx;
            b1[0] = density[timeStep - 1][1] * A * temperature[timeStep - 1][1] / Dt - CalculateChemicalReaction(timeStep - 1, 1) - (-density[timeStep - 1][1] * u[timeStep - 1][1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, 1) * A / CalculateHeatCapacity(timeStep - 1, 1) / dx / dx) * temperature[timeStep - 1][0];

            K1[nSpace - 2][nSpace - 3] = -density[timeStep - 1][nSpace - 1] * u[timeStep - 1][nSpace - 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, nSpace - 1) * A / CalculateHeatCapacity(timeStep - 1, nSpace - 1) / dx / dx;
            K1[nSpace - 2][nSpace - 2] = density[timeStep - 1][nSpace - 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, nSpace - 1) * A / CalculateHeatCapacity(timeStep - 1, nSpace - 1) / dx / dx;
            b1[nSpace - 2] = density[timeStep - 1][nSpace - 1] * A * temperature[timeStep - 1][nSpace - 1] / Dt - (-density[timeStep - 1][nSpace - 1] * u[timeStep - 1][nSpace - 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, nSpace - 1) * A / CalculateHeatCapacity(timeStep - 1, nSpace - 1) / dx / dx) * temperature[timeStep - 1][nSpace];

            for (int i = 1; i < nSpace / 10; i++)
            {
                K1[i][i - 1] = -density[timeStep - 1][i + 1] * u[timeStep - 1][i + 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / (dx * dx);
                K1[i][i] = density[timeStep - 1][i + 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / (dx * dx);
                K1[i][i + 1] = density[timeStep - 1][i + 1] * A * u[timeStep - 1][i + 1] * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / (dx * dx);
                b1[i] = density[timeStep - 1][i + 1] * A * temperature[timeStep - 1][i + 1] / Dt;
            }

            for (int i = nSpace / 10; i < nSpace - 2; i++)
            {
                K1[i][i - 1] = -density[timeStep - 1][i + 1] * u[timeStep - 1][i + 1] * A * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / (dx * dx);
                K1[i][i] = density[timeStep - 1][i + 1] * A / Dt + 2 * CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / (dx * dx);
                K1[i][i + 1] = density[timeStep - 1][i + 1] * A * u[timeStep - 1][i + 1] * 0.5 / dx - CalculateThermalConductivity(timeStep - 1, i + 1) * A / CalculateHeatCapacity(timeStep - 1, i + 1) / (dx * dx);
                b1[i] = density[timeStep - 1][i + 1] * A * temperature[timeStep - 1][i + 1] / Dt;
            }

            //온도 업데이트
            var X1 = LUdecompotision(K1, b1.ToList(), nSpace - 1);
            for (int i = 1; i < nSpace - 1; i++)
            {
                temperature[timeStep][i] = X1[i];
            }

            //Momentum equation 
            //에측자 uprime 구하기_upwind
            for (int i = 0; i < nSpace; i++)
            {
                K2.Add(new List<double>());
                for (int j = 0; j < nSpace; j++)
                {
                    K2[i].Add(0.0);
                }
            }

            K2[0][0] = 1 / Dt + u[timeStep - 1][1] / dx;
            b2[0] = 9.81 * (temperature[timeStep][1] - tempInf) / temperature[timeStep][1] + u[timeStep - 1][1] / Dt + (u[timeStep - 1][1] / dx) * u[timeStep - 1][0];

            for (int i = 1; i < nSpace; i++)
            {
                K2[i][i - 1] = -u[timeStep - 1][i + 1] / dx;
                K2[i][i] = 1 / Dt + u[timeStep - 1][i + 1] / dx;
                b2[i] = 9.81 * (temperature[timeStep][i + 1] - tempInf) / temperature[timeStep][i + 1] + u[timeStep - 1][i + 1] / Dt;
            }

            var X2 = LUdecompotision(K2, b2.ToList(), nSpace);

            //수정자 u 구하기_upwind
            for (int i = 0; i < nSpace; i++)
            {
                K3.Add(new List<double>());
                for (int j = 0; j < nSpace; j++)
                {
                    K3[i].Add(0.0);
                }
            }

            K3[0][0] = 1 / Dt + u[timeStep - 1][1] / dx;
            b3[0] = 9.81 * (temperature[timeStep][1] - tempInf) / temperature[timeStep][1] + (u[timeStep - 1][1] + X2[0]) * 0.5 / Dt + (u[timeStep - 1][1] / dx) * u[timeStep - 1][0];

            for (int i = 1; i < nSpace; i++)
            {
                K3[i][i - 1] = -u[timeStep - 1][i + 1] / dx;
                K3[i][i] = 1 / Dt + u[timeStep - 1][i + 1] / dx;
                b3[i] = 9.81 * (temperature[timeStep][i + 1] - tempInf) / temperature[timeStep][1 + i] + (u[timeStep - 1][i + 1] + X2[i]) * 0.5 / Dt;
            }

            //속도 업데이트
            var X3 = LUdecompotision(K3, b3.ToList(), nSpace);
            for (int i = 1; i < nSpace; i++)
            {
                u[timeStep][i] = X3[i];
            }
            u[timeStep][0] = u[timeStep][1]; //JY 추가
            u[timeStep][nSpace] = u[timeStep][nSpace - 1]; //JY 추가

            //Continuity_upwind
            for (int i = 0; i < nSpace; i++)
            {
                K4.Add(new List<double>());
                for (int j = 0; j < nSpace; j++)
                {
                    K4[i].Add(0.0);
                }
            }

            K4[0][0] = 1 / Dt - u[timeStep][1] / dx;
            b4[0] = density[timeStep - 1][1] / Dt - (u[timeStep][0] / Dt) * density[timeStep - 1][0];

            for (int i = 1; i < nSpace; i++)
            {
                K4[i][i - 1] = -u[timeStep][i] / Dt;
                K4[i][i] = 1 / Dt - u[timeStep][i + 1] / dx;
                b4[i] = density[timeStep - 1][i + 1] / Dt;
            }

            //밀도 업데이트
            var X4 = LUdecompotision(K4, b4.ToList(), nSpace);
            for (int i = 1; i < nSpace; i++)
            {
                density[timeStep][i] = X4[i];
            }
        }


        private double CalculateChemicalReaction(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            // Wk의 크기만큼 반복문을 도는 이유는 H, H2, O, O2, OH, H2O, HO2, N2, H2O2, M가 존재하기 때문이다.
            for (int i = 1; i < Wk.Count; i++)
            {
                tmp = tmp + (density[timeStep][spaceStep] * A * Yk[timeStep][spaceStep][i] * Vk * CalculateEachHeatCapacity(timeStep, spaceStep, i) * (temperature[timeStep][spaceStep] - temperature[timeStep][spaceStep - 1]) / dx) + (wDot[timeStep][spaceStep][i] * CalculateEachHeatCapacity(timeStep, spaceStep, i) * Wk[i]);
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

                double h_h2 = (h0_h2 + (temperature[timeStep][p] - 298) * cp_h2) * compCTR[timeStep - 1][p][1];
                double h_o2 = (h0_o2 + (temperature[timeStep][p] - 298) * cp_o2) * compCTR[timeStep - 1][p][3];
                double h_h2o = (h0_h2o + (temperature[timeStep][p] - 298) * cp_h2o) * compCTR[timeStep - 1][p][5];
                double h_n2 = (h0_n2 + (temperature[timeStep][p] - 298) * cp_n2) * compCTR[timeStep - 1][p][7];
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
                                var calcTmpVal = aTmpVal * Math.Pow(temperature[timeStep][p], nTmpVal) * Math.Exp(-eTmpVal / r / temperature[timeStep][p]) * 10E-06;
                                reactionConstant.Add(calcTmpVal);
                            }
                        }
                        var forwardBackwardReaction = Enumerable.Repeat<double>(0.0, 40).ToArray<double>();
                        if (i == 1)
                        {
                            // forward reaction 27개
                            forwardBackwardReaction[1] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][3] * reactionConstant[0];
                            forwardBackwardReaction[2] = compCTR[timeStep - 1][p - 1][1] * compCTR[timeStep - 1][p - 1][2] * reactionConstant[1];
                            forwardBackwardReaction[3] = compCTR[timeStep - 1][p - 1][1] * compCTR[timeStep - 1][p - 1][4] * reactionConstant[2];
                            forwardBackwardReaction[4] = compCTR[timeStep - 1][p - 1][4] * compCTR[timeStep - 1][p - 1][4] * reactionConstant[3];
                            forwardBackwardReaction[5] = 2 * (compCTR[timeStep - 1][p - 1][1] * compCTR[timeStep - 1][p - 1][3] * reactionConstant[4]);
                            forwardBackwardReaction[6] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][9] * reactionConstant[5];
                            forwardBackwardReaction[7] = 0.5 * (compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][1]) * reactionConstant[6];
                            forwardBackwardReaction[8] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][5] * reactionConstant[7];
                            forwardBackwardReaction[9] = compCTR[timeStep - 1][p - 1][2] * compCTR[timeStep - 1][p - 1][2] * compCTR[timeStep - 1][p - 1][9] * reactionConstant[8];
                            forwardBackwardReaction[10] = compCTR[timeStep - 1][p - 1][2] * compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][9] * reactionConstant[9];
                            forwardBackwardReaction[11] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][4] * compCTR[timeStep - 1][p - 1][9] * reactionConstant[10];
                            forwardBackwardReaction[12] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][3] * reactionConstant[11];
                            forwardBackwardReaction[13] = compCTR[timeStep - 1][p - 1][0] * 2 * compCTR[timeStep - 1][p - 1][3] * reactionConstant[12];
                            forwardBackwardReaction[14] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][3] * compCTR[timeStep - 1][p - 1][5] * reactionConstant[13];
                            forwardBackwardReaction[15] = compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][3] * compCTR[timeStep - 1][p - 1][7] * reactionConstant[14];
                            forwardBackwardReaction[16] = compCTR[timeStep - 1][p - 1][6] * compCTR[timeStep - 1][p - 1][0] * reactionConstant[15];
                            forwardBackwardReaction[17] = 2 * (compCTR[timeStep - 1][p - 1][6] * compCTR[timeStep - 1][p - 1][0] * reactionConstant[16]);
                            forwardBackwardReaction[18] = compCTR[timeStep - 1][p - 1][6] * compCTR[timeStep - 1][p - 1][0] * reactionConstant[17];
                            forwardBackwardReaction[19] = compCTR[timeStep - 1][p - 1][6] * compCTR[timeStep - 1][p - 1][2] * reactionConstant[18];
                            forwardBackwardReaction[20] = compCTR[timeStep - 1][p - 1][6] * compCTR[timeStep - 1][p - 1][4] * reactionConstant[19];
                            forwardBackwardReaction[21] = 2 * compCTR[timeStep - 1][p - 1][6] * reactionConstant[20];
                            forwardBackwardReaction[22] = 2 * compCTR[timeStep - 1][p - 1][7] * compCTR[timeStep - 1][p - 1][9] * reactionConstant[21];
                            forwardBackwardReaction[23] = compCTR[timeStep - 1][p - 1][4] * compCTR[timeStep - 1][p - 1][4] * compCTR[timeStep - 1][p - 1][9] * reactionConstant[22];
                            forwardBackwardReaction[24] = compCTR[timeStep - 1][p - 1][8] * compCTR[timeStep - 1][p - 1][0] * reactionConstant[23];
                            forwardBackwardReaction[25] = compCTR[timeStep - 1][p - 1][8] * compCTR[timeStep - 1][p - 1][0] * reactionConstant[24];
                            forwardBackwardReaction[26] = compCTR[timeStep - 1][p - 1][8] * compCTR[timeStep - 1][p - 1][2] * reactionConstant[25];
                            forwardBackwardReaction[27] = compCTR[timeStep - 1][p - 1][8] * compCTR[timeStep - 1][p - 1][4] * reactionConstant[26];
                            // backward reaction 3개
                            forwardBackwardReaction[28] = compCTR[timeStep - 1][p - 1][2] * compCTR[timeStep - 1][p - 1][2] * reactionConstant[27];
                            forwardBackwardReaction[29] = 0.5 * (compCTR[timeStep - 1][p - 1][2] * compCTR[timeStep - 1][p - 1][5]) * reactionConstant[28];
                            forwardBackwardReaction[30] = 0.5 * (compCTR[timeStep - 1][p - 1][1] * compCTR[timeStep - 1][p - 1][9]) * reactionConstant[29];
                        }
                        else
                        {
                            // forward reaction 27개
                            forwardBackwardReaction[1] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][3] * reactionConstant[0];
                            forwardBackwardReaction[2] = compCTR[timeStep][p - 1][1] * compCTR[timeStep][p - 1][2] * reactionConstant[1];
                            forwardBackwardReaction[3] = compCTR[timeStep][p - 1][1] * compCTR[timeStep][p - 1][4] * reactionConstant[2];
                            forwardBackwardReaction[4] = compCTR[timeStep][p - 1][4] * compCTR[timeStep][p - 1][4] * reactionConstant[3];
                            forwardBackwardReaction[5] = 2 * (compCTR[timeStep][p - 1][1] * compCTR[timeStep][p - 1][3] * reactionConstant[4]);
                            forwardBackwardReaction[6] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][9] * reactionConstant[5];
                            forwardBackwardReaction[7] = 0.5 * (compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep - 1][p - 1][0] * compCTR[timeStep][p - 1][1]) * reactionConstant[6];
                            forwardBackwardReaction[8] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][5] * reactionConstant[7];
                            forwardBackwardReaction[9] = compCTR[timeStep][p - 1][2] * compCTR[timeStep][p - 1][2] * compCTR[timeStep][p - 1][9] * reactionConstant[8];
                            forwardBackwardReaction[10] = compCTR[timeStep][p - 1][2] * compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][9] * reactionConstant[9];
                            forwardBackwardReaction[11] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][4] * compCTR[timeStep][p - 1][9] * reactionConstant[10];
                            forwardBackwardReaction[12] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][3] * reactionConstant[11];
                            forwardBackwardReaction[13] = compCTR[timeStep][p - 1][0] * 2 * compCTR[timeStep][p - 1][3] * reactionConstant[12];
                            forwardBackwardReaction[14] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][3] * compCTR[timeStep][p - 1][5] * reactionConstant[13];
                            forwardBackwardReaction[15] = compCTR[timeStep][p - 1][0] * compCTR[timeStep][p - 1][3] * compCTR[timeStep][p - 1][7] * reactionConstant[14];
                            forwardBackwardReaction[16] = compCTR[timeStep][p - 1][6] * compCTR[timeStep][p - 1][3] * reactionConstant[15];
                            forwardBackwardReaction[17] = 2 * (compCTR[timeStep][p - 1][6] * compCTR[timeStep][p - 1][3] * reactionConstant[16]);
                            forwardBackwardReaction[18] = compCTR[timeStep][p - 1][6] * compCTR[timeStep][p - 1][3] * reactionConstant[17];
                            forwardBackwardReaction[19] = compCTR[timeStep][p - 1][6] * compCTR[timeStep][p - 1][2] * reactionConstant[18];
                            forwardBackwardReaction[20] = compCTR[timeStep][p - 1][6] * compCTR[timeStep][p - 1][4] * reactionConstant[19];
                            forwardBackwardReaction[21] = 2 * compCTR[timeStep][p - 1][6] * reactionConstant[20];
                            forwardBackwardReaction[22] = 2 * compCTR[timeStep][p - 1][7] * compCTR[timeStep][p - 1][9] * reactionConstant[21];
                            forwardBackwardReaction[23] = compCTR[timeStep][p - 1][4] * compCTR[timeStep][p - 1][4] * compCTR[timeStep][p - 1][9] * reactionConstant[22];
                            forwardBackwardReaction[24] = compCTR[timeStep][p - 1][8] * compCTR[timeStep][p - 1][0] * reactionConstant[23];
                            forwardBackwardReaction[25] = compCTR[timeStep][p - 1][8] * compCTR[timeStep][p - 1][0] * reactionConstant[24];
                            forwardBackwardReaction[26] = compCTR[timeStep][p - 1][8] * compCTR[timeStep][p - 1][2] * reactionConstant[25];
                            forwardBackwardReaction[27] = compCTR[timeStep][p - 1][8] * compCTR[timeStep][p - 1][4] * reactionConstant[26];
                            // backward reaction 3개
                            forwardBackwardReaction[28] = compCTR[timeStep][p - 1][2] * compCTR[timeStep - 1][p - 1][2] * reactionConstant[27];
                            forwardBackwardReaction[29] = 0.5 * (compCTR[timeStep][p - 1][2] * compCTR[timeStep][p - 1][5]) * reactionConstant[28];
                            forwardBackwardReaction[30] = 0.5 * (compCTR[timeStep][p - 1][1] * compCTR[timeStep][p - 1][9]) * reactionConstant[29];
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
                            compCTR[timeStep][p][2] = (p_c_o - r_c_o) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][2];
                            compCTR[timeStep][p][1] = (p_c_h2 - r_c_h2) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][1];
                            compCTR[timeStep][p][3] = (p_c_o2 - r_c_o2) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][3];
                            compCTR[timeStep][p][0] = (p_c_h - r_c_h) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][0];
                            compCTR[timeStep][p][4] = (p_c_oh - r_c_oh) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][4];
                            compCTR[timeStep][p][6] = (p_c_ho2 - r_c_ho2) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][6];
                            compCTR[timeStep][p][5] = (p_c_h2o - r_c_h2o) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep - 1][p - 1][5];

                        }
                        else
                        {
                            compCTR[timeStep][p][2] = (p_c_o - r_c_o) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][2];
                            compCTR[timeStep][p][1] = (p_c_h2 - r_c_h2) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][1];
                            compCTR[timeStep][p][3] = (p_c_o2 - r_c_o2) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][3];
                            compCTR[timeStep][p][0] = (p_c_h - r_c_h) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][0];
                            compCTR[timeStep][p][4] = (p_c_oh - r_c_oh) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][4];
                            compCTR[timeStep][p][6] = (p_c_ho2 - r_c_ho2) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][6];
                            compCTR[timeStep][p][5] = (p_c_h2o - r_c_h2o) / ((1 / dt) + (u[timeStep][p] / dx)) + compCTR[timeStep][p][5];
                        }

                        double cp_n = (cp_h2 * compCTR[timeStep][p][1] + cp_h2o * compCTR[timeStep][p][5] + cp_n2 * compCTR[timeStep][p][7] + cp_o2 * compCTR[timeStep][p][3]) / (compCTR[timeStep][p][1] + compCTR[timeStep][p][5] + compCTR[timeStep][p][7] + compCTR[timeStep][p][3]);
                        temperature[timeStep][p] = temperature[timeStep][p] + 2 * (-h0_h2o) * forwardBackwardReaction[4] / (cp_n * (compCTR[timeStep][p][1] + compCTR[timeStep][p][5] + compCTR[timeStep][p][7] + compCTR[timeStep][p][3])) * dt;
                        // the hydrogen-air burning rate...ref
                        // temperature[timeStep+1][p];
                        // JY 추가

                        // H,H2,O,O2,OH,H2O,HO2,N2
                        double c_total = compCTR[timeStep][p][1] + compCTR[timeStep][p][0] + compCTR[timeStep][p][5] + compCTR[timeStep][p][3] + compCTR[timeStep][p][4] + compCTR[timeStep][p][6] + compCTR[timeStep][p][2] + compCTR[timeStep][p][7];

                        Yk[timeStep][p][0] = compCTR[timeStep][p][0] / c_total;
                        Yk[timeStep][p][1] = compCTR[timeStep][p][1] / c_total;
                        Yk[timeStep][p][2] = compCTR[timeStep][p][2] / c_total;
                        Yk[timeStep][p][3] = compCTR[timeStep][p][3] / c_total;
                        Yk[timeStep][p][4] = compCTR[timeStep][p][4] / c_total;
                        Yk[timeStep][p][5] = compCTR[timeStep][p][5] / c_total;
                        Yk[timeStep][p][6] = compCTR[timeStep][p][6] / c_total;

                        wDot[timeStep][p][0] = p_c_h - r_c_h;
                        wDot[timeStep][p][1] = p_c_h2 - r_c_h2;
                        wDot[timeStep][p][2] = p_c_o - r_c_o;
                        wDot[timeStep][p][3] = p_c_o2 - r_c_o2;
                        wDot[timeStep][p][4] = p_c_oh - r_c_oh;
                        wDot[timeStep][p][5] = p_c_h2o - r_c_h2o;
                        wDot[timeStep][p][6] = p_c_ho2 - r_c_ho2;
                    }
                }
                else
                {
                    compCTR[timeStep][p][2] = compCTR[timeStep - 1][p - 1][2];
                    compCTR[timeStep][p][1] = compCTR[timeStep - 1][p - 1][1];
                    compCTR[timeStep][p][3] = compCTR[timeStep - 1][p - 1][3];
                    compCTR[timeStep][p][0] = compCTR[timeStep - 1][p - 1][0];
                    compCTR[timeStep][p][4] = compCTR[timeStep - 1][p - 1][4];
                    compCTR[timeStep][p][6] = compCTR[timeStep - 1][p - 1][6];
                    compCTR[timeStep][p][5] = compCTR[timeStep - 1][p - 1][5];

                    double c_total = compCTR[timeStep][p][1] + compCTR[timeStep][p][0] + compCTR[timeStep][p][5] + compCTR[timeStep][p][3] + compCTR[timeStep][p][4] + compCTR[timeStep][p][6] + compCTR[timeStep][p][2] + compCTR[timeStep][p][7];

                    Yk[timeStep][p][0] = compCTR[timeStep][p][0] / c_total;
                    Yk[timeStep][p][1] = compCTR[timeStep][p][1] / c_total;
                    Yk[timeStep][p][2] = compCTR[timeStep][p][2] / c_total;
                    Yk[timeStep][p][3] = compCTR[timeStep][p][3] / c_total;
                    Yk[timeStep][p][4] = compCTR[timeStep][p][4] / c_total;
                    Yk[timeStep][p][5] = compCTR[timeStep][p][5] / c_total;
                    Yk[timeStep][p][6] = compCTR[timeStep][p][6] / c_total;

                    wDot[timeStep][p][0] = 0;
                    wDot[timeStep][p][1] = 0;
                    wDot[timeStep][p][2] = 0;
                    wDot[timeStep][p][3] = 0;
                    wDot[timeStep][p][4] = 0;
                    wDot[timeStep][p][5] = 0;
                    wDot[timeStep][p][6] = 0;
                }
            }
            time[timeStep] = time[timeStep - 1] + Dt;
        }

        private double CalculateHeatCapacity(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            // Wk의 크기만큼 반복문을 도는 이유는 H, H2, O, O2, OH, H2O, HO2, N2, H2O2, M가 존재하기 때문이다.
            for (int i = 0; i < Wk.Count; i++)
            {
                tmp += compCTR[timeStep][spaceStep][i] * CalculateEachHeatCapacity(timeStep, spaceStep, i);
            }
            return tmp;
        }

        private double CalculateEachHeatCapacity(int timeStep, int spaceStep, int speciesNo)
        {
            if (speciesNo == 1)
            {
                return Polynomial(temperature[timeStep][spaceStep], -1.313e-22, 8.49e-19, -2.39e-15, 3.828e-12, -3.815e-9, 2.423e-6, -0.0009553, 0.2132, 8.625);
            }
            else if (speciesNo == 3)
            {
                return Polynomial(temperature[timeStep][spaceStep], -8.836e-23, 4.992e-19, -1.191e-15, 1.526e-12, -1.083e-9, 3.593e-7, 6.151e-6, -0.02646, 32.96);
            }
            else if (speciesNo == 5)
            {
                return Polynomial(temperature[timeStep][spaceStep], 1.3e-21, -9.267e-18, 2.855e-14, -4.963e-11, 5.326e-8, -3.614e-5, 0.01516, -3.588, 401.3);
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
            for (int i = 0; i < Wk.Count; i++)
            {
                tmp += compCTR[timeStep][spaceStep][i] * CalculateEachThermalConductivity(timeStep, spaceStep, i);
            }
            return tmp;
        }

        private double CalculateEachThermalConductivity(int timeStep, int spaceStep, int speciesNo)
        {
            if (speciesNo == 1)
            {
                return Polynomial(temperature[timeStep][spaceStep], 4.716e-23, -2.716e-19, 6.768e-16, -9.528e-13, 8.285e-10, -4.555e-7, 0.0001546, -0.02915, 2.499);
            }
            else if (speciesNo == 3)
            {
                return Polynomial(temperature[timeStep][spaceStep], -2.136e-25, 1.293e-21, -3.401e-18, 5.064e-15, -4.631e-12, 2.62e-9, -8.841e-7, 0.0002421, -0.009727);
            }
            else if (speciesNo == 5)
            {
                return Polynomial(temperature[timeStep][spaceStep], 1.796e-25, -1.295e-21, 4.062e-18, -7.252e-15, 8.087e-12, -5.813e-9, 2.697e-6, -0.0006474, 0.07913);
            }
            else
            {
                return 0;
            }
        }

        private double CalculateEnthalpy(int timeStep, int spaceStep)
        {
            var tmp = 0.0;
            for (int i = 0; i < Wk.Count; i++)
            {
                tmp += compCTR[timeStep][spaceStep][i] * CalculateEachEnthalpy(timeStep, spaceStep, i);
            }
            return tmp;
        }

        private double CalculateEachEnthalpy(int timeStep, int spaceStep, int speciesNo)
        {
            if (speciesNo == 1)
            {
                return Polynomial(temperature[timeStep][spaceStep], 1.616e-19, -8.93e-16, 2.135e-12, -2.885e-9, 2.411e-6, -0.001275, 0.4169, -47.8, 5356);
            }
            else if (speciesNo == 3)
            {
                return Polynomial(temperature[timeStep][spaceStep], -1.313e-22, 8.49e-19, -2.39e-15, 3.828e-12, -3.815e-9, 2.423e-6, -0.0009553, 0.2132, 8.625);
            }
            else if (speciesNo == 5)
            {
                return Polynomial(temperature[timeStep][spaceStep], -5.347e-20, 3.821e-16, -1.181e-12, 2.06e-9, -2.222e-6, 0.00152, -0.6401, 186, 1.997e4);
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

        private List<double> LUdecompotision(List<List<double>> K, List<double> P, int size)
        {
            var a = 0.0;
            for (int i = 1; i < size; i++)
            {
                K[0][i] = K[0][i] / K[0][0];
            }

            for (int i = 1; i < size - 1; i++)
            {
                for (int j = i; j < size; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        a += K[j][k] * K[k][i];
                    }
                    K[j][i] = K[j][i] - a;
                    a = 0.0;
                }
                for (int j = i + 1; j < size; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        a += K[i][k] * K[k][j];
                    }
                    K[i][j] = (K[i][j] - a) / K[i][i];
                    a = 0;
                }
            }

            for (int i = 0; i < size - 1; i++)
            {
                a += K[size - 1][i] * K[i][size - 1];
            }

            K[size - 1][size - 1] = K[size - 1][size - 1] - a;
            a = 0.0;

            var C = Enumerable.Repeat<double>(0.0, size).ToArray<double>();
            C[0] = P[0] / K[0][0];
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    a += K[i][j] * C[j];
                }
                C[i] = (P[i] - a) / K[i][i];
                a = 0.0;
            }

            var X = Enumerable.Repeat<double>(0.0, size).ToArray<double>();
            X[size - 1] = C[size - 1];
            for (int i = size - 2; i >= 0; i--)
            {
                for (int j = size - 1; j >= i + 1; j--)
                {
                    a += K[i][j] * X[j];
                }
                X[i] = C[i] - a;
                a = 0.0;
            }

            return X.ToList();
        }
    }
}
