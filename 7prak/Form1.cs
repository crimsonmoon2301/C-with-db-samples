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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _7prak
{
    public partial class Form1: Form
    {
        static NpgsqlDataAdapter da;
        DataTable table = new DataTable();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;



            string conStr = "Server=localhost; Port=5432; Username=postgres; Password=students; Database=Student;";

            NpgsqlConnection con = new NpgsqlConnection(conStr);

            da = new NpgsqlDataAdapter("select * from student_info", con);
            NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(da);
            da.DeleteCommand = builder.GetDeleteCommand();
            da.UpdateCommand = builder.GetUpdateCommand();
            da.InsertCommand = builder.GetInsertCommand();

            da.Fill(table);
            listBox1.DataSource = table;
            listBox1.DisplayMember = "Name";
            CurrencyManager cm = (CurrencyManager)this.BindingContext[table];
            int rowIndex = (int)cm.Position;
            textBox1.Text = rowIndex.ToString();

            listBox2.DataSource = table;
            listBox2.DisplayMember = "Surname";
            CurrencyManager cm2 = (CurrencyManager)this.BindingContext[table];
            int rowIndex2 = (int)cm.Position;
            textBox1.Text = rowIndex.ToString();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "Name";
            dataGridView1.Columns[0].DataPropertyName = "name";
            dataGridView1.Columns[1].HeaderText = "Surname";
            dataGridView1.Columns[1].DataPropertyName = "surname";
            dataGridView1.DataSource = table;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    int rowIndex = int.Parse(textBox1.Text);
                    if (rowIndex >= table.Rows.Count || rowIndex < 0)
                    {
                        throw new FormatException();
                    }
                    CurrencyManager cm =
                    (CurrencyManager)this.BindingContext[listBox1.DataSource];
                    cm.Position = rowIndex;
                    //CurrencyManager cm2 = (CurrencyManager)this.BindingContext[table];
                   // int rowIndex2 = (int)cm.Position; // Uses `cm` instead of `cm2`
                    textBox1.Text = rowIndex.ToString();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Laukā norādītai vērtībai ir jābūt veselam skaitlim robežās no 0 līdz " + table.Rows.Count);
            }


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int index = dataGridView1.CurrentRow.Index;
                textBox1.Text = index.ToString();
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            da.Update((DataTable)dataGridView1.DataSource);
        }

       private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
