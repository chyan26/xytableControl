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
    public partial class MainMenu : System.Windows.Forms.Form
    {
        mainform MainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.GoManualMode();
            MainForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.GoAutoMode();
            MainForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm.GoCalibrateMode();
            MainForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
