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
    public partial class Update_gruppy : Form
    {
        static string conncetionString = "Host=localhost;Port=5432;Database=Homework;Username=postgres;Password=postgres";

        NpgsqlConnection connection = new NpgsqlConnection(conncetionString);

        private DataSet ds = new DataSet();

        private DataTable dt = new DataTable();

        int _id_fac;
        int _id_gr;
        public Update_gruppy(int id_fac, String kurs, String num_gr, int id_gr)
        {
            InitializeComponent();
            all_faculty();
            name_fac_id(id_fac);

            _id_gr = id_gr;

            textBox1.Text = kurs;
            textBox2.Text = num_gr;
        }

        private void button2_Click(object sender, EventArgs e)//Update
        {
            id_faculty_name(comboBox1.Text);
            Update(_id_fac, textBox1.Text, textBox2.Text, _id_gr);
        }

        void Update(int id_faculty, String kurs, String nomer_gr, int id_gr)
        {
            connection.Open();

            try
            {
                // Create insert command.
                NpgsqlCommand command = new NpgsqlCommand("update gruppy set id_faculty = :id_faculty, kurs = :kurs, nomer_gr = :nomer_gr where id_gr = :id_gr", connection);

                // Add paramaters.
                command.Parameters.Add(new NpgsqlParameter("id_faculty",
                    NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("kurs",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("nomer_gr",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("id_gr",
                    NpgsqlTypes.NpgsqlDbType.Integer));


                // Prepare the command.
                command.Prepare();

                // Add value to the paramater.
                command.Parameters[0].Value = id_faculty;
                command.Parameters[1].Value = kurs;
                command.Parameters[2].Value = nomer_gr;
                command.Parameters[3].Value = id_gr;

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

        void name_fac_id(int id_fac)
        {
            string sql = $"select name_faculty from faculty where id_faculty = {id_fac}";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["name_faculty"]);
                comboBox1.Text = Convert.ToString(id[0]);
            }
        }

        void id_faculty_name(String name)
        {
            string sql = $"select id_faculty from faculty where name_faculty = '{name}'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            int[] id = new int[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToInt32(dt.Rows[i]["id_faculty"]);
                _id_fac = id[0];
            }
        }

        void all_faculty()
        {
            string sql = $"select name_faculty from faculty";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[10];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["name_faculty"]);
                comboBox1.Items.Add(id[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox1 = null;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == true) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox1.Clear();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox2.Clear();
        }
    }
}
