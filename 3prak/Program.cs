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

            Console.WriteLine("Sveicināti! Izvēlieties savu darbību");

            while (!stop)
            {
                Console.WriteLine("Datubāžu informāciju apskate - [1] ");
                Console.WriteLine("Datubāžu ierakstu modificēšana - [2]");
                Console.WriteLine("Datubāžu ierakstu papildināšana - [3] ");
                Console.WriteLine("Datubāžu ierakstu dzēšana - [4] ");
                Console.WriteLine("Iziet - [5] \n");

                Console.Write("Tava izvēle: ");
                int option = Convert.ToInt32(Console.ReadLine());

                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\3.prak\\Database3.accdb";
                OleDbConnection con = new OleDbConnection(connectionString);


                switch (option)
                {
                    case 1:
                        Console.Clear();
                        OleDbCommand command = new OleDbCommand("Select * from Dators", con);
                        con.Open();
                        OleDbDataReader dataReader = command.ExecuteReader();
                        Console.WriteLine("ID    Modelis   Ražotājs   CPU  CietaisDisks Monitors");
                        while (dataReader.Read())
                        {
                            Console.WriteLine();
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                Console.Write(dataReader.GetValue(i) + "      ");
                            }
                        }
                        Console.WriteLine("\n\nSpiediet jebkuru pogu, lai ietu atpakaļ.");
                        Console.ReadKey();
                        Console.Clear();
                        con.Close();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
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
    }
}