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
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;

namespace service_center
{
    public partial class Form4 : Form
    {
        public readonly string TemplateFileName = @"D:\ВГПК\КУРСАЧ БД\Новый сервисный центр\service center\service center\bin\Debug\check1.docx";
        string connect = "server=localhost;user=root;database=service_center;port=3306;password=1111;";
        MySqlConnection myConnection = new MySqlConnection("server=localhost;user=root;database=service_center;port=3306;password=1111;");
        public Form4()
        {
            InitializeComponent();
        }
        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)//метод для экспорта в ворд
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }//для появления формы добавления
        private void Form4_Load(object sender, EventArgs e)
        {   //заказ 
            DataSet ds = new DataSet();
            string query = "select service_center.order.id_order as 'Номер заказа',service_center.client.fio as 'Фамилия',service_center.technique.name_technique as 'Название техники',service_center.order.date_of_admission as 'Дата приема',service_center.order.return_date as 'Дата возврата', service_center.order.price as 'Цена',service_center.order.accomplishment as 'Выполнение'" +
                            "FROM service_center.order inner join service_center.client on service_center.order.id_client = service_center.client.id inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique; ";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();

            //заполнение combobox с клиентами
            
            string comand = "Select service_center.client.fio From service_center.client;";
            MySqlCommand myCommand1 = new MySqlCommand(comand, myConnection);

            myConnection.Open();
            MySqlDataReader myReader;
            myReader = myCommand1.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    comboBox2.Items.Add(myReader.GetString(0));
                }
            }
            finally
            {
                myReader.Close();
                myConnection.Close();
            }

            //заполнение combobox с техникой
            string comand2 = "Select service_center.technique.name_technique From service_center.technique;";
            MySqlCommand myCommand2 = new MySqlCommand(comand2, myConnection);

            myConnection.Open();
            MySqlDataReader myReader2;
            myReader2 = myCommand2.ExecuteReader();
            try
            {
                while (myReader2.Read())
                {
                    comboBox3.Items.Add(myReader2.GetString(0));
                }
            }
            finally
            {
                myReader2.Close();
                myConnection.Close();
            }

            //заполнение таблицы с готовыми


        }//появление формы
        private void button9_Click(object sender, EventArgs e)
        {
                //удалить заказ
                myConnection.Open();
                string sql_query = $"DELETE FROM service_center.order WHERE id_order = {dataGridView1.CurrentCell.Value.ToString()}";
                MySqlCommand cmd = new MySqlCommand(sql_query, myConnection);
                cmd.ExecuteNonQuery();
                myConnection.Close();
                button10_Click(this, EventArgs.Empty);
        }//удаление
        private void button10_Click(object sender, EventArgs e)
        {
            //обновить заказ
            DataSet ds = new DataSet();
            string query = "select service_center.order.id_order as 'Номер заказа',service_center.client.fio as 'Фамилия',service_center.technique.name_technique as 'Название техники',service_center.order.date_of_admission as 'Дата приема',service_center.order.return_date as 'Дата возврата', service_center.order.price as 'Цена',service_center.order.accomplishment as 'Выполнение'" +
                            "FROM service_center.order inner join service_center.client on service_center.order.id_client = service_center.client.id inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique; ";
            MySqlCommand myCommand = new MySqlCommand(query, myConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
            myConnection.Open();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].ReadOnly = true;
            myConnection.Close();
        }//обновить
        private void textBox13_TextChanged(object sender, EventArgs e)
        {   //поиск
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
                    System.Data.DataTable dt = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT id_order as 'Номер заказа', id_client as 'Номер клиента', id_technique as 'Номер техники', date_of_admission as 'Дата поступления', return_date as 'Дата возврата', price as 'Цена', accomplishment as 'Готовность' FROM service_center.order WHERE id_order LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt = new System.Data.DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
            if (comboBox1.Text == "Фамилия клиента")
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
                    System.Data.DataTable dt = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT service_center.order.id_order as 'Номер заказа', service_center.client.fio as 'Фамилия',service_center.technique.name_technique as 'Название техники', service_center.order.date_of_admission as 'Дата приема',service_center.order.return_date as 'Дата возврата',service_center.order.price as 'Цена',service_center.order.accomplishment as 'Готовность' " +
                        "FROM service_center.order inner join service_center.client on service_center.order.id_client = service_center.client.id inner join service_center.technique on service_center.order.id_technique = service_center.technique.id_technique " +
                        "WHERE service_center.client.fio LIKE'" + textBox13.Text + "%'", myConnection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt = new System.Data.DataTable();
                    dt.Load(reader);
                    dataGridView1.DataSource = dt;
                    myConnection.Close();
                }
            }
            
            
        }//поиск
        private void button2_Click(object sender, EventArgs e)
        {            
            int idclient;
            int idtechnique;            
            string query =$"Select id " +
                $"From service_center.client " +
                $"Where fio='{comboBox2.Text}';";
            myConnection.Open();
            MySqlCommand command = new MySqlCommand(query, myConnection);
            var reader = command.ExecuteReader();
            reader.Read();
            idclient =int.Parse(reader["id"].ToString());
            reader.Close();            
            string query1 =$"Select id_technique " +
                $"From service_center.technique " +
                $"Where name_technique='{comboBox3.Text}';";
            MySqlCommand command1 = new MySqlCommand(query1, myConnection);
            var reader1 = command1.ExecuteReader();
            reader1.Read();
            idtechnique = int.Parse(reader1["id_technique"].ToString());
            myConnection.Close();
            reader1.Close();            
            //добавление заказа
            string sql = "INSERT INTO service_center.order (id_client, id_technique, date_of_admission,return_date,price,accomplishment)" +
                                             $"VALUES ('{idclient}'," +
                                             $"'{idtechnique}'," +
                                             $"'{DateTime.Now.ToShortDateString()}'," +
                                             $"'{dateTimePicker1.Value.ToShortDateString()}'," +
                                             $"'{Convert.ToInt32(textBox3.Text)}'," +
                                             $"'0')";//по дефолту в готовности 0 (1- выполнено)
            
            MySqlCommand cmd = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            cmd.ExecuteNonQuery();
            myConnection.Close();
            button10_Click(this, EventArgs.Empty);
            //groupBox1.Visible = false;
            comboBox2.Text = "";
            comboBox3.Text = "";
            textBox3.Text = "";
            
        }//нажатие на добавить
        private void button1_Click(object sender, EventArgs e)
        {
                DataSet ds = new DataSet();
                string query = "select service_center.order.id_order as 'Номер заказа',service_center.order.id_client as 'Номер клиента',service_center.order.id_technique as 'Номер техники',service_center.order.date_of_admission as 'Дата приема',service_center.order.return_date as 'Дата возврата', service_center.order.price as 'Цена',service_center.order.accomplishment as 'Выполнение'" +
                                "FROM service_center.order; ";
                MySqlCommand myCommand = new MySqlCommand(query, myConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, myConnection);
                myConnection.Open();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
                myConnection.Close();
                button4.Enabled = true;

           
        }// перевод в режим редактирования
        private void button3_Click(object sender, EventArgs e)
        {
            var id_order= Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            string name_technique= Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            string fio= Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            var price= dataGridView1.CurrentRow.Cells[5].Value.ToString();
            string date_of_admission= Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value.ToString());
            string return_date= Convert.ToString(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            string query = $"select service_center.technique.breakage " +
                $"from service_center.technique " +
                $"Where service_center.technique.name_technique= '{name_technique}';";
            myConnection.Open();
            MySqlCommand command = new MySqlCommand(query, myConnection);
            var reader = command.ExecuteReader();
            reader.Read();
            var breakage = reader["breakage"].ToString();

            var wordapp = new Word.Application();
            wordapp.Visible = false;
            var wordDocument = wordapp.Documents.Open(TemplateFileName);
            var wordApp = new Word.Application();
            wordapp.Visible = false;
            try
            {
                //var wordDocument = wordapp.Documents.Open(TemplateFileName);
                ReplaceWordStub("{id_order}", Convert.ToString(id_order), wordDocument);
                ReplaceWordStub("{name_technique}", Convert.ToString(name_technique), wordDocument);
                ReplaceWordStub("{breakage}", breakage, wordDocument);
                ReplaceWordStub("{fio}", Convert.ToString(fio), wordDocument);
                ReplaceWordStub("{price}", Convert.ToString(price), wordDocument);
                ReplaceWordStub("{date_of_admission}", Convert.ToString(date_of_admission), wordDocument);
                ReplaceWordStub("{return_date}", Convert.ToString(return_date), wordDocument);
                Random rnd = new Random();
                wordDocument.SaveAs2($@"D:\ВГПК\КУРСАЧ БД\Новый сервисный центр\service center\service center\bin\Debug\check{rnd.Next(9586)}.docx");
               // wordApp.Visible = true;
            }
            catch
            {
                MessageBox.Show("ошибка");
            }
        }//импорт в ворд
        private void button4_Click(object sender, EventArgs e)// сохранить заказ
        {
            //сохранит заказ
            MySqlConnection myConnection = new MySqlConnection(connect);
            MySqlCommand cmd = new MySqlCommand(connect);
            string sql = $" UPDATE service_center.order SET order.id_order='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value + "',order.id_client='" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value + "',order.id_technique='" +
                         dataGridView1[2, dataGridView1.CurrentRow.Index].Value + "',order.date_of_admission='" + dataGridView1[3, dataGridView1.CurrentRow.Index].Value + "',order.return_date='" + dataGridView1[4, dataGridView1.CurrentRow.Index].Value + "',order.price='" + dataGridView1[5, dataGridView1.CurrentRow.Index].Value + "',order.accomplishment='" + dataGridView1[6, dataGridView1.CurrentRow.Index].Value + "'where id_order=" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            myConnection.Open();
            cmd = new MySqlCommand(sql, myConnection);
            cmd.ExecuteNonQuery();
            myConnection.Close();
            button10_Click(this, EventArgs.Empty);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string query = $"Select breakage " +
                $"From service_center.technique inner join service_center.order on service_center.technique.id_technique = service_center.order.id_technique " +
                $"Where service_center.order.id_order = '{dataGridView1.CurrentRow.Cells[0].Value.ToString()}'; ; ";
            myConnection.Close();
            myConnection.Open();
            MySqlCommand command = new MySqlCommand(query, myConnection);
            var reader = command.ExecuteReader();
            reader.Read();
            // MessageBox.Show(reader["service_center.technique.breakage"].ToString());
            textBox6.Text = reader["breakage"].ToString();
            reader.Close();
            myConnection.Close();
        }//для отображения текста в большом textbox

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox13.Text = "";
        }
    }
}
