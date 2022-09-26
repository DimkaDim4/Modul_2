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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            this.sample_size = 100;

            this.textBox1.DataBindings.Add("Text", this, "SampleSize");
        }

        private int sample_size;

        public int SampleSize
        {
            get { return sample_size; }
            set
            {
                if (value > 0)
                this.sample_size = value; 
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double var;
            if ((double.TryParse(this.textBox1.Text, out var)) && (var > 0))
            {
                textBox1.BackColor = Color.White;
            }
            else
            {
                textBox1.BackColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
