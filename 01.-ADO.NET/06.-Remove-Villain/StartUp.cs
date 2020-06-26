using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _06._Remove_Villain
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            int villainId = int.Parse(Console.ReadLine());

            string result = RemoveVillainById(sqlConnection, villainId);

            Console.WriteLine(result);
        }

        private static string RemoveVillainById(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            string getVillainQueryText = "SELECT Name FROM Villains WHERE Id = @villainId";
            SqlCommand getVillainNameCommand = new SqlCommand(getVillainQueryText, sqlConnection);
            getVillainNameCommand.Parameters.AddWithValue("@villainId", villainId);

            getVillainNameCommand.Transaction = sqlTransaction;

            string villainName = getVillainNameCommand.ExecuteScalar()?.ToString();

            if (villainName == null)
            {
                sb.AppendLine("No such villain was found.");
            }
            else
            {
                try
                {
                    string removeMinionsQueryText = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                    SqlCommand removeMinionsCommand = new SqlCommand(removeMinionsQueryText, sqlConnection);
                    removeMinionsCommand.Parameters.AddWithValue("@villainId", villainId);

                    removeMinionsCommand.Transaction = sqlTransaction;

                    string minionsCount = removeMinionsCommand.ExecuteScalar()?.ToString();

                    string removeVillainQueryText = "DELETE FROM Villains WHERE Id = @villainId";
                    SqlCommand removeVillainCommand = new SqlCommand(removeVillainQueryText, sqlConnection);
                    removeVillainCommand.Parameters.AddWithValue("@villainId", villainId);

                    removeVillainCommand.Transaction = sqlTransaction;

                    removeVillainCommand.ExecuteNonQuery();

                    sqlTransaction.Commit();

                    sb
                        .AppendLine($"{villainName} was deleted.")
                        .AppendLine($"{minionsCount} minions were released.");


                }
                catch (Exception ex)
                {
                    sb.AppendLine(ex.Message);
                    try
                    {
                        sqlTransaction.Rollback();
                    }
                    catch (Exception rollback)
                    {
                        sb.AppendLine(rollback.Message);
                    }
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

            return sb.ToString().TrimEnd();
        }
    }
}
