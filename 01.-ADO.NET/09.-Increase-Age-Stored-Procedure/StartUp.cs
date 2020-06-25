using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text;

namespace _09._Increase_Age_Stored_Procedure
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            int minionId = int.Parse(Console.ReadLine());

            string result = IncreaseAge(sqlConnection, minionId);

            Console.WriteLine(result);
        }

        private static string IncreaseAge(SqlConnection sqlConnection, int minionId)
        {
            StringBuilder sb = new StringBuilder();

            string procedureName = "usp_GetOlder";
            using SqlCommand increaseAgeCommand = new SqlCommand(procedureName, sqlConnection);
            increaseAgeCommand.CommandType = CommandType.StoredProcedure;

            increaseAgeCommand.Parameters.AddWithValue("@id", minionId);
            increaseAgeCommand.ExecuteNonQuery();

            string getMinionInfoQueryText = "SELECT Name, Age FROM Minions WHERE Id = @Id";
            using SqlCommand getMinionCommand = new SqlCommand(getMinionInfoQueryText, sqlConnection);
            getMinionCommand.Parameters.AddWithValue("@Id", minionId);


            using SqlDataReader rdr = getMinionCommand.ExecuteReader();

            rdr.Read();

            string minionName = rdr["Name"]?.ToString();
            string minionAge = rdr["Age"]?.ToString();

            sb.AppendLine($"{minionName} - {minionAge} years old");

            return sb.ToString().TrimEnd();
        }
    }
}
