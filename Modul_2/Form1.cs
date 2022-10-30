using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using Accord;
using Accord.Math.Optimization;

namespace Modul_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.gauss_shiftparameter = 0.0;
            this.gauss_scale = 1.0;
            this.gauss_nu = 3.0;
            this.gamma = chart1.DataManipulator.Statistics.GammaFunction(1.0 / this.gauss_nu);
            this.clogg_expectedvalue = 0.0;
            this.clogg_dispersion = 1.0;
            this.clogg_scale = 1.0;
            this.clogg_level = 0.1;
            this.sample_size = 100;
            this.percent_emission = 5.0;
            this.Alpha1 = 0.05;
            this.Alpha2 = 0.1;
            this.Alpha3 = 0.15;
            this.Delta1 = 0.1;
            this.Delta2 = 0.5;
            this.Delta3 = 1.0;
            this.M = 100;
            this.N = 200;
            this.comboBox2.SelectedIndex = 0;

            this.PaintDistribution();
            this.PaintClogg();
            this.PaintClogging();

            this.textBox1.DataBindings.Add("Text", this, "GaussShiftParameter");
            this.textBox2.DataBindings.Add("Text", this, "GaussScale");
            this.textBox3.DataBindings.Add("Text", this, "GaussNu");
            this.textBox4.DataBindings.Add("Text", this, "CloggExpectedValue");
            this.textBox5.DataBindings.Add("Text", this, "CloggDispersion");
            this.textBox6.DataBindings.Add("Text", this, "CloggLevel");
            this.textBox7.DataBindings.Add("Text", this, "CloggScale");
            this.textBox8.DataBindings.Add("Text", this, "SampleSize");
            this.textBox9.DataBindings.Add("Text", this, "PercentEmission");
            this.textBox10.DataBindings.Add("Text", this, "Alpha1");
            this.textBox14.DataBindings.Add("Text", this, "Alpha2");
            this.textBox15.DataBindings.Add("Text", this, "Alpha3");
            this.textBox11.DataBindings.Add("Text", this, "Delta1");
            this.textBox16.DataBindings.Add("Text", this, "Delta2");
            this.textBox17.DataBindings.Add("Text", this, "Delta3");
            this.textBox12.DataBindings.Add("Text", this, "M");
            this.textBox13.DataBindings.Add("Text", this, "N");

            //this.label15.Text = "";
            //this.label20.Text = "";
            //this.label21.Text = "";
            //this.label22.Text = "";
            //this.label23.Text = "";
            //this.label27.Text = "";
            //this.label28.Text = "";
            //this.label29.Text = "";
        }

        private double gauss_shiftparameter;
        private double gauss_scale;
        private double gauss_nu;

        private double clogg_expectedvalue;
        private double clogg_dispersion;
        private double clogg_scale;
        private double clogg_level;
        private int sample_size;
        private double percent_emission;
        private double alpha_1;
        private double alpha_2;
        private double alpha_3;
        private double delta_1;
        private double delta_2;
        private double delta_3;
        private double gamma;
        private double const1;
        private double const2;
        private double const3;

        private List<double> sample = new List<double>();
        private List<double> medium = new List<double>();
        private List<double> median = new List<double>();
        private List<double> dispersion = new List<double>();
        private List<double> beta3 = new List<double>();
        private List<double> beta4 = new List<double>();
        private List<double> trimmed_mean_1 = new List<double>();
        private List<double> trimmed_mean_2 = new List<double>();
        private List<double> trimmed_mean_3 = new List<double>();
        private List<double> evaluation_1 = new List<double>();
        private List<double> evaluation_2 = new List<double>();
        private List<double> evaluation_3 = new List<double>();
        private List<double> evaluation_4 = new List<double>();

        private int m;
        private int n;


        public int M
        {
            get { return m; }
            set
            {
                if (value >= 100)
                    m = value;
            }
        }
        
        public int N
        {
            get { return n; }
            set
            {
                if (value >= 100)
                    n = value;
            }
        }


        public double GaussShiftParameter
        {
            get { return gauss_shiftparameter; }
            set
            {
                this.gauss_shiftparameter = value;
                this.PaintDistribution();
                this.PaintClogging();
            }
        }
        public double GaussScale
        { 
            get { return this.gauss_scale; }
            set
            {
                if (value > 0)
                { 
                    this.gauss_scale = value;
                    this.PaintDistribution();
                    this.PaintClogging();
                }
            }
        }
        public double GaussNu
        {
            get { return this.gauss_nu; }
            set
            {
                if (value >= 1)
                { 
                    this.gauss_nu = value;
                    gamma = chart1.DataManipulator.Statistics.GammaFunction(1.0 / this.gauss_nu);
                    this.PaintDistribution();
                    this.PaintClogging();
                } 
            }
        }
        public double CloggExpectedValue
        { 
            get { return clogg_expectedvalue; }
            set
            {
                this.clogg_expectedvalue = value;
                this.PaintClogg();
                this.PaintClogging();
            }
        }
        public double CloggDispersion
        { 
            get { return clogg_dispersion; }
            set
            {
                if (value > 0)
                {
                    this.clogg_dispersion = value;
                    this.PaintClogg();
                    this.PaintClogging();
                }
            }
        }
        public double CloggScale
        {
            get { return clogg_scale; }
            set
            {
                if (value > 0)
                {
                    this.clogg_scale = value;
                    this.PaintClogg();
                    this.PaintClogging();
                }
            }
        }
        public double CloggLevel
        {
            get { return this.clogg_level; }
            set
            {
                if ((value > 0) && (value < 0.5))
                {
                    this.clogg_level = value;
                    this.PaintClogging();
                }
            }
        }
        public int SampleSize
        {
            get { return sample_size; }
            set
            {
                if (value > 0)
                    sample_size = value;
            }
        }
        public double PercentEmission
        {
            get { return percent_emission; }
            set
            {
                if ((value > 0) && (value < 100))
                    percent_emission = value;
            }
        }
        public double Alpha1
        {
            get { return alpha_1; }
            set 
            {
                if ((value > 0) && (value < 0.5))
                    alpha_1 = value; 
            }
        }
        public double Alpha2
        {
            get { return alpha_2; }
            set
            {
                if ((value > 0) && (value < 0.5))
                    alpha_2 = value;
            }
        }
        public double Alpha3
        {
            get { return alpha_3; }
            set
            {
                if ((value > 0) && (value < 0.5))
                    alpha_3 = value;
            }
        }
        public double Delta1
        {
            get { return delta_1; }
            set
            {
                if ((value > 0) && (value <= 1.0))
                {
                    delta_1 = value;
                    const1 = -1.0 / Math.Pow(GaussFunction(0), delta_1);
                }
            }
        }
        public double Delta2
        {
            get { return delta_2; }
            set
            {
                if ((value > 0) && (value <= 1.0))
                {
                    delta_2 = value;
                    const2 = -1.0 / Math.Pow(GaussFunction(0), delta_2);
                }
            }
        }
        public double Delta3
        {
            get { return delta_3; }
            set
            {
                if ((value > 0) && (value <= 1.0))
                {
                    delta_3 = value;
                    const3 = -1.0 / Math.Pow(GaussFunction(0), delta_3);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double var;
            if (double.TryParse(this.textBox1.Text, out var))
            {
                textBox1.BackColor = Color.White;
            }
            else
            {
                textBox1.BackColor = Color.Red;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox2.Text, out var)) && (var > 0))
            {
                textBox2.BackColor = Color.White;
            }
            else
            {
                textBox2.BackColor = Color.Red;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox3.Text, out var)) && (var >= 1))
            {
                textBox3.BackColor = Color.White;
            }
            else
            {
                textBox3.BackColor = Color.Red;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            double var;
            if (double.TryParse(this.textBox4.Text, out var))
            {
                textBox4.BackColor = Color.White;
            }
            else
            {
                textBox4.BackColor = Color.Red;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox5.Text, out var)) && (var > 0))
            {
                textBox5.BackColor = Color.White;
            }
            else
            {
                textBox5.BackColor = Color.Red;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox6.Text, out var)) && (var > 0) && (var <= 0.5))
            {
                textBox6.BackColor = Color.White;
            }
            else
            {
                textBox6.BackColor = Color.Red;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox7.Text, out var)) && (var > 0))
            {
                textBox7.BackColor = Color.White;
            }
            else
            {
                textBox7.BackColor = Color.Red;
            }
        }

        private void PaintDistribution()
        {
            double a = this.GaussShiftParameter - this.GaussScale * 5.0;
            double b = this.GaussShiftParameter + this.GaussScale * 5.0;
            double h = (b - a) / 200.0;
            double x, y;

            this.chart1.Series[0].Points.Clear();
            x = a;
            while (x <= b)
            {
                y = gauss_nu / (2.0 * gamma) * Math.Exp(- Math.Pow(Math.Abs(x - GaussShiftParameter), gauss_nu)) * this.gauss_scale;
                this.chart1.Series[0].Points.AddXY(x, y);
                x += h;
            }
        }

        private double GaussFunction(double x)
        {
            return gauss_nu / (2.0 * gamma) * Math.Exp(-Math.Pow(Math.Abs(x), gauss_nu));
        }

        private double RhoMaximal(double y, double teta)
        {
            return -Math.Log(GaussFunction((y - teta) / GaussScale));
        }

        private double RhoRadical(double const_, double y, double teta, double delta)
        {
            return const_ * Math.Pow(GaussFunction((y - teta) / GaussScale), delta);
        }

        private double FunctionalMaximal(double teta)
        {
            double result = 0.0;
            foreach (double y in sample)
            {
                result += RhoMaximal(y, teta);
            }
            return result;
        }

        private double FunctionalRadical1(double teta)
        {
            double result = 0.0;
            foreach (double y in sample)
            {
                result += RhoRadical(const1, y, teta, delta_1);
            }
            return result;
        }

        private double FunctionalRadical2(double teta)
        {
            double result = 0.0;
            foreach (double y in sample)
            {
                result += RhoRadical(const2, y, teta, delta_2);
            }
            return result;
        }

        private double FunctionalRadical3(double teta)
        {
            double result = 0.0;
            foreach (double y in sample)
            {
                result += RhoRadical(const3, y, teta, delta_3);
            }
            return result;
        }

        private void PaintClogg()
        {
            double sigma = Math.Sqrt(this.clogg_dispersion);
            double a = this.clogg_expectedvalue - sigma * 3.0;
            double b = this.clogg_expectedvalue + sigma * 3.0;
            double h = (b - a) / 200.0;
            double const1 = 1.0 / (sigma * Math.Sqrt(2.0 * Math.PI));
            double x, y;

            this.chart2.Series[0].Points.Clear();
            x = a;
            while (x <= b)
            {
                y = const1 * Math.Exp(-0.5 * Math.Pow((x - this.clogg_expectedvalue) / sigma, 2)) * clogg_scale;
                this.chart2.Series[0].Points.AddXY(x, y);
                x += h;
            }
        }

        private void PaintClogging()
        {
            double g = chart1.DataManipulator.Statistics.GammaFunction(1.0 / this.gauss_nu);
            double sigma = Math.Sqrt(this.clogg_dispersion);

            double a1 = this.clogg_expectedvalue - sigma * 3.0;
            double b1 = this.clogg_expectedvalue + sigma * 3.0;
            double a2 = this.GaussShiftParameter - this.GaussScale * 5.0;
            double b2 = this.GaussShiftParameter + this.GaussScale * 5.0;
            double a = Math.Min(a1, a2);
            double b = Math.Max(b1, b2);
            double h = (b - a) / 200.0;

            double const1 = 1.0 / (sigma * Math.Sqrt(2.0 * Math.PI));
            double x, y1, y2, y;

            this.chart3.Series[0].Points.Clear();
            this.chart3.Series[1].Points.Clear();
            this.chart3.Series[2].Points.Clear();
            x = a;
            while (x <= b)
            {
                y1 = const1 * Math.Exp(-0.5 * Math.Pow((x - this.clogg_expectedvalue) / sigma, 2)) * clogg_scale;
                y2 = gauss_nu / (2.0 * g) * Math.Exp(-Math.Pow(Math.Abs(x - GaussShiftParameter), gauss_nu)) * this.gauss_scale;
                y = (1.0 - this.clogg_level) * y2 + this.clogg_level * y1;
                this.chart3.Series[0].Points.AddXY(x, y);
                this.chart3.Series[1].Points.AddXY(x, y1);
                this.chart3.Series[2].Points.AddXY(x, y2);
                x += h;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(this.comboBox2.SelectedIndex)
            {
                case 0:
                    {
                        this.panel1.Enabled = false;
                        this.panel1.Visible = false;

                        this.panel2.Enabled = false;
                        this.panel2.Visible = false;

                        this.panel3.Enabled = false;
                        this.panel3.Visible = false;

                        this.panel4.Enabled = false;
                        this.panel4.Visible = false;

                        this.panel5.Enabled = false;
                        this.panel5.Visible = false;

                        this.label12.Visible = false;
                        this.textBox8.Enabled = false;
                        this.textBox8.Visible = false;

                        this.panel6.Enabled = false;
                        this.panel6.Visible = false;

                        this.panel7.Enabled = false;
                        this.panel7.Visible = false;

                        this.panel8.Enabled = false;
                        this.panel8.Visible = false;

                        this.panel9.Enabled = false;
                        this.panel9.Visible = false;
                        break;
                    }

                case 1:
                    {
                        this.panel1.Enabled = false;
                        this.panel1.Visible = false;

                        this.panel2.Enabled = true;
                        this.panel2.Visible = true;

                        this.panel3.Enabled = true;
                        this.panel3.Visible = true;

                        this.panel4.Enabled = true;
                        this.panel4.Visible = true;

                        this.panel5.Enabled = false;
                        this.panel5.Visible = false;

                        this.label12.Visible = true;
                        this.textBox8.Enabled = true;
                        this.textBox8.Visible = true;

                        this.panel6.Enabled = false;
                        this.panel6.Visible = false;

                        this.panel7.Enabled = false;
                        this.panel7.Visible = false;

                        this.panel8.Enabled = false;
                        this.panel8.Visible = false;

                        this.panel9.Enabled = false;
                        this.panel9.Visible = false;

                        this.M = 1;
                        break;
                    }

                case 2:
                    {
                        this.panel1.Enabled = true;
                        this.panel1.Visible = true;

                        this.panel2.Enabled = false;
                        this.panel2.Visible = false;

                        this.panel3.Enabled = true;
                        this.panel3.Visible = true;

                        this.panel4.Enabled = false;
                        this.panel4.Visible = false;

                        this.panel5.Enabled = false;
                        this.panel5.Visible = false;

                        this.label12.Visible = true;
                        this.textBox8.Enabled = true;
                        this.textBox8.Visible = true;

                        this.panel6.Enabled = false;
                        this.panel6.Visible = false;

                        this.panel7.Enabled = false;
                        this.panel7.Visible = false;

                        this.panel8.Enabled = false;
                        this.panel8.Visible = false;

                        this.panel9.Enabled = false;
                        this.panel9.Visible = false;

                        this.M = 1;
                        break;
                    }

                case 3:
                    {
                        this.panel1.Enabled = false;
                        this.panel1.Visible = false;

                        this.panel2.Enabled = true;
                        this.panel2.Visible = true;

                        this.panel3.Enabled = true;
                        this.panel3.Visible = true;

                        this.panel4.Enabled = true;
                        this.panel4.Visible = true;

                        this.panel5.Enabled = false;
                        this.panel5.Visible = false;

                        this.label12.Visible = true;
                        this.textBox8.Enabled = true;
                        this.textBox8.Visible = true;

                        this.panel6.Enabled = false;
                        this.panel6.Visible = false;

                        this.panel7.Enabled = false;
                        this.panel7.Visible = false;

                        this.panel8.Enabled = false;
                        this.panel8.Visible = false;

                        this.panel9.Enabled = false;
                        this.panel9.Visible = false;

                        this.M = 1;
                        break;
                    }

                case 4:
                    {
                        this.panel1.Enabled = false;
                        this.panel1.Visible = false;

                        this.panel2.Enabled = false;
                        this.panel2.Visible = false;

                        this.panel3.Enabled = true;
                        this.panel3.Visible = true;

                        this.panel4.Enabled = true;
                        this.panel4.Visible = true;

                        this.panel5.Enabled = true;
                        this.panel5.Visible = true;

                        this.label12.Visible = false;
                        this.textBox8.Enabled = false;
                        this.textBox8.Visible = false;

                        this.panel6.Enabled = true;
                        this.panel6.Visible = true;

                        this.panel7.Enabled = true;
                        this.panel7.Visible = true;

                        this.panel8.Enabled = true;
                        this.panel8.Visible = true;

                        this.panel9.Enabled = true;
                        this.panel9.Visible = true;

                        break;
                    }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            int var;
            if ((int.TryParse(this.textBox8.Text, out var)) && (var > 0))
            {
                textBox8.BackColor = Color.White;
            }
            else
            {
                textBox8.BackColor = Color.Red;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox9.Text, out var)) && (var > 0) && (var < 100))
            {
                textBox9.BackColor = Color.White;
            }
            else
            {
                textBox9.BackColor = Color.Red;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox10.Text, out var)) && (var >= 0) && (var < 0.5))
            {
                textBox10.BackColor = Color.White;
            }
            else
            {
                textBox10.BackColor = Color.Red;
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox11.Text, out var)) && (var > 0) && (var <= 1.0))
            {
                textBox11.BackColor = Color.White;
            }
            else
            {
                textBox11.BackColor = Color.Red;
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            int var;
            if ((int.TryParse(this.textBox12.Text, out var)) && (var >= 100))
            {
                textBox12.BackColor = Color.White;
            }
            else
            {
                textBox12.BackColor = Color.Red;
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            int var;
            if ((int.TryParse(this.textBox13.Text, out var)) && (var >= 100))
            {
                textBox13.BackColor = Color.White;
            }
            else
            {
                textBox13.BackColor = Color.Red;
            }
        }

        private void CalculateAdditionalStatistic()
        {
            double trimmed_mean_1 = 0.0;
            double trimmed_mean_2 = 0.0;
            double trimmed_mean_3 = 0.0;
            int k_1 = (int)(sample_size * alpha_1);
            int k_2 = (int)(sample_size * alpha_2);
            int k_3 = (int)(sample_size * alpha_3);
            sample.Sort();

            for (int i = k_1; i < sample_size - k_1; i++)
            {
                trimmed_mean_1 += sample[i];
            }
            for (int i = k_2; i < sample_size - k_2; i++)
            {
                trimmed_mean_2 += sample[i];
            }
            for (int i = k_3; i < sample_size - k_3; i++)
            {
                trimmed_mean_3 += sample[i];
            }

            double q1 = sample[sample_size - k_1 - 1];
            double q2 = sample[sample_size - k_2 - 1];
            double q3 = sample[sample_size - k_3 - 1];

            trimmed_mean_1 /= (sample_size - 2 * k_1);
            trimmed_mean_2 /= (sample_size - 2 * k_2);
            trimmed_mean_3 /= (sample_size - 2 * k_3);

            Func<double[], double> func_1 = x => FunctionalRadical1(x[0]);
            Func<double[], double> func_2 = x => FunctionalRadical2(x[0]);
            Func<double[], double> func_3 = x => FunctionalRadical3(x[0]);
            Func<double[], double> func_4 = x => FunctionalMaximal(x[0]);

            Cobyla cobyla_1 = new Cobyla(1, func_1);
            Cobyla cobyla_2 = new Cobyla(1, func_2);
            Cobyla cobyla_3 = new Cobyla(1, func_3);
            Cobyla cobyla_4 = new Cobyla(1, func_4);

            bool success_1 = cobyla_1.Minimize();
            bool success_2 = cobyla_2.Minimize();
            bool success_3 = cobyla_3.Minimize();
            bool success_4 = cobyla_4.Minimize();

            double minimum_1 = cobyla_1.Value;
            double minimum_2 = cobyla_2.Value;
            double minimum_3 = cobyla_3.Value;
            double minimum_4 = cobyla_4.Value;

            double[] solution_1 = cobyla_1.Solution;
            double[] solution_2 = cobyla_2.Solution;
            double[] solution_3 = cobyla_3.Solution;
            double[] solution_4 = cobyla_4.Solution;

            this.trimmed_mean_1.Add(trimmed_mean_1);
            this.trimmed_mean_2.Add(trimmed_mean_2);
            this.trimmed_mean_3.Add(trimmed_mean_3);

            this.evaluation_1.Add(solution_1[0]);
            this.evaluation_2.Add(solution_2[0]);
            this.evaluation_3.Add(solution_3[0]);
            this.evaluation_4.Add(solution_4[0]);
        }

        private void CalculateStatistic()
        {
            double medium = 0.0;
            double median;
            double dispersion = 0.0;
            double beta3 = 0.0;
            double beta4 = 0.0;

            foreach (double y in sample)
            {
                medium += y;
            }
            medium /= sample_size;

            double d2;
            double d3;
            double d4;
            foreach (double y in sample)
            {
                d2 = (y - medium) * (y - medium);
                d3 = d2 * (y - medium);
                d4 = d2 * d2;

                dispersion += (y - medium) * (y - medium);
                beta3 += d3;
                beta4 += d4;
            }
            dispersion /= sample_size;
            beta3 /= sample_size * Math.Sqrt(Math.Pow(dispersion, 3.0));
            beta4 /= sample_size * dispersion * dispersion;

            if (sample_size % 2 != 0) median = sample.NthItem((int)(sample_size / 2));
            else median = 0.5 * (sample.NthItem(sample_size / 2 - 1) + sample.NthItem(sample_size / 2));

            this.medium.Add(medium);
            this.median.Add(median);
            this.dispersion.Add(dispersion);
            this.beta3.Add(beta3);
            this.beta4.Add(beta4);
        }

        private void DrawSample()
        {
            this.chart4.Series[0].Points.Clear();

            this.chart4.ChartAreas[0].AxisX.Minimum = 1;
            this.chart4.ChartAreas[0].AxisX.Maximum = sample_size;

            foreach (double y in sample)
            {
                this.chart4.Series[0].Points.Add(y);
            }
        }

        private void TextOfLabel()
        {
            if ((this.comboBox2.SelectedIndex == 1) || (this.comboBox2.SelectedIndex == 3) || (this.comboBox2.SelectedIndex == 4))
            {
                double trimmed_mean_1 = 0.0;
                double trimmed_mean_2 = 0.0;
                double trimmed_mean_3 = 0.0;

                double evaluation_1 = 0.0;
                double evaluation_2 = 0.0;
                double evaluation_3 = 0.0;
                double evaluation_4 = 0.0;

                for (int i = 0; i < this.trimmed_mean_1.Count; i++)
                {
                    trimmed_mean_1 += this.trimmed_mean_1[i];
                }
                for (int i = 0; i < this.trimmed_mean_2.Count; i++)
                {
                    trimmed_mean_2 += this.trimmed_mean_2[i];
                }
                for (int i = 0; i < this.trimmed_mean_3.Count; i++)
                {
                    trimmed_mean_3 += this.trimmed_mean_3[i];

                }

                for (int i = 0; i < this.evaluation_1.Count; i++)
                {
                    evaluation_1 += this.evaluation_1[i];
                }
                for (int i = 0; i < this.evaluation_2.Count; i++)
                {
                    evaluation_2 += this.evaluation_2[i];
                }
                for (int i = 0; i < this.evaluation_3.Count; i++)
                {
                    evaluation_3 += this.evaluation_3[i];
                }
                for (int i = 0; i < this.evaluation_4.Count; i++)
                {
                    evaluation_4 += this.evaluation_4[i];
                }

                trimmed_mean_1 /= this.trimmed_mean_1.Count;
                trimmed_mean_2 /= this.trimmed_mean_2.Count;
                trimmed_mean_3 /= this.trimmed_mean_3.Count;
                evaluation_1 /= this.evaluation_1.Count;
                evaluation_2 /= this.evaluation_2.Count;
                evaluation_3 /= this.evaluation_3.Count;
                evaluation_4 /= this.evaluation_4.Count;

                label27.Text = trimmed_mean_1.ToString("f4");
                label34.Text = trimmed_mean_2.ToString("f4");
                label35.Text = trimmed_mean_3.ToString("f4");

                label28.Text = evaluation_1.ToString("f4");
                label36.Text = evaluation_2.ToString("f4");
                label37.Text = evaluation_3.ToString("f4");

                label29.Text = evaluation_4.ToString("f4");

                if (this.comboBox2.SelectedIndex == 4)
                {
                    double d1_trimmed_mean_1 = 0.0;
                    double d2_trimmed_mean_1 = 0.0;
                    double d3_trimmed_mean_1 = 0.0;

                    double d1_trimmed_mean_2 = 0.0;
                    double d2_trimmed_mean_2 = 0.0;
                    double d3_trimmed_mean_2 = 0.0;

                    double d1_trimmed_mean_3 = 0.0;
                    double d2_trimmed_mean_3 = 0.0;
                    double d3_trimmed_mean_3 = 0.0;

                    double d1_evaluation_1 = 0.0;
                    double d2_evaluation_1 = 0.0;
                    double d3_evaluation_1 = 0.0;

                    double d1_evaluation_2 = 0.0;
                    double d2_evaluation_2 = 0.0;
                    double d3_evaluation_2 = 0.0;

                    double d1_evaluation_3 = 0.0;
                    double d2_evaluation_3 = 0.0;
                    double d3_evaluation_3 = 0.0;

                    double d1_evaluation_4 = 0.0;
                    double d2_evaluation_4 = 0.0;
                    double d3_evaluation_4 = 0.0;

                    for (int i = 0; i < this.trimmed_mean_1.Count; i++)
                    {
                        d1_trimmed_mean_1 += (this.trimmed_mean_1[i] - trimmed_mean_1) * (this.trimmed_mean_1[i] - trimmed_mean_1);
                    }
                    for (int i = 0; i < this.trimmed_mean_2.Count; i++)
                    {
                        d1_trimmed_mean_2 += (this.trimmed_mean_2[i] - trimmed_mean_2) * (this.trimmed_mean_2[i] - trimmed_mean_2);
                    }
                    for (int i = 0; i < this.trimmed_mean_3.Count; i++)
                    {
                        d1_trimmed_mean_3 += (this.trimmed_mean_3[i] - trimmed_mean_3) * (this.trimmed_mean_3[i] - trimmed_mean_3);
                    }

                    for (int i = 0; i < this.evaluation_1.Count; i++)
                    {
                        d1_evaluation_1 += (this.evaluation_1[i] - evaluation_1) * (this.evaluation_1[i] - evaluation_1);
                    }
                    for (int i = 0; i < this.evaluation_2.Count; i++)
                    {
                        d1_evaluation_2 += (this.evaluation_2[i] - evaluation_2) * (this.evaluation_2[i] - evaluation_2);
                    }
                    for (int i = 0; i < this.evaluation_3.Count; i++)
                    {
                        d1_evaluation_3 += (this.evaluation_3[i] - evaluation_3) * (this.evaluation_3[i] - evaluation_3);
                    }
                    for (int i = 0; i < this.evaluation_4.Count; i++)
                    {
                        d1_evaluation_4 += (this.evaluation_4[i] - evaluation_4) * (this.evaluation_4[i] - evaluation_4);
                    }

                    d1_trimmed_mean_1 /= this.trimmed_mean_1.Count;
                    d2_trimmed_mean_1 = (trimmed_mean_1 - gauss_shiftparameter) * (trimmed_mean_1 - gauss_shiftparameter);
                    d3_trimmed_mean_1 = d1_trimmed_mean_1 + d2_trimmed_mean_1;

                    d1_trimmed_mean_2 /= this.trimmed_mean_2.Count;
                    d2_trimmed_mean_2 = (trimmed_mean_2 - gauss_shiftparameter) * (trimmed_mean_2 - gauss_shiftparameter);
                    d3_trimmed_mean_2 = d1_trimmed_mean_2 + d2_trimmed_mean_2;

                    d1_trimmed_mean_3 /= this.trimmed_mean_3.Count;
                    d2_trimmed_mean_3 = (trimmed_mean_3 - gauss_shiftparameter) * (trimmed_mean_3 - gauss_shiftparameter);
                    d3_trimmed_mean_3 = d1_trimmed_mean_3 + d2_trimmed_mean_3;

                    d1_evaluation_1 /= this.evaluation_1.Count;
                    d2_evaluation_1 = (evaluation_1 - gauss_shiftparameter) * (evaluation_1 - gauss_shiftparameter);
                    d3_evaluation_1 = d1_evaluation_1 + d2_evaluation_1;

                    d1_evaluation_2 /= this.evaluation_2.Count;
                    d2_evaluation_2 = (evaluation_2 - gauss_shiftparameter) * (evaluation_2 - gauss_shiftparameter);
                    d3_evaluation_2 = d1_evaluation_2 + d2_evaluation_2;

                    d1_evaluation_3 /= this.evaluation_3.Count;
                    d2_evaluation_3 = (evaluation_3 - gauss_shiftparameter) * (evaluation_3 - gauss_shiftparameter);
                    d3_evaluation_3 = d1_evaluation_3 + d2_evaluation_3;

                    d1_evaluation_4 /= this.evaluation_4.Count;
                    d2_evaluation_4 = (evaluation_4 - gauss_shiftparameter) * (evaluation_4 - gauss_shiftparameter);
                    d3_evaluation_4 = d1_evaluation_4 + d2_evaluation_4;


                    label41.Text = d1_trimmed_mean_1.ToString("f4");
                    label42.Text = d2_trimmed_mean_1.ToString("f4");
                    label43.Text = d3_trimmed_mean_1.ToString("f4");

                    label44.Text = d1_trimmed_mean_2.ToString("f4");
                    label45.Text = d2_trimmed_mean_2.ToString("f4");
                    label46.Text = d3_trimmed_mean_2.ToString("f4");

                    label47.Text = d1_trimmed_mean_3.ToString("f4");
                    label48.Text = d2_trimmed_mean_3.ToString("f4");
                    label49.Text = d3_trimmed_mean_3.ToString("f4");

                    label53.Text = d1_evaluation_1.ToString("f4");
                    label56.Text = d2_evaluation_1.ToString("f4");
                    label57.Text = d3_evaluation_1.ToString("f4");

                    label54.Text = d1_evaluation_2.ToString("f4");
                    label58.Text = d2_evaluation_2.ToString("f4");
                    label59.Text = d3_evaluation_2.ToString("f4");

                    label55.Text = d1_evaluation_3.ToString("f4");
                    label60.Text = d2_evaluation_3.ToString("f4");
                    label61.Text = d3_evaluation_3.ToString("f4");

                    label65.Text = d1_evaluation_4.ToString("f4");
                    label66.Text = d2_evaluation_4.ToString("f4");
                    label67.Text = d3_evaluation_4.ToString("f4");
                }
            }

            double medium = 0.0;
            double median = 0.0;
            double dispersion = 0.0;
            double beta3 = 0.0;
            double beta4 = 0.0;

            for (int i = 0; i < this.medium.Count; i++)
            {
                medium += this.medium[i];
                median += this.median[i];
                dispersion += this.dispersion[i];
                beta3 += this.beta3[i];
                beta4 += this.beta4[i];
            }

            medium /= this.medium.Count;
            median /= this.median.Count;
            dispersion /= this.dispersion.Count;
            beta3 /= this.beta3.Count;
            beta4 /= this.beta4.Count;

            label15.Text = medium.ToString("f4");
            label20.Text = median.ToString("f4");
            label21.Text = dispersion.ToString("f4");
            label22.Text = beta3.ToString("f4");
            label23.Text = beta4.ToString("f4");

            if (this.comboBox2.SelectedIndex == 4)
            {
                double d1_medium = 0.0;
                double d2_medium = 0.0;
                double d3_medium = 0.0;

                double d1_median = 0.0;
                double d2_median = 0.0;
                double d3_median = 0.0;

                for (int i = 0; i < this.medium.Count; i++)
                {
                    d1_medium += (this.medium[i] - medium) * (this.medium[i] - medium);
                }

                d1_medium /= this.medium.Count;
                d2_medium = (medium - gauss_shiftparameter) * (medium - gauss_shiftparameter);
                d3_medium = d1_medium + d2_medium;

                for (int i = 0; i < this.median.Count; i++)
                {
                    d1_median += (this.median[i] - median) * (this.median[i] - median);
                }
                d1_median /= this.median.Count;
                d2_median = (median - gauss_shiftparameter) * (median - gauss_shiftparameter);
                d3_median = d1_median + d2_median;

                label69.Text = d1_medium.ToString("f4");
                label70.Text = d2_medium.ToString("f4");
                label71.Text = d3_medium.ToString("f4");

                label72.Text = d1_median.ToString("f4");
                label73.Text = d2_median.ToString("f4");
                label74.Text = d3_median.ToString("f4");
            }
        }

        private void GenSample()
        {
            sample.Clear();
            Random r = new Random();
            int count = 0;

            double a = 1.0 / gauss_nu - 0.5;
            double b = 1.0 / Math.Pow(gauss_nu, 1.0 / gauss_nu);
            double c = 2.0 * b * b;

            if (this.comboBox2.SelectedIndex == 1)
            {
                do
                {
                    double x = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Cos(2.0 * Math.PI * r.NextDouble()) * b;

                    if (Math.Log(r.NextDouble() * b) <= -Math.Pow(Math.Abs(x), gauss_nu) + x * x / c + a)
                    {
                        x = x * gauss_scale + gauss_shiftparameter;
                        sample.Add(x);
                        count++;
                    }
                } while (count < sample_size);
            }
            else if (this.comboBox2.SelectedIndex == 2)
            {
                do
                {
                    double x = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Cos(2.0 * Math.PI * r.NextDouble()) * b;

                    if (Math.Log(r.NextDouble() * b) <= -Math.Pow(Math.Abs(x), gauss_nu) + x * x / c + a)
                    {
                        if (r.NextDouble() < percent_emission / 100.0) x *= 4;
                        x = x * gauss_scale + gauss_shiftparameter;
                        sample.Add(x);
                        count++;
                    }
                } while (count < sample_size);
            }
            else if ((this.comboBox2.SelectedIndex == 3) || (this.comboBox2.SelectedIndex == 4))
            {
                do
                {
                    double x = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Cos(2.0 * Math.PI * r.NextDouble()) * b;

                    if (r.NextDouble() < 1 - clogg_level)
                    {
                        if (Math.Log(r.NextDouble() * b) <= -Math.Pow(Math.Abs(x), gauss_nu) + x * x / c + a)
                        {
                            x = x * gauss_scale + gauss_shiftparameter;
                            sample.Add(x);
                            count++;
                        }
                    }
                    else
                    {
                        x = x * clogg_scale * clogg_dispersion + clogg_expectedvalue;
                        sample.Add(x);
                        count++;
                    }
                } while (count < sample_size);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            medium.Clear();
            median.Clear();
            dispersion.Clear();
            beta3.Clear();
            beta4.Clear();
            trimmed_mean_1.Clear();
            trimmed_mean_2.Clear();
            trimmed_mean_3.Clear();
            evaluation_1.Clear();
            evaluation_2.Clear();
            evaluation_3.Clear();
            evaluation_4.Clear();

            GenSample();
            DrawSample();
            CalculateStatistic();
            CalculateAdditionalStatistic();
            TextOfLabel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            medium.Clear();
            median.Clear();
            dispersion.Clear();
            beta3.Clear();
            beta4.Clear();
            trimmed_mean_1.Clear();
            trimmed_mean_2.Clear();
            trimmed_mean_3.Clear();
            evaluation_1.Clear();
            evaluation_2.Clear();
            evaluation_3.Clear();
            evaluation_4.Clear();

            GenSample();
            DrawSample();
            CalculateStatistic();
            CalculateAdditionalStatistic();
            TextOfLabel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            this.chart4.Series[0].Points.Clear();
            medium.Clear();
            median.Clear();
            dispersion.Clear();
            beta3.Clear();
            beta4.Clear();
            trimmed_mean_1.Clear();
            trimmed_mean_2.Clear();
            trimmed_mean_3.Clear();
            evaluation_1.Clear();
            evaluation_2.Clear();
            evaluation_3.Clear();
            evaluation_4.Clear();

            SampleSize = N;
            for (int i = 0; i < M; i++)
            {
                //this.comboBox2.SelectedIndex = 1;
                //CloggLevel = (double)r.Next(10, 40) / 100.0;
                GenSample();
                //this.comboBox2.SelectedIndex = 4;
                CalculateStatistic();
                CalculateAdditionalStatistic();
            }
            TextOfLabel();
        }
    }

    public static class NthExtensions
    {
        public static double NthItem(this IEnumerable<double> coll, int n)
        {
            return coll.OrderBy(x => x).Skip(n - 1).First();
        }
    }
}
