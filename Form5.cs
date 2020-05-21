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

namespace Database_ระบบร้านจำหน่ายอุปกรณ์คอมพ์
{
    public partial class Form5 : MetroFramework.Forms.MetroForm
    {
        MySqlConnection connect = new MySqlConnection("host=localhost;user=root;password=123456789;database=computer_shop");
        MySqlCommand command;
        MySqlDataAdapter adapter;

        public Form5()
        {
            InitializeComponent();
        }
        

        private void Form5_Load(object sender, EventArgs e)
        {
            dataGridViewupdate();
            metroComboBox1.Items.Add("Admin");
            metroComboBox1.Items.Add("User");
            metroComboBox1.Items.Add("Waitting");
        }

        public void openConnection() {

            if (connect.State == ConnectionState.Closed) {

                connect.Open();
            }

        }

        public void closeConnection() {


            if (connect.State == ConnectionState.Open) {

                connect.Close();
            }

        }


        public void dataGridViewupdate() {

            openConnection();
            string query = "SELECT * FROM computer_shop.admin ";
            adapter = new MySqlDataAdapter(query, connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            closeConnection();

        }
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            metroTextBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            metroTextBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            metroTextBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            metroTextBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            metroTextBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            metroComboBox1.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

        }
        public void executeQuery(String query) {

            try
            {
                openConnection();
                command = new MySqlCommand(query, connect);
                if (command.ExecuteNonQuery() == 1)
                {

                    MessageBox.Show("บันทึกข้อมูลสำเร็จ","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {

                    MessageBox.Show("บันทึกไม่สำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("ไม่สามารถกระทำการใดๆได้" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {

                closeConnection();
            }

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if ((metroTextBox1.Text == "" || metroTextBox1.Text == null) && (metroTextBox2.Text == "" || metroTextBox2.Text == null) )
            {
                MessageBox.Show("ERROR \nไม่สามารถข้ามการใส่ ID ลูกค้าได้", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {

                if (MessageBox.Show("ต้องการเพิ่มข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string insert_data_login = "INSERT INTO computer_shop.admin (ID,Password,Name,surname,Phone,Status) VALUES ('" + metroTextBox1.Text + "','" + metroTextBox2.Text + "','" + metroTextBox3.Text + "','" + metroTextBox4.Text + "','" + metroTextBox5.Text + "','" + metroComboBox1.Text + "')";
                    executeQuery(insert_data_login);
                    dataGridViewupdate();
                }
                
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการอัพเดทข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string update_data_login = "UPDATE computer_shop.admin SET Password='" + metroTextBox2.Text + "',Name='" + metroTextBox3.Text + "',surname='" + metroTextBox4.Text + "',Phone='" + metroTextBox5.Text + "',Status='" + metroComboBox1.Text + "' WHERE ID='" + metroTextBox1.Text + "'";
                executeQuery(update_data_login);
                dataGridViewupdate();
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการลบข้อมูลหรือไม่", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string delete_data_login = "DELETE FROM computer_shop.admin WHERE ID='" + metroTextBox1.Text + "'";
                executeQuery(delete_data_login);
                dataGridViewupdate();
            }
        }
    }
}

