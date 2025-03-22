using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace _5prak
{
    internal class Program
    {
        static DataTable dt = new DataTable();
        static void View(DataTable dt, string connectionString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM masina";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
                da.Fill(dt);


                Console.WriteLine("ID\tNosaukums\tSvars\t\tSēdvietas\t\tMarka\t\tModelis\n");
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"{row["ID"]}\t{row["nosaukums"]}\t\t{row["svars"]}\t\t{row["sedvietas"]}\t\t{row["marka"]}\t\t\t{row["modelis"]}");
                }
            }
        }

        static void Init(DataTable dt)
        {
            DataColumn id = new DataColumn("id", typeof(int));
            id.AutoIncrement = true;

            DataColumn marka = new DataColumn("nosaukums", typeof(string));
            DataColumn modelis = new DataColumn("svars", typeof(int));
            DataColumn gads = new DataColumn("sedvietas", typeof(int));
            DataColumn tilpums = new DataColumn("marka", typeof(string));
            DataColumn cena = new DataColumn("modelis", typeof(int));
            dt.Columns.Add(id);
            dt.Columns.Add(marka);
            dt.Columns.Add(modelis);
            dt.Columns.Add(gads);
            dt.Columns.Add(tilpums);
            dt.Columns.Add(cena);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
        }
         
        static void Load(DataTable dt, string connectionString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM masina", connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                dt.Clear();

                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = reader["id"];
                    dr["nosaukums"] = reader["nosaukums"];
                    dr["svars"] = reader["svars"];
                    dr["sedvietas"] = reader["sedvietas"];
                    dr["marka"] = reader["marka"];
                    dr["modelis"] = reader["modelis"];
                    dt.Rows.Add(dr);

                }

                dt.AcceptChanges();             
                
                //.Update(dt);
            }
        }
        static void Add (DataTable dt, string connectionString)
        {
            DataRow dr = dt.NewRow();

            //Console.Write("Ievadiet ID: ");
            //dr["id"] = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ievadiet Nosaukumu: ");
            dr["nosaukums"] = Console.ReadLine();

            Console.Write("Ievadiet Svaru: ");
            dr["svars"] = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ievadiet Sēdvietu skaitu: ");
            dr["sedvietas"] = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ievadiet Marku: ");
            dr["marka"] = Console.ReadLine();

            Console.Write("Ievadiet Modeli: ");
            dr["modelis"] = Convert.ToInt32(Console.ReadLine());

            dt.Rows.Add(dr);
        }
        static void Sync(DataTable dt, string connectionString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM masina";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, connection);
                NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(da);

                da.InsertCommand = cb.GetInsertCommand();
                da.UpdateCommand = cb.GetUpdateCommand();
                da.DeleteCommand = cb.GetDeleteCommand();

                da.Update(dt);
                dt.AcceptChanges();  // Mark all changes as applied
                Console.WriteLine("Izmaiņas sinhronizētas ar datubāzi!");
            }
        }
        static void UpdateRecord(DataTable dt)
        {
            Console.Write("Ievadiet ID, kuru vēlaties rediģēt: ");
            int id = Convert.ToInt32(Console.ReadLine());

            DataRow dr = dt.Rows.Find(id);
            if (dr != null)
            {
                Console.Write("Jaunais nosaukums: ");
                dr["nosaukums"] = Console.ReadLine();

                Console.Write("Jaunais svars: ");
                dr["svars"] = Convert.ToInt32(Console.ReadLine());

                Console.Write("Jaunais sēdvietu skaits: ");
                dr["sedvietas"] = Convert.ToInt32(Console.ReadLine());

                Console.Write("Jaunā marka: ");
                dr["marka"] = Console.ReadLine();

                Console.Write("Jaunais modelis: ");
                dr["modelis"] = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Ieraksts atjaunināts lokāli! Sinhronizējiet ar datubāzi, izmantojot opciju [5].");
            }
            else
            {
                Console.WriteLine("Ieraksts ar šo ID netika atrasts.");
            }
        }
        static void Delete(DataTable dt)
        {
            Console.Write("Ievadiet ID, kuru vēlaties dzēst: ");
            int id = Convert.ToInt32(Console.ReadLine());

            DataRow dr = dt.Rows.Find(id);
            if (dr != null)
            {
                dr.Delete();
                Console.WriteLine("Ieraksts dzēsts lokāli! Sinhronizējiet ar datubāzi, izmantojot opciju [5].");
            }
            else
            {
                Console.WriteLine("Ieraksts ar šo ID netika atrasts.");
            }
        }

        static void Main(string[] args)
        {
            string connectionString = "Host=localhost; Port=5432; Username=postgres; Password=students; Database=Masina";
            Init(dt);
            Load(dt, connectionString);

            bool stop = false;

            Console.WriteLine("Sveicināti! Norādiet vēlamo darbību");

            while (!stop)
            {
                Console.WriteLine("Apskatīt saturu - [1]");
                Console.WriteLine("Pievienot jaunu ierakstu - [2]");
                Console.WriteLine("Dzēst ierakstu - [3]");
                Console.WriteLine("Rediģēt ierakstu - [4]");
                Console.WriteLine("Sinhronizēt izmaiņas uz datubāzi - [5]");
                Console.WriteLine("Iziet - [6]");

                Console.Write("\nIzvēle: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        View(dt, connectionString);
                        Console.WriteLine("Nospiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        View(dt, connectionString);
                        Add(dt, connectionString);
                        Console.WriteLine("Nospiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        Console.Clear();
                        View(dt, connectionString);
                        Delete(dt);
                        Console.WriteLine("Nospiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();

                        break;
                    case 4:
                        Console.Clear();
                        View(dt, connectionString);
                        UpdateRecord(dt);
                        Console.WriteLine("Nospiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 5:
                        Console.Clear();
                        Sync(dt, connectionString);
                        Console.WriteLine("Datubāze sinhronizēta. Nospiediet jebkuru pogu, lai ietu atpakaļ");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 6:
                        stop = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
