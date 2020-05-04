using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;


namespace CDMA
{
    public partial class Form1 : Form
    {
        Pen p = new Pen(Color.Black, 5); //画笔
        //public Bitmap pbmap;       
        Graphics g;             //声明区间对象
        Color color;            //储存调色板颜色
        int font = 2;             //储存字体大小
        int tool;               //记录tool工具的选择
        int timeflag = 0;

        DataTable state = new DataTable(); // 实例化状态数据表
        DataTable employee = new DataTable(); // 实例化用户身份数据表
        String[] stringName = new string[50]; //保存数据节点的名称
        bool loginOK = false; //登入标志位  false表示未登入
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            button2.ForeColor = Color.Red;
            color = Color.Blue;   //初始让color为黑色

            init();                 //初始化功能按钮
            tool = 1;               //工具

        }

        private void button1_Click(object sender, EventArgs e)      //搜索串口开关
        {
            SearchAndAdd(serialPort1, comboBox1);               //搜索串口
        }

        private void button2_Click(object sender, EventArgs e)  //打开串口
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                }
                catch { }
                button2.Text = "打开";
                button2.ForeColor = Color.Red;   //将"关闭串口"的按钮字体变为红色
            }
            else
            {
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.Open();
                    button2.Text = "关闭";
                    button2.ForeColor = Color.Green;   //将"打开串口"的按钮字体变为绿色
                }
                catch
                {
                    MessageBox.Show("连接失败", "错误");  //错误事弹出提示框
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);//必须手动添加事件处理程序
            //            this.toolStripStatusLabel2.Text = System.DateTime.Now.ToString(); //获取时间

        }
        private void SearchAndAdd(SerialPort MyPort, ComboBox MyBox)   //搜索串口
        {
            timer1.Stop();
            string Buffer;                                              //缓存
            MyBox.Items.Clear();                                        //清空ComboBox内容
            for (int i = 1; i < 10; i++)                                //循环
            {
                try                                                     //核心原理是依靠try和catch完成遍历
                {
                    Buffer = "COM" + i.ToString();
                    MyPort.PortName = Buffer;
                    MyPort.Open();                                      //如果失败，后面的代码不会执行
                    MyBox.Items.Add(Buffer);                            //打开成功，添加至下俩列表
                    MyPort.Close();                                     //关闭
                }
                catch
                {
                }
            }
            timer1.Start();
        }
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)//串口数据接收事件
        {
            timer1.Stop();
            int temp;
            string str = serialPort1.ReadExisting();//字符串方式读
            if (int.TryParse(str, out temp))
            {
                temp = Convert.ToInt32(str);
            }
            p.Width = font; //画笔粗细

            switch (tool)                          //选择画图工具
            {
                default:
                    break;
            }
            timeflag = 0;
            timer1.Start();
        }
        private void 新建图形ToolStripMenuItem_Click(object sender, EventArgs e)  //新建图形按钮
        {

        }

        private void 保存文件ToolStripMenuItem_Click(object sender, EventArgs e)   //保存文件
        {

        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e) //打开文件
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();  //声明一个打开对象
            openFileDialog1.Multiselect = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)  //鼠标按下
        {
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)  //鼠标松开
        {

        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)  //定时器中断函数
        {
            timer1.Stop();    //定时器关闭
            if (loginOK)
            {   //以下是控制指示灯的闪烁状态
                if (timeflag == 1)
                {
                    timeflag = 0;
                    pictureBox2.Image = Image.FromFile(@"red.png");
                    pictureBox3.Image = Image.FromFile(@"green.png");
                    pictureBox4.Image = Image.FromFile(@"green.png");
                    pictureBox5.Image = Image.FromFile(@"yellow.png");
                    pictureBox6.Image = Image.FromFile(@"green.png");
                    pictureBox7.Image = Image.FromFile(@"green.png");
                    pictureBox8.Image = Image.FromFile(@"green.png");
                    pictureBox9.Image = Image.FromFile(@"green.png");
                    pictureBox10.Image = Image.FromFile(@"green.png");
                }
                else
                {
                    timeflag = 1;
                    pictureBox2.Image = Image.FromFile(@"gray.png");
                    pictureBox3.Image = Image.FromFile(@"gray.png");
                    pictureBox4.Image = Image.FromFile(@"gray.png");
                    pictureBox5.Image = Image.FromFile(@"gray.png");
                    pictureBox6.Image = Image.FromFile(@"gray.png");
                    pictureBox7.Image = Image.FromFile(@"gray.png");
                    pictureBox8.Image = Image.FromFile(@"gray.png");
                    pictureBox9.Image = Image.FromFile(@"gray.png");
                    pictureBox10.Image = Image.FromFile(@"gray.png");
                }
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            try
            {
                serialPort1.Open();
            }
            catch { }

            timer1.Start();     //定时器开启

        }
        private void init()         //复位按键
        {
            string Buffer;                                              //缓存
            comboBox2.Items.Clear();                                   //清空ComboBox内容
            for (int i = 1; i < 9; i++)                                //循环
            {
                Buffer = i.ToString();
                comboBox2.Items.Add(i);
            }
            //初始化各个数据点的名称
            textBox17.Text = "通讯基站A";
            textBox18.Text = "通讯基站B";
            textBox19.Text = "通讯基站C";
            textBox20.Text = "通讯基站D";
            textBox21.Text = "通讯基站E";
            textBox22.Text = "通讯基站F";
            textBox23.Text = "通讯基站G";
            textBox24.Text = "通讯基站H";
            textBox25.Text = "通讯基站I";

            comboBox3.Items.Clear();                                 //清空ComboBox内容
            comboBox3.Items.Add("4800");                             //添加至下俩列表
            comboBox3.Items.Add("9600");                             //添加至下俩列表
            comboBox3.Items.Add("14400");                            //添加至下俩列表
            comboBox3.Items.Add("19200");                            //添加至下俩列表
            comboBox3.Items.Add("28800");                            //添加至下俩列表
            comboBox3.Items.Add("38400");                            //添加至下俩列表
            comboBox3.Items.Add("57600");                            //添加至下俩列表
            comboBox3.Items.Add("115200");                           //添加至下俩列表

            //数据库链接
            String connsql = "server=.;database=CDMA;integrated security=SSPI"; // 数据库连接字符串,database设置为自己的数据库名，以Windows身份验证
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    String sql = "select * from stateTab"; // 查询语句
                    SqlDataAdapter myda = new SqlDataAdapter(sql, conn); // 实例化适配器
                    myda.Fill(state); // 保存数据 

                    sql = "select * from employeeTab"; // 查询语句
                    myda = new SqlDataAdapter(sql, conn); // 实例化适配器
                    myda.Fill(employee); // 保存数据 
                    //conn.Close(); // 关闭数据库连接
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message, "出现错误");
            }
        }

        public void SetCursor(Bitmap cursor, Point hotPoint)        //设置鼠标函数
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width,
            cursor.Height);

            this.Cursor = new Cursor(myNewCursor.GetHicon());

            g.Dispose();
            myNewCursor.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.Text;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (loginOK)
            {
                if (textBox26.Text.Trim() != String.Empty)//联系方式不等于空
                {
                    richTextBox1.AppendText("\n\n以上信息已发送给: ");
                    richTextBox1.AppendText(textBox26.Text.ToString() + "\n");
                    richTextBox1.AppendText("----------------------------------\n\n");

                }
                else
                {
                    richTextBox1.AppendText("\n\n请输入技术员的联系方式！！！");
                }
            }
            else
            {
                richTextBox1.Text = "\n\n  请先登入系统!";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i;
            bool flag = false;
            if (button4.Text.Equals("登入"))
            {
                if (textBox1.Text.Trim() == String.Empty)
                    textBox1.Text = "请输入工号";
                //if (textBox2.Text.Trim() == String.Empty)
                //    textBox2.Text = "请输入密码";
                if (textBox1.Text.Trim() != String.Empty && textBox2.Text.Trim() != String.Empty)//工号不等于空
                {
                    for (i = 0; i < employee.Rows.Count; i++)
                    {
                        if (textBox1.Text.Equals(employee.Rows[i][0].ToString()))//如果查询到的编号等于设定的编号就显示出来
                        {
                            if (textBox2.Text.Equals(employee.Rows[i][1].ToString()))//密码验证成功
                            {
                                loginOK = true;     //登入成功标志位开启
                                button4.Text = "注销";
                                flag = true;
                                break;
                            }
                            //textBox2.Text = "密码不正确!";
                        }
                    }
                    if (!flag)
                    {
                        textBox1.Text = "工号或密码不存在!";
                    }
                }
            }
            else
            {
                loginOK = false;     //登入成功标志位关闭
                button4.Text = "登入";
                //textBox1.Text = ""; //工号输入文本清空
                textBox2.Text = "";//密码输入文本清空
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loginOK)
            {
                richTextBox3.Text = "";//清空当前richTextBox3显示的数据
                richTextBox2.Text = "";//清空当前richTextBox3显示的数据
                //重新换取数据节点的名称
                stringName[1] = textBox17.Text;
                stringName[2] = textBox18.Text;
                stringName[3] = textBox19.Text;
                stringName[4] = textBox20.Text;
                stringName[5] = textBox21.Text;
                stringName[6] = textBox22.Text;
                stringName[7] = textBox23.Text;
                stringName[8] = textBox24.Text;
                stringName[9] = textBox25.Text;

                int pos = int.Parse(comboBox2.Text);
                richTextBox3.AppendText(stringName[pos] + "的情况如下: \n");
                for (int i = 0; i < state.Rows.Count; i++)
                {
                    if (comboBox2.Text.Equals(state.Rows[i][0].ToString()))//如果查询到的编号等于设定的编号就显示出来
                    {
                        for (int j = 1; j < state.Columns.Count; j++)//循环输出这一行数据
                        {
                            string str = state.Rows[i][j].ToString();
                            richTextBox3.AppendText(str + " ");
                        }
                        //显示维修信息
                        richTextBox2.AppendText("\n\n" + state.Rows[i][1].ToString());
                        richTextBox2.AppendText("   维修完毕");
                        richTextBox3.AppendText(" \n");//换行
                    }
                }
                //以下是变量整个表
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    for (int j = 1; j < dt.Columns.Count; j++)
                //    {
                //        string str = dt.Rows[i][j].ToString();
                //        richTextBox3.AppendText(str + " ");
                //    }
                //    richTextBox3.AppendText(" \n");
                //}

            }
            else
            {
                richTextBox3.Text = "\n\n  请先登入系统!";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
