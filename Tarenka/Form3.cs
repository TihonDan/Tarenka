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
using System.Xml.Linq;
using MaterialSkin.Controls;
using MaterialSkin;

namespace Tarenka
{
    public partial class Form3 : MaterialForm
    {
        static string conncetionString = "Host=localhost;Port=5432;Database=Homework;Username=postgres;Password=postgres";

        NpgsqlConnection connection = new NpgsqlConnection(conncetionString);

        NpgsqlCommand comm = new NpgsqlCommand();

        private DataSet ds = new DataSet();

        private DataTable dt = new DataTable();

        string selectGrid;
        string selectGrid1;
        string selectGrid2;
        string selectGrid3;
        string selectGrid4;
        string selectGrid5;


        public Form3()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DateNew();

            //dataGridView1.AutoResizeColumns();
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public void DateNew()
        {
            string sql = $"select id_pr, surname as Фамилия, name as Имя, middle_name as Отчество, name_faculty as Название_факультета, prepod.id_faculty, age as Возраст from prepod, faculty where prepod.id_faculty = faculty.id_faculty";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView1.DataSource = dt;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;

            connection.Close();
        }

        void update_rasp(int id_pr)
        {

            try
            {
                // Create insert command.
                NpgsqlCommand command = new NpgsqlCommand("delete from rasp where id_pr = :id_pr", connection);

                // Add paramaters.
                command.Parameters.Add(new NpgsqlParameter("id_pr",
                    NpgsqlTypes.NpgsqlDbType.Integer));


                // Prepare the command.
                command.Prepare();

                // Add value to the paramater.
                command.Parameters[0].Value = id_pr;

                // Execute SQL command.
                int recordAffected = command.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка");
            }
            connection.Close();
        }

        public void deleteRecord(int id_pr)
        {
            if(selectGrid != null)
            {
                connection.Open();

                try
                {
                    // Create insert command.
                    NpgsqlCommand command = new NpgsqlCommand("delete from prepod where id_pr = :id_pr", connection);

                    // Add paramaters.
                    command.Parameters.Add(new NpgsqlParameter("id_pr",
                        NpgsqlTypes.NpgsqlDbType.Integer));


                    // Prepare the command.
                    command.Prepare();

                    // Add value to the paramater.
                    command.Parameters[0].Value = id_pr;

                    // Execute SQL command.
                    int recordAffected = command.ExecuteNonQuery();
                    if (Convert.ToBoolean(recordAffected))
                    {
                        MessageBox.Show("Данные успешно удалены");
                        update_rasp(id_pr);
                        DateNew();
                    }

                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент");
            }

            connection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectGrid = dataGridView1.SelectedRows[0].Cells["Фамилия"].Value.ToString();
            selectGrid1 = dataGridView1.SelectedRows[0].Cells["Имя"].Value.ToString();
            selectGrid2 = dataGridView1.SelectedRows[0].Cells["Отчество"].Value.ToString();
            selectGrid3 = dataGridView1.SelectedRows[0].Cells["id_faculty"].Value.ToString();
            selectGrid4 = dataGridView1.SelectedRows[0].Cells["Возраст"].Value.ToString();
            selectGrid5 = dataGridView1.SelectedRows[0].Cells["id_pr"].Value.ToString();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (Convert.ToString(dataGridView1.Rows[i].Cells[j].Value) != "")
                    {
                        dataGridView1.Rows[i].Cells[j].ReadOnly = true;
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (selectGrid != null)
            {
                Update_Prepod update_form = new Update_Prepod(selectGrid, selectGrid1, selectGrid2, Convert.ToInt32(selectGrid3), selectGrid4, Convert.ToInt32(selectGrid5));
                update_form.ShowDialog();
            }
            else 
            {
                MessageBox.Show("Выберите элемент");
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            DateNew();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.ShowDialog();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            deleteRecord(Convert.ToInt32(selectGrid5));
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            if (selectGrid != null)
            {
                Update_Prepod update_form = new Update_Prepod(selectGrid, selectGrid1, selectGrid2, Convert.ToInt32(selectGrid3), selectGrid4, Convert.ToInt32(selectGrid5));
                update_form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите элемент");
            }
        }
    }
}
