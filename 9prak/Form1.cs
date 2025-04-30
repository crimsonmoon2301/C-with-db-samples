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
    public partial class Form1: Form
    {
        private DataSet ds = new DataSet();
        private NpgsqlConnection conn;
        private NpgsqlDataAdapter adapter, adapter2, adapter3;
        private DataTable dtmaja, dtipasnieks, dtprojekts;

        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // this.WindowState = FormWindowState.Maximized;


            string connStr = "Host=localhost;Port=5432;Username=postgres;Password=students;Database=9prak";
            conn = new NpgsqlConnection(connStr);

            adapter = new NpgsqlDataAdapter("SELECT * FROM maja", conn);
            adapter2 = new NpgsqlDataAdapter("SELECT * FROM ipasnieks", conn);
            adapter3 = new NpgsqlDataAdapter("SELECT * FROM projekts", conn);

            new NpgsqlCommandBuilder(adapter);
            new NpgsqlCommandBuilder(adapter2);
            new NpgsqlCommandBuilder(adapter3);

            adapter.Fill(ds, "maja");
            adapter2.Fill(ds, "ipasnieks");
            adapter3.Fill(ds, "projekts");

            dtmaja = ds.Tables["maja"];
            dtipasnieks = ds.Tables["ipasnieks"];
            dtprojekts = ds.Tables["projekts"];

            //Form2 f = new Form2(ds, dtmaja, dtipasnieks, dtprojekts, adapter, adapter2, adapter3);
            //f.MdiParent = this;
            //f.Show();
            //f.StartPosition = FormStartPosition.Manual;
            //f.Location = new Point(10, 90);

            //Form2 f = new Form2(ds, dtmaja, dtipasnieks, dtprojekts, adapter, adapter2, adapter3);
            //f.MdiParent = this.MdiParent; // ja izmanto MDI
            //f.Show();
            //f.StartPosition = FormStartPosition.Manual;
            //f.Location = new Point(10, 90);

            //Form3 x = new Form3(dtprojekts);
            //x.MdiParent = this;
            //x.Show();
            //x.StartPosition = FormStartPosition.Manual;
            //x.Location = new Point(700, 90);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(ds, dtmaja, dtipasnieks, dtprojekts, adapter, adapter2, adapter3);
            f.MdiParent = this.MdiParent;
            f.Show();

            Form3 f3 = new Form3(dtprojekts);
            f3.MdiParent = this.MdiParent;
            f3.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // 1) save any pending changes
            var dataSaver = new SaveData(adapter, adapter2, adapter3);
            dataSaver.Save(dtmaja, dtipasnieks, dtprojekts);
            // 2) reload the tables so the DB‐generated IDs come back
            dtprojekts.Clear();
            adapter3.Fill(dtprojekts);

            dtipasnieks.Clear();
            adapter2.Fill(dtipasnieks);

            dtmaja.Clear();
            adapter.Fill(dtmaja);
        }
    }
}
