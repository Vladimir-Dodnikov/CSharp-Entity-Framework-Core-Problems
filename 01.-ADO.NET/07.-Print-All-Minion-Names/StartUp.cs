using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _07._Print_All_Minion_Names
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            
            string result = ReOrderMinions(sqlConnection);

            Console.WriteLine(result);
        }

        private static string ReOrderMinions(SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();
            List<string> initialOrderOfMinions = new List<string>();
            List<string> reOrderedMinions = new List<string>();

            sqlConnection.Open();

            string getMinionsQueryText = "SELECT Name FROM Minions";
            using SqlCommand getAllNamesCommand = new SqlCommand(getMinionsQueryText, sqlConnection);
            using SqlDataReader rdr = getAllNamesCommand.ExecuteReader();

            if (!rdr.HasRows)
            {
                return sb.ToString().TrimEnd();
            }

            while (rdr.Read())
            {
                initialOrderOfMinions.Add((string)rdr["Name"]);
            }

            while (initialOrderOfMinions.Count > 0)
            {

                reOrderedMinions.Add(initialOrderOfMinions[0]);

                initialOrderOfMinions.RemoveAt(0);

                if (initialOrderOfMinions.Count > 0)
                {
                    reOrderedMinions.Add(initialOrderOfMinions[initialOrderOfMinions.Count - 1]);

                    initialOrderOfMinions.RemoveAt(initialOrderOfMinions.Count - 1);
                }
            }

            foreach (var minion in reOrderedMinions)
            {
               sb.AppendLine(minion);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
