using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;

namespace _3prak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // bool
            bool stop = false;

            // Parametri
            string model, manufact;
            int cpu;
            int storage;
            bool monitor;
            int id;
            

            Console.WriteLine("Sveicināti! Izvēlieties savu darbību");

            while (!stop)
            {
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\3prak\\Database3.accdb";
                OleDbConnection con = new OleDbConnection(connectionString);

                Console.WriteLine("Datubāžu informāciju apskate - [1] ");
                Console.WriteLine("Datubāžu ierakstu modificēšana - [2]");
                Console.WriteLine("Datubāžu ierakstu papildināšana - [3] ");
                Console.WriteLine("Datubāžu ierakstu dzēšana - [4] ");
                Console.WriteLine("Iziet - [5] \n");

                Console.Write("Tava izvēle: ");
                int option = Convert.ToInt32(Console.ReadLine());

                

                switch (option)
                {
                    case 1:
                        con.Open();
                        GetDB(con);
                        //Console.Clear();
                        //con.Open();
                        //using (con)
                        //{
                        //    OleDbCommand command = new OleDbCommand("Select * from Dators", con);
                        //    //con.Open();
                        //    OleDbDataReader dataReader = command.ExecuteReader();
                        //    Console.WriteLine("ID     Modelis    Ražotājs   CPU  CietaisDisks Monitors");
                        //    while (dataReader.Read())
                        //    {
                        //        Console.WriteLine();
                        //        for (int i = 0; i < dataReader.FieldCount; i++)
                        //        {
                        //            Console.Write(dataReader.GetValue(i) + "      ");
                        //        }
                        //    }
                        //    Console.WriteLine("\n\nSpiediet jebkuru pogu, lai ietu atpakaļ.");
                        //    Console.ReadKey();
                        //    Console.Clear();
                        //}
                        con.Close();
                        Console.WriteLine("\n\nSpiediet jebkuru pogu, lai ietu atpakaļ.");
                        Console.ReadKey();
                        Console.Clear();

                        break;
                    case 2:
                        con.Open();
                        using (con)
                        {
                            OleDbTransaction transaction = null;
                            try
                            {
                                OleDbCommand addCmd = new OleDbCommand("Update Dators set Nosaukums=?, Razotajs=?, Procesors=?, Cietais_disks=?, Monitors=? Where ID=?", con);

                                Console.Clear();
                                GetDB(con);
                                Console.WriteLine();
                                Console.Write("Ievadiet ID, kuru vēlaties modificēt: ");
                                id = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ievadiet modeli: ");
                                model = Console.ReadLine();
                                Console.Write("Ievadiet ražotāju: ");
                                manufact = Console.ReadLine();
                                Console.Write("Ievadiet procesoru kodolu skaitu: ");
                                cpu = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ievadiet cieta diska ietilpību: ");
                                storage = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Vai nāk ar ekrānu? (True/False): ");
                                monitor = Convert.ToBoolean(Console.ReadLine());

                                
                                addCmd.Parameters.Add(new OleDbParameter("@Nosaukums", model));
                                addCmd.Parameters.Add(new OleDbParameter("@Razotajs", manufact));
                                addCmd.Parameters.Add(new OleDbParameter("@Procesors", cpu));
                                addCmd.Parameters.Add(new OleDbParameter("@HDD", storage));
                                addCmd.Parameters.Add(new OleDbParameter("@Monitors", monitor));
                                addCmd.Parameters.Add(new OleDbParameter("@ID", id));

                                // Transakcijas daļa
                                transaction = con.BeginTransaction();
                                addCmd.Transaction = transaction;
                                addCmd.ExecuteNonQuery();
                                transaction.Commit();

                                Console.WriteLine("\nIeraksts veiksmīgi modificēts! Spied jebkuru pogu, lai ietu atpakaļ uz galveno izvēlni.");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            catch (Exception err)
                            {
                                if (transaction != null)
                                    transaction.Rollback();

                                Console.WriteLine("Notikusi kļūda! Kļūdas kods: " + err);
                            }
                        }

                        break;
                    case 3:
                        using (con)
                        {
                            OleDbTransaction transaction = null;
                            try
                            {
                                OleDbCommand addCmd = new OleDbCommand("Insert into Dators (Nosaukums, Razotajs, Procesors, Cietais_disks, Monitors) VALUES(?,?,?,?,?)", con);
                                
                                Console.Clear();
                                Console.Write("Ievadiet modeli: ");
                                model = Console.ReadLine();
                                Console.Write("Ievadiet ražotāju: ");
                                manufact = Console.ReadLine();
                                Console.Write("Ievadiet procesoru kodolu skaitu: ");
                                cpu = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ievadiet cieta diska ietilpību: ");
                                storage = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Vai nāk ar ekrānu? (True/False): ");
                                monitor = Convert.ToBoolean(Console.ReadLine());

                                con.Open();
                                addCmd.Parameters.Add(new OleDbParameter("@Nosaukums", model));
                                addCmd.Parameters.Add(new OleDbParameter("@Razotajs", manufact));
                                addCmd.Parameters.Add(new OleDbParameter("@Procesors", cpu));
                                addCmd.Parameters.Add(new OleDbParameter("@HDD", storage));
                                addCmd.Parameters.Add(new OleDbParameter("@Monitors", monitor));

                                // Transakcijas daļa
                                transaction = con.BeginTransaction();
                                addCmd.Transaction = transaction;
                                addCmd.ExecuteNonQuery();
                                transaction.Commit();

                                Console.WriteLine("\nIeraksts veiksmīgi pievienots! Spied jebkuru pogu, lai ietu atpakaļ uz galveno izvēlni.");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            catch (Exception err)
                            {
                                if (transaction != null)
                                    transaction.Rollback();
                                
                                Console.WriteLine("Notikusi kļūda! Kļūdas kods: " + err);
                            }
                        }
                        break;
                    case 4:
                        Console.Clear();
                        con.Open();
                        using (con)
                        {
                            GetDB(con);

                            OleDbTransaction transaction = null;
                            try
                            {
                               
                                OleDbCommand deleteCmd = new OleDbCommand("DELETE FROM Dators WHERE ID = ?", con);
                                Console.Write("\nIevadiet ID, kuru vēlaties dzēst: ");
                                id = Convert.ToInt32(Console.ReadLine());
                                deleteCmd.Parameters.Add(new OleDbParameter("@ID", id));

                                // Transakcijas daļa
                                transaction = con.BeginTransaction();
                                deleteCmd.Transaction = transaction;
                                deleteCmd.ExecuteNonQuery();
                                transaction.Commit();
                                Console.WriteLine("\nIeraksts veiksmīgi dzēsts! Spied jebkuru pogu, lai ietu atpakaļ uz galveno izvēlni.");
                                Console.ReadKey();
                                Console.Clear();
                                con.Close();
                            }
                            catch (Exception err)
                            {
                                if (transaction != null)
                                    transaction.Rollback();

                                Console.WriteLine("Notikusi kļūda! Kļūdas kods: " + err);
                            }
                        }
                        con.Close();
                        break;
                    case 5:
                        stop = true;
                        Console.WriteLine("\nProgramma beidz darbību. Spied jebkuru pogu, lai aizvērtu logu.");
                        break;
                    default:
                        Console.WriteLine("\nTādas izvēles nav!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }

            }
        }
        static void GetDB(OleDbConnection con)
        {
            Console.Clear();
            // Parāda datubāzes ierakstus lai vieglāk izlemt kurus dzēst.
            OleDbCommand command = new OleDbCommand("Select * from Dators", con);
            //con.Open();
            OleDbDataReader dataReader = command.ExecuteReader();
            Console.WriteLine("ID     Modelis    Ražotājs   CPU  CietaisDisks Monitors");
            while (dataReader.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Console.Write(dataReader.GetValue(i) + "      ");
                }
            }
            Console.WriteLine();
        }
    }
}
