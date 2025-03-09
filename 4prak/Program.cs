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
            Console.WriteLine("\nID\tRažotājs\tModelis\t\tRAM\tCPU\t\tOS");
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine($"{row["ID"]}\t{row["Razotajs"]}\t\t{row["Modelis"]}\t{row["RAM"]}\t{row["CPU"]}\t{row["OS"]}");
            }
        }
        static void InitDataTable(DataTable dt)
        {
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Razotajs", typeof(string)));
            dt.Columns.Add(new DataColumn("Modelis", typeof(string)));
            dt.Columns.Add(new DataColumn("RAM", typeof(string)));
            dt.Columns.Add(new DataColumn("CPU", typeof(string)));
            dt.Columns.Add(new DataColumn("OS", typeof(string)));
            dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
            dt.Clear();
        }

        static void LoadData(string connectionString)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM Dators", conn);
                OleDbDataReader reader = cmd.ExecuteReader();
                dt.Clear();

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = reader["ID"];
                    row["Razotajs"] = reader["Razotajs"];
                    row["Modelis"] = reader["Modelis"];
                    row["RAM"] = reader["RAM"];
                    row["CPU"] = reader["CPU"];
                    row["OS"] = reader["OS"];
                    dt.Rows.Add(row);

                }
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

                Console.Write("Ražotājs: ");
                row["Razotajs"] = Console.ReadLine();

                Console.Write("Modelis: ");
                row["Modelis"] = Console.ReadLine();

                Console.Write("RAM (GB): ");
                row["RAM"] = Console.ReadLine();

                Console.Write("CPU: ");
                row["CPU"] = Console.ReadLine();

                Console.Write("OS: ");
                row["OS"] = Console.ReadLine();

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

                Console.Write($"Jaunais ražotājs ({row["Razotajs"]}): ");
                string razotajs = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(razotajs)) row["Razotajs"] = razotajs;

                Console.Write($"Jaunais modelis ({row["Modelis"]}): ");
                string modelis = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(modelis)) row["Modelis"] = modelis;

                Console.Write($"Jaunais RAM ({row["RAM"]}): ");
                string ram = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ram)) row["RAM"] = ram;

                Console.Write($"Jaunais CPU ({row["CPU"]}): ");
                string cpu = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(cpu)) row["CPU"] = cpu;

                Console.Write($"Jaunā OS ({row["OS"]}): ");
                string os = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(os)) row["OS"] = os;

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
            string accessConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\4prak\\Database4.accdb";
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
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState == DataRowState.Added)
                                {
                                    string query = "INSERT INTO Dators (Razotajs, Modelis, RAM, CPU, OS) VALUES (?, ?, ?, ?, ?)";
                                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                                    {
                                        cmd.Parameters.AddWithValue("?", row["Razotajs"]);
                                        cmd.Parameters.AddWithValue("?", row["Modelis"]);
                                        cmd.Parameters.AddWithValue("?", row["RAM"]);
                                        cmd.Parameters.AddWithValue("?", row["CPU"]);
                                        cmd.Parameters.AddWithValue("?", row["OS"]);
                                        cmd.ExecuteNonQuery();
                                    }
                                    
                                }
                                else if (row.RowState == DataRowState.Modified)
                                {
                                    string query = "UPDATE Dators SET Razotajs=?, Modelis=?, RAM=?, CPU=?, OS=? WHERE ID=?";
                                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                                    {
                                        cmd.Parameters.AddWithValue("?", row["Razotajs"]);
                                        cmd.Parameters.AddWithValue("?", row["Modelis"]);
                                        cmd.Parameters.AddWithValue("?", row["RAM"]);
                                        cmd.Parameters.AddWithValue("?", row["CPU"]);
                                        cmd.Parameters.AddWithValue("?", row["OS"]);
                                        cmd.Parameters.AddWithValue("?", row["ID"]);
                                        cmd.ExecuteNonQuery();
                                    }
                                    dt.AcceptChanges();
                                    
                                }
                                else if (row.RowState == DataRowState.Deleted)
                                {
                                    string query = "DELETE FROM Dators WHERE ID=?";
                                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                                    {
                                        cmd.Parameters.AddWithValue("?", row["ID", DataRowVersion.Original]);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            dt.AcceptChanges();
                        }
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
