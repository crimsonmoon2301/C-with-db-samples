using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6prak
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

         private string connectionString = "Host=localhost; Port=5432; Username=postgres; Password=students; Database=Masina";
        private DataTable dt = new DataTable();
        private NpgsqlDataAdapter da;
        private void Save_Click(object sender, EventArgs e)
        {
            if (dt == null || da == null)
            {
                MessageBox.Show("Dati nav ielādēti! Lūdzu, vispirms ielādējiet datus.", "Kļūda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(da);
            da.Update(dt); // Save changes to DB
            dt.AcceptChanges(); // Confirm changes in DataTable

            MessageBox.Show("Izmaiņas saglabātas veiksmīgi!", "Paziņojums", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open(); // Keep the connection open

            string sql = "SELECT * FROM masina";
            da = new NpgsqlDataAdapter(sql, connection);
            NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(da);

            dt = new DataTable();
            da.Fill(dt); // Fill DataTable with data

            dataGridView1.DataSource = dt; // Bind to DataGridView
            
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
