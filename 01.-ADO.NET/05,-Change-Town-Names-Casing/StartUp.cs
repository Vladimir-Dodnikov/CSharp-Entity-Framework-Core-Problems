using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _05__Change_Town_Names_Casing
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            var inputCountry = Console.ReadLine();

            string result = ChangeAllTownsToUppercase(sqlConnection, inputCountry);

            Console.WriteLine(result);
        }
        private static string ChangeAllTownsToUppercase(SqlConnection sqlConnection, string inputCountry)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sqlConnection.Open();

                string getAllTownsQueryText = $"SELECT t.Name FROM Towns as t JOIN Countries AS c ON c.Id = t.CountryCode WHERE c.Name = @Country";

                using SqlCommand getAllTownsCommand = new SqlCommand(getAllTownsQueryText, sqlConnection);
                getAllTownsCommand.Parameters.AddWithValue("@Country", inputCountry);

                if (getAllTownsCommand.ExecuteScalar()?.ToString() == null)
                {
                    sb.AppendLine("No town names were affected.");
                    return sb.ToString().TrimEnd();
                }

                using SqlDataReader rdr = getAllTownsCommand.ExecuteReader();

                List<string> changedTowns = new List<string>();

                int townCounter = 0;
                while (rdr.Read())
                {
                    var townName = rdr["Name"].ToString();
                    var townNameUpper = rdr["Name"].ToString().ToUpper().ToString();

                    if (townNameUpper == townName)
                    {
                        continue;
                    }

                    string changeAllTownsQueryText = $"UPDATE Towns SET Name = '{rdr["Name"].ToString().ToUpper()}";
                    SqlCommand changeAllTownsCommand = new SqlCommand(changeAllTownsQueryText, sqlConnection);

                    changedTowns.Add(townNameUpper);
                    townCounter++;
                }

                if (townCounter == 0)
                {
                    sb.AppendLine("No town names were affected.");
                }
                else
                {
                    sb.AppendLine($"{townCounter} town names were affected.");
                    sb.AppendLine($"[{string.Join(", ", changedTowns)}]");
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