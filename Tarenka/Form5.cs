using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tarenka
{
    public partial class Form5 : Form
    {
        static string conncetionString = "Host=localhost;Port=5432;Database=Homework;Username=postgres;Password=postgres";

        NpgsqlConnection connection = new NpgsqlConnection(conncetionString);

        private DataSet ds = new DataSet();

        private DataSet dq = new DataSet();

        private DataTable dt = new DataTable();


        Form2 form2 = new Form2();

        string[] id = new string[10];

        string prepod;
        string _number_gr;
        string _sokr_name;
        string day;
        string aud;
        String result;
        int resInt;
        string id_gr;

        public Form5()
        {
            InitializeComponent();

            Date();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        void Date()
        {
            connection.Open();

            string sql = $"SELECT concat(surname, name, middle_name, '                        ', id_pr) from prepod";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["concat"]);
                comboBox1.Items.Add(id[i]);
            }

            number_gr();
            sokr_name();

            connection.Close();
        }
        public void id_gruppy(String name)
        {
            connection.Open();

            string sql = $"select id_gr from gruppy where nomer_gr = '{name}'";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[1];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["id_gr"]);
                id_gr = id[0];
            }


            connection.Close();
        }


        void number_gr()
        {
            string sql = $"select nomer_gr from gruppy";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["nomer_gr"]);
                comboBox2.Items.Add(id[i]);
            }

        }

        void sokr_name()
        {
            string sql = $"select disc from rasp";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["disc"]);
                comboBox3.Items.Add(id[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            prepod = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            result = prepod.Substring(prepod.Length - 6);
            resInt = Convert.ToInt32(result);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _number_gr = comboBox2.Items[comboBox2.SelectedIndex].ToString();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            day = comboBox4.Items[comboBox4.SelectedIndex].ToString();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            aud = comboBox5.Items[comboBox5.SelectedIndex].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            id_gruppy(comboBox2.Text);

            string para = textBox3.Text;

            insertRecord(_number_gr, day, para, comboBox3.Text, resInt, aud, Convert.ToInt32(id_gr));

        }



        public void insertRecord(String nomer_gr, String dayn, String para, String disc, int id_pr, String aud, int id_gr)
        {
            connection.Open();

            try
            {
                // Create insert command.
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO " +
                    "rasp (nomer_gr, dayn, para, disc, id_pr, aud, id_gr) VALUES(:nomer_gr, :dayn, " +
                    ":para, :disc, :id_pr, :aud, :id_gr)", connection);

                // Add paramaters.
                command.Parameters.Add(new NpgsqlParameter("nomer_gr",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("dayn",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("para",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("disc",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("id_pr",
                    NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("aud",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("id_gr",
                    NpgsqlTypes.NpgsqlDbType.Integer));


                // Prepare the command.
                command.Prepare();

                // Add value to the paramater.
                command.Parameters[0].Value = nomer_gr;
                command.Parameters[1].Value = dayn;
                command.Parameters[2].Value = para;
                command.Parameters[3].Value = disc;
                command.Parameters[4].Value = id_pr;
                command.Parameters[5].Value = aud;
                command.Parameters[6].Value = id_gr;

                // Execute SQL command.
                int recordAffected = command.ExecuteNonQuery();
                if (Convert.ToBoolean(recordAffected))
                {
                    MessageBox.Show("Данные успешно добавлены");
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка");
            }
            connection.Close();
            form2.DateNew();
            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Только цифры
            if (Char.IsDigit(e.KeyChar) == true) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox3.Clear();
        }
    }



}

