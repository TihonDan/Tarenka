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

namespace Tarenka
{
    public partial class Form7 : MaterialForm
    {
        static string conncetionString = "Host=localhost;Port=5432;Database=Homework;Username=postgres;Password=postgres";

        NpgsqlConnection connection = new NpgsqlConnection(conncetionString);

        private DataSet ds = new DataSet();

        private DataTable dt = new DataTable();

        Form3 form3 = new Form3();

        int id_faculty;


        public Form7()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            name_faculty();
        }

        public void insertRecord(String surname, String name, String middle_name, int id_faculty,  int age)
        {
            connection.Open();

            try
            {
                // Create insert command.
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO " +
                    "prepod (surname, name, middle_name, id_faculty, age) VALUES(:surname, :name, " +
                    ":middle_name, :id_faculty, :age)", connection);

                // Add paramaters.
                command.Parameters.Add(new NpgsqlParameter("surname",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("name",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("middle_name",
                    NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("id_faculty",
                    NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("age",
                    NpgsqlTypes.NpgsqlDbType.Integer));


                // Prepare the command.
                command.Prepare();

                // Add value to the paramater.
                command.Parameters[0].Value = surname;
                command.Parameters[1].Value = name;
                command.Parameters[2].Value = middle_name;
                command.Parameters[3].Value = id_faculty;
                command.Parameters[4].Value = age;

                // Execute SQL command.
                int recordAffected = command.ExecuteNonQuery();
                if (Convert.ToBoolean(recordAffected))
                {
                    MessageBox.Show("Данные успешно добавлены");
                    form3.DateNew();
                    this.Close();
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка");
            }
            connection.Close();
        }

        void name_faculty()
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

        void get_id_faculty(String name_faculty)
        {
            string sql = $"select id_faculty from faculty where name_faculty = '{name_faculty}'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            string[] id = new string[1];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id[i] = Convert.ToString(dt.Rows[i]["id_faculty"]);
                id_faculty = Convert.ToInt32(id[0]);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Только цифры
            if (Char.IsDigit(e.KeyChar) == true) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox6.Clear();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false) return;

            if (e.KeyChar == Convert.ToChar(Keys.Back)) return;

            e.Handled = true;

            textBox3.Clear();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            get_id_faculty(comboBox1.Text);
            string surname = textBox1.Text;
            string name = textBox2.Text;
            string middle_name = textBox3.Text;
            string age = textBox6.Text;

            insertRecord(surname, name, middle_name, id_faculty, Convert.ToInt32(age));
        }
    }
}
