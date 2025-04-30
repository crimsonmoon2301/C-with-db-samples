using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace _8.prak
{
    public partial class Form1: Form
    {
        private DataSet ds = new DataSet();
        private NpgsqlConnection conn;
        private NpgsqlDataAdapter daStudents, daProgramma, daFakultate;

        public Form1()
        {
            InitializeComponent();
            dataGridViewStudents.CellClick += dataGridViewStudents_CellClick;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connStr = "Host=localhost;Port=5432;Username=postgres;Password=students;Database=Student";
            conn = new NpgsqlConnection(connStr);
            
            try
            {
                daFakultate = new NpgsqlDataAdapter("SELECT * FROM Fakultate", conn);
                daProgramma = new NpgsqlDataAdapter("SELECT * FROM Programma", conn);
                daStudents = new NpgsqlDataAdapter("SELECT * FROM Students", conn);

                // Šīs 3 rindas ir ļoti svarīgas
                daFakultate.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daProgramma.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daStudents.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                // Tagad CommandBuilder var uzģenerēt komandas korekti
                new NpgsqlCommandBuilder(daFakultate);
                new NpgsqlCommandBuilder(daProgramma);
                new NpgsqlCommandBuilder(daStudents);

                daFakultate.Fill(ds, "Fakultate");
                daProgramma.Fill(ds, "Programma");
                daStudents.Fill(ds, "Students");

                daFakultate.AcceptChangesDuringUpdate = true;
                daStudents.AcceptChangesDuringUpdate = true;
                daProgramma.AcceptChangesDuringUpdate = true ;

                ds.Tables["Fakultate"].TableName = "Fakultate";
                ds.Tables["Programma"].TableName = "Programma";
                ds.Tables["Students"].TableName = "Students";

                ds.Relations.Add(new DataRelation("FK_Programma_Fakultate",
                    ds.Tables["Fakultate"].Columns["Id_F"],
                    ds.Tables["Programma"].Columns["Id_F"], true));

                ds.Relations.Add(new DataRelation("FK_Students_Programma",
                    ds.Tables["Programma"].Columns["Id_SP"],
                    ds.Tables["Students"].Columns["Id_SP"], true));

                dataGridViewFakultates.DataSource = ds.Tables["Fakultate"];
                dataGridViewProgrammas.DataSource = ds.Tables["Programma"];
                dataGridViewStudents.DataSource = ds.Tables["Students"];

                dataGridViewStudents.Columns["Id"].Visible = false;



                DataGridViewComboBoxColumn cbSP = new DataGridViewComboBoxColumn
                {
                    HeaderText = "Studiju programma",
                    DataSource = ds.Tables["Programma"],
                    DisplayMember = "Nosaukums",
                    ValueMember = "Id_SP",
                    DataPropertyName = "Id_SP"
                };
                dataGridViewStudents.Columns.Add(cbSP);
                dataGridViewStudents.Columns["Id_SP"].Visible = false;

                DataGridViewComboBoxColumn cbF = new DataGridViewComboBoxColumn
                {
                    HeaderText = "Fakultāte",
                    DataSource = ds.Tables["Fakultate"],
                    DisplayMember = "Nosaukums",
                    ValueMember = "Id_F",
                    DataPropertyName = "Id_F"
                };

                DataGridViewButtonColumn btnDetails = new DataGridViewButtonColumn
                {
                    HeaderText = "Darbība",
                    Text = "Visi dati",
                    UseColumnTextForButtonValue = true,
                    Name = "btnVisiDati"
                };
                dataGridViewStudents.Columns.Insert(0, btnDetails);

                dataGridViewProgrammas.Columns.Add(cbF);
                dataGridViewProgrammas.Columns["Id_F"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kļūda pievienojot datu avotus: " + ex.Message);
            }
        }

        // Messagebox
        private void dataGridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewStudents.Columns[e.ColumnIndex].Name == "btnVisiDati")
            {
                DataRowView rowView = (DataRowView)dataGridViewStudents.Rows[e.RowIndex].DataBoundItem;
                DataRow row = rowView.Row;

                DataRow programmaRow = row.GetParentRow("FK_Students_Programma");
                string programName = "Nezināma";
                string fakultateName = "Nezināma";
                string birthYear = "Nezināms";
                string gender = "Nezināms";

                if (programmaRow != null)
                {
                    programName = programmaRow["Nosaukums"].ToString();

                    DataRow fakultateRow = programmaRow.GetParentRow("FK_Programma_Fakultate");
                    if (fakultateRow != null)
                    {
                        fakultateName = fakultateRow["Nosaukums"].ToString();
                    }
                }

                // Get the birth year and gender from the current row
                birthYear = row["dzimsanas_gads"].ToString();
                gender = row["dzimums"].ToString();

                // Display all the information in the MessageBox
                string studentInfo = $"ID: {row["Id"]}\n" +
                                     $"Vārds: {row["Vards"]}\n" +
                                     $"Uzvārds: {row["Uzvards"]}\n" +
                                     $"Studiju programma: {programName}\n" +
                                     $"Fakultāte: {fakultateName}\n" +
                                     $"Dzimšanas gads: {birthYear}\n" +
                                     $"Dzimums: {gender}";

                MessageBox.Show(studentInfo, "Studentu dati", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
                MessageBox.Show("Dati veiksmīgi atsvaidzināti!", "Atsvaidzināts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kļūda atsvaidzinot datus: " + ex.Message);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                daFakultate.Update(ds.Tables["Fakultate"]);
                daProgramma.Update(ds.Tables["Programma"]);
                daStudents.Update(ds.Tables["Students"]);

                MessageBox.Show("Izmaiņas saglabātas veiksmīgi!", "Saglabāts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kļūda saglabājot: " + ex.Message);
            }
        }

        private void RefreshData()
        {
            try
            {
                ds.Clear();

                daFakultate.Fill(ds, "Fakultate");
                daProgramma.Fill(ds, "Programma");
                daStudents.Fill(ds, "Students");

                dataGridViewFakultates.DataSource = ds.Tables["Fakultate"];
                dataGridViewProgrammas.DataSource = ds.Tables["Programma"];
                dataGridViewStudents.DataSource = ds.Tables["Students"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kļūda atsvaidzinot datus no datu bāzes: " + ex.Message);
            }
        }
    }
}
