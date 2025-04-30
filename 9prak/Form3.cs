using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace _9prak
{
    public partial class Form3: Form
    {
        BindingSource bs;
        public Form3(DataTable dtprojekts)
        {
            InitializeComponent();
            bs = new BindingSource();
            bs.DataSource = dtprojekts;
            bs.AddNew();
            textBox1.DataBindings.Add("Text", bs, "Nosaukums");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nosaukums = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(nosaukums))
            {
                MessageBox.Show("Please enter a project name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection("Host=localhost;Username=postgres;Password=students;Database=9prak"))
                {
                    conn.Open();
                    var cmd = new Npgsql.NpgsqlCommand("INSERT INTO projekts (nosaukums) VALUES (@nosaukums)", conn);
                    cmd.Parameters.AddWithValue("nosaukums", nosaukums);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Project saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear(); // Clear for next input
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving project:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
