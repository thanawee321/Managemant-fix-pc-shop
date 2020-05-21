using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Database_ระบบร้านจำหน่ายอุปกรณ์คอมพ์
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        
        
        MySqlConnection connect = new MySqlConnection("host=localhost;user=root;password=123456789;database=computer_shop");
        MySqlDataAdapter adapter;
        DataTable table = new DataTable();
        public String status;
        String version = "Dev by รวมโปรแกรม windows xp,7,8,10 V17.3.8";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.MaximizeBox = false; // ห้ามขยายหน้าจอ
            metroLabel10.Text = version;
            metroTextBox2.UseSystemPasswordChar = true;//ซ่อน password ให้เป็นในรูปแบบ ********


        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Dispose(true);
            System.Windows.Forms.Application.Exit();
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {
            metroPanel1.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
        }

       

        public void openConnection() {

            if (connect.State == ConnectionState.Closed) {

                connect.Open();
            }
        
        }

        public void closeConnection() {

            if (connect.State == ConnectionState.Open)
            {

                connect.Close();
            }

        }

        //method นำข้อมูลที่รับจากหน้า Form มาตรวจสอบว่ามีข้อมูลในฐานข้อมูลมั้ย? เพื่อใช้ในการ login 
        public void dataAdapterLogin(String adapt)
        {
            openConnection();
            adapter = new MySqlDataAdapter(adapt, connect);
            adapter.Fill(table);


            if (table.Rows.Count > 0) //นับแถวทั้งหมด
            {
                string showStatus;

                /*this.Hide();
                Form2 form2 = new Form2("TEST");
                form2.Show();*/

                status = table.Rows[0][5].ToString().Trim(); //แถวที่ต้องการนำข้อมูลออกไปใช้ (ข้อมูลจาก Database)

                //ถ้าตรวจสอบแล้วตรงกับ Admin ก็เข้าเงื่อนไขด่านล่าง
                if (status == "Admin")
                {
                                                //นำข้อมุลที่อยู่ในตารางในช่องที่ต้องการออกมาแสดง                   
                    showStatus = "ชื่อผู้ใช้งาน : " + table.Rows[0][2].ToString().Trim() + " \nสถานะ : " + status;
                    MessageBox.Show("เข้าใช้งานสำเร็จ", "STATUE [ ADMIN ]", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    this.Hide();
                    Form2 form2 = new Form2(showStatus,version);//ส่งตัวแปรพารามิเคอร์ showStatus ที่เก้บข้อมูลของผู้ที่ login ไปให้ form2
                    form2.Show();//แสดง form2

                }

                else if (status == "User")

                {
                    showStatus = "ชื่อผู้ใช้งาน : " + table.Rows[0][2].ToString().Trim() + " \nสถานะ : " + status;
                    MessageBox.Show("เข้าใช้งานสำเร็จ", "STATUE [ USER ]", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    this.Hide();
                    Form3 form3 = new Form3(showStatus,version);//ส่งตัวแปรพารามิเคอร์ showStatus ที่เก้บข้อมูลของผู้ที่ login ไปให้ form2
                    form3.Show();//แสดง form3

                }
                else if (status == "Waitting")
                {

                    MessageBox.Show("รอยืนยันสถานะ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    metroTextBox1.Text = "";
                    metroTextBox2.Text = "";

                }

           }
            else

            {

                MessageBox.Show("เข้าใช้งานไม่สำเร็จ\nกรุณาตรวจสอบชื่อผู้ใช้และรหัสผ่านให้ถูกต้อง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                metroTextBox1.Text = "";
                metroTextBox2.Text = "";

                /*MessageBox.Show("เข้าใช้งานสำเร็จ [ADMIN]");
                this.Hide();
                form2.Show();*/
            }

        }

        //method แสดง password ให้เห็น (โดยได้ทำการHide password ให้เป็น ****** ที่ method Form1_Load())
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                metroTextBox2.UseSystemPasswordChar = false;
            }
            else {
                metroTextBox2.UseSystemPasswordChar = true;
            }
        }

        //method ปุ่ม login 
        private void metroButton1_Click(object sender, EventArgs e)
        {
            

                
                string login = "SELECT * FROM computer_shop.admin WHERE ID='" + metroTextBox1.Text + "' AND Password='" + metroTextBox2.Text + "'";//เลือกเข้ามูลทั้งแถวแต่จะใช้แค่ ID,Password เพื่อให้สามารถกูข้อมูลได้หมดในนี้ได้
                dataAdapterLogin(login);
            
            
           

        }

        


        //method ปุ่ม Register
        private void metroButton2_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://localhost:8080/test_project/colorlib-regform-14/colorlib-regform-14/Theme.html");
            System.Diagnostics.Process.Start("http://localhost:8080/phpMyAdmin/sql.php?db=computer_shop&table=customer&pos=0");
       
          

        }

        private void metroLabel10_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }
    }
    

}
