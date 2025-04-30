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
using Npgsql;

namespace _9prak
{
    public partial class Form2: Form
    {
        private DataSet ds;
        private DataTable dtmaja, dtipasnieks, dtprojekts;
        private NpgsqlDataAdapter adapter, adapter2, adapter3;

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public Form2(DataSet ds, DataTable dtmaja, DataTable dtipasnieks, DataTable dtprojekts,
                     NpgsqlDataAdapter adapter, NpgsqlDataAdapter adapter2, NpgsqlDataAdapter adapter3)
        {
            InitializeComponent();

            this.ds = ds;
            this.dtmaja = dtmaja;
            this.dtipasnieks = dtipasnieks;
            this.dtprojekts = dtprojekts;
            this.adapter = adapter;
            this.adapter2 = adapter2;
            this.adapter3 = adapter3;

            // Piemērs: piesaistīt tabulu datuGridView
            dataGridView3.DataSource = dtmaja;
            dataGridView2.DataSource = dtipasnieks;
            dataGridView1.DataSource = dtprojekts;

            if (dataGridView3.Columns.Contains("ipasnieksid"))
                dataGridView3.Columns["ipasnieksid"].Visible = false;

            if (dataGridView3.Columns.Contains("projektsid"))
                dataGridView3.Columns["projektsid"].Visible = false;
            //// Noņemam veco kolonnu
            //if (dataGridView1.Columns.Contains("projektsid"))
            //    dataGridView1.Columns.Remove("projektsid");

            //// Izveidojam ComboBox kolonnu
            //var comboColumn = new DataGridViewComboBoxColumn();
            //comboColumn.HeaderText = "Projekts";
            //comboColumn.DataPropertyName = "projektsid"; // tā saistās ar maja tabulas laukiem
            //comboColumn.DataSource = dtprojekts;
            //comboColumn.DisplayMember = "nosaukums";
            //comboColumn.ValueMember = "projektsid";

            //// Pievienojam jauno kolonnu
            //dataGridView1.Columns.Add(comboColumn);

            if (!dataGridView3.Columns.Contains("ipasnieksidCombo"))
            {

                var ipasnieksColumn = new DataGridViewComboBoxColumn();
                ipasnieksColumn.HeaderText = "Īpašnieks";
                ipasnieksColumn.Name = "ipasnieksidCombo";
                ipasnieksColumn.DataPropertyName = "ipasnieksid";
                ipasnieksColumn.DataSource = dtipasnieks;
                ipasnieksColumn.DisplayMember = "vards";
                ipasnieksColumn.ValueMember = "ipasnieksid";
                dataGridView3.Columns.Add(ipasnieksColumn);
            }


            if (!dataGridView3.Columns.Contains("projektsidCombo"))
            {
                var projektsColumn = new DataGridViewComboBoxColumn();
                projektsColumn.HeaderText = "Projekts";
                projektsColumn.Name = "projektsidCombo";
                projektsColumn.DataPropertyName = "projektsid";
                projektsColumn.DataSource = dtprojekts;
                projektsColumn.DisplayMember = "nosaukums";
                projektsColumn.ValueMember = "projektsid";
                dataGridView3.Columns.Add(projektsColumn);
            }

        }

        //public void SaveAndRefresh()
        //{
        //    try
        //    {
        //        adapter.Update(dtmaja);
        //        adapter2.Update(dtipasnieks);
        //        adapter3.Update(dtprojekts);

        //        ds.Tables["maja"].Clear();
        //        ds.Tables["ipasnieks"].Clear();
        //        ds.Tables["projekts"].Clear();

        //        adapter.Fill(ds.Tables["maja"]);
        //        adapter2.Fill(ds.Tables["ipasnieks"]);
        //        adapter3.Fill(ds.Tables["projekts"]);

        //        dataGridView3.DataSource = null;
        //        dataGridView3.DataSource = dtmaja;

        //        dataGridView2.DataSource = null;
        //        dataGridView2.DataSource = dtipasnieks;

        //        dataGridView1.DataSource = null;
        //        dataGridView1.DataSource = dtprojekts;

        //        MessageBox.Show("Dati veiksmīgi saglabāti un atjaunoti!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Kļūda saglabājot:\n" + ex.Message);
        //    }
        //}

        // projektu save
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                adapter.Update(dtmaja);
                adapter2.Update(dtipasnieks);
                adapter3.Update(dtprojekts);
                MessageBox.Show("Dati saglabāti!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kļūda saglabājot: " + ex.Message);
            }
        }
        private void dataGridView1_CellContentClick(object sender, EventArgs e)
        {

        }
    }
}
