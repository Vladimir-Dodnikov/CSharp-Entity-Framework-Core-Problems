using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Text;

namespace _08._Increase_Minion_Age
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            int[] inputMinionIds = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();

            string result = IncreaseMinionsAge(sqlConnection, inputMinionIds);

            Console.WriteLine(result);
        }

        private static string IncreaseMinionsAge(SqlConnection sqlConnection, int[] inputMinionIds)
        {
            sqlConnection.Open();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < inputMinionIds.Length; i++)
            {
                string updateMinionsQueryText = $"UPDATE Minions SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1 WHERE Id = @MinionId";
                SqlCommand updateMinionsCommand = new SqlCommand(updateMinionsQueryText, sqlConnection);
                updateMinionsCommand.Parameters.AddWithValue("@MinionId", inputMinionIds[i]);
            }

            string getNewMinionsQueryText = $"SELECT Name, Age FROM Minions";
            using SqlCommand getNewMinionsCommand = new SqlCommand(getNewMinionsQueryText, sqlConnection);
            
            using SqlDataReader rdr = getNewMinionsCommand.ExecuteReader();
            while (rdr.Read())
            {
                sb.AppendLine($" {rdr["Name"]} {rdr["Age"]}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
