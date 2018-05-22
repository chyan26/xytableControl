using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices; //for COMException class
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using nom.tam.image;
using nom.tam.fits;
using nom.tam.util;


namespace XY_Table
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    /// 

    public class mainform : System.Windows.Forms.Form
    {
        /// <summary>
        /// 
        //Motor===========
        public struct sCONTINUE_MOVE
        {
            public double x;
            public double y;
            public double vel;
        };

        //Error Table===========
        public struct POINT_ERROR
        {
            public double x;
            public double y;
            public double _x;
            public double _y;
        };

        SortedSet<POINT_ERROR> error_table1;     
        
        public POINT_ERROR[] error_table = new POINT_ERROR[16384];
        //Error Table===========

        public SPIIPLUSCOM660Lib.Channel Ch;
        private int ComTypeOld;
        int Axis;
        System.Drawing.Image Grey;
        System.Drawing.Image Green;
        bool bConnected;
        private Thread MotorStateThr;
        public int[] Axes = new int[3];
        public int[] MotorState = new int[2];
        public int[] LastMotorState = new int[2];
        public int[] MotorFault = new int[2];
        public int[] LastMotorFault = new int[2];
        public double[] Current_Position = new double[2];
        public int number_of_points = -1;
        //public double[] x_array = new double[16384];
        //public double[] y_array = new double[16384];
        //public double[] vel_array = new double[16384];
        //public byte[] raw_data = new byte[1310720];


        public sCONTINUE_MOVE[] continue_move_array = new sCONTINUE_MOVE[16384];
        public const double MinimumSpeed = 0.5;
        public const double MaximumSpeed = 200.0;
        bool continue_move = false;
        int continue_move_counter = 0;
        int stop_time = 0;
        int repeat_times = 0;

        public sCONTINUE_MOVE[] calibration_move_array = new sCONTINUE_MOVE[16384];
        bool calibration_move = false;
        int  calibration_move_counter = 0;
        StreamWriter sw;
        //============
        private PictureBox GreenPB;
        private PictureBox GreyPB;
        public static BitArray DI, LastDI;
        public static BitArray DO, LastDO;
        //Camera==========
        public uEye.Camera Camera0, Camera1, Camera2;
        public uEye.Camera CameraC, CameraF, CameraA;
        //Safety control Message==========
        public string[] fault_table =
        {
            "Right Limit",
            "Left Limit",
            "Network Error",
            "Motor Overheat",
            "Software Right Limit",
            "Software Left Limit",
            "Primary Encoder Not Connected",
            "Secondary Encoder Not Connected",
            "Driver Alarm",
            "Primary Encoder Error",
            "Secondary Encoder Error",
            "Position Error",
            "Critical Position Error",
            "Velocity Limit",
            "Acceleration Limit",
            "Current Limit",
            "Servo Processor Alarm",
            "Program Error",
            "Memory Overuse",
            "Time Overuse",
            "Emergency Stop",
            "Servo Interrupt",
            "Integrity Violation",
        };

        //Safety control Message==========

        public TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private IContainer components;
        private Button button1;
        private GroupBox groupBox2;
        private TextBox textBox4;
        private Label label20;
        private Button button3;
        private TextBox textBox6;
        private Label label21;
        private TextBox textBox7;
        private Label label22;
        private GroupBox groupBox3;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private Label label23;
        private TextBox textBox5;
        private Label label18;
        private GroupBox groupBox1;
        private Label label17;
        private TextBox textBox3;
        private Label label16;
        private Button button2;
        private TextBox textBox2;
        private Label label15;
        private TextBox textBox1;
        private Label label14;

        private Button button8;
        private Button button9;
        private Button CM_Load;
        private OpenFileDialog openFileDialog1;
        private Label label25;
        private Button CM_Start;
        private Button button12;

        private Button button13;
        private DataGridView dataGridView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Button button14;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Label label2;
        private Label label1;
        private PictureBox pictureBox9;
        private PictureBox pictureBox10;
        private PictureBox pictureBox11;
        private PictureBox pictureBox12;
        private PictureBox pictureBox13;
        private PictureBox pictureBox14;
        private PictureBox pictureBox15;
        private PictureBox pictureBox16;
        private PictureBox pictureBox8;
        private PictureBox pictureBox7;
        private PictureBox pictureBox6;
        private PictureBox pictureBox5;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private Label label26;
        private Label label24;
        private Label label32;
        private Label label31;
        private Label label30;
        private Label label29;
        private Label label28;
        private Label label27;
        private Label label33;
        private Label label34;
        private Label label35;
        private Label label36;
        private SaveFileDialog saveFileDialog1;
        private Button CM_Stop;
        private Button CM_Restart;
        private Label label37;
        private CheckBox CM_AutoCapture;
        private TabPage tabPage5;
        private Label label55;
        private Label label54;
        private Label label53;
        private Label label52;
        private Label label51;
        private Label label50;
        private Label label49;
        private Label label48;
        private Label label47;
        private Label label46;
        private Label label45;
        private Label label44;
        private Label label43;
        private Label label42;
        private Label label41;
        private Label label40;
        private Label label39;
        private Label label38;
        private PictureBox pictureBox34;
        private PictureBox pictureBox35;
        private PictureBox pictureBox36;
        private PictureBox pictureBox37;
        private PictureBox pictureBox38;
        private PictureBox pictureBox39;
        private PictureBox pictureBox40;
        private PictureBox pictureBox41;
        private PictureBox pictureBox42;
        private PictureBox pictureBox43;
        private PictureBox pictureBox44;
        private PictureBox pictureBox45;
        private PictureBox pictureBox46;
        private PictureBox pictureBox47;
        private PictureBox pictureBox48;
        private PictureBox pictureBox49;
        private PictureBox pictureBox26;
        private PictureBox pictureBox27;
        private PictureBox pictureBox28;
        private PictureBox pictureBox29;
        private PictureBox pictureBox30;
        private PictureBox pictureBox31;
        private PictureBox pictureBox32;
        private PictureBox pictureBox33;
        private PictureBox pictureBox18;
        private PictureBox pictureBox19;
        private PictureBox pictureBox20;
        private PictureBox pictureBox21;
        private PictureBox pictureBox22;
        private PictureBox pictureBox23;
        private PictureBox pictureBox24;
        private PictureBox pictureBox25;
        private Label label56;
        private Label label57;
        private Label label58;
        private Label label59;
        private Label label60;
        private Label label61;
        private Label label62;
        private Label label63;
        private Label label64;
        private Label label65;
        private Label label66;
        private Label label67;
        private Label label68;
        private Label label69;
        private Label label70;
        private Label label71;
        private Label label72;
        private PictureBox pictureBox56;
        private PictureBox pictureBox55;
        private PictureBox pictureBox54;
        private PictureBox pictureBox53;
        private PictureBox pictureBox52;
        private PictureBox pictureBox51;
        private PictureBox pictureBox50;
        private Label label77;
        private Label label78;
        private Label label79;
        private Label label73;
        private Label label74;
        private Label label75;
        private Label label76;
        private TabPage tabPage6;
        private PictureBox pictureBox58;
        private PictureBox pictureBox57;
        private Button button10;
        private Button CaliM_Start;
        private Label label83;
        private Label label82;
        private TextBox textBox9;
        private Label label81;
        private TextBox textBox8;
        private Label label80;
        private CheckBox checkBox1;
        private Button CaliM_Restart;
        private Button CaliM_Stop;
        private Label label84;
        private CheckBox CaliM_AutoCapture;
        private Label label85;
        private Button button11;
        private SaveFileDialog saveFileDialog2;
        private TrackBar ExposureBar0;
        private Label label19;
        private Label label87;
        private TrackBar ExposureBar2;
        private Label label86;
        private TrackBar ExposureBar1;
        private Label label90;
        private Label label89;
        private Label label88;
        private Button button15;
        private Label label93;
        private Label label94;
        private Label label91;
        private Label label92;
        private CheckBox AutoExposure0;
        private CheckBox AutoExposure2;
        private CheckBox AutoExposure1;
        private System.Windows.Forms.Timer timer1;
        private Button button16;
        private Button button17;
        private Button button18;
        private Label label95;
        private Label label96;
        private Label label97;
        private Label label98;
        private Label label99;
        private Label label100;
        private DataGridViewTextBoxColumn Col0;
        private DataGridViewTextBoxColumn Col1;
        private DataGridViewTextBoxColumn Col2;
        private DataGridViewTextBoxColumn Col3;
        private Label label106;
        private Label label105;
        private TextBox textBox10;
        private Label label103;
        private TextBox textBox11;
        private Label label104;
        private Label label101;
        private Label label102;
        private PictureBox pictureBox17;


        //================================================================
        public mainform()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            //InitCamera();

            DI = new BitArray(8);
            DO = new BitArray(8);
            LastDI = new BitArray(8);
            LastDO = new BitArray(8);

            for (int i = 0; i < 8; i++)
            {
                DI[i] = false;
                DO[i] = false;
                LastDI[i] = false;
                LastDO[i] = false;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainform));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.GreenPB = new System.Windows.Forms.PictureBox();
            this.GreyPB = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button15 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.CM_AutoCapture = new System.Windows.Forms.CheckBox();
            this.label37 = new System.Windows.Forms.Label();
            this.CM_Restart = new System.Windows.Forms.Button();
            this.CM_Stop = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CM_Start = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.CM_Load = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label95 = new System.Windows.Forms.Label();
            this.label96 = new System.Windows.Forms.Label();
            this.button16 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.label85 = new System.Windows.Forms.Label();
            this.CaliM_AutoCapture = new System.Windows.Forms.CheckBox();
            this.label84 = new System.Windows.Forms.Label();
            this.CaliM_Restart = new System.Windows.Forms.Button();
            this.CaliM_Stop = new System.Windows.Forms.Button();
            this.CaliM_Start = new System.Windows.Forms.Button();
            this.label83 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label81 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label80 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.pictureBox16 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label77 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.pictureBox56 = new System.Windows.Forms.PictureBox();
            this.pictureBox55 = new System.Windows.Forms.PictureBox();
            this.pictureBox54 = new System.Windows.Forms.PictureBox();
            this.pictureBox53 = new System.Windows.Forms.PictureBox();
            this.pictureBox52 = new System.Windows.Forms.PictureBox();
            this.pictureBox51 = new System.Windows.Forms.PictureBox();
            this.pictureBox50 = new System.Windows.Forms.PictureBox();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.pictureBox34 = new System.Windows.Forms.PictureBox();
            this.pictureBox35 = new System.Windows.Forms.PictureBox();
            this.pictureBox36 = new System.Windows.Forms.PictureBox();
            this.pictureBox37 = new System.Windows.Forms.PictureBox();
            this.pictureBox38 = new System.Windows.Forms.PictureBox();
            this.pictureBox39 = new System.Windows.Forms.PictureBox();
            this.pictureBox40 = new System.Windows.Forms.PictureBox();
            this.pictureBox41 = new System.Windows.Forms.PictureBox();
            this.pictureBox42 = new System.Windows.Forms.PictureBox();
            this.pictureBox43 = new System.Windows.Forms.PictureBox();
            this.pictureBox44 = new System.Windows.Forms.PictureBox();
            this.pictureBox45 = new System.Windows.Forms.PictureBox();
            this.pictureBox46 = new System.Windows.Forms.PictureBox();
            this.pictureBox47 = new System.Windows.Forms.PictureBox();
            this.pictureBox48 = new System.Windows.Forms.PictureBox();
            this.pictureBox49 = new System.Windows.Forms.PictureBox();
            this.pictureBox26 = new System.Windows.Forms.PictureBox();
            this.pictureBox27 = new System.Windows.Forms.PictureBox();
            this.pictureBox28 = new System.Windows.Forms.PictureBox();
            this.pictureBox29 = new System.Windows.Forms.PictureBox();
            this.pictureBox30 = new System.Windows.Forms.PictureBox();
            this.pictureBox31 = new System.Windows.Forms.PictureBox();
            this.pictureBox32 = new System.Windows.Forms.PictureBox();
            this.pictureBox33 = new System.Windows.Forms.PictureBox();
            this.pictureBox18 = new System.Windows.Forms.PictureBox();
            this.pictureBox19 = new System.Windows.Forms.PictureBox();
            this.pictureBox20 = new System.Windows.Forms.PictureBox();
            this.pictureBox21 = new System.Windows.Forms.PictureBox();
            this.pictureBox22 = new System.Windows.Forms.PictureBox();
            this.pictureBox23 = new System.Windows.Forms.PictureBox();
            this.pictureBox24 = new System.Windows.Forms.PictureBox();
            this.pictureBox25 = new System.Windows.Forms.PictureBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.AutoExposure2 = new System.Windows.Forms.CheckBox();
            this.AutoExposure1 = new System.Windows.Forms.CheckBox();
            this.AutoExposure0 = new System.Windows.Forms.CheckBox();
            this.label93 = new System.Windows.Forms.Label();
            this.label94 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.ExposureBar2 = new System.Windows.Forms.TrackBar();
            this.label86 = new System.Windows.Forms.Label();
            this.ExposureBar1 = new System.Windows.Forms.TrackBar();
            this.label19 = new System.Windows.Forms.Label();
            this.ExposureBar0 = new System.Windows.Forms.TrackBar();
            this.pictureBox58 = new System.Windows.Forms.PictureBox();
            this.pictureBox57 = new System.Windows.Forms.PictureBox();
            this.pictureBox17 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button14 = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.label97 = new System.Windows.Forms.Label();
            this.label98 = new System.Windows.Forms.Label();
            this.label99 = new System.Windows.Forms.Label();
            this.label100 = new System.Windows.Forms.Label();
            this.Col0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label101 = new System.Windows.Forms.Label();
            this.label102 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label103 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label104 = new System.Windows.Forms.Label();
            this.label105 = new System.Windows.Forms.Label();
            this.label106 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GreenPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreyPB)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox56)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox55)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox54)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox53)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox52)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox51)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox50)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox35)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox36)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox37)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox39)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox40)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox41)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox42)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox45)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox46)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox47)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox48)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox49)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox29)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox30)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox25)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBar0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox58)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox57)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // GreenPB
            // 
            this.GreenPB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.GreenPB.Image = ((System.Drawing.Image)(resources.GetObject("GreenPB.Image")));
            this.GreenPB.Location = new System.Drawing.Point(906, 12);
            this.GreenPB.Name = "GreenPB";
            this.GreenPB.Size = new System.Drawing.Size(24, 27);
            this.GreenPB.TabIndex = 24;
            this.GreenPB.TabStop = false;
            this.GreenPB.Visible = false;
            // 
            // GreyPB
            // 
            this.GreyPB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.GreyPB.Image = ((System.Drawing.Image)(resources.GetObject("GreyPB.Image")));
            this.GreyPB.Location = new System.Drawing.Point(871, 12);
            this.GreyPB.Name = "GreyPB";
            this.GreyPB.Size = new System.Drawing.Size(24, 27);
            this.GreyPB.TabIndex = 1;
            this.GreyPB.TabStop = false;
            this.GreyPB.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.tabControl1.ItemSize = new System.Drawing.Size(96, 32);
            this.tabControl1.Location = new System.Drawing.Point(3, 6);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(640, 831);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button15);
            this.tabPage1.Controls.Add(this.button10);
            this.tabPage1.Controls.Add(this.button9);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 36);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(632, 791);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Manual Mode";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.Location = new System.Drawing.Point(504, 441);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(98, 82);
            this.button15.TabIndex = 46;
            this.button15.Text = "Park";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click_4);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(296, 441);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(98, 82);
            this.button10.TabIndex = 45;
            this.button10.Text = "Stop";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click_1);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(400, 441);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(98, 82);
            this.button9.TabIndex = 4;
            this.button9.Text = "Home Move";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(236, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 229);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Move A Distance";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(76, 119);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(60, 26);
            this.textBox4.TabIndex = 25;
            this.textBox4.Text = "100";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 122);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 20);
            this.label20.TabIndex = 26;
            this.label20.Text = "Speed :";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(76, 166);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 45);
            this.button3.TabIndex = 24;
            this.button3.Text = "Move";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(76, 36);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(60, 26);
            this.textBox6.TabIndex = 22;
            this.textBox6.Text = "100";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(42, 40);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(28, 20);
            this.label21.TabIndex = 23;
            this.label21.Text = "X :";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(76, 77);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(60, 26);
            this.textBox7.TabIndex = 20;
            this.textBox7.Text = "100";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(42, 80);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(28, 20);
            this.label22.TabIndex = 21;
            this.label22.Text = "Y :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(13, 257);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(254, 266);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Free Move";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(166, 69);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(78, 74);
            this.button7.TabIndex = 32;
            this.button7.Tag = "2";
            this.button7.Text = "AxisX+";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button7_MouseDown);
            this.button7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button7_MouseUp);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(10, 69);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(71, 74);
            this.button6.TabIndex = 31;
            this.button6.Tag = "3";
            this.button6.Text = "AxisX-";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button6_MouseDown);
            this.button6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button6_MouseUp);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(87, 110);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(73, 73);
            this.button5.TabIndex = 30;
            this.button5.Tag = "1";
            this.button5.Text = "AxisY-";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button5_MouseDown);
            this.button5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button5_MouseUp);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(87, 29);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(73, 74);
            this.button4.TabIndex = 29;
            this.button4.Tag = "0";
            this.button4.Text = "AxisY+";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button4_MouseDown);
            this.button4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button4_MouseUp);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(153, 205);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(47, 20);
            this.label23.TabIndex = 28;
            this.label23.Text = "mm/s";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(87, 205);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(60, 26);
            this.textBox5.TabIndex = 22;
            this.textBox5.Text = "20";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 209);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 20);
            this.label18.TabIndex = 23;
            this.label18.Text = "Speed :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 229);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Move To Position";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(142, 122);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 20);
            this.label17.TabIndex = 27;
            this.label17.Text = "mm/s";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(76, 119);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(60, 26);
            this.textBox3.TabIndex = 25;
            this.textBox3.Text = "100";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 122);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 20);
            this.label16.TabIndex = 26;
            this.label16.Text = "Speed :";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(76, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 45);
            this.button2.TabIndex = 24;
            this.button2.Text = "Move";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(66, 36);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(60, 26);
            this.textBox2.TabIndex = 22;
            this.textBox2.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(32, 40);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 20);
            this.label15.TabIndex = 23;
            this.label15.Text = "X :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(66, 77);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 26);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(32, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 20);
            this.label14.TabIndex = 21;
            this.label14.Text = "Y :";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label106);
            this.tabPage2.Controls.Add(this.label105);
            this.tabPage2.Controls.Add(this.textBox10);
            this.tabPage2.Controls.Add(this.label103);
            this.tabPage2.Controls.Add(this.textBox11);
            this.tabPage2.Controls.Add(this.label104);
            this.tabPage2.Controls.Add(this.label101);
            this.tabPage2.Controls.Add(this.label102);
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.CM_AutoCapture);
            this.tabPage2.Controls.Add(this.label37);
            this.tabPage2.Controls.Add(this.CM_Restart);
            this.tabPage2.Controls.Add(this.CM_Stop);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Controls.Add(this.CM_Start);
            this.tabPage2.Controls.Add(this.label25);
            this.tabPage2.Controls.Add(this.CM_Load);
            this.tabPage2.Location = new System.Drawing.Point(4, 36);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(632, 791);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Auto Mode";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 597);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(119, 24);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Send Trigger";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // CM_AutoCapture
            // 
            this.CM_AutoCapture.AutoSize = true;
            this.CM_AutoCapture.Location = new System.Drawing.Point(6, 562);
            this.CM_AutoCapture.Name = "CM_AutoCapture";
            this.CM_AutoCapture.Size = new System.Drawing.Size(123, 24);
            this.CM_AutoCapture.TabIndex = 7;
            this.CM_AutoCapture.Text = "Auto Capture";
            this.CM_AutoCapture.UseVisualStyleBackColor = true;
            this.CM_AutoCapture.CheckedChanged += new System.EventHandler(this.CM_AutoCapture_CheckedChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(187, 636);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(119, 20);
            this.label37.TabIndex = 6;
            this.label37.Text = "Current Point: 0";
            this.label37.Click += new System.EventHandler(this.label37_Click);
            // 
            // CM_Restart
            // 
            this.CM_Restart.Enabled = false;
            this.CM_Restart.Location = new System.Drawing.Point(317, 466);
            this.CM_Restart.Name = "CM_Restart";
            this.CM_Restart.Size = new System.Drawing.Size(98, 72);
            this.CM_Restart.TabIndex = 5;
            this.CM_Restart.Text = "Restart";
            this.CM_Restart.UseVisualStyleBackColor = true;
            this.CM_Restart.Click += new System.EventHandler(this.button16_Click);
            // 
            // CM_Stop
            // 
            this.CM_Stop.Enabled = false;
            this.CM_Stop.Location = new System.Drawing.Point(213, 466);
            this.CM_Stop.Name = "CM_Stop";
            this.CM_Stop.Size = new System.Drawing.Size(98, 72);
            this.CM_Stop.TabIndex = 4;
            this.CM_Stop.Text = "Stop Move";
            this.CM_Stop.UseVisualStyleBackColor = true;
            this.CM_Stop.Click += new System.EventHandler(this.button15_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col0,
            this.Col1,
            this.Col2,
            this.Col3});
            this.dataGridView1.Location = new System.Drawing.Point(6, 7);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 32;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(353, 452);
            this.dataGridView1.TabIndex = 3;
            // 
            // CM_Start
            // 
            this.CM_Start.Enabled = false;
            this.CM_Start.Location = new System.Drawing.Point(110, 466);
            this.CM_Start.Name = "CM_Start";
            this.CM_Start.Size = new System.Drawing.Size(98, 72);
            this.CM_Start.TabIndex = 2;
            this.CM_Start.Text = "Start Move";
            this.CM_Start.UseVisualStyleBackColor = true;
            this.CM_Start.Click += new System.EventHandler(this.button11_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(187, 595);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(105, 20);
            this.label25.TabIndex = 1;
            this.label25.Text = "郎Α岿粇";
            this.label25.Visible = false;
            // 
            // CM_Load
            // 
            this.CM_Load.Location = new System.Drawing.Point(6, 466);
            this.CM_Load.Name = "CM_Load";
            this.CM_Load.Size = new System.Drawing.Size(98, 72);
            this.CM_Load.TabIndex = 0;
            this.CM_Load.Text = "Load Profile";
            this.CM_Load.UseVisualStyleBackColor = true;
            this.CM_Load.Click += new System.EventHandler(this.button10_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label95);
            this.tabPage3.Controls.Add(this.label96);
            this.tabPage3.Controls.Add(this.button16);
            this.tabPage3.Controls.Add(this.button11);
            this.tabPage3.Controls.Add(this.label85);
            this.tabPage3.Controls.Add(this.CaliM_AutoCapture);
            this.tabPage3.Controls.Add(this.label84);
            this.tabPage3.Controls.Add(this.CaliM_Restart);
            this.tabPage3.Controls.Add(this.CaliM_Stop);
            this.tabPage3.Controls.Add(this.CaliM_Start);
            this.tabPage3.Controls.Add(this.label83);
            this.tabPage3.Controls.Add(this.label82);
            this.tabPage3.Controls.Add(this.textBox9);
            this.tabPage3.Controls.Add(this.label81);
            this.tabPage3.Controls.Add(this.textBox8);
            this.tabPage3.Controls.Add(this.label80);
            this.tabPage3.Controls.Add(this.button8);
            this.tabPage3.Location = new System.Drawing.Point(4, 36);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(632, 791);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Calibrate";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.Location = new System.Drawing.Point(82, 253);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(152, 20);
            this.label95.TabIndex = 38;
            this.label95.Text = "Start Time: 00:00:00";
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.Location = new System.Drawing.Point(82, 292);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(151, 20);
            this.label96.TabIndex = 37;
            this.label96.Text = "Stop Time: 00:00:00";
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(460, 414);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(111, 62);
            this.button16.TabIndex = 36;
            this.button16.Text = "Set R Home";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Visible = false;
            this.button16.Click += new System.EventHandler(this.button16_Click_2);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(69, 121);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(98, 72);
            this.button11.TabIndex = 35;
            this.button11.Text = "Generate Points";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click_2);
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(82, 331);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(109, 20);
            this.label85.TabIndex = 34;
            this.label85.Text = "Total Points: 0";
            // 
            // CaliM_AutoCapture
            // 
            this.CaliM_AutoCapture.AutoSize = true;
            this.CaliM_AutoCapture.Location = new System.Drawing.Point(402, 249);
            this.CaliM_AutoCapture.Name = "CaliM_AutoCapture";
            this.CaliM_AutoCapture.Size = new System.Drawing.Size(123, 24);
            this.CaliM_AutoCapture.TabIndex = 33;
            this.CaliM_AutoCapture.Text = "Auto Capture";
            this.CaliM_AutoCapture.UseVisualStyleBackColor = true;
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(82, 370);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(119, 20);
            this.label84.TabIndex = 32;
            this.label84.Text = "Current Point: 0";
            // 
            // CaliM_Restart
            // 
            this.CaliM_Restart.Enabled = false;
            this.CaliM_Restart.Location = new System.Drawing.Point(385, 121);
            this.CaliM_Restart.Name = "CaliM_Restart";
            this.CaliM_Restart.Size = new System.Drawing.Size(98, 72);
            this.CaliM_Restart.TabIndex = 31;
            this.CaliM_Restart.Text = "Restart";
            this.CaliM_Restart.UseVisualStyleBackColor = true;
            this.CaliM_Restart.Click += new System.EventHandler(this.button15_Click_2);
            // 
            // CaliM_Stop
            // 
            this.CaliM_Stop.Enabled = false;
            this.CaliM_Stop.Location = new System.Drawing.Point(281, 121);
            this.CaliM_Stop.Name = "CaliM_Stop";
            this.CaliM_Stop.Size = new System.Drawing.Size(98, 72);
            this.CaliM_Stop.TabIndex = 30;
            this.CaliM_Stop.Text = "Stop Move";
            this.CaliM_Stop.UseVisualStyleBackColor = true;
            this.CaliM_Stop.Click += new System.EventHandler(this.button16_Click_1);
            // 
            // CaliM_Start
            // 
            this.CaliM_Start.Enabled = false;
            this.CaliM_Start.Location = new System.Drawing.Point(177, 121);
            this.CaliM_Start.Name = "CaliM_Start";
            this.CaliM_Start.Size = new System.Drawing.Size(98, 72);
            this.CaliM_Start.TabIndex = 28;
            this.CaliM_Start.Text = "Start Sampling";
            this.CaliM_Start.UseVisualStyleBackColor = true;
            this.CaliM_Start.Click += new System.EventHandler(this.button11_Click_1);
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(173, 67);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(93, 20);
            this.label83.TabIndex = 27;
            this.label83.Text = "( must >= 2)";
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(173, 28);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(93, 20);
            this.label82.TabIndex = 26;
            this.label82.Text = "( must >= 2)";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(107, 63);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(60, 26);
            this.textBox9.TabIndex = 24;
            this.textBox9.Text = "2";
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(14, 67);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(87, 20);
            this.label81.TabIndex = 25;
            this.label81.Text = "Y samples:";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(107, 24);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(60, 26);
            this.textBox8.TabIndex = 22;
            this.textBox8.Text = "2";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(14, 28);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(87, 20);
            this.label80.TabIndex = 23;
            this.label80.Text = "X samples:";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(343, 414);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(111, 62);
            this.button8.TabIndex = 0;
            this.button8.Text = "Set F Home";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label33);
            this.tabPage4.Controls.Add(this.label34);
            this.tabPage4.Controls.Add(this.label32);
            this.tabPage4.Controls.Add(this.label31);
            this.tabPage4.Controls.Add(this.label30);
            this.tabPage4.Controls.Add(this.label29);
            this.tabPage4.Controls.Add(this.label28);
            this.tabPage4.Controls.Add(this.label27);
            this.tabPage4.Controls.Add(this.label26);
            this.tabPage4.Controls.Add(this.label24);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.pictureBox9);
            this.tabPage4.Controls.Add(this.pictureBox10);
            this.tabPage4.Controls.Add(this.pictureBox11);
            this.tabPage4.Controls.Add(this.pictureBox12);
            this.tabPage4.Controls.Add(this.pictureBox13);
            this.tabPage4.Controls.Add(this.pictureBox14);
            this.tabPage4.Controls.Add(this.pictureBox15);
            this.tabPage4.Controls.Add(this.pictureBox16);
            this.tabPage4.Controls.Add(this.pictureBox8);
            this.tabPage4.Controls.Add(this.pictureBox7);
            this.tabPage4.Controls.Add(this.pictureBox6);
            this.tabPage4.Controls.Add(this.pictureBox5);
            this.tabPage4.Controls.Add(this.pictureBox4);
            this.tabPage4.Controls.Add(this.pictureBox3);
            this.tabPage4.Controls.Add(this.pictureBox2);
            this.tabPage4.Controls.Add(this.pictureBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 36);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(632, 791);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "I/O Status";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.OrangeRed;
            this.label33.Location = new System.Drawing.Point(371, 83);
            this.label33.Name = "label33";
            this.label33.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label33.Size = new System.Drawing.Size(68, 28);
            this.label33.TabIndex = 70;
            this.label33.Text = "0.0";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.OrangeRed;
            this.label34.Location = new System.Drawing.Point(371, 168);
            this.label34.Name = "label34";
            this.label34.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label34.Size = new System.Drawing.Size(68, 28);
            this.label34.TabIndex = 69;
            this.label34.Text = "0.0";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Lime;
            this.label32.Location = new System.Drawing.Point(267, 297);
            this.label32.Name = "label32";
            this.label32.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label32.Size = new System.Drawing.Size(68, 27);
            this.label32.TabIndex = 68;
            this.label32.Text = "0.0";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Lime;
            this.label31.Location = new System.Drawing.Point(267, 254);
            this.label31.Name = "label31";
            this.label31.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label31.Size = new System.Drawing.Size(68, 28);
            this.label31.TabIndex = 67;
            this.label31.Text = "0.0";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Lime;
            this.label30.Location = new System.Drawing.Point(267, 211);
            this.label30.Name = "label30";
            this.label30.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label30.Size = new System.Drawing.Size(68, 28);
            this.label30.TabIndex = 66;
            this.label30.Text = "0.0";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Lime;
            this.label29.Location = new System.Drawing.Point(267, 168);
            this.label29.Name = "label29";
            this.label29.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label29.Size = new System.Drawing.Size(68, 28);
            this.label29.TabIndex = 65;
            this.label29.Text = "0.0";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Lime;
            this.label28.Location = new System.Drawing.Point(267, 126);
            this.label28.Name = "label28";
            this.label28.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label28.Size = new System.Drawing.Size(68, 27);
            this.label28.TabIndex = 64;
            this.label28.Text = "0.0";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Lime;
            this.label27.Location = new System.Drawing.Point(267, 83);
            this.label27.Name = "label27";
            this.label27.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label27.Size = new System.Drawing.Size(68, 28);
            this.label27.TabIndex = 63;
            this.label27.Text = "0.0";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(386, 30);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(32, 20);
            this.label26.TabIndex = 62;
            this.label26.Text = "AO";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(287, 30);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(25, 20);
            this.label24.TabIndex = 61;
            this.label24.Text = "AI";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 20);
            this.label2.TabIndex = 60;
            this.label2.Text = "DO";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 20);
            this.label1.TabIndex = 59;
            this.label1.Text = "DI";
            // 
            // pictureBox9
            // 
            this.pictureBox9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox9.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox9.Image")));
            this.pictureBox9.Location = new System.Drawing.Point(181, 78);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(24, 28);
            this.pictureBox9.TabIndex = 58;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.Click += new System.EventHandler(this.pictureBox9_Click);
            // 
            // pictureBox10
            // 
            this.pictureBox10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox10.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox10.Image")));
            this.pictureBox10.Location = new System.Drawing.Point(181, 121);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(24, 28);
            this.pictureBox10.TabIndex = 57;
            this.pictureBox10.TabStop = false;
            this.pictureBox10.Click += new System.EventHandler(this.pictureBox10_Click);
            // 
            // pictureBox11
            // 
            this.pictureBox11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox11.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox11.Image")));
            this.pictureBox11.Location = new System.Drawing.Point(181, 164);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(24, 28);
            this.pictureBox11.TabIndex = 56;
            this.pictureBox11.TabStop = false;
            this.pictureBox11.Click += new System.EventHandler(this.pictureBox11_Click);
            // 
            // pictureBox12
            // 
            this.pictureBox12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox12.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox12.Image")));
            this.pictureBox12.Location = new System.Drawing.Point(181, 207);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(24, 27);
            this.pictureBox12.TabIndex = 55;
            this.pictureBox12.TabStop = false;
            this.pictureBox12.Click += new System.EventHandler(this.pictureBox12_Click);
            // 
            // pictureBox13
            // 
            this.pictureBox13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox13.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox13.Image")));
            this.pictureBox13.Location = new System.Drawing.Point(181, 249);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(24, 28);
            this.pictureBox13.TabIndex = 54;
            this.pictureBox13.TabStop = false;
            this.pictureBox13.Click += new System.EventHandler(this.pictureBox13_Click);
            // 
            // pictureBox14
            // 
            this.pictureBox14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox14.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox14.Image")));
            this.pictureBox14.Location = new System.Drawing.Point(181, 292);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(24, 28);
            this.pictureBox14.TabIndex = 53;
            this.pictureBox14.TabStop = false;
            this.pictureBox14.Click += new System.EventHandler(this.pictureBox14_Click);
            // 
            // pictureBox15
            // 
            this.pictureBox15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox15.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox15.Image")));
            this.pictureBox15.Location = new System.Drawing.Point(181, 335);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(24, 27);
            this.pictureBox15.TabIndex = 52;
            this.pictureBox15.TabStop = false;
            this.pictureBox15.Click += new System.EventHandler(this.pictureBox15_Click);
            // 
            // pictureBox16
            // 
            this.pictureBox16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox16.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox16.Image")));
            this.pictureBox16.Location = new System.Drawing.Point(181, 377);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(24, 28);
            this.pictureBox16.TabIndex = 51;
            this.pictureBox16.TabStop = false;
            this.pictureBox16.Click += new System.EventHandler(this.pictureBox16_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox8.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox8.Image")));
            this.pictureBox8.Location = new System.Drawing.Point(80, 382);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(24, 28);
            this.pictureBox8.TabIndex = 50;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(80, 339);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(24, 28);
            this.pictureBox7.TabIndex = 49;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(80, 297);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(24, 27);
            this.pictureBox6.TabIndex = 48;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(80, 254);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(24, 28);
            this.pictureBox5.TabIndex = 47;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(80, 211);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(24, 28);
            this.pictureBox4.TabIndex = 46;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(80, 168);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(24, 28);
            this.pictureBox3.TabIndex = 45;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(80, 126);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 27);
            this.pictureBox2.TabIndex = 44;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(80, 83);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 28);
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label77);
            this.tabPage5.Controls.Add(this.label78);
            this.tabPage5.Controls.Add(this.label79);
            this.tabPage5.Controls.Add(this.label73);
            this.tabPage5.Controls.Add(this.label74);
            this.tabPage5.Controls.Add(this.label75);
            this.tabPage5.Controls.Add(this.label76);
            this.tabPage5.Controls.Add(this.label72);
            this.tabPage5.Controls.Add(this.pictureBox56);
            this.tabPage5.Controls.Add(this.pictureBox55);
            this.tabPage5.Controls.Add(this.pictureBox54);
            this.tabPage5.Controls.Add(this.pictureBox53);
            this.tabPage5.Controls.Add(this.pictureBox52);
            this.tabPage5.Controls.Add(this.pictureBox51);
            this.tabPage5.Controls.Add(this.pictureBox50);
            this.tabPage5.Controls.Add(this.label56);
            this.tabPage5.Controls.Add(this.label57);
            this.tabPage5.Controls.Add(this.label58);
            this.tabPage5.Controls.Add(this.label59);
            this.tabPage5.Controls.Add(this.label60);
            this.tabPage5.Controls.Add(this.label61);
            this.tabPage5.Controls.Add(this.label62);
            this.tabPage5.Controls.Add(this.label63);
            this.tabPage5.Controls.Add(this.label64);
            this.tabPage5.Controls.Add(this.label65);
            this.tabPage5.Controls.Add(this.label66);
            this.tabPage5.Controls.Add(this.label67);
            this.tabPage5.Controls.Add(this.label68);
            this.tabPage5.Controls.Add(this.label69);
            this.tabPage5.Controls.Add(this.label70);
            this.tabPage5.Controls.Add(this.label71);
            this.tabPage5.Controls.Add(this.label55);
            this.tabPage5.Controls.Add(this.label54);
            this.tabPage5.Controls.Add(this.label53);
            this.tabPage5.Controls.Add(this.label52);
            this.tabPage5.Controls.Add(this.label51);
            this.tabPage5.Controls.Add(this.label50);
            this.tabPage5.Controls.Add(this.label49);
            this.tabPage5.Controls.Add(this.label48);
            this.tabPage5.Controls.Add(this.label47);
            this.tabPage5.Controls.Add(this.label46);
            this.tabPage5.Controls.Add(this.label45);
            this.tabPage5.Controls.Add(this.label44);
            this.tabPage5.Controls.Add(this.label43);
            this.tabPage5.Controls.Add(this.label42);
            this.tabPage5.Controls.Add(this.label41);
            this.tabPage5.Controls.Add(this.label40);
            this.tabPage5.Controls.Add(this.label39);
            this.tabPage5.Controls.Add(this.label38);
            this.tabPage5.Controls.Add(this.pictureBox34);
            this.tabPage5.Controls.Add(this.pictureBox35);
            this.tabPage5.Controls.Add(this.pictureBox36);
            this.tabPage5.Controls.Add(this.pictureBox37);
            this.tabPage5.Controls.Add(this.pictureBox38);
            this.tabPage5.Controls.Add(this.pictureBox39);
            this.tabPage5.Controls.Add(this.pictureBox40);
            this.tabPage5.Controls.Add(this.pictureBox41);
            this.tabPage5.Controls.Add(this.pictureBox42);
            this.tabPage5.Controls.Add(this.pictureBox43);
            this.tabPage5.Controls.Add(this.pictureBox44);
            this.tabPage5.Controls.Add(this.pictureBox45);
            this.tabPage5.Controls.Add(this.pictureBox46);
            this.tabPage5.Controls.Add(this.pictureBox47);
            this.tabPage5.Controls.Add(this.pictureBox48);
            this.tabPage5.Controls.Add(this.pictureBox49);
            this.tabPage5.Controls.Add(this.pictureBox26);
            this.tabPage5.Controls.Add(this.pictureBox27);
            this.tabPage5.Controls.Add(this.pictureBox28);
            this.tabPage5.Controls.Add(this.pictureBox29);
            this.tabPage5.Controls.Add(this.pictureBox30);
            this.tabPage5.Controls.Add(this.pictureBox31);
            this.tabPage5.Controls.Add(this.pictureBox32);
            this.tabPage5.Controls.Add(this.pictureBox33);
            this.tabPage5.Controls.Add(this.pictureBox18);
            this.tabPage5.Controls.Add(this.pictureBox19);
            this.tabPage5.Controls.Add(this.pictureBox20);
            this.tabPage5.Controls.Add(this.pictureBox21);
            this.tabPage5.Controls.Add(this.pictureBox22);
            this.tabPage5.Controls.Add(this.pictureBox23);
            this.tabPage5.Controls.Add(this.pictureBox24);
            this.tabPage5.Controls.Add(this.pictureBox25);
            this.tabPage5.Location = new System.Drawing.Point(4, 36);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(632, 791);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Fault Status";
            this.tabPage5.UseVisualStyleBackColor = true;
            this.tabPage5.Click += new System.EventHandler(this.tabPage5_Click);
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label77.Location = new System.Drawing.Point(217, 708);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(109, 16);
            this.label77.TabIndex = 131;
            this.label77.Text = "Integrity Violation";
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label78.Location = new System.Drawing.Point(217, 674);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(94, 16);
            this.label78.TabIndex = 130;
            this.label78.Text = "Servo Interrupt";
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label79.Location = new System.Drawing.Point(217, 639);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(108, 16);
            this.label79.TabIndex = 129;
            this.label79.Text = "Emergency Stop";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label73.Location = new System.Drawing.Point(51, 743);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(93, 16);
            this.label73.TabIndex = 128;
            this.label73.Text = "Time Overuse";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label74.Location = new System.Drawing.Point(51, 708);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(111, 16);
            this.label74.TabIndex = 127;
            this.label74.Text = "Memory Overuse";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label75.Location = new System.Drawing.Point(51, 674);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(92, 16);
            this.label75.TabIndex = 126;
            this.label75.Text = "Program Error";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label76.Location = new System.Drawing.Point(51, 639);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(89, 16);
            this.label76.TabIndex = 125;
            this.label76.Text = "Network Error";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(17, 607);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(62, 20);
            this.label72.TabIndex = 124;
            this.label72.Text = "System";
            // 
            // pictureBox56
            // 
            this.pictureBox56.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox56.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox56.Image")));
            this.pictureBox56.Location = new System.Drawing.Point(187, 703);
            this.pictureBox56.Name = "pictureBox56";
            this.pictureBox56.Size = new System.Drawing.Size(24, 27);
            this.pictureBox56.TabIndex = 123;
            this.pictureBox56.TabStop = false;
            // 
            // pictureBox55
            // 
            this.pictureBox55.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox55.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox55.Image")));
            this.pictureBox55.Location = new System.Drawing.Point(187, 668);
            this.pictureBox55.Name = "pictureBox55";
            this.pictureBox55.Size = new System.Drawing.Size(24, 28);
            this.pictureBox55.TabIndex = 122;
            this.pictureBox55.TabStop = false;
            // 
            // pictureBox54
            // 
            this.pictureBox54.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox54.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox54.Image")));
            this.pictureBox54.Location = new System.Drawing.Point(187, 633);
            this.pictureBox54.Name = "pictureBox54";
            this.pictureBox54.Size = new System.Drawing.Size(24, 28);
            this.pictureBox54.TabIndex = 121;
            this.pictureBox54.TabStop = false;
            // 
            // pictureBox53
            // 
            this.pictureBox53.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox53.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox53.Image")));
            this.pictureBox53.Location = new System.Drawing.Point(21, 736);
            this.pictureBox53.Name = "pictureBox53";
            this.pictureBox53.Size = new System.Drawing.Size(24, 28);
            this.pictureBox53.TabIndex = 120;
            this.pictureBox53.TabStop = false;
            // 
            // pictureBox52
            // 
            this.pictureBox52.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox52.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox52.Image")));
            this.pictureBox52.Location = new System.Drawing.Point(21, 702);
            this.pictureBox52.Name = "pictureBox52";
            this.pictureBox52.Size = new System.Drawing.Size(24, 27);
            this.pictureBox52.TabIndex = 119;
            this.pictureBox52.TabStop = false;
            // 
            // pictureBox51
            // 
            this.pictureBox51.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox51.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox51.Image")));
            this.pictureBox51.Location = new System.Drawing.Point(21, 667);
            this.pictureBox51.Name = "pictureBox51";
            this.pictureBox51.Size = new System.Drawing.Size(24, 28);
            this.pictureBox51.TabIndex = 118;
            this.pictureBox51.TabStop = false;
            // 
            // pictureBox50
            // 
            this.pictureBox50.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox50.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox50.Image")));
            this.pictureBox50.Location = new System.Drawing.Point(21, 633);
            this.pictureBox50.Name = "pictureBox50";
            this.pictureBox50.Size = new System.Drawing.Size(24, 28);
            this.pictureBox50.TabIndex = 117;
            this.pictureBox50.TabStop = false;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label56.Location = new System.Drawing.Point(313, 565);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(147, 16);
            this.label56.TabIndex = 116;
            this.label56.Text = "Servo Processor Alarm";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label57.Location = new System.Drawing.Point(313, 531);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(80, 16);
            this.label57.TabIndex = 115;
            this.label57.Text = "Current Limit";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.Location = new System.Drawing.Point(313, 496);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(113, 16);
            this.label58.TabIndex = 114;
            this.label58.Text = "Acceleration Limit";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.Location = new System.Drawing.Point(313, 462);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(86, 16);
            this.label59.TabIndex = 113;
            this.label59.Text = "Velocity Limit";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.Location = new System.Drawing.Point(313, 427);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(131, 16);
            this.label60.TabIndex = 112;
            this.label60.Text = "Critical Position Error";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.Location = new System.Drawing.Point(313, 392);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(88, 16);
            this.label61.TabIndex = 111;
            this.label61.Text = "Position Error";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.Location = new System.Drawing.Point(313, 358);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(160, 16);
            this.label62.TabIndex = 110;
            this.label62.Text = "Secondary Encoder Error";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label63.Location = new System.Drawing.Point(313, 323);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(140, 16);
            this.label63.TabIndex = 109;
            this.label63.Text = "Primary Encoder Error";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label64.Location = new System.Drawing.Point(313, 288);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(82, 16);
            this.label64.TabIndex = 108;
            this.label64.Text = "Driver Alarm";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label65.Location = new System.Drawing.Point(313, 254);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(220, 16);
            this.label65.TabIndex = 107;
            this.label65.Text = "Secondary Encoder Not Connected";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.Location = new System.Drawing.Point(313, 219);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(200, 16);
            this.label66.TabIndex = 106;
            this.label66.Text = "Primary Encoder Not Connected";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.Location = new System.Drawing.Point(313, 185);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(114, 16);
            this.label67.TabIndex = 105;
            this.label67.Text = "Software Left Limit";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.Location = new System.Drawing.Point(313, 150);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(124, 16);
            this.label68.TabIndex = 104;
            this.label68.Text = "Software Right Limit";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label69.Location = new System.Drawing.Point(313, 115);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(100, 16);
            this.label69.TabIndex = 103;
            this.label69.Text = "Motor Overheat";
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.Location = new System.Drawing.Point(313, 81);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(59, 16);
            this.label70.TabIndex = 102;
            this.label70.Text = "Left Limit";
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.Location = new System.Drawing.Point(313, 46);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(69, 16);
            this.label71.TabIndex = 101;
            this.label71.Text = "Right Limit";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.Location = new System.Drawing.Point(51, 565);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(147, 16);
            this.label55.TabIndex = 100;
            this.label55.Text = "Servo Processor Alarm";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.Location = new System.Drawing.Point(51, 531);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(80, 16);
            this.label54.TabIndex = 99;
            this.label54.Text = "Current Limit";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label53.Location = new System.Drawing.Point(51, 496);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(113, 16);
            this.label53.TabIndex = 98;
            this.label53.Text = "Acceleration Limit";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label52.Location = new System.Drawing.Point(51, 462);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(86, 16);
            this.label52.TabIndex = 97;
            this.label52.Text = "Velocity Limit";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.Location = new System.Drawing.Point(51, 427);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(131, 16);
            this.label51.TabIndex = 96;
            this.label51.Text = "Critical Position Error";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(51, 392);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(88, 16);
            this.label50.TabIndex = 95;
            this.label50.Text = "Position Error";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.Location = new System.Drawing.Point(51, 358);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(160, 16);
            this.label49.TabIndex = 94;
            this.label49.Text = "Secondary Encoder Error";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.Location = new System.Drawing.Point(51, 323);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(140, 16);
            this.label48.TabIndex = 93;
            this.label48.Text = "Primary Encoder Error";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.Location = new System.Drawing.Point(51, 288);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(82, 16);
            this.label47.TabIndex = 92;
            this.label47.Text = "Driver Alarm";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(51, 254);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(220, 16);
            this.label46.TabIndex = 91;
            this.label46.Text = "Secondary Encoder Not Connected";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(51, 219);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(200, 16);
            this.label45.TabIndex = 90;
            this.label45.Text = "Primary Encoder Not Connected";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(51, 185);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(114, 16);
            this.label44.TabIndex = 89;
            this.label44.Text = "Software Left Limit";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(51, 150);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(124, 16);
            this.label43.TabIndex = 88;
            this.label43.Text = "Software Right Limit";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(51, 115);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(100, 16);
            this.label42.TabIndex = 87;
            this.label42.Text = "Motor Overheat";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(51, 81);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(59, 16);
            this.label41.TabIndex = 86;
            this.label41.Text = "Left Limit";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(51, 46);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(69, 16);
            this.label40.TabIndex = 85;
            this.label40.Text = "Right Limit";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(279, 14);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(51, 20);
            this.label39.TabIndex = 84;
            this.label39.Text = "Axis 1";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(17, 14);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(51, 20);
            this.label38.TabIndex = 83;
            this.label38.Text = "Axis 0";
            // 
            // pictureBox34
            // 
            this.pictureBox34.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox34.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox34.Image")));
            this.pictureBox34.Location = new System.Drawing.Point(283, 561);
            this.pictureBox34.Name = "pictureBox34";
            this.pictureBox34.Size = new System.Drawing.Size(24, 27);
            this.pictureBox34.TabIndex = 82;
            this.pictureBox34.TabStop = false;
            // 
            // pictureBox35
            // 
            this.pictureBox35.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox35.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox35.Image")));
            this.pictureBox35.Location = new System.Drawing.Point(283, 526);
            this.pictureBox35.Name = "pictureBox35";
            this.pictureBox35.Size = new System.Drawing.Size(24, 28);
            this.pictureBox35.TabIndex = 81;
            this.pictureBox35.TabStop = false;
            // 
            // pictureBox36
            // 
            this.pictureBox36.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox36.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox36.Image")));
            this.pictureBox36.Location = new System.Drawing.Point(283, 492);
            this.pictureBox36.Name = "pictureBox36";
            this.pictureBox36.Size = new System.Drawing.Size(24, 27);
            this.pictureBox36.TabIndex = 80;
            this.pictureBox36.TabStop = false;
            // 
            // pictureBox37
            // 
            this.pictureBox37.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox37.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox37.Image")));
            this.pictureBox37.Location = new System.Drawing.Point(283, 457);
            this.pictureBox37.Name = "pictureBox37";
            this.pictureBox37.Size = new System.Drawing.Size(24, 28);
            this.pictureBox37.TabIndex = 79;
            this.pictureBox37.TabStop = false;
            // 
            // pictureBox38
            // 
            this.pictureBox38.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox38.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox38.Image")));
            this.pictureBox38.Location = new System.Drawing.Point(283, 422);
            this.pictureBox38.Name = "pictureBox38";
            this.pictureBox38.Size = new System.Drawing.Size(24, 28);
            this.pictureBox38.TabIndex = 78;
            this.pictureBox38.TabStop = false;
            // 
            // pictureBox39
            // 
            this.pictureBox39.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox39.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox39.Image")));
            this.pictureBox39.Location = new System.Drawing.Point(283, 388);
            this.pictureBox39.Name = "pictureBox39";
            this.pictureBox39.Size = new System.Drawing.Size(24, 27);
            this.pictureBox39.TabIndex = 77;
            this.pictureBox39.TabStop = false;
            // 
            // pictureBox40
            // 
            this.pictureBox40.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox40.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox40.Image")));
            this.pictureBox40.Location = new System.Drawing.Point(283, 353);
            this.pictureBox40.Name = "pictureBox40";
            this.pictureBox40.Size = new System.Drawing.Size(24, 28);
            this.pictureBox40.TabIndex = 76;
            this.pictureBox40.TabStop = false;
            // 
            // pictureBox41
            // 
            this.pictureBox41.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox41.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox41.Image")));
            this.pictureBox41.Location = new System.Drawing.Point(283, 318);
            this.pictureBox41.Name = "pictureBox41";
            this.pictureBox41.Size = new System.Drawing.Size(24, 28);
            this.pictureBox41.TabIndex = 75;
            this.pictureBox41.TabStop = false;
            // 
            // pictureBox42
            // 
            this.pictureBox42.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox42.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox42.Image")));
            this.pictureBox42.Location = new System.Drawing.Point(283, 284);
            this.pictureBox42.Name = "pictureBox42";
            this.pictureBox42.Size = new System.Drawing.Size(24, 28);
            this.pictureBox42.TabIndex = 74;
            this.pictureBox42.TabStop = false;
            // 
            // pictureBox43
            // 
            this.pictureBox43.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox43.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox43.Image")));
            this.pictureBox43.Location = new System.Drawing.Point(283, 249);
            this.pictureBox43.Name = "pictureBox43";
            this.pictureBox43.Size = new System.Drawing.Size(24, 28);
            this.pictureBox43.TabIndex = 73;
            this.pictureBox43.TabStop = false;
            // 
            // pictureBox44
            // 
            this.pictureBox44.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox44.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox44.Image")));
            this.pictureBox44.Location = new System.Drawing.Point(283, 215);
            this.pictureBox44.Name = "pictureBox44";
            this.pictureBox44.Size = new System.Drawing.Size(24, 27);
            this.pictureBox44.TabIndex = 72;
            this.pictureBox44.TabStop = false;
            // 
            // pictureBox45
            // 
            this.pictureBox45.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox45.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox45.Image")));
            this.pictureBox45.Location = new System.Drawing.Point(283, 180);
            this.pictureBox45.Name = "pictureBox45";
            this.pictureBox45.Size = new System.Drawing.Size(24, 28);
            this.pictureBox45.TabIndex = 71;
            this.pictureBox45.TabStop = false;
            // 
            // pictureBox46
            // 
            this.pictureBox46.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox46.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox46.Image")));
            this.pictureBox46.Location = new System.Drawing.Point(283, 145);
            this.pictureBox46.Name = "pictureBox46";
            this.pictureBox46.Size = new System.Drawing.Size(24, 28);
            this.pictureBox46.TabIndex = 70;
            this.pictureBox46.TabStop = false;
            // 
            // pictureBox47
            // 
            this.pictureBox47.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox47.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox47.Image")));
            this.pictureBox47.Location = new System.Drawing.Point(283, 111);
            this.pictureBox47.Name = "pictureBox47";
            this.pictureBox47.Size = new System.Drawing.Size(24, 27);
            this.pictureBox47.TabIndex = 69;
            this.pictureBox47.TabStop = false;
            // 
            // pictureBox48
            // 
            this.pictureBox48.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox48.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox48.Image")));
            this.pictureBox48.Location = new System.Drawing.Point(283, 76);
            this.pictureBox48.Name = "pictureBox48";
            this.pictureBox48.Size = new System.Drawing.Size(24, 28);
            this.pictureBox48.TabIndex = 68;
            this.pictureBox48.TabStop = false;
            this.pictureBox48.Click += new System.EventHandler(this.pictureBox48_Click);
            // 
            // pictureBox49
            // 
            this.pictureBox49.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox49.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox49.Image")));
            this.pictureBox49.Location = new System.Drawing.Point(283, 42);
            this.pictureBox49.Name = "pictureBox49";
            this.pictureBox49.Size = new System.Drawing.Size(24, 27);
            this.pictureBox49.TabIndex = 67;
            this.pictureBox49.TabStop = false;
            // 
            // pictureBox26
            // 
            this.pictureBox26.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox26.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox26.Image")));
            this.pictureBox26.Location = new System.Drawing.Point(21, 284);
            this.pictureBox26.Name = "pictureBox26";
            this.pictureBox26.Size = new System.Drawing.Size(24, 28);
            this.pictureBox26.TabIndex = 66;
            this.pictureBox26.TabStop = false;
            // 
            // pictureBox27
            // 
            this.pictureBox27.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox27.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox27.Image")));
            this.pictureBox27.Location = new System.Drawing.Point(21, 249);
            this.pictureBox27.Name = "pictureBox27";
            this.pictureBox27.Size = new System.Drawing.Size(24, 28);
            this.pictureBox27.TabIndex = 65;
            this.pictureBox27.TabStop = false;
            // 
            // pictureBox28
            // 
            this.pictureBox28.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox28.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox28.Image")));
            this.pictureBox28.Location = new System.Drawing.Point(21, 215);
            this.pictureBox28.Name = "pictureBox28";
            this.pictureBox28.Size = new System.Drawing.Size(24, 27);
            this.pictureBox28.TabIndex = 64;
            this.pictureBox28.TabStop = false;
            // 
            // pictureBox29
            // 
            this.pictureBox29.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox29.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox29.Image")));
            this.pictureBox29.Location = new System.Drawing.Point(21, 180);
            this.pictureBox29.Name = "pictureBox29";
            this.pictureBox29.Size = new System.Drawing.Size(24, 28);
            this.pictureBox29.TabIndex = 63;
            this.pictureBox29.TabStop = false;
            // 
            // pictureBox30
            // 
            this.pictureBox30.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox30.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox30.Image")));
            this.pictureBox30.Location = new System.Drawing.Point(21, 145);
            this.pictureBox30.Name = "pictureBox30";
            this.pictureBox30.Size = new System.Drawing.Size(24, 28);
            this.pictureBox30.TabIndex = 62;
            this.pictureBox30.TabStop = false;
            // 
            // pictureBox31
            // 
            this.pictureBox31.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox31.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox31.Image")));
            this.pictureBox31.Location = new System.Drawing.Point(21, 111);
            this.pictureBox31.Name = "pictureBox31";
            this.pictureBox31.Size = new System.Drawing.Size(24, 27);
            this.pictureBox31.TabIndex = 61;
            this.pictureBox31.TabStop = false;
            // 
            // pictureBox32
            // 
            this.pictureBox32.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox32.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox32.Image")));
            this.pictureBox32.Location = new System.Drawing.Point(21, 76);
            this.pictureBox32.Name = "pictureBox32";
            this.pictureBox32.Size = new System.Drawing.Size(24, 28);
            this.pictureBox32.TabIndex = 60;
            this.pictureBox32.TabStop = false;
            // 
            // pictureBox33
            // 
            this.pictureBox33.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox33.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox33.Image")));
            this.pictureBox33.Location = new System.Drawing.Point(21, 42);
            this.pictureBox33.Name = "pictureBox33";
            this.pictureBox33.Size = new System.Drawing.Size(24, 27);
            this.pictureBox33.TabIndex = 59;
            this.pictureBox33.TabStop = false;
            // 
            // pictureBox18
            // 
            this.pictureBox18.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox18.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox18.Image")));
            this.pictureBox18.Location = new System.Drawing.Point(21, 561);
            this.pictureBox18.Name = "pictureBox18";
            this.pictureBox18.Size = new System.Drawing.Size(24, 27);
            this.pictureBox18.TabIndex = 58;
            this.pictureBox18.TabStop = false;
            // 
            // pictureBox19
            // 
            this.pictureBox19.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox19.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox19.Image")));
            this.pictureBox19.Location = new System.Drawing.Point(21, 526);
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.Size = new System.Drawing.Size(24, 28);
            this.pictureBox19.TabIndex = 57;
            this.pictureBox19.TabStop = false;
            // 
            // pictureBox20
            // 
            this.pictureBox20.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox20.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox20.Image")));
            this.pictureBox20.Location = new System.Drawing.Point(21, 492);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(24, 27);
            this.pictureBox20.TabIndex = 56;
            this.pictureBox20.TabStop = false;
            // 
            // pictureBox21
            // 
            this.pictureBox21.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox21.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox21.Image")));
            this.pictureBox21.Location = new System.Drawing.Point(21, 457);
            this.pictureBox21.Name = "pictureBox21";
            this.pictureBox21.Size = new System.Drawing.Size(24, 28);
            this.pictureBox21.TabIndex = 55;
            this.pictureBox21.TabStop = false;
            // 
            // pictureBox22
            // 
            this.pictureBox22.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox22.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox22.Image")));
            this.pictureBox22.Location = new System.Drawing.Point(21, 422);
            this.pictureBox22.Name = "pictureBox22";
            this.pictureBox22.Size = new System.Drawing.Size(24, 28);
            this.pictureBox22.TabIndex = 54;
            this.pictureBox22.TabStop = false;
            // 
            // pictureBox23
            // 
            this.pictureBox23.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox23.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox23.Image")));
            this.pictureBox23.Location = new System.Drawing.Point(21, 388);
            this.pictureBox23.Name = "pictureBox23";
            this.pictureBox23.Size = new System.Drawing.Size(24, 27);
            this.pictureBox23.TabIndex = 53;
            this.pictureBox23.TabStop = false;
            // 
            // pictureBox24
            // 
            this.pictureBox24.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox24.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox24.Image")));
            this.pictureBox24.Location = new System.Drawing.Point(21, 353);
            this.pictureBox24.Name = "pictureBox24";
            this.pictureBox24.Size = new System.Drawing.Size(24, 28);
            this.pictureBox24.TabIndex = 52;
            this.pictureBox24.TabStop = false;
            // 
            // pictureBox25
            // 
            this.pictureBox25.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox25.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox25.Image")));
            this.pictureBox25.Location = new System.Drawing.Point(21, 318);
            this.pictureBox25.Name = "pictureBox25";
            this.pictureBox25.Size = new System.Drawing.Size(24, 28);
            this.pictureBox25.TabIndex = 51;
            this.pictureBox25.TabStop = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.AutoExposure2);
            this.tabPage6.Controls.Add(this.AutoExposure1);
            this.tabPage6.Controls.Add(this.AutoExposure0);
            this.tabPage6.Controls.Add(this.label93);
            this.tabPage6.Controls.Add(this.label94);
            this.tabPage6.Controls.Add(this.label91);
            this.tabPage6.Controls.Add(this.label92);
            this.tabPage6.Controls.Add(this.label90);
            this.tabPage6.Controls.Add(this.label89);
            this.tabPage6.Controls.Add(this.label88);
            this.tabPage6.Controls.Add(this.label87);
            this.tabPage6.Controls.Add(this.ExposureBar2);
            this.tabPage6.Controls.Add(this.label86);
            this.tabPage6.Controls.Add(this.ExposureBar1);
            this.tabPage6.Controls.Add(this.label19);
            this.tabPage6.Controls.Add(this.ExposureBar0);
            this.tabPage6.Controls.Add(this.pictureBox58);
            this.tabPage6.Controls.Add(this.pictureBox57);
            this.tabPage6.Controls.Add(this.pictureBox17);
            this.tabPage6.Location = new System.Drawing.Point(4, 36);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(632, 791);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Image";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // AutoExposure2
            // 
            this.AutoExposure2.AutoSize = true;
            this.AutoExposure2.Location = new System.Drawing.Point(478, 673);
            this.AutoExposure2.Name = "AutoExposure2";
            this.AutoExposure2.Size = new System.Drawing.Size(133, 24);
            this.AutoExposure2.TabIndex = 63;
            this.AutoExposure2.Text = "Auto Exposure";
            this.AutoExposure2.UseVisualStyleBackColor = true;
            this.AutoExposure2.Visible = false;
            this.AutoExposure2.CheckedChanged += new System.EventHandler(this.AutoExposure2_CheckedChanged);
            // 
            // AutoExposure1
            // 
            this.AutoExposure1.AutoSize = true;
            this.AutoExposure1.Location = new System.Drawing.Point(478, 417);
            this.AutoExposure1.Name = "AutoExposure1";
            this.AutoExposure1.Size = new System.Drawing.Size(133, 24);
            this.AutoExposure1.TabIndex = 62;
            this.AutoExposure1.Text = "Auto Exposure";
            this.AutoExposure1.UseVisualStyleBackColor = true;
            this.AutoExposure1.Visible = false;
            this.AutoExposure1.CheckedChanged += new System.EventHandler(this.AutoExposure1_CheckedChanged);
            // 
            // AutoExposure0
            // 
            this.AutoExposure0.AutoSize = true;
            this.AutoExposure0.Location = new System.Drawing.Point(478, 162);
            this.AutoExposure0.Name = "AutoExposure0";
            this.AutoExposure0.Size = new System.Drawing.Size(133, 24);
            this.AutoExposure0.TabIndex = 61;
            this.AutoExposure0.Text = "Auto Exposure";
            this.AutoExposure0.UseVisualStyleBackColor = true;
            this.AutoExposure0.Visible = false;
            this.AutoExposure0.CheckedChanged += new System.EventHandler(this.AutoExposure0_CheckedChanged);
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(479, 37);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(24, 20);
            this.label93.TabIndex = 60;
            this.label93.Text = "Y:";
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(509, 37);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(49, 20);
            this.label94.TabIndex = 59;
            this.label94.Text = "0.000";
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.Location = new System.Drawing.Point(479, 14);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(24, 20);
            this.label91.TabIndex = 58;
            this.label91.Text = "X:";
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(509, 14);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(49, 20);
            this.label92.TabIndex = 57;
            this.label92.Text = "0.000";
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Location = new System.Drawing.Point(478, 749);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(80, 20);
            this.label90.TabIndex = 56;
            this.label90.Text = "Exposure:";
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(478, 494);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(80, 20);
            this.label89.TabIndex = 55;
            this.label89.Text = "Exposure:";
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Location = new System.Drawing.Point(478, 239);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(80, 20);
            this.label88.TabIndex = 54;
            this.label88.Text = "Exposure:";
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Location = new System.Drawing.Point(555, 749);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(65, 20);
            this.label87.TabIndex = 53;
            this.label87.Text = "0.32 ms";
            // 
            // ExposureBar2
            // 
            this.ExposureBar2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ExposureBar2.Location = new System.Drawing.Point(478, 706);
            this.ExposureBar2.Maximum = 105;
            this.ExposureBar2.Minimum = 1;
            this.ExposureBar2.Name = "ExposureBar2";
            this.ExposureBar2.Size = new System.Drawing.Size(103, 45);
            this.ExposureBar2.TabIndex = 52;
            this.ExposureBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ExposureBar2.Value = 1;
            this.ExposureBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.Location = new System.Drawing.Point(555, 494);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(65, 20);
            this.label86.TabIndex = 51;
            this.label86.Text = "0.32 ms";
            // 
            // ExposureBar1
            // 
            this.ExposureBar1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ExposureBar1.Location = new System.Drawing.Point(478, 451);
            this.ExposureBar1.Maximum = 105;
            this.ExposureBar1.Minimum = 1;
            this.ExposureBar1.Name = "ExposureBar1";
            this.ExposureBar1.Size = new System.Drawing.Size(103, 45);
            this.ExposureBar1.TabIndex = 50;
            this.ExposureBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ExposureBar1.Value = 1;
            this.ExposureBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(555, 239);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 20);
            this.label19.TabIndex = 49;
            this.label19.Text = "0.32 ms";
            // 
            // ExposureBar0
            // 
            this.ExposureBar0.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ExposureBar0.Location = new System.Drawing.Point(478, 196);
            this.ExposureBar0.Maximum = 105;
            this.ExposureBar0.Minimum = 1;
            this.ExposureBar0.Name = "ExposureBar0";
            this.ExposureBar0.Size = new System.Drawing.Size(103, 45);
            this.ExposureBar0.TabIndex = 45;
            this.ExposureBar0.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ExposureBar0.Value = 1;
            this.ExposureBar0.Scroll += new System.EventHandler(this.trackBarExposure_Scroll);
            // 
            // pictureBox58
            // 
            this.pictureBox58.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox58.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox58.Location = new System.Drawing.Point(5, 524);
            this.pictureBox58.Name = "pictureBox58";
            this.pictureBox58.Size = new System.Drawing.Size(320, 256);
            this.pictureBox58.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox58.TabIndex = 7;
            this.pictureBox58.TabStop = false;
            // 
            // pictureBox57
            // 
            this.pictureBox57.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox57.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox57.Location = new System.Drawing.Point(5, 269);
            this.pictureBox57.Name = "pictureBox57";
            this.pictureBox57.Size = new System.Drawing.Size(320, 256);
            this.pictureBox57.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox57.TabIndex = 6;
            this.pictureBox57.TabStop = false;
            // 
            // pictureBox17
            // 
            this.pictureBox17.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox17.Location = new System.Drawing.Point(5, 14);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(320, 256);
            this.pictureBox17.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox17.TabIndex = 5;
            this.pictureBox17.TabStop = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(983, 47);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(100, 28);
            this.label5.TabIndex = 31;
            this.label5.Text = "-123.456";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(746, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 25);
            this.label4.TabIndex = 30;
            this.label4.Text = "Pos. Error :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(728, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 25);
            this.label3.TabIndex = 29;
            this.label3.Text = "Current Pos :";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Lime;
            this.label6.Location = new System.Drawing.Point(872, 83);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(100, 28);
            this.label6.TabIndex = 32;
            this.label6.Text = "0.0";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(781, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 25);
            this.label7.TabIndex = 33;
            this.label7.Text = "Status :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(997, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 25);
            this.label8.TabIndex = 34;
            this.label8.Text = "Stop";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(886, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 25);
            this.label9.TabIndex = 35;
            this.label9.Text = "Stop";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Lime;
            this.label10.Location = new System.Drawing.Point(983, 83);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(100, 28);
            this.label10.TabIndex = 37;
            this.label10.Text = "0.0";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Black;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Lime;
            this.label11.Location = new System.Drawing.Point(872, 47);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(100, 28);
            this.label11.TabIndex = 36;
            this.label11.Text = "-123.456";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(997, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 25);
            this.label12.TabIndex = 38;
            this.label12.Text = "Axis Y";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(885, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 25);
            this.label13.TabIndex = 39;
            this.label13.Text = "Axis X";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(730, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 63);
            this.button1.TabIndex = 40;
            this.button1.Text = "Capture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button12
            // 
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(730, 352);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(101, 63);
            this.button12.TabIndex = 44;
            this.button12.Text = "Stop";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(730, 211);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(101, 64);
            this.button13.TabIndex = 45;
            this.button13.Text = "Enable Interrupts";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // chart1
            // 
            chartArea3.AxisX.Interval = 40D;
            chartArea3.AxisX.Maximum = 280D;
            chartArea3.AxisX.Minimum = -280D;
            chartArea3.AxisY.Interval = 40D;
            chartArea3.AxisY.Maximum = 280D;
            chartArea3.AxisY.Minimum = -280D;
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.Location = new System.Drawing.Point(837, 226);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series3.Color = System.Drawing.Color.Red;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Position";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(530, 495);
            this.chart1.TabIndex = 46;
            this.chart1.Text = "chart1";
            // 
            // button14
            // 
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.Location = new System.Drawing.Point(891, 758);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(323, 79);
            this.button14.TabIndex = 47;
            this.button14.Text = "Back to Main Menu";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1401, 852);
            this.shapeContainer1.TabIndex = 48;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 662;
            this.lineShape1.X2 = 665;
            this.lineShape1.Y1 = 21;
            this.lineShape1.Y2 = 737;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(767, 727);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(11, 12);
            this.label35.TabIndex = 50;
            this.label35.Text = "0";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(730, 727);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(35, 12);
            this.label36.TabIndex = 49;
            this.label36.Text = "Time :";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "png";
            this.saveFileDialog1.Filter = "|*.png\"";
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog2_FileOk);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.Location = new System.Drawing.Point(730, 440);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(101, 63);
            this.button17.TabIndex = 51;
            this.button17.Text = "Disable Motors";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button18.Location = new System.Drawing.Point(730, 509);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(101, 63);
            this.button18.TabIndex = 52;
            this.button18.Text = "Enable Motors";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // label97
            // 
            this.label97.AutoSize = true;
            this.label97.Location = new System.Drawing.Point(768, 748);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(11, 12);
            this.label97.TabIndex = 54;
            this.label97.Text = "0";
            // 
            // label98
            // 
            this.label98.AutoSize = true;
            this.label98.Location = new System.Drawing.Point(731, 748);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(35, 12);
            this.label98.TabIndex = 53;
            this.label98.Text = "Time :";
            // 
            // label99
            // 
            this.label99.AutoSize = true;
            this.label99.Location = new System.Drawing.Point(768, 769);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(11, 12);
            this.label99.TabIndex = 56;
            this.label99.Text = "0";
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Location = new System.Drawing.Point(731, 769);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(35, 12);
            this.label100.TabIndex = 55;
            this.label100.Text = "Time :";
            // 
            // Col0
            // 
            this.Col0.HeaderText = "No.";
            this.Col0.Name = "Col0";
            this.Col0.ReadOnly = true;
            this.Col0.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Col0.Width = 48;
            // 
            // Col1
            // 
            this.Col1.HeaderText = "Y";
            this.Col1.Name = "Col1";
            this.Col1.ReadOnly = true;
            this.Col1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Col2
            // 
            this.Col2.HeaderText = "X";
            this.Col2.Name = "Col2";
            this.Col2.ReadOnly = true;
            this.Col2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Col3
            // 
            this.Col3.HeaderText = "Velocity";
            this.Col3.Name = "Col3";
            this.Col3.ReadOnly = true;
            this.Col3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.Location = new System.Drawing.Point(440, 478);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(152, 20);
            this.label101.TabIndex = 40;
            this.label101.Text = "Start Time: 00:00:00";
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Location = new System.Drawing.Point(440, 517);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(151, 20);
            this.label102.TabIndex = 39;
            this.label102.Text = "Stop Time: 00:00:00";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(89, 672);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(36, 26);
            this.textBox10.TabIndex = 43;
            this.textBox10.Text = "3";
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.Location = new System.Drawing.Point(10, 675);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(73, 20);
            this.label103.TabIndex = 44;
            this.label103.Text = "氨ゎ计";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(89, 709);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(36, 26);
            this.textBox11.TabIndex = 41;
            this.textBox11.Text = "2";
            // 
            // label104
            // 
            this.label104.AutoSize = true;
            this.label104.Location = new System.Drawing.Point(10, 712);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(73, 20);
            this.label104.TabIndex = 42;
            this.label104.Text = "狡Ω计";
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Location = new System.Drawing.Point(131, 675);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(25, 20);
            this.label105.TabIndex = 45;
            this.label105.Text = "";
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Location = new System.Drawing.Point(131, 712);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(25, 20);
            this.label106.TabIndex = 46;
            this.label106.Text = "Ω";
            // 
            // mainform
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.ClientSize = new System.Drawing.Size(1401, 852);
            this.ControlBox = false;
            this.Controls.Add(this.label99);
            this.Controls.Add(this.label100);
            this.Controls.Add(this.label97);
            this.Controls.Add(this.label98);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.GreenPB);
            this.Controls.Add(this.GreyPB);
            this.Controls.Add(this.shapeContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XY Table Control";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainform_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GreenPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreyPB)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox56)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox55)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox54)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox53)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox52)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox51)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox50)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox35)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox36)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox37)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox39)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox40)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox41)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox42)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox45)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox46)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox47)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox48)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox49)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox29)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox30)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox25)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureBar0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox58)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox57)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new mainform());
        }

        //================================================================
        private void InitCamera()
        {
            Camera0 = new uEye.Camera();
            Camera1 = new uEye.Camera();
            Camera2 = new uEye.Camera();

            uEye.Defines.Status statusRet = 0;
            uEye.Defines.ColorMode colorMode = uEye.Defines.ColorMode.Mono8;
            //uEye.Defines.ColorConvertMode convertMode = uEye.Defines.ColorConvertMode.Hardware3X3;

            //Camera 0===================================================
            // Open Camera
            statusRet = Camera0.Init();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Camera0 initializing failed");
                Environment.Exit(-1);
            }

            //Set PixelFormat to Grey8
            //statusRet = Camera0.PixelFormat.Set(colorMode);
            if (statusRet != uEye.Defines.Status.Success)
            {
                //MessageBox.Show("Camera0 set Color failed 0.");
                //Environment.Exit(-1);
            }
            //statusRet = Camera0.Color.Converter.Set(colorMode, convertMode);
            //if (statusRet != uEye.Defines.Status.Success)
            //{
            //    MessageBox.Show("Camera0 set Color failed 1.");
            //    Environment.Exit(-1);
            //}

            // Allocate Memory
            statusRet = Camera0.Memory.Allocate();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Allocate Memory failed");
                Environment.Exit(-1);
            }

            //Exposure
            statusRet = Camera0.Timing.Exposure.Set(0.32);

            // Start Live Video
            //statusRet = Camera.Acquisition.Capture();
            if (statusRet != uEye.Defines.Status.Success)
            {
                //MessageBox.Show("Start Live Video failed");
            }

            //Camera 1===================================================
            // Open Camera
            statusRet = Camera1.Init();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Camera1 initializing failed");
                Environment.Exit(-1);
            }
            
            //Set PixelFormat to Grey8
            //statusRet = Camera1.PixelFormat.Set(colorMode);
            if (statusRet != uEye.Defines.Status.Success)
            {
                //MessageBox.Show("Camera1 set Color failed 0.");
               // Environment.Exit(-1);
            }
            //statusRet = Camera1.Color.Converter.Set(colorMode, convertMode);
            //if (statusRet != uEye.Defines.Status.Success)
            //{
            //    MessageBox.Show("Camera1 set Color failed 1.");
            //    Environment.Exit(-1);
            //}

            // Allocate Memory
            statusRet = Camera1.Memory.Allocate();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Allocate Memory failed");
                Environment.Exit(-1);
            }

            //Exposure
            statusRet = Camera1.Timing.Exposure.Set(0.32);

            // Start Live Video
            //statusRet = Camera.Acquisition.Capture();
            if (statusRet != uEye.Defines.Status.Success)
            {
                //MessageBox.Show("Start Live Video failed");
            }

            //Camera 2===================================================
            // Open Camera
            statusRet = Camera2.Init();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Camera2 initializing failed");
                Environment.Exit(-1);
            }

            //Set PixelFormat to Grey8
            //statusRet = Camera2.PixelFormat.Set(colorMode);
            if (statusRet != uEye.Defines.Status.Success)
            {
                //MessageBox.Show("Camera2 set Color failed 0.");
                //Environment.Exit(-1);
            }
            //statusRet = Camera2.Color.Converter.Set(colorMode, convertMode);
            //if (statusRet != uEye.Defines.Status.Success)
            //{
            //    MessageBox.Show("Camera2 set Color failed 1.");
            //    Environment.Exit(-1);
            //}

            // Allocate Memory
            statusRet = Camera2.Memory.Allocate();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Allocate Memory failed");
                Environment.Exit(-1);
            }

            //Exposure
            statusRet = Camera2.Timing.Exposure.Set(0.32);

            // Start Live Video
            //statusRet = Camera.Acquisition.Capture();
            if (statusRet != uEye.Defines.Status.Success)
            {
                //MessageBox.Show("Start Live Video failed");
            }
            //=========================================================
            // Connect Event

            int DevID;
            Camera0.Device.GetCameraID(out DevID);
            switch (DevID)
            {
                case 1:
                    CameraC = Camera0;
                    break;
                case 2:
                    CameraF = Camera0;
                    break;
                case 3:
                    CameraA = Camera0;
                    break;
                default:
                    CameraC = Camera0;
                    break;
            }

            Camera1.Device.GetCameraID(out DevID);
            switch (DevID)
            {
                case 1:
                    CameraC = Camera1;
                    break;
                case 2:
                    CameraF = Camera1;
                    break;
                case 3:
                    CameraA = Camera1;
                    break;
                default:
                    CameraF = Camera1;
                    break;
            }

            Camera2.Device.GetCameraID(out DevID);
            switch (DevID)
            {
                case 1:
                    CameraC = Camera2;
                    break;
                case 2:
                    CameraF = Camera2;
                    break;
                case 3:
                    CameraA = Camera2;
                    break;
                default:
                    CameraF = Camera2;
                    break;
            }


            CameraC.EventFrame += onFrameEvent0;
            CameraF.EventFrame += onFrameEvent1;
            CameraA.EventFrame += onFrameEvent2;

            //CB_Auto_Gain_Balance.Enabled = Camera0.AutoFeatures.Software.Gain.Supported;
            //CB_Auto_White_Balance.Enabled = Camera0.AutoFeatures.Software.WhiteBalance.Supported;
        }

        //================================================================
        private void onFrameEvent0(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            uEye.Camera Camera = sender as uEye.Camera;
            byte[] raw_data = new byte[1310720];

            Int32 s32MemID;
            Camera.Memory.GetActive(out s32MemID);
            Camera.Memory.CopyToArray(s32MemID, out raw_data);

            Bitmap bm = new Bitmap(1280, 1024, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            //bm.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            Camera.Memory.CopyToBitmap(s32MemID, out bm);
            pictureBox17.Image = bm;
            string file_name_jpg = string.Format("D:\\{0:MM-dd}-X{1:f03}-Y{2:f03}-image0.jpg", DateTime.Now, Math.Round(Current_Position[0], 2), Math.Round(Current_Position[1], 2));
            //file_name_bmp += string.Format("-X=%f-Y=%f", Math.Round(Current_Position[0], 3), Math.Round(Current_Position[0], 3));
            label92.Text = string.Format("{0:f03}", Current_Position[0]);
            label94.Text = string.Format("{0:f03}", Current_Position[1]);
            Camera.Image.Save(file_name_jpg, System.Drawing.Imaging.ImageFormat.Jpeg);

            //㊣场祘Α===========
            string target = Environment.CurrentDirectory + @"\idscheck.exe";
            string arg_str = "-f " + file_name_jpg + " --center -t 200";

            Process idscheck = new Process();
            idscheck.StartInfo.FileName = target;
            idscheck.StartInfo.Arguments = arg_str;
            idscheck.StartInfo.UseShellExecute = false;
            idscheck.StartInfo.RedirectStandardOutput = true;
            idscheck.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            idscheck.Start();

            string idscheck_output = idscheck.StandardOutput.ReadToEnd();
            int index = idscheck_output.LastIndexOf(' ');
            if (index != -1)
            {
                string result = idscheck_output.Substring(index);
                MessageBox.Show(result, "center");
            }
            idscheck.Close();
            //㊣场祘Α===========
     
            sw.Stop();
            TimeSpan t = sw.Elapsed;
            label35.Text = t.ToString();
        }

        //================================================================
        private void onFrameEvent1(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            uEye.Camera Camera = sender as uEye.Camera;
            byte[] raw_data = new byte[1310720];

            Int32 s32MemID;
            Camera.Memory.GetActive(out s32MemID);
            Camera.Memory.CopyToArray(s32MemID, out raw_data);

            Bitmap bm = new Bitmap(1280, 1024, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            Camera.Memory.CopyToBitmap(s32MemID, out bm);
            pictureBox57.Image = bm;
            string file_name_jpg = string.Format("D:\\{0:MM-dd}-X{1:f03}-Y{2:f03}-image1.jpg", DateTime.Now, Math.Round(Current_Position[0], 2), Math.Round(Current_Position[1], 2));
            Camera.Image.Save(file_name_jpg, System.Drawing.Imaging.ImageFormat.Jpeg);

            //㊣场祘Α===========
            string target = Environment.CurrentDirectory + @"\idscheck.exe";
            string arg_str = "-f " + file_name_jpg + " --focus -t 100";

            Process idscheck = new Process();
            idscheck.StartInfo.FileName = target;
            idscheck.StartInfo.Arguments = arg_str;
            idscheck.StartInfo.UseShellExecute = false;
            idscheck.StartInfo.RedirectStandardOutput = true;
            idscheck.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            idscheck.Start();

            string idscheck_output = idscheck.StandardOutput.ReadToEnd();
            int index = idscheck_output.LastIndexOf(' ');
            if (index != -1)
            {
                string result = idscheck_output.Substring(index);
                MessageBox.Show(result, "focus");
            }
            idscheck.Close();
            //㊣场祘Α===========

            sw.Stop();
            TimeSpan t = sw.Elapsed;
            label97.Text = t.ToString();
        }
        //================================================================
        private void onFrameEvent2(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            uEye.Camera Camera = sender as uEye.Camera;
            byte[] raw_data = new byte[1310720];

            Int32 s32MemID;
            Camera.Memory.GetActive(out s32MemID);
            Camera.Memory.CopyToArray(s32MemID, out raw_data);

            Bitmap bm = new Bitmap(1280, 1024, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            Camera.Memory.CopyToBitmap(s32MemID, out bm);
            pictureBox58.Image = bm;
            string file_name_jpg = string.Format("D:\\{0:MM-dd}-X{1:f03}-Y{2:f03}-image2.jpg", DateTime.Now, Math.Round(Current_Position[0], 2), Math.Round(Current_Position[1], 2));
            Camera.Image.Save(file_name_jpg, System.Drawing.Imaging.ImageFormat.Jpeg);

            //㊣场祘Α===========
            string target = Environment.CurrentDirectory + @"\idscheck.exe";
            string arg_str = "-f " + file_name_jpg + " --angle -t 130";

            Process idscheck = new Process();
            idscheck.StartInfo.FileName = target;
            idscheck.StartInfo.Arguments = arg_str;
            idscheck.StartInfo.UseShellExecute = false;
            idscheck.StartInfo.RedirectStandardOutput = true;
            idscheck.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            idscheck.Start();

            string idscheck_output = idscheck.StandardOutput.ReadToEnd();
            int index = idscheck_output.LastIndexOf(' ');
            if (index != -1)
            {
                string result = idscheck_output.Substring(index);
                MessageBox.Show(result, "angle");
            }
            idscheck.Close();
            //㊣场祘Α===========

            sw.Stop();
            TimeSpan t = sw.Elapsed;
            label99.Text = t.ToString();
        }


        //================================================================
        private void TakePhoto()
        {
            if (Camera0.Acquisition.Freeze() == uEye.Defines.Status.Success)
            {
            }
            Thread.Sleep(100);
            if (Camera1.Acquisition.Freeze() == uEye.Defines.Status.Success)
            {
            }
            Thread.Sleep(100);
            if (Camera2.Acquisition.Freeze() == uEye.Defines.Status.Success)
            {
            }
        }

        //================================================================
        private void Form1_Load(object sender, System.EventArgs e)
        {
            Axis = 0;
            // Create new object of class Channel
            // Type Channel is defined in SPiiPlusCOM650 Type Library
            Ch = new SPIIPLUSCOM660Lib.Channel();
            
            MotorStateThr = new Thread(new ThreadStart(GetMotorState));
            MotorStateThr.IsBackground = true;
            MotorStateThr.Start();
            MotorStateThr.Suspend();
            bConnected = false;
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(mainform));
            Green = ((System.Drawing.Image)(resources.GetObject("GreenPB.Image")));
            Grey = ((System.Drawing.Image)(resources.GetObject("GreyPB.Image")));

            //if communication is closed
            if (!bConnected)
            {
                try
                {
                    int Protocol;
                    Protocol = Ch.ACSC_SOCKET_STREAM_PORT;
                    // Open ethernet communuication.
                    // RemoteAddress.Text defines the controller's TCP/IP address.
                    // Protocol is TCP/IP in case of network connection, and UDP in case of point-to-point connection.
                    Ch.OpenCommEthernet("10.0.0.100", Protocol);
                    Ch.SetVelocity(0, 20.0);
                    Ch.SetVelocity(1, 20.0);

                    //Resume MotorState Thread 
                    MotorStateThr.Resume();
                    bConnected = true;
                    // Make ConnInd green 

                    Axes[0] = Ch.ACSC_AXIS_1;
                    Axes[1] = Ch.ACSC_AXIS_0;
                    Axes[2] = -1;

                    Ch.EnableM(Axes);

                    Ch.WaitMotorEnabled(Ch.ACSC_AXIS_0, 1, 5000);
                    Ch.WaitMotorEnabled(Ch.ACSC_AXIS_1, 1, 5000);
                    //Ch.RegisterEmergencyStop();

                    InitCamera();

                    //X:275.318, Y:273.129 箂翴
                    //Ch.SetFPosition(Axes[0], Ch.GetFPosition(0)-275);
                    //Ch.SetFPosition(Axes[1], Ch.GetFPosition(1)-275);
                }
                catch (COMException Ex)
                {
                    ErorMsg(Ex);
                }
            }
        }

        //================================================================
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // if communication is open
            if (bConnected)
            {
                try
                {
                    Ch.CloseComm();
                }
                catch (COMException Ex)
                {
                    ErorMsg(Ex);
                }
            }
        }

        //================================================================
        private void ZeroFBackPosBtn_Click(object sender, System.EventArgs e)
        {
            // if communication is open
            if (bConnected)
            {
                try
                {
                    Ch.SetFPosition(Axis, 0);
                }
                catch (COMException Ex)
                {
                    ErorMsg(Ex);
                }
            }
        }

        //================================================================
        private void GetMotorState()
        {
            double current_pos_x;
            double current_pos_y;
            while (true)
            {
                try
                {
                    //Get IO Status
                    RefreshIOStatus();
                    // Get motor state of the Axis
                    MotorState[0] = Ch.GetMotorState(Axes[0]);
                    MotorState[1] = Ch.GetMotorState(Axes[1]);
                    MotorFault[0] = Ch.GetFault(Axes[0]);
                    MotorFault[1] = Ch.GetFault(Axes[1]);

                    /*
                    if (DI[0] == true)
                    {
                        if (!Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_ENABLE))
                        {
                            Ch.Enable(Ch.ACSC_AXIS_0);
                            Ch.WaitMotorEnabled(Ch.ACSC_AXIS_0, 1, 5000);
                        }

                        if (!Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_ENABLE))
                        {
                            Ch.Enable(Ch.ACSC_AXIS_1);
                            Ch.WaitMotorEnabled(Ch.ACSC_AXIS_1, 1, 5000);
                        }
                    }
                    else
                    {
                        Ch.DisableM(Axes);
                    }
                    */

                    CheckForIllegalCrossThreadCalls = false;
                    // Get feedback position
                    current_pos_x = Ch.GetRPosition(Axes[0]);
                    current_pos_y = Ch.GetRPosition(Axes[1]);
                    Current_Position[0] = current_pos_x;
                    Current_Position[1] = current_pos_y;
                    current_pos_x = Math.Round(current_pos_x, 3);
                    current_pos_y = Math.Round(current_pos_y, 3);
                    string pos_string;
                    pos_string = string.Format("{0:f03}", Math.Round(current_pos_x, 3));
                    label5.Text = pos_string;
                    pos_string = string.Format("{0:f03}", Math.Round(current_pos_y, 3));
                    label11.Text = pos_string;

                    pos_string = string.Format("{0:f03}", Math.Round(Ch.GetFPosition(Axes[0]), 3));
                    label10.Text = pos_string;
                    pos_string = string.Format("{0:f03}", Math.Round(Ch.GetRPosition(Axes[1]), 3));
                    label6.Text = pos_string;

                    chart1.Series[0].Points.Clear();
                    chart1.Series[0].Points.AddXY(current_pos_y, current_pos_x); 

                    if (MotorState[0] != LastMotorState[0])
                    {
                        if (Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_INPOS))
                        {
                            label8.ForeColor = Color.Black;
                            label8.Text = "Stoped";
                        }
                        else
                        {
                            label8.ForeColor = Color.Green;
                            label8.Text = "Moving";
                        }
                    }

                    if (MotorState[1] != LastMotorState[1])
                    {
                        if (Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_INPOS))
                        {
                            label9.ForeColor = Color.Black;
                            label9.Text = "Stoped";
                        }
                        else
                        {
                            label9.ForeColor = Color.Green;
                            label9.Text = "Moving";
                        }
                    }

                    //Continue Move
                    if (continue_move)
                    {
                        if (Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_INPOS) &&
                            Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_INPOS))
                        {
                            if (CM_AutoCapture.Checked)  //Auto Capture
                            {
                                TakePhoto();
                            }

                            if (stop_time > 0)
                            {
                                stop_time--;
                            }
                            else
                            {
                                continue_move_counter++;
                                if (continue_move_counter < number_of_points)
                                {
                                    MoveToPoint(continue_move_counter);
                                    label37.Text = "Current Point: " + Convert.ToString(continue_move_counter);
                                    stop_time = Convert.ToInt32(textBox10.Text) * 10;
                                }
                                else
                                {
                                    continue_move_counter = 0;
                                    repeat_times--;
                                    label102.Text = string.Format("Stop Time: {0:hh:mm:ss}", DateTime.Now);

                                    if (repeat_times > 0)
                                    {
                                        if (Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_INPOS) &&
                                            Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_INPOS))
                                        {
                                            MoveToPoint(continue_move_counter);
                                            stop_time = Convert.ToInt32(textBox10.Text) * 10;
                                        }
                                    }
                                    else
                                    {
                                        continue_move = false;
                                        CM_Load.Enabled = true;
                                        CM_Start.Enabled = true;
                                        CM_Stop.Enabled = false;
                                        CM_Restart.Enabled = false;
                                    }
                                }
                            }
                        }
                    }

                    //Calibration Move
                    if (calibration_move)
                    {
                        if (Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_INPOS) &&
                            Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_INPOS))
                        {
                            if (CaliM_AutoCapture.Checked)  //Auto Capture
                            {
                                if (Camera0.Acquisition.Freeze() == uEye.Defines.Status.Success)
                                {
                                }
                            }
                            calibration_move_counter++;

                            //Send the trigger signal to laser to get real position
                            Ch.SetAnalogOutput(0, 3);
                            Thread.Sleep(5);
                            Ch.SetAnalogOutput(0, 0);


                            if (sw != null)  //Write data to log file
                            {
                                string log_string = string.Format("{0:f03},{1:f03}", Math.Round(Current_Position[0], 4), Math.Round(Current_Position[1], 4));
                                sw.WriteLine(log_string);
                            }

                            if (calibration_move_counter < number_of_points)
                            {
                                MoveToPoint(calibration_move_array[calibration_move_counter].x, calibration_move_array[calibration_move_counter].y, calibration_move_array[calibration_move_counter].vel);
                                label84.Text = "Current Point: " + Convert.ToString(calibration_move_counter);
                            }
                            else
                            {
                                if (sw != null)
                                {
                                    label96.Text = string.Format("Stop Time: {0:hh:mm:ss}", DateTime.Now);
                                    sw.Close();
                                    sw = null;
                                }
                                calibration_move = false;
                                calibration_move_counter = 0;
                                CaliM_Start.Enabled = true;
                                CaliM_Stop.Enabled = false;
                                CaliM_Restart.Enabled = false;
                            }
                        }
                    }
                    LastMotorState[0] = MotorState[0];
                    LastMotorState[1] = MotorState[1];

                    RefreshFaultState();
                    LastMotorFault[0] = MotorFault[0];
                    LastMotorFault[1] = MotorFault[1];
                }
                catch (COMException Ex)
                {
                    ErorMsg(Ex);
                }
                Thread.Sleep(50);
            }
        }
        //================================================================
        private void RefreshFaultState()
        {
            if (MotorFault[0] != LastMotorFault[0])
            {
                pictureBox33.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_RL) ? Green : Grey;
                pictureBox32.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_LL) ? Green : Grey;
                pictureBox31.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_HOT) ? Green : Grey;
                pictureBox30.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_SRL) ? Green : Grey;
                pictureBox29.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_SLL) ? Green : Grey;
                pictureBox28.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_ENCNC) ? Green : Grey;
                pictureBox27.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_ENC2NC) ? Green : Grey;
                pictureBox26.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_DRIVE) ? Green : Grey;
                pictureBox25.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_ENC) ? Green : Grey;
                pictureBox24.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_ENC2) ? Green : Grey;
                pictureBox23.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_PE) ? Green : Grey;
                pictureBox22.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_CPE) ? Green : Grey;
                pictureBox21.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_VL) ? Green : Grey;
                pictureBox20.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_AL) ? Green : Grey;
                pictureBox19.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_CL) ? Green : Grey;
                pictureBox18.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_SP) ? Green : Grey;

                //System Faults
                pictureBox50.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_NETWORK) ? Green : Grey;
                pictureBox51.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_PROG) ? Green : Grey;
                pictureBox52.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_MEM) ? Green : Grey;
                pictureBox53.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_TIME) ? Green : Grey;
                pictureBox54.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_ES) ? Green : Grey;
                pictureBox55.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_INT) ? Green : Grey;
                pictureBox56.Image = Convert.ToBoolean(MotorFault[0] & Ch.ACSC_SAFETY_INTGR) ? Green : Grey;
            }

            if (MotorFault[1] != LastMotorFault[1])
            {
                pictureBox49.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_RL) ? Green : Grey;
                pictureBox48.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_LL) ? Green : Grey;
                pictureBox47.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_HOT) ? Green : Grey;
                pictureBox46.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_SRL) ? Green : Grey;
                pictureBox45.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_SLL) ? Green : Grey;
                pictureBox44.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_ENCNC) ? Green : Grey;
                pictureBox43.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_ENC2NC) ? Green : Grey;
                pictureBox42.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_DRIVE) ? Green : Grey;
                pictureBox41.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_ENC) ? Green : Grey;
                pictureBox40.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_ENC2) ? Green : Grey;
                pictureBox39.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_PE) ? Green : Grey;
                pictureBox38.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_CPE) ? Green : Grey;
                pictureBox37.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_VL) ? Green : Grey;
                pictureBox36.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_AL) ? Green : Grey;
                pictureBox35.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_CL) ? Green : Grey;
                pictureBox34.Image = Convert.ToBoolean(MotorFault[1] & Ch.ACSC_SAFETY_SP) ? Green : Grey;
            }
        }
        //================================================================
        private void MakeDisconnectedState()
        {

        }

        //================================================================
        private void RefreshIOStatus()
        {
            int temp;

            //DI===================================================
            temp = Ch.GetInputPort(0);
            for (int i = 0; i < 8; i++)
            {
                DI[i] = Convert.ToBoolean(temp & (1 << i));
            }
            if (DI[0] != LastDI[0]) pictureBox1.Image = DI[0] ? Green : Grey;
            if (DI[1] != LastDI[1]) pictureBox2.Image = DI[1] ? Green : Grey;
            if (DI[2] != LastDI[2]) pictureBox3.Image = DI[2] ? Green : Grey;
            if (DI[3] != LastDI[3]) pictureBox4.Image = DI[3] ? Green : Grey;
            if (DI[4] != LastDI[4]) pictureBox5.Image = DI[4] ? Green : Grey;
            if (DI[5] != LastDI[5]) pictureBox6.Image = DI[5] ? Green : Grey;
            if (DI[6] != LastDI[6]) pictureBox7.Image = DI[6] ? Green : Grey;
            if (DI[7] != LastDI[7]) pictureBox8.Image = DI[7] ? Green : Grey;
            for (int i = 0; i < 8; i++)
            {
                LastDI[i] = DI[i];
            }

            //DO===================================================
            temp = Ch.GetOutputPort(0);
            for (int i = 0; i < 8; i++)
            {
                DO[i] = Convert.ToBoolean(temp & (1 << i));
            }
            if (DO[0] != LastDO[0]) pictureBox9.Image = DO[0] ? Green : Grey;
            if (DO[1] != LastDO[1]) pictureBox10.Image = DO[1] ? Green : Grey;
            if (DO[2] != LastDO[2]) pictureBox11.Image = DO[2] ? Green : Grey;
            if (DO[3] != LastDO[3]) pictureBox12.Image = DO[3] ? Green : Grey;
            if (DO[4] != LastDO[4]) pictureBox13.Image = DO[4] ? Green : Grey;
            if (DO[5] != LastDO[5]) pictureBox14.Image = DO[5] ? Green : Grey;
            if (DO[6] != LastDO[6]) pictureBox15.Image = DO[6] ? Green : Grey;
            if (DO[7] != LastDO[7]) pictureBox16.Image = DO[7] ? Green : Grey;
            for (int i = 0; i < 8; i++)
            {
                LastDO[i] = DO[i];
            }

            //AI===================================================
            label27.Text = Convert.ToString(Ch.GetAnalogInput(0));
            label28.Text = Convert.ToString(Ch.GetAnalogInput(1));
            label29.Text = Convert.ToString(Ch.GetAnalogInput(2));
            label30.Text = Convert.ToString(Ch.GetAnalogInput(3));
            label31.Text = Convert.ToString(Ch.GetAnalogInput(4));
            label32.Text = Convert.ToString(Ch.GetAnalogInput(5));

            //AO===================================================
            label33.Text = Convert.ToString(Ch.GetAnalogOutput(0));
            label34.Text = Convert.ToString(Ch.GetAnalogOutput(1));
        }

        //================================================================
        private void ErorMsg(COMException Ex)
        {
            string Str = "Error from " + Ex.Source + "\n\r";
            Str = Str + Ex.Message + "\n\r";
            Str = Str + "HRESULT:" + String.Format("0x{0:X}", (Ex.ErrorCode));
            MessageBox.Show(Str, "EnableEvent");
        }

        //================================================================
        private void mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if communication is open
            if (bConnected)
            {
                try
                {
                    MotorStateThr.Suspend();
                    Ch.CloseComm();
                    bConnected = false;
                    MakeDisconnectedState();
                }
                catch (COMException Ex)
                {
                    ErorMsg(Ex);
                }

            }
        }

        //================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            TakePhoto();
        }
        //================================================================
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_MOVE)))
                {
                    double velocity = Convert.ToDouble(textBox5.Text);
                    Ch.Jog(Ch.ACSC_AMF_VELOCITY, Axes[0], velocity);
                }
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_MOVE)))
                {
                    double velocity = Convert.ToDouble(textBox5.Text);
                    Ch.Jog(Ch.ACSC_AMF_VELOCITY, Axes[0], -velocity);
                }
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Ch.Kill(Axes[0]);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Ch.Kill(Axes[0]);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_MOVE)))
                {
                    double velocity = Convert.ToDouble(textBox5.Text);
                    Ch.Jog(Ch.ACSC_AMF_VELOCITY, Axes[1], velocity);
                }
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_MOVE)))
                {
                    double velocity = Convert.ToDouble(textBox5.Text);
                    Ch.Jog(Ch.ACSC_AMF_VELOCITY, Axes[1], -velocity);
                }
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Ch.Kill(Axes[1]);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Ch.Kill(Axes[1]);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button2_Click(object sender, EventArgs e)
        {
            double [] Points = new double[2];
            double velocity;

            try
            {
                Points[0] = Convert.ToDouble(textBox1.Text);
                Points[1] = Convert.ToDouble(textBox2.Text);
                velocity = Convert.ToDouble(textBox3.Text);

                Ch.SetVelocity(Axes[0], velocity);
                Ch.SetVelocity(Axes[1], velocity);

                Ch.ToPointM(0, Axes, Points);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button3_Click(object sender, EventArgs e)
        {
            double[] Points = new double[2];
            double velocity;

            try
            {
                Points[0] = Convert.ToDouble(textBox7.Text);
                Points[1] = Convert.ToDouble(textBox6.Text);

                velocity = Convert.ToDouble(textBox4.Text);
                Ch.SetVelocity(Axes[0], velocity);
                Ch.SetVelocity(Axes[1], velocity);

                Ch.ToPointM(Ch.ACSC_AMF_RELATIVE, Axes, Points);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (bConnected)
                {
                    try
                    {
                        Ch.SetFPosition(Axes[0], 0);
                        Ch.SetFPosition(Axes[1], 0);
                    }
                    catch (COMException Ex)
                    {
                        ErorMsg(Ex);
                    }
                }
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button10_Click(object sender, EventArgs e)
        {
             label25.Visible = false;
            try
            {
                switch (openFileDialog1.ShowDialog())
                {
                    case DialogResult.OK:
                        number_of_points = LoadPoints(openFileDialog1.FileName);
                        if (number_of_points > 0)
                        {
                            CM_Start.Enabled = true;
                            CM_Stop.Enabled = false;
                            CM_Restart.Enabled = true;
                        }
                        else
                        {
                            CM_Start.Enabled = false;
                            CM_Stop.Enabled = false;
                            CM_Restart.Enabled = false;
                        }
                        break;
                    case DialogResult.Cancel:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                
            }
        }

        //================================================================
        private int LoadPoints(string filename)
        {
            List<string> string_list = new List<string>();
            StreamReader sr = new StreamReader(filename);

            while (sr.Peek() != -1)
            {
                string str = sr.ReadLine().Trim();
                int g = str.Split(',').Length;
                if (str.Split(',').Length == 3)
                {
                    string_list.Add(str);
                }
            }

            if (string_list.Count > 16384)
            {
                label25.Text = "Numbers of Point must less than 16384.";
                label25.Visible = true;
                return -2;
            }

            try
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < string_list.Count; i++)
                {
                    string[] x_y_vel = string_list[i].Split(',');
                    continue_move_array[i].x = Convert.ToDouble(x_y_vel[0]);
                    continue_move_array[i].y = Convert.ToDouble(x_y_vel[1]);
                    continue_move_array[i].vel = Convert.ToDouble(x_y_vel[2]);

                    if (continue_move_array[i].vel <= MinimumSpeed) continue_move_array[i].vel = MinimumSpeed;
                    else if (continue_move_array[i].vel >= MaximumSpeed) continue_move_array[i].vel = MaximumSpeed;

                    dataGridView1.Rows.Add(Convert.ToString(i), continue_move_array[i].x, continue_move_array[i].y, continue_move_array[i].vel);
                }
            }
            catch (Exception Ex)
            {
                label25.Text = "File format is incorrect." + Ex.Message;
                label25.Visible = true;
                return -1;
            }

            label25.Text = "Total Points : " + Convert.ToString(string_list.Count);
            label25.Visible = true;
            continue_move_counter = 0;
            return string_list.Count;
        }
        //================================================================
        private int GenerateCalibrationPoints(int x_points, int y_points)
        {
            const int MINX = -261;
            const int MAXX = 261;
            const int MINY = -261;
            const int MAXY = 261;

            int k = 0;
            for (int i = 0; i < x_points; i++)
            {
                for (int j = 0; j < y_points; j++)
                {
                    calibration_move_array[k].x = MINX + i * (double)(MAXX - MINX) / (double)(x_points-1);
                    if (!Convert.ToBoolean(i & 0x01)) calibration_move_array[k].y = MINY + j * (double)(MAXY - MINY) / (double)(y_points-1);
                    else calibration_move_array[k].y = MAXY - j * (double)(MAXY - MINY) / (double)(y_points - 1);
                    calibration_move_array[k].vel = 100.0;
                    k++;
                }
            }

            return k;
        }
        //================================================================
        private void MoveToPoint(int num)
        {
            double [] Points = new double[2];
            double velocity;

            try
            {
                Points[0] = continue_move_array[num].x;
                Points[1] = continue_move_array[num].y;
                velocity = continue_move_array[num].vel;

                Ch.SetVelocity(Axes[0], velocity);
                Ch.SetVelocity(Axes[1], velocity);

                Ch.ToPointM(0, Axes, Points);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void MoveToPoint(double x, double y, double vel)
        {
            double[] Points = new double[2];
            double velocity;

            try
            {
                Points[0] = x;
                Points[1] = y;
                velocity = vel;

                Ch.SetVelocity(Axes[0], velocity);
                Ch.SetVelocity(Axes[1], velocity);

                Ch.ToPointM(0, Axes, Points);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void GetRealXY(double x, double y, out double _x, out double _y)
        {
            double delta_x = 0, delta_y = 0;













            _x = x + delta_x;
            _y = y + delta_y;
        }
        //================================================================
        private void button9_Click(object sender, EventArgs e)
        {
            double[] points = new double[2];
            points[0] = 0.0;
            points[1] = 0.0;
            CM_Load.Enabled = false;

            try
            {
                Ch.SetVelocity(Axes[0], 100.0);
                Ch.SetVelocity(Axes[1], 100.0);
                Ch.ToPointM(0, Axes, points);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void button11_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_INPOS) &&
                Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_INPOS))
            {
                label101.Text = string.Format("Start Time: {0:hh:mm:ss}", DateTime.Now);
                label102.Text = string.Format("Stop Time: {0:--:--:--}", DateTime.Now);

                continue_move = true;
                //continue_move_counter = 0;
                MoveToPoint(continue_move_counter);
                CM_Load.Enabled = false;
                CM_Start.Enabled = false;
                CM_Stop.Enabled = true;
                CM_Restart.Enabled = false;
                stop_time = Convert.ToInt32(textBox10.Text) * 10;
                repeat_times = Convert.ToInt32(textBox11.Text);
            }
        }

        //================================================================
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                continue_move = false;
                calibration_move = false;
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                Ch.KillAll();
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button13_Click(object sender, EventArgs e)
        {
            Ch.EnableEvent(Ch.ACSC_INTR_PEG);
            Ch.EnableEvent(Ch.ACSC_INTR_LOGICAL_MOTION_END);
            Ch.SetEventMask(Ch.ACSC_INTR_PEG, Ch.ACSC_MASK_AXIS_0);
            Ch.SetEventMask(Ch.ACSC_INTR_LOGICAL_MOTION_END, Ch.ACSC_MASK_AXIS_0);
        }
        //================================================================
        private void button14_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //================================================================
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            //This DO is for brake, don't use
            try
            {
                //Ch.SetOutput(0, 0, Convert.ToInt32(!DO[0]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 1, Convert.ToInt32(!DO[1]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 2, Convert.ToInt32(!DO[2]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 3, Convert.ToInt32(!DO[3]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        //================================================================
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 4, Convert.ToInt32(!DO[4]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 5, Convert.ToInt32(!DO[5]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void pictureBox15_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 6, Convert.ToInt32(!DO[6]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void pictureBox16_Click(object sender, EventArgs e)
        {
            try
            {
                Ch.SetOutput(0, 7, Convert.ToInt32(!DO[7]));
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                switch (saveFileDialog1.ShowDialog())
                {
                    case DialogResult.OK:
                        if (saveFileDialog1.FileName != "") Camera0.Image.Save(saveFileDialog1.FileName);
                        break;
                    case DialogResult.Cancel:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message + "Can't save to the file.");
            }            
        }
        //================================================================
        private void button15_Click_1(object sender, EventArgs e)
        {
            continue_move = false;
            CM_Load.Enabled = true;
            CM_Start.Enabled = true;
            CM_Stop.Enabled = false;
            CM_Restart.Enabled = true;

            try
            {
                Ch.KillAll();
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button16_Click(object sender, EventArgs e)
        {
            continue_move_counter = 0;
        }
        //================================================================
        private void pictureBox48_Click(object sender, EventArgs e)
        {

        }
        //================================================================
        private void tabPage5_Click(object sender, EventArgs e)
        {

        }
        //================================================================
        private void button10_Click_1(object sender, EventArgs e)
        {
            try
            {
                Ch.KillAll();
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button11_Click_1(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(MotorState[0] & Ch.ACSC_MST_INPOS) &&
                Convert.ToBoolean(MotorState[1] & Ch.ACSC_MST_INPOS))
            {
                //郎
                try
                {
                    switch (saveFileDialog1.ShowDialog())
                    {
                        case DialogResult.OK:
                            if (sw == null) sw = new StreamWriter(saveFileDialog1.FileName);

                            label95.Text = string.Format("Start Time: {0:hh:mm:ss}", DateTime.Now);
                            label96.Text = string.Format("Stop Time: {0:--:--:--}", DateTime.Now);

                            calibration_move = true;
                            calibration_move_counter = 0;
                            MoveToPoint(calibration_move_array[calibration_move_counter].x, calibration_move_array[calibration_move_counter].y, calibration_move_array[calibration_move_counter].vel);
                            CaliM_Start.Enabled = false;
                            CaliM_Stop.Enabled = true;
                            CaliM_Restart.Enabled = false;
                            break;
                        case DialogResult.Cancel:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);

                }
            }
        }
        //================================================================
        private void button16_Click_1(object sender, EventArgs e)
        {
            label96.Text = string.Format("Stop Time: {0:hh:mm:ss}", DateTime.Now);
            calibration_move = false;
            CaliM_Start.Enabled = true;
            CaliM_Stop.Enabled = false;
            CaliM_Restart.Enabled = true;

            try
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                Ch.KillAll();
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void button15_Click_2(object sender, EventArgs e)
        {
            calibration_move_counter = 0;
            label95.Text = string.Format("Stop Time: {0:hh:mm:ss}", DateTime.Now);
            label96.Text = string.Format("Stop Time: {0:--:--:--}", DateTime.Now);
        }
        //================================================================
        private void button11_Click_2(object sender, EventArgs e)
        {
            int x_points = Convert.ToInt32(textBox8.Text);
            int y_points = Convert.ToInt32(textBox9.Text);

            number_of_points = GenerateCalibrationPoints(x_points, y_points);
            label85.Text = "Total Points: " + Convert.ToString(number_of_points);
            CaliM_Start.Enabled = true;
        }
        //================================================================
        private void saveFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }
        //================================================================
        private void trackBarExposure_Scroll(object sender, EventArgs e)
        {
            if (ExposureBar0.Focused)
            {
                uEye.Defines.Status statusRet;
                Double dValue = ExposureBar0.Value * 0.32;

                string exposure_string = string.Format("{0:f02} ms", dValue);
                label19.Text = exposure_string;
                // set exposure
                statusRet = CameraC.Timing.Exposure.Set(dValue);
            }
        }
        //================================================================
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (ExposureBar1.Focused)
            {
                uEye.Defines.Status statusRet;
                Double dValue = ExposureBar1.Value * 0.32;

                string exposure_string = string.Format("{0:f02} ms", dValue);
                label86.Text = exposure_string;
                // set exposure
                statusRet = CameraF.Timing.Exposure.Set(dValue);
            }
        }
        //================================================================
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (ExposureBar2.Focused)
            {
                uEye.Defines.Status statusRet;
                Double dValue = ExposureBar2.Value * 0.32;

                string exposure_string = string.Format("{0:f02} ms", dValue);
                label87.Text = exposure_string;
                // set exposure
                statusRet = CameraA.Timing.Exposure.Set(dValue);
            }
        }
        //================================================================
        private void button15_Click_3(object sender, EventArgs e)
        {
        }
        //================================================================
        private void button15_Click_4(object sender, EventArgs e)
        {
            double[] points = new double[2];
            points[0] = -261.0;
            points[1] = -261.0;
            CM_Load.Enabled = false;

            try
            {
                Ch.SetVelocity(Axes[0], 100.0);
                Ch.SetVelocity(Axes[1], 100.0);
                Ch.ToPointM(0, Axes, points);
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }
        //================================================================
        private void AutoExposure0_CheckedChanged(object sender, EventArgs e)
        {
            ExposureBar0.Enabled = !AutoExposure0.Checked;
            CameraC.AutoFeatures.Software.Shutter.SetEnable(AutoExposure0.Checked);
        }
        //================================================================
        private void AutoExposure1_CheckedChanged(object sender, EventArgs e)
        {
            ExposureBar1.Enabled = !AutoExposure1.Checked;
            CameraF.AutoFeatures.Software.Shutter.SetEnable(AutoExposure1.Checked);
        }
        //================================================================
        private void AutoExposure2_CheckedChanged(object sender, EventArgs e)
        {
            ExposureBar2.Enabled = !AutoExposure2.Checked;
            CameraA.AutoFeatures.Software.Shutter.SetEnable(AutoExposure2.Checked);
        }
        //================================================================
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (AutoExposure0.Checked)
            {
                uEye.Defines.Status statusRet;

                uEye.Types.Range<Double> range;
                Camera0.Timing.Exposure.GetRange(out range);

                Double dValue;
                statusRet = Camera0.Timing.Exposure.Get(out dValue);

                Int32 s32Value = Convert.ToInt32((dValue - range.Minimum) / range.Increment + 0.0005);
                ExposureBar0.Value = s32Value;
            }
        }

        private void button16_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (bConnected)
                {
                    try
                    {
                        Ch.SetRPosition(Axes[0], 0);
                        Ch.SetRPosition(Axes[1], 0);
                    }
                    catch (COMException Ex)
                    {
                        ErorMsg(Ex);
                    }
                }
            }
            catch (COMException Ex)
            {
                ErorMsg(Ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            Ch.DisableM(Axes);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Ch.Enable(0);
            Ch.Enable(1);   
        }

        private void button19_Click(object sender, EventArgs e)
        {
        }

        private void CM_AutoCapture_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

       

      
        //================================================================
    }
}
