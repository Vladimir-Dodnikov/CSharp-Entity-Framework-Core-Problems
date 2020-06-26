using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace _02._Villain_Names
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlDataReader rdr = null;

            string result = GetAllVillainNames(sqlConnection, rdr);

            Console.WriteLine(result);
        }

        private static string GetAllVillainNames(SqlConnection sqlConnection, SqlDataReader rdr)
        {
            try
            {
                sqlConnection.Open();

                StringBuilder sb = new StringBuilder();

                string getAllVillainNamesQueryText = "SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  " +
                                                     "FROM Villains AS v " +
                                                     "JOIN MinionsVillains AS mv ON v.Id = mv.VillainId " +
                                                     "GROUP BY v.Id, v.Name " +
                                                     "HAVING COUNT(mv.VillainId) > 3 " +
                                                     "ORDER BY COUNT(mv.VillainId)";

                using SqlCommand getAllNamesSqlCommand = new SqlCommand(getAllVillainNamesQueryText, sqlConnection);

                rdr = getAllNamesSqlCommand.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string name = rdr["Name"]?.ToString();
                        string minionsCount = rdr["MInionsCount"]?.ToString();
                        
                        sb.AppendLine($"{name} - {minionsCount}");
                    }
                }
                else
                {
                    sb.AppendLine("(no data)");
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
