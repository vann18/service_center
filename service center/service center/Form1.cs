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
    public partial class Form1 : Form
    {
        string connect = "server=localhost;user=root;database=servis_center;port=3306;password=1111;";
        MySqlConnection myConnection = new MySqlConnection("server=localhost;user=root;database=service_center;port=3306;password=1111;");
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)// заполнение таблицы
        {
            DataSet ds = new DataSet();
            string query = "Select service_center.order.id_order, service_center.client.fio, service_center.client.mail, service_center.technique.name_technique, service_center.client.telefon ,service_center.order.accomplishment " +
                "from service_center.order inner join service_center.client on service_center.order.id_client = service_center.client.id inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique " +
                $"where service_center.order.return_date = '{DateTime.Now.ToShortDateString()}'; ";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string query = "Select service_center.order.id_order, service_center.client.fio, service_center.client.mail, service_center.technique.name_technique, service_center.client.telefon,service_center.order.accomplishment  " +
                "from service_center.order inner join service_center.client on service_center.order.id_client = service_center.client.id inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique " +
                $"where service_center.order.return_date = '{dateTimePicker1.Value.ToShortDateString()}'; ";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query = $"Select accomplishment " +
                $"From service_center.order " +
                $"Where service_center.order.id_order='{dataGridView1.CurrentRow.Cells[0].Value.ToString()}';";
            myConnection.Open();
            MySqlCommand command = new MySqlCommand(query, myConnection);
            var reader = command.ExecuteReader();
            reader.Read();
            int gotovnost = int.Parse(reader["accomplishment"].ToString());
            reader.Close();
            myConnection.Close();
            if (gotovnost != 1)
            {
                DialogResult result = MessageBox.Show("Заказ не готов.Хотите сделать его готовым", "Уведомление", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    MySqlCommand cmd = new MySqlCommand(connect);
                    string sql = $"update service_center.order " +
                        $"set service_center.order.accomplishment = 1 " +
                        $"where service_center.order.id_order = '{dataGridView1.CurrentRow.Cells[0].Value.ToString()}';";
                    myConnection.Open();
                    cmd = new MySqlCommand(sql, myConnection);
                    cmd.ExecuteNonQuery();
                    myConnection.Close();
                    button4_Click(this, EventArgs.Empty);
                }
            }
            if (gotovnost == 1)
            {
                string query1 = $"select service_center.order.id_order, service_center.client.fio, service_center.client.mail, service_center.technique.name_technique " +
                    $"from service_center.order inner join service_center.client on service_center.order.id_client = service_center.client.id " +
                    $"inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique " +
                    $"where service_center.order.id_order='{dataGridView1.CurrentRow.Cells[0].Value.ToString()}';";
                myConnection.Open();
                MySqlCommand command1 = new MySqlCommand(query1, myConnection);
                var reader1 = command1.ExecuteReader();
                reader1.Read();
                string fio = reader1["fio"].ToString();
                string mail = reader1["mail"].ToString();
                string id_order = reader1["id_order"].ToString();
                string name_technique = reader1["name_technique"].ToString();
                reader1.Close();
                myConnection.Close();

                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("petrtimohin40@gmail.com", "Сервин сломал-чини");
                // кому отправляем
                MailAddress to = new MailAddress(mail);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Сервисный центр";
                // текст письма
                m.Body = $"<h5>Уважаемы {fio} ваш заказ {id_order} {name_technique} готов.</h5><br><h5>С уважением сервисный центр Сломал-Чини.</h5>";
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

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 newForm3 = new Form3();
            newForm3.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 newForm4 = new Form4();
            newForm4.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm2 = new Form2();
            newForm2.Show();
        }
    }
    }

