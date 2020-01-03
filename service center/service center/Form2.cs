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
using System.Net.Mail;

namespace service_center
{

    public partial class Form2 : Form
    {
        string connect = "server=localhost;user=root;database=service_center;port=3306;password=1111;";
        MySqlConnection myConnection = new MySqlConnection("server=localhost;user=root;database=service_center;port=3306;password=1111;");
        
        
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 f)
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            //клиент
            string query = "SELECT id as Номер, fio as ФИО, telefon as Телефон, mail as Почта FROM client";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {   
            //обновить клиенты
            DataSet ds = new DataSet();
            string query = "SELECT id as Номер, fio as ФИО, telefon as Телефон, mail as Почта FROM client";
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
                //удалить клиент 
                myConnection.Open();
                string sql_query = $"DELETE FROM client WHERE id  = {dataGridView1.CurrentCell.Value.ToString()}";
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
                        $"inner join service_center.client on service_center.order.id_client = service_center.client.id " +
                        $"WHERE service_center.client.id = {dataGridView1.CurrentCell.Value.ToString()}";
                    MySqlCommand cmd = new MySqlCommand(sql_query, myConnection);
                    cmd.ExecuteNonQuery();
                    myConnection.Close();
                    button9_Click(this, EventArgs.Empty);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {   //сохранить клиент 
            MySqlConnection myConnection = new MySqlConnection(connect);
            MySqlCommand cmd = new MySqlCommand(connect);
            string sql = $" UPDATE service_center.client SET client.id='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value + "',client.fio='" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value + "',client.telefon='" +
                         dataGridView1[2, dataGridView1.CurrentRow.Index].Value + "',client.mail='" + dataGridView1[3, dataGridView1.CurrentRow.Index].Value +"'where id=" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            myConnection.Open();
            cmd = new MySqlCommand(sql, myConnection);
            cmd.ExecuteNonQuery();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            //поиск
            if(comboBox1.Text=="Номер")
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

                    MySqlCommand cmd = new MySqlCommand("SELECT id as Номер, fio as ФИО, telefon as Телефон, mail as Почта FROM client WHERE id LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    dt = new DataTable();

                    dt.Load(reader);

                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
            if (comboBox1.Text == "ФИО")
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

                    MySqlCommand cmd = new MySqlCommand("SELECT id as Номер, fio as ФИО, telefon as Телефон, mail as Почта FROM client WHERE fio LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    dt = new DataTable();

                    dt.Load(reader);

                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
            if (comboBox1.Text == "Телефон")
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

                    MySqlCommand cmd = new MySqlCommand("SELECT id as Номер, fio as ФИО, telefon as Телефон, mail as Почта FROM client WHERE telefon LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    dt = new DataTable();

                    dt.Load(reader);

                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }

            }
            if (comboBox1.Text == "Почта")
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

                    MySqlCommand cmd = new MySqlCommand("SELECT id as Номер, fio as ФИО, telefon as Телефон, mail as Почта FROM client WHERE mail LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    dt = new DataTable();

                    dt.Load(reader);

                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //проверить почту
            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите почту и попробуйте снова", "Ошибка почты", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //int kod = rnd.Next(9999);
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("petrtimohin40@gmail.com", "Сервин сломал-чини");
                // кому отправляем
                MailAddress to = new MailAddress(textBox3.Text);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Сервисный центр";
                // текст письма
                m.Body = $"<h1>Ваша почта используется для связи с клиентом</h1>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль

                smtp.Credentials = new System.Net.NetworkCredential("petrtimohin40@gmail.com", "vann142001");
                smtp.EnableSsl = true;
                smtp.Send(m);
                MessageBox.Show("Сообщение отправлено на вашу почту", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
                if(textBox1.Text=="" && textBox2.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //добавить техника
                    string sql = "INSERT INTO client (fio,telefon,mail)" +
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
    }
    }

