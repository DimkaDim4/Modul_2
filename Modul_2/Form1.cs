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
            this.clogg_expectedvalue = 0.0;
            this.clogg_dispersion = 1.0;
            this.clogg_scale = 1.0;
            this.clogg_level = 0.1;
            this.sample_size = 100;
            this.percent_emission = 5.0;
            this.alpha = 0.1;
            this.delta = 0.1;
            this.gamma = chart1.DataManipulator.Statistics.GammaFunction(1.0 / this.gauss_nu);
            this.M = 100;
            this.Nmax = 200;
            this.Nmin = 100;
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
            this.textBox10.DataBindings.Add("Text", this, "Alpha");
            this.textBox11.DataBindings.Add("Text", this, "Delta");
            this.textBox12.DataBindings.Add("Text", this, "M");
            this.textBox13.DataBindings.Add("Text", this, "Nmin");
            this.textBox14.DataBindings.Add("Text", this, "Nmax");

            this.label15.Text = "";
            this.label20.Text = "";
            this.label21.Text = "";
            this.label22.Text = "";
            this.label23.Text = "";
            this.label27.Text = "";
            this.label28.Text = "";
            this.label29.Text = "";
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
        private double alpha;
        private double delta;
        private double gamma;
        private double const1;

        private List<double> sample = new List<double>();
        private List<double> medium = new List<double>();
        private List<double> median = new List<double>();
        private List<double> dispersion = new List<double>();
        private List<double> beta3 = new List<double>();
        private List<double> beta4 = new List<double>();
        private List<double> trimmed_mean = new List<double>();
        private List<double> evaluation_1 = new List<double>();
        private List<double> evaluation_2 = new List<double>();

        private int m;
        private int n_min;
        private int n_max;


        public int M
        {
            get { return m; }
            set
            {
                if (value >= 100)
                    m = value;
            }
        }
        
        public int Nmin
        {
            get { return n_min; }
            set
            {
                if ((value <= n_max) && (value >= 100))
                    n_min = value;
            }
        }

        public int Nmax
        {
            get { return n_max; }
            set
            {
                if ((value >= n_min) && (value >= 100))
                    n_max = value;
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
        public double Alpha
        {
            get { return alpha; }
            set 
            {
                if ((value > 0) && (value < 0.5))
                    alpha = value; 
            }
        }
        public double Delta
        {
            get { return delta; }
            set
            {
                if ((value > 0) && (value <= 1.0))
                {
                    delta = value;
                    const1 = -1.0 / Math.Pow(GaussFunction(0), Delta);
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

        private double RhoRadical(double y, double teta)
        {
            return const1 * Math.Pow(GaussFunction((y - teta) / GaussScale), delta);
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

        private double FunctionalRadical(double teta)
        {
            double result = 0.0;
            foreach (double y in sample)
            {
                result += RhoRadical(y, teta);
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

                        this.panel4.Enabled = false;
                        this.panel4.Visible = false;

                        this.panel5.Enabled = false;
                        this.panel5.Visible = false;

                        this.label12.Visible = true;
                        this.textBox8.Enabled = true;
                        this.textBox8.Visible = true;

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
            if ((int.TryParse(this.textBox13.Text, out var)) && (var >= 100) && (var <= n_max))
            {
                textBox13.BackColor = Color.White;
            }
            else
            {
                textBox13.BackColor = Color.Red;
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            int var;
            if ((int.TryParse(this.textBox14.Text, out var)) && (var >= n_min) && (var >= 100))
            {
                textBox14.BackColor = Color.White;
            }
            else
            {
                textBox14.BackColor = Color.Red;
            }
        }

        private void CalculateAdditionalStatistic()
        {
            double trimmed_mean = 0.0;
            int k = (int)(sample_size * alpha);
            sample.Sort();

            for (int i = k; i < sample_size - k; i++)
            {
                trimmed_mean += sample[i];
            }
            trimmed_mean /= (sample_size - 2 * k);

            Func<double[], double> func_1 = x => FunctionalRadical(x[0]);
            Func<double[], double> func_2 = x => FunctionalMaximal(x[0]);

            Cobyla cobyla_1 = new Cobyla(1, func_1);
            Cobyla cobyla_2 = new Cobyla(1, func_2);

            bool success_1 = cobyla_1.Minimize();
            bool success_2 = cobyla_2.Minimize();

            double minimum_1 = cobyla_1.Value;
            double minimum_2 = cobyla_2.Value;

            double[] solution_1 = cobyla_1.Solution;
            double[] solution_2 = cobyla_2.Solution;

            this.trimmed_mean.Add(trimmed_mean);
            this.evaluation_1.Add(solution_1[0]);
            this.evaluation_2.Add(solution_2[0]);
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
            if ((this.comboBox2.SelectedIndex == 3) || (this.comboBox2.SelectedIndex == 4))
            {
                double trimmed_mean = 0.0;
                double evaluation_1 = 0.0;
                double evaluation_2 = 0.0;

                for (int i = 0; i < this.trimmed_mean.Count; i++)
                {
                    trimmed_mean += this.trimmed_mean[i];
                    evaluation_1 += this.evaluation_1[i];
                    evaluation_2 += this.evaluation_2[i];
                }

                trimmed_mean /= this.trimmed_mean.Count;
                evaluation_1 /= this.evaluation_1.Count;
                evaluation_2 /= this.evaluation_2.Count;

                label27.Text = trimmed_mean.ToString();
                label28.Text = evaluation_1.ToString();
                label29.Text = evaluation_2.ToString();
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

            label15.Text = medium.ToString();
            label20.Text = median.ToString();
            label21.Text = dispersion.ToString();
            label22.Text = beta3.ToString();
            label23.Text = beta4.ToString();
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
            trimmed_mean.Clear();
            evaluation_1.Clear();
            evaluation_2.Clear();

            GenSample();
            DrawSample();
            CalculateStatistic();
            TextOfLabel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            medium.Clear();
            median.Clear();
            dispersion.Clear();
            beta3.Clear();
            beta4.Clear();
            trimmed_mean.Clear();
            evaluation_1.Clear();
            evaluation_2.Clear();

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
            trimmed_mean.Clear();
            evaluation_1.Clear();
            evaluation_2.Clear();

            for (int i = 0; i < M; i++)
            {
                SampleSize = r.Next(Nmin, Nmax);
                CloggLevel = (double)r.Next(5, 40) / 100.0;
                GenSample();
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
