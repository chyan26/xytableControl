using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XY_Table
{
    public partial class MainMenu : Form
    {
        public mainform MainForm;

        [STAThread]
        static void Main()
        {
            Application.Run(new MainMenu());
        }

        public MainMenu()
        {
            InitializeComponent();
            MainForm = new mainform();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.tabControl1.SelectedIndex = 0; 
            MainForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.tabControl1.SelectedIndex = 1;
            MainForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm.tabControl1.SelectedIndex = 2;
            MainForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.Ch.KillAll();
                MainForm.Ch.CloseComm();
                MainForm.Camera0.Exit();
                MainForm.Camera1.Exit();
                MainForm.Camera2.Exit();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            MainForm.Close();
            this.Close();
        }
    }
}
