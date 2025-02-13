using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;


namespace _1prak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string excelConnString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\1prak\\Book1.xlsx; Extended Properties='Excel 12.0;HDR=YES;'");

            OleDbConnection con = new OleDbConnection(excelConnString);
            //OleDbCommand cmd = new OleDbCommand("SELECT ID FROM [Sheet1$]", con);
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", con);
            con.Open();
            OleDbDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            Console.WriteLine("ID \t name \t surname \t tel");

            while (dataReader.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Console.Write(dataReader.GetValue(i) + " ");
                }
            }
            con.Close();
            Console.WriteLine();
            string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\prog ar db\\1prak\\Database1.accdb";
            OleDbConnection acccon = new OleDbConnection(ConnectionString);
            OleDbCommand addCmd = new OleDbCommand("INSERT INTO Persona (Vards, Uzvards) VALUES(?,?)",acccon);
            Console.WriteLine();
            Console.Write("Cik Personas pievienot? : ");
            int sk = Convert.ToInt32(Console.ReadLine());
            acccon.Open();
            for (int i = 0; i < sk; i++)
            {
                Console.WriteLine("Ievadiet vārdu : ");
                addCmd.Parameters.Add(new OleDbParameter("@Vards", Console.ReadLine()));
                Console.WriteLine("Ievadiet uzvārdu : ");
                addCmd.Parameters.Add(new OleDbParameter("@Uzvards", Console.ReadLine()));
                addCmd.ExecuteNonQuery();
                addCmd.Parameters.Clear();
            }
            
            Console.WriteLine("Personas pievienotas!");
            acccon.Close();
        }
    }
}
