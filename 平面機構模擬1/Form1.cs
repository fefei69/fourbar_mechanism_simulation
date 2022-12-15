using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 平面機構模擬_fourbar
{
    public partial class Form1 : Form
    {
        double x;
        int R = 50;
        //請輸入固定軸樞座標
        int[] M = { 1, 1 };
        int[] Q = { 5, 2 };
        double A_X, A_Y, A_R , Q_X , Q_Y , Q_R;
        //intialize input angle theta2
        double theta2 = 0;
        double MQ;
        int dir = 5;
        
        

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        

        double MA =2, AB=3, QB=4; 
      
        public Form1()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Minimum = -10;
            chart1.ChartAreas[0].AxisX.Maximum = 10;
            chart1.ChartAreas[0].AxisY.Minimum = -10;
            chart1.ChartAreas[0].AxisY.Maximum = 10;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisX.Minimum = -10;
            chart2.ChartAreas[0].AxisX.Maximum = 10;
            chart2.ChartAreas[0].AxisY.Minimum = -10;
            chart2.ChartAreas[0].AxisY.Maximum = 10;
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        }

        private void Form1_load(object sender, EventArgs e)
        {
       
            timer1.Tick += timer1_Tick_1;
            timer1.Interval = 10;

        }
        private void timer1_Tick_1(object sender, EventArgs e)
            
        {
            MQ = Math.Sqrt((M[0] - Q[0]) ^ 2 - (M[1] - Q[1]) ^ 2);
            //請輸入桿長
            double[] anArray = { MQ, MA, AB, QB };
            double MaxLinkage = anArray.Max();
            double MinLinkage = anArray.Min();
            double sum = anArray.Sum();
            double D_linkage = (MaxLinkage + MinLinkage) * 2 - sum;
            double theta1 = Math.Atan2(M[1] - Q[1], M[0] - Q[0]) * (180 / Math.PI); //in degrees
            //要動的角度 theta2 用徑度表示(radian)
            double theta3 = 0;
            double theta4 = 0;
            A_X = M[0] + MA * Math.Cos(theta2);
            A_Y = M[1] + MA * Math.Sin(theta2);
            A_R = AB;
            Q_X = Q[0];
            Q_Y = Q[1];
            Q_R = QB;
            //L M N A B C
            double L = Math.Pow(A_X, 2) + Math.Pow(A_Y, 2) + Math.Pow(Q_X, 2) + Math.Pow(Q_Y, 2) - Math.Pow(A_R, 2) + Math.Pow(Q_R, 2) - 2 * (A_X * Q_X + A_Y * Q_Y);
            double m = 2 * Q_R * (A_X - Q_X);
            double N = 2 * Q_R * (A_Y - Q_Y);
            double A = L + m;
            double B = -2 * N;
            double C = L - m;
            //判別式解A B C -> D = B^2 - 4AC 
            double D = Math.Pow(B, 2) - 4 * A * C;
            double x1, x2;
            x1 = 0;
            x2 = 0;
            if (D == 0)
            {
                x1 = -C / B;
                x2 = 1000000; //x2 -> infinite
                
            }
            else if (D > 0)
            {
                x1 = (-B + Math.Sqrt(D)) / (2 * A);
                x2 = (-B - Math.Sqrt(D)) / (2 * A);
            }
            else
            {
                Console.WriteLine("D < 0 ,no solution");
            }
            Console.WriteLine("x1");
            Console.WriteLine(x1);
            Console.WriteLine("x2");
            Console.WriteLine(x2);
            //Console.WriteLine("x1:",x1,x2);
            double beta1 = 2 * Math.Atan(x1); //in radian
            double beta2 = 2 * Math.Atan(x2); //in radian

            //兩圓交點方程式
            double xc1 = Q_X + Q_R * Math.Cos(beta1);
            double xc2 = Q_X + Q_R * Math.Cos(beta2);
            double yc1 = Q_Y + Q_R * Math.Sin(beta1);
            double yc2 = Q_Y + Q_R * Math.Sin(beta2);
            double theta3_1 = (xc1 - A_X) / (yc1 - A_Y);
            double theta3_2 = (xc2 - A_X) / (yc2 - A_Y);
            double theta4_1 = beta1;
            double theta4_2 = beta2;

            if (chart1.Series[0].Points.Count > 1)
            {
               chart1.Series[0].Points.RemoveAt(0);
               chart1.Series[1].Points.RemoveAt(0);
               chart1.Series[5].Points.RemoveAt(0);

               chart2.Series[0].Points.RemoveAt(0);
               chart2.Series[1].Points.RemoveAt(0);
               chart2.Series[5].Points.RemoveAt(0);

            }
            //chart1
            //函數 x 為角度 
            chart1.Series[0].Points.AddXY(M[0],M[1]);
            chart1.Series[0].Points.AddXY(A_X, A_Y);
            chart1.Series[1].Points.AddXY(A_X, A_Y);
            chart1.Series[1].Points.AddXY(xc1, yc1);
            chart1.Series[5].Points.AddXY(xc1, yc1);
            chart1.Series[5].Points.AddXY(Q_X, Q_Y);
            
            //x axis y axis
            chart1.Series[2].Points.AddXY(-10, 0);
            chart1.Series[2].Points.AddXY(10, 0);
            chart1.Series[3].Points.AddXY(0, 10);
            chart1.Series[3].Points.AddXY(0, -10);
            //箭頭
            chart1.Series[4].Points.AddXY(0, 10);
            chart1.Series[4].Points.AddXY(10, 0);
            //接地符號
            chart1.Series[6].Points.AddXY(Q[0], Q[1]);
            chart1.Series[6].Points.AddXY(M[0], M[1]);
            //chart2
            //函數 x 為角度 
            chart2.Series[0].Points.AddXY(M[0], M[1]);
            chart2.Series[0].Points.AddXY(A_X, A_Y);
            chart2.Series[1].Points.AddXY(A_X, A_Y);
            chart2.Series[1].Points.AddXY(xc2, yc2);
            chart2.Series[5].Points.AddXY(xc2, yc2);
            chart2.Series[5].Points.AddXY(Q_X, Q_Y);

            //x axis y axis
            chart2.Series[2].Points.AddXY(-10, 0);
            chart2.Series[2].Points.AddXY(10, 0);
            chart2.Series[3].Points.AddXY(0, 10);
            chart2.Series[3].Points.AddXY(0, -10);
            //箭頭
            chart2.Series[4].Points.AddXY(0, 10);
            chart2.Series[4].Points.AddXY(10, 0);
            //接地符號
            chart2.Series[6].Points.AddXY(Q[0], Q[1]);
            chart2.Series[6].Points.AddXY(M[0], M[1]);
            if (dir == 1)
            {
                theta2 -= 1;
            }
            else
            {
                theta2 += 1;
            }
            //Console.WriteLine(dir);


            if (chart1.Series[0].Points.Count > 0)
            {
                chart1.Series[0].Points.RemoveAt(0);
                chart1.Series[1].Points.RemoveAt(0);
                chart1.Series[5].Points.RemoveAt(0);

                chart2.Series[0].Points.RemoveAt(0);
                chart2.Series[1].Points.RemoveAt(0);
                chart2.Series[5].Points.RemoveAt(0);

            }

            //設定x軸y軸大小
            /*chart1.ChartAreas[0].AxisX.ScaleView.Position = 100;
            chart1.ChartAreas[0].AxisY.ScaleView.Position = 100;
            chart1.ChartAreas[0].AxisX.Minimum = -100;
            chart1.ChartAreas[0].AxisX.Maximum = 100;
            chart1.ChartAreas[0].AxisY.Minimum = -100;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            */


        
            //chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
            //chart1.ChartAreas[0].AxisX.Maximum = x;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dir = 1;//順時鐘轉
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            dir = -1;//逆時鐘轉
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();

            }
        }
        


        }
        


    
}
