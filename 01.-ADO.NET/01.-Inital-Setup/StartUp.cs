using Microsoft.Data.SqlClient;
using System;

namespace _01._Inital_Setup
{
    public class StartUp
    {
        private const string firstConnectionString = @"Data Source=DESKTOP-MQ67QHE;Database=master;Integrated Security=True";
        private const string secondConnectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnectionForCreatingDb = new SqlConnection(firstConnectionString);

            SqlCommand sqlCommnadForCreatingDB = CreateDatabase(sqlConnectionForCreatingDb);
            
            using SqlConnection sqlConnectionForCreatingTables = new SqlConnection(secondConnectionString);
            for (int i = 1; i <= 6; i++)
            {
                SqlCommand sqlCommand = CreateTable(sqlConnectionForCreatingTables);
            }

            using SqlConnection sqlConnectionForInsertingIntoTables = new SqlConnection(secondConnectionString);
            for (int i = 1; i <= 6; i++)
            {
                SqlCommand sqlCommandInsertingDataToCountries = InsertIntoCountries(sqlConnectionForInsertingIntoTables);
            }
            Console.WriteLine("Finished.");
        }

        private static SqlCommand InsertIntoCountries(SqlConnection sqlConnectionForInsertingIntoTables)
        {
            string insertDataToCountriesQueryText = Console.ReadLine();
            SqlCommand sqlCommand = new SqlCommand(insertDataToCountriesQueryText, sqlConnectionForInsertingIntoTables);

            try
            {
                sqlConnectionForInsertingIntoTables.Open();
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine($"Insert data into Table Countries was successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Generated. Details: " + ex.ToString());
            }
            finally
            {
                if (sqlConnectionForInsertingIntoTables.State == System.Data.ConnectionState.Open)
                {
                    sqlConnectionForInsertingIntoTables.Close();
                }
            }

            return sqlCommand;
        }
        private static SqlCommand CreateTable(SqlConnection secondConnectionString)
        {
            string createDatabaseQueryText = Console.ReadLine();
            SqlCommand sqlCommand = new SqlCommand(createDatabaseQueryText, secondConnectionString);
            try
            {
                secondConnectionString.Open();
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine($"Table was created successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Generated. Details: " + ex.ToString());
            }
            finally
            {
                if (secondConnectionString.State == System.Data.ConnectionState.Open)
                {
                    secondConnectionString.Close();
                }
            }
            return sqlCommand;
        }
        private static SqlCommand CreateDatabase(SqlConnection sqlConnectionForCreatingDb)
        {
            string createDatabaseQueryText = Console.ReadLine();
            SqlCommand sqlCommand = new SqlCommand(createDatabaseQueryText, sqlConnectionForCreatingDb);

            try
            {
                sqlConnectionForCreatingDb.Open();
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine($"MinionsDB database is created successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Generated. Details: " + ex.ToString());
            }
            finally
            {
                if (sqlConnectionForCreatingDb.State == System.Data.ConnectionState.Open)
                {
                    sqlConnectionForCreatingDb.Close();
                }
            }

            return sqlCommand;
        }
    }
}
