using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


//user ใช้งานธรรมดา


namespace Database_ระบบร้านจำหน่ายอุปกรณ์คอมพ์ 
{
    public partial class Form3 : MetroFramework.Forms.MetroForm
    {

        MySqlConnection connect = new MySqlConnection("host=localhost;user=root;password=123456789;database=computer_shop");
        MySqlCommand command;
        MySqlDataAdapter adapter;
       
        //รับพารามิเตอร์ที่ส่งมาจาก form1 เพื่อแสดงข้อมูลของผู้ที่ login เข้ามา
        public Form3(String status_login,String version)
        {
            InitializeComponent();
            metroLabel8.Text = status_login;

            
            metroLabel10.Text = version;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;//lock ไม่ให้ขยายจอแบบเต็ม
           
            dataGridViewUpdate();//แสดงข้อมูลในฐานาข้อมูลตอนที่เข้ามายัง form นี้ทันที

            //สั่งปิดไม่ให้สามารถใช้งานในส่วนของ metroTextBox ได้
            metroTextBox1.Enabled = false;
            metroTextBox2.Enabled = false;
            metroTextBox3.Enabled = false;
            metroTextBox4.Enabled = false;
            label2.Hide();
            //richTextBox1.Enabled  = false;


            //add ข้อมูลเข้าไปใน Combobox
            metroComboBox1.Items.Add("กำลังซ่อม");
            metroComboBox1.Items.Add("ซ่อมเสร็จแล้ว");
            metroComboBox1.Items.Add("กำลังรออะไหล่");
            metroComboBox1.Items.Add("");

        }

        //method สั่งปิดโปรแกรมโดยกด x ที่ form สามารถ kill process ทิ้งได้ 
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Dispose(true);
            Application.Exit();
        }


        //method metroPanel1_Paint เปลี่ยน BorderStyle ของ metroPanel1 เป็น FixedSingle
        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

            metroPanel1.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;

        }
        

        public void openConnection()
        {
            if (connect.State == ConnectionState.Closed)
            {

                connect.Open();

            }
        }
        public void closeConnection()
        {

            if (connect.State == ConnectionState.Open)
            {

                connect.Close();

            }
        }

        //method textbox ให้ส่งพารามิเตอร์ชุดข้อความใน textbox นี้ไปยัง method searchData()
        private void metroTextBox6_TextChanged(object sender, EventArgs e)
        {
            searchData(metroTextBox6.Text);
        }
        private void searchData(String search) //รับพารามิเตอร์มาจาก metroTextBox6_TextChanged
        {

            openConnection();
            String adapt = "SELECT * FROM computer_shop.customer WHERE Name LIKE '%" + search + "%'"; //คำสั่ง SQL search
            adapter = new MySqlDataAdapter(adapt, connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            closeConnection();
        }



        public void executeQuery(String query) {

            try
            {
                openConnection();
                command = new MySqlCommand(query, connect);
                if (command.ExecuteNonQuery() == 1)
                {

                    MessageBox.Show("ดำเนินการเสร็จสิ้น", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    MessageBox.Show("ดำเนินการไม่สำเร็จ\nเนื่องจากไม่มีข้อมูลในฐานข้อมูล", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {

                closeConnection();
            }
        }

        

        //method อัพเดทข้อมูลใน dataGridView แต่เอาแค่ข้อมูลบางตัวเท่านั้น
        public void dataGridViewUpdate()
        {

            openConnection();
            string adapt = "SELECT *FROM computer_shop.customer ";
            adapter = new MySqlDataAdapter(adapt, connect);

            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            closeConnection();

            //ID,Name,Surname,Issus,Phone,Status_fix,Price

        }

        //method button กลับไปยังหน้า login แบบถามก่อน
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการกลับไปยังหน้า Login หรือไม่", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();

            }
        }


        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            
            metroTextBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            metroTextBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            metroTextBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            //richTextBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            richTextBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            metroTextBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            metroComboBox1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            metroTextBox5.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();

        }

        
        //เพิ่มข้อมูลเกี่ยวกับ สถานะซ่อมในรูปแบบของการอัพเดทข้อมูล
        private void metroButton2_Click(object sender, EventArgs e)
        {
           // double price = double.Parse(metroTextBox5.Text);
            try
            {

                if ((metroComboBox1.SelectedIndex == 0) || (metroComboBox1.SelectedIndex == 1) || (metroComboBox1.SelectedIndex == 2)|| (metroComboBox1.SelectedIndex==3))
                {

                    string update_fix = "UPDATE computer_shop.customer SET Issus='" + richTextBox2.Text + "',Status_fix='" + metroComboBox1.SelectedItem + "',Price='" + metroTextBox5.Text + "' WHERE ID='" + metroTextBox1.Text + "'";
                    executeQuery(update_fix);
                }
                else {
                    string update_fix = "UPDATE computer_shop.customer SET Issus='" + richTextBox2.Text + "',Price='" + metroTextBox5.Text + "' WHERE ID='" + metroTextBox1.Text + "'";
                    executeQuery(update_fix);

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally {

                dataGridViewUpdate();
            }
        }


        private void metroButton4_Click(object sender, EventArgs e)
        {
            dataGridViewUpdate();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            String address = dataGridView1.CurrentRow.Cells[3].Value.ToString(); ;
            if (metroComboBox1.Text == "ซ่อมเสร็จแล้ว")
            {
                e.Graphics.DrawString("ID ลูกค้า : " + metroTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 200));
                e.Graphics.DrawString("วัน/เวลา : " + DateTime.Now, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 200));
                e.Graphics.DrawString("ชื่อลูกค้า : " + metroTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 250));
                e.Graphics.DrawString("นามสกุล : " + metroTextBox3.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 250));
                e.Graphics.DrawString("ที่อยู่ : " + address, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 300));
                e.Graphics.DrawString("ปัญหาที่เป็น : " + richTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 350));
                e.Graphics.DrawString("เบอร์โทรศัพท์ : " + metroTextBox4.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 400));
                e.Graphics.DrawString("สถานะ : " + metroComboBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Green, new Point(350, 400));
                e.Graphics.DrawString("ค่าบริการ : " + metroTextBox5.Text + " บาท", new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 450));
                e.Graphics.DrawString(label2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(35, 550));
                e.Graphics.DrawString("ขอบคุณที่ใช้บริการ หีคอม บริการเย็ดสดแตกในใส่คอมครับ", new Font("Angsana new", 18, FontStyle.Regular), Brushes.Black, new Point(95, 575));
            }
            else if (metroComboBox1.Text == "กำลังซ่อม")
            {

                e.Graphics.DrawString("ID ลูกค้า : " + metroTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 200));
                e.Graphics.DrawString("วัน/เวลา : " + DateTime.Now, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 200));
                e.Graphics.DrawString("ชื่อลูกค้า : " + metroTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 250));
                e.Graphics.DrawString("นามสกุล : " + metroTextBox3.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 250));
                e.Graphics.DrawString("ที่อยู่ : " + address, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 300));
                e.Graphics.DrawString("ปัญหาที่เป็น : " + richTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 350));
                e.Graphics.DrawString("เบอร์โทรศัพท์ : " + metroTextBox4.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 400));
                e.Graphics.DrawString("สถานะ : " + metroComboBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Red, new Point(350, 400));
                e.Graphics.DrawString("ค่าบริการ : " + metroTextBox5.Text + " บาท", new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 450));
                e.Graphics.DrawString(label2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(35, 550));

            }
            else if (metroComboBox1.Text == "กำลังรออะไหล่")
            {

                e.Graphics.DrawString("ID ลูกค้า : " + metroTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 200));
                e.Graphics.DrawString("วัน/เวลา : " + DateTime.Now, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 200));
                e.Graphics.DrawString("ชื่อลูกค้า : " + metroTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 250));
                e.Graphics.DrawString("นามสกุล : " + metroTextBox3.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 250));
                e.Graphics.DrawString("ที่อยู่ : " + address, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 300));
                e.Graphics.DrawString("ปัญหาที่เป็น : " + richTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 350));
                e.Graphics.DrawString("เบอร์โทรศัพท์ : " + metroTextBox4.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 400));
                e.Graphics.DrawString("สถานะ : " + metroComboBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Yellow, new Point(350, 400));
                e.Graphics.DrawString("ค่าบริการ : " + metroTextBox5.Text + " บาท", new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 450));
                e.Graphics.DrawString(label2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(35, 550));

            }
        }

        private void metroLabel10_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        
    }
}
