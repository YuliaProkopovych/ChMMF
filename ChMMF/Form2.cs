using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChMMF
{
    public partial class Form2 : Form
    {
        Form1 form;
        public Form2()
        {
            InitializeComponent();
            form = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form != null)
            {
                //form.Close();
                
            }
            form = new Form1();
            form.Show();
            form.buttonClick(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
        }

    }
}
