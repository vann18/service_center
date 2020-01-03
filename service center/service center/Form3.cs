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

namespace service_center
{
    public partial class Form3 : Form
    {
        string connect = "server=localhost;user=root;database=service_center;port=3306;password=1111;";
        MySqlConnection myConnection = new MySqlConnection("server=localhost;user=root;database=service_center;port=3306;password=1111;");
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {   //техника
            DataSet ds = new DataSet();
            string query = "SELECT id_technique as Номер, name_technique as Название, year_of_manufacture as 'Год выпуска', breakage as Поломка FROM technique";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //техника обновить
            DataSet ds = new DataSet();
            string query = "SELECT id_technique as Номер, name_technique as Название, year_of_manufacture as 'Год выпуска', breakage as Поломка FROM technique";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                //удалить техника
                myConnection.Open();
                string sql_query = $"DELETE FROM technique WHERE id_technique = {dataGridView1.CurrentCell.Value.ToString()}";
                MySqlCommand cmd = new MySqlCommand(sql_query, myConnection);
                cmd.ExecuteNonQuery();
                myConnection.Close();
                button10_Click(this, EventArgs.Empty);
            }
            catch (Exception)
            {   
                DialogResult result = MessageBox.Show("Нельзя удалить клиента если у него есть заказ. Хотите удалить заказ?", "Ошибка", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    myConnection.Close();
                    myConnection.Open();
                    string sql_query = $"DELETE service_center.order " +
                        $"FROM service_center.order " +
                        $"inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique " +
                        $"WHERE service_center.technique.id_technique = {dataGridView1.CurrentCell.Value.ToString()}";
                    MySqlCommand cmd = new MySqlCommand(sql_query, myConnection);
                    cmd.ExecuteNonQuery();
                    myConnection.Close();
                    button9_Click(this, EventArgs.Empty);
                }
            } 
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //сохранит техника
            MySqlConnection myConnection = new MySqlConnection(connect);
            MySqlCommand cmd = new MySqlCommand(connect);
            string sql = $" UPDATE service_center.technique SET technique.id_technique='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value + "',technique.name_technique='" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value + "',technique.year_of_manufacture='" +
                         dataGridView1[2, dataGridView1.CurrentRow.Index].Value + "',technique.breakage='" + dataGridView1[3, dataGridView1.CurrentRow.Index].Value + "'where id_technique=" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            myConnection.Open();
            cmd = new MySqlCommand(sql, myConnection);
            cmd.ExecuteNonQuery();
        }

        private void button2_Click(object sender, EventArgs e)
        {   //добавить техника
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text=="")
            {
                MessageBox.Show("Заполните все поля", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //добавить техника
                string sql = "INSERT INTO technique (name_technique, year_of_manufacture, breakage)" +
                                             $"VALUES ( '{textBox1.Text}', '{textBox2.Text}','{textBox3.Text}')";
                MySqlCommand cmd = new MySqlCommand(sql, myConnection);
                myConnection.Open();
                cmd.ExecuteNonQuery();
                myConnection.Close();
                button10_Click(this, EventArgs.Empty);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                groupBox1.Visible = false;
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            //поиск
            if (comboBox1.Text == "Номер")
            {
                if (textBox13.Text == "")
                {

                    DataSet ds = new DataSet();
                    dataGridView1.DataSource = ds;
                    button10_Click(this, EventArgs.Empty);//кнопка обновления
                }
                else
                {
                    myConnection.Open();
                    DataTable dt = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT id_technique as Номер, name_technique as Название, year_of_manufacture as 'Год выпуска', breakage as Поломка FROM technique WHERE id_technique LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
            if (comboBox1.Text == "Название")
            {
                if (textBox13.Text == "")
                {
                    DataSet ds = new DataSet();
                    dataGridView1.DataSource = ds;
                    button10_Click(this, EventArgs.Empty);//кнопка обновления
                }
                else
                {
                    myConnection.Open();
                    DataTable dt = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT id_technique as Номер, name_technique as Название, year_of_manufacture as 'Год выпуска', breakage as Поломка FROM technique WHERE name_technique LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
            if (comboBox1.Text == "Год выпуска")
            {
                if (textBox13.Text == "")
                {
                    DataSet ds = new DataSet();
                    dataGridView1.DataSource = ds;
                    button10_Click(this, EventArgs.Empty);//кнопка обновления
                }
                else
                {
                    myConnection.Open();
                    DataTable dt = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT id_technique as Номер, name_technique as Название, year_of_manufacture as 'Год выпуска', breakage as Поломка FROM technique WHERE year_of_manufacture LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }

            }
            if (comboBox1.Text == "Поломка")
            {
                if (textBox13.Text == "")
                {
                    DataSet ds = new DataSet();
                    dataGridView1.DataSource = ds;
                    button10_Click(this, EventArgs.Empty);//кнопка обновления
                }
                else
                {
                    myConnection.Open();
                    DataTable dt = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT id_technique as Номер, name_technique as Название, year_of_manufacture as 'Год выпуска', breakage as Поломка FROM technique WHERE breakage LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
        }
    }
}
