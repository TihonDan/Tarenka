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
    public partial class Form6 : Form
    {
        static string conncetionString = "Host=localhost;Port=5432;Database=Homework;Username=postgres;Password=postgres";

        NpgsqlConnection connection = new NpgsqlConnection(conncetionString);

        private DataSet ds = new DataSet();

        private DataTable dt = new DataTable();

        int _id_pr;
        int _id_rs;
        int _id_gr;
        string _trim_name;

        int last_id_gr;

        string id_prepod;
        string[] id = new string[1];

        public Form6(String numberGroup, String day, String para, String trimName, int id_pr, String aud, int id_rs, int id_gr)
        {
            InitializeComponent();

            comboBox1.Text = numberGroup;
            comboBox2.Text = day; 
            textBox3.Text = para;
            comboBox3.Text = trimName;
            comboBox4.Text = aud;
            _id_pr = id_pr;
            _id_rs = id_rs;

            selectPrepod();
            selectAllPrepod();
            number_gr();
            sokr_name();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            get_id_gr();
            updateDate(comboBox1.Text, comboBox2.Text, textBox3.Text, comboBox3.Text, _id_pr, comboBox4.Text, _id_rs, last_id_gr);
        }

        void selectPrepod()
        {
            string sql = $"select concat(surname, name, middle_name) from prepod where id_pr = {_id_pr}";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["concat"]);
                comboBox5.Text = Convert.ToString(id[i]);
            }
        }

        void selectAllPrepod()
        {
            string sql = $"select concat(surname, name, middle_name) from prepod";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[10];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["concat"]);
                comboBox5.Items.Add(id[i]);
            }
        }

        void number_gr()
        {
            string sql = $"select nomer_gr from gruppy";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[10];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["nomer_gr"]);
                comboBox1.Items.Add(id[i]);
            }

        }

        void sokr_name()
        {
            string sql = $"select disc from rasp";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[10];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["disc"]);
                comboBox3.Items.Add(id[i]);
            }
        }

        //void idPrepod()
        //{

        //}

        void get_id_gr()
        {
            string n_gr = comboBox1.Text;
            string sql = $"select id_gr from gruppy where nomer_gr = '{n_gr}'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                last_id_gr = Convert.ToInt32(dt.Rows[i]["id_gr"]);
            }
        }

        public void updateDate(String nomer_gr, String dayn, String para, String disc, int id_pr, String aud, int id_rs, int id_gr)
        {
            connection.Open();

            try
            {
                // Create insert command.
                NpgsqlCommand command = new NpgsqlCommand("update rasp set nomer_gr = :nomer_gr, dayn = :dayn, para = :para, disc = :disc, id_pr = :id_pr, aud = :aud, id_gr = :id_gr where id_rs = :id_rs", connection);

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
                command.Parameters.Add(new NpgsqlParameter("id_rs",
                    NpgsqlTypes.NpgsqlDbType.Integer));
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
                command.Parameters[6].Value = id_rs;
                command.Parameters[7].Value = id_gr;

                // Execute SQL command.
                int recordAffected = command.ExecuteNonQuery();
                if (Convert.ToBoolean(recordAffected))
                {
                    MessageBox.Show("Данные успешно обновлены");
                }        
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка");
            }
            connection.Close();
            this.Close();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_prepod = comboBox5.Items[comboBox5.SelectedIndex].ToString();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox1 = null;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox2 = null;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == true) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox3.Clear();
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox3 = null;
        }

        private void comboBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox5 = null;
        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox4 = null;
        }
    }
}
