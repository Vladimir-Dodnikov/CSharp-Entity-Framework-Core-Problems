using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _03._Minion_Names
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            int villainId = int.Parse(Console.ReadLine());

            string result = GetAllMinionByViilainId(sqlConnection, villainId);

            Console.WriteLine(result);
        }

        private static string GetAllMinionByViilainId(SqlConnection sqlConnection, int villainId)
        {
            try
            {
                sqlConnection.Open();

                StringBuilder sb = new StringBuilder();

                string getVillainNameQueryText = "SELECT Name FROM Villains WHERE Id = @Id";

                using SqlCommand getVillainNameSqlCommand = new SqlCommand(getVillainNameQueryText, sqlConnection);

                getVillainNameSqlCommand.Parameters.AddWithValue("@Id", villainId);

                string villainName = getVillainNameSqlCommand.ExecuteScalar()?.ToString();

                if (villainName == null)
                {
                    sb.AppendLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    sb.AppendLine($"Villain: {villainName}");

                    string getAllMinionsQueryText = "SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum, m.[Name], m.Age " +
                                                        "FROM MinionsVillains AS mv " +
                                                        "JOIN Minions As m ON mv.MinionId = m.Id " +
                                                        "WHERE mv.VillainId = @Id " +
                                                        "ORDER BY m.Name";

                    SqlCommand getMinionsCommand = new SqlCommand(getAllMinionsQueryText, sqlConnection);
                    getMinionsCommand.Parameters.AddWithValue("@Id", villainId);

                    SqlDataReader rdr = getMinionsCommand.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        int rowCounter = 1;
                        while (rdr.Read())
                        {
                            string miinionName = rdr["Name"]?.ToString();
                            string minionAge = rdr["Age"]?.ToString();

                            sb.AppendLine($"{rowCounter}. {miinionName} - {minionAge}");
                            rowCounter++;
                        }
                    }
                    else
                    {
                        sb.AppendLine("(no minions)");
                    }
                }
                
                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return "Error Generated. Details: " + ex.ToString();
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
