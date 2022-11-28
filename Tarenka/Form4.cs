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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tarenka
{
    public partial class Form4 : MaterialForm
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

        public Form4()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);


            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DateNew();
        }

        void DateNew()
        {
            if (connection.FullState == ConnectionState.Open)
            {
                connection.Close();
                connection.Open();
            }
            else
            {
                connection.Open();
            }

            string sql = $"select gruppy.id_faculty, name_faculty as Факультет, kurs as Курс, id_gr, nomer_gr as Номер_группы from gruppy, faculty where gruppy.id_faculty = faculty.id_faculty";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView1.DataSource = dt;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;

            connection.Close();
        }

        public void update_rasp(int id_gr)
        {
            connection.Open();
            // Create insert command.
            NpgsqlCommand command = new NpgsqlCommand("delete from rasp where id_gr = :id_gr", connection);

            // Add paramaters.
            command.Parameters.Add(new NpgsqlParameter("id_gr",
                NpgsqlTypes.NpgsqlDbType.Integer));


            // Prepare the command.
            command.Prepare();

            // Add value to the paramater.
            command.Parameters[0].Value = id_gr;

            // Execute SQL command.
            int recordAffected = command.ExecuteNonQuery();

            connection.Close();
        }

        void Add()
        {
            Add_Gruppy add_gruppy = new Add_Gruppy();
            add_gruppy.ShowDialog();
        }

        public void deleteRecord(int id_gr)
        {
            if (selectGrid != null)
            {
                connection.Open();

                try
                {
                    // Create insert command.
                    NpgsqlCommand command = new NpgsqlCommand("delete from gruppy where id_gr = :id_gr", connection);

                    // Add paramaters.
                    command.Parameters.Add(new NpgsqlParameter("id_gr",
                        NpgsqlTypes.NpgsqlDbType.Integer));


                    // Prepare the command.
                    command.Prepare();

                    // Add value to the paramater.
                    command.Parameters[0].Value = id_gr;

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
            update_rasp(id_gr);
            connection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectGrid = dataGridView1.SelectedRows[0].Cells["id_faculty"].Value.ToString();
            selectGrid1 = dataGridView1.SelectedRows[0].Cells["Курс"].Value.ToString();
            selectGrid2 = dataGridView1.SelectedRows[0].Cells["id_gr"].Value.ToString();
            selectGrid3 = dataGridView1.SelectedRows[0].Cells["id_gr"].Value.ToString();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            DateNew();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            if (selectGrid != null)
            {
                deleteRecord(Convert.ToInt32(selectGrid2));
            }
            else
            {
                MessageBox.Show("Выберите элемент");
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            if (selectGrid != null)
            {
                Update_gruppy update_gruppy = new Update_gruppy(Convert.ToInt32(selectGrid), selectGrid1, selectGrid3, Convert.ToInt32(selectGrid2));
                update_gruppy.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите элемент");
            }
        }
    }
}
