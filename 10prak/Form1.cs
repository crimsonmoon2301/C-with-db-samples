using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;

namespace _10prak
{
    public partial class Form1: Form
    {
        public string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
        public NpgsqlConnection conn;
        public NpgsqlDataAdapter katAdapt;
        public NpgsqlDataAdapter katAdapt1 = new NpgsqlDataAdapter();
        NpgsqlDataAdapter precAdapt;
        public DataTable kategorijas;
        public int rowIndex;
        DataTable preces1;
        public int cur_node;

        
        public Form1()
        {

            InitializeComponent();

            //drīkst mainīt TreeView mezglu tekstu
            treeView1.LabelEdit = true;

            string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
            NpgsqlConnection conn = new NpgsqlConnection(connString);

            NpgsqlCommand sel = new NpgsqlCommand("SELECT * FROM preces", conn);
            // UPDATE
            NpgsqlCommand up = new NpgsqlCommand("UPDATE preces SET Nosaukums = @nosaukums, Cena = @cena, Kategorija_id = @kategorija_id WHERE p_ID = @id", conn);
            up.Parameters.Add(new NpgsqlParameter("@nosaukums", DbType.String, 255, "nosaukums"));
            up.Parameters.Add(new NpgsqlParameter("@cena", DbType.Double, sizeof(double), "cena"));
            up.Parameters.Add(new NpgsqlParameter("@kategorija_id", DbType.Int32, sizeof(int), "kategorija_id"));
            up.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32, sizeof(int), "p_id")); // ← kolonnas nosaukumam jābūt 'p_id'

            // INSERT
            NpgsqlCommand add = new NpgsqlCommand("INSERT INTO preces (Nosaukums, Cena, Kategorija_id) VALUES (@nosaukums, @cena, @kategorija_id)", conn);
            add.Parameters.Add(new NpgsqlParameter("@nosaukums", DbType.String, 255, "nosaukums"));
            add.Parameters.Add(new NpgsqlParameter("@cena", DbType.Double, sizeof(double), "cena"));
            add.Parameters.Add(new NpgsqlParameter("@kategorija_id", DbType.Int32, sizeof(int), "kategorija_id")); // ← bija kļūda nosaukumā

            // DELETE
            NpgsqlCommand del = new NpgsqlCommand("DELETE FROM preces WHERE p_ID = @p_id", conn);
            del.Parameters.Add(new NpgsqlParameter("@p_id", DbType.Int32, sizeof(int), "p_id"));

            precAdapt = new NpgsqlDataAdapter();
            precAdapt.SelectCommand = sel;
            precAdapt.InsertCommand = add;
            precAdapt.DeleteCommand = del;
            precAdapt.UpdateCommand = up;

            preces1 = new DataTable();
            precAdapt.Fill(preces1);
            dataGridView1.DataSource = preces1;

            SubLevel(0, null); //Funkcija, kura izveido TreeView struktūru
        }

        public void SubLevel(int parentid, TreeNode parentNode)
        {
            string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                NpgsqlCommand rootCmd = new NpgsqlCommand("SELECT id, nosaukums FROM kategorijas WHERE kategorijas_id IS NULL", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(rootCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CreateNodes(dt, treeView1.Nodes, conn);
            }
        }

        //public void SubLevel(int? parentid, TreeNode parentNode)
        //{
        //    string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
        //    NpgsqlConnection conn = new NpgsqlConnection(connString);


        //    NpgsqlCommand s1Cmd = new NpgsqlCommand("SELECT id, nosaukums as Name FROM kategorijas WHERE kategorijas_id=@parentid", conn);

        //    s1Cmd.Parameters.Add("@parentid", NpgsqlTypes.NpgsqlDbType.Integer).Value = parentid;
        //    NpgsqlDataAdapter da = new NpgsqlDataAdapter(s1Cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    if (parentid == 0)
        //        CreateNodes(dt, treeView1.Nodes);
        //    else
        //        CreateNodes(dt, parentNode.Nodes);
        //}

        private void CreateNodes(DataTable dt, TreeNodeCollection nodes, NpgsqlConnection conn)
        {
            foreach (DataRow row in dt.Rows)
            {
                TreeNode categoryNode = new TreeNode(row["nosaukums"].ToString());
                categoryNode.Tag = row["id"];
                categoryNode.Name = row["id"].ToString(); // <-- Add this line
                nodes.Add(categoryNode);

                int categoryId = Convert.ToInt32(row["id"]);

                // Add products for this category
                NpgsqlCommand productCmd = new NpgsqlCommand("SELECT nosaukums FROM preces WHERE kategorija_id = @catid", conn);
                productCmd.Parameters.AddWithValue("@catid", categoryId);
                NpgsqlDataAdapter productAdapter = new NpgsqlDataAdapter(productCmd);
                DataTable productTable = new DataTable();
                productAdapter.Fill(productTable);

                

                // Add subcategories (recursive)
                NpgsqlCommand childCmd = new NpgsqlCommand("SELECT id, nosaukums FROM kategorijas WHERE kategorijas_id=@parentid", conn);
                childCmd.Parameters.AddWithValue("@parentid", categoryId);
                NpgsqlDataAdapter childAdapter = new NpgsqlDataAdapter(childCmd);
                DataTable childTable = new DataTable();
                childAdapter.Fill(childTable);

                if (childTable.Rows.Count > 0)
                    CreateNodes(childTable, categoryNode.Nodes, conn);
            }
        }
        //public void CreateNodes(DataTable dt, TreeNodeCollection nodes)
        //{
        //    foreach (DataRow dr1 in dt.Rows)
        //    {
        //        TreeNode tn = new TreeNode();
        //        tn.Text = dr1["Name"].ToString();
        //        tn.Name = dr1["id"].ToString();
        //        nodes.Add(tn);
        //        SubLevel(Convert.ToInt32(tn.Name), tn);

        //    }
        //}
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name != "")
            {
                TreeNode tn = new TreeNode();
                tn.Text = "new";
                e.Node.Nodes.Add(tn);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int y = 0;
            if (e.Node.Name != "")
                //Ja mezglam ir nosaukums ( mezgls ir ierakstīts datu bāzē), tiek izpildīta funkcija dgv_select() ,
                //kura kā parametru saņem mezgla vārdu(kategorijas ID)
                y = Convert.ToInt32(e.Node.Name);
            dgv_select(y);
        }
       
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void dgv_select(int e)
        {
            string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
            NpgsqlConnection conn = new NpgsqlConnection(connString);

            if (e.ToString() != "")
            {
                preces1 = new DataTable();
                precAdapt = new NpgsqlDataAdapter();

                // Fill DataGridView with products from selected category
                NpgsqlCommand pr2 = new NpgsqlCommand("SELECT * FROM preces WHERE kategorija_id = @kategorija_id", conn);
                pr2.Parameters.Add("@kategorija_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = e;
                precAdapt.SelectCommand = pr2;

                precAdapt.Fill(preces1);
                dataGridView1.DataSource = preces1;
            }

            // Show category description in label1
            NpgsqlCommand selectapraksts = new NpgsqlCommand("SELECT apraksts FROM kategorijas WHERE id = @id", conn);
            selectapraksts.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = e;

            conn.Open();
            object result = selectapraksts.ExecuteScalar();
            conn.Close();

            label1.Text = result != null ? result.ToString() : "";

            // Set current category node
            cur_node = e;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            int rc = dataGridView1.Rows.Count - 1;
            if (rc > 0)
                dataGridView1.Rows[rc].Cells[3].Value = cur_node;
            else
                dataGridView1.Rows[0].Cells[3].Value = cur_node;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
                NpgsqlConnection conn = new NpgsqlConnection(connString);

                if (precAdapt == null || preces1 == null)
                    return;

                precAdapt.UpdateCommand = new NpgsqlCommand(
                    "UPDATE preces SET Nosaukums = @Nosaukums, Cena = @Cena, Kategorija_id = @Kategorija_id WHERE p_ID = @p_ID", conn);
                precAdapt.UpdateCommand.Parameters.Add("@Nosaukums", NpgsqlDbType.Varchar).SourceColumn = "Nosaukums";
                precAdapt.UpdateCommand.Parameters.Add("@Cena", NpgsqlDbType.Double).SourceColumn = "Cena";
                precAdapt.UpdateCommand.Parameters.Add("@Kategorija_id", NpgsqlDbType.Integer).SourceColumn = "Kategorija_id";
                var pUpdateID = precAdapt.UpdateCommand.Parameters.Add("@p_ID", NpgsqlDbType.Integer);
                pUpdateID.SourceColumn = "p_ID";
                pUpdateID.SourceVersion = DataRowVersion.Original;

                precAdapt.InsertCommand = new NpgsqlCommand(
                    "INSERT INTO preces (Nosaukums, Cena, Kategorija_id) VALUES (@Nosaukums, @Cena, @Kategorija_id)", conn);
                precAdapt.InsertCommand.Parameters.Add("@Nosaukums", NpgsqlDbType.Varchar).SourceColumn = "Nosaukums";
                precAdapt.InsertCommand.Parameters.Add("@Cena", NpgsqlDbType.Double).SourceColumn = "Cena";
                precAdapt.InsertCommand.Parameters.Add("@Kategorija_id", NpgsqlDbType.Integer).SourceColumn = "Kategorija_id";

                precAdapt.DeleteCommand = new NpgsqlCommand("DELETE FROM preces WHERE p_ID = @p_ID", conn);
                var pDeleteID = precAdapt.DeleteCommand.Parameters.Add("@p_ID", NpgsqlDbType.Integer);
                pDeleteID.SourceColumn = "p_ID";
                pDeleteID.SourceVersion = DataRowVersion.Original;

                precAdapt.Update(preces1);
                MessageBox.Show("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();
                if (changes != null)
                {
                    NpgsqlCommandBuilder x = new NpgsqlCommandBuilder(precAdapt);
                    precAdapt.UpdateCommand = x.GetUpdateCommand();
                    precAdapt.Update(changes);
                    ((DataTable)dataGridView1.DataSource).AcceptChanges();

                    MessageBox.Show("Cell Updated");
                    return;
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";
            NpgsqlConnection conn = new NpgsqlConnection(connString);

            if (e.Label != null)
                e.Node.Text = e.Label;
            NpgsqlCommand sCmd = new NpgsqlCommand("SELECT * FROM kategorijas", conn);
            NpgsqlCommand upCmd = new NpgsqlCommand("UPDATE preces SET Nosaukums = ?, Cena = ?, Kategorija_id = ? WHERE p_ID = ?", conn);
            upCmd.Parameters.Add(new NpgsqlParameter("@nosaukums", DbType.String, 255, "nosaukums"));
            upCmd.Parameters.Add(new NpgsqlParameter("@apraksts", DbType.String, 255, "apraksts"));
            upCmd.Parameters.Add(new NpgsqlParameter("@kategorijas_id", DbType.Int32, sizeof(int), "kategorijas_id"));
            upCmd.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32, sizeof(int), "id"));
            NpgsqlCommand addCmd = new NpgsqlCommand("INSERT INTO kategorijas (nosaukums, apraksts, kategorijas_id) VALUES(:nosaukums, :apraksts, :kategorijas_id)", conn);
            addCmd.Parameters.Add(new NpgsqlParameter("@nosaukums", DbType.String, 255, "nosaukums"));
            addCmd.Parameters.Add(new NpgsqlParameter("@apraksts", DbType.String, 255, "apraksts"));
            addCmd.Parameters.Add(new NpgsqlParameter("@kategorijas_id", DbType.Int32, sizeof(int), "kategorijas_id"));
            NpgsqlCommand delCmd = new NpgsqlCommand("DELETE FROM kategorijas WHERE id=:id", conn);
            delCmd.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32, sizeof(int), "id"));
            katAdapt1.SelectCommand = sCmd;
            katAdapt1.InsertCommand = addCmd;
            katAdapt1.DeleteCommand = delCmd;
            katAdapt1.UpdateCommand = upCmd;
            DataTable kategorijas1 = new DataTable();
            katAdapt1.Fill(kategorijas1);
            int i = 0;
            int t = -1;
            foreach (DataRow dr in kategorijas1.Rows)
            {
                if (dr[0].ToString() == e.Node.Name)
                {
                    t = i;
                }
                i++;
            }
            if (t != -1)
            {
                kategorijas1.Rows[t]["nosaukums"] = e.Node.Text.ToString();
                kategorijas1.Rows[t]["apraksts"] = textBox1.Text.ToString();
            }
            else
            {
                DataRow dr1 = kategorijas1.NewRow();
                dr1["id"] = 0;
                dr1["nosaukums"] = e.Node.Text.ToString();
                dr1["apraksts"] = textBox1.Text.ToString();
                dr1["kategorijas_id"] = Convert.ToInt32(e.Node.Parent.Name);
                kategorijas1.Rows.Add(dr1);
            }
            katAdapt1.Update(kategorijas1);
            textBox1.Text = "";
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag == null) return;

            int categoryId = Convert.ToInt32(e.Node.Tag);
            string connString = "Host=localhost;Username=postgres;Password=students;Database=prak10";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                precAdapt = new NpgsqlDataAdapter("SELECT * FROM preces WHERE kategorija_id = @catid", conn);
                precAdapt.SelectCommand.Parameters.AddWithValue("@catid", categoryId);
                preces1.Clear();
                precAdapt.Fill(preces1);
                dataGridView1.DataSource = preces1;
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //try
        //    //{
        //    //    if (precAdapt == null || preces1 == null)
        //    //        return;

        //    //    precAdapt.UpdateCommand = new OleDbCommand(
        //    //        "UPDATE preces SET Nosaukums = ?, Cena = ?, Kategorija_id = ? WHERE p_ID = ?", conn);
        //    //    precAdapt.UpdateCommand.Parameters.Add("@Nosaukums", OleDbType.VarWChar, 255, "Nosaukums");
        //    //    precAdapt.UpdateCommand.Parameters.Add("@Cena", OleDbType.Double, 0, "Cena");
        //    //    precAdapt.UpdateCommand.Parameters.Add("@Kategorija_id", OleDbType.Integer, 0, "Kategorija_id");
        //    //    OleDbParameter pUpdateID = precAdapt.UpdateCommand.Parameters.Add("@p_ID", OleDbType.Integer, 0, "p_ID");
        //    //    pUpdateID.SourceVersion = DataRowVersion.Original;

        //    //    precAdapt.InsertCommand = new OleDbCommand(
        //    //        "INSERT INTO preces (Nosaukums, Cena, Kategorija_id) VALUES (?, ?, ?)", conn);
        //    //    precAdapt.InsertCommand.Parameters.Add("@Nosaukums", OleDbType.VarWChar, 255, "Nosaukums");
        //    //    precAdapt.InsertCommand.Parameters.Add("@Cena", OleDbType.Double, 0, "Cena");
        //    //    precAdapt.InsertCommand.Parameters.Add("@Kategorija_id", OleDbType.Integer, 0, "Kategorija_id");

        //    //    precAdapt.DeleteCommand = new OleDbCommand("DELETE FROM preces WHERE p_ID = ?", conn);
        //    //    OleDbParameter pDeleteID = precAdapt.DeleteCommand.Parameters.Add("@p_ID", OleDbType.Integer, 0, "p_ID");
        //    //    pDeleteID.SourceVersion = DataRowVersion.Original;

        //    //    precAdapt.Update(preces1);
        //    //    MessageBox.Show("Changes saved successfully.");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Error saving changes: " + ex.Message);
        //    //}
        //}
    }
}
