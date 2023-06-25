using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValheimLauncher
{
    public partial class choosePath : Form
    {
        
        public choosePath()
        {
            InitializeComponent();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            //Select path
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select your path";
            if (fbd.ShowDialog() == DialogResult.OK)
                textBox1.Text = fbd.SelectedPath;
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            //Check file exist
            if (!File.Exists(@"D:\path.txt")) 
            {
                File.CreateText(@"D:\path.txt").Close();
                using (StreamWriter sw = File.AppendText(@"D:\path.txt"))
                {
                    sw.WriteLine(textBox1.Text);
                    MessageBox.Show("Save done");
                }
            }
            else 
            {
                File.Delete(@"D:\path.txt");
                File.CreateText(@"D:\path.txt").Close();
                using (StreamWriter sw = File.AppendText(@"D:\path.txt"))
                {
                    sw.WriteLine(textBox1.Text);
                    MessageBox.Show("Save done");
                }
            }

            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            

            
        }

        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
