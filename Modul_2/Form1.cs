using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            this.comboBox2.SelectedIndex = 0;

            this.PaintDistribution();
            this.PaintClogg();
            this.PaintClogging();

            this.textBox1.DataBindings.Add("Text", this, "GaussShiftParameter");
            this.textBox2.DataBindings.Add("Text", this, "GaussScale");
            this.textBox3.DataBindings.Add("Text", this, "GaussNu");

            this.textBox4.DataBindings.Add("Text", this, "CloggExpectedValue");
            this.textBox5.DataBindings.Add("Text", this, "CloggDispersion");
            this.textBox7.DataBindings.Add("Text", this, "CloggScale");

            this.textBox6.DataBindings.Add("Text", this, "CloggLevel");

            this.textBox8.DataBindings.Add("Text", this, "SampleSize");
            this.textBox9.DataBindings.Add("Text", this, "PercentEmission");
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
            double g = chart1.DataManipulator.Statistics.GammaFunction(1.0 / this.gauss_nu);
            double a = this.GaussShiftParameter - this.GaussScale * 5.0;
            double b = this.GaussShiftParameter + this.GaussScale * 5.0;
            double h = (b - a) / 200.0;
            double x, y;

            this.chart1.Series[0].Points.Clear();
            x = a;
            while (x <= b)
            {
                y = gauss_nu / (2.0 * g) * Math.Exp(- Math.Pow(Math.Abs(x - GaussShiftParameter), gauss_nu)) * this.gauss_scale;
                this.chart1.Series[0].Points.AddXY(x, y);
                x += h;
            }
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
            if (this.comboBox2.SelectedIndex == 0)
            {
                this.panel1.Enabled = false;
                this.panel1.Visible = false;

                this.panel2.Enabled = false;
                this.panel2.Visible = false;

                this.panel3.Enabled = false;
                this.panel3.Visible = false;
            }
            else if (this.comboBox2.SelectedIndex == 2)
            {
                this.panel2.Enabled = false;
                this.panel2.Visible = false;

                this.panel1.Enabled = true;
                this.panel1.Visible = true;
            }
            else
            {
                this.panel1.Enabled = false;
                this.panel1.Visible = false;

                this.panel2.Enabled = true;
                this.panel2.Visible = true;
            }

            if (this.comboBox2.SelectedIndex != 0)
            {
                this.panel3.Enabled = true;
                this.panel3.Visible = true;
            }

            if (this.comboBox2.SelectedIndex == 3)
            {
                this.panel4.Enabled = true;
                this.panel4.Visible = true;
            }
            else
            {
                this.panel4.Enabled = false;
                this.panel4.Visible = false;
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

        private void GenSample()
        {
            Random r = new Random();
            double medium = 0.0;
            int count = 0;

            this.chart4.Series[0].Points.Clear();
            if (this.comboBox2.SelectedIndex == 1)
            {
                double a = 1.0 / gauss_nu - 0.5;
                double b = 1.0 / Math.Pow(gauss_nu, 1.0 / gauss_nu);
                double c = 2.0 * b * b;

                do
                {
                    double x = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Cos(2.0 * Math.PI * r.NextDouble()) * b;

                    if (Math.Log(r.NextDouble() * b) <= -Math.Pow(Math.Abs(x), gauss_nu) + x * x / c + a)
                    {
                        count++;
                        x = x * gauss_scale + gauss_shiftparameter;
                        medium += x;

                        this.chart4.Series[0].Points.AddXY(count, x);
                    }
                } while (count < sample_size);
            }
            else if (this.comboBox2.SelectedIndex == 2)
            {
                double a = 1.0 / gauss_nu - 0.5;
                double b = 1.0 / Math.Pow(gauss_nu, 1.0 / gauss_nu);
                double c = 2.0 * b * b;

                do
                {
                    double x = Math.Sqrt(-2.0 * Math.Log(r.NextDouble())) * Math.Cos(2.0 * Math.PI * r.NextDouble()) * b;

                    if (Math.Log(r.NextDouble() * b) <= -Math.Pow(Math.Abs(x), gauss_nu) + x * x / c + a)
                    {
                        count++;
                        if (r.NextDouble() < percent_emission / 100.0) x *= 4;
                        x = x * gauss_scale + gauss_shiftparameter;
                        medium += x;

                        this.chart4.Series[0].Points.AddXY(count, x);
                    }
                } while (count < sample_size);
            }
            else if (this.comboBox2.SelectedIndex == 3)
            {

            }
            medium /= sample_size;
            label15.Text = medium.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenSample();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenSample();
        }
    }
}
