using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace OilBots
{
    public partial class Form1 : Form
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Form1 myform = new Form1();
            //myform.Show();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please pick a number of Robots!");
            }
            else
            {
                EcoSystem myEcoSystem = new EcoSystem();
                myEcoSystem.Show();
                //Application.Run(myEcoSystem);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                try
                {
                    int numBots = int.Parse(textBox1.Text);
                    numBots = Math.Abs(numBots);
                    Swarm.updateBots(numBots);
                }
                catch
                {
                    MessageBox.Show("Please enter a positive integer!");
                }
            }
        }


    }
}
