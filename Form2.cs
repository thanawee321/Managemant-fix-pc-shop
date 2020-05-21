using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MySql.Data.MySqlClient;

namespace Database_ระบบร้านจำหน่ายอุปกรณ์คอมพ์
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {

        MySqlConnection connect = new MySqlConnection("host=localhost;user=root;password=123456789;database=computer_shop");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        


        public Form2(String login_status,String version)//รับตัวแปรพารามิเตอร์จาก form1
        {
            InitializeComponent();
            metroLabel7.Text = login_status;

            
            this.MaximizeBox = false;
            metroLabel10.Text = version;

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            //System.Windows.Forms.Application.Exit();
           
            dataGridviewUpdate();

            metroComboBox1.Items.Add("กำลังซ่อม");
            metroComboBox1.Items.Add("ซ่อมเสร็จแล้ว");
            metroComboBox1.Items.Add("กำลังรออะไหล่");
            metroComboBox1.Items.Add("");
            label2.Hide();



            /* metroTextBox2.Enabled = false;
             metroTextBox3.Enabled = false;
             metroTextBox4.Enabled = false;
             metroTextBox5.Enabled = false;
             richTextBox1.Enabled = false;
             richTextBox2.Enabled = false;*/

            

        }
      
        
        //method ของ metropanel1 setting BorderStyle เป็น FixedSingle
        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {
            metroPanel1.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
        }

        //ปิดโปรแกรมจากการกด x ที่ form แล้ว kill process ทิ้งไปเลย
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Dispose(true);
            System.Windows.Forms.Application.Exit();
        }

        //method สั่งเปิดฐานข้อมูล
        public void openConnection() {

            if (connect.State == ConnectionState.Closed) {

                connect.Open();
            }
        }

        //method สั่งปิดฐานข้อมูล
        public void closeConnection() {

            if (connect.State == ConnectionState.Open) {

                connect.Close();
            }
        }
       

        //method textbox ให้ส่งพารามิเตอร์ชุดข้อความใน textbox นี้ไปยัง method searchData()
        private void metroTextBox6_TextChanged(object sender, EventArgs e)
        {
            searchData(metroTextBox6.Text);
        }
        public void searchData(String search)   //รับพารามิเตอร์มาจาก metroTextBox6_TextChanged 
        {                                      

            openConnection();
            String adapt = "SELECT * FROM computer_shop.customer WHERE Name LIKE '%" + search + "%'";//คำสั่ง SQL ใช้ search หาข้อมูล
            adapter = new MySqlDataAdapter(adapt, connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            closeConnection();
        }

        //method ดูข้อมูล dataGridView แบบ update
        public void dataGridviewUpdate() {

            openConnection();
            string adapt = "SELECT * FROM computer_shop.customer";
            adapter = new MySqlDataAdapter(adapt,connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            
            closeConnection();

        }
        //method executeQuery ข้อมูลเพื่อนำข้อมูลไปเก็ยในฐานข้อมูล
        public void executeQuery(String query) {

            try
            {
                openConnection();
                command = new MySqlCommand(query, connect);
                if (command.ExecuteNonQuery() == 1)
                {

                    MessageBox.Show("ดำเนินการเสร็จสิ้น","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {

                    MessageBox.Show("ดำเนินการไม่สำเร็จ\nเนื่องจากไม่มีข้อมูลในฐานข้อมูลหรือไม่ได้มีการใส่ข้อมูลใดๆลงไป","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด"+ex.Message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
            finally {

                closeConnection();
            }

        }

        //method dataGridView1_MouseClick กดคลิกที่ข้อมูลที่เราต้องการในตาราง สามารถนำไปแสดงที่ textBox ตามที่ต้องการ
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            metroTextBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            metroTextBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            metroTextBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            richTextBox1.Text  = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            richTextBox2.Text  = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            metroTextBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            metroComboBox1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            dataGridView1.ReadOnly = true;//ให้อ่านข้อมูลใน dataGridview อย่างเดียวไม่สามารถ edit ใน dataGridview ได้ 

        }

        //method แสดงข้อมูลใน dataGridview แบบ update
        private void metroButton4_Click(object sender, EventArgs e)
        {
            dataGridviewUpdate();
            
        }

        //method button บันทึก มีการยืนยันก่อน insert ข้อมูลเข้าไปในฐานข้อมูล
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == null || metroTextBox1.Text == "")
            {

                MessageBox.Show("ERROR \nไม่สามารถข้ามการใส่ ID ลูกค้าได้", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {

                if (MessageBox.Show("ต้องการเพิ่มข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {


                    string insert_data_customer = "INSERT INTO computer_shop.customer (ID,Name,Surname,Address,Issus,Phone,Status_fix,Price) VALUES ('" + metroTextBox1.Text + "','" + metroTextBox2.Text + "','" + metroTextBox3.Text + "','" + richTextBox1.Text + "','" + richTextBox2.Text + "','" + metroTextBox4.Text + "','" + metroComboBox1.Text + "','" + metroTextBox5.Text + "')";
                    executeQuery(insert_data_customer);
                    dataGridviewUpdate();

                    /*string insert_data_customer = "INSERT INTO computer_shop.customer (ID,Name,Surname,Address,Issus,Phone,Status_fix,Price) VALUES ('" + metroTextBox1.Text + "','ตัวทดลอง','ตัวทดลอง','ตัวทดลอง','ตัวทดลอง','ตัวทดลอง','" + metroComboBox1.Text + "','0')";
                    executeQuery(insert_data_customer);
                    dataGridviewUpdate();*/

                }
            }
        }

        //method button อัพเดท มีการยืนยันก่อน update ข้อมูลเข้าไปในฐานข้อมูล
        private void metroButton2_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("ต้องอัพเดทข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string update_data_customer = "UPDATE computer_shop.customer SET Name='" + metroTextBox2.Text + "',Surname='" + metroTextBox3.Text + "',Address='" + richTextBox1.Text + "',Issus='" + richTextBox2.Text + "',Phone='" + metroTextBox4.Text + "',Status_fix='"+metroComboBox1.Text+"',Price='"+metroTextBox5.Text+"' WHERE ID ='" + metroTextBox1.Text + "'";
                executeQuery(update_data_customer);
                dataGridviewUpdate();
            }
           
        }

        //method button ลบ มีการยืนยันก่อน delete ออกจากฐานข้อมูล
        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการลบข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string delete_data_customer = "DELETE FROM computer_shop.customer WHERE ID='" + metroTextBox1.Text + "'";
                executeQuery(delete_data_customer);
                dataGridviewUpdate();
            }
        }

        //method button ลบสถานะซ่อมของลูกค้า มีการยืนยันก่อน delete ออกจากฐานข้อมูลแต่เขียนให้อยู่ในรูปแบบ update เพราะมันลบข้อมูลบาง column ไม่ได้
        private void metroButton7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการลบข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string update_data_statusfix_customer = "UPDATE computer_shop.customer SET Status_fix ='' WHERE ID ='" + metroTextBox1.Text + "'";
                executeQuery(update_data_statusfix_customer);

                dataGridviewUpdate();
            }
        }

        //method ปุ่มกลับไปยังหน้า login แบบถามก่อน
        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการกลับไปยังหน้า Login หรือไม่", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();

            }
        }

        //method เช็คerror ห้ามกรอกข่อความในเบอร์โทรแหละห้ามใส่เกิน10ตัว
        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {
            int valueint;
            String message = "ห้ามมีตัวอักษรและห้ามใส่เลขเกิน10หลัก";
            if (!int.TryParse(metroTextBox4.Text , out valueint) || (metroTextBox4.Text.Length > 10)) {

                MessageBox.Show(message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                metroTextBox4.Text = "0";

            }
        }

        //method ทำการสร้าง printDocument ให้ไปอยู่ใน printPreviewDialog
        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[6].Value.ToString() == null || dataGridView1.CurrentRow.Cells[6].Value.ToString() == "")
            {
                MessageBox.Show("กรุณาเลือกสถานะการซ่อม","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else {
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
            }
            
        }

        //method setting printDocument กำหนดตัวอักษรที่จะปริ้นอยากจะให้ปริ้นอะไรออกมาก็มาทำในนี้
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            DateTime date = DateTime.Now;

            if (metroComboBox1.Text == "ซ่อมเสร็จแล้ว")
            {

                e.Graphics.DrawString("ID ลูกค้า : " + metroTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 200));
                e.Graphics.DrawString("วัน/เวลา : " + date, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 200));
                e.Graphics.DrawString("ชื่อลูกค้า : " + metroTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 250));
                e.Graphics.DrawString("นามสกุล : " + metroTextBox3.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 250));
                e.Graphics.DrawString("ที่อยู่ : " + richTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 300));
                e.Graphics.DrawString("ปัญหาที่เป็น : " + richTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 350));
                e.Graphics.DrawString("เบอร์โทรศัพท์ : " + metroTextBox4.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 400));
                e.Graphics.DrawString("สถานะ : " + metroComboBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Green, new Point(350, 400));
                e.Graphics.DrawString("ค่าบริการ : " + metroTextBox5.Text +" บาท", new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 450));
                e.Graphics.DrawString(label2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(35, 550));
                e.Graphics.DrawString("ขอบคุณที่ใช้บริการ", new Font("Angsana new", 18, FontStyle.Regular), Brushes.Black, new Point(95, 575));
            }
            else if (metroComboBox1.Text == "กำลังซ่อม")
            {

                e.Graphics.DrawString("ID ลูกค้า : " + metroTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 200));
                e.Graphics.DrawString("วัน/เวลา : " + date, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 200));
                e.Graphics.DrawString("ชื่อลูกค้า : " + metroTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 250));
                e.Graphics.DrawString("นามสกุล : " + metroTextBox3.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 250));
                e.Graphics.DrawString("ที่อยู่ : " + richTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 300));
                e.Graphics.DrawString("ปัญหาที่เป็น : " + richTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 350));
                e.Graphics.DrawString("เบอร์โทรศัพท์ : " + metroTextBox4.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 400));
                e.Graphics.DrawString("สถานะ : " + metroComboBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Red, new Point(350, 400));
                e.Graphics.DrawString("ค่าบริการ : " + metroTextBox5.Text + " บาท", new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 450));
                e.Graphics.DrawString(label2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(35, 550));

            }
            else if (metroComboBox1.Text == "กำลังรออะไหล่") {

                e.Graphics.DrawString("ID ลูกค้า : " + metroTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 200));
                e.Graphics.DrawString("วัน/เวลา : " + date, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 200));
                e.Graphics.DrawString("ชื่อลูกค้า : " + metroTextBox2.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 250));
                e.Graphics.DrawString("นามสกุล : " + metroTextBox3.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(350, 250));
                e.Graphics.DrawString("ที่อยู่ : " + richTextBox1.Text, new Font("Angsana new", 24, FontStyle.Regular), Brushes.Black, new Point(70, 300));
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

        private void metroButton8_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            long id_key = rand.Next(00000000, 100000001);

            metroTextBox1.Text = id_key.ToString();

        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
        }
    }
}
