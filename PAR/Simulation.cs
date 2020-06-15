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
        private double Vk, A, beforeDt, dx, pressure = 1, tempInf;
        private List<double> time, space, Wk;
        private List<List<double>> density, u, temperature;
        private List<List<List<double>>> compCTR, wDot, Yk;

        public Simulation(int nTime, int nSpace, double beforeDt, double dx)
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
            this.beforeDt = beforeDt;
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

                for (int j = 0; j < nSpace + 1; j++)
                {
                    this.density[i].Add(density);
                    this.u[i].Add(0);
                    this.temperature[i].Add(temperature);
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
                    }
                }
            }
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
                    double dT = 0;
                    double dt = 0;
                    int iMax = 10000;
                    dt = beforeDt / iMax;
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
            time[timeStep] = time[timeStep - 1] + beforeDt;
        }

        private double Polynomial(double x, double p1, double p2, double p3, double p4, double p5, double p6, double p7, double p8, double p9)
        {
            double ans = p1 * Math.Pow(x, 8) + p2 * Math.Pow(x, 7) + p3 * Math.Pow(x, 6) + p4 * Math.Pow(x, 5) + p5 * Math.Pow(x, 4) + p6 * Math.Pow(x, 3) + p7 * Math.Pow(x, 2) + p8 * Math.Pow(x, 1) + p9;
            return ans;
        }
    }
}
