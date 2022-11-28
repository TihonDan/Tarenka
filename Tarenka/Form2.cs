using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tarenka
{
    public partial class Form2 : MaterialForm
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
        string selectGrid6;
        string selectGrid7;

        public Form2()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DateNew();

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)//Close
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)//Add
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }

        public void DateNew()
        {

            if(connection.FullState == ConnectionState.Open)
            {
                connection.Close();
                connection.Open();
            }
            else
            {
                connection.Open();
            }

            string sql = $"select disc as Предмет, rasp.id_pr, concat(surname, name, middle_name) as Преподаватель, aud as Аудитория, id_rs, nomer_gr as Номер_группы , dayn as День, para as Пара, id_gr from rasp, prepod where rasp.id_pr = prepod.id_pr";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView1.DataSource = dt;

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[8].Visible = false;

            connection.Close();
        }
        public void deleteRecord(int id_rs) 
        {
            if(selectGrid != null)
            {
                connection.Open();

                try
                {
                    // Create insert command.
                    NpgsqlCommand command = new NpgsqlCommand("delete from rasp where id_rs = :id_rs", connection);

                    // Add paramaters.
                    command.Parameters.Add(new NpgsqlParameter("id_rs",
                        NpgsqlTypes.NpgsqlDbType.Integer));


                    // Prepare the command.
                    command.Prepare();

                    // Add value to the paramater.
                    command.Parameters[0].Value = id_rs;

                    // Execute SQL command.
                    int recordAffected = command.ExecuteNonQuery();
                    if (Convert.ToBoolean(recordAffected))
                    {
                        MessageBox.Show("Данные успешно удалены");
                    }

                    DateNew();
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
            selectGrid = dataGridView1.SelectedRows[0].Cells["Номер_группы"].Value.ToString();
            selectGrid1 = dataGridView1.SelectedRows[0].Cells["День"].Value.ToString();
            selectGrid2 = dataGridView1.SelectedRows[0].Cells["Пара"].Value.ToString();
            selectGrid3 = dataGridView1.SelectedRows[0].Cells["Предмет"].Value.ToString();
            selectGrid4 = dataGridView1.SelectedRows[0].Cells["id_pr"].Value.ToString();
            selectGrid5 = dataGridView1.SelectedRows[0].Cells["Аудитория"].Value.ToString();
            selectGrid6 = dataGridView1.SelectedRows[0].Cells["id_rs"].Value.ToString();
            selectGrid7 = dataGridView1.SelectedRows[0].Cells["id_gr"].Value.ToString();

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

        private void materialButton1_Click(object sender, EventArgs e)
        {
            DateNew();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            deleteRecord(Convert.ToInt32(selectGrid6));
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            if (selectGrid != null)
            {
                Form6 form6 = new Form6(selectGrid, selectGrid1, selectGrid2, selectGrid3, Convert.ToInt32(selectGrid4), selectGrid5, Convert.ToInt32(selectGrid6), Convert.ToInt32(selectGrid7));
                form6.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите элемент");
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }
    }
}
