using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;


namespace _2.prak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Stop bools
            bool programstop = false;

            // Interfeiss
            Console.WriteLine("Sveicināti! Izvēlēties savu vēlamo darbību.\n ");

            while (!programstop)
            {
                Console.WriteLine("Datubāžu informāciju apskate - [1] ");
                Console.WriteLine("Datubāžu ierakstu papildināšana - [2] ");
                Console.WriteLine("Datubāžu ierakstu dzēšana - [3] ");
                Console.WriteLine("Iziet - [4] \n");
                Console.Write("Tava izvēle: ");
                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    // Ierakstu apskate
                    case 1:
                        Console.Clear();
                        string accessConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\2.prak\\Database2.accdb";
                        OleDbConnection con = new OleDbConnection(accessConnString);
                        OleDbCommand command = new OleDbCommand("Select * from workers",con);
                        con.Open();
                        OleDbDataReader dataReader = command.ExecuteReader();
                        Console.WriteLine("\tDARBINIEKU SARAKSTS\n");
                        Console.WriteLine("ID\t Vārds\t Uzvārds\t Amats");
                        while (dataReader.Read())
                        {
                            Console.WriteLine();
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                Console.Write(dataReader.GetValue(i) + "\t");
                            }
                        }
                        Console.WriteLine("\n\nSpiediet jebkuru pogu, lai ietu atpakaļ.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    // Ierakstu papildināšana
                    case 2:
                        Console.Clear();
                        accessConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\2.prak\\Database2.accdb";
                        OleDbConnection editcon = new OleDbConnection(accessConnString);
                        OleDbCommand editcommand = new OleDbCommand("INSERT INTO Workers (Vards, Uzvards, Amats) VALUES(?,?,?)", editcon);
                        Console.WriteLine("Norādiet personu skaitu");
                        Console.Write("Personu skaits : ");
                        editcon.Open();
                        int skaits = Convert.ToInt32(Console.ReadLine());
                        for (int i = 0; i < skaits; i++)
                        {
                            Console.Write("\nIevadiet vārdu : ");
                            editcommand.Parameters.Add(new OleDbParameter("@Vards", Console.ReadLine()));
                            Console.Write("\nIevadiet uzvārdu : ");
                            editcommand.Parameters.Add(new OleDbParameter("@Uzvards", Console.ReadLine()));
                            Console.Write("\nNorādiet amatu : ");
                            editcommand.Parameters.Add(new OleDbParameter("@Amats", Console.ReadLine()));
                            editcommand.ExecuteNonQuery();
                            editcommand.Parameters.Clear();
                        }
                        editcon.Close();
                        Console.WriteLine($"{skaits} personas pievienotas! Spied jebkuru pogu, lai ietu atpakaļ uz izvēlni");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    // Ierakstu dzēšana
                    case 3:
                        Console.Clear();
                        accessConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\2.prak\\Database2.accdb";
                        OleDbConnection delcon = new OleDbConnection(accessConnString);
                        OleDbCommand showtable = new OleDbCommand("Select * from Workers", delcon);
                        OleDbCommand del = new OleDbCommand("DELETE FROM Workers WHERE ID=?", delcon);
                        delcon.Open();
                        Console.WriteLine("\tDARBINIEKU SARAKSTS\n");
                        Console.WriteLine("ID\t Vārds\t Uzvārds\t Amats");
                        dataReader = showtable.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Console.WriteLine();
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                Console.Write(dataReader.GetValue(i) + "\t");
                            }
                        }
                        Console.Write("\nNorādiet persona ID, kuru vēlaties izdzēst.\n");
                        Console.Write("\nIzvēlētais ID : ");
                        del.Parameters.Add(new OleDbParameter("@ID", Console.ReadLine()));
                        del.ExecuteNonQuery();
                        del.Parameters.Clear();
                        delcon.Close();
                        Console.WriteLine($"Izvēlētā persona ir izdzēsta no datubāzes. Spied jebkuru pogu, lai atgrieztos atpakaļ izvēlnē.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    // Aptur darbību
                    case 4:
                        Console.WriteLine("Programma beidz darbību. Spied jebkuru pogu, lai izietu.");
                        programstop = true;
                        break;
                    default:
                        Console.WriteLine("Tādu izvēlņu nav!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
