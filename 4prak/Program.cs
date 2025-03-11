using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace _4prak
{
    internal class Program
    {
        static DataTable dt = new DataTable();
        static void DisplayDataTable(DataTable dt)
        {
            Console.WriteLine("\nID\tDistroName\tDifficulty\t\tPackage_Manager\tInit_system\t\tUpdate_type");
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"{row["ID"]}\t{row["Disname"]}\t\t{row["Difficulty"]}\t\t\t{row["Packman"]}\t\t{row["Init"]}\t\t\t{row["Update_type"]}");
                }
            }
        }
        static void InitDataTable(DataTable dt)
        {
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Disname", typeof(string)));
            dt.Columns.Add(new DataColumn("Difficulty", typeof(string)));
            dt.Columns.Add(new DataColumn("Packman", typeof(string)));
            dt.Columns.Add(new DataColumn("Init", typeof(string)));
            dt.Columns.Add(new DataColumn("Update_type", typeof(string)));
            dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
            dt.Clear();
        }

        static void LoadData(string connectionString)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM Linux", conn);
                OleDbDataReader reader = cmd.ExecuteReader();
                dt.Clear();

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = reader["ID"];
                    row["Disname"] = reader["Disname"];
                    row["Difficulty"] = reader["Difficulty"];
                    row["Packman"] = reader["Packman"];
                    row["Init"] = reader["Init"];
                    row["Update_type"] = reader["Update_type"];
                    dt.Rows.Add(row);

                }
                dt.AcceptChanges();
            }
        }

        static void AddData()
        {
            dt.Clear();
            Console.Write("Cik ierakstus pievienot? ");
            int count = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                DataRow row = dt.NewRow();

                row["ID"] = dt.Rows.Count == 0 ? 1 : dt.Rows.Cast<DataRow>().Max(r => r.Field<int>("ID")) + 1;

                Console.Write("Norādiet distributīva nosaukumu: ");
                row["Disname"] = Console.ReadLine();

                Console.Write("Grūtības pakāpe?: ");
                row["Difficulty"] = Console.ReadLine();

                Console.Write("Kāds ir izmantots pakotņu menedžeris?: ");
                row["Packman"] = Console.ReadLine();

                Console.Write("Norādiet init sistēmu (piem. systemd):  ");
                row["Init"] = Console.ReadLine();

                Console.Write("Kāds ir update veids?: ");
                row["Update_type"] = Console.ReadLine();

                dt.Rows.Add(row);
            }
            Console.WriteLine("Dati pievienoti lokāli! Neaizmirsti sinhronizēt ar [5].");
        }

        static void DeleteData()
        {
            Console.Clear();
            DisplayDataTable(dt);
            Console.WriteLine();

            Console.Write("Ievadi ID, kuru vēlies dzēst: ");
            int id = Convert.ToInt32(Console.ReadLine());

            DataRow[] rows = dt.Select($"ID = {id}");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                Console.WriteLine($"Ieraksts ar ID {id} atzīmēts dzēšanai. Neaizmirsti sinhronizēt ar [5].");
            }
            else
            {
                Console.WriteLine("Ieraksts ar norādīto ID netika atrasts!");
            }
        }

        static void UpdateData()
        {
            Console.Clear();

            DisplayDataTable(dt); Console.WriteLine();

            Console.Write("Ievadi ID, kuru vēlies rediģēt: ");
            int id = Convert.ToInt32(Console.ReadLine());

            DataRow[] rows = dt.Select($"ID = {id}");
            if (rows.Length > 0)
            {
                DataRow row = rows[0];

                Console.Write($"Jaunais distributīvu nosaukums ({row["Disname"]}): ");
                string disname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(disname)) row["Disname"] = disname;

                Console.Write($"Grūtības pakāpe ({row["Difficulty"]}): ");
                string diff = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(diff)) row["Difficulty"] = diff;

                Console.Write($"Kādu pakotņu menedžeri izmanto? ({row["Packman"]}): ");
                string pacman = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(pacman)) row["Packman"] = pacman;

                Console.Write($"Norādiet init sistēmu (piem. systemd) ({row["Init"]}): ");
                string init = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(init)) row["Init"] = init;

                Console.Write($"Atjaunojumu tips ({row["Update_type"]}): ");
                string update = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(update)) row["Update_type"] = update;

                Console.WriteLine($"Ieraksts ar ID {id} atjaunināts! Neaizmirsti sinhronizēt ar [5].");
            }
            else
            {
                Console.WriteLine("Ieraksts ar norādīto ID netika atrasts!");
            }
        }


        
        static void Main(string[] args)
        {
            bool stop = false;
            string accessConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\4prak\\Database5.accdb";
            InitDataTable(dt);
            LoadData(accessConnString);

            Console.WriteLine("Sveicināti! Izvēlieties savu darbību.");
            while (!stop)
            {
                Console.WriteLine("Attēlo datus - [1]");
                Console.WriteLine("Pievienot datus - [2]");
                Console.WriteLine("Dzēst datus - [3]");
                Console.WriteLine("Rediģēt datus - [4]");
                Console.WriteLine("Sinhronizēt izmaiņas - [5]");
                Console.WriteLine("Iziet - [6]");

                Console.Write("\nIzvēlētā darbība: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        DisplayDataTable(dt);
                        Console.WriteLine("Spiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        AddData();
                        Console.WriteLine("Spiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        DeleteData();
                        Console.WriteLine("Spiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        UpdateData();
                        Console.WriteLine("Spiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 5:
                        using (OleDbConnection conn = new OleDbConnection(accessConnString))
                        {
                            conn.Open();

                            // Process deletions first to avoid conflicts
                            DataRow[] deletedRows = dt.Select(null, null, DataViewRowState.Deleted);
                            foreach (DataRow row in deletedRows)
                            {
                                string query = "DELETE FROM Linux WHERE ID=?";
                                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("?", row["ID", DataRowVersion.Original]);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Process inserts
                            foreach (DataRow row in dt.Select(null, null, DataViewRowState.Added))
                            {
                                string query = "INSERT INTO Linux (Disname, Difficulty, Packman, Init, Update_type) VALUES (?, ?, ?, ?, ?)";
                                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("?", row["Disname"]);
                                    cmd.Parameters.AddWithValue("?", row["Difficulty"]);
                                    cmd.Parameters.AddWithValue("?", row["Packman"]);
                                    cmd.Parameters.AddWithValue("?", row["Init"]);
                                    cmd.Parameters.AddWithValue("?", row["Update_type"]);
                                    cmd.ExecuteNonQuery();
                                }

                                // Retrieve the last inserted ID
                                //using (OleDbCommand getIdCmd = new OleDbCommand("SELECT @@IDENTITY", conn))
                                //{
                                //    row["ID"] = Convert.ToInt32(getIdCmd.ExecuteScalar());
                                //}
                            }

                            // Process updates
                            foreach (DataRow row in dt.Select(null, null, DataViewRowState.ModifiedCurrent))
                            {
                                string query = "UPDATE Linux SET Disname=?, Difficulty=?, Packman=?, Init=?, Update_type=? WHERE ID=?";
                                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("?", row["Disname"]);
                                    cmd.Parameters.AddWithValue("?", row["Difficulty"]);
                                    cmd.Parameters.AddWithValue("?", row["Packman"]);
                                    cmd.Parameters.AddWithValue("?", row["Init"]);
                                    cmd.Parameters.AddWithValue("?", row["Update_type"]);
                                    cmd.Parameters.AddWithValue("?", row["ID"]);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            dt.AcceptChanges(); // Confirm all changes
                        }
                        LoadData(accessConnString);
                        Console.WriteLine("Izmaiņas sinhronizētas ar datubāzi!");
                        break;
                    case 6:
                        stop = true;
                        break;
                    default:
                        Console.WriteLine("Tādas izvēlnes nav!");
                        break;
                }
            }

        }
    }
}
