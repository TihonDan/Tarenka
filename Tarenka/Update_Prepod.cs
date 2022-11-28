using MaterialSkin;
using MaterialSkin.Controls;
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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tarenka
{
    public partial class Update_Prepod : MaterialForm
    {
        static string conncetionString = "Host=localhost;Port=5432;Database=Homework;Username=postgres;Password=postgres";

        NpgsqlConnection connection = new NpgsqlConnection(conncetionString);

        private DataSet ds = new DataSet();

        private DataTable dt = new DataTable();

        int id_fac;
        int _id_pr;
        string name_fac;

        public Update_Prepod(String fam, String name, String last_name, int faculty, String age, int id_pr)
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            selectNameFac(faculty);
            textBox5.Text = fam;
            textBox4.Text = name; 
            textBox3.Text = last_name;
            comboBox1.Text = name_fac;
            textBox2.Text = age;
            _id_pr = id_pr;

            selectAllPrepod();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void selectAllPrepod()
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

        void selectNameFac(int id_fac)
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
                name_fac = id[0];
            }
        }

        void selectComboId(string selectName)
        {
            string sql = $"select id_faculty from faculty where name_faculty = '{selectName}'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["id_faculty"]);
                id_fac = Convert.ToInt32(id[0]);
            }
        }

        public void updateDate(String fam, String name, String last_name, int fac, int age, int id_pr)
        {
            connection.Open();

            try
            {
                // Create insert command.
                NpgsqlCommand command = new NpgsqlCommand("update prepod set surname = :fam, name = :name, middle_name = :last_name, id_faculty = :fac, age = :age where id_pr = :id_pr", connection);

                // Add paramaters.
                command.Parameters.Add(new NpgsqlParameter("fam",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("name",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("last_name",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("fac",
                    NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("id_pr",
                    NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("age",
                    NpgsqlTypes.NpgsqlDbType.Integer));


                // Prepare the command.
                command.Prepare();

                // Add value to the paramater.
                command.Parameters[0].Value = fam;
                command.Parameters[1].Value = name;
                command.Parameters[2].Value = last_name;
                command.Parameters[3].Value = fac;
                command.Parameters[4].Value = id_pr;
                command.Parameters[5].Value = age;

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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox5.Clear();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox4.Clear();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox3.Clear();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == true) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox2.Clear();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            comboBox1 = null;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            selectComboId(comboBox1.Text);
            updateDate(textBox5.Text, textBox4.Text, textBox3.Text, id_fac, Convert.ToInt32(textBox2.Text), _id_pr);
        }
    }
}
